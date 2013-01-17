using System;
using System.Text;


namespace SQLDiagRunner
{
    public static class StringExtensions
    {
        private static readonly Random Rnd = new Random();
        
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static string RandomiseLastNChars(this string source, int numChars)
        {
            if (numChars > source.Length)
                throw new ArgumentException("numChars must be less than or equal to string length.");

            var sb = new StringBuilder(source.Substring(0, source.Length - numChars), source.Length);

            for (int i = 0; i < numChars; i++)
                sb.Append(Alphabet[Rnd.Next(Alphabet.Length)]);

            return sb.ToString();
        }

    }
}
