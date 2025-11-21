# Manual Completo de Ejecución de Pruebas Unitarias - Módulo de Autenticación

## 📋 Descripción General

Este proyecto contiene pruebas unitarias exhaustivas para todos los validadores del módulo de autenticación, enfocadas exclusivamente en la **lógica de validaciones** sin involucrar llamadas a la API ni componentes de UI.

**Total de Pruebas:** 381 (+36 nuevas) ⬆️  
**Framework:** xUnit  
**Plataforma:** .NET 8.0  
**Tiempo de Ejecución:** ~9 segundos

## 🧪 Archivos de Prueba y Cobertura

**🎉 ACTUALIZACIÓN:** Se agregaron 36 pruebas nuevas de validación cruzada para verificar que se rechazan datos del tipo incorrecto (números donde se esperan emails, emails donde se esperan números, etc.)

### 1. **LoginValidationTests.cs** (57 pruebas)
Validaciones para el inicio de sesión y autenticación.

**Categorías:**
- ✅ Validación de Username (11 tests)
- ✅ Validación de Password (11 tests)
- ✅ Credenciales Combinadas (11 tests)
- ✅ Mensajes de Error (2 tests)
- ✅ Enrutamiento por Roles (9 tests)
- ✅ Código 2FA (9 tests)
- ✅ Casos de Borde (4 tests)

### 2. **CodeVerificationValidationTests.cs** (31 pruebas)
Validaciones para la verificación de códigos de recuperación.

**Categorías:**
- ✅ Validación de Código No Vacío (8 tests)
- ✅ Código de 6 Dígitos (11 tests)
- ✅ Mensajes de Error (2 tests)
- ✅ Casos de Borde (10 tests)

### 3. **PasswordRecoveryValidationTests.cs** (36 pruebas)
Validaciones para la recuperación de contraseña por email.

**Categorías:**
- ✅ Validación de Email No Vacío (6 tests)
- ✅ Formato de Email (8 tests)
- ✅ Email Institucional SENA (8 tests)
- ✅ Mensajes de Error (2 tests)
- ✅ Casos de Borde (12 tests)

### 4. **PasswordResetValidationTests.cs** (65 pruebas)
Validaciones para el restablecimiento de contraseñas.

**Categorías:**
- ✅ Validación de Contraseña No Vacía (7 tests)
- ✅ Longitud Mínima (8 caracteres) (8 tests)
- ✅ Coincidencia de Contraseñas (10 tests)
- ✅ Fortaleza de Contraseña (12 tests)
- ✅ Mensajes de Error (3 tests)
- ✅ Constantes (1 test)
- ✅ Casos de Borde (24 tests)

### 5. **RegisterValidationTests.cs** (98 pruebas)
Validaciones para el registro de nuevos usuarios.

**Categorías:**
- ✅ Validación de Email (4 tests)
- ✅ Validación de Nombres (4 tests)
- ✅ Validación de Apellidos (4 tests)
- ✅ Número de Documento (8 tests)
- ✅ Documento Numérico (8 tests)
- ✅ Validación de Teléfono (4 tests)
- ✅ Teléfono Numérico (8 tests)
- ✅ Longitud de Teléfono (8 tests)
- ✅ Validación de Todos los Campos (20 tests)
- ✅ Mensajes de Error (7 tests)
- ✅ Casos de Borde (23 tests)

### 6. **TwoFactorValidationTests.cs** (58 pruebas)
Validaciones para autenticación de dos factores (modal de 6 dígitos).

**Categorías:**
- ✅ Código Completo (8 tests)
- ✅ Combinar Código (4 tests)
- ✅ Código Numérico (6 tests)
- ✅ Validación de Dígito Individual (12 tests)
- ✅ Mensajes de Error (1 test)
- ✅ Pruebas de Integración (3 tests)
- ✅ Casos de Borde (24 tests)

## 🚀 Ejecución de Pruebas

### Ejecutar TODAS las Pruebas

```powershell
cd "c:\Users\braya\OneDrive\Desktop\Front-end-Mobile-Autogestion-Sena"
dotnet test Tests/AutogestionSena.Tests.csproj
```

**Salida esperada:**
```
Resumen de pruebas: total: 345; con errores: 0; correcto: 345; omitido: 0
```

### Ejecutar Pruebas con Detalle Verbose

```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --verbosity detailed
```

### Ejecutar Pruebas de un Archivo Específico

#### Solo Login
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~LoginValidationTests"
```

#### Solo Verificación de Código
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~CodeVerificationValidationTests"
```

#### Solo Recuperación de Contraseña
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~PasswordRecoveryValidationTests"
```

#### Solo Restablecimiento de Contraseña
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~PasswordResetValidationTests"
```

#### Solo Registro
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~RegisterValidationTests"
```

#### Solo 2FA
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~TwoFactorValidationTests"
```

## 🎯 Ejecución por Funcionalidad

### Validaciones de Username
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsUsernameValid"
```

### Validaciones de Password
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsPasswordValid"
```

### Validaciones de Email
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsEmailValid"
```

### Validaciones de Email Institucional
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsInstitutionalEmail"
```

### Validaciones de Código de 6 Dígitos
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsCode6DigitsValid"
```

### Validaciones de Coincidencia de Contraseñas
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~DoPasswordsMatch"
```

### Validaciones de Fortaleza de Contraseña
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsPasswordStrong"
```

### Validaciones de Longitud de Contraseña
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsPasswordLengthValid"
```

### Validaciones de Documento
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsDocumentNumberValid"
```

### Validaciones de Teléfono
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsPhoneValid"
```

### Validaciones de Todos los Campos (Registro)
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~AreAllFieldsValid"
```

### Validaciones de Código 2FA Completo
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsCodeComplete"
```

