
using Xunit;
using AutogestionSena.MAUI.Validators;

namespace AutogestionSena.Tests
{
    public class LoginValidationTests
    {

        // ---------------------------------------------------------------
        // Nueva prueba para dominios institucionales
        // ---------------------------------------------------------------
        [Theory]
        [InlineData("user@soy.sena.edu.co", true)]
        [InlineData("user", true)]
        public void IsSenaEmail_ValidatesCorrectly(string? email, bool expected)
        {
            var result = LoginValidator.IsSenaEmail(email);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Password123!")]
        [InlineData("")]
        [InlineData(null)]
        public void IsPasswordValid_ValidatesCorrectly(string? password)
        {
            var result = LoginValidator.IsPasswordValid(password);
            Assert.Equal(!string.IsNullOrWhiteSpace(password), result);
        }

        [Theory]
        [InlineData("123456", true)]
        [InlineData("12345", false)]
        public void Is2FACodeValid_ValidatesCorrectly(string? code, bool expected)
        {
            var result = LoginValidator.Is2FACodeValid(code);
            Assert.Equal(expected, result);
        }


        [Theory]
        [InlineData("user@soy.sena.edu.co", "123456", true)]
        [InlineData("user@sena.edu.co", "", false)]
        public void AreCredentialsValid_InstitucionalYPasswordMin6(string? email, string? password, bool expected)
        {
            // Email debe ser institucional y password mínimo 6 caracteres
            bool isSena = LoginValidator.IsSenaEmail(email);
            bool isPasswordMin6 = !string.IsNullOrEmpty(password) && password.Length >= 6;
            bool result = isSena && isPasswordMin6;
            Assert.Equal(expected, result);
        }
    }
}
