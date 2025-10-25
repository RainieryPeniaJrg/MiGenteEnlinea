# 🎯 PLAN DE INTEGRACIÓN API - ANÁLISIS LEGACY vs CLEAN

**Fecha:** 24 de Octubre 2025  
**Proyecto:** MiGente En Línea - Migración Clean Architecture  
**Estado API Backend:** ✅ Funcionando en <http://localhost:5015>  
**Base de Datos:** ✅ Conectada a PersonalPC (MiGenteDev)  

---

## 📊 RESUMEN EJECUTIVO

### Servicios Legacy Analizados (9 servicios, ~95 métodos)

| # | Servicio Legacy | Métodos | Complejidad | Prioridad |
|---|----------------|---------|-------------|-----------|
| 1 | **LoginService.asmx.cs** | 13 | 🟡 MEDIA | 🔴 CRÍTICA |
| 2 | **EmpleadosService.cs** | 42 | 🔴 ALTA | 🟠 ALTA |
| 3 | **ContratistasService.cs** | 10 | 🟢 BAJA | 🟠 ALTA |
| 4 | **SuscripcionesService.cs** | 19 | 🟡 MEDIA | 🔴 CRÍTICA |
| 5 | **CalificacionesService.cs** | 4 | 🟢 BAJA | 🟡 MEDIA |
| 6 | **PaymentService.cs** | 3 | 🟡 MEDIA | 🔴 CRÍTICA |
| 7 | **EmailService.cs** | 1 | 🟢 BAJA | 🟢 BAJA |
| 8 | **BotServices.cs** | 1 | 🟢 BAJA | 🟢 BAJA |
| 9 | **Utilitario.cs** | ~5 (estimado) | 🟢 BAJA | 🟢 BAJA |

### Controllers Clean Implementados (12 controllers)

| # | Controller Clean | Endpoints | Estado |
|---|-----------------|-----------|--------|
| 1 | **AuthController** | ~8 | ✅ IMPLEMENTADO |
| 2 | **EmpleadosController** | ~15 | ⚠️ PARCIAL (70%) |
| 3 | **EmpleadoresController** | ~10 | ⚠️ PARCIAL (60%) |
| 4 | **ContratistasController** | ~12 | ⚠️ PARCIAL (80%) |
| 5 | **SuscripcionesController** | ~8 | ⚠️ PARCIAL (50%) |
| 6 | **CalificacionesController** | ~6 | ✅ IMPLEMENTADO |
| 7 | **PagosController** | ~5 | ❌ MOCK (30%) |
| 8 | **NominasController** | ~10 | ⚠️ PARCIAL (60%) |
| 9 | **DashboardController** | ~5 | ✅ IMPLEMENTADO |
| 10 | **ContratacionesController** | ~8 | ⚠️ PARCIAL (70%) |
| 11 | **ConfiguracionController** | ~4 | ✅ IMPLEMENTADO |
| 12 | **WeatherForecastController** | 1 | ⚠️ DEMO (eliminar) |

### 📈 Estadísticas de Implementación

- **Total métodos Legacy:** ~98 métodos  
- **Total endpoints Clean:** ~92 endpoints  
- **Endpoints implementados:** ~65 (71%)  
- **🚨 GAPS IDENTIFICADOS:** **27 funcionalidades faltantes o parciales**  

---

## 🗺️ MAPEO DETALLADO: LEGACY → CLEAN

### 1️⃣ LoginService.asmx.cs → AuthController

| Método Legacy | Parámetros | Endpoint Clean | HTTP | Estado |
|---------------|------------|----------------|------|--------|
| `login()` | email, pass | `/api/auth/login` | POST | ✅ IMPLEMENTADO |
| `borrarUsuario()` | userID, credencialID | ❌ FALTANTE | DELETE | ❌ NO EXISTE |
| `obtenerPerfil()` | userID | `/api/auth/perfil/{userID}` | GET | ✅ IMPLEMENTADO |
| `obtenerPerfilByEmail()` | email | `/api/auth/perfil/by-email/{email}` | GET | ✅ IMPLEMENTADO |
| `obtenerCredenciales()` | userID | `/api/auth/credenciales/{userID}` | GET | ✅ IMPLEMENTADO |
| `actualizarPerfil()` | perfilesInfo, Cuentas | `/api/auth/perfil` | PUT | ⚠️ PARCIAL (solo Cuenta) |
| `actualizarPerfil1()` | Cuentas | `/api/auth/perfil` | PUT | ✅ IMPLEMENTADO |
| `agregarPerfilInfo()` | perfilesInfo | ❌ FALTANTE | POST | ❌ NO EXISTE |
| `getPerfilByID()` | cuentaID | ❌ FALTANTE | GET | ❌ NO EXISTE |
| `validarCorreo()` | correo | `/api/auth/validate-email/{email}` | GET | ✅ IMPLEMENTADO |
| `getPerfilInfo()` | userID (Guid) | `/api/auth/perfil/{userID}` | GET | ✅ IMPLEMENTADO (duplicado) |

**GAPS IDENTIFICADOS:**

- ❌ **GAP-001:** `borrarUsuario()` - Delete user functionality missing
- ❌ **GAP-002:** `agregarPerfilInfo()` - Add PerfilesInfo separately
- ❌ **GAP-003:** `getPerfilByID()` - Get profile by cuentaID (not userID)
- ⚠️ **GAP-004:** `actualizarPerfil()` - Missing PerfilesInfo update logic

---

### 2️⃣ EmpleadosService.cs → EmpleadosController + NominasController

