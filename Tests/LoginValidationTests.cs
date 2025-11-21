using Xunit;
using AutogestionSena.MAUI.Validators;

namespace AutogestionSena.Tests
{
    /// <summary>
    /// Pruebas unitarias para validaciones de la lógica del Login
    /// Solo se prueban validaciones de entrada, sin llamadas a API ni UI
    /// </summary>
    public class LoginValidationTests
    {
        #region Username Validation Tests

        [Theory]
        [InlineData("usuario@test.com")]
        [InlineData("user123")]
        [InlineData("test.user@domain.com")]
        [InlineData("admin")]
        [InlineData("a")]
        [Trait("Category", "Validation")]
        public void IsUsernameValid_WithValidUsername_ReturnsTrue(string validUsername)
        {
            // Act
            var result = LoginValidator.IsUsernameValid(validUsername);

            // Assert
            Assert.True(result, $"Username '{validUsername}' debería ser válido");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData(null)]
        [Trait("Category", "Validation")]
        public void IsUsernameValid_WithInvalidUsername_ReturnsFalse(string? invalidUsername)
        {
            // Act
            var result = LoginValidator.IsUsernameValid(invalidUsername);

            // Assert
            Assert.False(result, $"Username '{invalidUsername}' debería ser inválido");
        }

        #endregion

        #region Password Validation Tests

        [Theory]
        [InlineData("Password123!")]
        [InlineData("abc123")]
        [InlineData("MiContraseña2024")]
        [InlineData("P@ssw0rd")]
        [InlineData("a")]
        [Trait("Category", "Validation")]
        public void IsPasswordValid_WithValidPassword_ReturnsTrue(string validPassword)
        {
            // Act
            var result = LoginValidator.IsPasswordValid(validPassword);

            // Assert
            Assert.True(result, $"Password '{validPassword}' debería ser válido");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData(null)]
        [Trait("Category", "Validation")]
        public void IsPasswordValid_WithInvalidPassword_ReturnsFalse(string? invalidPassword)
        {
            // Act
            var result = LoginValidator.IsPasswordValid(invalidPassword);

            // Assert
            Assert.False(result, $"Password '{invalidPassword}' debería ser inválido");
        }

        #endregion

        #region Combined Credentials Validation Tests

        [Theory]
        [InlineData("user@test.com", "Password123!")]
        [InlineData("admin", "pass")]
        [InlineData("test123", "12345")]
        [Trait("Category", "Validation")]
        public void AreCredentialsValid_WithBothValid_ReturnsTrue(string username, string password)
        {
            // Act
            var result = LoginValidator.AreCredentialsValid(username, password);

            // Assert
            Assert.True(result, $"Credenciales ('{username}', '{password}') deberían ser válidas");
        }

        [Theory]
        [InlineData("user@test.com", "Password123!")]
        [InlineData("user@test.com", "")]
        [InlineData("", "")]
        [InlineData("   ", "password")]
        [InlineData("user", "   ")]
        [InlineData(null, "password")]
        [InlineData("user", null)]
        [InlineData(null, null)]
        [Trait("Category", "Validation")]
        public void AreCredentialsValid_WithInvalidData_ReturnsFalse(string? username, string? password)
        {
            // Act
            var result = LoginValidator.AreCredentialsValid(username, password);

            // Assert
            Assert.False(result, $"Credenciales ('{username}', '{password}') deberían ser inválidas");
        }

        #endregion

        #region Error Messages Tests

        [Fact]
        [Trait("Category", "Validation")]
        public void GetUsernameErrorMessage_ReturnsCorrectMessage()
        {
            // Act
            var message = LoginValidator.GetUsernameErrorMessage();

            // Assert
            Assert.Equal("Ingresa tu usuario", message);
            Assert.NotEmpty(message);
        }

        [Fact]
        [Trait("Category", "Validation")]
        public void GetPasswordErrorMessage_ReturnsCorrectMessage()
        {
            // Act
            var message = LoginValidator.GetPasswordErrorMessage();

            // Assert
            Assert.Equal("Ingresa tu contraseña", message);
            Assert.NotEmpty(message);
        }

        #endregion

        #region Role-Based Routing Tests

        [Theory]
        [InlineData(1, "SecurityMainPage")]
        [InlineData(2, "ApprenticeDashboard")]
        [InlineData(3, "InstructorDashboard")]
        [InlineData(4, "CoordinatorDashboard")]
        [InlineData(5, "SofiaOperatorDashboard")]
        [Trait("Category", "Routing")]
        public void GetRouteForRole_WithValidRole_ReturnsCorrectRoute(int roleId, string expectedRoute)
        {
            // Act
            var route = LoginValidator.GetRouteForRole(roleId);

            // Assert
            Assert.Equal(expectedRoute, route);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        [InlineData(99)]
        [InlineData(-1)]
        [Trait("Category", "Routing")]
        public void GetRouteForRole_WithInvalidRole_ReturnsHomePage(int invalidRoleId)
        {
            // Act
            var route = LoginValidator.GetRouteForRole(invalidRoleId);

            // Assert
            Assert.Equal("HomePage", route);
        }

        #endregion

        #region 2FA Code Validation Tests

        [Theory]
        [InlineData("123456")]
        [InlineData("000000")]
        [InlineData("999999")]
        [Trait("Category", "Validation")]
        public void Is2FACodeValid_WithValidCode_ReturnsTrue(string validCode)
        {
            // Act
            var result = LoginValidator.Is2FACodeValid(validCode);

            // Assert
            Assert.True(result, $"Código 2FA '{validCode}' debería ser válido");
        }

        [Theory]
        [InlineData("12345")]       // 5 dígitos
        [InlineData("1234567")]     // 7 dígitos
        [InlineData("12345a")]      // contiene letra
        [InlineData("123 456")]     // contiene espacio
        [InlineData("")]            // vacío
        [InlineData("   ")]         // espacios
        [InlineData(null)]          // null
        [InlineData("abcdef")]      // solo letras
        [Trait("Category", "Validation")]
        public void Is2FACodeValid_WithInvalidCode_ReturnsFalse(string? invalidCode)
        {
            // Act
            var result = LoginValidator.Is2FACodeValid(invalidCode);

            // Assert
            Assert.False(result, $"Código 2FA '{invalidCode}' debería ser inválido");
        }

        #endregion

        #region Edge Cases Tests

        [Fact]
        [Trait("Category", "EdgeCase")]
        public void IsUsernameValid_WithOnlySpaces_ReturnsFalse()
        {
            // Act
            var result = LoginValidator.IsUsernameValid("     ");

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Category", "EdgeCase")]
        public void IsPasswordValid_WithOnlyTabs_ReturnsFalse()
        {
            // Act
            var result = LoginValidator.IsPasswordValid("\t\t\t");

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Category", "EdgeCase")]
        public void AreCredentialsValid_WithWhitespaceUsername_ReturnsFalse()
        {
            // Act
            var result = LoginValidator.AreCredentialsValid("   \t\n   ", "password123");

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("Category", "EdgeCase")]
        public void Is2FACodeValid_WithMixedContent_ReturnsFalse()
        {
            // Act
            var result = LoginValidator.Is2FACodeValid("12A456");

            // Assert
            Assert.False(result);
        }

        #endregion
    }
}