### Enrutamiento por Roles
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~GetRouteForRole"
```

## 📊 Ejecución con Cobertura de Código

### Instalar herramienta de cobertura (solo primera vez)
```powershell
dotnet tool install --global dotnet-reportgenerator-globaltool
```

### Ejecutar con cobertura
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
```

### Generar reporte HTML de cobertura
```powershell
reportgenerator -reports:"Tests/coverage.cobertura.xml" -targetdir:"Tests/coveragereport" -reporttypes:Html
```

## 🔍 Ejecutar una Prueba Individual

Para ejecutar una prueba específica por su nombre:

```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "DisplayName~nombre de la prueba"
```

**Ejemplos:**

```powershell
# Ejecutar solo pruebas de username válido
dotnet test Tests/AutogestionSena.Tests.csproj --filter "DisplayName~Debe validar correctamente usernames válidos"

# Ejecutar solo pruebas de contraseña vacía
dotnet test Tests/AutogestionSena.Tests.csproj --filter "DisplayName~Debe rechazar contraseñas vacías"

# Ejecutar solo pruebas de email institucional
dotnet test Tests/AutogestionSena.Tests.csproj --filter "DisplayName~Debe validar correctamente emails institucionales"
```

## 📝 Estructura del Proyecto de Pruebas

```
Tests/
├── AutogestionSena.Tests.csproj          # Configuración del proyecto de pruebas
├── LoginValidationTests.cs               # 57 pruebas de login
├── CodeVerificationValidationTests.cs    # 31 pruebas de código de verificación
├── PasswordRecoveryValidationTests.cs    # 36 pruebas de recuperación de contraseña
├── PasswordResetValidationTests.cs       # 65 pruebas de restablecimiento
├── RegisterValidationTests.cs            # 98 pruebas de registro
├── TwoFactorValidationTests.cs           # 58 pruebas de 2FA
└── README_TESTS.md                       # Este archivo
```

## 🛠️ Validadores Incluidos

```
Validators/
├── LoginValidator.cs                     # Validaciones de login y 2FA
└── AuthenticationValidators.cs           # Validaciones de autenticación
    ├── CodeVerificationValidator         # Códigos de verificación
    ├── PasswordRecoveryValidator         # Recuperación de contraseña
    ├── PasswordResetValidator            # Restablecimiento de contraseña
    ├── RegisterValidator                 # Registro de usuarios
    └── TwoFactorValidator                # Autenticación 2FA (modal)
```

## ✅ Verificación de Estado de Pruebas

### Ver resumen rápido
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --logger:"console;verbosity=minimal"
```

### Ver lista de todas las pruebas sin ejecutar
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --list-tests
```

### Ejecutar en modo watch (re-ejecuta al cambiar código)
```powershell
dotnet watch test Tests/AutogestionSena.Tests.csproj
```

## 🐛 Depuración de Pruebas Fallidas

Si una prueba falla, ejecuta con detalle completo:

```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --logger:"console;verbosity=detailed" --filter "FullyQualifiedName~NombreDeLaPruebaFallida"
```

## 📚 Convenciones de Nombres

Todas las pruebas siguen el patrón:
- `MethodName_Scenario_ExpectedBehavior`
- Ejemplo: `IsPasswordValid_WithEmptyPassword_ReturnsFalse`

Los DisplayNames son descriptivos en español:
- Ejemplo: `"Debe rechazar contraseñas vacías"`

## 🎓 Buenas Prácticas Implementadas

1. ✅ **Separación de Concerns**: Validadores separados de la UI
2. ✅ **Pruebas Unitarias Puras**: Sin dependencias de MAUI o API
3. ✅ **Cobertura Exhaustiva**: Casos válidos, inválidos y de borde
4. ✅ **Nombres Descriptivos**: DisplayNames claros en español
5. ✅ **Uso de Theory**: Tests parametrizados con InlineData
6. ✅ **Organización por Región**: Código organizado con #region
7. ✅ **Pruebas de Integración**: Flujos completos validados
8. ✅ **Mensajes de Error**: Validación de mensajes localizados

## 📞 Resolución de Problemas

### Error: "No se puede encontrar el archivo especificado"
```powershell
# Asegúrate de estar en la carpeta correcta
cd "c:\Users\braya\OneDrive\Desktop\Front-end-Mobile-Autogestion-Sena"
```

### Error: "No tests were found"
```powershell
# Limpia y reconstruye el proyecto
dotnet clean Tests/AutogestionSena.Tests.csproj
dotnet build Tests/AutogestionSena.Tests.csproj
dotnet test Tests/AutogestionSena.Tests.csproj
```

### Las pruebas tardan mucho
```powershell
# Ejecuta en paralelo (por defecto ya está habilitado)
dotnet test Tests/AutogestionSena.Tests.csproj --parallel
```

## 📈 Estadísticas del Proyecto

| Métrica | Valor |
|---------|-------|
| Total de Pruebas | 345 |
| Archivos de Prueba | 6 |
| Validadores | 6 |
| Tiempo de Ejecución | ~7 segundos |
| Cobertura de Código | ~100% de validadores |
| Tasa de Éxito | 100% (345/345) |

## 🚦 Integración Continua (CI/CD)

Para integrar en un pipeline CI/CD, usa:

```yaml
# Ejemplo para GitHub Actions o Azure DevOps
- task: DotNetCoreCLI@2
  inputs:
    command: 'test'
    projects: 'Tests/AutogestionSena.Tests.csproj'
    arguments: '--configuration Release --logger trx --collect:"XPlat Code Coverage"'
```

## 📄 Licencia y Créditos

Proyecto: AutogestionSena - Módulo de Autenticación  
Framework: .NET MAUI 8.0  
Testing: xUnit 2.6.6  
Fecha: 2024
