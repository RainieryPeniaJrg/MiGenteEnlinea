# 📊 LOTE 4: EMPLEADOS Y NÓMINA - RESUMEN VISUAL

**Documento Principal:** `LOTE_4_EMPLEADOS_NOMINA_PROMPTS_SECUENCIALES.md`  
**Fecha de Creación:** 13 de octubre, 2025  
**Estrategia:** 6 prompts secuenciales con checkpoints intermedios  
**Tiempo Total:** 12-15 horas

---

## 🎯 VISIÓN GENERAL

```
LOTE 4 (Empleados y Nómina)
    ├── SUB-LOTE 4.1: Análisis y Contexto (30-45 mins) 📖
    │   └── CHECKPOINT_4.1_ANALISIS.md
    │
    ├── SUB-LOTE 4.2: CRUD Básico Empleados (2-3 horas) 👤
    │   ├── 3 Commands (Create, Update, Desactivar)
    │   ├── 2 Queries (GetById, GetByEmpleador)
    │   └── CHECKPOINT_4.2_CRUD_EMPLEADOS.md
    │
    ├── SUB-LOTE 4.3: Remuneraciones Extras (1-2 horas) 💰
    │   ├── 2 Commands (Add, Remove)
    │   ├── 1 Query (GetByEmpleado)
    │   └── CHECKPOINT_4.3_REMUNERACIONES.md
    │
    ├── SUB-LOTE 4.4: Procesamiento de Nómina (3-4 horas) 📋
    │   ├── 2 Commands (ProcesarPago, AnularRecibo)
    │   ├── 2 Queries (GetReciboById, GetRecibosByEmpleado)
    │   ├── INominaCalculatorService (lógica compleja)
    │   └── CHECKPOINT_4.4_NOMINA.md
    │
    ├── SUB-LOTE 4.5: Empleados Temporales (2-3 horas) ⏱️
    │   ├── 3 Commands (Create, Update, Delete)
    │   ├── 2 Queries (GetById, GetByEmpleador)
    │   └── CHECKPOINT_4.5_TEMPORALES.md
    │
    └── SUB-LOTE 4.6: API Padrón + Controller (1-2 horas) 🌐
        ├── 1 Query (ConsultarPadronQuery)
        ├── EmpleadosController (15+ endpoints REST)
        └── CHECKPOINT_4.6_CONTROLLER.md
```

---

## 📈 MÉTRICAS DE IMPLEMENTACIÓN

### Por Sub-Lote

| Sub-Lote | Archivos | Líneas | Tiempo | Complejidad | Prioridad |
|----------|----------|--------|--------|-------------|-----------|
| 4.1 Análisis | 1 doc | 0 código | 30-45 min | 🟡 MEDIA | 🔴 CRÍTICA |
| 4.2 CRUD Básico | 12 | ~900 | 2-3 h | 🟡 MEDIA | 🟠 ALTA |
| 4.3 Remuneraciones | 9 | ~600 | 1-2 h | 🟢 BAJA | 🟡 MEDIA |
| 4.4 Nómina | 15 | ~1,200 | 3-4 h | 🔴 ALTA | 🔴 CRÍTICA |
| 4.5 Temporales | 12 | ~800 | 2-3 h | 🟡 MEDIA | 🟡 MEDIA |
| 4.6 API + Controller | 8 | ~600 | 1-2 h | 🟡 MEDIA | 🟠 ALTA |
| **TOTAL** | **56** | **~4,100** | **12-15 h** | | |

### Desglose por Tipo de Archivo

```
Commands:    13 Commands × 3 archivos = 39 archivos
Queries:     10 Queries × 2 archivos  = 20 archivos
DTOs:        5 archivos
Services:    2 archivos (INominaCalculatorService, IPadronService)
Controller:  1 archivo (EmpleadosController con 15+ endpoints)
--------------------------------------------------------
TOTAL:       67 archivos (~4,100 líneas de código)
```

---

## 🔄 FLUJO DE EJECUCIÓN SECUENCIAL

### Paso 1: Análisis (OBLIGATORIO)