| Método Legacy | Parámetros | Endpoint Clean | HTTP | Estado |
|---------------|------------|----------------|------|--------|
| `getEmpleados()` | userID | `/api/empleados` | GET | ✅ IMPLEMENTADO |
| `getVEmpleados()` | userID | `/api/empleados/vista` | GET | ✅ IMPLEMENTADO |
| `getContrataciones()` | userID | `/api/contrataciones` | GET | ✅ IMPLEMENTADO |
| `getEmpleadosByID()` | userID, id | `/api/empleados/{id}` | GET | ✅ IMPLEMENTADO |
| `obtenerRemuneraciones()` | userID, empleadoID | `/api/empleados/{id}/remuneraciones` | GET | ✅ IMPLEMENTADO |
| `quitarRemuneracion()` | userID, id | `/api/empleados/remuneraciones/{id}` | DELETE | ✅ IMPLEMENTADO |
| `guardarEmpleado()` | Empleados | `/api/empleados` | POST | ✅ IMPLEMENTADO |
| `actualizarEmpleado()` | Empleados | `/api/empleados/{id}` | PUT | ✅ IMPLEMENTADO (método 1) |
| `ActualizarEmpleado()` | Empleados | `/api/empleados/{id}` | PUT | ✅ IMPLEMENTADO (método 2) |
| `procesarPago()` | header, detalle | `/api/nominas/procesar-pago` | POST | ✅ IMPLEMENTADO |
| `procesarPagoContratacion()` | header, detalle | `/api/contrataciones/procesar-pago` | POST | ⚠️ PARCIAL (sin update estatus) |
| `GetEmpleador_Recibos_Empleado()` | userID, empleadoID | `/api/nominas/empleado/{empleadoID}/recibos` | GET | ✅ IMPLEMENTADO |
| `GetEmpleador_ReciboByPagoID()` | pagoID | `/api/nominas/recibo/{pagoID}` | GET | ✅ IMPLEMENTADO |
| `GetContratacion_ReciboByPagoID()` | pagoID | `/api/contrataciones/recibo/{pagoID}` | GET | ✅ IMPLEMENTADO |
| `cancelarTrabajo()` | contratacionID, detalleID | `/api/contrataciones/{contratacionID}/detalle/{detalleID}/cancelar` | POST | ⚠️ PARCIAL |
| `eliminarReciboEmpleado()` | pagoID | `/api/nominas/recibo/{pagoID}` | DELETE | ✅ IMPLEMENTADO |
| `eliminarReciboContratacion()` | pagoID | `/api/contrataciones/recibo/{pagoID}` | DELETE | ✅ IMPLEMENTADO |
| `eliminarEmpleadoTemporal()` | contratacionID | `/api/contrataciones/{contratacionID}` | DELETE | ⚠️ PARCIAL (cascade) |
| `GetEmpleador_RecibosContratacionesByID()` | contratacionID, detalleID | `/api/contrataciones/{contratacionID}/detalle/{detalleID}/recibos` | GET | ✅ IMPLEMENTADO |
| `darDeBaja()` | empleadoID, userID, fechaBaja, prestaciones, motivo | `/api/empleados/{id}/dar-baja` | POST | ✅ IMPLEMENTADO |
| `nuevoTemporal()` | EmpleadosTemporales, DetalleContrataciones | `/api/contrataciones` | POST | ✅ IMPLEMENTADO |
| `nuevaContratacionTemporal()` | DetalleContrataciones | `/api/contrataciones/{id}/detalle` | POST | ✅ IMPLEMENTADO |
| `actualizarContratacion()` | DetalleContrataciones | `/api/contrataciones/{id}/detalle/{detalleID}` | PUT | ✅ IMPLEMENTADO |
| `calificarContratacion()` | contratacionID, calificacionID | `/api/contrataciones/{id}/calificar` | POST | ✅ IMPLEMENTADO |
| `modificarCalificacionDeContratacion()` | Calificaciones | `/api/calificaciones/{id}` | PUT | ✅ IMPLEMENTADO |
| `obtenerFichaTemporales()` | contratacionID, userID | `/api/contrataciones/{id}` | GET | ✅ IMPLEMENTADO |
| `obtenerTodosLosTemporales()` | userID | `/api/contrataciones` | GET | ✅ IMPLEMENTADO |
| `obtenerVistaTemporal()` | contratacionID, userID | `/api/contrataciones/{id}/vista` | GET | ✅ IMPLEMENTADO |
| `consultarPadron()` | cedula | `/api/empleados/consultar-padron/{cedula}` | GET | ✅ IMPLEMENTADO (PadronService) |
| `guardarOtrasRemuneraciones()` | List<Remuneraciones> | `/api/empleados/remuneraciones/batch` | POST | ❌ FALTANTE |
| `actualizarRemuneraciones()` | List<Remuneraciones>, empleadoID | `/api/empleados/{id}/remuneraciones` | PUT | ❌ FALTANTE (replace all) |
| `deducciones()` | - | `/api/configuracion/deducciones-tss` | GET | ✅ IMPLEMENTADO |

**GAPS IDENTIFICADOS:**

- ⚠️ **GAP-005:** `procesarPagoContratacion()` - Falta lógica de update `detalle.estatus = 2` cuando `Concepto == "Pago Final"`
- ⚠️ **GAP-006:** `cancelarTrabajo()` - Implementar lógica para cambiar `estatus = 3`
- ⚠️ **GAP-007:** `eliminarEmpleadoTemporal()` - Cascade delete no está completo (recibos → detalles → empleado temporal)
- ❌ **GAP-008:** `guardarOtrasRemuneraciones()` - Batch insert remuneraciones missing
- ❌ **GAP-009:** `actualizarRemuneraciones()` - Replace all remuneraciones (delete → insert)

