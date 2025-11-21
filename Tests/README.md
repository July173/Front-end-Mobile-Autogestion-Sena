# 📚 Índice de Documentación - Pruebas Unitarias del Módulo de Autenticación

## 🎯 ¿Qué hay en esta carpeta?

Esta carpeta contiene **345 pruebas unitarias** para validar toda la lógica de autenticación del proyecto AutogestionSena MAUI.

---

## 📖 Documentación Disponible

### 🚀 Para Empezar Rápido
**[guias/GUIA_RAPIDA_EJECUCION.md](guias/GUIA_RAPIDA_EJECUCION.md)**
- Comandos más usados
- Ejecución por módulo
- Ejecución por funcionalidad
- Tips y tricks
- **⭐ RECOMENDADO PARA INICIO RÁPIDO**

### 🎯 Para Ejecutar por Funcionalidad de Usuario
**[guias/GUIA_EJECUCION_POR_FUNCIONALIDAD.md](guias/GUIA_EJECUCION_POR_FUNCIONALIDAD.md)**
- Ejecutar solo REGISTRO
- Ejecutar solo LOGIN
- Ejecutar solo RECUPERACIÓN de contraseña
- Ejecutar solo RESTABLECIMIENTO de contraseña
- Flujos completos por página
- **⭐ RECOMENDADO PARA DESARROLLO ENFOCADO**

### 📊 Resumen Ejecutivo
**[RESUMEN_COMPLETO_PRUEBAS.md](RESUMEN_COMPLETO_PRUEBAS.md)**
- Estadísticas completas (345 pruebas)
- Distribución por archivo
- Cobertura de validaciones
- Métricas de calidad
- Logros clave
- **⭐ RECOMENDADO PARA REVISIÓN GERENCIAL**

### 📘 Manual Completo
**[README_TESTS_COMPLETO.md](README_TESTS_COMPLETO.md)**
- Documentación exhaustiva
- Todos los comandos posibles
- Configuración de cobertura
- Integración CI/CD
- Resolución de problemas
- **⭐ RECOMENDADO PARA DESARROLLO Y MANTENIMIENTO**

### 📄 Manual Original (Login)
**[README_TESTS.md](README_TESTS.md)**
- Documentación inicial del módulo de login
- 57 pruebas específicas de login
- Primera versión del manual

---

## 🧪 Archivos de Prueba

### 1️⃣ [LoginValidationTests.cs](LoginValidationTests.cs) - 57 pruebas
Validaciones de inicio de sesión:
- Username y password
- Enrutamiento por roles (5 roles)
- Código 2FA
- Mensajes de error

### 2️⃣ [CodeVerificationValidationTests.cs](CodeVerificationValidationTests.cs) - 31 pruebas
Validaciones de códigos de verificación:
- Código no vacío
- Formato de 6 dígitos numéricos
- Caracteres no permitidos

### 3️⃣ [PasswordRecoveryValidationTests.cs](PasswordRecoveryValidationTests.cs) - 36 pruebas
Validaciones de recuperación de contraseña:
- Email válido
- Formato de email
- Email institucional SENA (@soy.sena.edu.co)

### 4️⃣ [PasswordResetValidationTests.cs](PasswordResetValidationTests.cs) - 65 pruebas
Validaciones de restablecimiento de contraseña:
- Longitud mínima (8 caracteres)
- Coincidencia de contraseñas
- Fortaleza (letras + números)

### 5️⃣ [RegisterValidationTests.cs](RegisterValidationTests.cs) - 98 pruebas
Validaciones de registro de usuarios:
- Email, nombres, apellidos
- Documento y tipo de documento
- Teléfono (7-10 dígitos)
- Validación completa de formulario

### 6️⃣ [TwoFactorValidationTests.cs](TwoFactorValidationTests.cs) - 58 pruebas
Validaciones de autenticación 2FA:
- Código completo (6 dígitos individuales)
- Validación numérica
- Combinación de dígitos

---

## ⚙️ Configuración

**[AutogestionSena.Tests.csproj](AutogestionSena.Tests.csproj)**
- Configuración del proyecto de pruebas
- Referencias a validadores
- Paquetes NuGet (xUnit, coverlet)

---

## 🎓 ¿Por Dónde Empezar?

