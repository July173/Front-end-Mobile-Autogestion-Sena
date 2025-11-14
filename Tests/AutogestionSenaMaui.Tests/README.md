# 🧪 Pruebas Unitarias - AutoGestión SENA MAUI

Este proyecto contiene las pruebas unitarias para la aplicación móvil AutoGestión SENA.

## 📋 Estructura

```
Tests/AutogestionSenaMaui.Tests/
├── AutogestionSenaMaui.Tests.csproj  # Proyecto de pruebas
├── ApiServiceTests.cs                 # Pruebas para ApiService
├── UserServiceTests.cs                # Pruebas para UserService
└── README.md                          # Esta documentación
```

## 🛠️ Frameworks y Herramientas

- **xUnit** - Framework de pruebas unitarias
- **Moq** - Framework para crear objetos mock
- **FluentAssertions** - Librería para aserciones más legibles
- **.NET 8.0** - Framework de desarrollo

## 📦 Paquetes Instalados

```xml
<PackageReference Include="xunit" Version="2.6.2" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.5.4" />
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
<PackageReference Include="Moq" Version="4.20.70" />
<PackageReference Include="FluentAssertions" Version="6.12.0" />
<PackageReference Include="coverlet.collector" Version="6.0.0" />
```

## 🚀 Ejecutar Pruebas

### Desde la terminal PowerShell:

```powershell
# Navegar al directorio del proyecto de pruebas
cd Tests\AutogestionSenaMaui.Tests

# Ejecutar todas las pruebas
dotnet test

# Ejecutar con más detalle
dotnet test --verbosity normal

# Ejecutar con cobertura de código
dotnet test --collect:"XPlat Code Coverage"
```

### Desde Visual Studio Code:

1. Instala la extensión ".NET Core Test Explorer"
2. Abre el panel de pruebas (Testing)
3. Ejecuta las pruebas desde la interfaz gráfica

### Desde Visual Studio:

1. Abre el Test Explorer (Test > Test Explorer)
2. Haz clic en "Run All" para ejecutar todas las pruebas
3. O selecciona pruebas individuales y ejecuta

## 📊 Cobertura de Pruebas

### ApiServiceTests.cs

✅ **Constructor sin parámetros** - Verifica creación de instancia  
✅ **Constructor con HttpClient** - Verifica inyección de dependencias  
✅ **SetAuthToken con token válido** - Configura header de autorización  
✅ **SetAuthToken con tokens inválidos** - Maneja null, vacío y espacios  
✅ **ClearAuthToken** - Limpia header de autorización  
✅ **GetAsync con respuesta exitosa** - Deserializa correctamente  
✅ **GetAsync con timeout** - Lanza excepción descriptiva  
✅ **GetAsync con error de conexión** - Maneja errores de red  
✅ **PostAsync con respuesta exitosa** - Envía y deserializa datos  
✅ **PostAsync con error HTTP** - Maneja errores de servidor  
✅ **DeleteAsync con respuesta exitosa** - Retorna HttpResponseMessage  

**Total: 11 pruebas para ApiService**

---

### UserServiceTests.cs

✅ **Constructor sin parámetros** - Verifica creación de instancia  
✅ **Constructor con ApiService** - Verifica inyección de dependencias  
✅ **GetDocumentTypesAsync** - Retorna lista de tipos de documento  
✅ **GetDocumentTypesAsync con lista vacía** - Maneja respuestas vacías  
✅ **ValidateLoginAsync con credenciales válidas** - Login exitoso  
✅ **ValidateLoginAsync verifica payload** - Envía datos correctos  
✅ **ValidateSecondFactorAsync con código válido** - Retorna tokens  
✅ **ValidateSecondFactorAsync con código inválido** - Retorna null  
✅ **RegisterApprenticeAsync con datos válidos** - Registro exitoso  
✅ **RequestPasswordResetAsync** - Ejecuta sin errores  
✅ **SetAuthToken** - Llama a ApiService correctamente  
✅ **ClearAuthToken** - Limpia token correctamente  
✅ **ValidateLoginAsync con diferentes emails** - Valida formatos  

