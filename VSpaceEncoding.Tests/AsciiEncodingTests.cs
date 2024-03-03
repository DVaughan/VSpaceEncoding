using FluentAssertions;

namespace VSpaceEncoding
{
    public class AsciiEncodingTests
    {
        #region Basic Encoding and Decoding Tests

        [Fact]
        public void EncodeThenDecode_ShouldReturnOriginalString_WhenGivenSimpleAsciiText()
        {
            // Arrange
            var encoder = new AsciiVSpaceEncoder();
            string originalText = "Hello, World!";

            // Act
            string encodedText = encoder.Encode(originalText);
            string decodedText = encoder.Decode(encodedText);

            // Assert
            decodedText.Should().Be(originalText);
        }

        [Fact]
        public void EncodeThenDecode_ShouldReturnEmptyString_WhenGivenEmptyString()
        {
            // Arrange
            var encoder = new AsciiVSpaceEncoder();
            string originalText = string.Empty;

            // Act
            string encodedText = encoder.Encode(originalText);
            string decodedText = encoder.Decode(encodedText);

            // Assert
            decodedText.Should().BeEmpty();
        }

        #endregion

        #region CharacterCoverageTests

        [Fact]
        public void EncodeThenDecode_ShouldReturnOriginalString_WhenGivenAllPrintableAsciiCharacters()
        {
            // Arrange
            var encoder = new AsciiVSpaceEncoder();
            string originalText = "";
            for (int i = 32; i <= 126; i++) // All printable ASCII characters
            {
                originalText += (char)i;
            }

            // Act
            string encodedText = encoder.Encode(originalText);
            string decodedText = encoder.Decode(encodedText);

            // Assert
            decodedText.Should().Be(originalText, because: "all printable ASCII characters should be encoded and then decoded accurately");
        }

        [Fact]
        public void EncodeThenDecode_ShouldReturnOriginalString_WhenGivenExtendedAsciiCharacters()
        {
            // Arrange
            var encoder = new AsciiVSpaceEncoder();
            string originalText = "";
            for (int i = 128; i <= 128; i++) // Extended ASCII characters //255
            {
                originalText += (char)i;
            }

            // Act
            string encodedText = encoder.Encode(originalText);
            string decodedText = encoder.Decode(encodedText);

            // Assert
            // Note: This test assumes your encoding scheme supports extended ASCII characters.
            // If it does not, this test may need to be adjusted or removed.
            decodedText.Should().Be(originalText, because: "extended ASCII characters should be encoded and then decoded accurately if supported");
        }

        #endregion

        #region SpecialCasesTests

        [Fact]
        public void EncodeThenDecode_ShouldReturnOriginalString_WhenGivenSingleCharacter()
        {
            // Arrange
            var encoder = new AsciiVSpaceEncoder();
            string originalText = "A";

            // Act
            string encodedText = encoder.Encode(originalText);
            string decodedText = encoder.Decode(encodedText);

            // Assert
            decodedText.Should().Be(originalText, because: "encoding and then decoding a single character should return the original character");
        }

        [Fact]
        public void EncodeThenDecode_ShouldReturnOriginalString_WhenGivenStringWithRepeatingCharacters()
        {
            // Arrange
            var encoder = new AsciiVSpaceEncoder();
            string originalText = "AAAAA";

            // Act
            string encodedText = encoder.Encode(originalText);
            string decodedText = encoder.Decode(encodedText);

            // Assert
            decodedText.Should().Be(originalText, because: "encoding and then decoding a string of repeating characters should return the original string");
        }

        [Fact]
        public void EncodeThenDecode_ShouldHandleWhitespaceCorrectly()
        {
            // Arrange
            var encoder = new AsciiVSpaceEncoder();
            string originalText = " \t\n\r";

            // Act
            string encodedText = encoder.Encode(originalText);
            string decodedText = encoder.Decode(encodedText);

            // Assert
            decodedText.Should().Be(originalText, because: "encoding and then decoding a string with spaces, tabs, and other whitespace should return the original string");
        }

        #endregion

        #region Error Handling Tests

