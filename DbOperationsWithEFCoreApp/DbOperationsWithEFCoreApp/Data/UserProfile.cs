﻿using System.Text.Json.Serialization;

namespace DbOperationsWithEFCoreApp.Data
{
    public class UserProfile
    {
        public int Id { get; set; } // Primary Key (and Foreign Key to User)
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ProfilePictureUrl { get; set; }

        // Navigation Property
        public int UserId { get; set; } // Foreign Key
        [JsonIgnore]
        public User? User { get; set; }
    }
}