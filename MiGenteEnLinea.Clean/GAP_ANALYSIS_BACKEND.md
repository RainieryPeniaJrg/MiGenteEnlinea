# 📊 GAP ANALYSIS: Legacy Services → Clean Architecture API

**Fecha de Análisis:** 2025-01-15  
**Objetivo:** Identificar qué métodos del Legacy faltan implementar en la API Clean Architecture  
**Estado:** ✅ Análisis Completo

---

## 🔍 RESUMEN EJECUTIVO

| Categoría | Legacy | API | Implementado | Faltante | % Completado |
|-----------|--------|-----|--------------|----------|--------------|
| **Authentication** | 11 métodos | 6 endpoints | 6 | 5 | **55%** |
| **Empleados & Nómina** | 32 métodos | ~15 endpoints | 12 | 20 | **38%** |
| **Contratistas** | 10 métodos | 5 endpoints | 5 | 5 | **50%** |
| **Suscripciones** | 17 métodos | 5 endpoints | 5 | 12 | **29%** |
| **Calificaciones** | 4 métodos | 5 endpoints | 4 | 0 | **100%** ✅ |
| **Pagos (Cardnet)** | 3 métodos | 3 endpoints | 3 | 0 | **100%** ✅ |
| **Email** | 1 método | Infraestructure | ✅ | 0 | **100%** ✅ |
| **Bot (OpenAI)** | 1 método | N/A | 0 | 1 | **0%** |
| **Utilitario** | 2 métodos | N/A | 0 | 2 | **0%** |
| **TOTAL** | **81 métodos** | **~40 endpoints** | **35** | **45** | **43%** |

---

## 📦 INVENTARIO COMPLETO: LEGACY SERVICES

### 1️⃣ LoginService.asmx.cs (11 métodos)

**Ubicación:** `Codigo Fuente Mi Gente/MiGente_Front/Services/LoginService.asmx.cs`  
**Propósito:** Autenticación, gestión de usuarios y perfiles

| # | Método Legacy | Parámetros | Retorna | Status API | Endpoint API | Notas |
|---|---------------|------------|---------|------------|--------------|-------|
| 1 | `login()` | email, pass | int (2=success, 0=invalid, -1=inactive) | ✅ | POST /api/auth/login | Migrado con JWT en LOTE 1 |
| 2 | `borrarUsuario()` | userID, credencialID | void | ❌ | N/A | **FALTA: DELETE /api/auth/users/{userId}/credentials/{credentialId}** |
| 3 | `obtenerPerfil()` | userID | VPerfiles | ✅ | GET /api/auth/perfil/{userId} | Migrado en LOTE 1 |
| 4 | `obtenerPerfilByEmail()` | email | VPerfiles | ✅ | GET /api/auth/perfil/email/{email} | Migrado en LOTE 1 |
| 5 | `obtenerCredenciales()` | userID | List<Credenciales> | ✅ | GET /api/auth/credenciales/{userId} | Migrado en LOTE 1 |
| 6 | `actualizarPerfil()` | perfilesInfo, Cuentas | bool | ⚠️ | PUT /api/auth/profile | Migrado parcial - solo Cuentas |
| 7 | `actualizarPerfil1()` | Cuentas | bool | ⚠️ | PUT /api/auth/profile | Ver nota anterior |
| 8 | `agregarPerfilInfo()` | perfilesInfo | bool | ❌ | N/A | **FALTA: POST /api/auth/profile-info** |
| 9 | `getPerfilByID()` | cuentaID | Cuentas | ❌ | N/A | **FALTA: GET /api/auth/cuenta/{cuentaId}** |
| 10 | `validarCorreo()` | correo | bool | ✅ | GET /api/auth/validar-correo | Migrado en LOTE 1 |
| 11 | `getPerfilInfo()` | userID (Guid) | VPerfiles | ✅ | GET /api/auth/perfil/{userId} | Mismo que #3 |

**Prioridad:** 🔴 ALTA (Autenticación crítica)  
**Tiempo Estimado Faltantes:** 3-4 horas

---

### 2️⃣ EmpleadosService.cs (32 métodos) 🔥 MÁS COMPLEJO

**Ubicación:** `Codigo Fuente Mi Gente/MiGente_Front/Services/EmpleadosService.cs`  
**Propósito:** Gestión de empleados, contratos temporales, nómina, pagos

