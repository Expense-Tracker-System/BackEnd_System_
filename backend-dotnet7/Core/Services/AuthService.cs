using backend_dotnet7.Controllers;
using backend_dotnet7.Core.Constants;
using backend_dotnet7.Core.Dtos.Auth;
using backend_dotnet7.Core.Dtos.General;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using backend_dotnet7.Core.Template;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;

namespace backend_dotnet7.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogService _logService;
        private readonly IConfiguration _configuration;
        private readonly IUserPasswordConfirmService _userPasswordConfirmService;
        private readonly IUserEmailService _userEmailService;
        private readonly IUserPhoneNumberService _userPhoneNumberService;
        private readonly IGenerateResponseService _generateResponseService;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<AuthService> _logger;
        private readonly IEmailService _emailService;

        public AuthService(UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            ILogService logService, 
            IConfiguration configuration, 
            IUserPasswordConfirmService userPasswordConfirmService,
            IUserEmailService userEmailService,
            IUserPhoneNumberService userPhoneNumberService,
            IGenerateResponseService generateResponseService,
            IWebHostEnvironment environment,
            ILogger<AuthService> logger,
            IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logService = logService;
            _configuration = configuration;
            _userPasswordConfirmService = userPasswordConfirmService;
            _userEmailService = userEmailService;
            _userPhoneNumberService = userPhoneNumberService;
            _generateResponseService = generateResponseService;
            _environment = environment;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task<GeneralServiceResponseDto> SeedRolesAsync()
        {
            bool isAdminRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);
            bool isUserRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.USER);

            if (isAdminRoleExists && isUserRoleExists)
                return new GeneralServiceResponseDto()
                {
                    IsSucceed = true,
                    StatusCode = 200,
                    Message = "Roles Seeding is Already Done"
                };

            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));

            return new GeneralServiceResponseDto()
            {
                IsSucceed = true,
                StatusCode = 201,
                Message = "Roles Seeding Done Successfully"
            };
        }

        public async Task<GeneralServiceResponseDto> RegisterAsync(RegisterDto registerDto, string callback_url)
        {
            var isExistsUser = await _userManager.FindByNameAsync(registerDto.Username);

            if (isExistsUser is not null)
                return new GeneralServiceResponseDto()
                {
                    IsSucceed = false,
                    StatusCode = 409,
                    Message = "UserName Alredy Exists"
                };

            var confirmPasswordDto = new ConfirmPasswordDto
            {
                password = registerDto.Password,
                confirmPassword = registerDto.ConfirmPassword
            };

            var emailValidationResult = await _userEmailService.EmailValidation(registerDto.Email);

            if (emailValidationResult is false)
                return new GeneralServiceResponseDto()
                {
                    IsSucceed = false,
                    StatusCode = 400,
                    Message = "User Email is invalid"
                };

            /*
            var isUnique = await _userEmailService.IsEmailUnique(registerDto.Email);

            if (isUnique is false)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSucceed = false,
                    StatusCode = 409,
                    Message = "Already has account using this Email"
                };
            }
            */

            var validPhoneNumber = await _userPhoneNumberService.PhoneNumberValidation(registerDto.PhoneNumber);

            if (validPhoneNumber is false)
                return new GeneralServiceResponseDto()
                {
                    IsSucceed = false,
                    StatusCode = 400,
                    Message = "User Phone Number is invalid"
                };

            var isPhoneNumberUnique = await _userPhoneNumberService.IsPhoneNumberUnique(registerDto.PhoneNumber);

            if (isPhoneNumberUnique is false)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSucceed = false,
                    StatusCode = 409,
                    Message = "Already has account using this Phone Number"
                };
            }

            var passwordValidationResult = await _userPasswordConfirmService.userPasswordConfirm(confirmPasswordDto);

            if (passwordValidationResult is false)
                return new GeneralServiceResponseDto()
                {
                    IsSucceed = false,
                    StatusCode = 400,
                    Message = "User Password Confirmation is Failed"
                };

            ApplicationUser newUser = new ApplicationUser()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                EmailConfirmed = false,
                UserName = registerDto.Username,
                PhoneNumber = registerDto.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var createUserResult = await _userManager.CreateAsync(newUser,registerDto.Password);

            if (!createUserResult.Succeeded)
            {
                var errorString = "User Creation failed because : ";
                foreach(var error in createUserResult.Errors)
                {
                    errorString += " # " + error.Description;
                }

                return new GeneralServiceResponseDto()
                {
                    IsSucceed = false,
                    StatusCode = 400,
                    Message = errorString
                };
            }

            //Add a Default User Role to all users
            await _userManager.AddToRoleAsync(newUser, StaticUserRoles.USER);
            await _logService.SaveNewLog(newUser.UserName, "Registered to Website");

            // Generate Email confirmation Token
            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

            // Append values
            var new_callback_url = $"{callback_url}?userId={newUser.Id}&code={Uri.EscapeDataString(emailConfirmationToken)}";

            // Send email
            var registrationConfirmEmailTemplate = new RegistrationConfirmEmailTemplate();
            var htmlText = registrationConfirmEmailTemplate.RegistrationConfirmEmail(new_callback_url,newUser.Email);
            var emailSendResponse = await _emailService.SendEmail(htmlText, registerDto.Email, "Email Confirmation");

            // check response
            if(emailSendResponse.IsSuccess is false)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSucceed = true,
                    StatusCode = 201,
                    Message = "Register Was Successfull " + emailSendResponse.Message + " Error Occured in Send Mail, Please Contact Admin",
                };
            }

            return new GeneralServiceResponseDto()
            {
                IsSucceed = true,
                StatusCode = 201,
                Message = "Register Was Successfull " + emailSendResponse.Message + " Please Confirm Your Email",
            };

        }

        public async Task<LoginServiceResponseDto?> LoginAsync(LoginDto loginDto)
        {
            //Find user with username
            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if (user is null)
            {
                return new LoginServiceResponseDto
                {
                    IsSucceed = false,
                    StatusCode = 404,
                    Message = "User not founded",
                    NewToken = null,
                    userInfo = null,
                };
            }

            // check confirm Email / confirm Account
            if (!user.EmailConfirmed)
            {
                return new LoginServiceResponseDto
                {
                    IsSucceed = false,
                    StatusCode = 403,
                    Message = "Email not confirmed",
                    NewToken = null,
                    userInfo = null,
                };
            }

            // check if the user locked out
            var isLockedOut = await _userManager.IsLockedOutAsync(user);

            if (isLockedOut)
            {
                return new LoginServiceResponseDto
                {
                    IsSucceed = false,
                    StatusCode = 423,
                    Message = "User account is Locked",
                    NewToken = null,
                    userInfo = null,
                };
            }

            //Check password of user
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordCorrect)
            {
                //increase the locked count
                await _userManager.AccessFailedAsync(user);

                // check if the user locked out
                var userStatus = await _userManager.IsLockedOutAsync(user);

                if (userStatus)
                {
                    return new LoginServiceResponseDto
                    {
                        IsSucceed = false,
                        StatusCode = 423,
                        Message = "User account is Locked",
                        NewToken = null,
                        userInfo = null,
                    };
                }
                return new LoginServiceResponseDto
                {
                    IsSucceed = false,
                    StatusCode = 401,
                    Message = "Invalid username or password",
                    NewToken = null,
                    userInfo = null,
                };
            }

            // find user role
            var roles = await _userManager.GetRolesAsync(user);

            // check pathname
            bool containsRole = false;
            foreach (var role in roles)
            {
                if (loginDto.pathName.Contains(role.ToLower(), StringComparison.OrdinalIgnoreCase))
                {
                    containsRole = true;
                    break;
                }
            }

            if(containsRole)
            {
                //Return Token and userInfo to front-end
                var NewToken = await _generateResponseService.GenerateJwtTokenAsync(user);

                //var rolesList = roles.ToList();

                var userInfo = _generateResponseService.GenerateUserInfoAsync(user, roles);
                await _logService.SaveNewLog(user.UserName, "New Login");

                return new LoginServiceResponseDto()
                {
                    IsSucceed = true,
                    StatusCode = 201,
                    Message = "Login was successfull",
                    NewToken = NewToken,
                    userInfo = userInfo
                };
            }

            return new LoginServiceResponseDto
            {
                IsSucceed = false,
                StatusCode = 401,
                Message = "Inavalid path",
                NewToken = null,
                userInfo = null,
            };

        }

        public async Task<LoginServiceResponseDto?> MeAsync(MeDto meDto)
        {
            ClaimsPrincipal handler = new JwtSecurityTokenHandler().ValidateToken(meDto.Token, new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                ValidAudience = _configuration["JWT:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]))
            }, out SecurityToken securityToken);

            string decodedUserName = handler.Claims.First(q => q.Type == ClaimTypes.Name).Value;
            if (decodedUserName is null)
                return null;

            var user = await _userManager.FindByNameAsync(decodedUserName);
            if (user is null)
                return null;

            var NewToken = await _generateResponseService.GenerateJwtTokenAsync(user);

            // find user role
            var roles = await _userManager.GetRolesAsync(user);

            //var rolesList = roles.ToList();

            var userInfo = _generateResponseService.GenerateUserInfoAsync(user, roles);
            await _logService.SaveNewLog(user.UserName, "New Token Generated");

            return new LoginServiceResponseDto()
            {
                NewToken = NewToken,
                userInfo = userInfo
            };


        }

        public async Task<IEnumerable<UserInfoResult>> GetUsersListAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            List<UserInfoResult> userInfoResults = new List<UserInfoResult>();

            foreach(var user in users)
            {
                // find user role
                var roles = await _userManager.GetRolesAsync(user);

                //var rolesList = roles.ToList();

                var userInfo = _generateResponseService.GenerateUserInfoAsync(user, roles);
                userInfoResults.Add(userInfo);
            }

            return userInfoResults;
        }

        public async Task<UserInfoResult?> GetUserDetailsByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
                return null;

            // find user role
            var roles = await _userManager.GetRolesAsync(user);

            //var rolesList = roles.ToList();

            var userInfo = _generateResponseService.GenerateUserInfoAsync(user, roles);

            return userInfo;
        }

        public async Task<IEnumerable<string>> GetUsernameListAsync()
        {
            var userNames = await _userManager.Users
                .Select(q => q.UserName)
                .ToListAsync();

            return userNames;
        }

        public async Task<GeneralServiceResponseDto> ConfirmEmailAsync(string userId, string code)
        {
            if(string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
            {
                return new GeneralServiceResponseDto
                {
                    IsSucceed = false,
                    StatusCode = 400,
                    Message = "Invalid Email Confirmation Url"
                };
            }

            var isExist = await _userManager.FindByIdAsync(userId);

            if(isExist is null)
            {
                return new GeneralServiceResponseDto
                {
                    IsSucceed = false,
                    StatusCode = 400,
                    Message = "Invalid Email Parameter"
                };
            }

            var result = await _userManager.ConfirmEmailAsync(isExist, code);

            if (!result.Succeeded)
            {
                var errorString = "Email Confirmed failed because : ";
                foreach (var error in result.Errors)
                {
                    errorString += " # " + error.Description;
                }

                return new GeneralServiceResponseDto()
                {
                    IsSucceed = false,
                    StatusCode = 400,
                    Message = errorString
                };
            }

            return new GeneralServiceResponseDto
            {
                IsSucceed = true,
                StatusCode = 200,
                Message = "Email Confirmed Successfully"
            };
        }
    }
}
