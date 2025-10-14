# 🚀 LOTE 4: EMPLEADOS Y NÓMINA - PROMPTS SECUENCIALES

**Fecha de Creación:** 13 de octubre, 2025  
**Proyecto:** MiGente En Línea - Clean Architecture Migration  
**Módulo:** Empleados y Nómina (Alta Complejidad)  
**Estrategia:** Dividir en 6 prompts secuenciales ejecutables  
**Tiempo Total Estimado:** 12-15 horas

---

## 📊 ESTADO ACTUAL DEL PROYECTO

### ✅ Completado (100%)

- **Domain Layer:** 36 entidades migradas (incluye Empleado, ReciboHeader, ReciboDetalle, DeduccionTss)
- **Infrastructure Layer:** Relaciones validadas + DbContext configurado
- **LOTE 1:** Authentication (85% → pendiente completar)
- **LOTE 2:** Empleadores CRUD (100% - 20 archivos)
- **LOTE 3:** Contratistas CRUD + Servicios (100% - 30 archivos)

### 🔄 En Ejecución

- **LOTE 4:** Empleados y Nómina (0% → dividido en 6 sub-lotes)

---

## 🎯 OBJETIVO DEL LOTE 4

Migrar la gestión completa de empleados y procesamiento de nómina desde Legacy (ASP.NET Web Forms) a Clean Architecture usando CQRS con MediatR.

### Módulos a Implementar

1. **CRUD de Empleados** - Crear, leer, actualizar, desactivar empleados
2. **Remuneraciones Extras** - Bonos, comisiones adicionales al salario
3. **Procesamiento de Nómina** - Generar recibos de pago con deducciones TSS
4. **Consulta de Recibos** - Historial de pagos por empleado
5. **Empleados Temporales** - Contrataciones temporales (persona física/jurídica)
6. **Integración API Padrón** - Validación de cédulas con API gubernamental

### Complejidad

- **🔴 ALTA:** 32 métodos en EmpleadosService.cs
- **Cálculos complejos:** Deducciones TSS, nómina proporcional, prestaciones laborales
- **2 tablas principales:** Empleados, EmpleadosTemporales
- **Tablas relacionadas:** Remuneraciones, ReciboHeader, ReciboDetalle, DeduccionTss

---

## 📋 DIVISIÓN EN 6 SUB-LOTES (PROMPTS SECUENCIALES)

### 📦 SUB-LOTE 4.1: ANÁLISIS Y CONTEXTO (30-45 mins)

**Objetivo:** Leer Legacy completo, actualizar contexto  
**Archivos a analizar:** 5 archivos  
**Sin código:** Solo documentación  

### 📦 SUB-LOTE 4.2: CRUD BÁSICO DE EMPLEADOS (2-3 horas)

**Objetivo:** Crear, leer, actualizar, desactivar empleados  
**Commands:** 3 (Create, Update, Desactivar)  
**Queries:** 2 (GetById, GetByEmpleador)  
**Archivos:** ~12 archivos

### 📦 SUB-LOTE 4.3: REMUNERACIONES EXTRAS (1-2 horas)

**Objetivo:** Gestión de bonos y comisiones  
**Commands:** 2 (AddRemuneracion, RemoveRemuneracion)  
**Queries:** 1 (GetRemuneracionesByEmpleado)  
**Archivos:** ~9 archivos

### 📦 SUB-LOTE 4.4: PROCESAMIENTO DE NÓMINA (3-4 horas)

**Objetivo:** Generar recibos de pago con cálculos TSS  
**Commands:** 2 (ProcesarPago, AnularRecibo)  
**Queries:** 2 (GetReciboById, GetRecibosByEmpleado)  
**Services:** INominaCalculatorService (lógica compleja)  
**Archivos:** ~15 archivos

### 📦 SUB-LOTE 4.5: EMPLEADOS TEMPORALES (2-3 horas)

**Objetivo:** Contrataciones temporales (persona física/jurídica)  
**Commands:** 3 (CreateTemporal, UpdateTemporal, DeleteTemporal)  
**Queries:** 2 (GetTemporalById, GetTemporalesByEmpleador)  
**Archivos:** ~12 archivos

### 📦 SUB-LOTE 4.6: INTEGRACIÓN API PADRÓN + CONTROLLER (1-2 horas)

**Objetivo:** Validación de cédulas + REST API completo  
**Queries:** 1 (ConsultarPadronQuery)  
**Controller:** EmpleadosController (15+ endpoints)  
**Archivos:** ~8 archivos

---

## 🔥 REGLAS CRÍTICAS PARA TODOS LOS PROMPTS

### ⚠️ ANTES DE EJECUTAR CADA PROMPT

```markdown
✅ OBLIGATORIO: Leer archivo de contexto del prompt anterior
✅ OBLIGATORIO: Actualizar archivo de contexto al finalizar
✅ OBLIGATORIO: Leer método Legacy ANTES de implementar Handler
✅ OBLIGATORIO: Compilar con dotnet build antes de pasar al siguiente
✅ OBLIGATORIO: Documentar archivos creados en CHECKPOINT_4.X.md
```

### 📝 Formato de Archivos de Contexto

Cada sub-lote genera un archivo:

