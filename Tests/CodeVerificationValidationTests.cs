using AutogestionSena.MAUI.Validators;
using Xunit;

namespace AutogestionSena.Tests
{
    /// <summary>
    /// Pruebas unitarias básicas para validación de códigos de verificación
    /// </summary>
    public class CodeVerificationValidationTests
    {
        [Theory]
        [InlineData("123456", true)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsCodeValid_ValidatesCorrectly(string? code, bool expected)
        {
            var result = CodeVerificationValidator.IsCodeValid(code);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("123456", true)]
        [InlineData("12345", false)]
        [InlineData("1234567", false)]
        public void IsCode6DigitsValid_ValidatesCorrectly(string code, bool expected)
        {
            var result = CodeVerificationValidator.IsCode6DigitsValid(code);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetCodeInvalidErrorMessage_ReturnsCorrectMessage()
        {
            var message = CodeVerificationValidator.GetCodeInvalidErrorMessage();
            Assert.NotEmpty(message);
            Assert.Contains("6", message);
            Assert.Contains("d�gitos", message.ToLower());
        }

        [Theory]
        [InlineData(" 123456", false)]
        [InlineData("123456 ", false)]
        [InlineData("123 456", false)]
        public void IsCode6DigitsValid_WithWhitespace_ReturnsFalse(string code, bool expected)
        {
            var result = CodeVerificationValidator.IsCode6DigitsValid(code);
            Assert.Equal(expected, result);
        }
    }
}
