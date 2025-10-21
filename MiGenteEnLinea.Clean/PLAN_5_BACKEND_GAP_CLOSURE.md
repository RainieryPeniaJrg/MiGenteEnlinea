# 🎯 PLAN 5: BACKEND GAP CLOSURE - Lógica de Negocio Faltante

**Fecha de Creación:** 2025-01-18  
**Estado:** 📋 **PLANIFICACIÓN**  
**Objetivo:** Migrar TODA la lógica de negocio Legacy faltante al Clean Architecture

---

## 📊 RESUMEN EJECUTIVO

### Análisis de Brechas (Legacy vs Clean)

**Estado Actual:**

- ✅ **Domain Layer:** 100% completo (36 entidades)
- ✅ **Repository Pattern:** 100% completo (29 repositorios - PLAN 4)
- ⚠️ **Application Layer:** 83% completo (5/6 módulos core)
- ⚠️ **Services Layer:** 38% migrado (5/13 servicios legacy)
- ❌ **Business Logic Gaps:** ~15-20% de lógica faltante identificada

**Servicios Legacy Identificados:**

| # | Servicio Legacy | Líneas | Estado | Prioridad | LOTE |
|---|----------------|--------|--------|-----------|------|
| 1 | EmailService.cs + EmailSender.cs | ~180 | ❌ NO MIGRADO | 🔴 CRÍTICA | LOTE 5.1 |
| 2 | CalificacionesService.cs | 63 | ❌ NO MIGRADO | 🔴 ALTA | LOTE 5.2 |
| 3 | Utilitario.cs | ~60 | ❌ NO MIGRADO | 🟡 MEDIA | LOTE 5.3 |
| 4 | BotServices.cs + botService.asmx.cs | ~30 | ❌ NO MIGRADO | 🟢 BAJA | LOTE 5.4 |
| 5 | **Contrataciones (ASPX Logic)** | ~400 | ❌ NO MIGRADO | 🔴 ALTA | LOTE 5.5 |
| 6 | **Nómina Avanzada (ASPX Logic)** | ~300 | ❌ NO MIGRADO | 🟡 MEDIA | LOTE 5.6 |
| 7 | **Dashboard & Reports (ASPX Logic)** | ~200 | ❌ NO MIGRADO | 🟡 MEDIA | LOTE 5.7 |

**Total Estimado:** ~1,233 líneas de lógica Legacy sin migrar

---

## 🎯 OBJETIVOS DEL PLAN 5

### Objetivo General

Alcanzar **100% de paridad funcional** entre Legacy y Clean Architecture, asegurando que toda la lógica de negocio esté migrada al patrón CQRS.

### Objetivos Específicos

1. **Completar funcionalidades core bloqueantes** (LOTEs 5.1-5.2)
   - EmailService funcional (CRÍTICO - bloquea registro)
   - Sistema de calificaciones completo

2. **Migrar lógica de negocio avanzada** (LOTEs 5.3-5.5)
   - Utilidades y helpers
   - Contrataciones completas (gestión de trabajos temporales)
   - Nómina avanzada (cálculos TSS, impuestos)

3. **Funcionalidades opcionales** (LOTEs 5.6-5.7)
   - Dashboards y reportes
   - Bot de OpenAI (abogado virtual)

4. **Consolidación y testing**
   - Testing exhaustivo de toda la lógica migrada
   - Documentación de APIs
   - Performance optimization

---

## 📋 LOTES DE IMPLEMENTACIÓN

### LOTE 5.1: EmailService Implementation 🔴 CRÍTICA

**Prioridad:** 🔴 **CRÍTICA - BLOQUEANTE**  
**Estimación:** 1-2 días (8-16 horas)  
**Estado:** ❌ NO INICIADO

#### Problema Identificado

**BLOCKER ACTUAL:**

```csharp
// RegisterCommandHandler.cs línea 58
await _emailService.SendActivationEmailAsync(
    credencial.Email.Value, 
    credencial.Id.ToString(), 
    activationToken
);
// ❌ ERROR: Service no registrado en DI
```

**DependencyInjection.cs línea 23:**

```csharp
// services.AddScoped<IEmailService, EmailService>(); // ❌ COMENTADO
```

#### Análisis de Legacy

**EmailService.cs (15 líneas):**

```csharp
public class EmailService
{
    migenteEntities db = new migenteEntities();
    public Config_Correo Config_Correo()
    {
        return db.Config_Correo.FirstOrDefault();
    }
}
```

**EmailSender.cs (180 líneas - 3 métodos):**

1. `SendEmailRegistro(name, to, subject, url)` - Email de activación
2. `SendEmailCompra(name, to, subject, plan, monto, numero)` - Email de confirmación de compra
3. `SendEmailReset(name, to, subject, url)` - Email de reseteo de contraseña

**Templates HTML existentes:**

- `/MailTemplates/confirmacionRegistro.html`
- `/MailTemplates/checkout.html`
- `/MailTemplates/recuperarPass.html`

#### Tareas de Implementación

