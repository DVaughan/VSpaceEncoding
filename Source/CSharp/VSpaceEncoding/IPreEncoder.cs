/* Copyright © 2024, Daniel Vaughan. All rights reserved.
   Licensed under the MIT license. See the LICENSE file. */

namespace VSpaceEncoding
{
	public interface IPreEncoder
	{
		List<int> Encode(string rawInput);
		string Decode(List<int> values);

		/// <summary>
		/// The number of different symbols in an encoded string. 
		/// For Base64 this is 64.
		/// </summary>
		int SymbolCount { get; }
	}
}