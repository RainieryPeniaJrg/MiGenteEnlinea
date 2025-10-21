# CHECKPOINT 4.1: ANÁLISIS Y CONTEXTO - LOTE 4 EMPLEADOS Y NÓMINA

**Fecha:** 13 de octubre, 2025  
**Fase:** SUB-LOTE 4.1 - Análisis previo a implementación  
**Estado:** ✅ COMPLETADO  
**Duración:** 45 minutos

---

## 📋 RESUMEN EJECUTIVO

Este documento contiene el análisis exhaustivo de los servicios Legacy relacionados con **Empleados y Nómina** antes de iniciar la implementación CQRS en Clean Architecture. Se han identificado **32 métodos públicos** en `EmpleadosService.cs`, más la lógica crítica de cálculo de nómina en `fichaEmpleado.aspx.cs` (método `armarNovedad()`).

### Archivos Legacy Analizados

| Archivo | Líneas | Métodos Públicos | Complejidad |
|---------|--------|------------------|-------------|
| `EmpleadosService.cs` | 680 | 32 | 🔴 ALTA |
| `fichaEmpleado.aspx.cs` | 500+ | 12 | 🔴 ALTA |
| `colaboradores.aspx.cs` | 250 | 5 WebMethods | 🟢 BAJA |
| `nomina.aspx.cs` | 150 | 4 (vacíos) | 🟢 BAJA |

**Total:** ~1,580 líneas de código Legacy analizadas

---

## 🎯 DECISIONES TÉCNICAS CRÍTICAS

### Decisión #1: Mantener Patrón 2 DbContext en `procesarPago()`

**Problema:** El método `procesarPago()` usa deliberadamente 2 instancias separadas de `migenteEntities`:

```csharp
// Legacy: EmpleadosService.cs líneas 132-151
public int procesarPago(Empleador_Recibos_Header header, List<Empleador_Recibos_Detalle> detalle)
{
    // DbContext #1: Guardar header primero
    using (var db = new migenteEntities())
    {
        db.Empleador_Recibos_Header.Add(header);
        db.SaveChanges(); // ← Genera pagoID (auto-increment)
    }

    // DbContext #2: Usar pagoID generado para detalles
    using (var db1 = new migenteEntities())
    {
        foreach (var item in detalle)
        {
            item.pagoID = header.pagoID; // ← Usar ID generado
        }
        db1.Empleador_Recibos_Detalle.AddRange(detalle);
        db1.SaveChanges();
    }
    return header.pagoID;
}
```

**Análisis:**
- ✅ **Razón válida:** Necesita `pagoID` auto-generado ANTES de insertar detalles
- ✅ **Funciona en producción:** Patrón usado desde hace años sin errores
- ⚠️ **No estándar:** Viola principio de una transacción unificada
- ❌ **Alternativa no probada:** `TransactionScope` funcionaría pero NO está validado en Legacy

**DECISIÓN FINAL:**
```
✅ MANTENER patrón exacto con 2 SaveChangesAsync() separados
✅ Documentar razón en comentarios del código
✅ Usar UnitOfWork con 2 contextos si es necesario
❌ NO usar TransactionScope hasta probarlo en Legacy primero
```

**Implementación en Clean:**
```csharp
// Handler: ProcesarPagoCommandHandler.cs
public async Task<int> Handle(ProcesarPagoCommand request, CancellationToken ct)
{
    // PASO 1: Guardar header primero (genera pagoID)
    var header = ReciboHeader.Create(/* ... */);
    await _context.RecibosHeader.AddAsync(header, ct);
    await _context.SaveChangesAsync(ct); // ← SaveChanges #1
    
    // PASO 2: Crear detalles con pagoID generado
    foreach (var concepto in conceptos)
    {
        var detalle = ReciboDetalle.Create(header.PagoId, concepto.Nombre, concepto.Monto);
        await _context.RecibosDetalle.AddAsync(detalle, ct);
    }
    await _context.SaveChangesAsync(ct); // ← SaveChanges #2
    
    return header.PagoId;
}
```

---

### Decisión #2: Extraer `armarNovedad()` a `INominaCalculatorService`

**Problema:** El método `armarNovedad()` mezcla 150+ líneas de lógica UI con cálculos complejos:

```csharp
// Legacy: fichaEmpleado.aspx.cs líneas 200-370
public void armarNovedad()
{
    // CÁLCULO 1: Dividendo según período
    int dividendo = 1;
    if (periodoPago.InnerText == "Semanal") dividendo = 4;
    if (periodoPago.InnerText == "Quincenal") dividendo = 2;
    if (periodoPago.InnerText == "Mensual") dividendo = 1;
    
    // CÁLCULO 2: Fracción de salario (días trabajados)
    bool fraccion = (cbPeriodo.Value == 2);
    decimal dividendoFraccion = 23.83m; // Días promedio por quincena
    int diasTrabajados = (fechaFin - dtfechaInicio).Days;
    
    if (fraccion)
        pn.Monto = (salario / dividendoFraccion) * diasTrabajados;
    else
        pn.Monto = salario / dividendo;
    
    // CÁLCULO 3: Deducciones TSS (SIEMPRE NEGATIVAS)
    foreach (var deduccion in deducciones)
    {
        decimal monto = (salario * (deduccion.Porcentaje / 100)) * -1; // ← CRÍTICO: * -1
        // ...
    }
}
```

**Análisis:**
- ❌ **Violación SRP:** Mezcla cálculo + manipulación UI
- ✅ **Lógica reutilizable:** Necesaria para nómina, regalía pascual, fracciones
- 🔴 **Alta complejidad:** 150+ líneas con 3 subsistemas (salario, extras, TSS)
- ⚠️ **Fórmulas críticas:** Dividendo 23.83, deducciones negativas

