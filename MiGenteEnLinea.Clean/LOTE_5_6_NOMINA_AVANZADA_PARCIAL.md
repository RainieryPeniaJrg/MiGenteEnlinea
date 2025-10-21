# LOTE 5.6: Nómina Avanzada - EN PROGRESO ⏸️

**Fecha Inicio:** 2025-10-18  
**Branch:** `feature/lote-5.6-nomina-avanzada`  
**Estado:** PARCIAL (~30%) - BLOQUEADO  
**Build:** ❌ 6 errores de compilación

---

## 📊 Resumen Ejecutivo

**Objetivo:** Implementar funcionalidades avanzadas de nómina incluyendo batch processing, generación masiva de PDFs, exportación a Excel y queries de análisis.

**Resultado Actual:** 6 archivos creados (~800 líneas), estructura base lista pero bloqueada por dependencias del Domain Layer.

---

## 🎯 Progreso Actual

| Categoría | Completado | Total | % |
|-----------|------------|-------|---|
| Commands | 2/5 | 5 | 40% |
| Queries | 1/3 | 3 | 33% |
| DTOs | 3/3 | 3 | 100% |
| Handlers | 1/5 | 5 | 20% |
| Validators | 1/5 | 5 | 20% |
| Controllers | 0/1 | 1 | 0% |
| **TOTAL LOTE 5.6** | **8/22** | **22** | **~30%** ⏸️ |

---

## ✅ Archivos Creados (6 total)

### 1. ProcesarNominaLoteCommand.cs (~85 líneas)

```
src/Core/MiGenteEnLinea.Application/Features/Nominas/Commands/ProcesarNominaLote/
└── ProcesarNominaLoteCommand.cs
```

**Funcionalidad:**

- Command para batch processing de nómina (múltiples empleados)
- Propiedades: EmpleadorId, Periodo, FechaPago, Lista de Empleados
- Opciones: GenerarPdfs (bool), EnviarEmails (bool)
- Retorna: ProcesarNominaLoteResult con contadores y errores

**Estructura de Datos:**

```csharp
public record EmpleadoNominaItem
{
    public int EmpleadoId { get; init; }
    public decimal Salario { get; init; }
    public List<ConceptoNominaItem> Conceptos { get; init; }
}

public record ConceptoNominaItem
{
    public string Concepto { get; init; }
    public decimal Monto { get; init; }
    public bool EsDeduccion { get; init; } // true = deducción, false = ingreso
}
```

---

### 2. ProcesarNominaLoteCommandHandler.cs (~190 líneas)

```
src/Core/MiGenteEnLinea.Application/Features/Nominas/Commands/ProcesarNominaLote/
└── ProcesarNominaLoteCommandHandler.cs
```

**Lógica Implementada:**

1. ✅ Validar empleador existe
2. ✅ Iterar sobre cada empleado
3. ✅ Validar empleado existe y pertenece al empleador
4. ✅ Calcular ingresos + deducciones + neto
5. ❌ Crear ReciboHeader (BLOQUEADO - método Crear() no existe)
6. ❌ Crear ReciboDetalle para salario base (BLOQUEADO)
7. ❌ Crear ReciboDetalle para conceptos adicionales (BLOQUEADO)
8. ✅ Manejo de errores individuales sin detener proceso completo
9. ✅ Retornar resultado con contadores y errores

**❌ ERRORES DE COMPILACIÓN (6 total):**

```
Error CS0117: 'ReciboHeader' does not contain a definition for 'Crear'
Error CS1061: 'IUnitOfWork' does not contain a definition for 'RecibosHeader'
Error CS0117: 'ReciboDetalle' does not contain a definition for 'Crear'
Error CS1061: 'IUnitOfWork' does not contain a definition for 'RecibosDetalle'
```

**CAUSA RAÍZ:**

- `ReciboHeader` y `ReciboDetalle` en Domain no tienen factory methods `Crear()`
- `IUnitOfWork` no expone repositorios `RecibosHeader` y `RecibosDetalle`

---

### 3. ProcesarNominaLoteCommandValidator.cs (~80 líneas)

```
src/Core/MiGenteEnLinea.Application/Features/Nominas/Commands/ProcesarNominaLote/
└── ProcesarNominaLoteCommandValidator.cs
```

**Validaciones Implementadas:**

- EmpleadorId > 0
- Periodo: Required, max 20 chars
- FechaPago: >= 2020-01-01
- Empleados: Min 1, Max 500 (limit batch)
- EmpleadoId > 0
- Salario: > 0 y <= RD$1,000,000
- Concepto: Required, max 100 chars
- Monto concepto: > 0
- Detalle concepto: Optional, max 250 chars
- Notas: Optional, max 500 chars

---

### 4. GenerarRecibosPdfLoteCommand.cs (~45 líneas)

