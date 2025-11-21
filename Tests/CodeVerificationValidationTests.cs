using AutogestionSena.MAUI.Validators;
using Xunit;

namespace AutogestionSena.Tests
{
    /// <summary>
    /// Pruebas unitarias para la validación de códigos de verificación
    /// </summary>
    public class CodeVerificationValidationTests
    {
        #region IsCodeValid Tests

        [Theory(DisplayName = "Debe validar correctamente códigos no vacíos")]
        [InlineData("123456")]
        [InlineData("000000")]
        [InlineData("999999")]
        [InlineData("123")]
        [InlineData("1")]
        [InlineData("abcdef")]
        [InlineData("abc123")]
        public void IsCodeValid_WithNonEmptyCode_ReturnsTrue(string code)
        {
            // Act
            var result = CodeVerificationValidator.IsCodeValid(code);

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe rechazar códigos vacíos")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public void IsCodeValid_WithEmptyCode_ReturnsFalse(string? code)
        {
            // Act
            var result = CodeVerificationValidator.IsCodeValid(code);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region IsCode6DigitsValid Tests

        [Theory(DisplayName = "Debe validar correctamente códigos de 6 dígitos")]
        [InlineData("123456")]
        [InlineData("000000")]
        [InlineData("999999")]
        [InlineData("100200")]
        [InlineData("543210")]
        public void IsCode6DigitsValid_WithValid6DigitCode_ReturnsTrue(string code)
        {
            // Act
            var result = CodeVerificationValidator.IsCode6DigitsValid(code);

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe rechazar códigos con longitud incorrecta")]
        [InlineData("12345")]    // 5 dígitos
        [InlineData("1234567")]  // 7 dígitos
        [InlineData("1")]        // 1 dígito
        [InlineData("12")]       // 2 dígitos
        public void IsCode6DigitsValid_WithIncorrectLength_ReturnsFalse(string code)
        {
            // Act
            var result = CodeVerificationValidator.IsCode6DigitsValid(code);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar códigos con caracteres no numéricos")]
        [InlineData("12345a")]   // Con letra
        [InlineData("abcdef")]   // Solo letras
        [InlineData("12 456")]   // Con espacio
        [InlineData("123-56")]   // Con guión
        [InlineData("123.56")]   // Con punto
        public void IsCode6DigitsValid_WithNonNumericCharacters_ReturnsFalse(string code)
        {
            // Act
            var result = CodeVerificationValidator.IsCode6DigitsValid(code);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar emails cuando se espera código")]
        [InlineData("")]
        [InlineData("test@sena.edu.co")]
        [InlineData("a@b.co")]
        public void IsCode6DigitsValid_WithEmailInput_ReturnsFalse(string emailInput)
        {
            // Act
            var result = CodeVerificationValidator.IsCode6DigitsValid(emailInput);

            // Assert
            Assert.False(result, $"El validador debe rechazar email '{emailInput}' cuando se espera un código de 6 dígitos");
        }

        [Theory(DisplayName = "Debe rechazar texto aleatorio cuando se espera código")]
        [InlineData("código")]
        [InlineData("verificación")]
        [InlineData("password")]
        [InlineData("usuario123")]
        public void IsCode6DigitsValid_WithRandomText_ReturnsFalse(string textInput)
        {
            // Act
            var result = CodeVerificationValidator.IsCode6DigitsValid(textInput);

            // Assert
            Assert.False(result, $"El validador debe rechazar texto '{textInput}' cuando se espera un código de 6 dígitos");
        }

        [Theory(DisplayName = "Debe rechazar códigos vacíos o nulos")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void IsCode6DigitsValid_WithEmptyOrNull_ReturnsFalse(string? code)
        {
            // Act
            var result = CodeVerificationValidator.IsCode6DigitsValid(code);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Error Messages Tests

        [Fact(DisplayName = "Debe retornar mensaje de error para código vacío")]
        public void GetCodeEmptyErrorMessage_ReturnsCorrectMessage()
        {
            // Act
            var message = CodeVerificationValidator.GetCodeEmptyErrorMessage();

            // Assert
            Assert.NotEmpty(message);
            Assert.Contains("código", message.ToLower());
        }

        [Fact(DisplayName = "Debe retornar mensaje de error para código inválido")]
        public void GetCodeInvalidErrorMessage_ReturnsCorrectMessage()
        {
            // Act
            var message = CodeVerificationValidator.GetCodeInvalidErrorMessage();

            // Assert
            Assert.NotEmpty(message);
            Assert.Contains("6", message);
            Assert.Contains("dígitos", message.ToLower());
        }

        #endregion

        #region Edge Cases Tests

        [Theory(DisplayName = "Debe manejar códigos con espacios en blanco")]
        [InlineData(" 123456")]
        [InlineData("123456 ")]
        [InlineData("123 456")]
        public void IsCode6DigitsValid_WithWhitespace_ReturnsFalse(string code)
        {
            // Act
            var result = CodeVerificationValidator.IsCode6DigitsValid(code);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe manejar códigos con caracteres especiales")]
        [InlineData("123@56")]
        [InlineData("12#456")]
        [InlineData("!23456")]
        public void IsCode6DigitsValid_WithSpecialCharacters_ReturnsFalse(string code)
        {
            // Act
            var result = CodeVerificationValidator.IsCode6DigitsValid(code);

            // Assert
            Assert.False(result);
        }

        #endregion
    }
}
