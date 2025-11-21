# 🎯 Guía de Ejecución por Funcionalidad de Usuario

Esta guía te ayuda a ejecutar pruebas según la funcionalidad específica que estés desarrollando o debuggeando.

---

## 📱 Flujos de Usuario Completos

### 1️⃣ INICIO DE SESIÓN COMPLETO (57 pruebas)
**Cuándo usar:** Estás trabajando en LoginPage.xaml.cs

```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~LoginValidationTests"
```

**Qué se prueba:**
- ✅ Validación de username (vacío, formato)
- ✅ Validación de password (vacío, formato)
- ✅ Validación de credenciales combinadas
- ✅ Enrutamiento según rol del usuario:
  - Rol 1: Seguridad → SecurityMainPage
  - Rol 2: Aprendiz → ApprenticeDashboard
  - Rol 3: Instructor → InstructorDashboard
  - Rol 4: Coordinador → CoordinatorDashboard
  - Rol 5: Operador Sofia → SofiaOperatorDashboard
- ✅ Validación de código 2FA (6 dígitos)

**Resultado esperado:** 57/57 pruebas exitosas

---

### 2️⃣ REGISTRO DE NUEVO USUARIO (98 pruebas)
**Cuándo usar:** Estás trabajando en RegisterPage.xaml.cs

```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~RegisterValidationTests"
```

**Qué se prueba:**
- ✅ Email institucional obligatorio
- ✅ Nombres obligatorios (con soporte para tildes y ñ)
- ✅ Apellidos obligatorios (con soporte para tildes y ñ)
- ✅ Tipo de documento seleccionado (requerido)
- ✅ Número de documento:
  - No vacío
  - Solo dígitos numéricos
- ✅ Teléfono:
  - No vacío
  - Solo dígitos numéricos
  - Longitud válida (7-10 dígitos)
  - Compatible con teléfonos colombianos
- ✅ Validación completa del formulario
- ✅ Mensajes de error específicos para cada campo

**Resultado esperado:** 98/98 pruebas exitosas

---

### 3️⃣ RESTABLECIMIENTO DE CONTRASEÑA (65 pruebas)
**Cuándo usar:** Estás trabajando en PasswordResetPage.xaml.cs

```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~PasswordResetValidationTests"
```

**Qué se prueba:**
- ✅ Contraseña no vacía
- ✅ Longitud mínima de 8 caracteres
- ✅ Contraseñas coinciden (password == confirmPassword)
- ✅ Fortaleza de contraseña:
  - Al menos una letra
  - Al menos un número
- ✅ Sensibilidad a mayúsculas en comparación
- ✅ Manejo de espacios en contraseñas
- ✅ Contraseñas muy largas
- ✅ Mensajes de error localizados

**Resultado esperado:** 65/65 pruebas exitosas

---

### 4️⃣ RECUPERACIÓN DE CONTRASEÑA (36 pruebas)
**Cuándo usar:** Estás trabajando en PasswordRecoveryPage.xaml.cs

```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~PasswordRecoveryValidationTests"
```

**Qué se prueba:**
- ✅ Email no vacío
- ✅ Formato básico de email:
  - Contiene @
  - Contiene .
- ✅ Email institucional del SENA:
  - Termina en @soy.sena.edu.co
  - Insensible a mayúsculas
- ✅ Rechazo de emails personales (Gmail, Hotmail, etc.)
- ✅ Mensajes de error apropiados

**Resultado esperado:** 36/36 pruebas exitosas

---

### 5️⃣ VERIFICACIÓN DE CÓDIGO (31 pruebas)
**Cuándo usar:** Estás trabajando en CodeVerificationPage.xaml.cs

```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~CodeVerificationValidationTests"
```

**Qué se prueba:**
- ✅ Código no vacío
- ✅ Formato de 6 dígitos exactos
- ✅ Solo caracteres numéricos
- ✅ Rechazo de letras
- ✅ Rechazo de caracteres especiales
- ✅ Rechazo de espacios
- ✅ Longitud exacta (no 5, no 7, solo 6)
- ✅ Mensajes de error