---

### 3️⃣ ContratistasService.cs → ContratistasController

| Método Legacy | Parámetros | Endpoint Clean | HTTP | Estado |
|---------------|------------|----------------|------|--------|
| `getTodasUltimos20()` | - | `/api/contratistas/ultimos` | GET | ✅ IMPLEMENTADO |
| `getMiPerfil()` | userID | `/api/contratistas/perfil` | GET | ✅ IMPLEMENTADO |
| `getServicios()` | contratistaID | `/api/contratistas/{id}/servicios` | GET | ✅ IMPLEMENTADO |
| `agregarServicio()` | Contratistas_Servicios | `/api/contratistas/{id}/servicios` | POST | ✅ IMPLEMENTADO |
| `removerServicio()` | servicioID, contratistaID | `/api/contratistas/{contratistaID}/servicios/{servicioID}` | DELETE | ✅ IMPLEMENTADO |
| `GuardarPerfil()` | Contratistas, userID | `/api/contratistas/perfil` | PUT | ✅ IMPLEMENTADO |
| `ActivarPerfil()` | userID | `/api/contratistas/perfil/activar` | POST | ✅ IMPLEMENTADO |
| `DesactivarPerfil()` | userID | `/api/contratistas/perfil/desactivar` | POST | ✅ IMPLEMENTADO |
| `getConCriterio()` | palabrasClave, zona | `/api/contratistas/buscar?q={palabrasClave}&zona={zona}` | GET | ✅ IMPLEMENTADO |

**GAPS IDENTIFICADOS:**

- ✅ **Completamente implementado** (100%)

---

### 4️⃣ SuscripcionesService.cs → SuscripcionesController + AuthController

| Método Legacy | Parámetros | Endpoint Clean | HTTP | Estado |
|---------------|------------|----------------|------|--------|
| `GuardarPerfil()` | Cuentas, host, email | `/api/auth/register` | POST | ⚠️ PARCIAL (sin Contratistas auto-create) |
| `guardarNuevoContratista()` | Contratistas | `/api/contratistas` | POST | ✅ IMPLEMENTADO |
| `enviarCorreoActivacion()` | host, email, Cuentas, userID | `/api/auth/resend-activation` | POST | ⚠️ PARCIAL |
| `guardarCredenciales()` | Credenciales | `/api/auth/register` | POST | ✅ IMPLEMENTADO (incluido) |
| `actualizarPass()` | Credenciales | `/api/auth/change-password` | POST | ✅ IMPLEMENTADO |
| `actualizarCredenciales()` | Credenciales | `/api/auth/credenciales/{id}` | PUT | ❌ FALTANTE |
| `obtenerCedula()` | userID | `/api/contratistas/cedula/{userID}` | GET | ❌ FALTANTE |
| `actualizarPassByID()` | Credenciales (by ID) | `/api/auth/credenciales/{id}/password` | PUT | ❌ FALTANTE |
| `validarCorreo()` | correo | `/api/auth/validate-email/{email}` | GET | ✅ IMPLEMENTADO |
| `validarCorreoCuentaActual()` | correo, userID | `/api/auth/validate-email/{email}?exclude={userID}` | GET | ❌ FALTANTE |
| `obtenerSuscripcion()` | userID | `/api/suscripciones/{userID}` | GET | ✅ IMPLEMENTADO |
| `actualizarSuscripcion()` | Suscripciones | `/api/suscripciones/{id}` | PUT | ✅ IMPLEMENTADO |
| `obtenerPlanes()` | - | `/api/suscripciones/planes/empleadores` | GET | ✅ IMPLEMENTADO |
| `obtenerPlanesContratistas()` | - | `/api/suscripciones/planes/contratistas` | GET | ✅ IMPLEMENTADO |
| `procesarVenta()` | Ventas | `/api/pagos/procesar-venta` | POST | ⚠️ MOCK (50%) |
| `guardarSuscripcion()` | Suscripciones | `/api/suscripciones` | POST | ✅ IMPLEMENTADO |
| `obtenerDetalleVentasBySuscripcion()` | userID | `/api/suscripciones/{userID}/ventas` | GET | ⚠️ PARCIAL |

**GAPS IDENTIFICADOS:**

- ⚠️ **GAP-010:** `GuardarPerfil()` - Registro debe crear automáticamente Contratista con `activo=false`
- ⚠️ **GAP-011:** `enviarCorreoActivacion()` - Parámetros completos (con Cuentas o userID)
- ❌ **GAP-012:** `actualizarCredenciales()` - Update full credential (password + activo + email)
- ❌ **GAP-013:** `obtenerCedula()` - Get cedula by userID endpoint
- ❌ **GAP-014:** `actualizarPassByID()` - Change password by credential ID
- ❌ **GAP-015:** `validarCorreoCuentaActual()` - Validate email excluding current user
- ⚠️ **GAP-016:** `procesarVenta()` - Payment gateway integration (Cardnet) is MOCK
- ⚠️ **GAP-017:** `obtenerDetalleVentasBySuscripcion()` - Get ventas history by userID

---

### 5️⃣ CalificacionesService.cs → CalificacionesController