**DECISIÓN FINAL:**
```
✅ Crear INominaCalculatorService en Application Layer (NO Domain)
✅ Extraer TODA la lógica de cálculo al servicio
✅ Handler solo orquesta: obtiene empleado → calcula → guarda recibo
❌ NO mover a Domain (no es regla universal, es específica de RD)
```

**Interfaz propuesta:**
```csharp
// Application/Common/Interfaces/INominaCalculatorService.cs
public interface INominaCalculatorService
{
    Task<NominaCalculoResult> CalcularNominaAsync(
        int empleadoId,
        DateTime fechaPago,
        TipoPago tipo, // 1=Salario, 2=Regalía Pascual
        TipoPeriodo periodo, // 1=Completo, 2=Fracción
        bool aplicarTss,
        CancellationToken ct);
}

public class NominaCalculoResult
{
    public List<ConceptoNomina> Percepciones { get; set; } // Salario, extras
    public List<ConceptoNomina> Deducciones { get; set; }  // TSS (negativos)
    public decimal TotalPercepciones => Percepciones.Sum(x => x.Monto);
    public decimal TotalDeducciones => Deducciones.Sum(x => Math.Abs(x.Monto));
    public decimal NetoPagar => TotalPercepciones - TotalDeducciones;
}

public class ConceptoNomina
{
    public string Descripcion { get; set; }
    public decimal Monto { get; set; } // Negativo para deducciones
}
```

**Implementación:**
```csharp
// Application/Features/Empleados/Services/NominaCalculatorService.cs
public class NominaCalculatorService : INominaCalculatorService
{
    private const decimal DIVIDENDO_FRACCION = 23.83m;
    
    public async Task<NominaCalculoResult> CalcularNominaAsync(/* ... */)
    {
        // 1. Obtener empleado y configuración
        var empleado = await ObtenerEmpleadoAsync(empleadoId);
        
        // 2. Calcular dividendo según período
        int dividendo = CalcularDividendo(empleado.PeriodoPago);
        
        // 3. Calcular salario (con fracción si aplica)
        decimal montoSalario = periodo == TipoPeriodo.Fraccion
            ? CalcularSalarioFraccion(empleado, fechaPago, DIVIDENDO_FRACCION)
            : empleado.Salario / dividendo;
        
        // 4. Agregar remuneraciones extras
        var percepciones = AgregarRemuneracionesExtras(empleado, dividendo, periodo);
        
        // 5. Calcular deducciones TSS
        var deducciones = aplicarTss 
            ? await CalcularDeduccionesTssAsync(montoSalario, periodo)
            : new List<ConceptoNomina>();
        
        return new NominaCalculoResult
        {
            Percepciones = percepciones,
            Deducciones = deducciones
        };
    }
    
    private int CalcularDividendo(PeriodoPago periodo)
    {
        return periodo switch
        {
            PeriodoPago.Semanal => 4,
            PeriodoPago.Quincenal => 2,
            PeriodoPago.Mensual => 1,
            _ => 1
        };
    }
    
    private decimal CalcularSalarioFraccion(Empleado empleado, DateTime fechaPago, decimal dividendoFraccion)
    {
        var diasTrabajados = (fechaPago - empleado.FechaInicio).Days;
        return (empleado.Salario / dividendoFraccion) * diasTrabajados;
    }
    
    private async Task<List<ConceptoNomina>> CalcularDeduccionesTssAsync(decimal salario, TipoPeriodo periodo)
    {
        var deducciones = await _context.DeduccionesTss.ToListAsync();
        
        return deducciones.Select(d => new ConceptoNomina
        {
            Descripcion = periodo == TipoPeriodo.Fraccion 
                ? $"Fracción de {d.Descripcion}" 
                : d.Descripcion,
            Monto = (salario * (d.Porcentaje / 100)) * -1 // ← CRÍTICO: Negativo
        }).ToList();
    }
}
```

---

### Decisión #3: API Padrón - Servicio en Infrastructure

**Problema:** `consultarPadron()` integra con API externa del gobierno dominicano:

```csharp
// Legacy: EmpleadosService.cs líneas 527-591
public async Task<PadronModel> consultarPadron(string cedula)
{
    HttpClient client = new HttpClient();
    
    // PASO 1: Autenticación
    string loginUrl = "https://abcportal.online/Sigeinfo/public/api/login";
    var loginContent = new FormUrlEncodedContent(new[]
    {
        new KeyValuePair<string, string>("username", "131345042"),
        new KeyValuePair<string, string>("password", "1313450422022@*SRL")
    });
    
    var loginResponse = await client.PostAsync(loginUrl, loginContent);
    var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
    
    if (loginResponse.IsSuccessStatusCode)
    {
        // PASO 2: Extraer Bearer token
        var jsonResponse = JObject.Parse(loginResponseContent);
        var token = jsonResponse["token"].ToString();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        
        // PASO 3: Consultar individuo por cédula
        string searchUrl = $"https://abcportal.online/Sigeinfo/public/api/individuo/{cedula}";
        var searchResponse = await client.GetAsync(searchUrl);
        var searchResponseContent = await searchResponse.Content.ReadAsStringAsync();
        
        if (searchResponse.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<PadronModel>(searchResponseContent);
        }
    }
    
    return null;
}
```

**Análisis:**
- 🔴 **Credenciales hardcodeadas:** Username/password en código
- ⚠️ **Sin retry logic:** Falla permanente si API timeout
- ✅ **Lógica funcional:** Autenticación + consulta secuencial correcta
- 🟡 **Sin cache:** Consulta repetida para misma cédula

**DECISIÓN FINAL:**
```
✅ Crear IPadronService en Infrastructure.Services
✅ Mover credenciales a appsettings.json (User Secrets en dev)
✅ Agregar Polly para retry policy (3 intentos, backoff exponencial)
✅ Agregar IMemoryCache (5 minutos por cédula)
✅ Implementar logging estructurado (Serilog)
❌ NO cambiar lógica de autenticación (funciona en producción)
```

