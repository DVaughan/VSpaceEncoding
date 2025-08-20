/* Copyright © 2024, Daniel Vaughan. All rights reserved.
   Licensed under the MIT license. See the LICENSE file. */

using System.Text;

namespace VSpaceEncoding
{
	public class AsciiVSpaceEncoder
	{
		class Characters
		{
			public const char NonBreakingSpace = '\u00A0';
			public const char LineFeed = '\n';
			public const char HorizontalTab = '\t';
			public const char CarriageReturn = '\r';
			public const char Space = '\u0020';

			/* Extended set. */
			public const char SoftHyphen = '\u00AD';
		}

		static readonly char[] symbolsDefault = [Characters.Space, 
															Characters.NonBreakingSpace, 
															Characters.HorizontalTab];

		readonly char[] symbols;
		readonly Dictionary<char, int> charToIndexMap = [];
		readonly IPreEncoder preEncoder;
		readonly int symbolsNeeded;

		static AsciiVSpaceEncoder()
		{
			Array.Sort(symbolsDefault);
		}

		public AsciiVSpaceEncoder(char[]? symbols = null, IPreEncoder? preEncoder = null)
		{
			this.preEncoder = preEncoder ?? new Base64PreEncoder();

			if (symbols == null)
			{
				this.symbols = symbolsDefault;
			}
			else
			{
				if (symbols.Length == 0)
				{
					throw new ArgumentException("Must not contain zero items.", nameof(symbols));
				}

				this.symbols = symbols;
				Array.Sort(this.symbols);
			}

			var symbolCount = this.symbols.Length;

			for (int i = 0; i < symbolCount; i++)
			{
				charToIndexMap[this.symbols[i]] = i;
			}

			/* Calculate the fixed length for each encoded character. */
			symbolsNeeded = (int)Math.Ceiling(Math.Log(this.preEncoder.SymbolCount, symbolCount));
		}

		public string Encode(string asciiText)
		{
			if (asciiText == null)
			{
				throw new ArgumentNullException(nameof(asciiText));
			}

			if (!IsExtendedAscii(asciiText))
			{
				throw new FormatException("Expected standard Ascii and extended Ascii format.");
			}

			var inputValues = preEncoder.Encode(asciiText);
			int symbolCount = symbols.Length;

			// Simplify by directly encoding each Base64 character to a fixed-length sequence
			StringBuilder encodedStringBuilder = new StringBuilder();
			foreach (int inputValue in inputValues)
			{
				int v = inputValue;

				// Assume a fixed length for each encoded base64 character for simplicity
				for (int i = 0; i < symbolsNeeded; i++) // Encoding each base64 character into 4 symbols
				{
					encodedStringBuilder.Append(symbols[v % symbolCount]);
					v /= symbolCount;
				}
			}

			return encodedStringBuilder.ToString();
		}

		public string Decode(string encodedText)
		{
			int encodingCount = symbols.Length;
			var textLength = encodedText.Length;

			List<int> outputs = new();

			// Decode each sequence of 4 symbols back to a base64 character.
			for (int i = 0; i < textLength; i += symbolsNeeded)
			{
				int output = 0;

				for (int j = 0; j < symbolsNeeded; j++)
				{
					if (charToIndexMap.TryGetValue(encodedText[i + j], out int index))
					{
						output += index * (int)Math.Pow(encodingCount, j);
					}
					else
					{
						throw new FormatException($"Encoded text contains an invalid character: {encodedText[i + j]}");
					}
				}

				outputs.Add(output);
			}

			return preEncoder.Decode(outputs);
		}

		static bool IsExtendedAscii(string text)
		{
			foreach (char c in text)
			{
				if (c > 255) // Extended ASCII includes characters up to 255
				{
					return false;
				}
			}

			return true;
		}
	}
}
