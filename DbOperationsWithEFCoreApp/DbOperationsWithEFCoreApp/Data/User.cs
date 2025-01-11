namespace DbOperationsWithEFCoreApp.Data
{
    public class User
    {
        public int Id { get; set; } // Primary Key
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        // Navigation Property for One-to-One Relationship
        public UserProfile? Profile { get; set; }
    }
}