- `CHECKPOINT_4.1_ANALISIS.md` - Análisis Legacy completo
- `CHECKPOINT_4.2_CRUD_EMPLEADOS.md` - CRUD básico completado
- `CHECKPOINT_4.3_REMUNERACIONES.md` - Remuneraciones completado
- `CHECKPOINT_4.4_NOMINA.md` - Nómina completado
- `CHECKPOINT_4.5_TEMPORALES.md` - Temporales completado
- `CHECKPOINT_4.6_CONTROLLER.md` - Controller completado

---

# 📦 SUB-LOTE 4.1: ANÁLISIS Y CONTEXTO

## 🎯 Objetivo

Leer y analizar TODOS los archivos Legacy relacionados con empleados y nómina para comprender la lógica de negocio completa ANTES de escribir código.

## 📚 Archivos a Analizar

### 1. EmpleadosService.cs (800 líneas)

**Ubicación:** `Codigo Fuente Mi Gente/MiGente_Front/Services/EmpleadosService.cs`

**Métodos Críticos a Documentar (32 total):**

1. `getEmpleados(userID)` - Listar empleados
2. `getVEmpleados(userID)` - Listar con vista
3. `getContrataciones(userID)` - Listar temporales
4. `getEmpleadosByID(userID, id)` - Obtener empleado con recibos
5. `obtenerRemuneraciones(userID, empleadoID)` - Listar remuneraciones extras
6. `quitarRemuneracion(userID, id)` - Eliminar remuneración
7. `guardarEmpleado(empleado)` - Crear empleado
8. `actualizarEmpleado(empleado)` - Actualizar empleado (método 1)
9. `ActualizarEmpleado(empleado)` - Actualizar empleado (método 2)
10. **`procesarPago(header, detalle)`** - ⚠️ CRÍTICO: Generar recibo de pago (2 DbContext)
11. **`procesarPagoContratacion(header, detalle)`** - ⚠️ CRÍTICO: Pago temporal + actualizar estatus
12. `GetEmpleador_Recibos_Empleado(userID, empleadoID)` - Listar recibos
13. `GetEmpleador_ReciboByPagoID(pagoID)` - Obtener recibo con detalles
14. `GetContratacion_ReciboByPagoID(pagoID)` - Obtener recibo contratación
15. `cancelarTrabajo(contratacionID, detalleID)` - Cancelar contratación
16. `eliminarReciboEmpleado(pagoID)` - Eliminar recibo (hard delete)
17. `eliminarReciboContratacion(pagoID)` - Eliminar recibo contratación
18. `eliminarEmpleadoTemporal(contratacionID)` - Eliminar temporal
19. `GetEmpleador_RecibosContratacionesByID(contratacionID, detalleID)` - Listar recibos contratación
20. **`darDeBaja(empleadoID, userID, fechaBaja, prestaciones, motivo)`** - Soft delete + prestaciones
21. `nuevoTemporal(temp, det)` - Crear temporal + detalle contratación
22. `nuevaContratacionTemporal(det)` - Agregar detalle contratación
23. `actualizarContratacion(det)` - Actualizar contratación
24. `calificarContratacion(contratacionID, calificacionID)` - Relacionar calificación
25. `modificarCalificacionDeContratacion(cal)` - Actualizar calificación
26. `obtenerFichaTemporales(contratacionID, userID)` - Obtener temporal con detalles
27. `obtenerTodosLosTemporales(userID)` - Listar todos temporales
28. `obtenerVistaTemporal(contratacionID, userID)` - Obtener vista temporal
29. **`consultarPadron(cedula)`** - ⚠️ API EXTERNA: Validar cédula dominicana
30. `guardarOtrasRemuneraciones(rem)` - Guardar múltiples remuneraciones
31. `actualizarRemuneraciones(rem, empleadoID)` - Actualizar remuneraciones (delete + insert)
32. **`deducciones()`** - Obtener deducciones TSS configuradas

### 2. fichaEmpleado.aspx.cs (500+ líneas)

**Ubicación:** `Codigo Fuente Mi Gente/MiGente_Front/Empleador/fichaEmpleado.aspx.cs`

**Lógica a Documentar:**

- `obtenerFicha()` - Cargar ficha completa del empleado
- `btnRealizarPago_Click()` - Iniciar proceso de pago
- **`armarNovedad()`** - ⚠️ CRÍTICO: Calcular nómina completa con TSS
- `procesarPago()` - Ejecutar pago y generar recibo
- `imprimirReciboPago()` - Imprimir PDF del recibo
- `DarDeBaja()` - Dar de baja con cálculo de prestaciones
- `procesarPagoDescargo()` - Generar recibo de liquidación

### 3. colaboradores.aspx.cs (250 líneas)

**Ubicación:** `Codigo Fuente Mi Gente/MiGente_Front/Empleador/colaboradores.aspx.cs`

**Lógica a Documentar:**

- `GetColaboradores()` - WebMethod para listado paginado
- `GetColaboradoresInactivos()` - Empleados dados de baja
- `GetContratacionesTemporales()` - Temporales con filtro de estatus

### 4. nomina.aspx.cs (150 líneas)

**Ubicación:** `Codigo Fuente Mi Gente/MiGente_Front/Empleador/nomina.aspx.cs`

