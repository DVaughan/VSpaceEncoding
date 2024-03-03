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