```
src/Core/MiGenteEnLinea.Application/Features/Nominas/Commands/GenerarRecibosPdfLote/
└── GenerarRecibosPdfLoteCommand.cs
```

**Funcionalidad (SOLO ESTRUCTURA):**

- Command para generar PDFs en batch
- Propiedades: List<int> ReciboIds, RutaDestino, ComprimirEnZip, NombreArchivoZip
- Retorna: GenerarRecibosPdfLoteResult con rutas archivos y errores

**❌ FALTA IMPLEMENTAR:**

- Handler con integración a PdfService (LOTE 5.3)
- Validator
- Lógica de compresión ZIP

---

### 5. GetNominaResumenQuery.cs (~30 líneas)

```
src/Core/MiGenteEnLinea.Application/Features/Nominas/Queries/GetNominaResumen/
└── GetNominaResumenQuery.cs
```

**Funcionalidad (SOLO ESTRUCTURA):**

- Query para resumen de nómina por período
- Filtros: EmpleadorId, Periodo, FechaInicio/Fin
- Opción: IncluirDetalleEmpleados
- Retorna: NominaResumenDto con totales y estadísticas

**❌ FALTA IMPLEMENTAR:**

- Handler con queries SQL agregadas
- Cálculos de TSS (AFP, SFS, Infotep)
- Promedios y métricas

---

### 6. NominaResumenDto.cs (~90 líneas)

```
src/Core/MiGenteEnLinea.Application/Features/Nominas/DTOs/
└── NominaResumenDto.cs
```

**DTOs Implementados (3 total):**

#### NominaResumenDto

- Totales: TotalEmpleados, TotalRecibos, TotalIngresos, TotalDeducciones, TotalNeto
- Deducciones TSS: TotalAFP, TotalSFS, TotalInfotep
- Promedios: PromedioSalario, PromedioDeducciones
- Detalle: List<EmpleadoNominaResumenDto>

#### EmpleadoNominaResumenDto

- EmpleadoId, NombreCompleto, Identificacion
- CantidadRecibos, TotalIngresos, TotalDeducciones, TotalNeto
- UltimoPago (DateTime?)

#### EstadisticasNominaDto

- Métricas: TotalEmpleadosActivos/Inactivos, MasaSalarial, CostoTotalEmpresa
- Distribución: SalarioMin/Max/Promedio/Mediano
- Deducciones: Legales vs Voluntarias
- Tendencias: VariacionPorcentual, VariacionEmpleados

---

## 🚫 Bloqueadores Críticos

### Bloqueador #1: ReciboHeader sin Factory Method

**Archivo:** `src/Core/MiGenteEnLinea.Domain/Entities/Nominas/ReciboHeader.cs`

**Necesidad:**

```csharp
// REQUERIDO en Domain:
public static ReciboHeader Crear(
    string userId,
    int empleadoId,
    DateTime fechaPago,
    string periodo,
    decimal totalIngresos,
    decimal totalDeducciones,
    decimal montoNeto,
    string? notas = null)
{
    var recibo = new ReciboHeader
    {
        UserId = userId,
        EmpleadoId = empleadoId,
        FechaPago = fechaPago,
        Periodo = periodo,
        TotalIngresos = totalIngresos,
        TotalDeducciones = totalDeducciones,
        MontoNeto = montoNeto,
        Notas = notas,
        Estatus = "Pagado",
        FechaCreacion = DateTime.Now
    };
    
    return recibo;
}
```

---

### Bloqueador #2: ReciboDetalle sin Factory Method

**Archivo:** `src/Core/MiGenteEnLinea.Domain/Entities/Nominas/ReciboDetalle.cs`

**Necesidad:**

```csharp
// REQUERIDO en Domain:
public static ReciboDetalle Crear(
    int pagoId,
    string concepto,
    decimal monto,
    string? detalle = null)
{
    var detalleItem = new ReciboDetalle
    {
        PagoId = pagoId,
        Concepto = concepto,
        Monto = monto,
        Detalle = detalle
    };
    
    return detalleItem;
}
```

---

### Bloqueador #3: IUnitOfWork sin Repositorios

**Archivo:** `src/Core/MiGenteEnLinea.Domain/Interfaces/Repositories/IUnitOfWork.cs`

**Necesidad:**

```csharp
// REQUERIDO agregar en IUnitOfWork:
IReciboHeaderRepository RecibosHeader { get; }
IReciboDetalleRepository RecibosDetalle { get; }
```

---

## ❌ Archivos NO Creados (Pendientes)

### Commands (3 pendientes)

1. **EnviarRecibosEmailLoteCommand** - Envío masivo por email
2. **ExportarNominaExcelCommand** - Exportar a Excel
3. **ImportarEmpleadosExcelCommand** - Importar empleados desde Excel

### Queries (2 pendientes)

4. **GetReciboPorPeriodoQuery** - Recibos por período (mes/año)
5. **GetEstadisticasNominaQuery** - Métricas agregadas

