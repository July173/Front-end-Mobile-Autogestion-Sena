using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutogestionSena.MAUI.Api.Services;
using AutogestionSena.MAUI.Api.Dtos;

namespace AutogestionSenaMaui.Tests.Services
{
    /// <summary>
    /// Pruebas unitarias para UserService
    /// Estas pruebas verifican la lógica del servicio sin mockear ApiService
    /// ya que ApiService no tiene métodos virtuales.
    /// </summary>
    public class UserServiceTests
    {

        [Fact(DisplayName = "Constructor sin parámetros - Debe crear instancia válida")]
        public void Constructor_WithoutParameters_ShouldCreateValidInstance()
        {
            // Arrange & Act
            var service = new UserService();

            // Assert
            service.Should().NotBeNull();
        }

        [Fact(DisplayName = "Constructor con ApiService - Debe crear instancia válida")]
        public void Constructor_WithApiService_ShouldCreateValidInstance()
        {
            // Arrange
            var apiService = new ApiService();

            // Act
            var service = new UserService(apiService);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact(DisplayName = "SetAuthToken - No debe lanzar excepción con token válido")]
        public void SetAuthToken_WithValidToken_ShouldNotThrow()
        {
            // Arrange
            var service = new UserService();
            var token = "test_token_123";

            // Act
            Action act = () => service.SetAuthToken(token);

            // Assert
            act.Should().NotThrow();
        }

        [Fact(DisplayName = "ClearAuthToken - No debe lanzar excepción")]
        public void ClearAuthToken_ShouldNotThrow()
        {
            // Arrange
            var service = new UserService();

            // Act
            Action act = () => service.ClearAuthToken();

            // Assert
            act.Should().NotThrow();
        }

        [Theory(DisplayName = "SetAuthToken - Debe manejar diferentes tipos de tokens")]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...")]
        [InlineData("simple_token_123")]
        [InlineData("")]
        public void SetAuthToken_WithDifferentTokens_ShouldNotThrow(string token)
        {
            // Arrange
            var service = new UserService();

            // Act
            Action act = () => service.SetAuthToken(token);

            // Assert
            act.Should().NotThrow();
        }

        [Fact(DisplayName = "UserService - Debe poder configurar y limpiar tokens múltiples veces")]
        public void UserService_ShouldConfigureAndClearTokensMultipleTimes()
        {
            // Arrange
            var service = new UserService();

            // Act & Assert
            service.Invoking(s => {
                s.SetAuthToken("token1");
                s.ClearAuthToken();
                s.SetAuthToken("token2");
                s.ClearAuthToken();
                s.SetAuthToken("token3");
            }).Should().NotThrow();
        }

        [Fact(DisplayName = "RegisterPayloadDto - Debe crear instancia con datos válidos")]
        public void RegisterPayloadDto_ShouldCreateValidInstance()
        {
            // Arrange & Act
            var payload = new RegisterPayloadDto
            {
                Email = "nuevo@soy.sena.edu.co",
                FirstName = "Juan",
                FirstLastName = "Pérez",
                TypeIdentification = 1,
                NumberIdentification = 1234567890,
                PhoneNumber = 300123456
            };

            // Assert
            payload.Should().NotBeNull();
            payload.Email.Should().Be("nuevo@soy.sena.edu.co");
            payload.FirstName.Should().Be("Juan");
            payload.FirstLastName.Should().Be("Pérez");
            payload.TypeIdentification.Should().Be(1);
            payload.NumberIdentification.Should().Be(1234567890);
            payload.PhoneNumber.Should().Be(300123456);
        }

        [Fact(DisplayName = "SecondFactorRequest - Debe crear instancia con datos válidos")]
        public void SecondFactorRequest_ShouldCreateValidInstance()
        {
            // Arrange & Act
            var request = new SecondFactorRequest
            {
                Email = "test@soy.sena.edu.co",
                Code = "123456"
            };

            // Assert
            request.Should().NotBeNull();
            request.Email.Should().Be("test@soy.sena.edu.co");
            request.Code.Should().Be("123456");
        }
    }
}
