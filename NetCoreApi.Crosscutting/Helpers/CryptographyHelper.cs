using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;

namespace NetCoreApp.Crosscutting.Helpers
{
    public static class CryptographyHelper
    {
        public static string EncryptPassword(string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: new byte[128 / 8],
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
        }
    }
}