**Resultado esperado:** 31/31 pruebas exitosas

---

### 6️⃣ AUTENTICACIÓN DE DOS FACTORES - MODAL (58 pruebas)
**Cuándo usar:** Estás trabajando en TwoFactorModal.xaml.cs

```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~TwoFactorValidationTests"
```

**Qué se prueba:**
- ✅ Código completo de 6 dígitos individuales
- ✅ Validación de cada dígito por separado
- ✅ Combinación correcta de los 6 dígitos
- ✅ Validación numérica de cada campo
- ✅ Rechazo si falta algún dígito
- ✅ Manejo de espacios en blanco
- ✅ Códigos repetidos (111111)
- ✅ Flujos de integración completos

**Resultado esperado:** 58/58 pruebas exitosas

---

## 🔧 Pruebas por Campo/Validación Específica

### Solo Validación de Email
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsEmailValid"
```
**Usado en:** RegisterPage, PasswordRecoveryPage

### Solo Validación de Email Institucional SENA
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsInstitutionalEmail"
```
**Usado en:** PasswordRecoveryPage

### Solo Validación de Nombres
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsNameValid"
```
**Usado en:** RegisterPage

### Solo Validación de Apellidos
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsLastNameValid"
```
**Usado en:** RegisterPage

### Solo Validación de Documento
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsDocumentNumberValid"
```
**Usado en:** RegisterPage

### Solo Validación de Teléfono
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsPhoneValid"
```
**Usado en:** RegisterPage

### Solo Validación de Longitud de Teléfono
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsPhoneLengthValid"
```
**Usado en:** RegisterPage (7-10 dígitos para teléfonos colombianos)

### Solo Validación de Contraseña
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsPasswordValid"
```
**Usado en:** LoginPage, PasswordResetPage

### Solo Validación de Longitud de Contraseña
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsPasswordLengthValid"
```
**Usado en:** PasswordResetPage (mínimo 8 caracteres)

### Solo Validación de Coincidencia de Contraseñas
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~DoPasswordsMatch"
```
**Usado en:** PasswordResetPage

### Solo Validación de Fortaleza de Contraseña
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsPasswordStrong"
```
**Usado en:** PasswordResetPage (debe tener letras y números)

### Solo Validación de Código de 6 Dígitos
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsCode6DigitsValid"
```
**Usado en:** CodeVerificationPage

### Solo Validación de Código 2FA Completo
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsCodeComplete"
```
**Usado en:** TwoFactorModal

### Solo Validación de Username
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsUsernameValid"
```
**Usado en:** LoginPage

### Solo Enrutamiento por Roles
```powershell
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~GetRouteForRole"
```
**Usado en:** LoginPage (redirige según el rol del usuario)

---

## 🎨 Casos de Uso Prácticos

### Estoy arreglando un bug en el formulario de registro
```powershell
# Ejecuta solo las pruebas de registro
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~RegisterValidationTests"
```

### Estoy modificando la validación de teléfonos
```powershell
# Ejecuta solo las pruebas relacionadas con teléfonos
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsPhone"
```

### Cambié la lógica de validación de emails institucionales
```powershell
# Ejecuta solo las pruebas de email institucional
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsInstitutionalEmail"
```

### Modifiqué el flujo de restablecimiento de contraseña
```powershell
# Ejecuta todas las pruebas de reset de contraseña
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~PasswordResetValidationTests"
```

### Trabajé en el modal de 2FA
```powershell
# Ejecuta todas las pruebas del modal 2FA
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~TwoFactorValidationTests"
```

### Cambié la longitud mínima de contraseñas
```powershell
# Ejecuta solo las pruebas de longitud de contraseña
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsPasswordLengthValid"
```

---

## 📊 Tabla de Referencia Rápida

