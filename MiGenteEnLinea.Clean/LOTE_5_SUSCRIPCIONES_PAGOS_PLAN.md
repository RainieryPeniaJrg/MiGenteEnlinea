# 📋 LOTE 5: SUSCRIPCIONES Y PAGOS - PLAN DE IMPLEMENTACIÓN

**Fecha de Creación:** 15 de octubre, 2025  
**Estado:** PLANIFICACIÓN  
**Prioridad:** 🟡 MEDIA (después de completar LOTE 4)  
**Complejidad:** 🔴 ALTA (Integración con Payment Gateway externo)

---

## 🎯 OBJETIVO

Migrar completamente el módulo de Suscripciones y Pagos desde Legacy (SuscripcionesService.cs + PaymentService.cs) hacia Clean Architecture usando CQRS con MediatR, incluyendo integración con Cardnet Payment Gateway.

---

## 📊 ANÁLISIS DE SERVICIOS LEGACY

### 1. SuscripcionesService.cs - Análisis Completo

**Ubicación:** `Codigo Fuente Mi Gente/MiGente_Front/Services/SuscripcionesService.cs`  
**Total Métodos:** 17  
**Métodos Migrados en LOTE 1:** 7  
**Métodos Pendientes:** 10

#### Métodos Ya Migrados (LOTE 1: Authentication)

| # | Método Legacy | Migrado Como | Estado |
|---|---------------|--------------|--------|
| 1 | `GuardarPerfil()` | RegisterCommand | ✅ LOTE 1 |
| 2 | `guardarNuevoContratista()` | RegisterCommand (interno) | ✅ LOTE 1 |
| 3 | `enviarCorreoActivacion()` | RegisterCommand (IEmailService) | ✅ LOTE 1 |
| 4 | `guardarCredenciales()` | RegisterCommand (crear Credencial) | ✅ LOTE 1 |
| 5 | `actualizarPass()` | ChangePasswordCommand | ✅ LOTE 1 |
| 6 | `actualizarPassByID()` | ChangePasswordCommand (variante) | ✅ LOTE 1 |
| 7 | `actualizarCredenciales()` | UpdateProfileCommand | ✅ LOTE 1 |
| 8 | `validarCorreo()` | ValidarCorreoQuery | ✅ LOTE 1 |
| 9 | `validarCorreoCuentaActual()` | ValidarCorreoQuery (variante) | ✅ LOTE 1 |

#### Métodos Pendientes (LOTE 5)

| # | Método Legacy | Descripción | Params | Return | Complejidad |
|---|---------------|-------------|--------|--------|-------------|
| 10 | `obtenerCedula(userID)` | Obtiene identificación de contratista | userID | string (cédula) | 🟢 BAJA |
| 11 | `obtenerSuscripcion(userID)` | Obtiene suscripción más reciente | userID | ObtenerSuscripcion_Result | 🟡 MEDIA |
| 12 | `actualizarSuscripcion(suscripcion)` | Actualiza plan y vencimiento | Suscripciones | Suscripciones | 🟢 BAJA |
| 13 | `obtenerPlanes()` | Lista todos los planes de empleadores | - | List<Planes_empleadores> | 🟢 BAJA |
| 14 | `obtenerPlanesContratistas()` | Lista todos los planes de contratistas | - | List<Planes_Contratistas> | 🟢 BAJA |
| 15 | `procesarVenta(venta)` | Registra venta/checkout | Ventas | bool | 🟡 MEDIA |
| 16 | `guardarSuscripcion(suscripcion)` | Crea nueva suscripción | Suscripciones | bool | 🟡 MEDIA |
| 17 | `obtenerDetalleVentasBySuscripcion(userID)` | Lista ventas de usuario | userID | List<Ventas> | 🟢 BAJA |

---

### 2. PaymentService.cs - Análisis Completo

**Ubicación:** `Codigo Fuente Mi Gente/MiGente_Front/Services/PaymentService.cs`  
**Total Métodos:** 3  
**Complejidad:** 🔴 ALTA (Integración externa)

#### Métodos a Migrar

| # | Método Legacy | Descripción | Params | Return | Complejidad |
|---|---------------|-------------|--------|--------|-------------|
| 1 | `consultarIdempotency(url)` | Genera idempotency key para Cardnet | url | dynamic (ikey) | 🟡 MEDIA |
| 2 | `Payment(...)` | Procesa pago con Cardnet Gateway | 7 params | PaymentResponse | 🔴 ALTA |
| 3 | `getPaymentParameters()` | Obtiene configuración gateway | - | PaymentGateway | 🟢 BAJA |

#### Lógica Crítica a Replicar

**Payment() - Flow Completo:**

