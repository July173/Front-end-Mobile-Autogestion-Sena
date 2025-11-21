using AutogestionSena.MAUI.Validators;
using Xunit;

namespace AutogestionSena.Tests
{
    /// <summary>
    /// Pruebas unitarias para la validación de restablecimiento de contraseña
    /// </summary>
    public class PasswordResetValidationTests
    {
        #region IsPasswordValid Tests

        [Theory(DisplayName = "Debe validar correctamente contraseñas no vacías")]
        [InlineData("password123")]
        [InlineData("12345678")]
        [InlineData("abcdefgh")]
        [InlineData("Pass123!")]
        [InlineData("a")]
        public void IsPasswordValid_WithNonEmptyPassword_ReturnsTrue(string password)
        {
            // Act
            var result = PasswordResetValidator.IsPasswordValid(password);

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe rechazar contraseñas vacías")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t")]
        public void IsPasswordValid_WithEmptyPassword_ReturnsFalse(string? password)
        {
            // Act
            var result = PasswordResetValidator.IsPasswordValid(password);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region IsPasswordLengthValid Tests

        [Theory(DisplayName = "Debe validar correctamente contraseñas con longitud mínima")]
        [InlineData("12345678")]      // Exactamente 8
        [InlineData("123456789")]     // 9 caracteres
        [InlineData("password123")]   // Más de 8
        [InlineData("abcdefghij")]    // 10 caracteres
        public void IsPasswordLengthValid_WithValidLength_ReturnsTrue(string password)
        {
            // Act
            var result = PasswordResetValidator.IsPasswordLengthValid(password);

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe rechazar contraseñas menores a 8 caracteres")]
        [InlineData("1234567")]   // 7 caracteres
        [InlineData("123456")]    // 6 caracteres
        [InlineData("12345")]     // 5 caracteres
        [InlineData("1")]         // 1 carácter
        public void IsPasswordLengthValid_WithShortPassword_ReturnsFalse(string password)
        {
            // Act
            var result = PasswordResetValidator.IsPasswordLengthValid(password);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar contraseñas vacías o nulas")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void IsPasswordLengthValid_WithEmptyOrNull_ReturnsFalse(string? password)
        {
            // Act
            var result = PasswordResetValidator.IsPasswordLengthValid(password);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region DoPasswordsMatch Tests

        [Theory(DisplayName = "Debe validar correctamente contraseñas coincidentes")]
        [InlineData("password123", "password123")]
        [InlineData("12345678", "12345678")]
        [InlineData("Test@123", "Test@123")]
        [InlineData("abc", "abc")]
        public void DoPasswordsMatch_WithMatchingPasswords_ReturnsTrue(string password, string confirmPassword)
        {
            // Act
            var result = PasswordResetValidator.DoPasswordsMatch(password, confirmPassword);

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe rechazar contraseñas que no coinciden")]
        [InlineData("password123", "password124")]
        [InlineData("Test123", "test123")]           // Sensible a mayúsculas
        [InlineData("abc", "abcd")]
        [InlineData("password", "Password")]
        public void DoPasswordsMatch_WithNonMatchingPasswords_ReturnsFalse(string password, string confirmPassword)
        {
            // Act
            var result = PasswordResetValidator.DoPasswordsMatch(password, confirmPassword);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar si alguna contraseña está vacía")]
        [InlineData("password123", null)]
        [InlineData(null, "password123")]
        [InlineData("", "password123")]
        [InlineData("password123", "")]
        [InlineData("   ", "password123")]
        public void DoPasswordsMatch_WithEmptyPassword_ReturnsFalse(string? password, string? confirmPassword)
        {
            // Act
            var result = PasswordResetValidator.DoPasswordsMatch(password, confirmPassword);

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "Debe rechazar si ambas contraseñas están vacías")]
        public void DoPasswordsMatch_WithBothEmpty_ReturnsFalse()
        {
            // Act
            var result = PasswordResetValidator.DoPasswordsMatch(null, null);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region IsPasswordStrong Tests

        [Theory(DisplayName = "Debe validar contraseñas fuertes con letras y números")]
        [InlineData("password123")]
        [InlineData("abc123")]
        [InlineData("Test1234")]
        [InlineData("1a2b3c4d")]
        [InlineData("Secure99")]
        public void IsPasswordStrong_WithLettersAndNumbers_ReturnsTrue(string password)
        {
            // Act
            var result = PasswordResetValidator.IsPasswordStrong(password);

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe rechazar contraseñas solo con letras")]
        [InlineData("password")]
        [InlineData("abcdefgh")]
        [InlineData("Test")]
        public void IsPasswordStrong_WithOnlyLetters_ReturnsFalse(string password)
        {
            // Act
            var result = PasswordResetValidator.IsPasswordStrong(password);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar contraseñas solo con números")]
        [InlineData("12345678")]
        [InlineData("99999999")]
        [InlineData("11223344")]
        public void IsPasswordStrong_WithOnlyNumbers_ReturnsFalse(string password)
        {
            // Act
            var result = PasswordResetValidator.IsPasswordStrong(password);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar contraseñas vacías o nulas")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void IsPasswordStrong_WithEmptyOrNull_ReturnsFalse(string? password)
        {
            // Act
            var result = PasswordResetValidator.IsPasswordStrong(password);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe validar contraseñas con caracteres especiales y letras/números")]
        [InlineData("Pass@123")]
        [InlineData("Test!456")]
        [InlineData("Secure#99")]
        public void IsPasswordStrong_WithSpecialCharactersAndAlphanumeric_ReturnsTrue(string password)
        {
            // Act
            var result = PasswordResetValidator.IsPasswordStrong(password);

            // Assert
            Assert.True(result);
        }

        #endregion

        #region Error Messages Tests

        [Fact(DisplayName = "Debe retornar mensaje de error para contraseña vacía")]
        public void GetPasswordEmptyErrorMessage_ReturnsCorrectMessage()
        {
            // Act
            var message = PasswordResetValidator.GetPasswordEmptyErrorMessage();

            // Assert
            Assert.NotEmpty(message);
            Assert.Contains("contraseña", message.ToLower());
        }

        [Fact(DisplayName = "Debe retornar mensaje de error para contraseña corta")]
        public void GetPasswordLengthErrorMessage_ReturnsCorrectMessage()
        {
            // Act
            var message = PasswordResetValidator.GetPasswordLengthErrorMessage();

            // Assert
            Assert.NotEmpty(message);
            Assert.Contains("8", message);
            Assert.Contains("caracteres", message.ToLower());
        }

        [Fact(DisplayName = "Debe retornar mensaje de error cuando las contraseñas no coinciden")]
        public void GetPasswordMismatchErrorMessage_ReturnsCorrectMessage()
        {
            // Act
            var message = PasswordResetValidator.GetPasswordMismatchErrorMessage();

            // Assert
            Assert.NotEmpty(message);
            Assert.Contains("no coinciden", message.ToLower());
        }

        #endregion

        #region MinPasswordLength Constant Test

        [Fact(DisplayName = "MinPasswordLength debe ser 8")]
        public void MinPasswordLength_ShouldBe8()
        {
            // Assert
            Assert.Equal(8, PasswordResetValidator.MinPasswordLength);
        }

        #endregion

        #region Edge Cases Tests

        [Theory(DisplayName = "Debe manejar contraseñas con espacios")]
        [InlineData("pass word")]
        [InlineData(" password")]
        [InlineData("password ")]
        public void IsPasswordLengthValid_WithSpaces_ConsidersSpacesInLength(string password)
        {
            // Act
            var result = PasswordResetValidator.IsPasswordLengthValid(password);

            // Assert
            // Los espacios cuentan como caracteres
            Assert.Equal(password.Length >= 8, result);
        }

        [Fact(DisplayName = "Debe manejar contraseñas muy largas")]
        public void IsPasswordLengthValid_WithVeryLongPassword_ReturnsTrue()
        {
            // Arrange
            var longPassword = new string('a', 1000);

            // Act
            var result = PasswordResetValidator.IsPasswordLengthValid(longPassword);

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe ser sensible a mayúsculas al comparar contraseñas")]
        [InlineData("Password", "password")]
        [InlineData("TEST", "test")]
        [InlineData("AbC", "aBc")]
        public void DoPasswordsMatch_IsCaseSensitive_ReturnsFalse(string password, string confirmPassword)
        {
            // Act
            var result = PasswordResetValidator.DoPasswordsMatch(password, confirmPassword);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Cross-Validation Tests (Datos del tipo incorrecto)

        [Theory(DisplayName = "Debe rechazar números cuando se espera contraseña (pero pasa validación básica)")]
        [InlineData("12345678")]
        [InlineData("99999999")]
        public void IsPasswordValid_WithOnlyNumbers_ReturnsTrueButWeak(string numberPassword)
        {
            // Act
            var isValid = PasswordResetValidator.IsPasswordValid(numberPassword);
            var isStrong = PasswordResetValidator.IsPasswordStrong(numberPassword);

            // Assert
            Assert.True(isValid, "La contraseña no está vacía");
            Assert.False(isStrong, "Pero la contraseña solo con números NO es fuerte (falta letras)");
        }

        [Theory(DisplayName = "Debe rechazar emails cuando se espera contraseña (pero pasa validación básica)")]
        [InlineData("usuario@example.com")]
        [InlineData("test@sena.edu.co")]
        public void IsPasswordValid_WithEmailInput_ReturnsTrueButIncorrect(string emailInput)
        {
            // Act
            var isValid = PasswordResetValidator.IsPasswordValid(emailInput);

            // Assert
            Assert.True(isValid, "IsPasswordValid solo verifica que no esté vacío, un email puede usarse como contraseña técnicamente");
        }

        [Fact(DisplayName = "Validación completa: contraseña con solo letras debe ser rechazada por IsPasswordStrong")]
        public void CompleteValidation_OnlyLettersPassword_FailsStrengthCheck()
        {
            // Arrange
            var weakPassword = "abcdefgh";

            // Act
            var isNotEmpty = PasswordResetValidator.IsPasswordValid(weakPassword);
            var hasMinLength = PasswordResetValidator.IsPasswordLengthValid(weakPassword);
            var isStrong = PasswordResetValidator.IsPasswordStrong(weakPassword);

            // Assert
            Assert.True(isNotEmpty, "✅ No está vacía");
            Assert.True(hasMinLength, "✅ Tiene 8+ caracteres");
            Assert.False(isStrong, "❌ Pero NO es fuerte (solo letras, falta números)");
        }

        [Fact(DisplayName = "Validación completa: contraseña con solo números debe ser rechazada por IsPasswordStrong")]
        public void CompleteValidation_OnlyNumbersPassword_FailsStrengthCheck()
        {
            // Arrange
            var weakPassword = "12345678";

            // Act
            var isNotEmpty = PasswordResetValidator.IsPasswordValid(weakPassword);
            var hasMinLength = PasswordResetValidator.IsPasswordLengthValid(weakPassword);
            var isStrong = PasswordResetValidator.IsPasswordStrong(weakPassword);

            // Assert
            Assert.True(isNotEmpty, "✅ No está vacía");
            Assert.True(hasMinLength, "✅ Tiene 8+ caracteres");
            Assert.False(isStrong, "❌ Pero NO es fuerte (solo números, falta letras)");
        }

        [Fact(DisplayName = "Validación completa: contraseña fuerte pasa todas las validaciones")]
        public void CompleteValidation_StrongPassword_PassesAllChecks()
        {
            // Arrange
            var strongPassword = "Password123";

            // Act
            var isNotEmpty = PasswordResetValidator.IsPasswordValid(strongPassword);
            var hasMinLength = PasswordResetValidator.IsPasswordLengthValid(strongPassword);
            var isStrong = PasswordResetValidator.IsPasswordStrong(strongPassword);

            // Assert
            Assert.True(isNotEmpty, "✅ No está vacía");
            Assert.True(hasMinLength, "✅ Tiene 8+ caracteres");
            Assert.True(isStrong, "✅ Es fuerte (tiene letras Y números)");
        }

        #endregion
    }
}