**Implementación:**
```csharp
// Infrastructure/Services/PadronService.cs
public class PadronService : IPadronService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PadronService> _logger;
    private readonly IMemoryCache _cache;
    private readonly PadronSettings _settings;
    
    public async Task<PadronModel?> ConsultarCedulaAsync(string cedula, CancellationToken ct)
    {
        // 1. Verificar cache
        if (_cache.TryGetValue($"padron_{cedula}", out PadronModel? cached))
        {
            _logger.LogInformation("Padrón encontrado en cache para cédula {Cedula}", cedula);
            return cached;
        }
        
        try
        {
            // 2. Autenticar y obtener token
            var token = await AutenticarAsync(ct);
            
            // 3. Consultar API con token
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);
            
            var response = await _httpClient.GetAsync(
                $"{_settings.BaseUrl}/individuo/{cedula}", ct);
            
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync(ct);
            var result = JsonSerializer.Deserialize<PadronModel>(content);
            
            // 4. Guardar en cache (5 minutos)
            _cache.Set($"padron_{cedula}", result, TimeSpan.FromMinutes(5));
            
            _logger.LogInformation("Cédula {Cedula} consultada exitosamente", cedula);
            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error consultando Padrón para cédula {Cedula}", cedula);
            return null;
        }
    }
    
    private async Task<string> AutenticarAsync(CancellationToken ct)
    {
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("username", _settings.Username),
            new KeyValuePair<string, string>("password", _settings.Password)
        });
        
        var response = await _httpClient.PostAsync(
            $"{_settings.BaseUrl}/login", content, ct);
        
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync(ct);
        var tokenResponse = JsonSerializer.Deserialize<JsonElement>(json);
        
        return tokenResponse.GetProperty("token").GetString() 
            ?? throw new InvalidOperationException("Token no recibido");
    }
}

// appsettings.json
{
  "PadronApi": {
    "BaseUrl": "https://abcportal.online/Sigeinfo/public/api",
    "Username": "131345042",
    "Password": "*** (usar User Secrets en dev, Azure KeyVault en prod)"
  }
}
```

---

## 📊 MAPEO LEGACY → CQRS (32 MÉTODOS)

### Grupo 1: CRUD Básico Empleados (8 métodos)

| # | Método Legacy | Tipo | Command/Query | Prioridad |
|---|---------------|------|---------------|-----------|
| 1 | `getEmpleados(userID)` | Read | `GetEmpleadosByEmpleadorQuery` | 🔴 CRÍTICA |
| 2 | `getVEmpleados(userID)` | Read | `GetEmpleadosByEmpleadorQuery` (misma) | 🟡 MEDIA |
| 3 | `getEmpleadosByID(userID, id)` | Read | `GetEmpleadoByIdQuery` | 🔴 CRÍTICA |
| 4 | `guardarEmpleado(empleado)` | Write | `CreateEmpleadoCommand` | 🔴 CRÍTICA |
| 5 | `actualizarEmpleado(empleado)` | Write | `UpdateEmpleadoCommand` | 🔴 CRÍTICA |
| 6 | `ActualizarEmpleado(empleado)` | Write | `UpdateEmpleadoCommand` (duplicado) | 🟢 BAJA |
| 7 | `darDeBaja(empleadoID, ...)` | Write | `DesactivarEmpleadoCommand` | 🟠 ALTA |
| 8 | `consultarPadron(cedula)` | Read | `ConsultarPadronQuery` | 🟡 MEDIA |

**Notas:**
- `getEmpleados()` y `getVEmpleados()` retornan mismo dataset (uno usa View SQL)
- `actualizarEmpleado()` y `ActualizarEmpleado()` son duplicados (consolidar en uno)
- `darDeBaja()` es soft delete: `Activo=false`, guarda `fechaSalida`, `motivoBaja`, `prestaciones`

---

### Grupo 2: Remuneraciones Extras (4 métodos)

| # | Método Legacy | Tipo | Command/Query | Prioridad |
|---|---------------|------|---------------|-----------|
| 9 | `obtenerRemuneraciones(userID, empleadoID)` | Read | `GetRemuneracionesByEmpleadoQuery` | 🟡 MEDIA |
| 10 | `quitarRemuneracion(userID, id)` | Write | `RemoveRemuneracionCommand` | 🟡 MEDIA |
| 11 | `guardarOtrasRemuneraciones(rem)` | Write | `AddRemuneracionCommand` | 🟡 MEDIA |
| 12 | `actualizarRemuneraciones(rem, empleadoID)` | Write | `UpdateRemuneracionesCommand` | 🟢 BAJA |

**Notas:**
- Empleado tiene 3 slots para remuneraciones extras: `Extra1`, `Extra2`, `Extra3`
- `actualizarRemuneraciones()` borra todas y re-inserta (replace strategy)
- Domain: `Empleado.AgregarRemuneracionExtra(numero, descripcion, monto)` ya existe

---

### Grupo 3: Procesamiento de Nómina (6 métodos)

| # | Método Legacy | Tipo | Command/Query | Prioridad |
|---|---------------|------|---------------|-----------|
| 13 | `procesarPago(header, detalle)` | Write | `ProcesarPagoCommand` | 🔴 CRÍTICA |
| 14 | `GetEmpleador_Recibos_Empleado(userID, empID)` | Read | `GetRecibosByEmpleadoQuery` | 🟠 ALTA |
| 15 | `GetEmpleador_ReciboByPagoID(pagoID)` | Read | `GetReciboByIdQuery` | 🟠 ALTA |
| 16 | `eliminarReciboEmpleado(pagoID)` | Write | `AnularReciboCommand` | 🟡 MEDIA |
| 17 | `armarNovedad()` (fichaEmpleado.aspx.cs) | Logic | `INominaCalculatorService` | 🔴 CRÍTICA |
| 18 | `deducciones()` | Read | Query inline (simple) | 🟢 BAJA |