### Handlers (4 pendientes)

6. GenerarRecibosPdfLoteCommandHandler
7. GetNominaResumenQueryHandler
8. GetReciboPorPeriodoQueryHandler
9. GetEstadisticasNominaQueryHandler

### Validators (4 pendientes)

10. GenerarRecibosPdfLoteCommandValidator
11. EnviarRecibosEmailLoteCommandValidator
12. ExportarNominaExcelCommandValidator
13. ImportarEmpleadosExcelCommandValidator

### Controllers (1 pendiente)

14. NominasAvanzadasController

---

## 📋 Plan de Resolución

### Paso 1: Actualizar Domain Layer (CRÍTICO - 1 hora)

```csharp
// 1. Agregar factory methods a ReciboHeader.cs
public static ReciboHeader Crear(...) { }

// 2. Agregar factory methods a ReciboDetalle.cs
public static ReciboDetalle Crear(...) { }

// 3. Crear IReciboHeaderRepository.cs (si no existe)
public interface IReciboHeaderRepository : IRepository<ReciboHeader> { }

// 4. Crear IReciboDetalleRepository.cs (si no existe)
public interface IReciboDetalleRepository : IRepository<ReciboDetalle> { }

// 5. Agregar a IUnitOfWork.cs
IReciboHeaderRepository RecibosHeader { get; }
IReciboDetalleRepository RecibosDetalle { get; }

// 6. Implementar en Infrastructure/Persistence/Repositories/
public class ReciboHeaderRepository : Repository<ReciboHeader>, IReciboHeaderRepository { }
public class ReciboDetalleRepository : Repository<ReciboDetalle>, IReciboDetalleRepository { }

// 7. Registrar en Infrastructure/DependencyInjection.cs
services.AddScoped<IReciboHeaderRepository, ReciboHeaderRepository>();
services.AddScoped<IReciboDetalleRepository, ReciboDetalleRepository>();
```

---

### Paso 2: Completar Handlers (2-3 horas)

1. GenerarRecibosPdfLoteCommandHandler
   - Integrar con PdfService (LOTE 5.3)
   - Implementar lógica de compresión ZIP

2. GetNominaResumenQueryHandler
   - Queries SQL con agregaciones (SUM, AVG, COUNT)
   - Cálculos TSS (AFP, SFS, Infotep)

---

### Paso 3: Crear Commands/Queries Faltantes (3-4 horas)

3. EnviarRecibosEmailLoteCommand
   - Integrar con EmailService (LOTE 5.1)

4. ExportarNominaExcelCommand
   - EPPlus o ClosedXML para Excel

5. GetReciboPorPeriodoQuery & GetEstadisticasNominaQuery

---

### Paso 4: Controller REST (1-2 horas)

6. NominasAvanzadasController
   - POST /api/nominas-avanzadas/procesar-lote
   - POST /api/nominas-avanzadas/generar-pdfs-lote
   - GET /api/nominas-avanzadas/resumen
   - POST /api/nominas-avanzadas/exportar-excel

---

### Paso 5: Testing & Documentation (2 horas)

7. Build validation
8. Manual testing en Swagger UI
9. Crear LOTE_5_6_NOMINA_AVANZADA_COMPLETADO.md

**TIEMPO TOTAL ESTIMADO:** 9-12 horas

---

## 🎯 Decisión Recomendada

**OPCIÓN A:** Pausar LOTE 5.6 y completar Domain Layer primero

- Prioridad: Agregar factory methods y repositorios en Domain/Infrastructure
- Ventaja: Desbloquea LOTE 5.6 y futuros LOTEs que usen ReciboHeader/Detalle

**OPCIÓN B:** Continuar con LOTEs que no dependan de Recibos

- LOTE 5.7: Dashboard & Reports (usa otras entidades)
- Volver a LOTE 5.6 después

**OPCIÓN C:** Refactorizar Handler para usar constructores directos

- Menos elegante pero desbloquea progreso inmediato

---

## 📊 Estado General PLAN 5

| LOTE | Nombre | Estado | Completion |
|------|--------|--------|------------|
| 5.1 | EmailService | ✅ | 100% |
| 5.2 | Calificaciones | ✅ | 100% |
| 5.3 | Utilities (PDF, Image) | ✅ | 100% |
| 5.4 | Bot Integration | ⏸️ POSTPONED | 0% |
| 5.5 | Contrataciones Avanzadas | ✅ | 100% |
| **5.6** | **Nómina Avanzada** | **⏸️** | **30%** |
| 5.7 | Dashboard & Reports | ❌ | 0% |

**Overall PLAN 5:** 4.3/7 LOTEs = **61% complete** 🚀

---

**Documentado por:** GitHub Copilot  
**Fecha:** 2025-10-18  
**Próxima Acción:** Decidir estrategia de resolución (A, B o C)
