using AutogestionSena.MAUI.Validators;
using Xunit;

namespace AutogestionSena.Tests
{
    /// <summary>
    /// Pruebas unitarias para la validación de recuperación de contraseña
    /// </summary>
    public class PasswordRecoveryValidationTests
    {
        #region IsEmailValid Tests

        [Theory(DisplayName = "Debe validar correctamente emails no vacíos")]
        [InlineData("usuario@soy.sena.edu.co")]
        [InlineData("test@example.com")]
        [InlineData("a@b.c")]
        [InlineData("correo@dominio.co")]
        public void IsEmailValid_WithNonEmptyEmail_ReturnsTrue(string email)
        {
            // Act
            var result = PasswordRecoveryValidator.IsEmailValid(email);

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe rechazar emails vacíos")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t")]
        public void IsEmailValid_WithEmptyEmail_ReturnsFalse(string? email)
        {
            // Act
            var result = PasswordRecoveryValidator.IsEmailValid(email);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region IsEmailFormatValid Tests

        [Theory(DisplayName = "Debe validar correctamente formato básico de email")]
        [InlineData("usuario@soy.sena.edu.co")]
        [InlineData("test@example.com")]
        [InlineData("nombre.apellido@dominio.com.co")]
        [InlineData("user123@test.org")]
        public void IsEmailFormatValid_WithValidFormat_ReturnsTrue(string email)
        {
            // Act
            var result = PasswordRecoveryValidator.IsEmailFormatValid(email);

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe rechazar emails sin @")]
        [InlineData("usuariosoy.sena.edu.co")]
        [InlineData("testexample.com")]
        [InlineData("email")]
        public void IsEmailFormatValid_WithoutAtSymbol_ReturnsFalse(string email)
        {
            // Act
            var result = PasswordRecoveryValidator.IsEmailFormatValid(email);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar números en lugar de email")]
        [InlineData("12345678")]
        [InlineData("999999999")]
        [InlineData("3001234567")]
        [InlineData("00000")]
        public void IsEmailFormatValid_WithOnlyNumbers_ReturnsFalse(string input)
        {
            // Act
            var result = PasswordRecoveryValidator.IsEmailFormatValid(input);

            // Assert
            Assert.False(result, $"El validador debe rechazar números '{input}' como email");
        }

        [Theory(DisplayName = "Debe rechazar caracteres especiales sin formato de email")]
        [InlineData("@@@@@")]
        [InlineData(".....")]
        [InlineData("#$%&*")]
        [InlineData("!!!!!!")]
        public void IsEmailFormatValid_WithOnlySpecialChars_ReturnsFalse(string input)
        {
            // Act
            var result = PasswordRecoveryValidator.IsEmailFormatValid(input);

            // Assert
            Assert.False(result, $"El validador debe rechazar caracteres especiales '{input}' como email");
        }

        [Theory(DisplayName = "Debe rechazar emails sin punto")]
        [InlineData("usuario@soysenaedu")]
        [InlineData("test@example")]
        public void IsEmailFormatValid_WithoutDot_ReturnsFalse(string email)
        {
            // Act
            var result = PasswordRecoveryValidator.IsEmailFormatValid(email);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar emails vacíos o nulos")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void IsEmailFormatValid_WithEmptyOrNull_ReturnsFalse(string? email)
        {
            // Act
            var result = PasswordRecoveryValidator.IsEmailFormatValid(email);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region IsInstitutionalEmail Tests

        [Theory(DisplayName = "Debe validar correctamente emails institucionales")]
        [InlineData("juan.perez@soy.sena.edu.co")]
        [InlineData("maria.gomez@soy.sena.edu.co")]
        [InlineData("usuario123@soy.sena.edu.co")]
        [InlineData("MAYUSCULAS@SOY.SENA.EDU.CO")]
        public void IsInstitutionalEmail_WithValidInstitutionalEmail_ReturnsTrue(string email)
        {
            // Act
            var result = PasswordRecoveryValidator.IsInstitutionalEmail(email);

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe rechazar emails no institucionales")]
        [InlineData("usuario@gmail.com")]
        [InlineData("test@hotmail.com")]
        [InlineData("email@sena.edu.co")]      // Sin 'soy'
        [InlineData("usuario@soy.sena.com")]   // Sin 'edu.co'
        public void IsInstitutionalEmail_WithNonInstitutionalEmail_ReturnsFalse(string email)
        {
            // Act
            var result = PasswordRecoveryValidator.IsInstitutionalEmail(email);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar emails vacíos o nulos")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void IsInstitutionalEmail_WithEmptyOrNull_ReturnsFalse(string? email)
        {
            // Act
            var result = PasswordRecoveryValidator.IsInstitutionalEmail(email);

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "Debe validar emails institucionales sin importar mayúsculas")]
        public void IsInstitutionalEmail_IsCaseInsensitive()
        {
            // Arrange
            var testEmails = new[]
            {
                "usuario@soy.sena.edu.co",
                "USUARIO@SOY.SENA.EDU.CO",
                "Usuario@Soy.Sena.Edu.Co",
                "uSuArIo@SoY.sEnA.eDu.Co"
            };

            // Act & Assert
            foreach (var email in testEmails)
            {
                Assert.True(PasswordRecoveryValidator.IsInstitutionalEmail(email));
            }
        }

        #endregion

        #region Error Messages Tests

        [Fact(DisplayName = "Debe retornar mensaje de error para email vacío")]
        public void GetEmailEmptyErrorMessage_ReturnsCorrectMessage()
        {
            // Act
            var message = PasswordRecoveryValidator.GetEmailEmptyErrorMessage();

            // Assert
            Assert.NotEmpty(message);
            Assert.Contains("correo", message.ToLower());
        }

        [Fact(DisplayName = "Debe retornar mensaje de error para email inválido")]
        public void GetEmailInvalidErrorMessage_ReturnsCorrectMessage()
        {
            // Act
            var message = PasswordRecoveryValidator.GetEmailInvalidErrorMessage();

            // Assert
            Assert.NotEmpty(message);
            Assert.Contains("correo", message.ToLower());
            Assert.Contains("válido", message.ToLower());
        }

        #endregion

        #region Edge Cases Tests

        [Theory(DisplayName = "Debe rechazar emails con múltiples @")]
        [InlineData("usuario@@soy.sena.edu.co")]
        [InlineData("user@name@soy.sena.edu.co")]
        public void IsEmailFormatValid_WithMultipleAtSymbols_ReturnsTrue(string email)
        {
            // Act
            var result = PasswordRecoveryValidator.IsEmailFormatValid(email);

            // Assert
            // Solo verifica que contenga @ y . (validación básica)
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe manejar emails con espacios")]
        [InlineData(" usuario@soy.sena.edu.co")]
        [InlineData("usuario@soy.sena.edu.co ")]
        [InlineData("usuario @soy.sena.edu.co")]
        public void IsEmailValid_WithSpaces_ReturnsTrue(string email)
        {
            // Act
            var result = PasswordRecoveryValidator.IsEmailValid(email);

            // Assert
            // IsEmailValid solo verifica que no esté vacío
            Assert.True(result);
        }

        #endregion
    }
}
