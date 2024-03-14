using System.Security.Cryptography;

namespace NanoERP.API.Services
{
    public class AuthService
    {
        public static string GenerateRandomPassword(int length = 10)
        {
            const string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
            const string numericChars = "0123456789";
            const string specialChars = "!@#$%^&*()_+";

            var passwordChars = new char[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                passwordChars[0] = uppercaseChars[GetCryptoRandomNumber(0, uppercaseChars.Length, rng)];
                passwordChars[1] = lowercaseChars[GetCryptoRandomNumber(0, lowercaseChars.Length, rng)];
                passwordChars[2] = numericChars[GetCryptoRandomNumber(0, numericChars.Length, rng)];
                passwordChars[3] = specialChars[GetCryptoRandomNumber(0, specialChars.Length, rng)];

                string allChars = uppercaseChars + lowercaseChars + numericChars + specialChars;
                for (int i = 4; i < length; i++)
                {
                    passwordChars[i] = allChars[GetCryptoRandomNumber(0, allChars.Length, rng)];
                }

                FisherYatesShuffle(passwordChars, rng);
            }

            return new string(passwordChars);
        }

        private static int GetCryptoRandomNumber(int min, int max, RandomNumberGenerator rng)
        {
            byte[] randomNumber = new byte[4];
            rng.GetBytes(randomNumber);
            int value = BitConverter.ToInt32(randomNumber, 0);
            return Math.Abs(value % (max - min)) + min;
        }

        private static void FisherYatesShuffle(char[] array, RandomNumberGenerator rng)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = GetCryptoRandomNumber(0, n, rng);
                n--;
                char temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }
}