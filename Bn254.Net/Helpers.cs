using System;
using System.Text.RegularExpressions;

namespace Bn254.Net
{
    internal static class Helpers
    {
        public static byte[] LeftPad(byte[] array, int length)
        {
            if (array.Length == length)
                return array;
            if (array.Length > length)
                throw new ArgumentException("Array is too long", nameof(array));
            var newArray = new byte[length];
            Array.Copy(array, 0, newArray, length - array.Length, array.Length);
            return newArray;
        }

        public static bool IsValidHex(string hex)
        {
            var regex = new Regex(@"^(0x|0X)?[a-fA-F0-9]{64}$");
            return regex.IsMatch(hex);
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            // ReSharper disable once ComplexConditionExpression
            if (hex.Length >= 2 && hex[0] == '0' && (hex[1] == 'x' || hex[1] == 'X'))
                hex = hex.Substring(2);
            var numberChars = hex.Length;
            var bytes = new byte[numberChars / 2];

            for (var i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);

            return bytes;
        }
    }
}