```csharp
// PASO 1: Obtener configuración (test vs production)
var gatewayParams = getPaymentParameters();
string url = gatewayParams.test ? gatewayParams.testURL : gatewayParams.productionURL;

// PASO 2: Generar idempotency key (prevenir duplicados)
var result = await consultarIdempotency(url);
string idempotency = result.result?.Substring("ikey:".Length);

// PASO 3: Construir request JSON
var jsonBody = $@"{{
    ""amount"": {amount},
    ""card-number"":""{crypt.Decrypt(cardNumber)}"", // ⚠️ Desencriptar tarjeta
    ""client-ip"": ""{clientIP}"",
    ""currency"": ""214"", // Peso dominicano
    ""cvv"": ""{cvv}"",
    ""environment"": ""ECommerce"",
    ""expiration-date"": ""{expirationDate}"",
    ""idempotency-key"": ""{idempotency}"",
    ""merchant-id"": ""{gatewayParams.merchantID}"",
    ""reference-number"": ""{referenceNumber}"",
    ""terminal-id"": ""{gatewayParams.terminalID}"",
    ""token"": ""454500350001"", // ⚠️ Token fijo
    ""invoice-number"":""{invoiceNumber}""
}}";

// PASO 4: POST request a Cardnet
var request = new RestRequest(url + "sales", Method.Post);
request.AddHeader("Content-Type", "application/json");
request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);

// PASO 5: Deserializar respuesta
PaymentResponse paymentResponse = JsonConvert.DeserializeObject<PaymentResponse>(response.Content);
```

**PaymentResponse Structure:**

```csharp
public class PaymentResponse
{
    [JsonProperty(PropertyName = "idempotency-key")]
    public string IdempotencyKey { get; set; }
    
    [JsonProperty(PropertyName = "response-code")]
    public string ResponseCode { get; set; } // "00" = success
    
    [JsonProperty(PropertyName = "internal-response-code")]
    public string InternalResponseCode { get; set; }
    
    [JsonProperty(PropertyName = "response-code-desc")]
    public string ResponseCodeDesc { get; set; } // Mensaje descriptivo
    
    [JsonProperty(PropertyName = "response-code-source")]
    public string ResponseCodeSource { get; set; }
    
    [JsonProperty(PropertyName = "approval-code")]
    public string ApprovalCode { get; set; } // Código de aprobación
    
    [JsonProperty(PropertyName = "pnRef")]
    public string PnRef { get; set; } // Referencia de pago
}
```

**Códigos de Respuesta Cardnet:**

| Código | Descripción | Acción |
|--------|-------------|--------|
| 00 | Transacción aprobada | Guardar suscripción |
| 01 | Referirse al banco emisor | Rechazar |
| 05 | No aprobada | Rechazar |
| 14 | Tarjeta inválida | Rechazar |
| 51 | Fondos insuficientes | Rechazar |
| 54 | Tarjeta expirada | Rechazar |
| 96 | Falla en el sistema | Reintentar |

---

## 🏗️ ARQUITECTURA DE IMPLEMENTACIÓN

### Entidades Domain Involucradas

**Ya Existentes en Domain Layer:**

1. **Suscripcion** (Domain/Entities/Suscripciones/Suscripcion.cs)
   - Properties: SuscripcionId, UserId, PlanId, FechaInicio, Vencimiento, Activo
   - Factory Methods: Create(), CreateSinPlan()
   - Domain Methods: Renovar(), Cancelar(), CambiarPlan()

2. **PlanEmpleador** (Domain/Entities/Planes/PlanEmpleador.cs)
   - Read Model (solo lectura)
   - Properties: PlanId, Nombre, Precio, Duracion, CantidadEmpleados, etc.

3. **PlanContratista** (Domain/Entities/Planes/PlanContratista.cs)
   - Read Model (solo lectura)
   - Properties: PlanId, Nombre, Precio, Duracion

4. **Venta** (Domain/Entities/Ventas/Venta.cs)
   - Properties: VentaId, UserId, PlanId, Monto, FechaVenta, MetodoPago, etc.
   - Factory Method: Create()

---

## 📁 ESTRUCTURA DE ARCHIVOS A CREAR

### Application Layer - Features/Suscripciones/ (32 archivos, ~2,400 líneas estimadas)

#### Commands (18 archivos, ~1,400 líneas)

**1. CreateSuscripcionCommand** (3 archivos, ~180 líneas)

```
Features/Suscripciones/Commands/CreateSuscripcion/
├── CreateSuscripcionCommand.cs (30 líneas)
├── CreateSuscripcionCommandHandler.cs (95 líneas)
└── CreateSuscripcionCommandValidator.cs (55 líneas)
```

**Properties:**

- UserId (Guid, required)
- PlanId (int, required)
- FechaInicio (DateTime, default: DateTime.UtcNow)
- Vencimiento (DateTime, calculado desde FechaInicio + Plan.Duracion)

**Legacy Mapping:** `SuscripcionesService.guardarSuscripcion()`