| Método Legacy | Parámetros | Endpoint Clean | HTTP | Estado |
|---------------|------------|----------------|------|--------|
| `getTodas()` | - | `/api/calificaciones` | GET | ✅ IMPLEMENTADO |
| `getById()` | id, userID (optional) | `/api/calificaciones/by-identificacion/{id}?userID={userID}` | GET | ✅ IMPLEMENTADO |
| `getCalificacionByID()` | calificacionID | `/api/calificaciones/{id}` | GET | ✅ IMPLEMENTADO |
| `calificarPerfil()` | Calificaciones | `/api/calificaciones` | POST | ✅ IMPLEMENTADO |

**GAPS IDENTIFICADOS:**

- ✅ **Completamente implementado** (100%)

---

### 6️⃣ PaymentService.cs → PagosController

| Método Legacy | Parámetros | Endpoint Clean | HTTP | Estado |
|---------------|------------|----------------|------|--------|
| `consultarIdempotency()` | url | `/api/pagos/idempotency` | GET | ❌ FALTANTE (Cardnet) |
| `Payment()` | cardNumber, cvv, amount, clientIP, expirationDate, referenceNumber, invoiceNumber | `/api/pagos/procesar` | POST | ⚠️ MOCK (MockPaymentService) |
| `getPaymentParameters()` | - | `/api/configuracion/payment-gateway` | GET | ✅ IMPLEMENTADO |

**GAPS IDENTIFICADOS:**

- 🚨 **GAP-018 (CRÍTICO):** `consultarIdempotency()` - Cardnet idempotency key generation NOT implemented
- 🚨 **GAP-019 (CRÍTICO):** `Payment()` - Cardnet payment integration is **MOCK** (RestSharp + encryption missing)

---

### 7️⃣ EmailService.cs → ConfiguracionController

| Método Legacy | Parámetros | Endpoint Clean | HTTP | Estado |
|---------------|------------|----------------|------|--------|
| `Config_Correo()` | - | `/api/configuracion/email` | GET | ✅ IMPLEMENTADO |

**GAPS IDENTIFICADOS:**

- ✅ **Completamente implementado** (100%)

---

### 8️⃣ BotServices.cs → ConfiguracionController

| Método Legacy | Parámetros | Endpoint Clean | HTTP | Estado |
|---------------|------------|----------------|------|--------|
| `getOpenAI()` | - | `/api/configuracion/openai` | GET | ✅ IMPLEMENTADO |

**GAPS IDENTIFICADOS:**

- ✅ **Completamente implementado** (100%)

---

### 9️⃣ Utilitario.cs → (Various Controllers)

**NOTA:** No se encontró archivo `Utilitario.cs` en Legacy. Posiblemente sea una clase interna o se use `NumeroEnLetras.cs` para conversión de números a texto.

| Funcionalidad Legacy | Endpoint Clean | HTTP | Estado |
|---------------------|----------------|------|--------|
| `NumeroEnLetras.Convertir()` | ❌ FALTANTE | - | ❌ No implementado en Clean |

**GAPS IDENTIFICADOS:**

- ❌ **GAP-020:** Conversión de números a letras (para documentos legales/PDFs)

---

## 🚨 RESUMEN DE GAPS POR CRITICIDAD

### 🔴 GAPS CRÍTICOS (6 gaps)

#### GAP-018: Cardnet Idempotency Key Generation

- **Servicio Legacy:** `PaymentService.consultarIdempotency()`
- **Impacto:** **BLOQUEADOR** - Sin esto, no se pueden procesar pagos reales
- **Descripción:** Genera clave de idempotencia para prevenir transacciones duplicadas en Cardnet
- **Implementación:**

  ```
  Application/Features/Pagos/Queries/GenerateIdempotencyKeyQuery.cs
  Infrastructure/Services/CardnetPaymentService.cs (implementar consulta REST)
  API/Controllers/PagosController.cs → GET /api/pagos/idempotency
  ```

- **Estimado:** 4 horas
- **Prioridad:** 🔴 CRÍTICA

#### GAP-019: Cardnet Payment Processing (Real Implementation)

- **Servicio Legacy:** `PaymentService.Payment()`
- **Impacto:** **BLOQUEADOR** - Pagos actualmente son MOCK
- **Descripción:** Integración completa con Cardnet (RestSharp + encryption + request building)
- **Implementación:**

  ```
  Infrastructure/Services/CardnetPaymentService.cs
  - Implementar Crypt.Decrypt() para números de tarjeta
  - RestClient setup con certificados SSL
  - JSON body building exacto según Cardnet specs
  - Response parsing con manejo de errores
  Application/Features/Pagos/Commands/ProcesarPagoCommand.cs (actualizar)
  API/Controllers/PagosController.cs → POST /api/pagos/procesar (actualizar)
  ```

- **Estimado:** 16 horas
- **Prioridad:** 🔴 CRÍTICA

#### GAP-010: Auto-create Contratista on Registration

- **Servicio Legacy:** `SuscripcionesService.GuardarPerfil()`
- **Impacto:** **ALTO** - Usuarios registrados no tienen perfil de contratista automático
- **Descripción:** Al registrarse, debe crear automáticamente registro en `Contratistas` con `activo=false`
- **Implementación:**

  ```
  Application/Features/Authentication/Commands/RegisterCommand.cs
  - Después de crear Cuenta, crear Contratista automáticamente
  - Copiar Nombre, Apellido, Email, Telefono, fechaIngreso
  - Set activo=false, tipo=1
  ```

- **Estimado:** 3 horas
- **Prioridad:** 🔴 CRÍTICA

#### GAP-016: Payment Gateway Integration (Cardnet)