| # | Método Legacy | Parámetros | Retorna | Status API | Endpoint API | Notas |
|---|---------------|------------|---------|------------|--------------|-------|
| 1 | `getEmpleados()` | userID | IQueryable<Empleados> | ✅ | GET /api/empleados | LOTE 1 |
| 2 | `getVEmpleados()` | userID | List<VEmpleados> | ✅ | GET /api/empleados | Mismo query |
| 3 | `getContrataciones()` | userID | List<EmpleadosTemporales> | ⚠️ | GET /api/contrataciones | LOTE 3 - verificar include DetalleContrataciones |
| 4 | `getEmpleadosByID()` | userID, id | Empleados | ✅ | GET /api/empleados/{id} | LOTE 1 |
| 5 | `obtenerRemuneraciones()` | userID, empleadoID | List<Remuneraciones> | ❌ | N/A | **FALTA: GET /api/empleados/{id}/remuneraciones** |
| 6 | `quitarRemuneracion()` | userID, id | void | ❌ | N/A | **FALTA: DELETE /api/remuneraciones/{id}** |
| 7 | `guardarEmpleado()` | Empleados | Empleados | ✅ | POST /api/empleados | LOTE 1 |
| 8 | `actualizarEmpleado()` | Empleados | Empleados | ✅ | PUT /api/empleados/{id} | LOTE 1 |
| 9 | `ActualizarEmpleado()` | Empleados | bool | ✅ | PUT /api/empleados/{id} | Duplicado del anterior |
| 10 | `procesarPago()` | header, detalle | int (pagoID) | ✅ | POST /api/nominas/procesar-lote | LOTE 5.6 |
| 11 | `procesarPagoContratacion()` | header, detalle | int (pagoID) | ⚠️ | POST /api/contrataciones/procesar-pago? | **VERIFICAR SI EXISTE** |
| 12 | `GetEmpleador_Recibos_Empleado()` | userID, empleadoID | List<VRecibosEmpleados> | ✅ | GET /api/empleados/{id}/recibos | LOTE 5.6 |
| 13 | `GetEmpleador_ReciboByPagoID()` | pagoID | Empleador_Recibos_Header | ✅ | GET /api/nominas/recibos/{pagoId} | LOTE 5.6 |
| 14 | `GetContratacion_ReciboByPagoID()` | pagoID | Header_Contrataciones | ❌ | N/A | **FALTA: GET /api/contrataciones/recibos/{pagoId}** |
| 15 | `cancelarTrabajo()` | contratacionID, detalleID | bool | ❌ | N/A | **FALTA: POST /api/contrataciones/{id}/detalle/{detalleId}/cancelar** |
| 16 | `eliminarReciboEmpleado()` | pagoID | bool | ✅ | DELETE /api/nominas/recibos/{id} | LOTE 5.6 (Anular) |
| 17 | `eliminarReciboContratacion()` | pagoID | bool | ❌ | N/A | **FALTA: DELETE /api/contrataciones/recibos/{pagoId}** |
| 18 | `eliminarEmpleadoTemporal()` | contratacionID | bool | ❌ | N/A | **FALTA: DELETE /api/contrataciones/{id}** (con cascade de recibos) |
| 19 | `GetEmpleador_RecibosContratacionesByID()` | contratacionID, detalleID | List<VPagosContrataciones> | ❌ | N/A | **FALTA: GET /api/contrataciones/{id}/detalle/{detalleId}/pagos** |
| 20 | `darDeBaja()` | empleadoID, userID, fechaBaja, prestaciones, motivo | bool | ✅ | POST /api/empleados/{id}/dar-baja | LOTE 1 |
| 21 | `nuevoTemporal()` | temp, det | bool | ⚠️ | POST /api/contrataciones | LOTE 3 - verificar |
| 22 | `nuevaContratacionTemporal()` | DetalleContrataciones | bool | ⚠️ | POST /api/contrataciones/{id}/detalles | LOTE 3 - verificar |
| 23 | `actualizarContratacion()` | DetalleContrataciones | bool | ⚠️ | PUT /api/contrataciones/{id}/detalles/{detalleId} | LOTE 3 - verificar |
| 24 | `calificarContratacion()` | contratacionID, calificacionID | bool | ❌ | N/A | **FALTA: POST /api/contrataciones/{id}/calificar** |
| 25 | `modificarCalificacionDeContratacion()` | Calificaciones | bool | ❌ | N/A | **FALTA: PUT /api/calificaciones/{id}** (verificar con CalificacionesController) |
| 26 | `obtenerFichaTemporales()` | contratacionID, userID | EmpleadosTemporales | ⚠️ | GET /api/contrataciones/{id} | LOTE 3 - verificar |
| 27 | `obtenerTodosLosTemporales()` | userID | List<EmpleadosTemporales> | ⚠️ | GET /api/contrataciones | LOTE 3 - verificar |
| 28 | `obtenerVistaTemporal()` | contratacionID, userID | VContratacionesTemporales | ❌ | N/A | **FALTA: GET /api/contrataciones/{id}/vista** (view con datos completos) |
| 29 | `consultarPadron()` | cedula | PadronModel (async) | ❌ | N/A | **FALTA: GET /api/empleados/consultar-padron/{cedula}** (API externa JCE) |
| 30 | `guardarOtrasRemuneraciones()` | List<Remuneraciones> | bool | ❌ | N/A | **FALTA: POST /api/empleados/{id}/remuneraciones/batch** |
| 31 | `actualizarRemuneraciones()` | List<Remuneraciones>, empleadoID | bool | ❌ | N/A | **FALTA: PUT /api/empleados/{id}/remuneraciones/batch** |
| 32 | `deducciones()` | - | List<Deducciones_TSS> | ❌ | N/A | **FALTA: GET /api/catalogos/deducciones-tss** (catálogo) |

