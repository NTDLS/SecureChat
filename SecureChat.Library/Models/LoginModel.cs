﻿namespace SecureChat.Library.Models
{
    public class LoginModel
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string ProfileJson { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
    }
}