```
┌─────────────────────────────────────────────────┐
│  SUB-LOTE 4.1: ANÁLISIS Y CONTEXTO              │
│                                                 │
│  ✅ Leer EmpleadosService.cs (32 métodos)      │
│  ✅ Leer fichaEmpleado.aspx.cs (armarNovedad)  │
│  ✅ Leer colaboradores.aspx.cs (WebMethods)    │
│  ✅ Documentar en CHECKPOINT_4.1_ANALISIS.md   │
│                                                 │
│  ⏱️  Tiempo: 30-45 minutos                     │
│  📝 Output: 1 documento (~400 líneas)          │
│  🔨 Compilación: NO (no genera código)         │
└─────────────────────────────────────────────────┘
          │
          ↓
┌─────────────────────────────────────────────────┐
│  CHECKPOINT_4.1_ANALISIS.md                     │
│  ✅ 32 métodos Legacy documentados              │
│  ✅ Mapeo a Commands/Queries completado         │
│  ✅ Decisiones técnicas tomadas                 │
└─────────────────────────────────────────────────┘
```

### Paso 2: CRUD Básico

```
┌─────────────────────────────────────────────────┐
│  SUB-LOTE 4.2: CRUD BÁSICO EMPLEADOS            │
│                                                 │
│  📖 Leer: CHECKPOINT_4.1_ANALISIS.md            │
│  🔨 Crear:                                      │
│     ├── CreateEmpleadoCommand (3 archivos)     │
│     ├── UpdateEmpleadoCommand (3 archivos)     │
│     ├── DesactivarEmpleadoCommand (3 archivos) │
│     ├── GetEmpleadoByIdQuery (2 archivos)      │
│     ├── GetEmpleadosByEmpleadorQuery (2 archivos)│
│     └── EmpleadoDetalleDto (1 archivo)         │
│                                                 │
│  ⏱️  Tiempo: 2-3 horas                         │
│  📝 Output: 12 archivos (~900 líneas)          │
│  🔨 Compilación: dotnet build (0 errores)      │
└─────────────────────────────────────────────────┘
          │
          ↓
┌─────────────────────────────────────────────────┐
│  CHECKPOINT_4.2_CRUD_EMPLEADOS.md               │
│  ✅ 12 archivos creados y compilados            │
│  ✅ Contexto actualizado para SUB-LOTE 4.3      │
└─────────────────────────────────────────────────┘
```

### Paso 3-6: Continuar Secuencialmente

```
SUB-LOTE 4.3 → CHECKPOINT_4.3_REMUNERACIONES.md
       ↓
SUB-LOTE 4.4 → CHECKPOINT_4.4_NOMINA.md
       ↓
SUB-LOTE 4.5 → CHECKPOINT_4.5_TEMPORALES.md
       ↓
SUB-LOTE 4.6 → CHECKPOINT_4.6_CONTROLLER.md
       ↓
   LOTE_4_EMPLEADOS_NOMINA_COMPLETADO.md (Reporte Final)
```

---

## 🎯 COMANDOS Y QUERIES IMPLEMENTADOS

### SUB-LOTE 4.2: CRUD Básico

**Commands (3):**
```
✅ CreateEmpleadoCommand
   - Legacy: EmpleadosService.guardarEmpleado()
   - Factory: Empleado.Create()
   - Validations: 15+ reglas

✅ UpdateEmpleadoCommand
   - Legacy: EmpleadosService.actualizarEmpleado()
   - Domain: 5 métodos (ActualizarInformacionPersonal, etc.)
   - Partial update: Solo campos no nulos

✅ DesactivarEmpleadoCommand
   - Legacy: EmpleadosService.darDeBaja()
   - Domain: Empleado.Desactivar()
   - Soft delete: Activo=false + motivo + prestaciones
```

**Queries (2):**
```
✅ GetEmpleadoByIdQuery
   - Legacy: EmpleadosService.getEmpleadosByID()
   - Include opcional: RecibosHeader
   - DTO: EmpleadoDetalleDto (30+ campos)

✅ GetEmpleadosByEmpleadorQuery
   - Legacy: EmpleadosService.getEmpleados() + getVEmpleados()
   - Filtros: searchTerm, soloActivos
   - Paginación: PageIndex, PageSize
   - DTO: EmpleadoListDto (resumen)
```

### SUB-LOTE 4.3: Remuneraciones Extras

**Commands (2):**
```
AddRemuneracionCommand (3 slots: Extra1, Extra2, Extra3)
RemoveRemuneracionCommand (eliminar por número)
```