**Lógica a Documentar:**

- Gestión de percepciones y deducciones
- Cálculo de nómina con ViewState

### 5. Empleado.cs (Domain Entity - Ya migrado)

**Ubicación:** `MiGenteEnLinea.Clean/src/Core/MiGenteEnLinea.Domain/Entities/Empleados/Empleado.cs`

**Métodos de Dominio Disponibles:**

- `Create()` - Factory method
- `ActualizarInformacionPersonal()`
- `ActualizarDireccion()`
- `ActualizarContacto()`
- `ActualizarPosicion()` - Con evento SalarioActualizadoEvent
- `AgregarRemuneracionExtra(numero, descripcion, monto)`
- `EliminarRemuneracionExtra(numero)`
- `MarcarContratoFirmado()`
- `InscribirEnTss()`
- `Desactivar(motivoBaja, prestaciones)` - Soft delete
- `Reactivar()`
- `CalcularSalarioMensual()` - Según período de pago
- `CalcularTotalExtras()` - Suma de bonos
- `CalcularEdad()` - Basado en fecha de nacimiento
- `CalcularAntiguedad()` - Años de servicio

---

## 📝 TAREAS A EJECUTAR

### Paso 1: Leer EmpleadosService.cs Completo (20 mins)

```powershell
# Ejecutar desde workspace root
code "Codigo Fuente Mi Gente/MiGente_Front/Services/EmpleadosService.cs"
```

**Documentar en `CHECKPOINT_4.1_ANALISIS.md`:**

Para CADA método:

1. **Firma:** Nombre + parámetros + tipo de retorno
2. **Lógica:** Paso a paso en pseudocódigo
3. **Dependencias:** Entidades/tablas usadas
4. **Queries:** LINQ queries específicos (Include, Where, OrderBy)
5. **Códigos de retorno:** Si retorna int/bool, qué significa cada valor
6. **DbContext:** ¿Usa 1 o múltiples DbContext?
7. **Mapeo CQRS:** ¿Es Command o Query?

**Ejemplo de Documentación:**

```markdown
### Método 10: procesarPago(header, detalle)

**Firma:**
```csharp
public int procesarPago(Empleador_Recibos_Header header, List<Empleador_Recibos_Detalle> detalle)
```

**Retorno:** int (pagoID generado)

**Lógica (5 pasos):**

1. Crear nuevo DbContext #1
2. Agregar header a Empleador_Recibos_Header
3. SaveChanges() → esto genera pagoID (auto-increment)
4. Crear nuevo DbContext #2 (⚠️ separado)
5. Para cada detalle:
   - Asignar pagoID del header
6. AddRange detalles a Empleador_Recibos_Detalle
7. SaveChanges()
8. Retornar header.pagoID

**Dependencias:**

- Empleador_Recibos_Header (tabla)
- Empleador_Recibos_Detalle (tabla)

**Queries:**

```csharp
db.Empleador_Recibos_Header.Add(header);
db.SaveChanges(); // Genera pagoID

db1.Empleador_Recibos_Detalle.AddRange(detalle);
db1.SaveChanges();
```

**⚠️ CRÍTICO:**

- Usa 2 DbContext separados deliberadamente
- NO usar transacción única
- Razón: Necesita pagoID generado antes de insertar detalles

**Mapeo CQRS:** ProcesarPagoCommand

```

### Paso 2: Leer fichaEmpleado.aspx.cs - Método armarNovedad() (15 mins)

**Objetivo:** Entender cálculo de nómina completo

**Documentar:**
- Cálculo de salario según período (semanal/quincenal/mensual)
- Cálculo de fracción de salario (días trabajados)
- Aplicación de deducciones TSS (AFP, ARS, etc.)
- Lógica de "fraccion" vs pago completo
- Cálculo de regalía pascual

### Paso 3: Leer colaboradores.aspx.cs - WebMethods (10 mins)

**Documentar:**
- Lógica de paginación (Skip, Take)
- Filtros de búsqueda (searchTerm)
- Ordenamiento (OrderByDescending)

### Paso 4: Verificar Entidades de Dominio Migradas (10 mins)

**Comando:**
```powershell
cd "MiGenteEnLinea.Clean/src/Core/MiGenteEnLinea.Domain/Entities"
ls -Recurse -Filter *Empleado*.cs | Select-Object Name, Directory
```

**Verificar que existen:**

- ✅ Empleados/Empleado.cs
- ✅ Empleados/EmpleadoNota.cs
- ✅ Empleados/EmpleadoTemporal.cs
- ✅ Nominas/ReciboHeader.cs
- ✅ Nominas/ReciboDetalle.cs
- ✅ Nominas/DeduccionTss.cs

### Paso 5: Crear Documento de Mapeo (15 mins)

**Crear:** `CHECKPOINT_4.1_ANALISIS.md`

**Estructura del documento:**