- **Servicio Legacy:** `SuscripcionesService.procesarVenta()`
- **Impacto:** **BLOQUEADOR** - Ventas no se procesan con gateway real
- **Descripción:** Actualmente usa `MockPaymentService`, debe usar Cardnet real
- **Implementación:**

  ```
  Application/Features/Suscripciones/Commands/ProcesarVentaCommand.cs
  - Inyectar IPaymentService (CardnetPaymentService)
  - Llamar Payment() con parámetros correctos
  - Guardar Ventas con ApprovalCode + PnRef
  - Actualizar Suscripcion si pago exitoso
  ```

- **Estimado:** 8 horas (depende de GAP-018 y GAP-019)
- **Prioridad:** 🔴 CRÍTICA

#### GAP-020: NumeroEnLetras (Number to Spanish Words)

- **Servicio Legacy:** `NumeroEnLetras.Convertir()`
- **Impacto:** **ALTO** - Necesario para PDFs de contratos y recibos legales
- **Descripción:** Convierte números decimales a texto en español (ej: 1500.50 → "Mil quinientos con 50/100")
- **Implementación:**

  ```
  Infrastructure/Services/NumeroEnLetrasService.cs
  - Port logic from Legacy NumeroEnLetras.cs
  - Unit tests para validar conversiones
  Application/Common/Interfaces/INumeroEnLetrasService.cs
  Infrastructure/Services/PdfService.cs (usar en PDFs)
  ```

- **Estimado:** 6 horas
- **Prioridad:** 🔴 CRÍTICA (para producción)

#### GAP-001: Delete User (borrarUsuario)

- **Servicio Legacy:** `LoginService.borrarUsuario()`
- **Impacto:** **MEDIO-ALTO** - No hay forma de eliminar usuarios/credenciales
- **Descripción:** Eliminar credencial por userID + credencialID
- **Implementación:**

  ```
  Application/Features/Authentication/Commands/DeleteUserCommand.cs
  - Validar que userID coincide con credencialID
  - Soft delete preferido (Activo=false)
  - Hard delete si requerido (cascade considerations)
  API/Controllers/AuthController.cs → DELETE /api/auth/users/{userID}/credentials/{credencialID}
  ```

- **Estimado:** 4 horas
- **Prioridad:** 🟠 ALTA

---

### 🟠 GAPS ALTA PRIORIDAD (8 gaps)

#### GAP-005: procesarPagoContratacion - Update Detalle Estatus

- **Descripción:** Cuando `Concepto == "Pago Final"`, debe actualizar `DetalleContrataciones.estatus = 2`
- **Implementación:**

  ```
  Application/Features/Contrataciones/Commands/ProcesarPagoContratacionCommand.cs
  - Agregar lógica: if (detalle.FirstOrDefault().Concepto == "Pago Final") { update estatus = 2 }
  ```

- **Estimado:** 2 horas

#### GAP-006: cancelarTrabajo - Change Estatus to 3

- **Descripción:** Cambiar `DetalleContrataciones.estatus = 3` (cancelado)
- **Implementación:**

  ```
  Application/Features/Contrataciones/Commands/CancelarTrabajoCommand.cs
  API/Controllers/ContratacionesController.cs → POST /api/contrataciones/{contratacionID}/detalle/{detalleID}/cancelar
  ```

- **Estimado:** 2 horas

#### GAP-007: eliminarEmpleadoTemporal - Cascade Delete Complete

- **Descripción:** Delete debe ser cascade completo: Recibos → Detalles → Empleado Temporal
- **Implementación:**

  ```
  Application/Features/Contrataciones/Commands/EliminarEmpleadoTemporalCommand.cs
  - Loop through Empleador_Recibos_Header_Contrataciones
  - Delete Empleador_Recibos_Detalle_Contrataciones for each
  - Delete Empleador_Recibos_Header_Contrataciones
  - Delete EmpleadoTemporal
  ```

- **Estimado:** 4 horas

#### GAP-008: guardarOtrasRemuneraciones - Batch Insert

- **Descripción:** Insertar múltiples remuneraciones en una transacción
- **Implementación:**

  ```
  Application/Features/Empleados/Commands/GuardarOtrasRemuneracionesCommand.cs
  API/Controllers/EmpleadosController.cs → POST /api/empleados/remuneraciones/batch
  ```

- **Estimado:** 2 horas

#### GAP-009: actualizarRemuneraciones - Replace All

- **Descripción:** Eliminar todas las remuneraciones de un empleado y reemplazar con nuevas
- **Implementación:**

  ```
  Application/Features/Empleados/Commands/ActualizarRemuneracionesCommand.cs
  - Delete all where empleadoID
  - Insert new list
  API/Controllers/EmpleadosController.cs → PUT /api/empleados/{id}/remuneraciones
  ```

- **Estimado:** 3 horas

#### GAP-011: enviarCorreoActivacion - Full Parameters

- **Descripción:** Enviar email con opción de pasar `Cuentas` o `userID`
- **Implementación:**

  ```
  Application/Features/Authentication/Commands/ResendActivationEmailCommand.cs
  - Aceptar userID o email
  - Query Cuentas si solo se pasa userID
  - Build activation URL con userID + email
  ```

- **Estimado:** 2 horas

#### GAP-012: actualizarCredenciales - Full Update

- **Descripción:** Update password + activo + email en una credencial
- **Implementación:**

  ```
  Application/Features/Authentication/Commands/UpdateCredencialCommand.cs
  API/Controllers/AuthController.cs → PUT /api/auth/credenciales/{id}
  ```

- **Estimado:** 2 horas

#### GAP-013: obtenerCedula - Get Cedula by UserID