**Lógica:**

1. Validar que userId existe
2. Validar que planId existe
3. Verificar si ya tiene suscripción activa (cancelar la anterior)
4. Crear suscripción con `Suscripcion.Create()`
5. Guardar en DbContext
6. Retornar SuscripcionId

---

**2. UpdateSuscripcionCommand** (3 archivos, ~160 líneas)

```
Features/Suscripciones/Commands/UpdateSuscripcion/
├── UpdateSuscripcionCommand.cs (25 líneas)
├── UpdateSuscripcionCommandHandler.cs (85 líneas)
└── UpdateSuscripcionCommandValidator.cs (50 líneas)
```

**Properties:**

- UserId (Guid, required)
- NuevoPlanId (int, required)
- NuevoVencimiento (DateTime, required)

**Legacy Mapping:** `SuscripcionesService.actualizarSuscripcion()`

**Lógica:**

1. Buscar suscripción activa por userId
2. Validar que nuevo planId existe
3. Llamar método de dominio `suscripcion.CambiarPlan(nuevoPlanId, nuevoVencimiento)`
4. Guardar cambios

---

**3. RenovarSuscripcionCommand** (3 archivos, ~170 líneas)

```
Features/Suscripciones/Commands/RenovarSuscripcion/
├── RenovarSuscripcionCommand.cs (28 líneas)
├── RenovarSuscripcionCommandHandler.cs (90 líneas)
└── RenovarSuscripcionCommandValidator.cs (52 líneas)
```

**Properties:**

- UserId (Guid, required)
- MesesExtension (int, default: 1)

**Nuevo:** No existe en Legacy, pero es útil

**Lógica:**

1. Buscar suscripción activa
2. Calcular nueva fecha vencimiento
3. Llamar `suscripcion.Renovar(nuevaFecha)`
4. Guardar cambios

---

**4. CancelarSuscripcionCommand** (3 archivos, ~150 líneas)

```
Features/Suscripciones/Commands/CancelarSuscripcion/
├── CancelarSuscripcionCommand.cs (22 líneas)
├── CancelarSuscripcionCommandHandler.cs (80 líneas)
└── CancelarSuscripcionCommandValidator.cs (48 líneas)
```

**Properties:**

- UserId (Guid, required)
- MotivoCancelacion (string, optional, max 250)

**Nuevo:** No existe en Legacy

**Lógica:**

1. Buscar suscripción activa
2. Llamar `suscripcion.Cancelar(motivo)`
3. Guardar cambios

---

**5. ProcesarVentaCommand** (3 archivos, ~240 líneas)

```
Features/Suscripciones/Commands/ProcesarVenta/
├── ProcesarVentaCommand.cs (45 líneas)
├── ProcesarVentaCommandHandler.cs (140 líneas)
└── ProcesarVentaCommandValidator.cs (55 líneas)
```

**Properties:**

- UserId (Guid, required)
- PlanId (int, required)
- Monto (decimal, required)
- MetodoPago (string, required: "Tarjeta", "Transferencia")
- NumeroTarjeta (string, optional, encrypted)
- CVV (string, optional)
- FechaExpiracion (string, optional)
- ClienteIP (string, required)

**Legacy Mapping:** `SuscripcionesService.procesarVenta()` + `PaymentService.Payment()`

**Lógica Crítica:**

```csharp
// PASO 1: Validar plan existe
var plan = await _context.PlanesEmpleadores.FindAsync(command.PlanId);
if (plan == null) throw new NotFoundException("Plan no encontrado");

// PASO 2: Si pago con tarjeta, procesar con Cardnet
PaymentResult paymentResult = null;
if (command.MetodoPago == "Tarjeta")
{
    paymentResult = await _paymentService.ProcessPaymentAsync(new PaymentRequest
    {
        Amount = command.Monto,
        CardNumber = command.NumeroTarjeta,
        CVV = command.CVV,
        ExpirationDate = command.FechaExpiracion,
        ClientIP = command.ClienteIP,
        ReferenceNumber = Guid.NewGuid().ToString(),
        InvoiceNumber = $"SUB-{command.UserId}-{DateTime.UtcNow:yyyyMMddHHmmss}"
    });
    
    // Si pago rechazado, lanzar excepción
    if (paymentResult.ResponseCode != "00")
    {
        throw new PaymentFailedException(paymentResult.ResponseCodeDesc);
    }
}

// PASO 3: Crear registro de venta
var venta = Venta.Create(
    userId: command.UserId,
    planId: command.PlanId,
    monto: command.Monto,
    metodoPago: command.MetodoPago,
    aprobadoPor: paymentResult?.ApprovalCode,
    referenciaTransaccion: paymentResult?.PnRef
);
_context.Ventas.Add(venta);

// PASO 4: Crear o actualizar suscripción
var suscripcionExistente = await _context.Suscripciones
    .Where(s => s.UserId == command.UserId && s.Activo)
    .FirstOrDefaultAsync();

if (suscripcionExistente != null)
{
    // Renovar existente
    suscripcionExistente.Renovar(plan.Duracion);
}
else
{
    // Crear nueva
    var nuevaSuscripcion = Suscripcion.Create(
        userId: command.UserId,
        planId: command.PlanId,
        fechaInicio: DateTime.UtcNow,
        vencimiento: DateTime.UtcNow.AddMonths(plan.Duracion)
    );
    _context.Suscripciones.Add(nuevaSuscripcion);
}

await _context.SaveChangesAsync();

// PASO 5: Enviar email confirmación (IEmailService)
await _emailService.SendSuscripcionActivadaEmailAsync(command.UserId, plan.Nombre);

return venta.VentaId;
```