**FASE 1: Infrastructure Setup (3-4 horas)**

1. **Crear EmailSettings Options Pattern**

   ```
   Infrastructure/Options/EmailSettings.cs (~40 líneas)
   ```

   - Propiedades: FromName, FromEmail, SmtpServer, SmtpPort, UseSsl, Username, Password

2. **Implementar IEmailService en Infrastructure**

   ```
   Infrastructure/Services/EmailService.cs (~250 líneas)
   ```

   - `SendActivationEmailAsync(email, userId, token)`
   - `SendPasswordResetEmailAsync(email, userId, token)`
   - `SendWelcomeEmailAsync(email, nombre)`
   - `SendPaymentConfirmationEmailAsync(email, venta)`
   - `SendContractNotificationEmailAsync(email, contratacion)`
   - Helper: `LoadEmailTemplate(templateName, replacements)`

3. **Migrar Email Templates**

   ```
   Infrastructure/Templates/
   ├── ActivationEmail.html
   ├── PasswordResetEmail.html
   ├── WelcomeEmail.html
   ├── PaymentConfirmationEmail.html
   └── ContractNotificationEmail.html
   ```

4. **Configurar appsettings.json**

   ```json
   {
     "EmailSettings": {
       "FromName": "MiGente En Línea",
       "FromEmail": "noreply@migenteonline.com",
       "SmtpServer": "smtp.gmail.com",
       "SmtpPort": 587,
       "UseSsl": true,
       "Username": "migente@gmail.com",
       "Password": "***"
     }
   }
   ```

5. **Instalar NuGet Package**

   ```bash
   cd src/Infrastructure/MiGenteEnLinea.Infrastructure
   dotnet add package MailKit --version 4.3.0
   ```

6. **Registrar en DI (DependencyInjection.cs)**

   ```csharp
   services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
   services.AddScoped<IEmailService, EmailService>();
   ```

**FASE 2: Testing & Validation (2-3 horas)**

1. **Unit Tests**
   - Test `LoadEmailTemplate()` con reemplazos de placeholders
   - Mock SMTP client para tests

2. **Integration Tests**
   - Test con cuenta SMTP de prueba (Ethereal Email o Mailtrap)
   - Verificar templates se cargan correctamente

3. **End-to-End Testing**
   - Ejecutar `RegisterCommand` y verificar email llega
   - Ejecutar `PasswordResetCommand` y verificar email
   - Ejecutar `ProcesarVentaCommand` y verificar email de confirmación

**FASE 3: Documentation (1 hora)**

1. Crear `LOTE_5_1_EMAIL_SERVICE_COMPLETADO.md`
2. Documentar configuración SMTP
3. Documentar troubleshooting común (Gmail App Passwords, firewall, etc.)

#### Archivos a Crear (Total: 12 archivos, ~600 líneas)

**Application Layer:**

- `Application/Common/Interfaces/IEmailService.cs` (~60 líneas)

**Infrastructure Layer:**

- `Infrastructure/Options/EmailSettings.cs` (~40 líneas)
- `Infrastructure/Services/EmailService.cs` (~250 líneas)
- `Infrastructure/Templates/ActivationEmail.html` (~50 líneas)
- `Infrastructure/Templates/PasswordResetEmail.html` (~50 líneas)
- `Infrastructure/Templates/WelcomeEmail.html` (~40 líneas)
- `Infrastructure/Templates/PaymentConfirmationEmail.html` (~60 líneas)
- `Infrastructure/Templates/ContractNotificationEmail.html` (~50 líneas)

**Tests:**

- `Tests/Infrastructure.Tests/Services/EmailServiceTests.cs` (~150 líneas)

**Documentation:**

- `LOTE_5_1_EMAIL_SERVICE_COMPLETADO.md` (~800 líneas)

#### Métricas de Éxito

- ✅ `RegisterCommand` ejecuta sin errores
- ✅ Email de activación llega al inbox
- ✅ Email de confirmación de compra llega al inbox
- ✅ Email de reseteo de contraseña llega al inbox
- ✅ Templates HTML se renderizan correctamente
- ✅ Build sin errores (0 errors, 0 warnings nuevos)
- ✅ Tests pasan (100% success rate)

---

### LOTE 5.2: Calificaciones (Ratings & Reviews) 🔴 ALTA

**Prioridad:** 🔴 **ALTA - FUNCIONALIDAD CORE**  
**Estimación:** 2-3 días (16-24 horas)  
**Estado:** ❌ NO INICIADO

#### Análisis de Legacy

**CalificacionesService.cs (63 líneas - 4 métodos):**

```csharp
// 1. Obtener todas las calificaciones
public List<VCalificaciones> getTodas()

// 2. Obtener calificaciones por ID de contratista/empleado
public List<VCalificaciones> getById(string id, string userID = null)

// 3. Obtener calificación específica
public Calificaciones getCalificacionByID(int calificacionID)

// 4. Crear calificación
public Calificaciones calificarPerfil(Calificaciones cal)
```

**Páginas Legacy que usan calificaciones:**

