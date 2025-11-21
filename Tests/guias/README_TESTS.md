# Manual de Ejecución de Pruebas Unitarias - Login Validations

## 📋 Descripción

Este proyecto contiene pruebas unitarias para la clase `LoginValidator`, enfocadas exclusivamente en la **lógica de validaciones** del login sin involucrar llamadas a la API ni componentes de UI.

**Total de Pruebas:** 57  
**Framework:** xUnit  
**Plataforma:** .NET 8.0

## 🧪 Pruebas Implementadas

### Categorías de Pruebas

1. **Validación de Username** (11 pruebas)
   - Usernames válidos (emails, nombres de usuario, strings simples)
   - Usernames inválidos (vacíos, null, espacios en blanco, tabs, newlines)

2. **Validación de Password** (11 pruebas)
   - Passwords válidos de diferentes formatos
   - Passwords inválidos (vacíos, null, espacios en blanco, tabs, newlines)

3. **Validación de Credenciales Combinadas** (11 pruebas)
   - Validación de username y password juntos
   - Casos donde uno o ambos son inválidos

4. **Mensajes de Error** (2 pruebas)
   - Verificación de mensajes de error correctos para username
   - Verificación de mensajes de error correctos para password

5. **Enrutamiento Basado en Roles** (9 pruebas)
   - Rutas correctas para cada rol (1-5)
   - Ruta por defecto para roles inválidos

6. **Validación de Código 2FA** (9 pruebas)
   - Códigos válidos de 6 dígitos
   - Códigos inválidos (longitud incorrecta, caracteres no numéricos, vacío, null)

7. **Casos de Borde** (4 pruebas)
   - Validaciones con solo espacios, tabs, contenido mixto

## 🚀 Requisitos Previos

- .NET 8.0 SDK instalado
- xUnit instalado (se instala automáticamente con restore)

## 📦 Instalación

Desde la carpeta raíz del proyecto:

```powershell
# Restaurar paquetes NuGet
dotnet restore Tests/AutogestionSena.Tests.csproj
```

## ▶️ Ejecución de Pruebas

### Ejecutar TODAS las pruebas

```powershell
dotnet test Tests/AutogestionSena.Tests.csproj
```

### Ejecutar pruebas con información detallada

```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --verbosity detailed
```

### Ejecutar una prueba específica por nombre

```powershell
# Sintaxis general
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~NombreDeLaPrueba"

# Ejemplos específicos:
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~Username_SetValidValue_PropertyChanges"

dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~Password_EmptyOrWhitespace_IsInvalid"

dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsBusy_InitialValue_IsFalse"

dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~LoginCommand_WhenIsBusyIsTrue_CannotExecute"
```

### Ejecutar pruebas por categoría

```powershell
# Solo pruebas de validación
dotnet test Tests/AutogestionSena.Tests.csproj --filter "Category=Validation"

# Solo pruebas de PropertyChanged
dotnet test Tests/AutogestionSena.Tests.csproj --filter "Category=PropertyChanged"
```

### Ejecutar pruebas que contengan una palabra en el nombre

```powershell
# Todas las pruebas relacionadas con Username
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~Username"

# Todas las pruebas relacionadas con Password
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~Password"

# Todas las pruebas relacionadas con IsBusy
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsBusy"

# Todas las pruebas de validación de credenciales
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~Credentials"
```

## 📊 Generar Reporte de Cobertura

```powershell
# Ejecutar pruebas con cobertura
dotnet test Tests/AutogestionSena.Tests.csproj /p:CollectCoverage=true

# Con formato detallado
dotnet test Tests/AutogestionSena.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## 📝 Lista de Pruebas Individual

### 1. Validación de Username (11 pruebas)

```powershell
# Todas las pruebas de username
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsUsernameValid"

# Solo usernames válidos
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsUsernameValid_WithValidUsername"

# Solo usernames inválidos
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsUsernameValid_WithInvalidUsername"
```

### 2. Validación de Password (11 pruebas)

```powershell
# Todas las pruebas de password
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsPasswordValid"

# Solo passwords válidos
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsPasswordValid_WithValidPassword"

# Solo passwords inválidos
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsPasswordValid_WithInvalidPassword"
```

### 3. Validación de Credenciales Combinadas (11 pruebas)

```powershell
# Todas las pruebas de credenciales combinadas
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~AreCredentialsValid"

# Solo credenciales válidas
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~AreCredentialsValid_WithBothValid"

# Solo credenciales inválidas
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~AreCredentialsValid_WithInvalidData"
```

### 4. Mensajes de Error (2 pruebas)

```powershell
# Mensaje de error de username
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~GetUsernameErrorMessage"

# Mensaje de error de password
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~GetPasswordErrorMessage"
```

### 5. Enrutamiento por Roles (9 pruebas)

```powershell
# Todas las pruebas de routing
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~GetRouteForRole"

# Solo rutas válidas
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~GetRouteForRole_WithValidRole"

# Solo rutas por defecto (roles inválidos)
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~GetRouteForRole_WithInvalidRole"
```

### 6. Validación de Código 2FA (9 pruebas)

```powershell
# Todas las pruebas de código 2FA
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~Is2FACodeValid"

# Solo códigos 2FA válidos
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~Is2FACodeValid_WithValidCode"

# Solo códigos 2FA inválidos
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~Is2FACodeValid_WithInvalidCode"
```

### 7. Casos de Borde (4 pruebas)

```powershell
# Todas las pruebas de casos edge
dotnet test Tests/AutogestionSena.Tests.csproj --filter "Category=EdgeCase"
```

## 🎯 Ejemplos de Uso Común

### Ver solo nombres de pruebas sin ejecutar

```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --list-tests
```

### Ejecutar y mostrar solo pruebas fallidas

```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --logger:"console;verbosity=minimal"
```

### Ejecutar en modo watch (re-ejecuta al cambiar código)

```powershell
dotnet watch test --project Tests/AutogestionSena.Tests.csproj
```

## ✅ Resultados Esperados

Todas las pruebas deberían pasar exitosamente. Un resultado exitoso se ve así:

```text
Resumen de pruebas: total: 57; con errores: 0; correcto: 57; omitido: 0
```

## 🐛 Solución de Problemas

### Error: "No se encuentra el proyecto"
```powershell
# Asegúrate de estar en la carpeta raíz del proyecto
cd C:\Users\braya\OneDrive\Desktop\Front-end-Mobile-Autogestion-Sena
```

### Error: "No se pueden restaurar paquetes"
```powershell
dotnet restore Tests/AutogestionSena.Tests.csproj --force
```

### Error: "No se encuentra xUnit"
```powershell
dotnet add Tests/AutogestionSena.Tests.csproj package xunit
dotnet add Tests/AutogestionSena.Tests.csproj package xunit.runner.visualstudio
```

## 📚 Recursos Adicionales

- [Documentación xUnit](https://xunit.net/)
- [Documentación dotnet test](https://docs.microsoft.com/es-es/dotnet/core/tools/dotnet-test)
- [Filtros de pruebas](https://docs.microsoft.com/es-es/dotnet/core/testing/selective-unit-tests)

---

**Fecha de creación:** 21 de noviembre de 2025
**Versión:** 1.0
**Autor:** Sistema de Pruebas Unitarias