```markdown
# CHECKPOINT 4.1: ANÁLISIS LEGACY COMPLETADO ✅

**Fecha:** 2025-10-13  
**Tiempo Invertido:** 60 minutos  
**Archivos Analizados:** 5

---

## 📊 INVENTARIO DE MÉTODOS LEGACY

### EmpleadosService.cs

| # | Método Legacy | Tipo | Complejidad | Sub-Lote | Prioridad |
|---|---------------|------|-------------|----------|-----------|
| 1 | getEmpleados(userID) | Query | 🟢 BAJA | 4.2 | Alta |
| 2 | getEmpleadosByID(...) | Query | 🟡 MEDIA | 4.2 | Alta |
| ... | ... | ... | ... | ... | ... |
| 10 | procesarPago(...) | Command | 🔴 ALTA | 4.4 | Crítica |
| ... | ... | ... | ... | ... | ... |

**Total:** 32 métodos

---

## 🔍 ANÁLISIS DETALLADO POR MÉTODO

### [Aquí va el análisis completo de cada método como en el ejemplo anterior]

---

## 📋 MAPEO A CQRS

### SUB-LOTE 4.2: CRUD BÁSICO (3 Commands, 2 Queries)

**Commands:**
1. **CreateEmpleadoCommand** 
   - Legacy: guardarEmpleado()
   - Handler: Usar Empleado.Create()
   - Validations: 10+ reglas

2. **UpdateEmpleadoCommand**
   - Legacy: actualizarEmpleado() + ActualizarEmpleado()
   - Handler: Llamar múltiples métodos de dominio
   - Validations: Campos opcionales

3. **DesactivarEmpleadoCommand**
   - Legacy: darDeBaja()
   - Handler: Usar Empleado.Desactivar()
   - Validations: Motivo requerido

**Queries:**
1. **GetEmpleadoByIdQuery**
   - Legacy: getEmpleadosByID()
   - Include: Remuneraciones, Recibos (opcional)

2. **GetEmpleadosByEmpleadorQuery**
   - Legacy: getEmpleados() + getVEmpleados()
   - Paginación: Sí
   - Filtros: searchTerm, activo/inactivo

---

## 🎯 DECISIONES TÉCNICAS

### 1. Procesamiento de Nómina (procesarPago)

**Decisión:** Crear INominaCalculatorService en Application/Common/Services/

**Justificación:**
- Lógica de cálculo compleja (TSS, fracciones, extras)
- No pertenece al dominio (es lógica de aplicación)
- Reutilizable en múltiples Commands

**Firma propuesta:**
```csharp
public interface INominaCalculatorService
{
    Task<NominaCalculoResult> CalcularNomina(
        int empleadoId,
        DateTime fechaPago,
        TipoPago tipo, // 1=Salario, 2=Regalía
        PeriodoPago periodo, // 1=Completo, 2=Fracción
        bool aplicarTss);
}
```

### 2. Múltiples DbContext en procesarPago()

**Decisión:** Mantener patrón Legacy (2 SaveChanges separados)

**Justificación:**

- Legacy funciona así hace años
- Necesita pagoID generado antes de insertar detalles
- NO usar transacción envolvente

**Alternativa futura:** Usar TransactionScope si se requiere rollback atómico

### 3. Deducciones TSS

**Decisión:** Query a tabla Deducciones_TSS en cada cálculo

**Justificación:**

- Porcentajes pueden cambiar por ley
- No hardcodear valores

---

## 🚨 PUNTOS CRÍTICOS IDENTIFICADOS

### 1. armarNovedad() - Cálculo Complejo

**Complejidad:** 🔴 ALTA  
**Problema:** 150+ líneas de lógica mezclada (UI + cálculo)  
**Solución:** Extraer a INominaCalculatorService

**Lógica a replicar:**

- Dividendo según período: Semanal=4, Quincenal=2, Mensual=1
- Fracción: (salario / 23.83) * diasTrabajados
- Deducciones: salario *(porcentaje / 100)* -1 (negativo)
- Remuneraciones extras: mismo dividendo que salario

### 2. procesarPago() - 2 DbContext

**Complejidad:** 🟡 MEDIA  
**Problema:** Patrón no estándar  
**Solución:** Documentar claramente, NO cambiar comportamiento

### 3. consultarPadron() - API Externa

**Complejidad:** 🟡 MEDIA  
**Problema:** Autenticación + manejo de errores  
**Solución:** Crear IPadronService en Infrastructure

---

## ✅ ENTREGABLES

- [x] EmpleadosService.cs analizado (32 métodos documentados)
- [x] fichaEmpleado.aspx.cs analizado (método armarNovedad())
- [x] colaboradores.aspx.cs analizado (WebMethods)
- [x] Entidades Domain verificadas (6 entidades)
- [x] Mapeo a CQRS completado (Commands y Queries identificados)
- [x] Decisiones técnicas documentadas
- [x] Puntos críticos identificados

---

## 🎯 SIGUIENTE PASO

**Ejecutar:** SUB-LOTE 4.2 - CRUD BÁSICO DE EMPLEADOS

**Leer primero:** Este archivo (CHECKPOINT_4.1_ANALISIS.md) antes de escribir código

**Compilar antes de continuar:** NO (este lote no genera código)

---

**Generado por:** GitHub Copilot  
**Validado por:** [Nombre desarrollador]  
**Fecha:** 2025-10-13

```

---

## ✅ CRITERIOS DE COMPLETADO

