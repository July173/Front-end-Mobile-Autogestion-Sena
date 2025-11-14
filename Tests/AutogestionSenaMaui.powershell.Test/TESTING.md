# 🧪 Pruebas de Endpoints - AutoGestión SENA

Este directorio contiene scripts para probar los endpoints de la API antes de integrarlos en la aplicación móvil.

## 📁 Estructura de Tests

```
Tests/
├── run-all-tests.ps1       # Suite completa de pruebas automatizadas
├── test-individual.ps1     # Pruebas individuales paso a paso
├── test-endpoints.ps1      # Script interactivo guiado
├── test-endpoints-quick.ps1 # Comandos rápidos manuales
└── TESTING.md             # Esta documentación
```

## 📋 Archivos de prueba

### 1. `run-all-tests.ps1` ⭐ (Recomendado)
Suite completa de pruebas automatizadas con credenciales preconfiguradas.

**Uso:**
```powershell
cd Tests
.\run-all-tests.ps1
```

**Características:**
- ✅ Credenciales ya configuradas (bscortes40@soy.sena.edu.co)
- 📊 Resumen visual con estadísticas
- 🎯 Flujo completo: Login → 2FA → Registro
- 🎨 Interfaz mejorada con colores y símbolos

**Credenciales preconfiguradas:**
- Email: `bscortes40@soy.sena.edu.co`
- Password: `1129844804`

---

### 2. `test-individual.ps1`
Pruebas paso a paso con código comentado para personalizar.

**Uso:**
Abre el archivo, edita los datos necesarios y copia/pega el comando que quieras probar.

**Ejemplo:**
```powershell
# Edita el email y contraseña
$loginBody = @{
    email = "tuusuario@soy.sena.edu.co"
    password = "tucontraseña"
} | ConvertTo-Json

# Ejecuta la prueba
Invoke-RestMethod -Uri "$baseUrl/security/users/validate-institutional-login/" -Method Post -Body $loginBody -ContentType "application/json"
```

---

## 🎯 Endpoints disponibles

### 1. **GET** - Tipos de documento
```
GET /security/document-types/
```
**Respuesta esperada:**
```json
[
  {
    "id": 1,
    "name": "Cédula de Ciudadanía",
    "acronyms": "CC",
    "active": true
  }
]
```

---

### 2. **POST** - Validar login institucional
```
POST /security/users/validate-institutional-login/
```
**Body:**
```json
{
  "email": "usuario@soy.sena.edu.co",
  "password": "contraseña"
}
```
**Respuesta esperada:**
```json
{
  "message": "Código de verificación enviado al correo"
}
```

---

### 3. **POST** - Validar código 2FA
```
POST /security/users/validate-2fa-code/
```
**Body:**
```json
{
  "email": "usuario@soy.sena.edu.co",
  "code": "123456"
}
```
**Respuesta esperada:**
```json
{
  "access": "token_jwt_aqui",
  "refresh": "refresh_token_aqui",
  "user": { ... }
}
```

---

### 4. **POST** - Registrar aprendiz
```
POST /security/persons/register-apprentice/
```
**Body:**
```json
{
  "email": "nuevo@soy.sena.edu.co",
  "first_name": "Juan",
  "first_last_name": "Pérez",
  "type_identification": 1,
  "number_identification": 1234567890,
  "phone_number": 3001234567
}
```
**Respuesta esperada:**
```json
{
  "success": true,
  "detail": "Registro exitoso",
  "user": { ... }
}
```

---

### 5. **POST** - Solicitar recuperación de contraseña
```
POST /security/users/request-password-reset/
```
**Body:**
```json
{
  "email": "usuario@soy.sena.edu.co"
}
```

---

### 6. **POST** - Resetear contraseña
```
POST /security/users/reset-password/
```
**Body:**
```json
{
  "email": "usuario@soy.sena.edu.co",
  "code": "123456",
  "new_password": "NuevaContraseña123!"
}
```

---

## 🚀 Comenzar a probar

### Opción 1: Suite completa automatizada ⭐ (Recomendado)
```powershell
cd Tests
.\run-all-tests.ps1
```

### Opción 2: Pruebas individuales
```powershell
cd Tests
.\test-individual.ps1
```

### Opción 3: Modo interactivo guiado
```powershell
cd Tests
.\test-endpoints.ps1
```

### Opción 4: Comandos manuales
1. Abre `test-endpoints-quick.ps1`
2. Edita los datos de prueba (email, contraseña, etc.)
3. Copia el comando que quieras probar
4. Pégalo en PowerShell y presiona Enter

### Opción 3: Comandos directos

**Probar tipos de documento:**
```powershell
Invoke-WebRequest -Uri "http://192.168.1.18:8000/api/security/document-types/" -Method Get | Select-Object -ExpandProperty Content | ConvertFrom-Json | Format-Table
```

**Probar login:**
```powershell
$body = @{ email = "tu@soy.sena.edu.co"; password = "tupassword" } | ConvertTo-Json
Invoke-WebRequest -Uri "http://192.168.1.18:8000/api/security/users/validate-institutional-login/" -Method Post -Body $body -ContentType "application/json"
```

---

## ⚙️ Configuración

Si tu servidor está en una IP diferente, edita la variable `$baseUrl` en los scripts:

```powershell
$baseUrl = "http://TU_IP:8000/api"
```

---

## 📝 Notas importantes

1. **Credenciales válidas:** Necesitas un usuario registrado en el sistema para probar el login
2. **Código 2FA:** El código se envía por email y expira después de unos minutos
3. **Email institucional:** Debe terminar en `@soy.sena.edu.co`
4. **Tipos de identificación:**
   - 1 = Cédula de Ciudadanía (CC)
   - 2 = Tarjeta de Identidad (TI)
   - 3 = Cédula de Extranjería (CE)
   - etc.

---

## 🐛 Troubleshooting

**Error: "No se puede conectar"**
- Verifica que el servidor esté corriendo
- Confirma la IP con `ipconfig`
- Prueba con: `curl http://192.168.1.18:8000/api/`

**Error: "Credenciales inválidas"**
- Verifica que el usuario exista en la base de datos
- Confirma que la contraseña sea correcta
- Revisa que el email termine en `@soy.sena.edu.co`

**Error: "Código 2FA inválido"**
- El código expira después de algunos minutos
- Asegúrate de usar el código más reciente
- Verifica que no tenga espacios adicionales

---

## ✅ Checklist de pruebas

Antes de integrar en la app móvil, verifica que funcionen:

- [ ] ✓ GET - Tipos de documento
- [ ] ✓ POST - Login institucional
- [ ] ✓ POST - Validación 2FA
- [ ] ✓ POST - Registro de aprendiz
- [ ] ✓ POST - Recuperación de contraseña
- [ ] ✓ POST - Reseteo de contraseña

---

## 📚 Documentación adicional

Para más información sobre la API, consulta:
- Swagger/OpenAPI: `http://192.168.1.18:8000/api/docs/`
- Redoc: `http://192.168.1.18:8000/api/redoc/`