**Prioridad:** 🔴 CRÍTICA (Módulo más grande del sistema)  
**Tiempo Estimado Faltantes:** 15-20 horas

---

### 3️⃣ SuscripcionesService.cs (17 métodos)

**Ubicación:** `Codigo Fuente Mi Gente/MiGente_Front/Services/SuscripcionesService.cs`  
**Propósito:** Registro de usuarios, suscripciones, planes

| # | Método Legacy | Parámetros | Retorna | Status API | Endpoint API | Notas |
|---|---------------|------------|---------|------------|--------------|-------|
| 1 | `GuardarPerfil()` | Cuentas, host, email | bool | ✅ | POST /api/auth/register | LOTE 1 - incluye envío de email activación |
| 2 | `guardarNuevoContratista()` | Contratistas | bool | ✅ | POST /api/contratistas | LOTE 2 |
| 3 | `enviarCorreoActivacion()` | host, email, Cuentas?, userID? | void | ✅ | Internal (EmailService) | Migrado en Infrastructure |
| 4 | `guardarCredenciales()` | Credenciales | bool | ✅ | POST /api/auth/register | Parte del registro |
| 5 | `actualizarPass()` | Credenciales | bool | ✅ | PUT /api/auth/change-password | LOTE 1 |
| 6 | `actualizarCredenciales()` | Credenciales | bool | ⚠️ | PUT /api/auth/change-password | Verificar si incluye activo + email |
| 7 | `obtenerCedula()` | userID | string | ❌ | N/A | **FALTA: GET /api/contratistas/{userId}/cedula** |
| 8 | `actualizarPassByID()` | Credenciales | bool | ❌ | N/A | **FALTA: PUT /api/auth/credentials/{id}/password** |
| 9 | `validarCorreo()` | correo | Cuentas | ✅ | GET /api/auth/validar-correo | LOTE 1 (retorna bool, no entity) |
| 10 | `validarCorreoCuentaActual()` | correo, userID | Cuentas | ❌ | N/A | **FALTA: GET /api/auth/validar-correo?userID={id}** (ignora cuenta actual) |
| 11 | `obtenerSuscripcion()` | userID | ObtenerSuscripcion_Result | ⚠️ | GET /api/suscripciones/{userId} | LOTE 2 - verificar |
| 12 | `actualizarSuscripcion()` | Suscripciones | Suscripciones | ⚠️ | PUT /api/suscripciones/{id} | LOTE 2 - verificar |
| 13 | `obtenerPlanes()` | - | List<Planes_empleadores> | ✅ | GET /api/suscripciones/planes-empleadores | LOTE 2 |
| 14 | `obtenerPlanesContratistas()` | - | List<Planes_Contratistas> | ✅ | GET /api/suscripciones/planes-contratistas | LOTE 2 |
| 15 | `procesarVenta()` | Ventas | bool | ⚠️ | POST /api/pagos/ventas | LOTE 2 - verificar |
| 16 | `guardarSuscripcion()` | Suscripciones | bool | ⚠️ | POST /api/suscripciones | LOTE 2 - verificar |
| 17 | `obtenerDetalleVentasBySuscripcion()` | userID | List<Ventas> | ❌ | N/A | **FALTA: GET /api/suscripciones/{userId}/ventas** |