- [ ] EmpleadosService.cs leído completo (32 métodos)
- [ ] fichaEmpleado.aspx.cs leído (método armarNovedad)
- [ ] colaboradores.aspx.cs leído (WebMethods)
- [ ] Entidades Domain verificadas (6 entidades existen)
- [ ] Documento CHECKPOINT_4.1_ANALISIS.md creado
- [ ] Mapeo de 32 métodos Legacy → Commands/Queries completado
- [ ] Decisiones técnicas documentadas (3 mínimo)
- [ ] Puntos críticos identificados (3 mínimo)
- [ ] **NO compilar** (no se genera código en este lote)
- [ ] Archivo commiteado a Git

---

## 🔄 ACTUALIZACIÓN DE CONTEXTO

Al finalizar, agregar al final de `CHECKPOINT_4.1_ANALISIS.md`:

```markdown
---

## 🔄 CONTEXTO PARA SIGUIENTE PROMPT

**Estado Actual:**
- ✅ Análisis Legacy completado (60 minutos)
- ✅ 32 métodos documentados
- ✅ Mapeo CQRS identificado
- ✅ Decisiones técnicas tomadas

**Entidades Domain Disponibles:**
- Empleado (con 15+ métodos)
- EmpleadoNota
- EmpleadoTemporal
- ReciboHeader (con agregados)
- ReciboDetalle
- DeduccionTss

**Próximo Sub-Lote:** 4.2 - CRUD BÁSICO DE EMPLEADOS

**Archivos a crear (estimado):**
- 3 Commands × 3 archivos = 9
- 2 Queries × 2 archivos = 4
- Total: ~13 archivos (~900 líneas)

**Tiempo estimado:** 2-3 horas

**⚠️ LEER ESTE ARCHIVO COMPLETO ANTES DE EJECUTAR SUB-LOTE 4.2**
```

---

# 📦 SUB-LOTE 4.2: CRUD BÁSICO DE EMPLEADOS

## 🎯 Objetivo

Implementar operaciones CRUD básicas para empleados: Crear, Leer, Actualizar y Desactivar (soft delete).

## 📚 PRE-REQUISITOS

**OBLIGATORIO:**

```markdown
✅ Leer CHECKPOINT_4.1_ANALISIS.md completo ANTES de continuar
✅ Verificar que Domain entities existen: Empleado, EmpleadoNota
✅ Verificar IApplicationDbContext tiene DbSet<Empleado> Empleados
```

## 📦 ARCHIVOS A CREAR (12 archivos, ~900 líneas)

### 1. CreateEmpleadoCommand (3 archivos, ~250 líneas)

**Ubicación:**

```
Features/Empleados/Commands/CreateEmpleado/
├── CreateEmpleadoCommand.cs (~60 líneas)
├── CreateEmpleadoCommandHandler.cs (~120 líneas)
└── CreateEmpleadoCommandValidator.cs (~70 líneas)
```

**Legacy Reference:** `EmpleadosService.guardarEmpleado(empleado)`

**Propiedades del Command (18 campos):**

```csharp
public record CreateEmpleadoCommand : IRequest<CreateEmpleadoResult>
{
    // Identificación empleador y empleado
    public string UserId { get; init; } = null!; // GUID del empleador
    public string Identificacion { get; init; } = null!; // Cédula 11 dígitos
    
    // Información personal
    public string Nombre { get; init; } = null!;
    public string Apellido { get; init; } = null!;
    public string? Alias { get; init; }
    public DateOnly? FechaInicio { get; init; }
    public DateOnly? Nacimiento { get; init; }
    public int? EstadoCivil { get; init; } // 1-5
    
    // Ubicación
    public string? Direccion { get; init; }
    public string? Provincia { get; init; }
    public string? Municipio { get; init; }
    
    // Contacto
    public string? Telefono1 { get; init; }
    public string? Telefono2 { get; init; }
    public string? ContactoEmergencia { get; init; }
    public string? TelefonoEmergencia { get; init; }
    
    // Laboral
    public string? Posicion { get; init; }
    public decimal Salario { get; init; } // > 0
    public int PeriodoPago { get; init; } // 1=Semanal, 2=Quincenal, 3=Mensual
}
```

**Handler Logic (7 pasos):**

