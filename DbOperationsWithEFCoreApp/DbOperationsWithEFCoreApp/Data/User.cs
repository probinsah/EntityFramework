using Microsoft.EntityFrameworkCore;
using System;

namespace DbOperationsWithEFCoreApp.Data
{
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int Id { get; set; } // Primary Key
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }

        // Navigation Property for One-to-One Relationship
        public UserProfile Profile { get; set; }
    }
    public class LoginRequest
    {
        public string Username { get; set; }
        public string EmailId   { get; set; }
        public string PasswordHash { get; set; }
    }
    public class ForgetPassword
    {
        public string Email { get; set; }
        public DateOnly dateOfbirth { get; set; }
    }
    public class ResetPassword
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