**Prioridad:** 🟠 ALTA (Monetización del sistema)  
**Tiempo Estimado Faltantes:** 6-8 horas

---

### 4️⃣ ContratistasService.cs (10 métodos)

**Ubicación:** `Codigo Fuente Mi Gente/MiGente_Front/Services/ContratistasService.cs`  
**Propósito:** Gestión de perfiles de contratistas

| # | Método Legacy | Parámetros | Retorna | Status API | Endpoint API | Notas |
|---|---------------|------------|---------|------------|--------------|-------|
| 1 | `getTodasUltimos20()` | - | List<VContratistas> | ⚠️ | GET /api/contratistas?top=20 | LOTE 2 - verificar filtro activos |
| 2 | `getMiPerfil()` | userID | VContratistas | ⚠️ | GET /api/contratistas/mi-perfil | LOTE 2 - verificar |
| 3 | `getServicios()` | contratistaID | List<Contratistas_Servicios> | ❌ | N/A | **FALTA: GET /api/contratistas/{id}/servicios** |
| 4 | `agregarServicio()` | Contratistas_Servicios | bool | ❌ | N/A | **FALTA: POST /api/contratistas/{id}/servicios** |
| 5 | `removerServicio()` | servicioID, contratistaID | bool | ❌ | N/A | **FALTA: DELETE /api/contratistas/{id}/servicios/{servicioId}** |
| 6 | `GuardarPerfil()` | Contratistas, userID | bool | ⚠️ | PUT /api/contratistas/{id} | LOTE 2 - verificar campos |
| 7 | `ActivarPerfil()` | userID | bool | ❌ | N/A | **FALTA: POST /api/contratistas/{id}/activar** |
| 8 | `DesactivarPerfil()` | userID | bool | ❌ | N/A | **FALTA: POST /api/contratistas/{id}/desactivar** |
| 9 | `getConCriterio()` | palabrasClave, zona | List<VContratistas> | ⚠️ | GET /api/contratistas/buscar?q={}&zona={} | LOTE 2 - verificar filtros |
| 10 | N/A (CRUD básico) | - | - | ✅ | GET/POST/PUT/DELETE /api/contratistas | LOTE 2 |

**Prioridad:** 🟠 ALTA (Marketplace de servicios)  
**Tiempo Estimado Faltantes:** 5-6 horas

---

### 5️⃣ CalificacionesService.cs (4 métodos) ✅ COMPLETO

**Ubicación:** `Codigo Fuente Mi Gente/MiGente_Front/Services/CalificacionesService.cs`  
**Propósito:** Sistema de calificaciones entre empleadores y contratistas

| # | Método Legacy | Parámetros | Retorna | Status API | Endpoint API | Notas |
|---|---------------|------------|---------|------------|--------------|-------|
| 1 | `getTodas()` | - | List<VCalificaciones> | ✅ | GET /api/calificaciones | LOTE 4 |
| 2 | `getById()` | id, userID? | List<VCalificaciones> | ✅ | GET /api/calificaciones/{identificacion} | LOTE 4 |
| 3 | `getCalificacionByID()` | calificacionID | Calificaciones | ✅ | GET /api/calificaciones/{id} | LOTE 4 |
| 4 | `calificarPerfil()` | Calificaciones | Calificaciones | ✅ | POST /api/calificaciones | LOTE 4 |

**Prioridad:** 🟢 BAJA (Ya completo)  
**Tiempo Estimado:** 0 horas ✅

---

### 6️⃣ PaymentService.cs (3 métodos) ✅ COMPLETO

**Ubicación:** `Codigo Fuente Mi Gente/MiGente_Front/Services/PaymentService.cs`  
**Propósito:** Integración con Cardnet Payment Gateway

| # | Método Legacy | Parámetros | Retorna | Status API | Endpoint API | Notas |
|---|---------------|------------|---------|------------|--------------|-------|
| 1 | `consultarIdempotency()` | url | dynamic | ✅ | Infrastructure Layer | LOTE 2 |
| 2 | `Payment()` | cardNumber, cvv, amount, clientIP, expirationDate, referenceNumber, invoiceNumber | PaymentResponse | ✅ | POST /api/pagos/procesar | LOTE 2 |
| 3 | `getPaymentParameters()` | - | PaymentGateway | ✅ | Infrastructure Layer | Config desde DB |

