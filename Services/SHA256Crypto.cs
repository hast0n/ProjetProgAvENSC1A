using System;
using System.Text;
using System.Security.Cryptography;

// Not very secure, make use of BCrypt instead
// But for the sake of simplicity, let's only
// use .NET built-in packages

/*
    Not very secure, make use of BCrypt instead but for the sake of simplicity,
    let's only use .NET built-in packages.

    BCrypt available through nuget: https://www.nuget.org/packages/BCrypt.Net-Next/



    Example use :

    Hash a new password for storing in the database.
    The function automatically generates a cryptographically safe salt.
    
    >> string hashToStoreInDb = BCrypt.HashPassword(password);
       
    Check if the hash of the entered login password, matches the stored hash.
    The salt and the cost factor will be extracted from existingHashFromDb.
    
    >> bool isPasswordCorrect = BCrypt.Verify(password, existingHashFromDb);
 */


// From https://stackoverflow.com/a/57111649
namespace ProjetProgAvENSC1A.Services
{
    public static class SHA
    {

        public static string GenerateSHA512String(string inputString)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

    }
}