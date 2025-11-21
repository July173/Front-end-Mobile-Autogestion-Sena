using AutogestionSena.MAUI.Validators;
using Xunit;

namespace AutogestionSena.Tests
{
    /// <summary>
    /// Pruebas unitarias b�sicas para validaci�n de 2FA
    /// </summary>
    public class TwoFactorValidationTests
    {
        [Theory]
        [InlineData("1", "2", "3", "4", "5", "6", true)]
        [InlineData("", "2", "3", "4", "5", "6", false)]
        [InlineData("1", "2", "", "4", "5", "6", false)]
        public void IsCodeComplete_ValidatesCorrectly(string c1, string c2, string c3, string c4, string c5, string c6, bool expected)
        {
            var result = TwoFactorValidator.IsCodeComplete(c1, c2, c3, c4, c5, c6);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("1", "2", "3", "4", "5", "6", true)]
        [InlineData("1", "a", "3", "4", "5", "6", false)]
        [InlineData("1", "2", "@", "4", "5", "6", false)]
        public void IsCodeNumeric_ValidatesCorrectly(string c1, string c2, string c3, string c4, string c5, string c6, bool expected)
        {
            var result = TwoFactorValidator.IsCodeNumeric(c1, c2, c3, c4, c5, c6);
            Assert.Equal(expected, result);
        }
 
        [Fact]
        public void CombineCode_CombinesCorrectly()
        {
            var result = TwoFactorValidator.CombineCode("1", "2", "3", "4", "5", "6");
            Assert.Equal("123456", result);
        }

        [Theory]
        [InlineData("1", true)]
        [InlineData("a", false)]
        [InlineData("", false)]
        public void IsDigit_ValidatesCorrectly(string digit, bool expected)
        {
            var result = TwoFactorValidator.IsDigit(digit);
            Assert.Equal(expected, result);
        }
    }
}
