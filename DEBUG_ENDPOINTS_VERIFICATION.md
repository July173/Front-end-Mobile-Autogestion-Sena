# ?? Script de Verificación de Endpoints

## URLs a verificar según tu configuración actual:

### Android:
- Base URL: `http://10.3.232.121:8000/api/`
- Dashboard: `http://10.3.232.121:8000/api/assign/request_asignation/aprendiz-dashboard/?apprentice_id=2`
- Test: `http://10.3.232.121:8000/api/security/document-types/`

### Windows:
- Base URL: `http://localhost:8000/api/`
- Dashboard: `http://localhost:8000/api/assign/request_asignation/aprendiz-dashboard/?apprentice_id=2`
- Test: `http://localhost:8000/api/security/document-types/`

## ? Para probar manualmente:

### 1. Desde tu computadora (Windows):
```bash
# Probar endpoint de documentos
curl http://localhost:8000/api/security/document-types/

# Probar dashboard del aprendiz
curl "http://localhost:8000/api/assign/request_asignation/aprendiz-dashboard/?apprentice_id=2"
```

### 2. Desde el navegador de tu móvil:
1. Conecta el móvil a la misma red WiFi
2. Abre el navegador del móvil
3. Visita: `http://10.3.232.121:8000/api/security/document-types/`
4. Si carga JSON, la conectividad está bien
5. Prueba: `http://10.3.232.121:8000/api/assign/request_asignation/aprendiz-dashboard/?apprentice_id=2`

### 3. Con Postman/Insomnia:
```
GET http://10.3.232.121:8000/api/assign/request_asignation/aprendiz-dashboard/?apprentice_id=2
Headers:
Content-Type: application/json
```

## ?? Checklist de verificación:

- [ ] ? Servidor ejecutándose: `python manage.py runserver 0.0.0.0:8000`
- [ ] ? IP correcta en Endpoints.cs: `10.3.232.121`
- [ ] ? Puerto correcto: `8000`
- [ ] ? Móvil en la misma red WiFi
- [ ] ? Firewall permite conexiones al puerto 8000
- [ ] ? El endpoint existe en el backend
- [ ] ? El apprentice_id=2 existe en la base de datos

## ?? Qué buscar en los logs de depuración:

1. **Logs de inicio** (ApprenticeDashboardVM):
   ```
   ?? [ApprenticeDashboardVM] ===== INICIANDO CARGA DASHBOARD =====
   ?? [ApprenticeDashboardVM] Apprentice ID: 2
   ?? [ApprenticeDashboardVM] URL Base: http://10.3.232.121:8000/api/
   ```

2. **Logs de petición HTTP** (ApiService):
   ```
   ?? [ApiService] ===== INICIANDO PETICIÓN HTTP GET =====
   ?? [ApiService] URL Completa: http://10.3.232.121:8000/api/assign/request_asignation/aprendiz-dashboard/?apprentice_id=2
   ```

3. **Logs de respuesta** (AssignationService):
   ```
   ? [AssignationService] ===== RESPUESTA RECIBIDA =====
   ?? [AssignationService] Response != null: True
   ?? [AssignationService] Response.Id: 123
   ```

## ?? Errores comunes a identificar:

### Error de conexión:
```
?? [ApiService] ===== HTTP REQUEST ERROR =====
? [ApiService] HttpRequestException: No such host is known
```
**Solución**: Verificar la IP en Endpoints.cs

### Timeout:
```
? [ApiService] ===== TIMEOUT/CONNECTION ERROR =====
?? [ApiService] TaskCanceledException: The operation was canceled
```
**Solución**: Verificar que el servidor esté ejecutándose

### Endpoint no encontrado:
```
?? [ApiService] Status Code: 404 - NotFound
? [ApiService] Error Content: {"detail":"Not found."}
```
**Solución**: Verificar que el endpoint exista en el backend

### Sin datos:
```
?? [AssignationService] Response != null: True
?? [AssignationService] Response.Id: null
```
**Solución**: El apprentice_id no existe o no tiene datos