```csharp
public async Task<CreateEmpleadoResult> Handle(CreateEmpleadoCommand request, CancellationToken ct)
{
    // PASO 1: Validar que empleador existe
    var empleadorExists = await _context.Credenciales
        .AnyAsync(c => c.UserId == request.UserId, ct);
    if (!empleadorExists)
        throw new NotFoundException("Empleador no encontrado");
    
    // PASO 2: Validar que identificación no esté duplicada para este empleador
    var duplicado = await _context.Empleados
        .AnyAsync(e => e.UserId == request.UserId && 
                       e.Identificacion == request.Identificacion, ct);
    if (duplicado)
        throw new ValidationException("Ya existe un empleado con esta identificación");
    
    // PASO 3: Crear empleado usando factory method de dominio
    var empleado = Empleado.Create(
        request.UserId,
        request.Identificacion,
        request.Nombre,
        request.Apellido,
        request.Salario,
        request.PeriodoPago
    );
    
    // PASO 4: Actualizar información adicional (métodos de dominio)
    if (!string.IsNullOrWhiteSpace(request.Alias) || request.Nacimiento.HasValue)
    {
        empleado.ActualizarInformacionPersonal(
            request.Nombre,
            request.Apellido,
            request.Nacimiento,
            request.EstadoCivil,
            request.Alias
        );
    }
    
    if (!string.IsNullOrWhiteSpace(request.Direccion))
    {
        empleado.ActualizarDireccion(
            request.Direccion,
            request.Provincia,
            request.Municipio
        );
    }
    
    if (!string.IsNullOrWhiteSpace(request.Telefono1))
    {
        empleado.ActualizarContacto(
            request.Telefono1,
            request.Telefono2,
            request.ContactoEmergencia,
            request.TelefonoEmergencia
        );
    }
    
    if (!string.IsNullOrWhiteSpace(request.Posicion))
    {
        empleado.ActualizarPosicion(
            request.Posicion,
            request.Salario,
            request.PeriodoPago
        );
    }
    
    if (request.FechaInicio.HasValue)
    {
        empleado.ActualizarFechaInicio(request.FechaInicio.Value);
    }
    
    // PASO 5: Guardar en DbContext
    await _context.Empleados.AddAsync(empleado, ct);
    await _context.SaveChangesAsync(ct);
    
    // PASO 6: Log
    _logger.LogInformation(
        "Empleado creado: EmpleadoId={EmpleadoId}, UserId={UserId}, Nombre={Nombre}",
        empleado.EmpleadoId,
        empleado.UserId,
        empleado.NombreCompleto
    );
    
    // PASO 7: Retornar resultado
    return new CreateEmpleadoResult
    {
        EmpleadoId = empleado.EmpleadoId,
        Message = "Empleado creado exitosamente"
    };
}
```

**Validator (15+ reglas):**

```csharp
public class CreateEmpleadoCommandValidator : AbstractValidator<CreateEmpleadoCommand>
{
    public CreateEmpleadoCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .Must(BeValidGuid).WithMessage("UserId debe ser un GUID válido");
        
        RuleFor(x => x.Identificacion)
            .NotEmpty()
            .Length(11).WithMessage("Identificación debe tener 11 caracteres (cédula dominicana)")
            .Matches(@"^\d{11}$").WithMessage("Identificación debe contener solo dígitos");
        
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .MaximumLength(40);
        
        RuleFor(x => x.Apellido)
            .NotEmpty()
            .MaximumLength(40);
        
        RuleFor(x => x.Alias)
            .MaximumLength(50);
        
        RuleFor(x => x.Salario)
            .GreaterThan(0).WithMessage("Salario debe ser mayor a cero");
        
        RuleFor(x => x.PeriodoPago)
            .InclusiveBetween(1, 3)
            .WithMessage("PeriodoPago debe ser 1 (Semanal), 2 (Quincenal) o 3 (Mensual)");
        
        RuleFor(x => x.EstadoCivil)
            .InclusiveBetween(1, 5)
            .When(x => x.EstadoCivil.HasValue)
            .WithMessage("EstadoCivil debe estar entre 1 y 5");
        
        RuleFor(x => x.Telefono1)
            .MaximumLength(16);
        
        RuleFor(x => x.Provincia)
            .MaximumLength(50);
        
        // ... más validaciones
    }
    
    private bool BeValidGuid(string value)
    {
        return Guid.TryParse(value, out _);
    }
}
```

---

### 2. UpdateEmpleadoCommand (3 archivos, ~250 líneas)

**Legacy Reference:** `EmpleadosService.actualizarEmpleado()` + `ActualizarEmpleado()`

**Handler Logic:**

- Buscar empleado por EmpleadoId
- Actualizar con múltiples métodos de dominio (ActualizarInformacionPersonal, etc.)
- Solo actualizar campos no nulos (partial update)

---

### 3. DesactivarEmpleadoCommand (3 archivos, ~180 líneas)

**Legacy Reference:** `EmpleadosService.darDeBaja()`

**Command:**

```csharp
public record DesactivarEmpleadoCommand : IRequest<Unit>
{
    public int EmpleadoId { get; init; }
    public string UserId { get; init; } = null!; // Para validar propiedad
    public DateTime FechaBaja { get; init; }
    public decimal? Prestaciones { get; init; }
    public string MotivoBaja { get; init; } = null!;
}
```

**Handler:**

```csharp
// PASO 1: Buscar empleado
var empleado = await _context.Empleados
    .FirstOrDefaultAsync(e => e.EmpleadoId == request.EmpleadoId && 
                              e.UserId == request.UserId, ct);

if (empleado == null)
    throw new NotFoundException("Empleado no encontrado");

// PASO 2: Desactivar usando método de dominio
empleado.Desactivar(request.MotivoBaja, request.Prestaciones);

// PASO 3: Guardar cambios
await _context.SaveChangesAsync(ct);

_logger.LogInformation(
    "Empleado desactivado: EmpleadoId={EmpleadoId}, Motivo={Motivo}",
    request.EmpleadoId,
    request.MotivoBaja
);

return Unit.Value;
```

---

### 4. GetEmpleadoByIdQuery (2 archivos, ~120 líneas)

**Legacy Reference:** `EmpleadosService.getEmpleadosByID()`

**Query:**