- **Descripción:** Endpoint simple para obtener cédula de contratista por userID
- **Implementación:**

  ```
  Application/Features/Contratistas/Queries/GetCedulaByUserIdQuery.cs
  API/Controllers/ContratistasController.cs → GET /api/contratistas/cedula/{userID}
  ```

- **Estimado:** 1 hora

---

### 🟡 GAPS MEDIA PRIORIDAD (6 gaps)

#### GAP-002: agregarPerfilInfo - Add PerfilesInfo

- **Descripción:** Agregar PerfilesInfo separado de Cuenta
- **Implementación:**

  ```
  Application/Features/Authentication/Commands/AddPerfilInfoCommand.cs
  API/Controllers/AuthController.cs → POST /api/auth/perfil/info
  ```

- **Estimado:** 2 horas

#### GAP-003: getPerfilByID - Get Profile by CuentaID

- **Descripción:** Get cuenta by cuentaID (not userID)
- **Implementación:**

  ```
  Application/Features/Authentication/Queries/GetPerfilByCuentaIdQuery.cs
  API/Controllers/AuthController.cs → GET /api/auth/perfil/cuenta/{cuentaID}
  ```

- **Estimado:** 1 hora

#### GAP-004: actualizarPerfil - Include PerfilesInfo

- **Descripción:** Update debe manejar tanto Cuenta como PerfilesInfo
- **Implementación:**

  ```
  Application/Features/Authentication/Commands/UpdatePerfilCommand.cs
  - Actualizar lógica para manejar PerfilesInfo optional
  ```

- **Estimado:** 2 horas

#### GAP-014: actualizarPassByID - Change Password by Credential ID

- **Descripción:** Change password usando credential ID en lugar de email
- **Implementación:**

  ```
  Application/Features/Authentication/Commands/ChangePasswordByIdCommand.cs
  API/Controllers/AuthController.cs → PUT /api/auth/credenciales/{id}/password
  ```

- **Estimado:** 2 horas

#### GAP-015: validarCorreoCuentaActual - Validate Email Excluding User

- **Descripción:** Validar si email existe pero excluir userID actual (para update perfil)
- **Implementación:**

  ```
  Application/Features/Authentication/Queries/ValidateEmailExcludingUserQuery.cs
  API/Controllers/AuthController.cs → GET /api/auth/validate-email/{email}?exclude={userID}
  ```

- **Estimado:** 1 hora

#### GAP-017: obtenerDetalleVentasBySuscripcion - Get Ventas History

- **Descripción:** Obtener historial de ventas por userID
- **Implementación:**

  ```
  Application/Features/Suscripciones/Queries/GetVentasByUserIdQuery.cs
  API/Controllers/SuscripcionesController.cs → GET /api/suscripciones/{userID}/ventas
  ```

- **Estimado:** 2 horas

---

### 🟢 GAPS BAJA PRIORIDAD (0 gaps)

- **Ninguno identificado** - Todas las funcionalidades restantes son críticas a medias

---

## 📦 PLAN DE IMPLEMENTACIÓN POR LOTES

### 🎯 LOTE 1: Payment Gateway Integration (CRÍTICO)

**Objetivo:** Habilitar pagos reales con Cardnet  
**Duración estimada:** 32 horas (4 días)  
**Prioridad:** 🔴 BLOQUEADOR

**Funcionalidades:**

1. GAP-018: Cardnet Idempotency Key Generation (4h)
2. GAP-019: Cardnet Payment Processing - Real Implementation (16h)
3. GAP-016: Payment Gateway Integration in procesarVenta (8h)
4. **Testing completo con Cardnet Sandbox** (4h)

**Archivos a crear/modificar:**

```
Infrastructure/
  Services/
    CardnetPaymentService.cs                    [CREAR]
    EncryptionService.cs                        [CREAR - port from Crypt class]
  DependencyInjection.cs                        [MODIFICAR - register CardnetPaymentService]

Application/
  Features/
    Pagos/
      Queries/
        GenerateIdempotencyKeyQuery.cs          [CREAR]
        GenerateIdempotencyKeyQueryHandler.cs   [CREAR]
      Commands/
        ProcesarPagoCommand.cs                  [MODIFICAR - usar CardnetPaymentService]
    Suscripciones/
      Commands/
        ProcesarVentaCommand.cs                 [MODIFICAR - integrar pago real]

API/
  Controllers/
    PagosController.cs                          [MODIFICAR - agregar GET /idempotency]
```

**Validación:**

- ✅ Generar idempotency key exitosamente
- ✅ Procesar pago con tarjeta de prueba (sandbox)
- ✅ Capturar ApprovalCode y PnRef
- ✅ Guardar Venta con datos completos
- ✅ Actualizar Suscripcion automáticamente si pago OK

---

### 🎯 LOTE 2: User Management & Registration Flow

**Objetivo:** Completar flujo de registro y gestión de usuarios  
**Duración estimada:** 18 horas (2-3 días)  
**Prioridad:** 🔴 CRÍTICA

**Funcionalidades:**

1. GAP-010: Auto-create Contratista on Registration (3h)
2. GAP-001: Delete User (borrarUsuario) (4h)
3. GAP-011: enviarCorreoActivacion - Full Parameters (2h)
4. GAP-012: actualizarCredenciales - Full Update (2h)
5. GAP-013: obtenerCedula - Get Cedula by UserID (1h)
6. GAP-014: actualizarPassByID - Change Password by ID (2h)
7. GAP-015: validarCorreoCuentaActual - Validate Email Excluding User (1h)
8. **Testing e2e registration flow** (3h)

**Archivos a crear/modificar:**