**Queries (1):**
```
GetRemuneracionesByEmpleadoQuery
```

### SUB-LOTE 4.4: Procesamiento de Nómina (⚠️ ALTA COMPLEJIDAD)

**Commands (2):**
```
ProcesarPagoCommand
   - Legacy: EmpleadosService.procesarPago() + armarNovedad()
   - Service: INominaCalculatorService (cálculos complejos)
   - 2 DbContext separados (mantener patrón Legacy)
   - Cálculos: Salario, extras, deducciones TSS, fracción

AnularReciboCommand
   - Soft delete en ReciboHeader
   - Estado = 3 (Anulado)
```

**Queries (2):**
```
GetReciboByIdQuery (con detalles)
GetRecibosByEmpleadoQuery (historial paginado)
```

**Services (1):**
```
INominaCalculatorService
   - CalcularNomina() → NominaCalculoResult
   - Lógica: dividendo, fracción, TSS, extras
   - Replica armarNovedad() de Legacy
```

### SUB-LOTE 4.5: Empleados Temporales

**Commands (3):**
```
CreateEmpleadoTemporalCommand (persona física o jurídica)
UpdateEmpleadoTemporalCommand
DeleteEmpleadoTemporalCommand (hard delete)
```

**Queries (2):**
```
GetEmpleadoTemporalByIdQuery
GetEmpleadosTemporalesByEmpleadorQuery (con filtros)
```

### SUB-LOTE 4.6: API Padrón + Controller

**Queries (1):**
```
ConsultarPadronQuery
   - API Externa: https://abcportal.online/Sigeinfo/public/api/
   - Autenticación con Bearer token
   - Validar cédula dominicana (11 dígitos)
```

**Controller (1):**
```
EmpleadosController
   - 15+ endpoints REST (CRUD + Nómina + Temporales)
   - Swagger documentation completa
   - Rate limiting en endpoints sensibles
```

---

## 📋 ENDPOINTS REST API (EmpleadosController)

### Empleados Permanentes

```
POST   /api/empleados                           → CreateEmpleadoCommand
GET    /api/empleados/{empleadoId}              → GetEmpleadoByIdQuery
GET    /api/empleados/by-empleador/{userId}    → GetEmpleadosByEmpleadorQuery
PUT    /api/empleados/{empleadoId}              → UpdateEmpleadoCommand
DELETE /api/empleados/{empleadoId}              → DesactivarEmpleadoCommand
```

### Remuneraciones Extras

```
POST   /api/empleados/{empleadoId}/remuneraciones       → AddRemuneracionCommand
GET    /api/empleados/{empleadoId}/remuneraciones       → GetRemuneracionesByEmpleadoQuery
DELETE /api/empleados/{empleadoId}/remuneraciones/{num} → RemoveRemuneracionCommand
```

### Nómina

```
POST   /api/empleados/{empleadoId}/nomina/procesar     → ProcesarPagoCommand
GET    /api/empleados/{empleadoId}/nomina/recibos      → GetRecibosByEmpleadoQuery
GET    /api/empleados/nomina/recibos/{reciboId}        → GetReciboByIdQuery
DELETE /api/empleados/nomina/recibos/{reciboId}/anular → AnularReciboCommand
```

### Empleados Temporales

```
POST   /api/empleados/temporales                    → CreateEmpleadoTemporalCommand
GET    /api/empleados/temporales/{contratacionId}   → GetEmpleadoTemporalByIdQuery
GET    /api/empleados/temporales/by-empleador/{userId} → GetEmpleadosTemporalesByEmpleadorQuery
PUT    /api/empleados/temporales/{contratacionId}   → UpdateEmpleadoTemporalCommand
DELETE /api/empleados/temporales/{contratacionId}   → DeleteEmpleadoTemporalCommand
```

### Utilidades

```
GET    /api/empleados/padron/{cedula}  → ConsultarPadronQuery
```

---

## ⚠️ REGLAS CRÍTICAS DE EJECUCIÓN

### Antes de Cada Sub-Lote

```markdown
✅ Leer CHECKPOINT del sub-lote anterior
✅ Verificar que compilación anterior fue exitosa
✅ Revisar Context actualizado
```

### Durante Implementación