```csharp
public record GetEmpleadoByIdQuery : IRequest<EmpleadoDetalleDto>
{
    public int EmpleadoId { get; init; }
    public string UserId { get; init; } = null!; // Para validar propiedad
    public bool IncludeRecibos { get; init; } = false;
}
```

**Handler:**

```csharp
var query = _context.Empleados.AsNoTracking();

// Incluir recibos si se solicita (navegación opcional)
if (request.IncludeRecibos)
{
    query = query.Include(e => e.RecibosHeader)
                 .ThenInclude(r => r.Detalles);
}

var empleado = await query
    .Where(e => e.EmpleadoId == request.EmpleadoId && 
                e.UserId == request.UserId)
    .Select(e => new EmpleadoDetalleDto
    {
        EmpleadoId = e.EmpleadoId,
        UserId = e.UserId,
        FechaRegistro = e.FechaRegistro,
        FechaInicio = e.FechaInicio,
        Identificacion = e.Identificacion,
        NombreCompleto = e.NombreCompleto, // Computed property del dominio
        Salario = e.Salario,
        PeriodoPago = e.PeriodoPago,
        Posicion = e.Posicion,
        Activo = e.Activo,
        InscritoTss = e.InscritoTss,
        TieneContrato = e.TieneContrato,
        // Campos calculados
        SalarioMensual = e.CalcularSalarioMensual(),
        TotalExtras = e.CalcularTotalExtras(),
        Edad = e.CalcularEdad(),
        Antiguedad = e.CalcularAntiguedad(),
        // ... resto de campos
    })
    .FirstOrDefaultAsync(ct);

if (empleado == null)
    throw new NotFoundException("Empleado no encontrado");

return empleado;
```

---

### 5. GetEmpleadosByEmpleadorQuery (2 archivos, ~150 líneas)

**Legacy Reference:** `EmpleadosService.getEmpleados()` + `getVEmpleados()`

**Query con paginación:**

```csharp
public record GetEmpleadosByEmpleadorQuery : IRequest<GetEmpleadosResult>
{
    public string UserId { get; init; } = null!;
    public string? SearchTerm { get; init; }
    public bool SoloActivos { get; init; } = true;
    public int PageIndex { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
```

**Handler:**

```csharp
var query = _context.Empleados.AsNoTracking()
    .Where(e => e.UserId == request.UserId);

// Filtro activo/inactivo
if (request.SoloActivos)
    query = query.Where(e => e.Activo);

// Búsqueda case-insensitive
if (!string.IsNullOrWhiteSpace(request.SearchTerm))
{
    var searchLower = request.SearchTerm.ToLower();
    query = query.Where(e =>
        e.Nombre.ToLower().Contains(searchLower) ||
        e.Apellido.ToLower().Contains(searchLower) ||
        e.Identificacion.Contains(searchLower)
    );
}

// Contar total
var totalRecords = await query.CountAsync(ct);

// Paginar
var empleados = await query
    .OrderByDescending(e => e.FechaRegistro)
    .Skip((request.PageIndex - 1) * request.PageSize)
    .Take(request.PageSize)
    .Select(e => new EmpleadoListDto
    {
        EmpleadoId = e.EmpleadoId,
        Identificacion = e.Identificacion,
        NombreCompleto = e.NombreCompleto,
        Salario = e.Salario,
        PeriodoPago = e.PeriodoPago,
        Activo = e.Activo,
        FechaInicio = e.FechaInicio,
        Foto = e.Foto
    })
    .ToListAsync(ct);

return new GetEmpleadosResult
{
    Empleados = empleados,
    TotalRecords = totalRecords,
    PageIndex = request.PageIndex,
    PageSize = request.PageSize,
    TotalPages = (int)Math.Ceiling(totalRecords / (double)request.PageSize)
};
```

---

### 6. EmpleadoDetalleDto (1 archivo, ~100 líneas)

```csharp
public record EmpleadoDetalleDto
{
    // Identificación
    public int EmpleadoId { get; init; }
    public string UserId { get; init; } = null!;
    public DateTime FechaRegistro { get; init; }
    public DateOnly? FechaInicio { get; init; }
    
    // Personal
    public string Identificacion { get; init; } = null!;
    public string Nombre { get; init; } = null!;
    public string Apellido { get; init; } = null!;
    public string NombreCompleto { get; init; } = null!;
    public string? Alias { get; init; }
    public DateOnly? Nacimiento { get; init; }
    public int? EstadoCivil { get; init; }
    
    // Ubicación
    public string? Direccion { get; init; }
    public string? Provincia { get; init; }
    public string? Municipio { get; init; }
    
    // Contacto
    public string? Telefono1 { get; init; }
    public string? Telefono2 { get; init; }
    public string? ContactoEmergencia { get; init; }
    public string? TelefonoEmergencia { get; init; }
    
    // Laboral
    public string? Posicion { get; init; }
    public decimal Salario { get; init; }
    public int PeriodoPago { get; init; }
    public bool InscritoTss { get; init; }
    public bool TieneContrato { get; init; }
    public int? DiasPago { get; init; }
    
    // Remuneraciones extras
    public string? RemuneracionExtra1 { get; init; }
    public decimal? MontoExtra1 { get; init; }
    public string? RemuneracionExtra2 { get; init; }
    public decimal? MontoExtra2 { get; init; }
    public string? RemuneracionExtra3 { get; init; }
    public decimal? MontoExtra3 { get; init; }
    
    // Estado
    public bool Activo { get; init; }
    public DateTime? FechaSalida { get; init; }
    public string? MotivoBaja { get; init; }
    public decimal? Prestaciones { get; init; }
    
    // Campos calculados (del dominio)
    public decimal SalarioMensual { get; init; }
    public decimal TotalExtras { get; init; }
    public int? Edad { get; init; }
    public int? Antiguedad { get; init; }
    
    // Foto
    public string? Foto { get; init; }
}

public record EmpleadoListDto
{
    public int EmpleadoId { get; init; }
    public string Identificacion { get; init; } = null!;
    public string NombreCompleto { get; init; } = null!;
    public decimal Salario { get; init; }
    public int PeriodoPago { get; init; }
    public bool Activo { get; init; }
    public DateOnly? FechaInicio { get; init; }
    public string? Foto { get; init; }
}
```

