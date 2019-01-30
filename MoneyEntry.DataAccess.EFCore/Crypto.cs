using System.Security.Cryptography;

namespace MoneyEntry.DataAccess.EFCore
{
    public static class Crypto
    {
        private static readonly int _defaultByteLengthSalt = 64;

        internal static byte[] GetSalt(int length = 0)
        {
            if (length == 0)
                length = _defaultByteLengthSalt;

            var salt = new byte[length];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }
    }
}