```markdown
✅ Leer método Legacy ANTES de escribir Handler
✅ Copiar lógica EXACTA (no inventar)
✅ Mantener códigos de retorno Legacy
✅ Usar métodos de dominio cuando existan
```

### Después de Cada Sub-Lote

```markdown
✅ Ejecutar dotnet build (0 errores obligatorio)
✅ Crear CHECKPOINT_4.X.md con reporte
✅ Actualizar contexto para siguiente sub-lote
✅ Commit a Git
```

---

## 🚨 PUNTOS CRÍTICOS IDENTIFICADOS

### 1. armarNovedad() - Cálculo de Nómina

**Complejidad:** 🔴 ALTA (150+ líneas de lógica)  
**Problema:** Mezcla UI con cálculo  
**Solución:** Extraer a `INominaCalculatorService`

**Lógica Crítica:**
```
- Dividendo según período: Semanal=4, Quincenal=2, Mensual=1
- Fracción de salario: (salario / 23.83) * diasTrabajados
- Deducciones TSS: salario * (porcentaje / 100) * -1
- Remuneraciones extras: mismo dividendo que salario
```

### 2. procesarPago() - 2 DbContext Separados

**Complejidad:** 🟡 MEDIA  
**Problema:** Patrón no estándar (2 SaveChanges)  
**Decisión:** MANTENER patrón Legacy (funciona hace años)

**Razón:**
```
1. Guardar header primero → genera pagoID (auto-increment)
2. Usar pagoID para asignar a detalles
3. Guardar detalles en segundo DbContext
```

### 3. consultarPadron() - API Externa

**Complejidad:** 🟡 MEDIA  
**Problema:** Autenticación + manejo de errores  
**Solución:** Crear `IPadronService` en Infrastructure

**Pasos:**
```
1. POST /api/login → Obtener Bearer token
2. GET /api/individuo/{cedula} → Datos del ciudadano
3. Manejo de errores: Timeout, 404, 500
```

---

## 📊 PROGRESO ESTIMADO

```
Fase 1: Análisis          [████████████████████] 100% (45 mins)
Fase 2: CRUD Básico       [                    ]   0% (2-3 horas)
Fase 3: Remuneraciones    [                    ]   0% (1-2 horas)
Fase 4: Nómina            [                    ]   0% (3-4 horas)
Fase 5: Temporales        [                    ]   0% (2-3 horas)
Fase 6: API + Controller  [                    ]   0% (1-2 horas)
─────────────────────────────────────────────────────────────────
Total:                    [███                 ]  15% (12-15 horas)
```

---

## ✅ CHECKLIST DE VALIDACIÓN FINAL

### Por Sub-Lote

- [ ] Código Legacy leído y documentado
- [ ] Commands/Queries implementados con lógica exacta
- [ ] Validators creados con FluentValidation
- [ ] DTOs creados con campos calculados
- [ ] `dotnet build` exitoso (0 errores)
- [ ] CHECKPOINT creado con métricas
- [ ] Contexto actualizado para siguiente sub-lote
- [ ] Commit a Git

### LOTE 4 Completo

- [ ] 6 sub-lotes completados (4.1 a 4.6)
- [ ] 56 archivos creados (~4,100 líneas)
- [ ] EmpleadosController con 15+ endpoints
- [ ] Swagger UI probado manualmente
- [ ] Comparación Legacy vs Clean documentada
- [ ] LOTE_4_EMPLEADOS_NOMINA_COMPLETADO.md creado

---

## 🎯 SIGUIENTE PASO

**Acción:** Ejecutar SUB-LOTE 4.1 (Análisis y Contexto)

**Comando:**
```markdown
Leer prompts/LOTE_4_EMPLEADOS_NOMINA_PROMPTS_SECUENCIALES.md
Ir a sección: "SUB-LOTE 4.1: ANÁLISIS Y CONTEXTO"
Seguir instrucciones paso a paso
Crear: CHECKPOINT_4.1_ANALISIS.md
```

**Duración:** 30-45 minutos  
**Output:** 1 documento de análisis (sin código)

---

**Documento Generado por:** GitHub Copilot  
**Fecha:** 13 de octubre, 2025  
**Versión:** 1.0  
**Estado:** ✅ LISTO PARA EJECUCIÓN
