using AutogestionSena.MAUI.Validators;
using Xunit;

namespace AutogestionSena.Tests
{
    /// <summary>
    /// Pruebas unitarias para la validación de autenticación de dos factores (2FA)
    /// </summary>
    public class TwoFactorValidationTests
    {
        #region IsCodeComplete Tests

        [Fact(DisplayName = "Debe validar correctamente código completo de 6 dígitos")]
        public void IsCodeComplete_WithAllDigitsComplete_ReturnsTrue()
        {
            // Act
            var result = TwoFactorValidator.IsCodeComplete("1", "2", "3", "4", "5", "6");

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe rechazar código incompleto - falta primer dígito")]
        [InlineData("", "2", "3", "4", "5", "6")]
        [InlineData(null, "2", "3", "4", "5", "6")]
        [InlineData("   ", "2", "3", "4", "5", "6")]
        public void IsCodeComplete_WithMissingFirstDigit_ReturnsFalse(string? code1, string code2, 
            string code3, string code4, string code5, string code6)
        {
            // Act
            var result = TwoFactorValidator.IsCodeComplete(code1!, code2, code3, code4, code5, code6);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar código incompleto - falta último dígito")]
        [InlineData("1", "2", "3", "4", "5", "")]
        [InlineData("1", "2", "3", "4", "5", null)]
        [InlineData("1", "2", "3", "4", "5", "   ")]
        public void IsCodeComplete_WithMissingLastDigit_ReturnsFalse(string code1, string code2, 
            string code3, string code4, string code5, string? code6)
        {
            // Act
            var result = TwoFactorValidator.IsCodeComplete(code1, code2, code3, code4, code5, code6!);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar código incompleto - falta dígito del medio")]
        [InlineData("1", "2", "", "4", "5", "6")]
        [InlineData("1", "2", null, "4", "5", "6")]
        [InlineData("1", "", "3", "4", "5", "6")]
        public void IsCodeComplete_WithMissingMiddleDigit_ReturnsFalse(string code1, string? code2, 
            string? code3, string code4, string code5, string code6)
        {
            // Act
            var result = TwoFactorValidator.IsCodeComplete(code1, code2!, code3!, code4, code5, code6);

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "Debe rechazar cuando todos los dígitos están vacíos")]
        public void IsCodeComplete_WithAllDigitsEmpty_ReturnsFalse()
        {
            // Act
            var result = TwoFactorValidator.IsCodeComplete("", "", "", "", "", "");

            // Assert
            Assert.False(result);
        }

        #endregion

        #region CombineCode Tests

        [Fact(DisplayName = "Debe combinar correctamente los 6 dígitos")]
        public void CombineCode_WithSixDigits_ReturnsCombinedString()
        {
            // Act
            var result = TwoFactorValidator.CombineCode("1", "2", "3", "4", "5", "6");

            // Assert
            Assert.Equal("123456", result);
        }

        [Fact(DisplayName = "Debe combinar correctamente dígitos con ceros")]
        public void CombineCode_WithZeros_ReturnsCombinedString()
        {
            // Act
            var result = TwoFactorValidator.CombineCode("0", "0", "0", "0", "0", "0");

            // Assert
            Assert.Equal("000000", result);
        }

        [Fact(DisplayName = "Debe combinar correctamente dígitos mixtos")]
        public void CombineCode_WithMixedDigits_ReturnsCombinedString()
        {
            // Act
            var result = TwoFactorValidator.CombineCode("9", "8", "7", "6", "5", "4");

            // Assert
            Assert.Equal("987654", result);
        }

        [Fact(DisplayName = "Debe combinar incluso con caracteres no numéricos")]
        public void CombineCode_WithNonNumericCharacters_ReturnsCombinedString()
        {
            // Act
            var result = TwoFactorValidator.CombineCode("a", "b", "c", "1", "2", "3");

            // Assert
            Assert.Equal("abc123", result);
        }

        #endregion

        #region IsCodeNumeric Tests

        [Fact(DisplayName = "Debe validar correctamente código numérico completo")]
        public void IsCodeNumeric_WithAllNumericDigits_ReturnsTrue()
        {
            // Act
            var result = TwoFactorValidator.IsCodeNumeric("1", "2", "3", "4", "5", "6");

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Debe validar correctamente código con ceros")]
        public void IsCodeNumeric_WithZeros_ReturnsTrue()
        {
            // Act
            var result = TwoFactorValidator.IsCodeNumeric("0", "0", "0", "0", "0", "0");

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe rechazar código con letra en primer posición")]
        [InlineData("a", "2", "3", "4", "5", "6")]
        [InlineData("x", "2", "3", "4", "5", "6")]
        public void IsCodeNumeric_WithLetterInFirstPosition_ReturnsFalse(string code1, string code2, 
            string code3, string code4, string code5, string code6)
        {
            // Act
            var result = TwoFactorValidator.IsCodeNumeric(code1, code2, code3, code4, code5, code6);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar código con letra en última posición")]
        [InlineData("1", "2", "3", "4", "5", "a")]
        [InlineData("1", "2", "3", "4", "5", "z")]
        public void IsCodeNumeric_WithLetterInLastPosition_ReturnsFalse(string code1, string code2, 
            string code3, string code4, string code5, string code6)
        {
            // Act
            var result = TwoFactorValidator.IsCodeNumeric(code1, code2, code3, code4, code5, code6);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar código con carácter especial")]
        [InlineData("1", "@", "3", "4", "5", "6")]
        [InlineData("1", "2", "#", "4", "5", "6")]
        [InlineData("1", "2", "3", "!", "5", "6")]
        public void IsCodeNumeric_WithSpecialCharacter_ReturnsFalse(string code1, string code2, 
            string code3, string code4, string code5, string code6)
        {
            // Act
            var result = TwoFactorValidator.IsCodeNumeric(code1, code2, code3, code4, code5, code6);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region IsDigit Tests

        [Theory(DisplayName = "Debe validar correctamente dígitos únicos")]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("5")]
        [InlineData("9")]
        public void IsDigit_WithSingleDigit_ReturnsTrue(string digit)
        {
            // Act
            var result = TwoFactorValidator.IsDigit(digit);

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe rechazar letras")]
        [InlineData("a")]
        [InlineData("z")]
        [InlineData("A")]
        [InlineData("Z")]
        public void IsDigit_WithLetter_ReturnsFalse(string letter)
        {
            // Act
            var result = TwoFactorValidator.IsDigit(letter);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar caracteres especiales")]
        [InlineData("@")]
        [InlineData("#")]
        [InlineData("!")]
        [InlineData("-")]
        public void IsDigit_WithSpecialCharacter_ReturnsFalse(string specialChar)
        {
            // Act
            var result = TwoFactorValidator.IsDigit(specialChar);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar múltiples caracteres")]
        [InlineData("12")]
        [InlineData("123")]
        [InlineData("ab")]
        public void IsDigit_WithMultipleCharacters_ReturnsFalse(string multipleChars)
        {
            // Act
            var result = TwoFactorValidator.IsDigit(multipleChars);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar valores vacíos o nulos")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void IsDigit_WithEmptyOrNull_ReturnsFalse(string? empty)
        {
            // Act
            var result = TwoFactorValidator.IsDigit(empty);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar espacios")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void IsDigit_WithSpaces_ReturnsFalse(string spaces)
        {
            // Act
            var result = TwoFactorValidator.IsDigit(spaces);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region GetCodeIncompleteErrorMessage Tests

        [Fact(DisplayName = "Debe retornar mensaje de error para código incompleto")]
        public void GetCodeIncompleteErrorMessage_ReturnsCorrectMessage()
        {
            // Act
            var message = TwoFactorValidator.GetCodeIncompleteErrorMessage();

            // Assert
            Assert.NotEmpty(message);
            Assert.Contains("6", message);
            Assert.Contains("dígitos", message.ToLower());
        }

        #endregion

        #region Integration Tests

        [Fact(DisplayName = "Debe validar flujo completo: código válido")]
        public void CompleteFlow_WithValidCode_Success()
        {
            // Arrange
            string code1 = "1", code2 = "2", code3 = "3", code4 = "4", code5 = "5", code6 = "6";

            // Act
            bool isComplete = TwoFactorValidator.IsCodeComplete(code1, code2, code3, code4, code5, code6);
            bool isNumeric = TwoFactorValidator.IsCodeNumeric(code1, code2, code3, code4, code5, code6);
            string combined = TwoFactorValidator.CombineCode(code1, code2, code3, code4, code5, code6);

            // Assert
            Assert.True(isComplete, "El código debería estar completo");
            Assert.True(isNumeric, "El código debería ser numérico");
            Assert.Equal("123456", combined);
        }

        [Fact(DisplayName = "Debe validar flujo completo: código incompleto")]
        public void CompleteFlow_WithIncompleteCode_Fails()
        {
            // Arrange
            string code1 = "1", code2 = "2", code3 = "", code4 = "4", code5 = "5", code6 = "6";

            // Act
            bool isComplete = TwoFactorValidator.IsCodeComplete(code1, code2, code3, code4, code5, code6);

            // Assert
            Assert.False(isComplete, "El código no debería estar completo");
        }

        [Fact(DisplayName = "Debe validar flujo completo: código no numérico")]
        public void CompleteFlow_WithNonNumericCode_Fails()
        {
            // Arrange
            string code1 = "1", code2 = "2", code3 = "a", code4 = "4", code5 = "5", code6 = "6";

            // Act
            bool isComplete = TwoFactorValidator.IsCodeComplete(code1, code2, code3, code4, code5, code6);
            bool isNumeric = TwoFactorValidator.IsCodeNumeric(code1, code2, code3, code4, code5, code6);

            // Assert
            Assert.True(isComplete, "El código está completo pero...");
            Assert.False(isNumeric, "...no es numérico");
        }

        #endregion

        #region Edge Cases Tests

        [Fact(DisplayName = "Debe manejar todos los dígitos como espacios en blanco")]
        public void IsCodeComplete_WithAllWhitespace_ReturnsFalse()
        {
            // Act
            var result = TwoFactorValidator.IsCodeComplete("   ", "   ", "   ", "   ", "   ", "   ");

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "Debe combinar correctamente dígitos repetidos")]
        public void CombineCode_WithRepeatedDigits_ReturnsCombinedString()
        {
            // Act
            var result = TwoFactorValidator.CombineCode("1", "1", "1", "1", "1", "1");

            // Assert
            Assert.Equal("111111", result);
        }

        #endregion
    }
}