---

**6. ProcesarVentaSinPagoCommand** (3 archivos, ~140 líneas)

```
Features/Suscripciones/Commands/ProcesarVentaSinPago/
├── ProcesarVentaSinPagoCommand.cs (25 líneas)
├── ProcesarVentaSinPagoCommandHandler.cs (85 líneas)
└── ProcesarVentaSinPagoCommandValidator.cs (30 líneas)
```

**Propósito:** Para planes gratuitos o pruebas

**Properties:**

- UserId (Guid, required)
- PlanId (int, required)

**Lógica:**

1. Validar que plan es gratuito (Precio == 0)
2. Crear venta con MetodoPago = "Gratuito"
3. Crear suscripción
4. Enviar email confirmación

---

#### Queries (8 archivos, ~600 líneas)

**1. GetSuscripcionActivaQuery** (2 archivos, ~120 líneas)

```
Features/Suscripciones/Queries/GetSuscripcionActiva/
├── GetSuscripcionActivaQuery.cs (18 líneas)
└── GetSuscripcionActivaQueryHandler.cs (102 líneas)
```

**Properties:**

- UserId (Guid, required)

**Legacy Mapping:** `SuscripcionesService.obtenerSuscripcion()`

**Response DTO:** SuscripcionDetalleDto

**Lógica:**

```csharp
// Query con Include para Plan
var suscripcion = await _context.Suscripciones
    .Where(s => s.UserId == command.UserId)
    .Include(s => s.Plan) // PlanEmpleador o PlanContratista
    .OrderByDescending(s => s.FechaInicio) // ⚠️ Más reciente
    .FirstOrDefaultAsync();

if (suscripcion == null)
    return null; // Usuario sin suscripción

return new SuscripcionDetalleDto
{
    SuscripcionId = suscripcion.SuscripcionId,
    UserId = suscripcion.UserId,
    PlanId = suscripcion.PlanId,
    PlanNombre = suscripcion.Plan.Nombre,
    FechaInicio = suscripcion.FechaInicio,
    Vencimiento = suscripcion.Vencimiento,
    DiasFaltantes = (suscripcion.Vencimiento - DateTime.UtcNow).Days,
    Activo = suscripcion.Activo,
    EstaVencida = suscripcion.Vencimiento < DateTime.UtcNow
};
```

---

**2. GetPlanesEmpleadoresQuery** (2 archivos, ~90 líneas)

```
Features/Suscripciones/Queries/GetPlanesEmpleadores/
├── GetPlanesEmpleadoresQuery.cs (12 líneas)
└── GetPlanesEmpleadoresQueryHandler.cs (78 líneas)
```

**Properties:** None (obtiene todos)

**Legacy Mapping:** `SuscripcionesService.obtenerPlanes()`

**Response DTO:** List<PlanEmpleadorDto>

**Lógica:**

```csharp
var planes = await _context.PlanesEmpleadores
    .OrderBy(p => p.Precio) // Ordenar por precio
    .ToListAsync();

return planes.Select(p => new PlanEmpleadorDto
{
    PlanId = p.PlanId,
    Nombre = p.Nombre,
    Precio = p.Precio,
    Duracion = p.Duracion, // Meses
    CantidadEmpleados = p.Nomina,
    Descripcion = p.Descripcion,
    Beneficios = new[]
    {
        p.Nomina > 0 ? $"Hasta {p.Nomina} empleados" : "Empleados ilimitados",
        p.Servicios > 0 ? $"Hasta {p.Servicios} servicios" : "Servicios ilimitados",
        // ... más beneficios
    }
}).ToList();
```

---

**3. GetPlanesContratistasQuery** (2 archivos, ~80 líneas)

```
Features/Suscripciones/Queries/GetPlanesContratistas/
├── GetPlanesContratistasQuery.cs (12 líneas)
└── GetPlanesContratistasQueryHandler.cs (68 líneas)
```

**Properties:** None

**Legacy Mapping:** `SuscripcionesService.obtenerPlanesContratistas()`

