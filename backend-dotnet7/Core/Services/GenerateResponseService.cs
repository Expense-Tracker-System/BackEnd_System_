using backend_dotnet7.Core.Dtos.Auth;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using backend_dotnet7.Core.Template;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend_dotnet7.Core.Services
{
    public class GenerateResponseService : IGenerateResponseService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public GenerateResponseService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IEmailService emailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim("FirstName",user.FirstName),
                new Claim("LastName",user.LastName)
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            //Created Our Own Secret
            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            //Create Credential
            var signingCredentials = new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256);

            //Create new Token
            var tokenObject = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(1),
                claims: authClaims,
                signingCredentials: signingCredentials
                );


            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);
            return token;
        }

        public UserInfoResult GenerateUserInfoAsync(ApplicationUser user, IEnumerable<string> roles)
        {
            return new UserInfoResult()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                CreatedAt = user.CreatedAt,
                Roles = roles
            };
        }

        public async Task<LoginServiceResponseDto> GenerateOTPFor2Factor(ApplicationUser user)
        {
            // get providers
            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);

            // check providers
            if (!providers.Contains("Email"))
            {
                return new LoginServiceResponseDto
                {
                    IsSucceed = false,
                    StatusCode = 401,
                    Message = "Invalid 2-Factor Provider",
                    NewToken = null,
                    userInfo = null,
                    is2FactorRequired = false,
                    provider = null,
                };
            }

            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

            var _2FaEmailTemplate = new _2FAEmailTemplate();
            var htmltext = _2FaEmailTemplate._2FAEmail(token);
            var emailResponse = await _emailService.SendEmail(htmltext, user.Email, "2FA");

            if(emailResponse.IsSuccess is false)
            {
                return new LoginServiceResponseDto
                {
                    IsSucceed = false,
                    StatusCode = 400,
                    Message = emailResponse.Message,
                    NewToken = null,
                    userInfo = null,
                    is2FactorRequired = false,
                    provider = null,
                };
            }

            return new LoginServiceResponseDto
            {
                IsSucceed = false,
                StatusCode = 201,
                Message = emailResponse.Message + ", Please check your email for 2FA",
                NewToken = null,
                userInfo = null,
                is2FactorRequired = true,
                provider = "Email",
            };
        }
    }
}