**Total: 13 pruebas para UserService**

---

## 🎯 Tipos de Pruebas

### 1. Pruebas de Constructor
Verifican que los objetos se creen correctamente con y sin parámetros.

### 2. Pruebas de Métodos HTTP
- GET - Obtención de datos
- POST - Envío de datos
- DELETE - Eliminación de recursos

### 3. Pruebas de Manejo de Errores
- Timeout
- Errores de conexión
- Errores HTTP (4xx, 5xx)

### 4. Pruebas de Autenticación
- Configuración de tokens
- Limpieza de tokens
- Validación de login
- Validación de 2FA

### 5. Pruebas de Validación de Datos
- Emails válidos
- Códigos 2FA
- Payloads de registro

## 📖 Ejemplos de Uso

### Estructura de una prueba con xUnit:

```csharp
[Fact(DisplayName = "Descripción de la prueba")]
public void NombreDelMetodo_Escenario_ResultadoEsperado()
{
    // Arrange - Configuración
    var servicio = new MiServicio();
    
    // Act - Ejecución
    var resultado = servicio.MiMetodo();
    
    // Assert - Verificación
    resultado.Should().NotBeNull();
}
```

### Prueba con datos parametrizados:

```csharp
[Theory(DisplayName = "Descripción")]
[InlineData("dato1")]
[InlineData("dato2")]
[InlineData("dato3")]
public void MiMetodo_ConDiferentesDatos_DebeComportarseBien(string dato)
{
    // Arrange, Act, Assert...
}
```

### Prueba asíncrona:

```csharp
[Fact(DisplayName = "Descripción")]
public async Task MiMetodoAsync_Escenario_ResultadoEsperado()
{
    // Arrange
    var servicio = new MiServicio();
    
    // Act
    var resultado = await servicio.MiMetodoAsync();
    
    // Assert
    resultado.Should().NotBeNull();
}
```

### Uso de Moq para mocks:

```csharp
// Crear mock
var mockService = new Mock<IApiService>();

// Configurar comportamiento
mockService
    .Setup(x => x.GetAsync<Data>(It.IsAny<string>()))
    .ReturnsAsync(new Data { Id = 1 });

// Usar el mock
var userService = new UserService(mockService.Object);

// Verificar que se llamó
mockService.Verify(x => x.GetAsync<Data>(It.IsAny<string>()), Times.Once);
```

## 🐛 Troubleshooting

### Error: "No se pudo cargar el proyecto"
```powershell
dotnet restore
```

### Error: "Paquete no encontrado"
```powershell
dotnet restore --force
```

### Error: "Referencia de proyecto inválida"
Verifica que la ruta en el .csproj sea correcta:
```xml
<ProjectReference Include="..\..\AutogestionSenaMaui.csproj" />
```

## 📝 Convenciones

### Nomenclatura de pruebas:
```
NombreDelMetodo_Escenario_ResultadoEsperado
```

Ejemplos:
- `GetDocumentTypesAsync_ShouldReturnDocumentTypesList`
- `ValidateLoginAsync_WithValidCredentials_ShouldReturnSuccessResponse`
- `SetAuthToken_WithInvalidTokens_ShouldHandleGracefully`

### Estructura AAA:
```csharp
// Arrange - Preparar datos y configuración
// Act - Ejecutar el método a probar
// Assert - Verificar resultados
```

## 🎯 Próximos Pasos

- [ ] Agregar pruebas de integración
- [ ] Configurar CI/CD con GitHub Actions
- [ ] Aumentar cobertura de código al 90%+
- [ ] Agregar pruebas para ViewModels
- [ ] Implementar pruebas de UI con SpecFlow

## 📚 Referencias

- [xUnit Documentation](https://xunit.net/)
- [Moq Quick Start](https://github.com/moq/moq4)
- [FluentAssertions Documentation](https://fluentassertions.com/)
- [.NET Testing Best Practices](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)

---

**Última actualización:** 14 de noviembre de 2025  
**Pruebas totales:** 24  
**Estado:** ✅ Todas las pruebas pasando
