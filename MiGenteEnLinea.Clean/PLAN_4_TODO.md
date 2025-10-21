# ✅ TODO LIST - PLAN 4: REPOSITORY PATTERN

**Última actualización:** 16 de Octubre de 2025  
**Progreso Global:** 0% (0/9 LOTES completados)

---

## 📊 RESUMEN DE PROGRESO

```
PLAN 4: REPOSITORY PATTERN IMPLEMENTATION
╔══════════════════════════════════════════════════════════════╗
║  Progreso: [░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░] 0%              ║
║  Tiempo transcurrido: 0h / 18-25h estimadas                  ║
║  LOTES completados: 0 / 9                                    ║
╚══════════════════════════════════════════════════════════════╝
```

---

## 🎯 LOTE 0: FOUNDATION (GENÉRICO - BASE) ⏳

**⏱️ Tiempo Estimado:** 2-3 horas  
**📁 Archivos:** 7 archivos  
**🎯 Objetivo:** Crear infraestructura genérica de repositorios  
**Estado:** ⏳ PENDIENTE

### Tareas

#### Fase 1: Interfaces en Domain (30 min)

- [ ] Crear `Domain/Interfaces/Repositories/IRepository.cs`
  - [ ] Métodos: GetByIdAsync, GetAllAsync, FindAsync, FirstOrDefaultAsync
  - [ ] Métodos: AddAsync, AddRangeAsync
  - [ ] Métodos: Update, UpdateRange
  - [ ] Métodos: Remove, RemoveRange
  - [ ] Métodos: GetBySpecificationAsync, CountAsync, AnyAsync
- [ ] Crear `Domain/Interfaces/Repositories/IUnitOfWork.cs`
  - [ ] Métodos: SaveChangesAsync
  - [ ] Métodos: BeginTransactionAsync, CommitTransactionAsync, RollbackTransactionAsync
  - [ ] Propiedades: Credenciales, Empleadores, Contratistas, etc.
- [ ] Crear `Domain/Interfaces/Repositories/ISpecification.cs`
  - [ ] Propiedades: Criteria, Includes, OrderBy, OrderByDescending
  - [ ] Propiedades: Take, Skip, IsPagingEnabled

#### Fase 2: Implementaciones en Infrastructure (1.5 horas)

- [ ] Crear `Infrastructure/Persistence/Repositories/Repository.cs`
  - [ ] Implementar todos los métodos de IRepository<T>
  - [ ] Helper: ApplySpecification()
- [ ] Crear `Infrastructure/Persistence/Repositories/UnitOfWork.cs`
  - [ ] Implementar propiedades lazy de repositorios
  - [ ] Implementar SaveChangesAsync
  - [ ] Implementar manejo de transacciones
  - [ ] Implementar IDisposable
- [ ] Crear `Infrastructure/Persistence/Repositories/Specifications/Specification.cs`
  - [ ] Clase base abstracta
  - [ ] Métodos: AddInclude, ApplyOrderBy, ApplyPaging
- [ ] Crear `Infrastructure/Persistence/Repositories/Specifications/SpecificationEvaluator.cs`
  - [ ] Método: GetQuery (aplica spec a IQueryable)

#### Fase 3: Dependency Injection (15 min)

- [ ] Descomentar en `DependencyInjection.cs`:

  ```csharp
  services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
  services.AddScoped<IUnitOfWork, UnitOfWork>();
  ```

#### Fase 4: Testing (30 min)

- [ ] Crear `tests/Infrastructure.Tests/Repositories/RepositoryTests.cs`
  - [ ] Test: AddAsync_EntityValida_DebeAgregar
  - [ ] Test: GetByIdAsync_IdExiste_DebeRetornar
  - [ ] Test: FindAsync_PredicadoValido_DebeRetornarCoincidencias
- [ ] Crear `tests/Infrastructure.Tests/Repositories/UnitOfWorkTests.cs`
  - [ ] Test: SaveChangesAsync_DebeGuardarCambios
  - [ ] Test: BeginTransaction_Commit_DebeConfirmarCambios
  - [ ] Test: BeginTransaction_Rollback_DebeRevertirCambios

#### Fase 5: Documentación (10 min)

- [ ] Crear `LOTE_0_FOUNDATION_COMPLETADO.md`
  - [ ] Resumen de archivos creados
  - [ ] Lecciones aprendidas
  - [ ] Próximos pasos

---

## 🔐 LOTE 1: AUTHENTICATION ⏳

