﻿namespace backend_dotnet7.Core.Dtos.Email
{
    public class MailRequest
    {
        public string? Email { get; set; }
        public string? Subject { get; set; }
        public string? EmailBody { get; set; }
    }
}