- `/Empleador/CalificacionDePerfiles.aspx` - Empleador califica contratista
- `/Contratista/MisCalificaciones.aspx` - Contratista ve sus reviews

#### Tareas de Implementación

**FASE 1: Commands (1 día, 9 archivos, ~600 líneas)**

1. **CreateCalificacionCommand**

   ```
   Features/Calificaciones/Commands/CreateCalificacion/
   ├── CreateCalificacionCommand.cs (~30 líneas)
   ├── CreateCalificacionCommandHandler.cs (~80 líneas)
   ├── CreateCalificacionCommandValidator.cs (~60 líneas)
   └── CreateCalificacionCommandTests.cs (~100 líneas)
   ```

   - Validaciones: Rating (1-5), Comentario (max 500 chars), ContratistaId o EmpleadoId required
   - Business Logic: Solo puede calificar si hay contratación completada
   - Evento: CalificacionCreada (para actualizar promedio de contratista)

2. **UpdateCalificacionCommand**

   ```
   Features/Calificaciones/Commands/UpdateCalificacion/
   ├── UpdateCalificacionCommand.cs (~30 líneas)
   ├── UpdateCalificacionCommandHandler.cs (~70 líneas)
   ├── UpdateCalificacionCommandValidator.cs (~50 líneas)
   └── UpdateCalificacionCommandTests.cs (~80 líneas)
   ```

   - Validaciones: Solo puede editar propia calificación, dentro de 30 días de creación
   - Business Logic: Re-calcular promedio de contratista

3. **DeleteCalificacionCommand**

   ```
   Features/Calificaciones/Commands/DeleteCalificacion/
   ├── DeleteCalificacionCommand.cs (~20 líneas)
   ├── DeleteCalificacionCommandHandler.cs (~60 líneas)
   └── DeleteCalificacionCommandTests.cs (~60 líneas)
   ```

   - Soft delete (cambiar Activo = false)
   - Business Logic: Re-calcular promedio de contratista

**FASE 2: Queries (1 día, 8 archivos, ~500 líneas)**

1. **GetCalificacionesByContratistaQuery**

   ```
   Features/Calificaciones/Queries/GetCalificacionesByContratista/
   ├── GetCalificacionesByContratistaQuery.cs (~30 líneas)
   ├── GetCalificacionesByContratistaQueryHandler.cs (~80 líneas)
   └── GetCalificacionesByContratistaQueryTests.cs (~100 líneas)
   ```

   - Paginación (PageNumber, PageSize)
   - Ordenamiento por fecha descendente
   - Incluir datos del empleador que calificó

2. **GetCalificacionesByEmpleadoQuery**

   ```
   Features/Calificaciones/Queries/GetCalificacionesByEmpleado/
   ├── GetCalificacionesByEmpleadoQuery.cs (~30 líneas)
   ├── GetCalificacionesByEmpleadoQueryHandler.cs (~70 líneas)
   └── GetCalificacionesByEmpleadoQueryTests.cs (~80 líneas)
   ```

   - Similar a GetCalificacionesByContratista
   - Filtrado por EmpleadoId

3. **GetCalificacionByIdQuery**

   ```
   Features/Calificaciones/Queries/GetCalificacionById/
   ├── GetCalificacionByIdQuery.cs (~20 líneas)
   ├── GetCalificacionByIdQueryHandler.cs (~50 líneas)
   └── GetCalificacionByIdQueryTests.cs (~60 líneas)
   ```

4. **GetPromedioCalificacionQuery**

   ```
   Features/Calificaciones/Queries/GetPromedioCalificacion/
   ├── GetPromedioCalificacionQuery.cs (~30 líneas)
   ├── GetPromedioCalificacionQueryHandler.cs (~100 líneas)
   └── GetPromedioCalificacionQueryTests.cs (~80 líneas)
   ```

   - Calcular promedio, total reviews, distribución por estrellas (1★-5★)
   - Retornar PromedioCalificacionDto

**FASE 3: DTOs & Controller (4 horas, 3 archivos, ~300 líneas)**

1. **CalificacionDto**

   ```
   Features/Calificaciones/DTOs/CalificacionDto.cs (~80 líneas)
   ```

   - Propiedades calculadas: FechaRelativa (hace 2 días), EsReciente (< 7 días)

2. **PromedioCalificacionDto**

   ```
   Features/Calificaciones/DTOs/PromedioCalificacionDto.cs (~50 líneas)
   ```

   - Promedio (decimal)
   - TotalReviews (int)
   - Distribucion (Dictionary<int, int>: {5: 10, 4: 5, 3: 2, 2: 1, 1: 0})

3. **CalificacionesController**

   ```
   Presentation/Controllers/CalificacionesController.cs (~170 líneas)
   ```

   - `POST /api/calificaciones` - Crear calificación ✅
   - `PUT /api/calificaciones/{id}` - Editar calificación ✅
   - `DELETE /api/calificaciones/{id}` - Eliminar calificación ✅
   - `GET /api/calificaciones/contratista/{contratistaId}` - Listar reviews de contratista ✅
   - `GET /api/calificaciones/empleado/{empleadoId}` - Listar reviews de empleado ✅
   - `GET /api/calificaciones/{id}` - Obtener calificación específica ✅
   - `GET /api/calificaciones/contratista/{contratistaId}/promedio` - Obtener promedio ✅

