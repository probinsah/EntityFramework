namespace DbOperationsWithEFCoreApp.Utilities
{
    public class PasswordUtility
    {
        public static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
        public static bool VerifyPassword(string hashedPassword, string inputPassword)
        {
            var inputHashed = HashPassword(inputPassword);
            return hashedPassword == inputHashed;
        }
    }
}
