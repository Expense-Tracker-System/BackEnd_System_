﻿namespace backend_dotnet7.Core.Dtos.Auth
{
    public class UserInfoResult
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<string>? Roles { get; set; }
        public bool TwoFactorEnabled { get; set; }
    }
}