```
Application/
  Features/
    Authentication/
      Commands/
        RegisterCommand.cs                           [MODIFICAR - auto-create Contratista]
        DeleteUserCommand.cs                         [CREAR]
        ResendActivationEmailCommand.cs              [MODIFICAR]
        UpdateCredencialCommand.cs                   [CREAR]
        ChangePasswordByIdCommand.cs                 [CREAR]
      Queries/
        ValidateEmailExcludingUserQuery.cs           [CREAR]
    Contratistas/
      Queries/
        GetCedulaByUserIdQuery.cs                    [CREAR]

API/
  Controllers/
    AuthController.cs                                [MODIFICAR - agregar endpoints]
    ContratistasController.cs                        [MODIFICAR - GET /cedula/{userID}]
```

**Validación:**

- ✅ Registro crea Cuenta + Credencial + Contratista automáticamente
- ✅ Email de activación se envía correctamente
- ✅ Delete user funciona (soft/hard delete)
- ✅ Update credencial completo funciona
- ✅ Validar email excluyendo usuario actual

---

### 🎯 LOTE 3: Empleados & Nómina - Gaps Menores

**Objetivo:** Completar funcionalidades faltantes de empleados y nómina  
**Duración estimada:** 13 horas (1-2 días)  
**Prioridad:** 🟠 ALTA

**Funcionalidades:**

1. GAP-005: procesarPagoContratacion - Update Detalle Estatus (2h)
2. GAP-006: cancelarTrabajo - Change Estatus to 3 (2h)
3. GAP-007: eliminarEmpleadoTemporal - Cascade Delete Complete (4h)
4. GAP-008: guardarOtrasRemuneraciones - Batch Insert (2h)
5. GAP-009: actualizarRemuneraciones - Replace All (3h)

**Archivos a crear/modificar:**

```
Application/
  Features/
    Contrataciones/
      Commands/
        ProcesarPagoContratacionCommand.cs           [MODIFICAR - add estatus update]
        CancelarTrabajoCommand.cs                    [CREAR]
        EliminarEmpleadoTemporalCommand.cs           [MODIFICAR - cascade delete]
    Empleados/
      Commands/
        GuardarOtrasRemuneracionesCommand.cs         [CREAR]
        ActualizarRemuneracionesCommand.cs           [CREAR]

API/
  Controllers/
    ContratacionesController.cs                      [MODIFICAR]
    EmpleadosController.cs                           [MODIFICAR]
```

**Validación:**

- ✅ Pago final actualiza estatus de detalle a 2
- ✅ Cancelar trabajo cambia estatus a 3
- ✅ Delete empleado temporal elimina recibos cascade
- ✅ Batch insert remuneraciones funciona
- ✅ Replace all remuneraciones funciona

---

### 🎯 LOTE 4: Profile Management Completions

**Objetivo:** Completar gestión de perfiles y cuentas  
**Duración estimada:** 7 horas (1 día)  
**Prioridad:** 🟡 MEDIA

**Funcionalidades:**

1. GAP-002: agregarPerfilInfo - Add PerfilesInfo (2h)
2. GAP-003: getPerfilByID - Get Profile by CuentaID (1h)
3. GAP-004: actualizarPerfil - Include PerfilesInfo (2h)
4. GAP-017: obtenerDetalleVentasBySuscripcion - Get Ventas History (2h)

**Archivos a crear/modificar:**

```
Application/
  Features/
    Authentication/
      Commands/
        AddPerfilInfoCommand.cs                      [CREAR]
        UpdatePerfilCommand.cs                       [MODIFICAR]
      Queries/
        GetPerfilByCuentaIdQuery.cs                  [CREAR]
    Suscripciones/
      Queries/
        GetVentasByUserIdQuery.cs                    [CREAR]

API/
  Controllers/
    AuthController.cs                                [MODIFICAR]
    SuscripcionesController.cs                       [MODIFICAR]
```

**Validación:**

- ✅ Add PerfilesInfo separado funciona
- ✅ Get perfil by cuentaID funciona
- ✅ Update perfil incluye PerfilesInfo
- ✅ Get ventas history funciona

---

### 🎯 LOTE 5: PDF Generation & NumeroEnLetras

**Objetivo:** Habilitar generación de PDFs legales completos  
**Duración estimada:** 6 horas (1 día)  
**Prioridad:** 🔴 CRÍTICA (para producción)

**Funcionalidades:**

1. GAP-020: NumeroEnLetras (Number to Spanish Words) (6h)
   - Port logic from Legacy NumeroEnLetras.cs
   - Unit tests
   - Integrate with PdfService

**Archivos a crear/modificar:**

```
Infrastructure/
  Services/
    NumeroEnLetrasService.cs                         [CREAR]
    PdfService.cs                                    [MODIFICAR - usar NumeroEnLetras]

Application/
  Common/
    Interfaces/
      INumeroEnLetrasService.cs                      [CREAR]

Tests/
  MiGenteEnLinea.Infrastructure.Tests/
    Services/
      NumeroEnLetrasServiceTests.cs                  [CREAR]
```

**Validación:**

- ✅ Convertir 1500.50 → "Mil quinientos con 50/100"
- ✅ Edge cases (0, negativos, millones, decimales)
- ✅ PDFs de contratos incluyen monto en letras
- ✅ PDFs de recibos incluyen monto en letras

---

## 📝 RECOMENDACIONES TÉCNICAS

### 1. **Orden de Ejecución Sugerido**

```
LOTE 1 (Payment) → LOTE 2 (Users) → LOTE 5 (PDFs) → LOTE 3 (Empleados) → LOTE 4 (Profiles)
```