**Prioridad:** 🟢 BAJA (Ya completo)  
**Tiempo Estimado:** 0 horas ✅

---

### 7️⃣ EmailService.cs (1 método) ✅ COMPLETO

**Ubicación:** `Codigo Fuente Mi Gente/MiGente_Front/Services/EmailService.cs`  
**Propósito:** Obtener configuración SMTP desde DB

| # | Método Legacy | Parámetros | Retorna | Status API | Endpoint API | Notas |
|---|---------------|------------|---------|------------|--------------|-------|
| 1 | `Config_Correo()` | - | Config_Correo | ✅ | Infrastructure Layer | IEmailService implementado |

**Prioridad:** 🟢 BAJA (Ya completo)  
**Tiempo Estimado:** 0 horas ✅

---

### 8️⃣ BotServices.cs (1 método)

**Ubicación:** `Codigo Fuente Mi Gente/MiGente_Front/Services/BotServices.cs`  
**Propósito:** Obtener configuración OpenAI para bot

| # | Método Legacy | Parámetros | Retorna | Status API | Endpoint API | Notas |
|---|---------------|------------|---------|------------|--------------|-------|
| 1 | `getOpenAI()` | - | OpenAi_Config | ❌ | N/A | **FALTA: GET /api/configuracion/openai** (o Infrastructure) |

**Prioridad:** 🟡 MEDIA (Bot virtual no crítico)  
**Tiempo Estimado:** 1 hora

---

### 9️⃣ Utilitario.cs (2 métodos)

**Ubicación:** `Codigo Fuente Mi Gente/MiGente_Front/Services/Utilitario.cs`  
**Propósito:** Utilidades de conversión (imágenes, HTML to PDF)

| # | Método Legacy | Parámetros | Retorna | Status API | Endpoint API | Notas |
|---|---------------|------------|---------|------------|--------------|-------|
| 1 | `ObtenerImagenComoDataUrl()` | id | string (data URL) | ❌ | N/A | **Evaluar si es necesario** (legacy, no se usa?) |
| 2 | `ConvertHtmlToPdf()` | htmlContent | byte[] | ✅ | Infrastructure Layer | Ya existe PDF generation service |

**Prioridad:** 🟢 BAJA (Solo 1 falta, puede que no se use)  
**Tiempo Estimado:** 0-1 hora

---

## 🎯 PLAN DE IMPLEMENTACIÓN: LOTES DE CIERRE

### LOTE 6.0.1: Authentication Completion (3-4 horas) 🔴 CRÍTICO

**Objetivos:**

- Completar todos los métodos del LoginService que faltan
- Asegurar paridad 100% con Legacy Authentication

**Endpoints a Implementar:**

1. **DELETE /api/auth/users/{userId}/credentials/{credentialId}**
   - Command: `DeleteUserCredentialCommand`
   - Handler: Eliminar credencial específica de usuario
   - Validations: Usuario debe tener al menos 1 credencial activa
   - Security: Solo el propio usuario o admin

2. **POST /api/auth/profile-info**
   - Command: `AddProfileInfoCommand`
   - Handler: Agregar registro a tabla `perfilesInfo`
   - Entity: perfilesInfo (verificar estructura en Domain)

3. **GET /api/auth/cuenta/{cuentaId}**
   - Query: `GetCuentaByIdQuery`
   - Handler: Retornar entidad Cuentas completa
   - Response: CuentaDto

4. **PUT /api/auth/profile (MEJORAR)**
   - Modificar `UpdateProfileCommand` para incluir perfilesInfo
   - Handler: Actualizar ambas tablas: Cuentas + perfilesInfo
   - Nota: Legacy usa 2 DbContexts separados (anti-pattern), Clean usa UnitOfWork

**Testing:**

- Unit tests para Commands/Queries
- Integration tests con AuthController
- Validar con Swagger UI

---

### LOTE 6.0.2: Empleados - Remuneraciones & TSS (4-5 horas) 🟠 ALTA

**Objetivos:**

- Gestión de otras remuneraciones (horas extras, bonos, etc.)
- Consulta a Padrón Electoral (API externa JCE)
- Catálogo de deducciones TSS

**Endpoints a Implementar:**

1. **GET /api/empleados/{id}/remuneraciones**
   - Query: `GetRemuneracionesByEmpleadoQuery`
   - Response: List<RemuneracionDto>

