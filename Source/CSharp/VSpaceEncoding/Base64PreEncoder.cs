/* Copyright © 2024, Daniel Vaughan. All rights reserved.
   Licensed under the MIT license. See the LICENSE file. */

using System.Text;

namespace VSpaceEncoding
{
    public class Base64PreEncoder : IPreEncoder
    {
        readonly Encoding encoding;

        public Base64PreEncoder(Encoding? encoding = null)
        {
            this.encoding = encoding ?? Encoding.GetEncoding("ISO-8859-1");
        }

        public int SymbolCount => 64;

        public List<int> Encode(string asciiText)
        {
            var encoded = Convert.ToBase64String(encoding.GetBytes(asciiText)).TrimEnd('=');
            List<int> result = new();
            foreach (var symbol in encoded)
            {
                result.Add(ConvertTo6Bit(symbol));
            }

            return result;
        }

        public string Decode(List<int> values)
        {
            StringBuilder base64StringBuilder = new();
            foreach (int value in values)
            {
                base64StringBuilder.Append(ConvertFrom6Bit(value));
            }

            // Handle padding '=' signs for base64 decoding
            int mod4 = base64StringBuilder.Length % 4;
            if (mod4 > 0)
            {
                base64StringBuilder.Append(new string('=', 4 - mod4));
            }

            byte[] bytes = Convert.FromBase64String(base64StringBuilder.ToString());
            return encoding.GetString(bytes);
        }

        public int EncodeRawInput(char input)
        {
            return ConvertTo6Bit(input);
        }

        int ConvertTo6Bit(char c)
        {
            switch (c)
            {
                case >= 'A' and <= 'Z':
                    return c - 'A';
                case >= 'a' and <= 'z':
                    return c - 'a' + 26;
                case >= '0' and <= '9':
                    return c - '0' + 52;
                case '+':
                    return 62;
                case '/':
                    return 63;
            }

            throw new ArgumentException($"Invalid character for Base64 encoding: {c}");
        }

        char ConvertFrom6Bit(int value)
        {
            switch (value)
            {
                case < 26:
                    return (char)('A' + value);
                case < 52:
                    return (char)('a' + value - 26);
                case < 62:
                    return (char)('0' + value - 52);
                case 62:
                    return '+';
                case 63:
                    return '/';
                default:
                    throw new ArgumentException("Invalid 6-bit value.");
            }
        }
    }
}
