using System;
using System.Security.Cryptography;
using System.Text;

namespace Data.Services
{
    /// <summary>
    /// PBKDF2 (HMACSHA256) ile parola hash üretim/doğrulama yardımcı sınıfı.
    /// Saklama formatı: PBKDF2$HMACSHA256$<ITER>$<SaltBase64>$<HashBase64>
    /// </summary>
    public static class PasswordHasher
    {
        // Varsayılan parametreler (güncel pratikler için uygundur)
        private const int DefaultIterations = 100_000; // 100k+ önerilir
        private const int SaltSize = 16;               // 128 bit salt
        private const int HashSize = 32;               // 256 bit çıktı

        public static string CreateHash(string password, int iterations = DefaultIterations)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            // Kriptografik rastgele salt üret
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);

            byte[] hash = Pbkdf2(password, salt, iterations, HashSize);
            string saltB64 = Convert.ToBase64String(salt);
            string hashB64 = Convert.ToBase64String(hash);

            // Format: PBKDF2$HMACSHA256$<iter>$<salt>$<hash>
            return $"PBKDF2$HMACSHA256${iterations}${saltB64}${hashB64}";
        }

        public static bool Verify(string password, string stored)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(stored))
                return false;

            // Parçala
            // Beklenen: 5 parça
            // 0: PBKDF2
            // 1: HMACSHA256
            // 2: iter
            // 3: saltB64
            // 4: hashB64
            var parts = stored.Split('$');
            if (parts.Length != 5) return false;
            if (!parts[0].Equals("PBKDF2", StringComparison.OrdinalIgnoreCase)) return false;
            // İsterseniz algorithm adını da doğrulayın:
            // if (!parts[1].Equals("HMACSHA256", StringComparison.OrdinalIgnoreCase)) return false;

            if (!int.TryParse(parts[2], out int iterations)) return false;

            byte[] salt;
            byte[] expectedHash;
            try
            {
                salt = Convert.FromBase64String(parts[3]);
                expectedHash = Convert.FromBase64String(parts[4]);
            }
            catch
            {
                return false;
            }

            byte[] actualHash = Pbkdf2(password, salt, iterations, expectedHash.Length);

            // Zaman sabitliğinde karşılaştırma (timing-attack’e dayanıklı)
            return FixedTimeEquals(actualHash, expectedHash);
        }

        private static byte[] Pbkdf2(string password, byte[] salt, int iterations, int outputBytes)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(outputBytes);
            }
        }

        private static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length) return false;

            int diff = 0;
            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];
            return diff == 0;
        }
    }
}