2. **DELETE /api/remuneraciones/{id}**
   - Command: `DeleteRemuneracionCommand`
   - Security: Verificar userId del empleado

3. **POST /api/empleados/{id}/remuneraciones/batch**
   - Command: `AddRemuneracionesBatchCommand`
   - Input: List<RemuneracionDto>

4. **PUT /api/empleados/{id}/remuneraciones/batch**
   - Command: `UpdateRemuneracionesBatchCommand`
   - Logic: DELETE all + INSERT new (mismo patrón Legacy)

5. **GET /api/empleados/consultar-padron/{cedula}**
   - Query: `ConsultarPadronQuery`
   - External API: <https://abcportal.online/Sigeinfo/public/api>
   - Response: PadronDto (nombre, apellido, fecha nacimiento, etc.)
   - Security: Requiere auth headers

6. **GET /api/catalogos/deducciones-tss**
   - Query: `GetDeduccionesTssQuery`
   - Response: List<DeduccionTssDto>
   - Cache: Considerar caching (datos no cambian frecuentemente)

**Dependencias Externas:**

- RestSharp o HttpClient para API Padrón
- Credentials para API (username, password en appsettings)

---

### LOTE 6.0.3: Contrataciones Temporales (8-10 horas) 🔴 CRÍTICA

**Objetivos:**

- Gestión completa de empleados temporales (personas físicas y empresas)
- Pagos por contratación
- Calificaciones de contratos

**Endpoints a Implementar:**

1. **GET /api/contrataciones/{id}/detalle/{detalleId}/pagos**
   - Query: `GetPagosContratacionQuery`
   - Response: List<PagoContratacionDto>

2. **GET /api/contrataciones/recibos/{pagoId}**
   - Query: `GetReciboContratacionByPagoIdQuery`
   - Response: ReciboContratacionDto (header + detalles)

3. **POST /api/contrataciones/{id}/detalle/{detalleId}/cancelar**
   - Command: `CancelarTrabajoCommand`
   - Logic: Set estatus = 3 (Cancelado)

4. **DELETE /api/contrataciones/recibos/{pagoId}**
   - Command: `DeleteReciboContratacionCommand`
   - Logic: DELETE detalle + header (2 transacciones)

5. **DELETE /api/contrataciones/{id}**
   - Command: `DeleteEmpleadoTemporalCommand`
   - Logic: CASCADE delete recibos → detalles → contratación
   - ⚠️ Complex: Múltiples tablas relacionadas

6. **POST /api/contrataciones/{id}/calificar**
   - Command: `CalificarContratacionCommand`
   - Input: calificacionID
   - Logic: Set calificado=true, calificacionID en DetalleContrataciones

7. **GET /api/contrataciones/{id}/vista**
   - Query: `GetVistaContratacionQuery`
   - Response: VContratacionesTemporalesDto (vista completa con todos los datos)

8. **POST /api/contrataciones/procesar-pago**
   - Command: `ProcesarPagoContratacionCommand`
   - Input: header, detalles
   - Logic:
     - Insert header
     - Insert detalles (con pagoID generado)
     - Si es "Pago Final" → Update estatus DetalleContrataciones = 2

**Validaciones:**

- VERIFICAR endpoints actuales en ContratacionesController (LOTE 3)
- Comparar con Legacy logic (especialmente cascade deletes)
- Testing exhaustivo (muchas relaciones)

---

### LOTE 6.0.4: Contratistas - Servicios & Activación (5-6 horas) 🟠 ALTA

**Objetivos:**

- Gestión de servicios ofrecidos por contratistas
- Activación/desactivación de perfiles

**Endpoints a Implementar:**

1. **GET /api/contratistas/{id}/servicios**
   - Query: `GetServiciosContratistaQuery`
   - Response: List<ServicioDto>

2. **POST /api/contratistas/{id}/servicios**
   - Command: `AddServicioCommand`
   - Input: ServicioDto (nombre, descripción, precio?)

3. **DELETE /api/contratistas/{id}/servicios/{servicioId}**
   - Command: `RemoveServicioCommand`
   - Validations: Verificar ownership

4. **POST /api/contratistas/{id}/activar**
   - Command: `ActivarPerfilContratistaCommand`
   - Logic: Set activo=true

5. **POST /api/contratistas/{id}/desactivar**
   - Command: `DesactivarPerfilContratistaCommand`
   - Logic: Set activo=false