### Si eres nuevo en el proyecto:
1. Lee **[RESUMEN_COMPLETO_PRUEBAS.md](RESUMEN_COMPLETO_PRUEBAS.md)** para entender el panorama general
2. Revisa **[GUIA_RAPIDA_EJECUCION.md](GUIA_RAPIDA_EJECUCION.md)** para ejecutar tus primeras pruebas
3. Explora los archivos de prueba para ver ejemplos de cada validación

### Si necesitas ejecutar pruebas:
1. Abre **[GUIA_RAPIDA_EJECUCION.md](GUIA_RAPIDA_EJECUCION.md)**
2. Copia el comando que necesitas
3. Ejecuta en PowerShell desde la raíz del proyecto

### Si necesitas mantener o extender las pruebas:
1. Lee **[README_TESTS_COMPLETO.md](README_TESTS_COMPLETO.md)** completo
2. Revisa el código de los validadores en `../Validators/`
3. Sigue los patrones existentes en los archivos de prueba

### Si necesitas reportar al equipo:
1. Usa **[RESUMEN_COMPLETO_PRUEBAS.md](RESUMEN_COMPLETO_PRUEBAS.md)** como base
2. Ejecuta todas las pruebas y copia el resumen
3. Genera reporte de cobertura si es necesario

---

## 📊 Resumen Rápido

| Métrica | Valor |
|---------|-------|
| **Total de Pruebas** | 381 ⬆️ |
| **Archivos de Prueba** | 6 |
| **Tasa de Éxito** | 100% ✅ |
| **Tiempo de Ejecución** | ~7 segundos |
| **Framework** | xUnit 2.6.6 |
| **Plataforma** | .NET 8.0 |

---

## 🚀 Comando Más Usado

```powershell
# Ejecutar todas las pruebas
cd "c:\Users\braya\OneDrive\Desktop\Front-end-Mobile-Autogestion-Sena"
dotnet test Tests/AutogestionSena.Tests.csproj
```

**Resultado esperado:**
```
Resumen de pruebas: total: 345; con errores: 0; correcto: 345; omitido: 0
```

---

## 🔗 Enlaces Útiles

### Validadores (Código Fuente)
- [`../Validators/LoginValidator.cs`](../Validators/LoginValidator.cs) - Login y 2FA
- [`../Validators/AuthenticationValidators.cs`](../Validators/AuthenticationValidators.cs) - Todos los demás validadores

### Documentación del Proyecto
- [`../readmes/`](../readmes/) - Otras guías del proyecto
- [`../explicacionArquitectura/`](../explicacionArquitectura/) - Arquitectura del proyecto

---

## 💡 Tips Importantes

1. **Siempre ejecuta desde la raíz del proyecto** (`Front-end-Mobile-Autogestion-Sena/`)
2. **Usa filtros** (`--filter`) para ejecutar solo las pruebas que necesitas
3. **Modo watch** (`dotnet watch test`) es ideal para desarrollo activo
4. **Verbosity detailed** solo cuando necesites debuggear un problema específico
5. **Las pruebas son rápidas** (~7 segundos todas), ¡ejecútalas frecuentemente!

---

## 📞 ¿Problemas?

Si encuentras problemas al ejecutar las pruebas:

1. Verifica que estés en la carpeta correcta del proyecto
2. Ejecuta `dotnet clean` y `dotnet build` en el proyecto de pruebas
3. Revisa la sección "Resolución de Problemas" en **[README_TESTS_COMPLETO.md](README_TESTS_COMPLETO.md)**
4. Verifica que tengas instalado .NET 8.0 SDK

---

## ✨ Estado Actual

- ✅ **345 pruebas implementadas**
- ✅ **100% de tasa de éxito**
- ✅ **Documentación completa**
- ✅ **Listo para producción**
- ✅ **Listo para CI/CD**

---

## 📅 Última Actualización

**Fecha:** 2024  
**Versión de .NET:** 8.0  
**Versión de xUnit:** 2.6.6  
**Estado:** ✅ Completado y operacional

---

## 🎉 ¡Comienza Ahora!

1. Abre **[GUIA_RAPIDA_EJECUCION.md](GUIA_RAPIDA_EJECUCION.md)**
2. Ejecuta tu primer comando
3. ¡Disfruta de las pruebas automatizadas!

**¡Todo está listo para usar!** 🚀