**⏱️ Tiempo Estimado:** 1-2 horas  
**📁 Archivos:** 2 archivos  
**🎯 Objetivo:** Repositorio para Credencial  
**Estado:** ⏳ PENDIENTE (bloqueado por LOTE 0)

### Tareas

#### Fase 1: Interface en Domain (15 min)

- [ ] Crear `Domain/Interfaces/Repositories/Authentication/ICredencialRepository.cs`
  - [ ] Hereda de IRepository<Credencial>
  - [ ] Método: GetByEmailAsync
  - [ ] Método: GetByUserIdAsync
  - [ ] Método: ExistsByEmailAsync
  - [ ] Método: IsActivoAsync
  - [ ] Método: GetCredencialesInactivasAsync
  - [ ] Método: GetCredencialesBloqueadasAsync

#### Fase 2: Implementación en Infrastructure (30 min)

- [ ] Crear `Infrastructure/Persistence/Repositories/Authentication/CredencialRepository.cs`
  - [ ] Hereda de Repository<Credencial>
  - [ ] Implementar GetByEmailAsync (case-insensitive)
  - [ ] Implementar GetByUserIdAsync
  - [ ] Implementar ExistsByEmailAsync
  - [ ] Implementar IsActivoAsync
  - [ ] Implementar GetCredencialesInactivasAsync (Activo = false)
  - [ ] Implementar GetCredencialesBloqueadasAsync (FechaBloqueo reciente)

#### Fase 3: Dependency Injection (5 min)

- [ ] Descomentar en `DependencyInjection.cs`:

  ```csharp
  services.AddScoped<ICredencialRepository, CredencialRepository>();
  ```

#### Fase 4: Refactorizar Commands/Queries (30 min)

- [ ] Actualizar `LoginCommand.cs`
  - [ ] Cambiar IApplicationDbContext → ICredencialRepository
  - [ ] Usar GetByEmailAsync en lugar de FirstOrDefaultAsync
- [ ] Actualizar `RegisterCommand.cs`
  - [ ] Cambiar a usar ICredencialRepository
  - [ ] Usar ExistsByEmailAsync para validar duplicados
- [ ] Actualizar `ChangePasswordCommand.cs`
  - [ ] Cambiar a usar ICredencialRepository
- [ ] Actualizar `GetPerfilQuery.cs`
  - [ ] Cambiar a usar ICredencialRepository

#### Fase 5: Testing (20 min)

- [ ] Crear `tests/Infrastructure.Tests/Repositories/CredencialRepositoryTests.cs`
  - [ ] Test: GetByEmailAsync_EmailExiste_DebeRetornar
  - [ ] Test: GetByEmailAsync_EmailNoExiste_DebeRetornarNull
  - [ ] Test: ExistsByEmailAsync_EmailExiste_DebeRetornarTrue
  - [ ] Test: GetCredencialesInactivasAsync_DebeRetornarSoloInactivas

#### Fase 6: Validación (10 min)

- [ ] Compilar proyecto: `dotnet build`
- [ ] Ejecutar tests: `dotnet test`
- [ ] Verificar 0 errores

#### Fase 7: Documentación (5 min)

- [ ] Crear `LOTE_1_AUTHENTICATION_REPOSITORIES_COMPLETADO.md`

---

## 👔 LOTE 2: EMPLEADORES ⏳

**⏱️ Tiempo Estimado:** 2-3 horas  
**📁 Archivos:** 4 archivos  
**🎯 Objetivo:** Repositorios para Empleador y Recibos  
**Estado:** ⏳ PENDIENTE (bloqueado por LOTE 0)

### Tareas

#### Fase 1: Interfaces en Domain (30 min)

- [ ] Crear `Domain/Interfaces/Repositories/Empleadores/IEmpleadorRepository.cs`
  - [ ] Métodos: GetByUserIdAsync, GetByRNCAsync, ExistsByRNCAsync
  - [ ] Métodos: GetWithRecibosAsync, GetWithSuscripcionAsync
  - [ ] Métodos: GetByPlanAsync, GetActivosAsync, GetConPlanVencidoAsync
  - [ ] Método: SearchAsync (paginado con filtros)
- [ ] Crear `Domain/Interfaces/Repositories/Empleadores/IEmpleadorReciboHeaderRepository.cs`
  - [ ] Métodos: GetByEmpleadorIdAsync, GetByRangoFechasAsync, GetWithDetallesAsync
- [ ] Crear `Domain/Interfaces/Repositories/Empleadores/IEmpleadorReciboDetalleRepository.cs`
- [ ] Crear `Domain/Interfaces/Repositories/Empleadores/IEmpleadorReciboDetalleContratacionesRepository.cs`

