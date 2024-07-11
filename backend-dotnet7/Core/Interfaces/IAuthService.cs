﻿using backend_dotnet7.Core.Dtos.Auth;
using backend_dotnet7.Core.Dtos.General;
using System.Security.Claims;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IAuthService
    {
        Task<GeneralServiceResponseDto> SeedRolesAsync();
        Task<GeneralServiceResponseDto> RegisterAsync(RegisterDto registerDto, string callback_url);
        Task<LoginServiceResponseDto?> LoginAsync(LoginDto loginDto);
        Task<LoginServiceResponseDto?> MeAsync(MeDto meDto);
        Task<IEnumerable<UserInfoResult>> GetUsersListAsync();
        Task<UserInfoResult?> GetUserDetailsByUserNameAsync(string userName);
        Task<IEnumerable<string>> GetUsernameListAsync();
        Task<GeneralServiceResponseDto> ConfirmEmailAsync(string userId, string code);
        Task<LoginServiceResponseDto> TwoFactorAsync(TwoFactorDto twoFactorDto);
    }
}