**Mejoras sobre Legacy:**

- Agregar endpoint: GET /api/contratistas/{userId}/cedula (falta en Legacy)

---

### LOTE 6.0.5: Suscripciones - Gestión Avanzada (4-5 horas) 🟡 MEDIA

**Objetivos:**

- Completar gestión de suscripciones y ventas

**Endpoints a Implementar:**

1. **GET /api/auth/credentials/{id}/password**
   - Command: `UpdatePasswordByIdCommand`
   - Input: Credencial ID (no userID)

2. **GET /api/auth/validar-correo?userID={id}**
   - Query: `ValidarCorreoExcludingUserQuery`
   - Logic: Check email existe pero excluyendo cuenta actual
   - Use case: Validar en update profile

3. **GET /api/suscripciones/{userId}/ventas**
   - Query: `GetVentasByUserIdQuery`
   - Response: List<VentaDto> (historial de compras)

**Mejoras sobre Legacy:**

- Agregar endpoint: GET /api/contratistas/{userId}/cedula

---

### LOTE 6.0.6: Bot & Configuración (2-3 horas) 🟡 MEDIA

**Objetivos:**

- Configuración OpenAI para bot virtual
- Endpoints de configuración general

**Endpoints a Implementar:**

1. **GET /api/configuracion/openai**
   - Query: `GetOpenAiConfigQuery`
   - Response: OpenAiConfigDto (API key, model, temperature)
   - Security: Solo admin

Alternativa: Mover a Infrastructure Layer como IOpenAiService

---

### LOTE 6.0.7: Testing & Validación (6-8 horas) ✅ OBLIGATORIO

**Objetivos:**

- Asegurar paridad 100% con Legacy
- Validar TODOS los endpoints con datos reales
- Documentación Swagger completa

**Actividades:**

1. **Unit Testing**
   - Commands: Validations, Business logic
   - Queries: Filtros, Joins, Projections
   - Target: 80%+ code coverage

2. **Integration Testing**
   - Controllers: Request → Response flow
   - Authorization: Role-based access
   - Validation: FluentValidation rules

3. **Manual Testing con Swagger UI**
   - Crear checklist de todos los endpoints
   - Probar con datos reales de DB
   - Comparar respuestas con Legacy

4. **Performance Testing**
   - Queries N+1 (usar .Include apropiadamente)
   - Índices de base de datos
   - Response times < 500ms

5. **Security Testing**
   - Authorization checks
   - SQL injection prevention (EF Core)
   - Input validation

6. **Documentation**
   - Swagger annotations completas
   - XML comments en todos los endpoints
   - Postman collection para QA

---

## 📊 MÉTRICAS DE COMPLETITUD

### Por Módulo

| Módulo | Métodos Legacy | Endpoints API | % Completo | Horas Restantes |
|--------|----------------|---------------|------------|-----------------|
| **Authentication** | 11 | 10 | 55% → **100%** | 3-4h |
| **Empleados & Nómina** | 32 | 32 | 38% → **100%** | 12-15h |
| **Contratistas** | 10 | 10 | 50% → **100%** | 5-6h |
| **Suscripciones** | 17 | 17 | 29% → **100%** | 4-5h |
| **Calificaciones** | 4 | 5 | ✅ 100% | 0h |
| **Pagos (Cardnet)** | 3 | 3 | ✅ 100% | 0h |
| **Email** | 1 | 1 | ✅ 100% | 0h |
| **Bot** | 1 | 1 | 0% → **100%** | 2-3h |
| **Testing & Docs** | - | - | 0% → **100%** | 6-8h |
| **TOTAL** | **81** | **81** | **43% → 100%** | **32-46h** |

### Timeline Estimado

**Sprint Actual (2 semanas):**

- Semana 1: LOTEs 6.0.1 + 6.0.2 + 6.0.4 (12-15h)
- Semana 2: LOTEs 6.0.3 + 6.0.5 + 6.0.6 (14-18h)

**Sprint Siguiente (1 semana):**

- LOTE 6.0.7: Testing & Validación (6-8h)
- Buffer para ajustes y bugs (4-6h)

**TOTAL: 3 semanas para 100% paridad backend**

---

## ✅ CRITERIOS DE ACEPTACIÓN

### Para Cada Endpoint