#### Fase 2: Implementaciones en Infrastructure (1 hora)

- [ ] Crear `Infrastructure/Persistence/Repositories/Empleadores/EmpleadorRepository.cs`
  - [ ] Implementar todos los métodos de IEmpleadorRepository
  - [ ] SearchAsync con paginación y filtros múltiples
- [ ] Crear `Infrastructure/Persistence/Repositories/Empleadores/EmpleadorReciboHeaderRepository.cs`
- [ ] Crear `Infrastructure/Persistence/Repositories/Empleadores/EmpleadorReciboDetalleRepository.cs`
- [ ] Crear `Infrastructure/Persistence/Repositories/Empleadores/EmpleadorReciboDetalleContratacionesRepository.cs`

#### Fase 3: Dependency Injection (5 min)

- [ ] Registrar 4 repositorios en `DependencyInjection.cs`

#### Fase 4: Refactorizar Commands/Queries (1 hora)

- [ ] Actualizar ~15 handlers de Empleadores

#### Fase 5: Testing (30 min)

- [ ] Tests para EmpleadorRepository

#### Fase 6: Documentación (10 min)

- [ ] Crear `LOTE_2_EMPLEADORES_REPOSITORIES_COMPLETADO.md`

---

## 👷 LOTE 3: CONTRATISTAS ⏳

**⏱️ Tiempo Estimado:** 2-3 horas  
**📁 Archivos:** 6 archivos  
**🎯 Objetivo:** Repositorios para Contratista, Foto, Servicio  
**Estado:** ⏳ PENDIENTE (bloqueado por LOTE 0)

### Tareas

- [ ] **Fase 1:** Crear 3 interfaces en Domain (30 min)
- [ ] **Fase 2:** Crear 3 implementaciones en Infrastructure (1 hora)
- [ ] **Fase 3:** Registrar en DI (5 min)
- [ ] **Fase 4:** Refactorizar ~12 handlers (1 hora)
- [ ] **Fase 5:** Testing (30 min)
- [ ] **Fase 6:** Documentación (10 min)

---

## 👨‍💼 LOTE 4: EMPLEADOS & NÓMINA ⏳

**⏱️ Tiempo Estimado:** 4-5 horas ⚠️ **MÁS COMPLEJO**  
**📁 Archivos:** 12 archivos  
**🎯 Objetivo:** Repositorios para 10+ entidades de Empleados y Nómina  
**Estado:** ⏳ PENDIENTE (bloqueado por LOTE 0)

### Tareas

- [ ] **Fase 1:** Crear 6 interfaces principales en Domain (1 hora)
  - [ ] IEmpleadoRepository
  - [ ] IEmpleadoDependienteRepository
  - [ ] IEmpleadoRemuneracionRepository
  - [ ] IReciboHeaderRepository
  - [ ] IReciboDetalleRepository
  - [ ] IDeduccionesTSSRepository
- [ ] **Fase 2:** Crear 6 implementaciones en Infrastructure (2 horas)
- [ ] **Fase 3:** Registrar en DI (5 min)
- [ ] **Fase 4:** Refactorizar ~25 handlers (1.5 horas)
- [ ] **Fase 5:** Testing (45 min)
- [ ] **Fase 6:** Documentación (15 min)

---

## 💳 LOTE 5: SUSCRIPCIONES & PAGOS ⏳

**⏱️ Tiempo Estimado:** 2-3 horas  
**📁 Archivos:** 8 archivos  
**🎯 Objetivo:** Repositorios para Plan, Suscripcion, Cuenta, Transaccion  
**Estado:** ⏳ PENDIENTE (bloqueado por LOTE 0)

### Tareas

- [ ] **Fase 1:** Crear 4 interfaces en Domain (30 min)
- [ ] **Fase 2:** Crear 4 implementaciones en Infrastructure (1 hora)
- [ ] **Fase 3:** Registrar en DI (5 min)
- [ ] **Fase 4:** Refactorizar ~10 handlers (1 hora)
- [ ] **Fase 5:** Testing (30 min)
- [ ] **Fase 6:** Documentación (10 min)

---

## ⭐ LOTE 6: CALIFICACIONES ⏳

**⏱️ Tiempo Estimado:** 1 hora ✨ **MÁS SIMPLE**  
**📁 Archivos:** 2 archivos  
**🎯 Objetivo:** Repositorio para Calificacion  
**Estado:** ⏳ PENDIENTE (bloqueado por LOTE 0)

### Tareas