**Response DTO:** List<PlanContratistaDto>

---

**4. GetVentasByUserIdQuery** (2 archivos, ~110 líneas)

```
Features/Suscripciones/Queries/GetVentasByUserId/
├── GetVentasByUserIdQuery.cs (22 líneas)
└── GetVentasByUserIdQueryHandler.cs (88 líneas)
```

**Properties:**

- UserId (Guid, required)
- PageIndex (int, default: 1)
- PageSize (int, default: 20)

**Legacy Mapping:** `SuscripcionesService.obtenerDetalleVentasBySuscripcion()`

**Response DTO:** PaginatedList<VentaDto>

**Lógica:**

```csharp
var query = _context.Ventas
    .Where(v => v.UserId == command.UserId)
    .Include(v => v.Plan)
    .OrderByDescending(v => v.FechaVenta);

var ventas = await PaginatedList<Venta>.CreateAsync(
    query,
    command.PageIndex,
    command.PageSize
);

return new PaginatedList<VentaDto>(
    ventas.Items.Select(v => new VentaDto
    {
        VentaId = v.VentaId,
        FechaVenta = v.FechaVenta,
        PlanNombre = v.Plan.Nombre,
        Monto = v.Monto,
        MetodoPago = v.MetodoPago,
        Estado = v.AprobadoPor != null ? "Aprobado" : "Pendiente"
    }).ToList(),
    ventas.TotalCount,
    command.PageIndex,
    command.PageSize
);
```

---

#### DTOs (6 archivos, ~400 líneas)

1. **SuscripcionDetalleDto.cs** (70 líneas)
   - SuscripcionId, UserId, PlanId, PlanNombre
   - FechaInicio, Vencimiento, DiasFaltantes
   - Activo, EstaVencida (computed)

2. **PlanEmpleadorDto.cs** (80 líneas)
   - PlanId, Nombre, Precio, Duracion
   - CantidadEmpleados, CantidadServicios
   - Descripcion, Beneficios (array)

3. **PlanContratistaDto.cs** (70 líneas)
   - PlanId, Nombre, Precio, Duracion
   - Descripcion, Beneficios

4. **VentaDto.cs** (60 líneas)
   - VentaId, UserId, PlanId, PlanNombre
   - FechaVenta, Monto, MetodoPago
   - AprobadoPor, ReferenciaTransaccion, Estado

5. **CreateSuscripcionResult.cs** (40 líneas)
   - SuscripcionId, Success, Message

6. **ProcesarVentaResult.cs** (80 líneas)
   - VentaId, SuscripcionId
   - PaymentResponseCode, PaymentApprovalCode
   - Success, Message

---

### Infrastructure Layer - Payment Gateway (5 archivos, ~600 líneas)

#### Services/CardnetPaymentService.cs (300 líneas)

**Ubicación:** `Infrastructure/Services/CardnetPaymentService.cs`

**Dependencies:**

- IHttpClientFactory (named client "CardnetAPI")
- ILogger<CardnetPaymentService>
- IOptions<CardnetSettings>

**Methods:**

```csharp
public class CardnetPaymentService : IPaymentService
{
    // 1. Procesar pago principal
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request, CancellationToken ct = default)
    {
        // PASO 1: Generar idempotency key
        var idempotencyKey = await GenerateIdempotencyKeyAsync(ct);
        
        // PASO 2: Construir request JSON
        var requestBody = new
        {
            amount = request.Amount,
            card_number = DecryptCardNumber(request.CardNumber),
            client_ip = request.ClientIP,
            currency = "214", // Peso dominicano
            cvv = request.CVV,
            environment = "ECommerce",
            expiration_date = request.ExpirationDate,
            idempotency_key = idempotencyKey,
            merchant_id = _settings.MerchantId,
            reference_number = request.ReferenceNumber,
            terminal_id = _settings.TerminalId,
            token = "454500350001",
            invoice_number = request.InvoiceNumber
        };
        
        // PASO 3: POST a Cardnet
        var response = await _httpClient.PostAsJsonAsync("sales", requestBody, ct);
        response.EnsureSuccessStatusCode();
        
        // PASO 4: Deserializar respuesta
        var result = await response.Content.ReadFromJsonAsync<CardnetResponse>(ct);
        
        // PASO 5: Mapear a PaymentResult
        return new PaymentResult
        {
            Success = result.ResponseCode == "00",
            ResponseCode = result.ResponseCode,
            ResponseDescription = result.ResponseCodeDesc,
            ApprovalCode = result.ApprovalCode,
            TransactionReference = result.PnRef,
            IdempotencyKey = result.IdempotencyKey
        };
    }
    
    // 2. Generar idempotency key
    private async Task<string> GenerateIdempotencyKeyAsync(CancellationToken ct)
    {
        var response = await _httpClient.PostAsync("idenpotency-keys", null, ct);
        var plainText = await response.Content.ReadAsStringAsync(ct);
        return plainText.Substring("ikey:".Length);
    }
    
    // 3. Desencriptar número de tarjeta (BCrypt o AES)
    private string DecryptCardNumber(string encryptedCard)
    {
        // TODO: Implementar desencriptación
        // Legacy usa Crypt.Decrypt() de ClassLibrary_CSharp
        return encryptedCard;
    }
    
    // 4. Obtener configuración gateway desde DB
    public async Task<PaymentGatewayConfig> GetConfigurationAsync(CancellationToken ct)
    {
        // Query a tabla PaymentGateway
        var config = await _context.PaymentGateway.FirstOrDefaultAsync(ct);
        return new PaymentGatewayConfig
        {
            MerchantId = config.MerchantID,
            TerminalId = config.TerminalID,
            BaseUrl = config.Test ? config.TestURL : config.ProductionURL,
            IsTest = config.Test
        };
    }
}
```