**FASE 4: Testing & Documentation (4 horas)**

1. Integration tests para CalificacionesController
2. Validar cálculo de promedio correcto
3. Crear `LOTE_5_2_CALIFICACIONES_COMPLETADO.md`

#### Archivos a Crear (Total: 20 archivos, ~1,400 líneas)

**Application Layer (17 archivos):**

- 3 Commands + 3 Handlers + 3 Validators = 9 archivos
- 4 Queries + 4 Handlers = 8 archivos
- 2 DTOs

**Presentation Layer (1 archivo):**

- CalificacionesController.cs

**Tests (10+ archivos):**

- Command tests, Query tests, Controller integration tests

**Documentation:**

- LOTE_5_2_CALIFICACIONES_COMPLETADO.md

#### Métricas de Éxito

- ✅ 7 endpoints REST funcionando
- ✅ Sistema de rating 1-5 estrellas funcional
- ✅ Promedio de calificaciones se calcula correctamente
- ✅ Distribución por estrellas correcta
- ✅ Paginación funciona correctamente
- ✅ Validaciones FluentValidation funcionando
- ✅ Build sin errores
- ✅ Tests pasan (100% success rate)

---

### LOTE 5.3: Utilities & Helpers 🟡 MEDIA

**Prioridad:** 🟡 **MEDIA**  
**Estimación:** 1 día (6-8 horas)  
**Estado:** ❌ NO INICIADO

#### Análisis de Legacy

**Utilitario.cs (60 líneas - 2 métodos):**

1. `ObtenerImagenComoDataUrl(int id)` - Convertir imagen de BD a Data URL (Base64)
2. `ConvertHtmlToPdf(string htmlContent)` - Convertir HTML a PDF usando iText

**Uso Actual:**

- Generación de contratos PDF
- Generación de recibos de pago PDF
- Renderización de imágenes en reportes

#### Tareas de Implementación

**FASE 1: PDF Generation Service (3-4 horas)**

1. **Crear IPdfService**

   ```
   Application/Common/Interfaces/IPdfService.cs (~40 líneas)
   ```

   - `ConvertHtmlToPdfAsync(htmlContent, outputPath?)`
   - `GenerateContractPdfAsync(contratacion)`
   - `GeneratePayrollReceiptPdfAsync(recibo)`

2. **Implementar PdfService**

   ```
   Infrastructure/Services/PdfService.cs (~200 líneas)
   ```

   - Usar iText 8.0.5 (ya instalado en Legacy)
   - Métodos: ConvertHtmlToPdf, GenerateContractPdf, GeneratePayrollReceiptPdf
   - Templates: Cargar desde Infrastructure/Templates/Pdf/

3. **Crear PDF Templates**

   ```
   Infrastructure/Templates/Pdf/
   ├── ContratoTemplate.html
   ├── ReciboTemplate.html
   └── _Shared/
       ├── Header.html
       └── Footer.html
   ```

**FASE 2: Image Utilities (2-3 horas)**

1. **Crear IImageService**

   ```
   Application/Common/Interfaces/IImageService.cs (~30 líneas)
   ```

   - `ConvertToDataUrlAsync(imageBytes, mimeType)`
   - `ConvertToBase64Async(imageBytes)`
   - `ResizeImageAsync(imageBytes, width, height)`
   - `ValidateImageAsync(imageBytes, maxSizeKb, allowedFormats)`

2. **Implementar ImageService**

   ```
   Infrastructure/Services/ImageService.cs (~150 líneas)
   ```

   - Usar System.Drawing o ImageSharp
   - Validar formatos (JPEG, PNG, WebP)
   - Validar tamaños (max 5MB)
   - Redimensionar para thumbnails

**FASE 3: Other Utilities (1-2 horas)**

1. **Migrar NumeroEnLetras.cs** (si aún no migrado)

   ```
   Infrastructure/Utilities/NumberToWordsConverter.cs (~100 líneas)
   ```

   - Convertir números a letras (para contratos/recibos)
   - Idioma: Español (República Dominicana)

2. **Crear DateTimeExtensions**

   ```
   Application/Common/Extensions/DateTimeExtensions.cs (~80 líneas)
   ```

   - `ToRelativeDateString()` - "Hace 2 días", "Hace 3 meses"
   - `ToShortDateString()` - Formato dominicano
   - `IsBusinessDay()` - Excluir fines de semana y feriados

#### Archivos a Crear (Total: 10 archivos, ~700 líneas)

**Application Layer:**

- `Common/Interfaces/IPdfService.cs` (~40 líneas)
- `Common/Interfaces/IImageService.cs` (~30 líneas)
- `Common/Extensions/DateTimeExtensions.cs` (~80 líneas)

