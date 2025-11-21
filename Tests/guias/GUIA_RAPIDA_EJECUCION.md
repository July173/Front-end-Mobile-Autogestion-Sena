# 🚀 Guía Rápida - Ejecución de Pruebas Unitarias

## ⚡ Comandos más Usados

### Ejecutar TODAS las 345 pruebas
```powershell
cd "c:\Users\braya\OneDrive\Desktop\Front-end-Mobile-Autogestion-Sena"
dotnet test Tests/AutogestionSena.Tests.csproj
```

---

## 📂 Ejecutar por Archivo de Prueba

### 1️⃣ Login (57 pruebas)
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~LoginValidationTests"
```

### 2️⃣ Código de Verificación (31 pruebas)
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~CodeVerificationValidationTests"
```

### 3️⃣ Recuperación de Contraseña (36 pruebas)
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~PasswordRecoveryValidationTests"
```

### 4️⃣ Restablecimiento de Contraseña (65 pruebas)
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~PasswordResetValidationTests"
```

### 5️⃣ Registro de Usuarios (98 pruebas)
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~RegisterValidationTests"
```

### 6️⃣ Autenticación 2FA (58 pruebas)
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~TwoFactorValidationTests"
```

### pruebas por funcion

### Ejecutar prueba individual por nombre completo

```powershell

dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~AreCredentialsValid_WithInvalidData_ReturnsFalse" --verbosity normal

```

---

## 🎯 Ejecutar por Página/Funcionalidad Completa

### 🔐 Solo Pruebas de LOGIN (57 pruebas)
Ejecuta todas las validaciones relacionadas con el inicio de sesión:
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~LoginValidationTests"
```
**Incluye:** Username, password, credenciales, enrutamiento por roles, código 2FA

### 📝 Solo Pruebas de REGISTRO (98 pruebas)
Ejecuta todas las validaciones del formulario de registro:
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~RegisterValidationTests"
```
**Incluye:** Email, nombres, apellidos, tipo documento, número documento, teléfono, validación completa

### 🔑 Solo Pruebas de RESTABLECIMIENTO DE CONTRASEÑA (65 pruebas)
Ejecuta validaciones para resetear contraseña:
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~PasswordResetValidationTests"
```
**Incluye:** Longitud mínima, coincidencia de contraseñas, fortaleza, mensajes de error

### 📧 Solo Pruebas de RECUPERACIÓN DE CONTRASEÑA (36 pruebas)
Ejecuta validaciones para recuperación por email:
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~PasswordRecoveryValidationTests"
```
**Incluye:** Email válido, formato de email, email institucional SENA

### 🔢 Solo Pruebas de VERIFICACIÓN DE CÓDIGO (31 pruebas)
Ejecuta validaciones de código de 6 dígitos:
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~CodeVerificationValidationTests"
```
**Incluye:** Código no vacío, formato 6 dígitos, validación numérica

### 🔐 Solo Pruebas de AUTENTICACIÓN 2FA (58 pruebas)
Ejecuta validaciones del modal de 2FA:
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~TwoFactorValidationTests"
```
**Incluye:** 6 dígitos individuales, código completo, validación numérica, combinación

---

## 🔍 Ejecutar por Funcionalidad Específica (Métodos Individuales)

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

### Validaciones de Email Institucional SENA
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsInstitutionalEmail"
```

### Validaciones de Código de 6 Dígitos
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsCode6DigitsValid"
```

### Validaciones de Longitud de Contraseña
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsPasswordLengthValid"
```

### Validaciones de Coincidencia de Contraseñas
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~DoPasswordsMatch"
```

### Validaciones de Fortaleza de Contraseña
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsPasswordStrong"
```

### Validaciones de Documento
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsDocumentNumberValid"
```

### Validaciones de Teléfono
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsPhoneValid"
```

### Validaciones de Teléfono Numérico
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsPhoneNumeric"
```

### Validaciones de Longitud de Teléfono
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsPhoneLengthValid"
```

### Validación de Todos los Campos de Registro
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~AreAllFieldsValid"
```

### Validación de Código 2FA Completo
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsCodeComplete"
```

### Validación de Código Numérico 2FA
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsCodeNumeric"
```

### Enrutamiento por Roles
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~GetRouteForRole"
```

### Combinación de Código 2FA
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~CombineCode"
```

---

## 🔍 Ejecutar Prueba Individual por DisplayName

```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "DisplayName~texto del nombre"
```

**Ejemplos:**

```powershell
# Username válido
dotnet test Tests/AutogestionSena.Tests.csproj --filter "DisplayName~Debe validar correctamente usernames válidos"

# Contraseñas vacías
dotnet test Tests/AutogestionSena.Tests.csproj --filter "DisplayName~Debe rechazar contraseñas vacías"

# Email institucional
dotnet test Tests/AutogestionSena.Tests.csproj --filter "DisplayName~Debe validar correctamente emails institucionales"

# Código de 6 dígitos
dotnet test Tests/AutogestionSena.Tests.csproj --filter "DisplayName~Debe validar correctamente códigos de 6 dígitos"
```

---

## 📊 Opciones de Visualización

### Verbose (detalle completo)
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --verbosity detailed
```

### Minimal (solo resumen)
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --verbosity minimal
```

### Listar todas las pruebas sin ejecutar
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --list-tests
```

---

## 🔄 Modo Watch (Re-ejecuta al cambiar código)

```powershell
dotnet watch test Tests/AutogestionSena.Tests.csproj
```

---

## 🐛 Depuración de Pruebas Fallidas

### Ver detalle de una prueba específica
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --logger:"console;verbosity=detailed" --filter "FullyQualifiedName~NombreDeLaPrueba"
```

### Limpiar y reconstruir
```powershell
dotnet clean Tests/AutogestionSena.Tests.csproj
dotnet build Tests/AutogestionSena.Tests.csproj
dotnet test Tests/AutogestionSena.Tests.csproj
```

---

## 📈 Cobertura de Código

### Ejecutar con cobertura
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
```

### Instalar generador de reportes (solo primera vez)
```powershell
dotnet tool install --global dotnet-reportgenerator-globaltool
```

### Generar reporte HTML
```powershell
reportgenerator -reports:"Tests/coverage.cobertura.xml" -targetdir:"Tests/coveragereport" -reporttypes:Html
```

---

## ⏱️ Tiempos de Ejecución Aproximados

| Pruebas | Tiempo Aproximado |
|---------|-------------------|
| Todas (345) | ~7 segundos |
| Login (57) | ~1 segundo |
| Código Verificación (31) | ~0.5 segundos |
| Recuperación (36) | ~0.5 segundos |
| Restablecimiento (65) | ~1 segundo |
| Registro (98) | ~1.5 segundos |
| 2FA (58) | ~1 segundo |

---

## 💡 Tips

1. **Siempre ejecuta desde la raíz del proyecto** para evitar errores de rutas
2. **Usa `--filter`** para ejecutar solo lo que necesitas y ahorrar tiempo
3. **Modo watch** es ideal durante desarrollo activo
4. **`--list-tests`** te ayuda a encontrar el nombre exacto de una prueba
5. **Verbosity detailed** solo cuando necesites debuggear un fallo específico

---

## 📚 Más Información

Ver documentación completa en:
- **`Tests/README_TESTS_COMPLETO.md`** - Manual detallado
- **`Tests/RESUMEN_COMPLETO_PRUEBAS.md`** - Resumen ejecutivo

---

## ✅ Resultado Esperado

```
Resumen de pruebas: total: 345; con errores: 0; correcto: 345; omitido: 0
```

**¡100% de éxito!** 🎉