---

#### Interfaces/IPaymentService.cs (80 líneas)

```csharp
public interface IPaymentService
{
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request, CancellationToken ct = default);
    Task<PaymentGatewayConfig> GetConfigurationAsync(CancellationToken ct = default);
}

public record PaymentRequest
{
    public decimal Amount { get; init; }
    public string CardNumber { get; init; } = null!; // Encrypted
    public string CVV { get; init; } = null!;
    public string ExpirationDate { get; init; } = null!; // MM/YY
    public string ClientIP { get; init; } = null!;
    public string ReferenceNumber { get; init; } = null!;
    public string InvoiceNumber { get; init; } = null!;
}

public record PaymentResult
{
    public bool Success { get; init; }
    public string ResponseCode { get; init; } = null!;
    public string ResponseDescription { get; init; } = null!;
    public string? ApprovalCode { get; init; }
    public string? TransactionReference { get; init; }
    public string IdempotencyKey { get; init; } = null!;
}

public record PaymentGatewayConfig
{
    public string MerchantId { get; init; } = null!;
    public string TerminalId { get; init; } = null!;
    public string BaseUrl { get; init; } = null!;
    public bool IsTest { get; init; }
}
```

---

#### Services/CardnetSettings.cs (40 líneas)

```csharp
public class CardnetSettings
{
    public string BaseUrl { get; set; } = null!;
    public string MerchantId { get; set; } = null!;
    public string TerminalId { get; set; } = null!;
    public bool IsTest { get; set; } = true;
}
```

**appsettings.json:**

```json
{
  "Cardnet": {
    "BaseUrl": "https://ecommerce.cardnet.com.do/api/payment/",
    "MerchantId": "USE_USER_SECRETS",
    "TerminalId": "USE_USER_SECRETS",
    "IsTest": true
  }
}
```

**User Secrets:**

```bash
dotnet user-secrets set "Cardnet:MerchantId" "349000001"
dotnet user-secrets set "Cardnet:TerminalId" "00000001"
```

---

#### Models/CardnetResponse.cs (100 líneas)

```csharp
public record CardnetResponse
{
    [JsonPropertyName("idempotency-key")]
    public string IdempotencyKey { get; init; } = null!;
    
    [JsonPropertyName("response-code")]
    public string ResponseCode { get; init; } = null!;
    
    [JsonPropertyName("internal-response-code")]
    public string InternalResponseCode { get; init; } = null!;
    
    [JsonPropertyName("response-code-desc")]
    public string ResponseCodeDesc { get; init; } = null!;
    
    [JsonPropertyName("response-code-source")]
    public string ResponseCodeSource { get; init; } = null!;
    
    [JsonPropertyName("approval-code")]
    public string? ApprovalCode { get; init; }
    
    [JsonPropertyName("pnRef")]
    public string? PnRef { get; init; }
}
```

---

### Presentation Layer - Controllers (1 archivo, ~500 líneas)

#### SuscripcionesController.cs (500 líneas)

**Ubicación:** `Presentation/MiGenteEnLinea.API/Controllers/SuscripcionesController.cs`