**Infrastructure Layer:**

- `Services/PdfService.cs` (~200 líneas)
- `Services/ImageService.cs` (~150 líneas)
- `Utilities/NumberToWordsConverter.cs` (~100 líneas)
- `Templates/Pdf/ContratoTemplate.html` (~100 líneas)
- `Templates/Pdf/ReciboTemplate.html` (~100 líneas)

**Tests:**

- `Tests/Infrastructure.Tests/Services/PdfServiceTests.cs` (~80 líneas)
- `Tests/Infrastructure.Tests/Services/ImageServiceTests.cs` (~60 líneas)

#### Métricas de Éxito

- ✅ PDF de contrato se genera correctamente
- ✅ PDF de recibo se genera correctamente
- ✅ Imágenes se convierten a Data URL correctamente
- ✅ Redimensionamiento de imágenes funciona
- ✅ Conversión de números a letras correcta (1234 → "Mil doscientos treinta y cuatro")
- ✅ Fechas relativas se calculan correctamente
- ✅ Build sin errores

---

### LOTE 5.4: Bot Integration (OpenAI) 🟢 BAJA

**Prioridad:** 🟢 **BAJA - OPCIONAL**  
**Estimación:** 3-4 días (24-32 horas) **SI SE DECIDE IMPLEMENTAR**  
**Estado:** ❌ NO INICIADO

#### Análisis de Legacy

**BotServices.cs (15 líneas - 1 método):**

```csharp
public class BotServices
{
    public OpenAi_Config getOpenAI()
    {
        using (var db = new migenteEntities())
        {
            return db.OpenAi_Config.FirstOrDefault();
        }
    }
}
```

**Página Legacy:**

- `/abogadoVirtual.aspx` - Chat con "abogado virtual" usando OpenAI GPT

**Contexto de Negocio:**

- Asistente legal especializado en leyes laborales de República Dominicana
- Responde preguntas sobre TSS, contratos, derechos laborales
- NO es funcionalidad crítica para MVP

#### Tareas de Implementación (OPCIONAL)

**FASE 1: OpenAI Service (2 días)**

1. Crear IOpenAiService interface
2. Implementar OpenAiService usando Azure.AI.OpenAI SDK
3. Configurar API Key y modelo (GPT-4)
4. Implementar chat con contexto (historial de conversación)

**FASE 2: Commands & Queries (1 día)**

1. SendChatMessageCommand - Enviar mensaje al bot
2. GetChatHistoryQuery - Obtener historial de chat
3. ClearChatHistoryCommand - Limpiar historial

**FASE 3: Controller (1 día)**

1. BotController con 3 endpoints:
   - `POST /api/bot/chat` - Enviar mensaje
   - `GET /api/bot/history/{userId}` - Obtener historial
   - `DELETE /api/bot/history/{userId}` - Limpiar historial

**RECOMENDACIÓN:**
⚠️ **POSPONER** hasta post-MVP. Costo adicional de API OpenAI (~$0.03/1K tokens).

---

### LOTE 5.5: Contrataciones Avanzadas 🔴 ALTA

**Prioridad:** 🔴 **ALTA**  
**Estimación:** 2-3 días (16-24 horas)  
**Estado:** ❌ NO INICIADO

#### Análisis de Legacy

**Páginas Legacy con lógica de contrataciones:**

