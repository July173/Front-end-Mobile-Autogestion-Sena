using AutogestionSena.MAUI.Validators;
using Xunit;

namespace AutogestionSena.Tests
{
    /// <summary>
    /// Pruebas unitarias b�sicas para validaci�n de registro
    /// </summary>
    public class RegisterValidationTests
    {
        [Theory]
        [InlineData("usuario@soy.sena.edu.co", true)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsEmailValid_ValidatesCorrectly(string? email, bool expected)
        {
            var result = RegisterValidator.IsEmailValid(email);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Juan", true)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsNameValid_ValidatesCorrectly(string? name, bool expected)
        {
            var result = RegisterValidator.IsNameValid(name);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("1234567890", true)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsDocumentNumberValid_ValidatesCorrectly(string? doc, bool expected)
        {
            var result = RegisterValidator.IsDocumentNumberValid(doc);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("usuario@soy.sena.edu.co", "Juan", "Perez", "1234567890", "3001234567", true, true)]
        [InlineData("", "Juan", "Perez", "1234567890", "3001234567", true, false)]
        [InlineData("usuario@soy.sena.edu.co", "", "Perez", "1234567890", "3001234567", true, false)]
        public void AreAllFieldsValid_ValidatesCorrectly(string email, string firstName, string lastName, string doc, string phone, bool hasDocumentType, bool expected)
        {
            var result = RegisterValidator.AreAllFieldsValid(email, firstName, lastName, doc, phone, hasDocumentType);
            Assert.Equal(expected, result);
        }
    }
}
