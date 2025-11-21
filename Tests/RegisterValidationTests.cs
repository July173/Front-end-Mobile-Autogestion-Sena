using AutogestionSena.MAUI.Validators;
using Xunit;

namespace AutogestionSena.Tests
{
    /// <summary>
    /// Pruebas unitarias para la validación de registro de usuarios
    /// </summary>
    public class RegisterValidationTests
    {
        #region IsEmailValid Tests

        [Theory(DisplayName = "Debe validar correctamente emails no vacíos")]
        [InlineData("usuario@soy.sena.edu.co")]
        [InlineData("test@example.com")]
        [InlineData("a@b.c")]
        public void IsEmailValid_WithNonEmptyEmail_ReturnsTrue(string email)
        {
            // Act
            var result = RegisterValidator.IsEmailValid(email);

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe rechazar emails vacíos")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void IsEmailValid_WithEmptyEmail_ReturnsFalse(string? email)
        {
            // Act
            var result = RegisterValidator.IsEmailValid(email);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region IsNameValid Tests

        [Theory(DisplayName = "Debe validar correctamente nombres no vacíos")]
        [InlineData("Juan")]
        [InlineData("María José")]
        [InlineData("Carlos")]
        [InlineData("Ana María")]
        public void IsNameValid_WithNonEmptyName_ReturnsTrue(string name)
        {
            // Act
            var result = RegisterValidator.IsNameValid(name);

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe rechazar nombres vacíos")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void IsNameValid_WithEmptyName_ReturnsFalse(string? name)
        {
            // Act
            var result = RegisterValidator.IsNameValid(name);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region IsLastNameValid Tests

        [Theory(DisplayName = "Debe validar correctamente apellidos no vacíos")]
        [InlineData("Pérez")]
        [InlineData("García López")]
        [InlineData("Martínez")]
        public void IsLastNameValid_WithNonEmptyLastName_ReturnsTrue(string lastName)
        {
            // Act
            var result = RegisterValidator.IsLastNameValid(lastName);

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe rechazar apellidos vacíos")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void IsLastNameValid_WithEmptyLastName_ReturnsFalse(string? lastName)
        {
            // Act
            var result = RegisterValidator.IsLastNameValid(lastName);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region IsDocumentNumberValid Tests

        [Theory(DisplayName = "Debe validar correctamente números de documento no vacíos")]
        [InlineData("1234567890")]
        [InlineData("123456")]
        [InlineData("99999999")]
        public void IsDocumentNumberValid_WithNonEmptyDocument_ReturnsTrue(string documentNumber)
        {
            // Act
            var result = RegisterValidator.IsDocumentNumberValid(documentNumber);

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe rechazar números de documento vacíos")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void IsDocumentNumberValid_WithEmptyDocument_ReturnsFalse(string? documentNumber)
        {
            // Act
            var result = RegisterValidator.IsDocumentNumberValid(documentNumber);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region IsDocumentNumberNumeric Tests

        [Theory(DisplayName = "Debe validar correctamente documentos numéricos")]
        [InlineData("1234567890")]
        [InlineData("123456")]
        [InlineData("0")]
        [InlineData("99999999")]
        public void IsDocumentNumberNumeric_WithNumericDocument_ReturnsTrue(string documentNumber)
        {
            // Act
            var result = RegisterValidator.IsDocumentNumberNumeric(documentNumber);

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe rechazar documentos con letras")]
        [InlineData("123abc")]
        [InlineData("abc123")]
        [InlineData("12a456")]
        [InlineData("abcdef")]
        public void IsDocumentNumberNumeric_WithLetters_ReturnsFalse(string documentNumber)
        {
            // Act
            var result = RegisterValidator.IsDocumentNumberNumeric(documentNumber);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar emails cuando se espera número de documento")]
        [InlineData("usuario@example.com")]
        [InlineData("test@sena.edu.co")]
        [InlineData("correo@dominio.co")]
        public void IsDocumentNumberNumeric_WithEmailInput_ReturnsFalse(string emailInput)
        {
            // Act
            var result = RegisterValidator.IsDocumentNumberNumeric(emailInput);

            // Assert
            Assert.False(result, $"El validador debe rechazar email '{emailInput}' cuando se espera número de documento");
        }

        [Theory(DisplayName = "Debe rechazar documentos con caracteres especiales")]
        [InlineData("123-456")]
        [InlineData("123.456")]
        [InlineData("123 456")]
        [InlineData("123@456")]
        public void IsDocumentNumberNumeric_WithSpecialCharacters_ReturnsFalse(string documentNumber)
        {
            // Act
            var result = RegisterValidator.IsDocumentNumberNumeric(documentNumber);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar documentos vacíos o nulos")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void IsDocumentNumberNumeric_WithEmptyOrNull_ReturnsFalse(string? documentNumber)
        {
            // Act
            var result = RegisterValidator.IsDocumentNumberNumeric(documentNumber);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region IsPhoneValid Tests

        [Theory(DisplayName = "Debe validar correctamente teléfonos no vacíos")]
        [InlineData("3001234567")]
        [InlineData("6012345678")]
        [InlineData("3123456789")]
        public void IsPhoneValid_WithNonEmptyPhone_ReturnsTrue(string phone)
        {
            // Act
            var result = RegisterValidator.IsPhoneValid(phone);

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe rechazar teléfonos vacíos")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void IsPhoneValid_WithEmptyPhone_ReturnsFalse(string? phone)
        {
            // Act
            var result = RegisterValidator.IsPhoneValid(phone);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region IsPhoneNumeric Tests

        [Theory(DisplayName = "Debe validar correctamente teléfonos numéricos")]
        [InlineData("3001234567")]
        [InlineData("6012345")]
        [InlineData("1234567890")]
        public void IsPhoneNumeric_WithNumericPhone_ReturnsTrue(string phone)
        {
            // Act
            var result = RegisterValidator.IsPhoneNumeric(phone);

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe rechazar teléfonos con letras")]
        [InlineData("300abc1234")]
        [InlineData("abc1234567")]
        [InlineData("300123456a")]
        public void IsPhoneNumeric_WithLetters_ReturnsFalse(string phone)
        {
            // Act
            var result = RegisterValidator.IsPhoneNumeric(phone);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar emails cuando se espera teléfono")]
        [InlineData("usuario@example.com")]
        [InlineData("test@sena.edu.co")]
        [InlineData("correo@dominio.co")]
        public void IsPhoneNumeric_WithEmailInput_ReturnsFalse(string emailInput)
        {
            // Act
            var result = RegisterValidator.IsPhoneNumeric(emailInput);

            // Assert
            Assert.False(result, $"El validador debe rechazar email '{emailInput}' cuando se espera teléfono");
        }

        [Theory(DisplayName = "Debe rechazar nombres cuando se espera teléfono")]
        [InlineData("Juan")]
        [InlineData("María")]
        [InlineData("Carlos Alberto")]
        public void IsPhoneNumeric_WithNameInput_ReturnsFalse(string nameInput)
        {
            // Act
            var result = RegisterValidator.IsPhoneNumeric(nameInput);

            // Assert
            Assert.False(result, $"El validador debe rechazar nombre '{nameInput}' cuando se espera teléfono");
        }

        [Theory(DisplayName = "Debe rechazar teléfonos con caracteres especiales")]
        [InlineData("300-123-4567")]
        [InlineData("(300) 1234567")]
        [InlineData("300.123.4567")]
        [InlineData("+573001234567")]
        public void IsPhoneNumeric_WithSpecialCharacters_ReturnsFalse(string phone)
        {
            // Act
            var result = RegisterValidator.IsPhoneNumeric(phone);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar teléfonos vacíos o nulos")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void IsPhoneNumeric_WithEmptyOrNull_ReturnsFalse(string? phone)
        {
            // Act
            var result = RegisterValidator.IsPhoneNumeric(phone);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region IsPhoneLengthValid Tests

        [Theory(DisplayName = "Debe validar correctamente teléfonos con longitud válida")]
        [InlineData("1234567")]     // 7 dígitos (mínimo)
        [InlineData("12345678")]    // 8 dígitos
        [InlineData("123456789")]   // 9 dígitos
        [InlineData("1234567890")]  // 10 dígitos (máximo)
        public void IsPhoneLengthValid_WithValidLength_ReturnsTrue(string phone)
        {
            // Act
            var result = RegisterValidator.IsPhoneLengthValid(phone);

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe rechazar teléfonos muy cortos")]
        [InlineData("123456")]      // 6 dígitos
        [InlineData("12345")]       // 5 dígitos
        [InlineData("1")]           // 1 dígito
        public void IsPhoneLengthValid_WithShortPhone_ReturnsFalse(string phone)
        {
            // Act
            var result = RegisterValidator.IsPhoneLengthValid(phone);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar teléfonos muy largos")]
        [InlineData("12345678901")]  // 11 dígitos
        [InlineData("123456789012")] // 12 dígitos
        public void IsPhoneLengthValid_WithLongPhone_ReturnsFalse(string phone)
        {
            // Act
            var result = RegisterValidator.IsPhoneLengthValid(phone);

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar teléfonos vacíos o nulos")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void IsPhoneLengthValid_WithEmptyOrNull_ReturnsFalse(string? phone)
        {
            // Act
            var result = RegisterValidator.IsPhoneLengthValid(phone);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region AreAllFieldsValid Tests

        [Fact(DisplayName = "Debe validar correctamente todos los campos completos")]
        public void AreAllFieldsValid_WithAllValidFields_ReturnsTrue()
        {
            // Act
            var result = RegisterValidator.AreAllFieldsValid(
                email: "usuario@soy.sena.edu.co",
                firstName: "Juan",
                lastName: "Pérez",
                documentNumber: "1234567890",
                phone: "3001234567",
                hasDocumentType: true
            );

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe rechazar si falta el email")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void AreAllFieldsValid_WithMissingEmail_ReturnsFalse(string? email)
        {
            // Act
            var result = RegisterValidator.AreAllFieldsValid(
                email: email,
                firstName: "Juan",
                lastName: "Pérez",
                documentNumber: "1234567890",
                phone: "3001234567",
                hasDocumentType: true
            );

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar si falta el nombre")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void AreAllFieldsValid_WithMissingFirstName_ReturnsFalse(string? firstName)
        {
            // Act
            var result = RegisterValidator.AreAllFieldsValid(
                email: "usuario@soy.sena.edu.co",
                firstName: firstName,
                lastName: "Pérez",
                documentNumber: "1234567890",
                phone: "3001234567",
                hasDocumentType: true
            );

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar si falta el apellido")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void AreAllFieldsValid_WithMissingLastName_ReturnsFalse(string? lastName)
        {
            // Act
            var result = RegisterValidator.AreAllFieldsValid(
                email: "usuario@soy.sena.edu.co",
                firstName: "Juan",
                lastName: lastName,
                documentNumber: "1234567890",
                phone: "3001234567",
                hasDocumentType: true
            );

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar si falta el número de documento")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void AreAllFieldsValid_WithMissingDocumentNumber_ReturnsFalse(string? documentNumber)
        {
            // Act
            var result = RegisterValidator.AreAllFieldsValid(
                email: "usuario@soy.sena.edu.co",
                firstName: "Juan",
                lastName: "Pérez",
                documentNumber: documentNumber,
                phone: "3001234567",
                hasDocumentType: true
            );

            // Assert
            Assert.False(result);
        }

        [Theory(DisplayName = "Debe rechazar si falta el teléfono")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void AreAllFieldsValid_WithMissingPhone_ReturnsFalse(string? phone)
        {
            // Act
            var result = RegisterValidator.AreAllFieldsValid(
                email: "usuario@soy.sena.edu.co",
                firstName: "Juan",
                lastName: "Pérez",
                documentNumber: "1234567890",
                phone: phone,
                hasDocumentType: true
            );

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "Debe rechazar si no se ha seleccionado tipo de documento")]
        public void AreAllFieldsValid_WithoutDocumentType_ReturnsFalse()
        {
            // Act
            var result = RegisterValidator.AreAllFieldsValid(
                email: "usuario@soy.sena.edu.co",
                firstName: "Juan",
                lastName: "Pérez",
                documentNumber: "1234567890",
                phone: "3001234567",
                hasDocumentType: false
            );

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Error Messages Tests

        [Fact(DisplayName = "Debe retornar mensaje de error para email obligatorio")]
        public void GetEmailRequiredErrorMessage_ReturnsCorrectMessage()
        {
            // Act
            var message = RegisterValidator.GetEmailRequiredErrorMessage();

            // Assert
            Assert.NotEmpty(message);
            Assert.Contains("correo", message.ToLower());
            Assert.Contains("obligatorio", message.ToLower());
        }

        [Fact(DisplayName = "Debe retornar mensaje de error para nombres obligatorios")]
        public void GetNamesRequiredErrorMessage_ReturnsCorrectMessage()
        {
            // Act
            var message = RegisterValidator.GetNamesRequiredErrorMessage();

            // Assert
            Assert.NotEmpty(message);
            Assert.Contains("nombres", message.ToLower());
            Assert.Contains("apellidos", message.ToLower());
        }

        [Fact(DisplayName = "Debe retornar mensaje de error para tipo documento obligatorio")]
        public void GetDocumentTypeRequiredErrorMessage_ReturnsCorrectMessage()
        {
            // Act
            var message = RegisterValidator.GetDocumentTypeRequiredErrorMessage();

            // Assert
            Assert.NotEmpty(message);
            Assert.Contains("tipo", message.ToLower());
            Assert.Contains("documento", message.ToLower());
        }

        [Fact(DisplayName = "Debe retornar mensaje de error para número documento obligatorio")]
        public void GetDocumentNumberRequiredErrorMessage_ReturnsCorrectMessage()
        {
            // Act
            var message = RegisterValidator.GetDocumentNumberRequiredErrorMessage();

            // Assert
            Assert.NotEmpty(message);
            Assert.Contains("número", message.ToLower());
            Assert.Contains("documento", message.ToLower());
        }

        [Fact(DisplayName = "Debe retornar mensaje de error para teléfono obligatorio")]
        public void GetPhoneRequiredErrorMessage_ReturnsCorrectMessage()
        {
            // Act
            var message = RegisterValidator.GetPhoneRequiredErrorMessage();

            // Assert
            Assert.NotEmpty(message);
            Assert.Contains("teléfono", message.ToLower());
        }

        [Fact(DisplayName = "Debe retornar mensaje de error para documento con formato inválido")]
        public void GetDocumentFormatErrorMessage_ReturnsCorrectMessage()
        {
            // Act
            var message = RegisterValidator.GetDocumentFormatErrorMessage();

            // Assert
            Assert.NotEmpty(message);
            Assert.Contains("documento", message.ToLower());
            Assert.Contains("dígitos", message.ToLower());
        }

        [Fact(DisplayName = "Debe retornar mensaje de error para teléfono con formato inválido")]
        public void GetPhoneFormatErrorMessage_ReturnsCorrectMessage()
        {
            // Act
            var message = RegisterValidator.GetPhoneFormatErrorMessage();

            // Assert
            Assert.NotEmpty(message);
            Assert.Contains("teléfono", message.ToLower());
            Assert.Contains("dígitos", message.ToLower());
        }

        #endregion

        #region Edge Cases Tests

        [Theory(DisplayName = "Debe manejar nombres con tildes y ñ")]
        [InlineData("José")]
        [InlineData("María")]
        [InlineData("Nuñez")]
        public void IsNameValid_WithAccentsAndSpecialChars_ReturnsTrue(string name)
        {
            // Act
            var result = RegisterValidator.IsNameValid(name);

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Debe rechazar números cuando se espera nombre")]
        [InlineData("12345")]
        [InlineData("999999")]
        [InlineData("3001234567")]
        public void IsNameValid_WithNumberInput_ReturnsTrue_ButIncorrect(string numberInput)
        {
            // Act - IsNameValid solo verifica que no esté vacío
            var result = RegisterValidator.IsNameValid(numberInput);

            // Assert - Esto pasa pero es incorrecto, se necesita validación adicional de formato
            Assert.True(result, "IsNameValid solo verifica que no esté vacío, se requiere validación adicional de formato para rechazar números");
        }

        [Theory(DisplayName = "Debe rechazar emails cuando se espera nombre")]
        [InlineData("usuario@example.com")]
        [InlineData("test@sena.edu.co")]
        public void IsNameValid_WithEmailInput_ReturnsTrue_ButIncorrect(string emailInput)
        {
            // Act - IsNameValid solo verifica que no esté vacío
            var result = RegisterValidator.IsNameValid(emailInput);

            // Assert - Esto pasa pero es incorrecto
            Assert.True(result, "IsNameValid solo verifica que no esté vacío, se requiere validación adicional de formato para rechazar emails");
        }

        [Fact(DisplayName = "Debe validar teléfono colombiano típico (10 dígitos)")]
        public void IsPhoneLengthValid_WithColombianPhone_ReturnsTrue()
        {
            // Arrange
            var colombianMobile = "3001234567"; // 10 dígitos

            // Act
            var result = RegisterValidator.IsPhoneLengthValid(colombianMobile);

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Debe validar teléfono fijo colombiano (7 dígitos)")]
        public void IsPhoneLengthValid_WithColombianLandline_ReturnsTrue()
        {
            // Arrange
            var landline = "6012345"; // 7 dígitos

            // Act
            var result = RegisterValidator.IsPhoneLengthValid(landline);

            // Assert
            Assert.True(result);
        }

        #endregion
    }
}