**Razón:** Payment gateway es BLOQUEADOR para producción. Users flow es crítico para onboarding. PDFs necesarios para legalidad.

### 2. **Testing Strategy**

- **LOTE 1:** Sandbox de Cardnet obligatorio antes de producción
- **LOTE 2:** E2E tests para registration flow completo
- **LOTE 3:** Integration tests para cascade deletes
- **LOTE 4:** Unit tests para cada endpoint
- **LOTE 5:** Unit tests exhaustivos para NumeroEnLetras (edge cases)

### 3. **Riesgos Identificados**

#### 🚨 RIESGO ALTO: Cardnet API Changes

- **Problema:** API de Cardnet puede haber cambiado desde Legacy
- **Mitigación:** Validar documentación actualizada de Cardnet antes de LOTE 1
- **Contingencia:** Implementar adapter pattern para fácil cambio de gateway

#### 🚨 RIESGO MEDIO: Encryption Compatibility

- **Problema:** `Crypt.Decrypt()` de ClassLibrary_CSharp no disponible en Clean
- **Mitigación:** Port encryption logic o usar biblioteca compatible (AES-256)
- **Contingencia:** Almacenar tarjetas tokenizadas en lugar de encriptadas

#### 🚨 RIESGO MEDIO: Cascade Deletes

- **Problema:** EF Core cascade delete puede diferir de EF6 Legacy
- **Mitigación:** Testing exhaustivo de GAP-007 con datos reales
- **Contingencia:** Implementar soft deletes en lugar de hard deletes

### 4. **Consideraciones de Seguridad**

#### 🔒 PCI-DSS Compliance (Cardnet)

- ⚠️ **NUNCA** almacenar CVV en base de datos
- ⚠️ Encriptar números de tarjeta con AES-256 + salt
- ⚠️ Usar HTTPS obligatorio para `/api/pagos/*`
- ⚠️ Logging debe excluir datos sensibles (tarjetas, CVV)

#### 🔒 GDPR/LOPD Compliance (Delete User)

- ⚠️ GAP-001 debe permitir "Right to be Forgotten"
- ⚠️ Implementar soft delete por defecto
- ⚠️ Hard delete solo con confirmación explícita
- ⚠️ Cascade delete debe incluir datos personales

### 5. **Performance Considerations**

#### ⚡ Batch Operations

- GAP-008 y GAP-009: Usar `AddRange()` en lugar de loops
- Limitar batch size a 100 items por request
- Implementar retry logic para fallos transitorios

#### ⚡ Idempotency Keys Caching

- GAP-018: Cachear idempotency keys por 24 horas
- Usar distributed cache (Redis) en producción
- Implementar key rotation para seguridad

---

## 📊 MÉTRICAS DE ÉXITO

### KPIs Post-Implementación

| Métrica | Target | Validación |
|---------|--------|------------|
| **Cobertura de Endpoints** | 100% | Todos los métodos Legacy tienen equivalente Clean |
| **Payment Success Rate** | >95% | Cardnet transactions exitosas en sandbox |
| **Registration Completion** | >90% | Users completan flujo desde registro hasta activación |
| **PDF Generation Time** | <3s | PDFs generados en menos de 3 segundos |
| **API Response Time** | <500ms (p95) | 95% de requests responden en <500ms |
| **Test Coverage** | >80% | Unit + Integration tests |

---

## 🎯 SIGUIENTES PASOS INMEDIATOS

### PASO 1: Validar Documentación Cardnet (2 horas)

- Revisar documentación actualizada de Cardnet Payment Gateway
- Validar que endpoints y request format no hayan cambiado
- Obtener credenciales de Sandbox si no existen
- Verificar certificados SSL necesarios

### PASO 2: Configurar Ambiente de Testing (1 hora)

- Configurar Cardnet Sandbox en appsettings.Development.json
- Crear tarjetas de prueba según documentación Cardnet
- Configurar logging detallado para debugging de pagos

### PASO 3: Iniciar LOTE 1 - Payment Gateway (32 horas)

- Crear branch `feature/payment-gateway-integration`
- Implementar GAP-018, GAP-019, GAP-016 en orden
- Testing exhaustivo con Cardnet Sandbox
- Code review y merge a `main`

---

## 📄 APÉNDICE: Comparación Rápida

### Legacy Services → Clean Controllers Mapping

| Legacy Service | Clean Controller | Completitud |
|----------------|------------------|-------------|
| LoginService | AuthController | ⚠️ 85% |
| EmpleadosService | EmpleadosController + NominasController + ContratacionesController | ⚠️ 90% |
| ContratistasService | ContratistasController | ✅ 100% |
| SuscripcionesService | SuscripcionesController + AuthController | ⚠️ 70% |
| CalificacionesService | CalificacionesController | ✅ 100% |
| PaymentService | PagosController | ❌ 30% (MOCK) |
| EmailService | ConfiguracionController | ✅ 100% |
| BotServices | ConfiguracionController | ✅ 100% |

**PROMEDIO GENERAL:** **78% implementado**  
**GAPS TOTALES:** **20 gaps identificados**  
**TIEMPO ESTIMADO TOTAL:** **76 horas** (~10 días laborales)

---

**PRÓXIMA SESIÓN:** Iniciar LOTE 1 (Payment Gateway Integration)  
**Responsable:** Equipo de desarrollo  
**Fecha objetivo:** Semana del 28 de Octubre 2025  

---

_Documento generado el 24 de Octubre 2025_  
_Última actualización: 2025-10-24 19:30 UTC-4_
