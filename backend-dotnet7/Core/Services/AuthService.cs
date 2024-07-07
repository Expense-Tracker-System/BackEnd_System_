using backend_dotnet7.Core.Constants;
using backend_dotnet7.Core.Dtos.Auth;
using backend_dotnet7.Core.Dtos.General;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        public AuthService(UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            ILogService logService, 
            IConfiguration configuration, 
            IUserPasswordConfirmService userPasswordConfirmService,
            IUserEmailService userEmailService,
            IUserPhoneNumberService userPhoneNumberService,
            IGenerateResponseService generateResponseService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logService = logService;
            _configuration = configuration;
            _userPasswordConfirmService = userPasswordConfirmService;
            _userEmailService = userEmailService;
            _userPhoneNumberService = userPhoneNumberService;
            _generateResponseService = generateResponseService;
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

        public async Task<GeneralServiceResponseDto> RegisterAsync(RegisterDto registerDto)
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
                    Message = "errorString"
                };
            }

            //Add a Default User Role to all users
            await _userManager.AddToRoleAsync(newUser, StaticUserRoles.USER);
            await _logService.SaveNewLog(newUser.UserName, "Registered to Website");

            return new GeneralServiceResponseDto()
            {
                IsSucceed = true,
                StatusCode = 201,
                Message = "User"
            };

        }

        public async Task<LoginServiceResponseDto?> LoginAsync(LoginDto loginDto)
        {
            //Find user with username
            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if (user is null)
            {
                return null;
            }

            //Check password of user
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordCorrect)
            {
                return null;
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
                    NewToken = NewToken,
                    userInfo = userInfo
                };
            }

            return null;

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

        public async Task<LoginServiceResponseDto?> UpdateFirstLastName(UpdateFirstLastNameDto updateFirstLastNameDto)
        {
            var isExistsUser = await _userManager.FindByNameAsync(updateFirstLastNameDto.Username);

            if(isExistsUser is null)
            {
                return null;
            }

            isExistsUser.FirstName = updateFirstLastNameDto.FirstName;
            isExistsUser.LastName = updateFirstLastNameDto.LastName;

            var updateResult = await _userManager.UpdateAsync(isExistsUser);

            if (!updateResult.Succeeded)
            {
                return null;
            }

            // generate new JWT token...
            var newToken = await _generateResponseService.GenerateJwtTokenAsync(isExistsUser);

            // find user role
            var roles = await _userManager.GetRolesAsync(isExistsUser);

            //var rolesList = roles.ToList();

            var userInfo = _generateResponseService.GenerateUserInfoAsync(isExistsUser, roles);
            await _logService.SaveNewLog(isExistsUser.UserName, "New Token Generated");

            return new LoginServiceResponseDto
            {
                NewToken = newToken,
                userInfo = userInfo
            };
        }

    }
}
