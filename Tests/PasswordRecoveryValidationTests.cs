using AutogestionSena.MAUI.Validators;
using Xunit;

namespace AutogestionSena.Tests
{
    /// <summary>
    /// Pruebas unitarias básicas para validación de recuperación de contraseña
    /// </summary>
    public class PasswordRecoveryValidationTests
    {
        [Theory]
        [InlineData("usuario@soy.sena.edu.co", true)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsEmailValid_ValidatesCorrectly(string? email, bool expected)
        {
            var result = PasswordRecoveryValidator.IsEmailValid(email);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("usuario@soy.sena.edu.co", true)]
        [InlineData("usuario@", false)]
        [InlineData("usuario.sena.edu.co", false)]
        public void IsEmailFormatValid_ValidatesCorrectly(string email, bool expected)
        {
            var result = PasswordRecoveryValidator.IsEmailFormatValid(email);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("usuario@soy.sena.edu.co", true)]
        [InlineData("usuario@gmail.com", false)]
        [InlineData("usuario@soy.sena.com", false)]
        public void IsInstitutionalEmail_ValidatesCorrectly(string email, bool expected)
        {
            var result = PasswordRecoveryValidator.IsInstitutionalEmail(email);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetEmailEmptyErrorMessage_ReturnsCorrectMessage()
        {
            var message = PasswordRecoveryValidator.GetEmailEmptyErrorMessage();
            Assert.NotEmpty(message);
            Assert.Contains("correo", message.ToLower());
        }
    }
}