**Endpoints (10):**

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SuscripcionesController : ControllerBase
{
    // SUSCRIPCIONES (4 endpoints)
    
    [HttpPost] // POST /api/suscripciones
    public async Task<ActionResult<int>> CreateSuscripcion([FromBody] CreateSuscripcionCommand command)
    
    [HttpPut("{id}")] // PUT /api/suscripciones/123
    public async Task<IActionResult> UpdateSuscripcion(int id, [FromBody] UpdateSuscripcionCommand command)
    
    [HttpGet("usuario/{userId}")] // GET /api/suscripciones/usuario/{guid}
    public async Task<ActionResult<SuscripcionDetalleDto>> GetSuscripcionActiva(Guid userId)
    
    [HttpDelete("{id}")] // DELETE /api/suscripciones/123
    public async Task<IActionResult> CancelarSuscripcion(int id, [FromBody] CancelarSuscripcionRequest request)
    
    // PLANES (2 endpoints)
    
    [HttpGet("planes/empleadores")] // GET /api/suscripciones/planes/empleadores
    [AllowAnonymous] // ⚠️ Público
    public async Task<ActionResult<List<PlanEmpleadorDto>>> GetPlanesEmpleadores()
    
    [HttpGet("planes/contratistas")] // GET /api/suscripciones/planes/contratistas
    [AllowAnonymous]
    public async Task<ActionResult<List<PlanContratistaDto>>> GetPlanesContratistas()
    
    // VENTAS (3 endpoints)
    
    [HttpPost("ventas")] // POST /api/suscripciones/ventas
    public async Task<ActionResult<ProcesarVentaResult>> ProcesarVenta([FromBody] ProcesarVentaCommand command)
    
    [HttpGet("ventas/usuario/{userId}")] // GET /api/suscripciones/ventas/usuario/{guid}
    public async Task<ActionResult<PaginatedList<VentaDto>>> GetVentasByUserId(
        Guid userId,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 20)
    
    // UTILIDADES (1 endpoint)
    
    [HttpGet("verificar-vencimiento/{userId}")] // GET /api/suscripciones/verificar-vencimiento/{guid}
    public async Task<ActionResult<VerificarVencimientoResult>> VerificarVencimiento(Guid userId)
}
```

---

## 🔧 CONFIGURACIÓN REQUERIDA

### DependencyInjection.cs (Infrastructure)

```csharp
// HttpClient para Cardnet
services.AddHttpClient("CardnetAPI", (serviceProvider, client) =>
{
    var config = serviceProvider.GetRequiredService<IConfiguration>();
    client.BaseAddress = new Uri(config["Cardnet:BaseUrl"]!);
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddPolicyHandler(GetRetryPolicy()); // Retry 3 veces con exponential backoff

// Payment Service
services.Configure<CardnetSettings>(configuration.GetSection("Cardnet"));
services.AddScoped<IPaymentService, CardnetPaymentService>();
```

---

## ⚠️ RIESGOS Y MITIGACIONES

### Riesgo 1: Cardnet API no documentada oficialmente

**Mitigación:**

- Usar Postman/Insomnia para probar API manualmente
- Documentar todos los códigos de respuesta encontrados
- Crear mock service para testing sin consumir API real

### Riesgo 2: Tarjetas encriptadas en Legacy con Crypt.Encrypt()

**Mitigación:**

- Identificar algoritmo de encriptación (AES? TripleDES?)
- Implementar desencriptación compatible
- Considerar migración a tokenización PCI-compliant (futuro)

### Riesgo 3: Idempotency keys pueden fallar

**Mitigación:**

- Implementar retry con exponential backoff (Polly)
- Cachear idempotency keys generados (5 minutos)
- Log detallado de errores de idempotency

### Riesgo 4: Legacy usa 2 DbContext separados

**Mitigación:**

- Clean Architecture usa 1 solo DbContext con Transaction
- Usar `_context.Database.BeginTransactionAsync()` para atomicidad
- Rollback automático si Payment falla

---

## 📊 ESTIMACIÓN DE TIEMPO

| Tarea | Tiempo Estimado |
|-------|-----------------|
| 1. Análisis y documentación | 2 horas (COMPLETADO) |
| 2. Implementar Commands | 6 horas |
| 3. Implementar Queries | 3 horas |
| 4. Implementar CardnetPaymentService | 5 horas |
| 5. Crear DTOs y Validators | 2 horas |
| 6. Implementar SuscripcionesController | 3 horas |
| 7. Testing con Swagger UI | 2 horas |
| 8. Testing con Cardnet Sandbox | 3 horas |
| 9. Documentación final | 1 hora |
| **TOTAL** | **27 horas (~4 días)** |

---

## ✅ CHECKLIST DE IMPLEMENTACIÓN

### Fase 1: Setup (1 hora) ✅ COMPLETADO

- [x] Crear estructura de carpetas Features/Suscripciones/
- [x] Agregar CardnetSettings a appsettings.json
- [x] Configurar User Secrets para Cardnet credentials
- [x] Agregar IPaymentService interface
- [x] Crear CardnetSettings.cs
- [x] Actualizar DependencyInjection.cs con HttpClient + Polly
- [x] Verificar compilación exitosa (0 errores)

### Fase 2: Commands (6 horas)

- [ ] CreateSuscripcionCommand + Handler + Validator
- [ ] UpdateSuscripcionCommand + Handler + Validator
- [ ] RenovarSuscripcionCommand + Handler + Validator
- [ ] CancelarSuscripcionCommand + Handler + Validator
- [ ] ProcesarVentaCommand + Handler + Validator
- [ ] ProcesarVentaSinPagoCommand + Handler + Validator

### Fase 3: Queries (3 horas)

- [ ] GetSuscripcionActivaQuery + Handler
- [ ] GetPlanesEmpleadoresQuery + Handler
- [ ] GetPlanesContratistasQuery + Handler
- [ ] GetVentasByUserIdQuery + Handler

### Fase 4: DTOs (1 hora)

- [ ] SuscripcionDetalleDto
- [ ] PlanEmpleadorDto
- [ ] PlanContratistaDto
- [ ] VentaDto
- [ ] CreateSuscripcionResult
- [ ] ProcesarVentaResult

### Fase 5: Payment Service (5 horas)

- [ ] CardnetPaymentService.ProcessPaymentAsync()
- [ ] CardnetPaymentService.GenerateIdempotencyKeyAsync()
- [ ] CardnetPaymentService.GetConfigurationAsync()
- [ ] CardnetResponse model
- [ ] PaymentRequest/PaymentResult models
- [ ] Configurar HttpClient en DI

### Fase 6: Controller (3 horas)

- [ ] POST /api/suscripciones (CreateSuscripcion)
- [ ] PUT /api/suscripciones/{id} (UpdateSuscripcion)
- [ ] GET /api/suscripciones/usuario/{userId} (GetActiva)
- [ ] DELETE /api/suscripciones/{id} (Cancelar)
- [ ] GET /api/suscripciones/planes/empleadores
- [ ] GET /api/suscripciones/planes/contratistas
- [ ] POST /api/suscripciones/ventas (ProcesarVenta)
- [ ] GET /api/suscripciones/ventas/usuario/{userId}
- [ ] GET /api/suscripciones/verificar-vencimiento/{userId}

### Fase 7: Testing (5 horas)

- [ ] Unit tests para Commands Handlers
- [ ] Unit tests para Queries Handlers
- [ ] Mock CardnetPaymentService para tests
- [ ] Integration tests con Cardnet Sandbox
- [ ] Testing manual con Swagger UI
- [ ] Probar flujo completo de compra

### Fase 8: Documentación (1 hora)

- [ ] Documentar códigos de respuesta Cardnet
- [ ] Crear LOTE_5_SUSCRIPCIONES_PAGOS_COMPLETADO.md
- [ ] Actualizar README.md con nuevos endpoints
- [ ] Crear Postman collection

---

## 🎯 CRITERIOS DE ACEPTACIÓN

1. ✅ Todas las Commands compilan sin errores
2. ✅ Todas las Queries compilan sin errores
3. ✅ CardnetPaymentService integra correctamente con API externa
4. ✅ SuscripcionesController expone 10 endpoints REST
5. ✅ Swagger UI documenta todos los endpoints
6. ✅ Payment flow completo funciona: Checkout → Cardnet → Suscripción activa
7. ✅ Manejo correcto de errores de pago (tarjeta inválida, fondos insuficientes, etc.)
8. ✅ Idempotency keys previenen duplicados
9. ✅ Transacciones atómicas (si payment falla, no se crea suscripción)
10. ✅ Logging estructurado de todas las transacciones

---

## 📚 REFERENCIASte

**APIs Externas:**

- Cardnet API Base URL: `https://ecommerce.cardnet.com.do/api/payment/`
- Cardnet Docs: No disponible públicamente (reverse engineering desde Legacy)

**Entities Domain:**

- `Domain/Entities/Suscripciones/Suscripcion.cs`
- `Domain/Entities/Planes/PlanEmpleador.cs`
- `Domain/Entities/Planes/PlanContratista.cs`
- `Domain/Entities/Ventas/Venta.cs`

**Legacy Code:**

- `Services/SuscripcionesService.cs` (métodos 10-17)
- `Services/PaymentService.cs` (métodos 1-3)

---

## 🚀 PRÓXIMOS PASOS

1. ✅ **Revisar y aprobar este plan**
2. ⏳ **Implementar Fase 1: Setup** (1 hora)
3. ⏳ **Implementar Fase 2: Commands** (6 horas)
4. ⏳ **Implementar Fase 3: Queries** (3 horas)
5. ⏳ **Implementar Fase 4: DTOs** (1 hora)
6. ⏳ **Implementar Fase 5: Payment Service** (5 horas)
7. ⏳ **Implementar Fase 6: Controller** (3 horas)
8. ⏳ **Ejecutar Fase 7: Testing** (5 horas)
9. ⏳ **Documentar Fase 8** (1 hora)

**Fecha Estimada de Completado:** 4 días laborables (~27 horas)

---

_Documento creado: 15 de octubre, 2025_  
_Autor: GitHub Copilot + Análisis exhaustivo de Legacy Code_  
_Estado: PLAN APROBADO - LISTO PARA IMPLEMENTACIÓN_
