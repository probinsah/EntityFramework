using System.Security.Cryptography;

namespace DbOperationsWithEFCoreApp.Utilities
{
    public class PasswordUtility
    {
        public static string HashPassword(string password)
        {
            // Generate a salt
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Derive a 256-bit subkey (Hash the password with the salt)
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32);

                // Combine salt and hash into one array
                byte[] hashBytes = new byte[48];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 32);

                // Convert the combined salt + hash to a Base64 string
                return Convert.ToBase64String(hashBytes);
            }
        }
        public static bool VerifyPassword(string password, string storedHashedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHashedPassword);

            // Extract the salt from the stored hash
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Hash the input password with the extracted salt
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32);

                // Extract the hash from the stored hash bytes
                for (int i = 0; i < 32; i++)
                {
                    if (hashBytes[i + 16] != hash[i])
                        return false;
                }
            }

            return true;
        }
    }
}