**Notas:**
- `procesarPago()` usa patrón 2 DbContext (mantener)
- `eliminarReciboEmpleado()` es hard delete, cambiar a soft delete: `Estado=3` (Anulado)
- `armarNovedad()` extraer a `INominaCalculatorService` (ver Decisión #2)
- `deducciones()` retorna tabla `Deducciones_TSS` completa (query simple)

**Fórmulas Críticas en `armarNovedad()`:**
```csharp
// 1. Dividendo según período de pago
int dividendo = periodoPago switch
{
    "Semanal" => 4,    // 4 semanas por mes
    "Quincenal" => 2,  // 2 quincenas por mes
    "Mensual" => 1,    // 1 mes
    _ => 1
};

// 2. Salario fracción (días trabajados)
decimal dividendoFraccion = 23.83m; // Días promedio por quincena
int diasTrabajados = (fechaPago - fechaInicio).Days;
decimal montoFraccion = (salario / dividendoFraccion) * diasTrabajados;

// 3. Deducciones TSS (SIEMPRE NEGATIVAS)
decimal montoDeduccion = (salario * (porcentaje / 100)) * -1; // ← CRÍTICO: * -1

// 4. Remuneraciones extras usan MISMO dividendo que salario
decimal montoExtra = remuneracion.Monto / dividendo;
```

---

### Grupo 4: Empleados Temporales (12 métodos)

| # | Método Legacy | Tipo | Command/Query | Prioridad |
|---|---------------|------|---------------|-----------|
| 19 | `getContrataciones(userID)` | Read | `GetEmpleadosTemporalesByEmpleadorQuery` | 🟡 MEDIA |
| 20 | `obtenerFichaTemporales(contratacionID, userID)` | Read | `GetEmpleadoTemporalByIdQuery` | 🟡 MEDIA |
| 21 | `obtenerTodosLosTemporales(userID)` | Read | `GetEmpleadosTemporalesByEmpleadorQuery` | 🟡 MEDIA |
| 22 | `obtenerVistaTemporal(contratacionID, userID)` | Read | `GetEmpleadoTemporalByIdQuery` | 🟢 BAJA |
| 23 | `nuevoTemporal(temp, det)` | Write | `CreateEmpleadoTemporalCommand` | 🟡 MEDIA |
| 24 | `nuevaContratacionTemporal(det)` | Write | `AddContratacionCommand` | 🟢 BAJA |
| 25 | `actualizarContratacion(det)` | Write | `UpdateContratacionCommand` | 🟢 BAJA |
| 26 | `eliminarEmpleadoTemporal(contratacionID)` | Write | `DeleteEmpleadoTemporalCommand` | 🟡 MEDIA |
| 27 | `procesarPagoContratacion(header, detalle)` | Write | `ProcesarPagoContratacionCommand` | 🟠 ALTA |
| 28 | `GetContratacion_ReciboByPagoID(pagoID)` | Read | `GetReciboContratacionByIdQuery` | 🟢 BAJA |
| 29 | `GetEmpleador_RecibosContratacionesByID(...)` | Read | `GetRecibosContratacionQuery` | 🟢 BAJA |
| 30 | `eliminarReciboContratacion(pagoID)` | Write | `AnularReciboContratacionCommand` | 🟢 BAJA |

**Notas:**
- Temporales tienen `tipo`: 1=Persona Física, 2=Persona Jurídica (empresa)
- `nuevoTemporal()` usa 2 DbContext separados (similar a `procesarPago()`)
- `eliminarEmpleadoTemporal()` es hard delete en cascada (temporal + contrataciones + recibos)
- `procesarPagoContratacion()` actualiza `estatus=2` si concepto es "Pago Final"

---

### Grupo 5: Calificaciones (2 métodos)

| # | Método Legacy | Tipo | Command/Query | Prioridad |
|---|---------------|------|---------------|-----------|
| 31 | `calificarContratacion(contratacionID, calificacionID)` | Write | `CalificarContratacionCommand` | 🟢 BAJA |
| 32 | `modificarCalificacionDeContratacion(cal)` | Write | `UpdateCalificacionCommand` | 🟢 BAJA |

**Notas:**
- Calificaciones están en módulo separado (LOTE 6)
- Estos métodos solo actualizan foreign key en `DetalleContrataciones`

---

## 🔍 ANÁLISIS DE PATRONES CRÍTICOS

### Patrón #1: Soft Delete con Metadata

```csharp
// Legacy: EmpleadosService.cs líneas 496-512
public bool darDeBaja(int empleadoID, string userID, DateTime fechaBaja, 
                      decimal prestaciones, string motivo)
{
    using (var db = new migenteEntities())
    {
        Empleados empleado = db.Empleados
            .Where(x => x.empleadoID == empleadoID && x.userID == userID)
            .FirstOrDefault();
        
        if (empleado != null)
        {
            empleado.Activo = false;              // ← Soft delete
            empleado.fechaSalida = fechaBaja.Date; // ← Metadata
            empleado.motivoBaja = motivo;          // ← Metadata
            empleado.prestaciones = prestaciones;  // ← Metadata
            db.SaveChanges();
        }
        return true;
    }
}
```

**Implementación Clean:**
```csharp
// Command: DesactivarEmpleadoCommand.cs
public record DesactivarEmpleadoCommand(
    int EmpleadoId,
    DateTime FechaBaja,
    decimal Prestaciones,
    string MotivoBaja) : IRequest<bool>;

// Handler
public async Task<bool> Handle(DesactivarEmpleadoCommand request, CancellationToken ct)
{
    var empleado = await _context.Empleados
        .Where(e => e.EmpleadoId == request.EmpleadoId)
        .FirstOrDefaultAsync(ct)
        ?? throw new NotFoundException(nameof(Empleado), request.EmpleadoId);
    
    // Domain method
    empleado.Desactivar(request.FechaBaja, request.MotivoBaja, request.Prestaciones);
    
    await _context.SaveChangesAsync(ct);
    return true;
}

// Domain: Empleado.cs (YA EXISTE)
public void Desactivar(DateTime fechaBaja, string motivo, decimal prestaciones)
{
    if (!Activo)
        throw new DomainException("Empleado ya está inactivo");
    
    Activo = false;
    FechaSalida = fechaBaja;
    MotivoBaja = motivo;
    Prestaciones = prestaciones;
    
    AddDomainEvent(new EmpleadoDesactivadoEvent(EmpleadoId, fechaBaja, prestaciones));
}
```

---

### Patrón #2: Replace Strategy en Remuneraciones

```csharp
// Legacy: EmpleadosService.cs líneas 607-626
public bool actualizarRemuneraciones(List<Remuneraciones> rem, int empleadoID)
{
    // PASO 1: Borrar todas las existentes
    using (migenteEntities db = new migenteEntities())
    {
        var result = db.Remuneraciones.Where(x => x.empleadoID == empleadoID).FirstOrDefault();
        if (result != null)
        {
            db.Remuneraciones.Remove(result);
            db.SaveChanges();
        }
    }
    
    // PASO 2: Insertar nuevas
    using (migenteEntities db1 = new migenteEntities())
    {
        db1.Remuneraciones.AddRange(rem);
        db1.SaveChanges();
        return true;
    }
}
```

**Problema:** 2 transacciones separadas (riesgo de inconsistencia)

**Implementación Clean:**
```csharp
// Command: UpdateRemuneracionesCommand.cs
public record UpdateRemuneracionesCommand(
    int EmpleadoId,
    List<RemuneracionDto> Remuneraciones) : IRequest<bool>;

// Handler (transacción unificada)
public async Task<bool> Handle(UpdateRemuneracionesCommand request, CancellationToken ct)
{
    var empleado = await _context.Empleados
        .Include(e => e.Remuneraciones) // ← Incluir colección
        .FirstOrDefaultAsync(e => e.EmpleadoId == request.EmpleadoId, ct)
        ?? throw new NotFoundException(nameof(Empleado), request.EmpleadoId);
    
    // Domain method: reemplazar todas
    empleado.ActualizarRemuneraciones(request.Remuneraciones);
    
    await _context.SaveChangesAsync(ct); // ← Única transacción
    return true;
}

// Domain: Empleado.cs (NUEVO)
public void ActualizarRemuneraciones(List<RemuneracionDto> nuevas)
{
    // Limpiar existentes
    _remuneraciones.Clear();
    
    // Agregar nuevas
    foreach (var rem in nuevas)
    {
        AgregarRemuneracionExtra(rem.Numero, rem.Descripcion, rem.Monto);
    }
    
    AddDomainEvent(new RemuneracionesActualizadasEvent(EmpleadoId));
}
```

---

### Patrón #3: Hard Delete en Cascada

```csharp
// Legacy: EmpleadosService.cs líneas 375-440
public bool eliminarEmpleadoTemporal(int contratacionID)
{
    // PASO 1: Obtener empleado temporal
    EmpleadosTemporales tmp;
    using (var dbTmp = new migenteEntities())
    {
        tmp = dbTmp.EmpleadosTemporales.Where(a => a.contratacionID == contratacionID).FirstOrDefault();
    }
    
    if (tmp != null)
    {
        // PASO 2: Borrar recibos (detalle + header) en cascada
        foreach (var recibo in tmp.Empleador_Recibos_Header_Contrataciones)
        {
            // Borrar detalles
            using (var db = new migenteEntities())
            {
                var detallesAEliminar = db.Empleador_Recibos_Detalle_Contrataciones
                    .Where(d => d.pagoID == recibo.pagoID);
                db.Empleador_Recibos_Detalle_Contrataciones.RemoveRange(detallesAEliminar);
                db.SaveChanges();
            }
            
            // Borrar header
            using (var db1 = new migenteEntities())
            {
                var headerAEliminar = db1.Empleador_Recibos_Header_Contrataciones
                    .FirstOrDefault(h => h.pagoID == recibo.pagoID);
                if (headerAEliminar != null)
                {
                    db1.Empleador_Recibos_Header_Contrataciones.Remove(headerAEliminar);
                    db1.SaveChanges();
                }
            }
        }
        
        // PASO 3: Borrar empleado temporal
        using (var dbEmp = new migenteEntities())
        {
            var registroAEliminar = dbEmp.EmpleadosTemporales
                .FirstOrDefault(h => h.contratacionID == contratacionID);
            if (registroAEliminar != null)
            {
                dbEmp.EmpleadosTemporales.Remove(registroAEliminar);
                dbEmp.SaveChanges();
            }
        }
    }
    
    return true;
}
```

**Problema:** 
- ❌ Múltiples DbContext (N+1 transacciones)
- ❌ Sin manejo de errores (puede quedar inconsistente)
- ⚠️ Hard delete (no recuperable)

**DECISIÓN:**
```
✅ Cambiar a soft delete con IsDeleted=true
✅ Una sola transacción con EF Core cascade
✅ Agregar DeletedAt timestamp
❌ NO mantener hard delete (pérdida de data)
```

**Implementación Clean:**
```csharp
// Command: DeleteEmpleadoTemporalCommand.cs
public record DeleteEmpleadoTemporalCommand(int ContratacionId) : IRequest<bool>;

// Handler
public async Task<bool> Handle(DeleteEmpleadoTemporalCommand request, CancellationToken ct)
{
    var temporal = await _context.EmpleadosTemporales
        .Include(t => t.Contrataciones)
        .Include(t => t.Recibos) // ← EF Core navega relaciones
        .FirstOrDefaultAsync(t => t.ContratacionId == request.ContratacionId, ct)
        ?? throw new NotFoundException(nameof(EmpleadoTemporal), request.ContratacionId);
    
    // Soft delete
    temporal.MarcarComoEliminado();
    
    await _context.SaveChangesAsync(ct); // ← Única transacción
    return true;
}

// Domain: EmpleadoTemporal.cs (ACTUALIZAR)
public void MarcarComoEliminado()
{
    IsDeleted = true;
    DeletedAt = DateTime.UtcNow;
    
    AddDomainEvent(new EmpleadoTemporalEliminadoEvent(ContratacionId));
}
```

---

## 📦 RESUMEN DE ENTIDADES DOMAIN DISPONIBLES

### ✅ Entidades YA Migradas (Fase 1 - Domain Layer)

| Entidad | Archivo | Métodos Clave | Estado |
|---------|---------|---------------|--------|
| `Empleado` | Domain/Entities/Empleados/Empleado.cs | 15+ métodos | ✅ COMPLETO |
| `EmpleadoTemporal` | Domain/Entities/Empleados/EmpleadoTemporal.cs | Create, Actualizar | ✅ COMPLETO |
| `ReciboHeader` | Domain/Entities/Empleados/ReciboHeader.cs | AgregarIngreso, Recalcular | ✅ COMPLETO |
| `ReciboDetalle` | Domain/Entities/Empleados/ReciboDetalle.cs | Create | ✅ COMPLETO |
| `DeduccionTss` | Domain/Entities/Empleados/DeduccionTss.cs | CalcularMonto | ✅ COMPLETO |
| `Remuneracion` | (Value Object en Empleado) | 3 slots: Extra1, Extra2, Extra3 | ✅ COMPLETO |

### Métodos Domain en `Empleado.cs` (Disponibles para usar)

```csharp
// Factory
public static Empleado Create(string userId, string identificacion, ...)

// Actualizaciones
public void ActualizarInformacionPersonal(string nombre, string apellido, ...)
public void ActualizarInformacionContacto(string telefono1, string telefono2, ...)
public void ActualizarInformacionLaboral(string posicion, int periodoPago)
public void ActualizarPosicion(string nuevaPosicion, decimal nuevoSalario)
public void ActualizarDireccion(string direccion, string provincia, string municipio)

// Remuneraciones
public void AgregarRemuneracionExtra(int numero, string descripcion, decimal monto)
public void EliminarRemuneracionExtra(int numero)

// Ciclo de vida
public void Activar()
public void Desactivar(DateTime fechaBaja, string motivo, decimal prestaciones)

// Cálculos
public decimal CalcularSalarioMensual()
public int CalcularAntiguedad()
public bool RequiereActualizacionFoto()
```

### Métodos Domain en `ReciboHeader.cs` (Disponibles)

```csharp
// Factory
public static ReciboHeader Create(string userId, int empleadoId, DateTime fechaPago, ...)

// Gestión de líneas
public void AgregarIngreso(string concepto, decimal monto)
public void AgregarDeduccion(string concepto, decimal monto)
public void EliminarLinea(int detalleId)

// Cálculos
public void RecalcularTotales()

// Estado
public void MarcarComoPagado()
public void Anular(string motivo)
```

---

## 🎯 PLAN DE IMPLEMENTACIÓN DETALLADO

### SUB-LOTE 4.2: CRUD Básico Empleados (2-3 horas)

**Archivos a crear: 12 archivos (~900 líneas)**

#### Commands (3)

1. **CreateEmpleadoCommand**
   - Handler: `CreateEmpleadoCommandHandler.cs`
   - Validator: `CreateEmpleadoCommandValidator.cs`
   - DTO: `CreateEmpleadoDto.cs`
   - Legacy: `guardarEmpleado(empleado)`
   - Domain: `Empleado.Create(...)`

2. **UpdateEmpleadoCommand**
   - Handler: `UpdateEmpleadoCommandHandler.cs`
   - Validator: `UpdateEmpleadoCommandValidator.cs`
   - DTO: `UpdateEmpleadoDto.cs`
   - Legacy: `actualizarEmpleado(empleado)` + `ActualizarEmpleado(empleado)`
   - Domain: `empleado.ActualizarInformacionPersonal(...)`, etc.

3. **DesactivarEmpleadoCommand**
   - Handler: `DesactivarEmpleadoCommandHandler.cs`
   - Validator: `DesactivarEmpleadoCommandValidator.cs`
   - DTO: `DesactivarEmpleadoDto.cs`
   - Legacy: `darDeBaja(empleadoID, ...)`
   - Domain: `empleado.Desactivar(...)`

#### Queries (2)

4. **GetEmpleadoByIdQuery**
   - Handler: `GetEmpleadoByIdQueryHandler.cs`
   - DTO: `EmpleadoDetalleDto.cs`
   - Legacy: `getEmpleadosByID(userID, id)`

5. **GetEmpleadosByEmpleadorQuery**
   - Handler: `GetEmpleadosByEmpleadorQueryHandler.cs`
   - DTO: `EmpleadoListDto.cs`
   - Legacy: `getEmpleados(userID)` + `getVEmpleados(userID)`
   - Paginación: PageIndex, PageSize
   - Filtros: searchTerm, soloActivos

---

### SUB-LOTE 4.3: Remuneraciones Extras (1-2 horas)

**Archivos a crear: 9 archivos (~600 líneas)**

#### Commands (2)

1. **AddRemuneracionCommand**
   - Legacy: `guardarOtrasRemuneraciones(rem)`
   - Domain: `empleado.AgregarRemuneracionExtra(numero, descripcion, monto)`

2. **RemoveRemuneracionCommand**
   - Legacy: `quitarRemuneracion(userID, id)`
   - Domain: `empleado.EliminarRemuneracionExtra(numero)`

#### Queries (1)

3. **GetRemuneracionesByEmpleadoQuery**
   - Legacy: `obtenerRemuneraciones(userID, empleadoID)`
   - DTO: `RemuneracionDto.cs`

---

### SUB-LOTE 4.4: Procesamiento de Nómina (3-4 horas) ⚠️ ALTA COMPLEJIDAD

**Archivos a crear: 15 archivos (~1,200 líneas)**

#### Commands (2)

1. **ProcesarPagoCommand**
   - Handler: Usa `INominaCalculatorService`
   - Legacy: `procesarPago(header, detalle)` + `armarNovedad()`
   - **CRÍTICO:** Mantener 2 SaveChangesAsync() separados
   - Domain: `ReciboHeader.Create(...)`, `ReciboHeader.AgregarIngreso(...)`, `ReciboHeader.AgregarDeduccion(...)`

2. **AnularReciboCommand**
   - Handler: Soft delete (Estado = 3)
   - Legacy: `eliminarReciboEmpleado(pagoID)` (cambiar hard → soft)
   - Domain: `recibo.Anular(motivo)`

#### Queries (2)

3. **GetReciboByIdQuery**
   - Legacy: `GetEmpleador_ReciboByPagoID(pagoID)`
   - DTO: `ReciboDetalleDto.cs` (incluye líneas)

4. **GetRecibosByEmpleadoQuery**
   - Legacy: `GetEmpleador_Recibos_Empleado(userID, empleadoID)`
   - DTO: `ReciboListDto.cs`
   - Paginación: PageIndex, PageSize

#### Service (1)

5. **INominaCalculatorService**
   - Interface: `Application/Common/Interfaces/INominaCalculatorService.cs`
   - Implementation: `Application/Features/Empleados/Services/NominaCalculatorService.cs`
   - Métodos:
     - `CalcularNominaAsync(...)` → `NominaCalculoResult`
     - `CalcularDividendo(periodoPago)` → int
     - `CalcularSalarioFraccion(...)` → decimal
     - `CalcularDeduccionesTssAsync(...)` → List<ConceptoNomina>

---

### SUB-LOTE 4.5: Empleados Temporales (2-3 horas)

**Archivos a crear: 12 archivos (~800 líneas)**

#### Commands (3)

1. **CreateEmpleadoTemporalCommand**
   - Legacy: `nuevoTemporal(temp, det)`
   - Domain: `EmpleadoTemporal.Create(...)`
   - Tipo: 1=Persona Física, 2=Persona Jurídica

2. **UpdateEmpleadoTemporalCommand**
   - Legacy: Lógica dispersa en varios métodos
   - Domain: `temporal.Actualizar(...)`

3. **DeleteEmpleadoTemporalCommand**
   - Legacy: `eliminarEmpleadoTemporal(contratacionID)`
   - **CAMBIO:** Hard delete → Soft delete (IsDeleted=true)
   - Domain: `temporal.MarcarComoEliminado()`

#### Queries (2)

4. **GetEmpleadoTemporalByIdQuery**
   - Legacy: `obtenerFichaTemporales(contratacionID, userID)`
   - DTO: `EmpleadoTemporalDetalleDto.cs`

5. **GetEmpleadosTemporalesByEmpleadorQuery**
   - Legacy: `getContrataciones(userID)` + `obtenerTodosLosTemporales(userID)`
   - DTO: `EmpleadoTemporalListDto.cs`
   - Filtros: tipo, estatus, searchTerm
   - Paginación: PageIndex, PageSize

---

### SUB-LOTE 4.6: API Padrón + Controller (1-2 horas)

**Archivos a crear: 8 archivos (~600 líneas)**

#### Query (1)

1. **ConsultarPadronQuery**
   - Handler: Llama `IPadronService`
   - Legacy: `consultarPadron(cedula)`
   - Validation: Cédula dominicana (11 dígitos)
   - DTO: `PadronResultDto.cs`

#### Service (1)

2. **IPadronService**
   - Interface: `Application/Common/Interfaces/IPadronService.cs`
   - Implementation: `Infrastructure/Services/PadronService.cs`
   - Métodos:
     - `ConsultarCedulaAsync(cedula)` → `PadronModel?`
     - `AutenticarAsync()` → string (token)
   - Features:
     - IMemoryCache (5 minutos)
     - Polly retry policy (3 intentos)
     - Serilog logging

#### Controller (1)

3. **EmpleadosController**
   - Endpoints: 15+ REST API endpoints
   - Grupos:
     - CRUD Empleados Permanentes (5 endpoints)
     - Remuneraciones Extras (3 endpoints)
     - Nómina (4 endpoints)
     - Empleados Temporales (5 endpoints)
     - Utilidades (1 endpoint: Padrón)
   - Swagger: XML documentation completa
   - Rate Limiting: Por endpoint sensible

---

## 📝 TABLA RESUMEN: DECISIONES VS LEGACY

| Aspecto | Legacy | Clean Architecture | Razón |
|---------|--------|-------------------|-------|
| `procesarPago()` | 2 DbContext separados | 2 SaveChangesAsync() | Funciona en prod, pagoID necesario |
| `armarNovedad()` | Lógica en UI (150+ líneas) | INominaCalculatorService | Separation of Concerns |
| Hard delete temporal | Sí (cascada manual) | No (soft delete IsDeleted) | Auditoría y recuperación |
| Hard delete recibo | Sí | No (Estado=3 Anulado) | Trazabilidad |
| `actualizarRemuneraciones()` | 2 DbContext (delete+insert) | 1 transacción (clear+add) | Consistencia |
| Credenciales Padrón | Hardcoded en código | appsettings.json + User Secrets | Seguridad |
| Sin cache Padrón | Sí (consulta repetida) | IMemoryCache (5 min) | Performance |
| Sin retry logic | Sí (falla permanente) | Polly (3 intentos) | Resiliencia |
| Duplicados métodos | `actualizarEmpleado()` x2 | Consolidado en 1 Command | DRY |
| Queries View SQL | `getVEmpleados()` usa View | EF Core query optimizado | Mantenibilidad |

---

## 🚨 VALIDACIONES CRÍTICAS ANTES DE CONTINUAR

### Checklist Obligatorio

- [x] ✅ 32 métodos Legacy documentados con análisis
- [x] ✅ Mapeo Legacy → CQRS completo (32 métodos → 18 Commands/Queries)
- [x] ✅ 3 decisiones técnicas críticas documentadas con código ejemplo
- [x] ✅ 3 patrones Legacy identificados (2 DbContext, Replace Strategy, Hard Delete)
- [x] ✅ Fórmulas de nómina documentadas (dividendo, fracción, TSS)
- [x] ✅ Entidades Domain verificadas como disponibles
- [x] ✅ Plan de implementación detallado (6 sub-lotes)
- [x] ✅ Tabla comparativa Legacy vs Clean

### Métricas de Análisis

- **Tiempo invertido:** 45 minutos
- **Archivos Legacy leídos:** 4 archivos (~1,580 líneas)
- **Métodos analizados:** 32 métodos públicos
- **Patrones identificados:** 3 patrones críticos
- **Decisiones documentadas:** 3 decisiones técnicas mayores
- **Commands/Queries planificados:** 18 (11 Commands + 7 Queries)
- **Services planificados:** 2 (INominaCalculatorService, IPadronService)
- **Endpoints REST:** 15+ en EmpleadosController

---

## 📊 CONTEXTO PARA SUB-LOTE 4.2 (SIGUIENTE PASO)

### Estado Actual del Proyecto

**Compilación:**
```bash
dotnet build MiGenteEnLinea.Clean.sln
# ✅ Build succeeded.
# ✅ 0 Error(s)
# ✅ 0 Warning(s)
```

**Entidades Domain Disponibles:**
- ✅ `Empleado` (15+ métodos)
- ✅ `EmpleadoTemporal` (3 métodos)
- ✅ `ReciboHeader` (7 métodos)
- ✅ `ReciboDetalle` (factory method)
- ✅ `DeduccionTss` (1 método de cálculo)

**Infrastructure Configurado:**
- ✅ DbContext con 36 entidades
- ✅ Fluent API configurations completas
- ✅ Audit interceptor para CreatedAt/UpdatedAt
- ✅ Serilog configurado
- ✅ Connection string apuntando a db_a9f8ff_migente

### Objetivo SUB-LOTE 4.2

Implementar **CRUD Básico de Empleados Permanentes**:

1. ✅ CreateEmpleadoCommand (3 archivos)
2. ✅ UpdateEmpleadoCommand (3 archivos)
3. ✅ DesactivarEmpleadoCommand (3 archivos)
4. ✅ GetEmpleadoByIdQuery (2 archivos)
5. ✅ GetEmpleadosByEmpleadorQuery (2 archivos)

**Total:** 12 archivos, ~900 líneas, 2-3 horas

### Información Crítica a Recordar

**Códigos de Retorno Legacy (preservar):**
- `guardarEmpleado()` retorna `Empleados` (objeto completo)
- `actualizarEmpleado()` retorna `Empleados` (objeto completo)
- `darDeBaja()` retorna `bool` (true=success, false=not found)

**Validaciones Importantes:**
- Identificación única por empleador: `WHERE userID = X AND identificacion = Y`
- Empleado activo: `Activo = true` (soft delete)
- Fechas: `fechaInicio <= DateTime.Now`
- Salario: `> 0`
- Período de pago: 1=Semanal, 2=Quincenal, 3=Mensual

**Campos Calculados en DTO:**
```csharp
public class EmpleadoDetalleDto
{
    // ... campos básicos
    public decimal SalarioMensual { get; set; } // ← Calculado por domain
    public int Antiguedad { get; set; }         // ← Calculado: (Now - FechaInicio).Years
    public bool RequiereActualizacionFoto { get; set; } // ← > 1 año
}
```

---

## ✅ CONCLUSIÓN SUB-LOTE 4.1

### Resultados Obtenidos

✅ **Análisis Completo:** 32 métodos Legacy documentados con detalle  
✅ **Decisiones Tomadas:** 3 decisiones técnicas críticas (2 DbContext, INominaCalculatorService, IPadronService)  
✅ **Patrones Identificados:** 3 patrones Legacy críticos con código ejemplo  
✅ **Mapeo CQRS:** Legacy → Clean Architecture (18 Commands/Queries)  
✅ **Plan Detallado:** 6 sub-lotes con archivos, líneas y tiempos estimados  
✅ **Fórmulas Críticas:** Nómina documentada (dividendo, fracción, TSS)  
✅ **Contexto Preparado:** Información lista para SUB-LOTE 4.2

### Próximo Paso

**EJECUTAR SUB-LOTE 4.2: CRUD Básico Empleados**

**Pre-requisitos:**
1. ✅ Leer este CHECKPOINT_4.1_ANALISIS.md COMPLETAMENTE
2. ✅ Verificar compilación exitosa: `dotnet build`
3. ✅ Confirmar DbContext disponible: `IApplicationDbContext`

**Comando para iniciar:**
```markdown
Leer: CHECKPOINT_4.1_ANALISIS.md
Ir a: prompts/LOTE_4_EMPLEADOS_NOMINA_PROMPTS_SECUENCIALES.md
Sección: "SUB-LOTE 4.2: CRUD BÁSICO DE EMPLEADOS"
Crear: 12 archivos según especificación
Validar: dotnet build (0 errores)
Documentar: CHECKPOINT_4.2_CRUD_EMPLEADOS.md
```

**Tiempo estimado:** 2-3 horas  
**Output esperado:** 12 archivos (~900 líneas), compilación exitosa

---

**Documento generado por:** GitHub Copilot  
**Fecha:** 13 de octubre, 2025  
**Versión:** 1.0  
**Estado:** ✅ ANÁLISIS COMPLETADO - LISTO PARA SUB-LOTE 4.2