- [ ] **Fase 1:** Crear ICalificacionRepository (10 min)
- [ ] **Fase 2:** Crear CalificacionRepository (20 min)
- [ ] **Fase 3:** Registrar en DI (5 min)
- [ ] **Fase 4:** Refactorizar 6 handlers (15 min)
- [ ] **Fase 5:** Testing (10 min)
- [ ] **Fase 6:** Documentación (5 min)

---

## 📚 LOTE 7: CATÁLOGOS ⏳

**⏱️ Tiempo Estimado:** 2-3 horas  
**📁 Archivos:** 16 archivos  
**🎯 Objetivo:** Repositorios para 15+ entidades catálogo  
**Estado:** ⏳ PENDIENTE (bloqueado por LOTE 0)

### Tareas

- [ ] **Fase 1:** Crear 8 interfaces en Domain (30 min)
  - [ ] IBancoRepository
  - [ ] ICategoriaServicioRepository
  - [ ] IDeduccionRepository
  - [ ] IDepartamentoRepository
  - [ ] IMunicipioRepository
  - [ ] ISectorRepository
  - [ ] ITipoContratoRepository
  - [ ] ITipoDocumentoRepository
- [ ] **Fase 2:** Crear 8 implementaciones (1 hora)
- [ ] **Fase 3:** Registrar en DI (10 min)
- [ ] **Fase 4:** Refactorizar ~8 handlers (45 min)
- [ ] **Fase 5:** Testing (30 min)
- [ ] **Fase 6:** Documentación (10 min)

---

## 🔒 LOTE 8: CONTRATACIONES & SEGURIDAD ⏳

**⏱️ Tiempo Estimado:** 2-3 horas  
**📁 Archivos:** 8 archivos  
**🎯 Objetivo:** Repositorios para Contratacion, Permiso, Rol  
**Estado:** ⏳ PENDIENTE (bloqueado por LOTE 0)

### Tareas

- [ ] **Fase 1:** Crear 4 interfaces en Domain (30 min)
  - [ ] IContratacionRepository
  - [ ] IDetalleContratacionRepository
  - [ ] IPermisoRepository
  - [ ] IRolRepository
- [ ] **Fase 2:** Crear 4 implementaciones (1 hora)
- [ ] **Fase 3:** Registrar en DI (5 min)
- [ ] **Fase 4:** Refactorizar ~10 handlers (1 hora)
- [ ] **Fase 5:** Testing (30 min)
- [ ] **Fase 6:** Documentación (10 min)

---

## 📊 MÉTRICAS FINALES

### Al Completar TODOS los LOTES

- [ ] **65+ archivos de repositorios creados**
- [ ] **80+ Commands/Queries refactorizados**
- [ ] **0 uso directo de IApplicationDbContext**
- [ ] **Cobertura de tests >= 80%**
- [ ] **0 errores de compilación**
- [ ] **API ejecutándose correctamente**
- [ ] **9 documentos LOTE_X_COMPLETADO.md creados**

---

## 🚀 PRÓXIMO PASO INMEDIATO

### ⚡ INICIAR LOTE 0 (FOUNDATION)

**Comando para empezar:**

```bash
cd c:\Users\rpena\OneDrive - Dextra\Desktop\MIGENTEENELINE\MiGenteEnlinea\MiGenteEnLinea.Clean

# Crear estructura de carpetas
mkdir -p src/Core/MiGenteEnLinea.Domain/Interfaces/Repositories
mkdir -p src/Infrastructure/MiGenteEnLinea.Infrastructure/Persistence/Repositories/Specifications
mkdir -p src/Infrastructure/MiGenteEnLinea.Infrastructure/Persistence/Repositories/Authentication

# Crear branch
git checkout -b feature/repository-pattern-lote-0-foundation

# Abrir PLAN_4_REPOSITORY_PATTERN_IMPLEMENTATION.md
# Copiar código de LOTE 0 y comenzar implementación
```

---

## 📚 DOCUMENTOS DE REFERENCIA

| Documento | Propósito |
|-----------|-----------|
| `PLAN_4_REPOSITORY_PATTERN_IMPLEMENTATION.md` | 📘 Plan maestro con código completo |
| `PLAN_4_RESUMEN_EJECUTIVO.md` | 📊 Resumen ejecutivo con métricas |
| `PLAN_4_TODO.md` | ✅ Este documento (checklist detallado) |

---

**Última actualización:** 16 de Octubre de 2025  
**Progreso:** 0% (0/9 LOTES)  
**Estado:** 🚀 LISTO PARA INICIAR LOTE 0
