# ?? Guía de Solución de Problemas de Conectividad - AutoGestión SENA

## ?? Problema: "Connection failure" en dispositivos móviles

### ? ¿Por qué funciona en Windows pero no en móvil?

Esto es un problema común en aplicaciones MAUI cuando hay diferencias de configuración de red entre plataformas.

### ?? Pasos para diagnosticar y solucionar:

#### 1?? **Verificar la IP del servidor**
- Abre una terminal/CMD en la máquina del servidor
- Ejecuta: `ipconfig` (Windows) o `ifconfig` (Mac/Linux)
- Encuentra tu IP local (generalmente 192.168.x.x o 10.x.x.x)

#### 2?? **Configurar la IP correcta en el código**
Edita el archivo: `Api/config/Endpoints.cs`

```csharp
private static string GetBaseUrl()
{
#if ANDROID
    // ?? Cambia esta IP por la IP real de tu máquina
    return "http://TU_IP_AQUI:8000/api/";
    
    // Opciones comunes:
    // Para emulador Android: "http://10.0.2.2:8000/api/"
 // Para tu red local: "http://192.168.1.XX:8000/api/"
#elif IOS
    return "http://TU_IP_AQUI:8000/api/";
#else
    return "http://localhost:8000/api/";
#endif
}
```

#### 3?? **Verificar que el servidor permita conexiones externas**
- Asegúrate de que tu servidor Django/API esté configurado para aceptar conexiones desde cualquier IP
- En Django: `python manage.py runserver 0.0.0.0:8000`
- Verifica el firewall de Windows/antivirus

#### 4?? **Probar la conectividad desde el dispositivo**
- Conecta tu móvil a la misma red WiFi que tu computadora
- Abre el navegador del móvil
- Visita: `http://TU_IP:8000/api/security/document-types/`
- Si no carga, el problema es de red/firewall

#### 5?? **Usar el diagnóstico integrado**
La app incluye un botón "?? Diagnosticar Conexión" que te ayuda a identificar problemas.

### ??? Soluciones comunes:

#### Para Emulador Android:
```csharp
return "http://10.0.2.2:8000/api/";
```

#### Para Dispositivo Android físico:
```csharp
return "http://192.168.1.18:8000/api/";  // Cambia por tu IP
```

#### Para iOS (Simulador o dispositivo):
```csharp
return "http://192.168.1.18:8000/api/";  // Cambia por tu IP
```

### ?? Configuración del Firewall (Windows):

1. Abre "Windows Defender Firewall"
2. Clic en "Allow an app or feature..."
3. Busca Python o tu servidor web
4. Marca las casillas "Private" y "Public"

### ?? Lista de verificación:

- [ ] ? Servidor ejecutándose en `0.0.0.0:8000`
- [ ] ? Móvil en la misma red WiFi
- [ ] ? IP correcta configurada en Endpoints.cs
- [ ] ? Firewall permite conexiones al puerto 8000
- [ ] ? Antivirus no bloquea las conexiones
- [ ] ? Navegador del móvil puede acceder a la API

### ?? Si aún no funciona:

1. **Usa hotspot del móvil**: Conecta la computadora al hotspot del móvil
2. **Cambia el puerto**: Prueba puerto 8080 o 3000
3. **Revisa logs**: Verifica los logs del servidor para ver si llegan las peticiones
4. **Prueba con Postman**: Usa Postman en la computadora para probar la API

### ?? URLs de ejemplo para probar:
- Documentos: `http://TU_IP:8000/api/security/document-types/`
- Dashboard: `http://TU_IP:8000/api/assign/request_asignation/aprendiz-dashboard/?apprentice_id=1`

---
?? **Recuerda**: El problema más común es que la IP en el código no coincide con la IP real de tu máquina en la red.