- [ ] Command/Query implementado con Handler
- [ ] FluentValidation rules (si aplica)
- [ ] Controller endpoint con [Http*] attribute
- [ ] Swagger documentation (XML comments)
- [ ] Authorization attributes (si requiere autenticación)
- [ ] Unit test del Handler
- [ ] Integration test del Controller
- [ ] Manual test con Swagger UI (screenshot de success)
- [ ] Comparación con Legacy (mismo input → mismo output)

### Para Todo el Backend

- [ ] 0 métodos Legacy sin equivalente en API
- [ ] 100% de endpoints documentados en Swagger
- [ ] 80%+ code coverage en tests
- [ ] 0 errores de compilación
- [ ] 0 warnings críticos
- [ ] Performance: p95 < 500ms en queries
- [ ] Security: All endpoints protected con [Authorize]
- [ ] Logging: Todas las operaciones logueadas

---

## 🚨 RIESGOS IDENTIFICADOS

### 1. Complejidad de Contrataciones Temporales (LOTE 6.0.3)

**Problema:** Múltiples tablas relacionadas, cascade deletes complejos  
**Mitigación:**

- Testing exhaustivo en ambiente de QA
- Transacciones explícitas
- Rollback en caso de error

### 2. API Externa Padrón Electoral

**Problema:** Dependencia de servicio externo, puede estar caído  
**Mitigación:**

- Timeout de 10 segundos
- Retry logic (Polly)
- Cache de resultados previos
- Endpoint opcional (no bloquea creación de empleado)

### 3. Paridad Exacta con Legacy Codes

**Problema:** Legacy retorna códigos numéricos (2=success, 0=invalid, -1=inactive)  
**Mitigación:**

- Mantener códigos en DTOs para compatibilidad
- Documentar significado en XML comments
- Frontend puede migrar gradualmente a status codes HTTP

### 4. Multiple DbContext Pattern (Anti-pattern Legacy)

**Problema:** Legacy usa múltiples instancias de DbContext en mismo método  
**Solución Clean:**

- Usar UnitOfWork pattern
- Single DbContext per request (Scoped)
- Transacciones explícitas si necesario

---

## 📝 NOTAS PARA IMPLEMENTACIÓN

### Patrones Legacy vs Clean

| Aspecto | Legacy (Web Forms) | Clean Architecture |
|---------|-------------------|-------------------|
| **Data Access** | Direct DbContext per method | Repository + UnitOfWork |
| **Transactions** | Multiple DbContext instances | Single DbContext + explicit transactions |
| **Validation** | Manual if checks | FluentValidation |
| **Error Handling** | Return codes (0, -1, 2) | Exceptions + Result pattern |
| **Async** | Sync methods (some async) | 100% async/await |
| **Logging** | Console.WriteLine | Serilog structured logging |
| **Authentication** | FormsAuth + Cookies | JWT + Bearer tokens |
| **Authorization** | Cookie checks in code | [Authorize] attributes + Policies |

### Conversión de Retornos Legacy

```csharp
// Legacy
public int login(string email, string pass)
{
    if (!valid) return 0;      // Invalid credentials
    if (!active) return -1;     // Inactive user
    return 2;                   // Success
}

// Clean Architecture
public async Task<AuthenticationResultDto> Login(LoginCommand command)
{
    // Throw exceptions for errors
    if (!valid) throw new UnauthorizedException("Credenciales inválidas");
    if (!active) throw new UnauthorizedException("Usuario inactivo");
    
    // Return success DTO
    return new AuthenticationResultDto { ... };
}

// Controller maneja excepciones y retorna HTTP status codes
```

---

## 🎯 SIGUIENTE PASO INMEDIATO

**ACCIÓN:** Implementar **LOTE 6.0.1: Authentication Completion**

**Comando para empezar:**

```bash
# 1. Crear rama
git checkout -b feature/lote-6.0.1-authentication-completion

# 2. Crear archivos Commands
mkdir -p src/Core/MiGenteEnLinea.Application/Features/Authentication/Commands/DeleteUserCredential
mkdir -p src/Core/MiGenteEnLinea.Application/Features/Authentication/Commands/AddProfileInfo

# 3. Crear archivos Queries
mkdir -p src/Core/MiGenteEnLinea.Application/Features/Authentication/Queries/GetCuentaById

# 4. Iniciar implementación
code .
```

---

**Última Actualización:** 2025-01-15 12:30 PM  
**Próxima Revisión:** Después de completar LOTE 6.0.1  
**Responsable:** AI Agent + Usuario

---