1. `/Empleador/detalleContratacion.aspx` (~200 líneas C#)
   - Ver detalles de una contratación
   - Cambiar estado (Pendiente → Aceptada → En Progreso → Completada)
   - Calificar contratista al completar

2. `/Empleador/colaboradores.aspx` (~100 líneas C#)
   - Listar todas las contrataciones
   - Filtrar por estado
   - Búsqueda por nombre de contratista

3. `/Empleador/fichaColaboradorTemporal.aspx` (~100 líneas C#)
   - Crear nueva contratación
   - Asignar contratista a trabajo temporal

**Entidades Involucradas:**

- `DetalleContrataciones` (ya migrada en PLAN 4)
- `Contrataciones` (ya migrada)
- Estados: Pendiente (1), Aceptada (2), En Progreso (3), Completada (4), Cancelada (5), Rechazada (6)

#### Gap Identificado

**Funcionalidad Existente en Clean:**

- ✅ `CreateDetalleContratacionCommand` (existe pero básico)
- ✅ Repository pattern completo (PLAN 4 - LOTE 5)

**Funcionalidad Faltante:**

- ❌ `ChangeContratacionStatusCommand` - Cambiar estado de contratación
- ❌ `AcceptContratacionCommand` - Contratista acepta trabajo
- ❌ `RejectContratacionCommand` - Contratista rechaza trabajo
- ❌ `StartContratacionCommand` - Iniciar trabajo (Aceptada → En Progreso)
- ❌ `CompleteContratacionCommand` - Completar trabajo (En Progreso → Completada)
- ❌ `CancelContratacionCommand` - Cancelar trabajo
- ❌ `GetContratacionesByEmpleadorQuery` - Listar contrataciones de empleador (con filtros)
- ❌ `GetContratacionesByContratistaQuery` - Listar contrataciones de contratista
- ❌ `GetContratacionesPendientesQuery` - Contrataciones pendientes de aceptación
- ❌ `SearchContratacionesQuery` - Búsqueda avanzada

#### Tareas de Implementación

**FASE 1: Commands (1.5 días, 12 archivos, ~900 líneas)**

1. **ChangeContratacionStatusCommand** (Estado genérico)
2. **AcceptContratacionCommand** (Específico: Pendiente → Aceptada)
3. **RejectContratacionCommand** (Específico: Pendiente → Rechazada)
4. **StartContratacionCommand** (Específico: Aceptada → En Progreso)
5. **CompleteContratacionCommand** (Específico: En Progreso → Completada)
6. **CancelContratacionCommand** (Cualquier estado → Cancelada)

**Business Rules:**

- Solo empleador puede cancelar
- Solo contratista puede aceptar/rechazar
- Solo puede completar si está en progreso
- Al completar, crear notificación para calificar

**FASE 2: Queries (1 día, 8 archivos, ~600 líneas)**

1. **GetContratacionesByEmpleadorQuery** (paginado, filtros por estado)
2. **GetContratacionesByContratistaQuery** (paginado, filtros)
3. **GetContratacionesPendientesQuery** (para dashboard)
4. **SearchContratacionesQuery** (búsqueda por nombre, servicio, fecha)

**FASE 3: Controller & DTOs (4 horas, 3 archivos, ~250 líneas)**

1. Actualizar `ContratacionesController` (si existe) o crear nuevo
2. Crear DTOs: `ContratacionDetalleDto`, `ContratacionListDto`
3. Endpoints:
   - `POST /api/contrataciones/{id}/accept` ✅
   - `POST /api/contrataciones/{id}/reject` ✅
   - `POST /api/contrataciones/{id}/start` ✅
   - `POST /api/contrataciones/{id}/complete` ✅
   - `POST /api/contrataciones/{id}/cancel` ✅
   - `GET /api/contrataciones/empleador/{empleadorId}` ✅
   - `GET /api/contrataciones/contratista/{contratistaId}` ✅
   - `GET /api/contrataciones/pendientes` ✅

#### Archivos a Crear (Total: 23 archivos, ~1,750 líneas)

**Application Layer:**

- 6 Commands + 6 Handlers + 6 Validators = 18 archivos
- 4 Queries + 4 Handlers = 8 archivos
- 3 DTOs

**Presentation Layer:**

- ContratacionesController.cs (actualizar o crear)

**Tests:**

- Command tests, Query tests, Controller tests

**Documentation:**

- LOTE_5_5_CONTRATACIONES_AVANZADAS_COMPLETADO.md

#### Métricas de Éxito

- ✅ Flujo completo de contratación funciona (Pendiente → Completada)
- ✅ Validaciones de estado correctas
- ✅ Solo roles autorizados pueden ejecutar acciones
- ✅ Notificaciones se crean al cambiar estado
- ✅ 8 endpoints REST funcionando
- ✅ Build sin errores

---

### LOTE 5.6: Nómina Avanzada 🟡 MEDIA

**Prioridad:** 🟡 **MEDIA**  
**Estimación:** 2 días (12-16 horas)  
**Estado:** ❌ NO INICIADO

#### Análisis de Legacy

**Página Legacy:**

- `/Empleador/nomina.aspx` (~300 líneas C#)
  - Procesar nómina masiva (múltiples empleados)
  - Calcular deducciones TSS automáticamente
  - Generar recibos de pago en lote
  - Exportar nómina a Excel

**Funcionalidad Actual en Clean:**

- ✅ `ProcesarPagoCommand` - Procesar pago individual (LOTE 4)
- ✅ Cálculo de deducciones TSS básico

**Funcionalidad Faltante:**

- ❌ `ProcesarNominaLoteCommand` - Procesar múltiples empleados
- ❌ `GenerarRecibosPdfCommand` - Generar PDFs en lote
- ❌ `ExportNominaToExcelQuery` - Exportar a Excel
- ❌ `GetResumenNominaQuery` - Resumen de nómina (total salarios, deducciones, neto)
- ❌ `ValidarNominaQuery` - Validar antes de procesar (empleados activos, salarios válidos)

#### Tareas de Implementación

**FASE 1: Procesamiento en Lote (1 día, 6 archivos, ~500 líneas)**

1. **ProcesarNominaLoteCommand**
   - Recibir array de EmpleadoId
   - Validar todos los empleados
   - Procesar en transacción (todo o nada)
   - Generar recibos para todos
   - Retornar ResumenNominaDto

2. **ValidarNominaCommand**
   - Validar empleados activos
   - Validar salarios > 0
   - Validar deducciones calculadas correctamente

**FASE 2: Generación de Reportes (1 día, 4 archivos, ~400 líneas)**

1. **GenerarRecibosPdfCommand**
   - Usar PdfService (LOTE 5.3)
   - Generar PDF por empleado
   - Comprimir en ZIP si son múltiples

2. **ExportNominaToExcelQuery**
   - Usar EPPlus o ClosedXML
   - Exportar datos de nómina procesada
   - Incluir resumen y detalles

3. **GetResumenNominaQuery**
   - Calcular totales (salarios, deducciones, neto)
   - Agrupar por departamento/sector
   - Retornar ResumenNominaDto

#### Archivos a Crear (Total: 10 archivos, ~900 líneas)

**Application Layer:**

- 2 Commands + 2 Handlers + 2 Validators = 6 archivos
- 3 Queries + 3 Handlers = 6 archivos
- 2 DTOs (ResumenNominaDto, NominaLoteResultDto)

**Presentation Layer:**

- Actualizar NominasController con 4 endpoints

**Tests:**

- Command tests, Query tests

**Documentation:**

- LOTE_5_6_NOMINA_AVANZADA_COMPLETADO.md

#### NuGet Packages

```bash
dotnet add package EPPlus --version 7.0.0
# o
dotnet add package ClosedXML --version 0.102.1
```

#### Métricas de Éxito

- ✅ Procesar 10+ empleados en un solo comando
- ✅ Transacción se revierte si hay error en alguno
- ✅ PDF de recibos se genera correctamente
- ✅ Exportación a Excel funciona
- ✅ Resumen de nómina correcto
- ✅ Build sin errores

---

### LOTE 5.7: Dashboard & Reports 🟡 MEDIA

**Prioridad:** 🟡 **MEDIA**  
**Estimación:** 1-2 días (8-16 horas)  
**Estado:** ❌ NO INICIADO

#### Análisis de Legacy

**Páginas Legacy:**

- `/comunidad.aspx` - Dashboard de empleador (~100 líneas C#)
- `/Dashboard.aspx` - Dashboard general (~100 líneas C#)
- `/Contratista/index_contratista.aspx` - Dashboard de contratista (~100 líneas C#)

**Datos mostrados en Dashboard:**

**Empleador:**

- Total empleados activos
- Total nómina mensual
- Empleados dados de alta este mes
- Contrataciones pendientes
- Calificaciones pendientes
- Estado de suscripción

**Contratista:**

- Total calificaciones
- Promedio de rating
- Contrataciones activas
- Contrataciones completadas
- Ingresos totales
- Estado de suscripción

#### Tareas de Implementación

**FASE 1: Queries de Dashboard (1 día, 8 archivos, ~600 líneas)**

1. **GetDashboardEmpleadorQuery**
   - Retornar DashboardEmpleadorDto con todas las métricas

2. **GetDashboardContratistaQuery**
   - Retornar DashboardContratistaDto con todas las métricas

3. **GetEstadisticasEmpleadorQuery**
   - Gráficos: Empleados por mes, nómina por mes

4. **GetEstadisticasContratistaQuery**
   - Gráficos: Contrataciones por mes, ingresos por mes

**FASE 2: Controller & DTOs (4 horas, 3 archivos, ~200 líneas)**

1. Crear `DashboardController`
2. DTOs: `DashboardEmpleadorDto`, `DashboardContratistaDto`
3. Endpoints:
   - `GET /api/dashboard/empleador/{userId}` ✅
   - `GET /api/dashboard/contratista/{userId}` ✅
   - `GET /api/dashboard/empleador/{userId}/estadisticas` ✅
   - `GET /api/dashboard/contratista/{userId}/estadisticas` ✅

**FASE 3: Optimización (4 horas)**

1. Implementar caching para métricas (IMemoryCache)
2. Cache expiration: 5 minutos
3. Invalidar cache al crear/actualizar datos

#### Archivos a Crear (Total: 11 archivos, ~800 líneas)

**Application Layer:**

- 4 Queries + 4 Handlers = 8 archivos
- 4 DTOs

**Presentation Layer:**

- DashboardController.cs

**Tests:**

- Query tests, Controller tests

**Documentation:**

- LOTE_5_7_DASHBOARD_REPORTS_COMPLETADO.md

#### Métricas de Éxito

- ✅ Dashboard de empleador carga en < 1 segundo
- ✅ Dashboard de contratista carga en < 1 segundo
- ✅ Métricas correctas
- ✅ Gráficos con datos correctos
- ✅ Caching funciona correctamente
- ✅ Build sin errores

---

## 📊 MÉTRICAS DEL PLAN 5

### Resumen de LOTEs

| LOTE | Nombre | Prioridad | Estimación | Archivos | Líneas | Estado |
|------|--------|-----------|-----------|----------|--------|--------|
| 5.1 | EmailService | 🔴 CRÍTICA | 1-2 días | 12 | ~600 | ❌ |
| 5.2 | Calificaciones | 🔴 ALTA | 2-3 días | 20 | ~1,400 | ❌ |
| 5.3 | Utilities | 🟡 MEDIA | 1 día | 10 | ~700 | ❌ |
| 5.4 | Bot (OpenAI) | 🟢 BAJA | 3-4 días | 15 | ~1,000 | ⏸️ POSPONER |
| 5.5 | Contrataciones | 🔴 ALTA | 2-3 días | 23 | ~1,750 | ❌ |
| 5.6 | Nómina Avanzada | 🟡 MEDIA | 2 días | 10 | ~900 | ❌ |
| 5.7 | Dashboard | 🟡 MEDIA | 1-2 días | 11 | ~800 | ❌ |

**Total (sin Bot):**

- **Tiempo:** 11-16 días (~88-128 horas)
- **Archivos:** 86 archivos
- **Líneas:** ~6,150 líneas
- **LOTEs:** 6 (sin contar Bot)

### Priorización Recomendada

#### Sprint 1 (Semana 1-2): CRÍTICO

1. **LOTE 5.1: EmailService** (1-2 días) 🔴 CRÍTICA
2. **LOTE 5.2: Calificaciones** (2-3 días) 🔴 ALTA
3. **LOTE 5.5: Contrataciones** (2-3 días) 🔴 ALTA

**Total Sprint 1:** 5-8 días

#### Sprint 2 (Semana 3-4): IMPORTANTE

4. **LOTE 5.3: Utilities** (1 día) 🟡 MEDIA
5. **LOTE 5.6: Nómina Avanzada** (2 días) 🟡 MEDIA
6. **LOTE 5.7: Dashboard** (1-2 días) 🟡 MEDIA

**Total Sprint 2:** 4-5 días

#### Sprint 3 (Post-MVP): OPCIONAL

7. **LOTE 5.4: Bot Integration** (3-4 días) 🟢 BAJA

**Total Sprint 3:** 3-4 días (OPCIONAL)

---

## 🎯 CHECKLIST DE VALIDACIÓN

### Por Cada LOTE

- [ ] ✅ Análisis de Legacy completo
- [ ] ✅ Gap analysis documentado
- [ ] ✅ Tareas de implementación definidas
- [ ] ✅ Archivos creados según plan
- [ ] ✅ Build sin errores (0 errors, 0 warnings nuevos)
- [ ] ✅ Unit tests escritos y pasando
- [ ] ✅ Integration tests escritos y pasando
- [ ] ✅ Documentación `LOTE_X_COMPLETADO.md` creada
- [ ] ✅ Commit con mensaje descriptivo
- [ ] ✅ PR creado y revisado
- [ ] ✅ Merge a branch principal

### Validación Final del PLAN 5

- [ ] ✅ Todos los LOTEs críticos completados (5.1, 5.2, 5.5)
- [ ] ✅ Todos los LOTEs medios completados (5.3, 5.6, 5.7)
- [ ] ✅ 100% paridad funcional Legacy vs Clean
- [ ] ✅ Testing comprehensivo (80%+ coverage)
- [ ] ✅ Performance testing realizado
- [ ] ✅ Security audit validation
- [ ] ✅ API documentation completa (Swagger)
- [ ] ✅ Reporte final PLAN_5_COMPLETADO_100.md

---

## 📚 PRÓXIMOS PASOS

### Acción Inmediata (HOY)

```bash
# 1. Crear branch para LOTE 5.1
git checkout -b feature/lote-5.1-email-service

# 2. Instalar NuGet packages
cd src/Infrastructure/MiGenteEnLinea.Infrastructure
dotnet add package MailKit --version 4.3.0

# 3. Implementar EmailService (8-16 horas)
# 4. Testing (2-3 horas)
# 5. PR y merge
```

### Esta Semana

1. **Día 1-2:** LOTE 5.1 - EmailService ✅
2. **Día 3-5:** LOTE 5.2 - Calificaciones ✅
3. **Día 6-7:** LOTE 5.5 - Contrataciones (inicio) 🔄

### Próximas Semanas

- **Semana 2:** Completar LOTE 5.5 + iniciar 5.3
- **Semana 3-4:** LOTEs 5.6 y 5.7
- **Post-MVP:** LOTE 5.4 (Bot) si el cliente lo solicita

---

## 🎉 CONCLUSIÓN

**PLAN 5** cubre TODA la lógica de negocio faltante identificada entre Legacy y Clean Architecture. Al completar este plan:

- ✅ **100% paridad funcional** con sistema Legacy
- ✅ **EmailService desbloqueado** (registro funcional)
- ✅ **Sistema de calificaciones completo** (reviews/ratings)
- ✅ **Contrataciones avanzadas** (flujo completo)
- ✅ **Nómina en lote** (procesamiento masivo)
- ✅ **Dashboard & Reports** (métricas de negocio)
- ✅ **Utilities & Helpers** (PDF, imágenes, conversiones)
- ⏸️ **Bot Integration** (opcional, post-MVP)

**Estado Post-PLAN 5:** Backend Clean Architecture **100% completo** y listo para frontend.

---

**Elaborado por:** GitHub Copilot AI Agent  
**Fecha:** 2025-01-18  
**Versión:** 1.0  
**Siguiente Plan:** PLAN 6 - Frontend Migration