        [Fact]
        public void Decode_ShouldThrowException_WhenGivenInvalidEncodedCharacters()
        {
            // Arrange
            var encoder = new AsciiVSpaceEncoder();
            string invalidEncodedText = "InvalidCharacters";

            // Act & Assert
            Action act = () => encoder.Decode(invalidEncodedText);

            act.Should().Throw<FormatException>()
                .WithMessage("*invalid character*", because: "decoding should fail with an appropriate exception when given invalid encoded characters");
        }

        [Fact]
        public void Decode_ShouldThrowException_WhenGivenImproperlyFormattedEncodedString()
        {
            // Arrange
            var encoder = new AsciiVSpaceEncoder();
            // Assuming an encoded string should have a specific length to be valid.
            // This example string's length does not meet the hypothetical requirement.
            string improperlyFormattedEncodedText = "ImproperLength";

            // Act & Assert
            Action act = () => encoder.Decode(improperlyFormattedEncodedText);

            act.Should().Throw<FormatException>(because: "decoding should fail with an appropriate exception when the encoded string is improperly formatted");
        }

        #endregion

        #region PerformanceAndLimitsTests

        [Fact]
        public void EncodeThenDecode_ShouldHandleVeryLongStringCorrectly()
        {
            // Arrange
            var encoder = new AsciiVSpaceEncoder();
            string originalText = new string('A', 10000); // A very long string

            // Act
            string encodedText = encoder.Encode(originalText);
            string decodedText = encoder.Decode(encodedText);

            // Assert
            decodedText.Should().Be(originalText, because: "encoding and then decoding a very long string should return the original string without errors");
        }

        [Fact]
        public void EncodeThenDecode_ShouldNotThrowException_ForMaximumSupportedStringLength()
        {
            // Arrange
            var encoder = new AsciiVSpaceEncoder();
            // Assuming 65535 is the maximum length that we want to test, adjust based on your specific limits
            string originalText = new string('B', 65535);

            // Act & Assert
            var actEncode = () => encoder.Encode(originalText);
            var actDecode = () => encoder.Decode(actEncode());

            // Encoding and decoding actions should not throw any exception
            actEncode.Should().NotThrow(because: "encoding should support strings up to the maximum specified length without throwing exceptions");
            actDecode.Should().NotThrow(because: "decoding should support strings up to the maximum specified length without throwing exceptions");
        }

        #endregion

        #region RobustnessAndEdgeCasesTests

        [Fact]
        public void Encode_ShouldThrowException_WhenGivenNonAsciiCharacters()
        {
            // Arrange
            var encoder = new AsciiVSpaceEncoder();
            string nonAsciiText = "Hello, 世界!"; // Contains Unicode characters outside the ASCII range

            // Act & Assert
            Action act = () => encoder.Encode(nonAsciiText);

            // Expecting an exception indicates the system cannot handle non-ASCII text.
            act.Should().Throw<FormatException>(because: "the system should throw an exception when given non-ASCII characters");
        }

        #endregion

        #region More Encode and Decode Tests

        [Theory]
        [InlineData("Hello, World!")] // Basic ASCII
        [InlineData("1234567890")] // Digits
        [InlineData("!@#$%^&*()_+-=[]{}|;':,./<>?")] // Special characters
        [InlineData(" ")] // Single space
        [InlineData("    ")] // Multiple spaces
        [InlineData("\t")] // Tab
        [InlineData("\n")] // New line
        [InlineData("This is a test.\nWith multiple lines.\nAnd special characters!@#$%^&*()")]
        [InlineData("Extended ASCII: ñáéíóú")] // Extended ASCII characters
        public void EncodeThenDecode_ShouldReturnOriginalString_ForVariousInputText(string originalText)
        {
            // Arrange
            var encoder = new AsciiVSpaceEncoder();

            // Act
            string encodedText = encoder.Encode(originalText);
            string decodedText = encoder.Decode(encodedText);

            // Assert
            decodedText.Should().Be(originalText, because: "encoding and then decoding should return the original text for various types of input");
        }

        #endregion

    }
}