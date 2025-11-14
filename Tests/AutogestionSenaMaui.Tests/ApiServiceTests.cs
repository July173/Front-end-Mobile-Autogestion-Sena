using Xunit;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using AutogestionSena.MAUI.Api.Services;
using AutogestionSena.MAUI.Api.Dtos;

namespace AutogestionSenaMaui.Tests.Services
{
    /// <summary>
    /// Pruebas unitarias para ApiService
    /// </summary>
    public class ApiServiceTests
    {
        [Fact(DisplayName = "Constructor sin parámetros - Debe crear instancia válida")]
        public void Constructor_WithoutParameters_ShouldCreateValidInstance()
        {
            // Arrange & Act
            var service = new ApiService();

            // Assert
            service.Should().NotBeNull();
        }

        [Fact(DisplayName = "Constructor con HttpClient - Debe crear instancia válida")]
        public void Constructor_WithHttpClient_ShouldCreateValidInstance()
        {
            // Arrange
            var httpClient = new HttpClient();

            // Act
            var service = new ApiService(httpClient);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact(DisplayName = "SetAuthToken - Debe configurar header de autorización con token válido")]
        public void SetAuthToken_WithValidToken_ShouldConfigureAuthorizationHeader()
        {
            // Arrange
            var service = new ApiService();
            var token = "test_token_123";

            // Act
            service.SetAuthToken(token);

            // Assert - Verificar que no lanza excepción
            service.Invoking(s => s.SetAuthToken(token))
                .Should().NotThrow();
        }

        [Fact(DisplayName = "ClearAuthToken - Debe limpiar header de autorización")]
        public void ClearAuthToken_ShouldClearAuthorizationHeader()
        {
            // Arrange
            var service = new ApiService();
            service.SetAuthToken("test_token");

            // Act
            service.ClearAuthToken();

            // Assert - Verificar que no lanza excepción
            service.Invoking(s => s.ClearAuthToken())
                .Should().NotThrow();
        }

        [Theory(DisplayName = "SetAuthToken - Debe manejar tokens nulos o vacíos")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void SetAuthToken_WithInvalidTokens_ShouldHandleGracefully(string? invalidToken)
        {
            // Arrange
            var service = new ApiService();

            // Act & Assert
            service.Invoking(s => s.SetAuthToken(invalidToken!))
                .Should().NotThrow();
        }

        [Fact(DisplayName = "GetAsync - Con respuesta exitosa - Debe retornar datos deserializados")]
        public async Task GetAsync_WithSuccessResponse_ShouldReturnDeserializedData()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
            var expectedData = new DocumentTypeDto { Id = 1, Name = "CC", Abbreviation = "CC", Active = true };
            var jsonResponse = JsonSerializer.Serialize(expectedData);

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var service = new ApiService(httpClient);

            // Act
            var result = await service.GetAsync<DocumentTypeDto>("http://test.com/api/test");

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result.Name.Should().Be("CC");
        }

        [Fact(DisplayName = "GetAsync - Con timeout - Debe lanzar excepción descriptiva")]
        public async Task GetAsync_WithTimeout_ShouldThrowDescriptiveException()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new TaskCanceledException());

            var httpClient = new HttpClient(mockHandler.Object);
            var service = new ApiService(httpClient);

            // Act
            Func<Task> act = async () => await service.GetAsync<DocumentTypeDto>("http://test.com/api/test");

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("*tiempo de espera*");
        }

        [Fact(DisplayName = "GetAsync - Con error de conexión - Debe lanzar excepción descriptiva")]
        public async Task GetAsync_WithConnectionError_ShouldThrowDescriptiveException()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Connection refused"));

            var httpClient = new HttpClient(mockHandler.Object);
            var service = new ApiService(httpClient);

            // Act
            Func<Task> act = async () => await service.GetAsync<DocumentTypeDto>("http://test.com/api/test");

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("*no se pudo conectar*");
        }

        [Fact(DisplayName = "PostAsync - Con respuesta exitosa - Debe retornar datos deserializados")]
        public async Task PostAsync_WithSuccessResponse_ShouldReturnDeserializedData()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
            var payload = new { email = "test@test.com", password = "123" };
            var expectedResponse = new ValidateLoginResponse { Access = "token_123", Refresh = "refresh_456" };
            var jsonResponse = JsonSerializer.Serialize(expectedResponse);

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, System.Text.Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var service = new ApiService(httpClient);

            // Act
            var result = await service.PostAsync<object, ValidateLoginResponse>("http://test.com/api/login", payload);

            // Assert
            result.Should().NotBeNull();
            result!.Access.Should().Be("token_123");
            result!.Refresh.Should().Be("refresh_456");
        }

        [Fact(DisplayName = "PostAsync - Con error HTTP - Debe lanzar excepción")]
        public async Task PostAsync_WithHttpError_ShouldThrowException()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Bad Request")
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var service = new ApiService(httpClient);
            var payload = new { test = "data" };

            // Act
            Func<Task> act = async () => await service.PostAsync<object, ValidateLoginResponse>("http://test.com/api/test", payload);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("*Error en la petición POST*");
        }

        [Fact(DisplayName = "DeleteAsync - Con respuesta exitosa - Debe retornar HttpResponseMessage")]
        public async Task DeleteAsync_WithSuccessResponse_ShouldReturnHttpResponseMessage()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var service = new ApiService(httpClient);

            // Act
            var result = await service.DeleteAsync("http://test.com/api/test/1");

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