---

## ✅ CHECKLIST DE IMPLEMENTACIÓN

### Pre-Implementación

- [ ] Leer CHECKPOINT_4.1_ANALISIS.md completo
- [ ] Verificar Domain entities existen
- [ ] Verificar IApplicationDbContext actualizado

### Implementación

- [ ] CreateEmpleadoCommand creado (3 archivos)
- [ ] UpdateEmpleadoCommand creado (3 archivos)
- [ ] DesactivarEmpleadoCommand creado (3 archivos)
- [ ] GetEmpleadoByIdQuery creado (2 archivos)
- [ ] GetEmpleadosByEmpleadorQuery creado (2 archivos)
- [ ] EmpleadoDetalleDto creado (1 archivo)
- [ ] EmpleadoListDto creado (en mismo archivo)

### Validación

- [ ] `dotnet build` → 0 errores
- [ ] Warnings solo de Domain (nullability)
- [ ] Crear CHECKPOINT_4.2_CRUD_EMPLEADOS.md con reporte

### Documentación

- [ ] Listar 12 archivos creados con líneas
- [ ] Documentar decisiones técnicas
- [ ] Actualizar contexto para SUB-LOTE 4.3

---

## 🔄 ACTUALIZACIÓN DE CONTEXTO

Al finalizar, crear `CHECKPOINT_4.2_CRUD_EMPLEADOS.md`:

```markdown
# CHECKPOINT 4.2: CRUD BÁSICO EMPLEADOS ✅

**Fecha:** 2025-10-13  
**Tiempo Invertido:** 2.5 horas  
**Archivos Creados:** 12 (~900 líneas)

---

## 📊 ARCHIVOS CREADOS

### Commands (9 archivos, ~680 líneas)
1. CreateEmpleadoCommand + Handler + Validator
2. UpdateEmpleadoCommand + Handler + Validator
3. DesactivarEmpleadoCommand + Handler + Validator

### Queries (4 archivos, ~270 líneas)
1. GetEmpleadoByIdQuery + Handler
2. GetEmpleadosByEmpleadorQuery + Handler

### DTOs (1 archivo, ~100 líneas)
1. EmpleadoDetalleDto + EmpleadoListDto

---

## ✅ VALIDACIÓN

```

dotnet build
✅ BUILD SUCCEEDED
   0 Error(s)
   2 Warning(s) (pre-existentes Domain)
   Time: 18.45s

```

---

## 🔄 CONTEXTO PARA SIGUIENTE PROMPT

**Estado Actual:**
- ✅ SUB-LOTE 4.1 completado (Análisis)
- ✅ SUB-LOTE 4.2 completado (CRUD Básico)
- ⏳ SUB-LOTE 4.3 siguiente (Remuneraciones Extras)

**Próximo Sub-Lote:** 4.3 - REMUNERACIONES EXTRAS

**Archivos a crear:** ~9 archivos (~600 líneas)
**Tiempo estimado:** 1-2 horas

**⚠️ LEER CHECKPOINT_4.1 Y CHECKPOINT_4.2 ANTES DE CONTINUAR**
```

---

# 📦 SUB-LOTE 4.3: REMUNERACIONES EXTRAS

[... Continuar con prompts similares para SUB-LOTES 4.3 a 4.6 ...]

---

# 🎯 RESUMEN DE EJECUCIÓN

## Secuencia de Prompts

1. **SUB-LOTE 4.1:** Análisis → 60 mins → 0 archivos código
2. **SUB-LOTE 4.2:** CRUD Básico → 2-3 horas → 12 archivos
3. **SUB-LOTE 4.3:** Remuneraciones → 1-2 horas → 9 archivos
4. **SUB-LOTE 4.4:** Nómina → 3-4 horas → 15 archivos
5. **SUB-LOTE 4.5:** Temporales → 2-3 horas → 12 archivos
6. **SUB-LOTE 4.6:** API Padrón + Controller → 1-2 horas → 8 archivos

**Total:** 12-15 horas, 56 archivos

---

**Generado por:** GitHub Copilot  
**Fecha:** 2025-10-13  
**Versión:** 1.0 - Primera iteración