| Página/Componente | Comando | Pruebas | Tiempo |
|-------------------|---------|---------|--------|
| LoginPage | `--filter "FullyQualifiedName~LoginValidationTests"` | 57 | ~3-5s |
| RegisterPage | `--filter "FullyQualifiedName~RegisterValidationTests"` | 98 | ~4-6s |
| PasswordResetPage | `--filter "FullyQualifiedName~PasswordResetValidationTests"` | 65 | ~4-5s |
| PasswordRecoveryPage | `--filter "FullyQualifiedName~PasswordRecoveryValidationTests"` | 36 | ~2-3s |
| CodeVerificationPage | `--filter "FullyQualifiedName~CodeVerificationValidationTests"` | 31 | ~2-3s |
| TwoFactorModal | `--filter "FullyQualifiedName~TwoFactorValidationTests"` | 58 | ~3-4s |

---

## 💡 Tips para Desarrollo Eficiente

### Durante Desarrollo Activo
```powershell
# Usa modo watch para re-ejecutar automáticamente
dotnet watch test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~RegisterValidationTests"
```
Las pruebas se re-ejecutarán cada vez que guardes un archivo.

### Para Debug de un Bug Específico
```powershell
# Ejecuta con verbosity detailed para ver detalles
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~IsPhoneLengthValid" --verbosity detailed
```

### Antes de Hacer Commit
```powershell
# Ejecuta todas las pruebas para asegurar que nada se rompió
dotnet test Tests/AutogestionSena.Tests.csproj
```

### Para Verificar Cambios en un Validador
```powershell
# Si modificaste LoginValidator.cs
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~LoginValidationTests"

# Si modificaste RegisterValidator en AuthenticationValidators.cs
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~RegisterValidationTests"
```

---

## 🚀 Flujo de Trabajo Recomendado

1. **Identifica la página** en la que estás trabajando
2. **Ejecuta las pruebas de esa página** específica
3. **Haz tus cambios** en el código
4. **Re-ejecuta las pruebas** de esa página
5. **Si todo pasa**, ejecuta **todas las pruebas** antes de commit
6. **Haz commit** solo si todas las 345 pruebas pasan

---

## 📞 Ejemplo Completo de Sesión de Desarrollo

```powershell
# 1. Inicio - Voy a trabajar en RegisterPage
cd "c:\Users\braya\OneDrive\Desktop\Front-end-Mobile-Autogestion-Sena"

# 2. Ejecuto las pruebas actuales del registro
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~RegisterValidationTests"
# ✅ 98/98 pruebas pasan

# 3. Hago cambios en RegisterPage.xaml.cs o RegisterValidator.cs
# ... editando código ...

# 4. Re-ejecuto las pruebas de registro
dotnet test Tests/AutogestionSena.Tests.csproj --filter "FullyQualifiedName~RegisterValidationTests"
# ✅ 98/98 pruebas pasan (mis cambios no rompieron nada)

# 5. Antes de commit, ejecuto TODAS las pruebas
dotnet test Tests/AutogestionSena.Tests.csproj
# ✅ 345/345 pruebas pasan

# 6. Hago commit con confianza
git add .
git commit -m "feat: mejoras en validación de registro"
git push
```

---

## ✅ Resultado Esperado

Para cada comando, debes ver:
```
Resumen de pruebas: total: XX; con errores: 0; correcto: XX; omitido: 0
```

Si ves `con errores: 0`, ¡todo está bien! 🎉

---

## 📚 Documentación Relacionada

- **`README.md`** - Índice principal
- **`GUIA_RAPIDA_EJECUCION.md`** - Todos los comandos posibles
- **`README_TESTS_COMPLETO.md`** - Manual exhaustivo
- **`EJEMPLOS_SALIDA.md`** - Ejemplos de salidas reales

---

¡Usa esta guía para ejecutar solo lo que necesitas y desarrollar más eficientemente! 🚀
