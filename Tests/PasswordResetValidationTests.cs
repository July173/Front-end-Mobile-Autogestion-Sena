using AutogestionSena.MAUI.Validators;
using Xunit;

namespace AutogestionSena.Tests
{
    /// <summary>
    /// Pruebas unitarias básicas para validación de restablecimiento de contraseña
    /// </summary>
    public class PasswordResetValidationTests
    {
        [Theory]
        [InlineData("password123", true)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsPasswordValid_ValidatesCorrectly(string? password, bool expected)
        {
            var result = PasswordResetValidator.IsPasswordValid(password);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("12345678", true)]
        [InlineData("1234567", false)]
        [InlineData("", false)]
        public void IsPasswordLengthValid_ValidatesCorrectly(string password, bool expected)
        {
            var result = PasswordResetValidator.IsPasswordLengthValid(password);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("password123", "password123", true)]
        [InlineData("password123", "password124", false)]
        [InlineData("", "password123", false)]
        public void DoPasswordsMatch_ValidatesCorrectly(string password, string confirm, bool expected)
        {
            var result = PasswordResetValidator.DoPasswordsMatch(password, confirm);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("password123", true)]
        [InlineData("password", false)]
        [InlineData("12345678", false)]
        public void IsPasswordStrong_ValidatesCorrectly(string password, bool expected)
        {
            var result = PasswordResetValidator.IsPasswordStrong(password);
            Assert.Equal(expected, result);
        }
    }
}
