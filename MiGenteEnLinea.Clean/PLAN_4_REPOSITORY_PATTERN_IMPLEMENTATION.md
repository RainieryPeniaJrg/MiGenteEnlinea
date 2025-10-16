# 📋 PLAN 4: REPOSITORY PATTERN IMPLEMENTATION

**Fecha Creación:** 16 de Octubre de 2025  
**Estado:** 🔄 Pendiente de Ejecución  
**Prerequisitos:** PLAN 3 (JWT Authentication) ✅ Completado  
**Tiempo Estimado:** 18-25 horas (2.5-3 días de trabajo)

---

## 📊 RESUMEN EJECUTIVO

### 🎯 Objetivo

Implementar el **Repository Pattern** completo en MiGente Clean Architecture para:

1. **Abstracción de persistencia:** Desacoplar Application layer de EF Core
2. **Testabilidad:** Facilitar mocking en unit tests
3. **Mantenibilidad:** Centralizar queries complejas
4. **Clean Architecture:** Respetar Dependency Inversion Principle
5. **DDD:** Reforzar patrones de Domain-Driven Design

### 📈 Estado Actual vs Deseado

#### ❌ ESTADO ACTUAL (Problema)

```csharp
// ❌ Application Layer acoplado a EF Core
public class CreateEmpleadorCommandHandler
{
    private readonly IApplicationDbContext _context; // DbContext directo
    
    public async Task<int> Handle(CreateEmpleadorCommand request, CancellationToken ct)
    {
        var empleador = Empleador.Crear(request.Nombre, request.RNC);
        
        _context.Empleadores.Add(empleador); // ❌ Uso directo de DbSet
        await _context.SaveChangesAsync(ct); // ❌ SaveChanges en handler
        
        return empleador.Id;
    }
}
```

**Problemas:**
- ❌ Tight coupling a EF Core (difícil cambiar ORM)
- ❌ Difícil testing (requiere DbContext real o InMemory)
- ❌ Queries repetidas en múltiples handlers
- ❌ Violación de Single Responsibility (handlers saben de persistencia)
- ❌ No hay transacciones explícitas

#### ✅ ESTADO DESEADO (Solución)

```csharp
// ✅ Application Layer desacoplado con Repository
public class CreateEmpleadorCommandHandler
{
    private readonly IEmpleadorRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public async Task<int> Handle(CreateEmpleadorCommand request, CancellationToken ct)
    {
        var empleador = Empleador.Crear(request.Nombre, request.RNC);
        
        await _repository.AddAsync(empleador); // ✅ Abstracción
        await _unitOfWork.SaveChangesAsync(ct); // ✅ Transacción explícita
        
        return empleador.Id;
    }
}

// Query compleja encapsulada en repositorio
var empleadores = await _repository.GetByPlanAsync(planId, includeRecibos: true);
```

**Beneficios:**
- ✅ Testable (fácil mocking de IEmpleadorRepository)
- ✅ Queries complejas encapsuladas
- ✅ Transacciones explícitas con UnitOfWork
- ✅ Fácil cambiar ORM (implementar nueva clase)
- ✅ Single Responsibility (handlers solo lógica de aplicación)

---

## 🏗️ ARQUITECTURA DEL REPOSITORY PATTERN

### 📐 Estructura de 3 Capas

```
┌─────────────────────────────────────────────────────────────┐
│  DOMAIN LAYER (Interfaces)                                  │
│  - IRepository<T>           (genérico - CRUD básico)        │
│  - ISpecification<T>        (queries complejas reutilizables)│
│  - IUnitOfWork              (transacciones)                 │
│  - ICredencialRepository    (específico - queries de negocio)│
│  - IEmpleadorRepository     (específico - queries de negocio)│
│  - ...                                                       │
└─────────────────────────────────────────────────────────────┘
                            ↓ implements
┌─────────────────────────────────────────────────────────────┐
│  INFRASTRUCTURE LAYER (Implementaciones)                     │
│  - Repository<T>            (implementación genérica EF Core)│
│  - Specification<T>         (implementación con LINQ)        │
│  - UnitOfWork               (DbContext.SaveChangesAsync)     │
│  - CredencialRepository     (hereda Repository<Credencial>)  │
│  - EmpleadorRepository      (hereda Repository<Empleador>)   │
│  - ...                                                       │
└─────────────────────────────────────────────────────────────┘
                            ↓ injected into
┌─────────────────────────────────────────────────────────────┐
│  APPLICATION LAYER (Commands/Queries)                        │
│  - LoginCommandHandler      (usa ICredencialRepository)      │
│  - CreateEmpleadorHandler   (usa IEmpleadorRepository)       │
│  - GetEmpleadosQueryHandler (usa IEmpleadoRepository)        │
│  - ...                                                       │
└─────────────────────────────────────────────────────────────┘
```

### 🔧 Componentes Principales

#### 1. IRepository<T> (Genérico)

**Ubicación:** `Domain/Interfaces/Repositories/IRepository.cs`

```csharp
namespace MiGenteEnLinea.Domain.Interfaces.Repositories;

/// <summary>
/// Interfaz genérica para operaciones CRUD básicas
/// </summary>
public interface IRepository<T> where T : class
{
    // READ operations
    Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    
    // CREATE operations
    Task<T> AddAsync(T entity, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);
    
    // UPDATE operations
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    
    // DELETE operations
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    
    // QUERY operations with Specification Pattern
    Task<IEnumerable<T>> GetBySpecificationAsync(ISpecification<T> spec, CancellationToken ct = default);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
}
```

#### 2. IUnitOfWork (Transacciones)

**Ubicación:** `Domain/Interfaces/Repositories/IUnitOfWork.cs`

```csharp
namespace MiGenteEnLinea.Domain.Interfaces.Repositories;

/// <summary>
/// Patrón Unit of Work para manejo de transacciones
/// </summary>
public interface IUnitOfWork : IDisposable
{
    // Transacciones
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitTransactionAsync(CancellationToken ct = default);
    Task RollbackTransactionAsync(CancellationToken ct = default);
    
    // Repositories (acceso a repositorios específicos)
    ICredencialRepository Credenciales { get; }
    IEmpleadorRepository Empleadores { get; }
    IContratistaRepository Contratistas { get; }
    IEmpleadoRepository Empleados { get; }
    // ... más repositorios
}
```

#### 3. ISpecification<T> (Queries Complejas)

**Ubicación:** `Domain/Interfaces/Repositories/ISpecification.cs`

```csharp
namespace MiGenteEnLinea.Domain.Interfaces.Repositories;

/// <summary>
/// Patrón Specification para queries complejas y reutilizables
/// </summary>
public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    Expression<Func<T, object>>? OrderBy { get; }
    Expression<Func<T, object>>? OrderByDescending { get; }
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }
}
```

**Ejemplo de uso:**

```csharp
// Specification para empleadores activos con plan premium
public class EmpleadoresActivosConPlanPremiumSpec : Specification<Empleador>
{
    public EmpleadoresActivosConPlanPremiumSpec() 
        : base(e => e.Estado == EstadoEmpleador.Activo && e.PlanId >= 3)
    {
        AddInclude(e => e.Suscripcion);
        AddOrderByDescending(e => e.FechaRegistro);
    }
}

// Uso en handler
var empleadores = await _repository.GetBySpecificationAsync(
    new EmpleadoresActivosConPlanPremiumSpec()
);
```

#### 4. ICredencialRepository (Específico)

**Ubicación:** `Domain/Interfaces/Repositories/Authentication/ICredencialRepository.cs`

```csharp
namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Authentication;

/// <summary>
/// Repositorio específico para Credenciales con queries de negocio
/// </summary>
public interface ICredencialRepository : IRepository<Credencial>
{
    // Queries específicas del dominio
    Task<Credencial?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<Credencial?> GetByUserIdAsync(string userId, CancellationToken ct = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
    Task<bool> IsActivoAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<Credencial>> GetCredencialesInactivasAsync(CancellationToken ct = default);
    Task<IEnumerable<Credencial>> GetCredencialesBloqueadasAsync(CancellationToken ct = default);
}
```

---

## 📦 PLAN DE IMPLEMENTACIÓN POR LOTES

### 🎯 Estrategia de Implementación

1. **LOTE 0 (Foundation):** Implementar patrones genéricos primero
2. **LOTES 1-8:** Implementar repositorios específicos por dominio
3. **Actualizar Commands/Queries** en cada LOTE para usar repositorios
4. **Testing:** Unit tests + Integration tests por LOTE
5. **Documentación:** Actualizar docs de arquitectura

---

## 🚀 LOTE 0: FOUNDATION (BASE GENÉRICA)

**⏱️ Tiempo Estimado:** 2-3 horas  
**🎯 Objetivo:** Crear infraestructura genérica de repositorios

### 📝 Tareas

#### Paso 1: Crear Interfaces Genéricas en Domain (30 min)

**Archivos a crear:**

1. `Domain/Interfaces/Repositories/IRepository.cs` (código arriba)
2. `Domain/Interfaces/Repositories/IUnitOfWork.cs` (código arriba)
3. `Domain/Interfaces/Repositories/ISpecification.cs` (código arriba)

#### Paso 2: Implementar Clases Genéricas en Infrastructure (1.5 horas)

**Archivos a crear:**

1. **`Infrastructure/Persistence/Repositories/Repository.cs`**

```csharp
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Domain.Interfaces.Repositories;
using System.Linq.Expressions;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implementación genérica del patrón Repository usando EF Core
/// </summary>
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly MiGenteDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(MiGenteDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _dbSet.FindAsync(new object[] { id }, ct);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
    {
        return await _dbSet.ToListAsync(ct);
    }

    public virtual async Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate, 
        CancellationToken ct = default)
    {
        return await _dbSet.Where(predicate).ToListAsync(ct);
    }

    public virtual async Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate, 
        CancellationToken ct = default)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate, ct);
    }

    public virtual async Task<T> AddAsync(T entity, CancellationToken ct = default)
    {
        await _dbSet.AddAsync(entity, ct);
        return entity;
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
    {
        await _dbSet.AddRangeAsync(entities, ct);
    }

    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public virtual void UpdateRange(IEnumerable<T> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public virtual void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public virtual async Task<IEnumerable<T>> GetBySpecificationAsync(
        ISpecification<T> spec, 
        CancellationToken ct = default)
    {
        return await ApplySpecification(spec).ToListAsync(ct);
    }

    public virtual async Task<int> CountAsync(
        Expression<Func<T, bool>>? predicate = null, 
        CancellationToken ct = default)
    {
        if (predicate == null)
            return await _dbSet.CountAsync(ct);

        return await _dbSet.CountAsync(predicate, ct);
    }

    public virtual async Task<bool> AnyAsync(
        Expression<Func<T, bool>> predicate, 
        CancellationToken ct = default)
    {
        return await _dbSet.AnyAsync(predicate, ct);
    }

    // Helper method para aplicar Specification
    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(_dbSet.AsQueryable(), spec);
    }
}
```

2. **`Infrastructure/Persistence/Repositories/UnitOfWork.cs`**

```csharp
using Microsoft.EntityFrameworkCore.Storage;
using MiGenteEnLinea.Domain.Interfaces.Repositories;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Authentication;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Empleadores;
// ... más imports

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implementación del patrón Unit of Work
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly MiGenteDbContext _context;
    private IDbContextTransaction? _currentTransaction;

    // Lazy initialization de repositorios
    private ICredencialRepository? _credenciales;
    private IEmpleadorRepository? _empleadores;
    private IContratistaRepository? _contratistas;
    private IEmpleadoRepository? _empleados;
    // ... más repositorios

    public UnitOfWork(MiGenteDbContext context)
    {
        _context = context;
    }

    // Propiedades de repositorios (lazy loading)
    public ICredencialRepository Credenciales => 
        _credenciales ??= new CredencialRepository(_context);
    
    public IEmpleadorRepository Empleadores => 
        _empleadores ??= new EmpleadorRepository(_context);
    
    public IContratistaRepository Contratistas => 
        _contratistas ??= new ContratistaRepository(_context);
    
    public IEmpleadoRepository Empleados => 
        _empleados ??= new EmpleadoRepository(_context);

    // Transacciones
    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }

    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        if (_currentTransaction != null)
        {
            throw new InvalidOperationException("A transaction is already in progress.");
        }

        _currentTransaction = await _context.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        if (_currentTransaction == null)
        {
            throw new InvalidOperationException("No transaction in progress.");
        }

        try
        {
            await _context.SaveChangesAsync(ct);
            await _currentTransaction.CommitAsync(ct);
        }
        catch
        {
            await RollbackTransactionAsync(ct);
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken ct = default)
    {
        if (_currentTransaction == null)
        {
            throw new InvalidOperationException("No transaction in progress.");
        }

        try
        {
            await _currentTransaction.RollbackAsync(ct);
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void Dispose()
    {
        _currentTransaction?.Dispose();
        _context.Dispose();
    }
}
```

3. **`Infrastructure/Persistence/Repositories/Specifications/Specification.cs`**

```csharp
using System.Linq.Expressions;
using MiGenteEnLinea.Domain.Interfaces.Repositories;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Specifications;

/// <summary>
/// Clase base para implementar el patrón Specification
/// </summary>
public abstract class Specification<T> : ISpecification<T>
{
    protected Specification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    public Expression<Func<T, bool>>? Criteria { get; }
    public List<Expression<Func<T, object>>> Includes { get; } = new();
    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }
    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool IsPagingEnabled { get; private set; }

    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }
}
```

4. **`Infrastructure/Persistence/Repositories/Specifications/SpecificationEvaluator.cs`**

```csharp
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Domain.Interfaces.Repositories;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Specifications;

/// <summary>
/// Evalúa Specifications y las aplica a un IQueryable
/// </summary>
public static class SpecificationEvaluator<T> where T : class
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
    {
        var query = inputQuery;

        // Aplicar criteria (WHERE)
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }

        // Aplicar includes (JOIN)
        query = spec.Includes.Aggregate(query, (current, include) => 
            current.Include(include));

        // Aplicar OrderBy
        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }
        else if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        // Aplicar Paging
        if (spec.IsPagingEnabled)
        {
            query = query.Skip(spec.Skip).Take(spec.Take);
        }

        return query;
    }
}
```

#### Paso 3: Registrar en Dependency Injection (15 min)

**Archivo:** `Infrastructure/DependencyInjection.cs`

```csharp
// Descomentar en línea 103-105
services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
services.AddScoped<IUnitOfWork, UnitOfWork>();
```

#### Paso 4: Testing de Foundation (30 min)

Crear tests básicos para validar que el patrón funciona.

---

## 🔐 LOTE 1: AUTHENTICATION

**⏱️ Tiempo Estimado:** 1-2 horas  
**🎯 Objetivo:** Implementar repositorio para Credencial

### 📝 Entidades

- **Credencial** (tabla: Credenciales)

### 📂 Archivos a Crear

#### 1. Interface en Domain

**`Domain/Interfaces/Repositories/Authentication/ICredencialRepository.cs`**

```csharp
using MiGenteEnLinea.Domain.Entities.Authentication;

namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Authentication;

/// <summary>
/// Repositorio para entidad Credencial con queries específicas de autenticación
/// </summary>
public interface ICredencialRepository : IRepository<Credencial>
{
    /// <summary>
    /// Obtiene credencial por email
    /// </summary>
    Task<Credencial?> GetByEmailAsync(string email, CancellationToken ct = default);
    
    /// <summary>
    /// Obtiene credencial por UserID
    /// </summary>
    Task<Credencial?> GetByUserIdAsync(string userId, CancellationToken ct = default);
    
    /// <summary>
    /// Verifica si existe un email registrado
    /// </summary>
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
    
    /// <summary>
    /// Verifica si credencial está activa
    /// </summary>
    Task<bool> IsActivoAsync(int id, CancellationToken ct = default);
    
    /// <summary>
    /// Obtiene todas las credenciales inactivas (para reenvío de activación)
    /// </summary>
    Task<IEnumerable<Credencial>> GetCredencialesInactivasAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Obtiene credenciales bloqueadas por intentos fallidos
    /// </summary>
    Task<IEnumerable<Credencial>> GetCredencialesBloqueadasAsync(CancellationToken ct = default);
}
```

#### 2. Implementación en Infrastructure

**`Infrastructure/Persistence/Repositories/Authentication/CredencialRepository.cs`**

```csharp
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Domain.Entities.Authentication;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Authentication;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Authentication;

/// <summary>
/// Implementación del repositorio de Credencial
/// </summary>
public class CredencialRepository : Repository<Credencial>, ICredencialRepository
{
    public CredencialRepository(MiGenteDbContext context) : base(context)
    {
    }

    public async Task<Credencial?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower(), ct);
    }

    public async Task<Credencial?> GetByUserIdAsync(string userId, CancellationToken ct = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.UserId == userId, ct);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
    {
        return await _dbSet
            .AnyAsync(c => c.Email.ToLower() == email.ToLower(), ct);
    }

    public async Task<bool> IsActivoAsync(int id, CancellationToken ct = default)
    {
        return await _dbSet
            .Where(c => c.Id == id)
            .Select(c => c.Activo)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IEnumerable<Credencial>> GetCredencialesInactivasAsync(CancellationToken ct = default)
    {
        return await _dbSet
            .Where(c => !c.Activo && c.FechaActivacion == null)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<Credencial>> GetCredencialesBloqueadasAsync(CancellationToken ct = default)
    {
        return await _dbSet
            .Where(c => c.FechaBloqueo != null && c.FechaBloqueo > DateTime.UtcNow.AddHours(-24))
            .OrderByDescending(c => c.FechaBloqueo)
            .ToListAsync(ct);
    }
}
```

#### 3. Actualizar Commands/Queries

**Archivos a modificar:**

1. **LoginCommand.cs** - Cambiar `IApplicationDbContext` por `ICredencialRepository`
2. **RegisterCommand.cs** - Cambiar a usar repositorio
3. **ChangePasswordCommand.cs** - Cambiar a usar repositorio
4. **GetPerfilQuery.cs** - Cambiar a usar repositorio

**Ejemplo: LoginCommandHandler**

```csharp
// ❌ ANTES
public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IApplicationDbContext _context;
    
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken ct)
    {
        var credencial = await _context.Credenciales
            .FirstOrDefaultAsync(c => c.Email == request.Email, ct);
        // ...
    }
}

// ✅ DESPUÉS
public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly ICredencialRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken ct)
    {
        var credencial = await _repository.GetByEmailAsync(request.Email, ct);
        // ...
        await _unitOfWork.SaveChangesAsync(ct); // Explícito
    }
}
```

#### 4. Registrar en DI

**`Infrastructure/DependencyInjection.cs`**

```csharp
// Descomentar línea 108
services.AddScoped<ICredencialRepository, CredencialRepository>();
```

#### 5. Testing

Crear tests para validar queries específicas.

---

## 👔 LOTE 2: EMPLEADORES

**⏱️ Tiempo Estimado:** 2-3 horas  
**🎯 Objetivo:** Repositorios para Empleador y Recibos

### 📝 Entidades (4)

1. **Empleador** (tabla: Empleadores)
2. **Empleador_Recibo_Header** (tabla: Empleador_Recibos_Header)
3. **Empleador_Recibo_Detalle** (tabla: Empleador_Recibos_Detalle)
4. **Empleador_Recibo_Detalle_Contrataciones** (tabla: Empleador_Recibos_Detalle_Contrataciones)

### 📂 Archivos a Crear

#### 1. IEmpleadorRepository (Principal)

**`Domain/Interfaces/Repositories/Empleadores/IEmpleadorRepository.cs`**

```csharp
using MiGenteEnLinea.Domain.Entities.Empleadores;

namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Empleadores;

public interface IEmpleadorRepository : IRepository<Empleador>
{
    // Búsqueda básica
    Task<Empleador?> GetByUserIdAsync(string userId, CancellationToken ct = default);
    Task<Empleador?> GetByRNCAsync(string rnc, CancellationToken ct = default);
    Task<bool> ExistsByRNCAsync(string rnc, CancellationToken ct = default);
    
    // Búsqueda con relaciones
    Task<Empleador?> GetWithRecibosAsync(int id, CancellationToken ct = default);
    Task<Empleador?> GetWithSuscripcionAsync(int id, CancellationToken ct = default);
    
    // Búsqueda por criterios
    Task<IEnumerable<Empleador>> GetByPlanAsync(int planId, CancellationToken ct = default);
    Task<IEnumerable<Empleador>> GetActivosAsync(CancellationToken ct = default);
    Task<IEnumerable<Empleador>> GetConPlanVencidoAsync(CancellationToken ct = default);
    
    // Búsqueda paginada
    Task<(IEnumerable<Empleador> Items, int Total)> SearchAsync(
        string? searchTerm,
        int? planId,
        bool? activo,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default);
}
```

#### 2. IEmpleadorReciboRepository

**`Domain/Interfaces/Repositories/Empleadores/IEmpleadorReciboRepository.cs`**

```csharp
using MiGenteEnLinea.Domain.Entities.Empleadores;

namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Empleadores;

public interface IEmpleadorReciboHeaderRepository : IRepository<EmpleadorReciboHeader>
{
    Task<IEnumerable<EmpleadorReciboHeader>> GetByEmpleadorIdAsync(
        int empleadorId, 
        CancellationToken ct = default);
    
    Task<IEnumerable<EmpleadorReciboHeader>> GetByRangoFechasAsync(
        int empleadorId,
        DateTime fechaInicio,
        DateTime fechaFin,
        CancellationToken ct = default);
    
    Task<EmpleadorReciboHeader?> GetWithDetallesAsync(
        int id, 
        CancellationToken ct = default);
}
```

#### 3. Implementaciones

**`Infrastructure/Persistence/Repositories/Empleadores/EmpleadorRepository.cs`**

```csharp
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Domain.Entities.Empleadores;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Empleadores;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Empleadores;

public class EmpleadorRepository : Repository<Empleador>, IEmpleadorRepository
{
    public EmpleadorRepository(MiGenteDbContext context) : base(context)
    {
    }

    public async Task<Empleador?> GetByUserIdAsync(string userId, CancellationToken ct = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(e => e.UserId == userId, ct);
    }

    public async Task<Empleador?> GetByRNCAsync(string rnc, CancellationToken ct = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(e => e.RNC == rnc, ct);
    }

    public async Task<bool> ExistsByRNCAsync(string rnc, CancellationToken ct = default)
    {
        return await _dbSet
            .AnyAsync(e => e.RNC == rnc, ct);
    }

    public async Task<Empleador?> GetWithRecibosAsync(int id, CancellationToken ct = default)
    {
        // EF Core: Cargar entidad con navigation properties (si las tuviéramos)
        // Como usamos Shadow Properties (DDD puro), hacemos query separada
        return await _dbSet.FindAsync(new object[] { id }, ct);
    }

    public async Task<Empleador?> GetWithSuscripcionAsync(int id, CancellationToken ct = default)
    {
        return await _dbSet.FindAsync(new object[] { id }, ct);
    }

    public async Task<IEnumerable<Empleador>> GetByPlanAsync(int planId, CancellationToken ct = default)
    {
        return await _dbSet
            .Where(e => e.PlanId == planId)
            .OrderByDescending(e => e.FechaRegistro)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<Empleador>> GetActivosAsync(CancellationToken ct = default)
    {
        return await _dbSet
            .Where(e => e.VencimientoPlan > DateTime.UtcNow)
            .OrderBy(e => e.RazonSocial)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<Empleador>> GetConPlanVencidoAsync(CancellationToken ct = default)
    {
        return await _dbSet
            .Where(e => e.VencimientoPlan <= DateTime.UtcNow)
            .OrderBy(e => e.VencimientoPlan)
            .ToListAsync(ct);
    }

    public async Task<(IEnumerable<Empleador> Items, int Total)> SearchAsync(
        string? searchTerm,
        int? planId,
        bool? activo,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default)
    {
        var query = _dbSet.AsQueryable();

        // Filtros
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(e => 
                e.RazonSocial.ToLower().Contains(searchTerm) ||
                e.RNC.Contains(searchTerm) ||
                e.Email.ToLower().Contains(searchTerm));
        }

        if (planId.HasValue)
        {
            query = query.Where(e => e.PlanId == planId.Value);
        }

        if (activo.HasValue)
        {
            if (activo.Value)
            {
                query = query.Where(e => e.VencimientoPlan > DateTime.UtcNow);
            }
            else
            {
                query = query.Where(e => e.VencimientoPlan <= DateTime.UtcNow);
            }
        }

        // Total count
        var total = await query.CountAsync(ct);

        // Paginación
        var items = await query
            .OrderByDescending(e => e.FechaRegistro)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }
}
```

#### 4. Registrar en DI

```csharp
services.AddScoped<IEmpleadorRepository, EmpleadorRepository>();
services.AddScoped<IEmpleadorReciboHeaderRepository, EmpleadorReciboHeaderRepository>();
```

#### 5. Actualizar Commands/Queries

Modificar todos los handlers de Empleadores para usar repositorios.

---

## 👷 LOTE 3: CONTRATISTAS

**⏱️ Tiempo Estimado:** 2-3 horas  
**📝 Entidades:** Contratista, ContratistaFoto, ContratistaServicio

*(Estructura similar a LOTE 2)*

---

## 👨‍💼 LOTE 4: EMPLEADOS & NÓMINA

**⏱️ Tiempo Estimado:** 4-5 horas  
**📝 Entidades:** Empleado, EmpleadoDependiente, EmpleadoRemuneracion, ReciboHeader, ReciboDetalle, etc. (10+ entidades)

*(El más complejo por cantidad de entidades y queries de payroll)*

---

## 💳 LOTE 5: SUSCRIPCIONES & PAGOS

**⏱️ Tiempo Estimado:** 2-3 horas  
**📝 Entidades:** Plan, Suscripcion, Cuenta, Transaccion

---

## ⭐ LOTE 6: CALIFICACIONES

**⏱️ Tiempo Estimado:** 1 hora  
**📝 Entidades:** Calificacion

---

## 📚 LOTE 7: CATÁLOGOS

**⏱️ Tiempo Estimado:** 2-3 horas  
**📝 Entidades:** Banco, CategoriaServicio, Deduccion, Departamento, etc. (15+ entidades catálogo)

---

## 🔒 LOTE 8: CONTRATACIONES & SEGURIDAD

**⏱️ Tiempo Estimado:** 2-3 horas  
**📝 Entidades:** Contratacion, DetalleContratacion, Permiso, Rol, etc.

---

## 🧪 ESTRATEGIA DE TESTING

### Unit Tests (Por LOTE)

```csharp
public class CredencialRepositoryTests
{
    private readonly MiGenteDbContext _context;
    private readonly ICredencialRepository _repository;

    public CredencialRepositoryTests()
    {
        // Usar InMemory database para tests
        var options = new DbContextOptionsBuilder<MiGenteDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new MiGenteDbContext(options);
        _repository = new CredencialRepository(_context);
    }

    [Fact]
    public async Task GetByEmailAsync_EmailExiste_DebeRetornarCredencial()
    {
        // Arrange
        var email = "test@example.com";
        var credencial = Credencial.Crear(email, "hashedPassword", "user123");
        await _context.CredencialesRefactored.AddAsync(credencial);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByEmailAsync(email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(email, result.Email);
    }

    [Fact]
    public async Task ExistsByEmailAsync_EmailExiste_DebeRetornarTrue()
    {
        // Arrange
        var email = "exists@example.com";
        var credencial = Credencial.Crear(email, "hashedPassword", "user456");
        await _context.CredencialesRefactored.AddAsync(credencial);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _repository.ExistsByEmailAsync(email);

        // Assert
        Assert.True(exists);
    }
}
```

---

## 📊 RESUMEN DE ARCHIVOS A CREAR

| LOTE | Interfaces (Domain) | Implementaciones (Infrastructure) | Total Archivos |
|------|---------------------|-----------------------------------|----------------|
| 0 - Foundation | 3 (IRepository, IUnitOfWork, ISpecification) | 4 (Repository, UnitOfWork, Specification, SpecificationEvaluator) | 7 |
| 1 - Authentication | 1 | 1 | 2 |
| 2 - Empleadores | 2 | 2 | 4 |
| 3 - Contratistas | 3 | 3 | 6 |
| 4 - Empleados & Nómina | 6 | 6 | 12 |
| 5 - Suscripciones & Pagos | 4 | 4 | 8 |
| 6 - Calificaciones | 1 | 1 | 2 |
| 7 - Catálogos | 8 | 8 | 16 |
| 8 - Contrataciones & Seguridad | 4 | 4 | 8 |
| **TOTAL** | **32** | **33** | **65 archivos** |

**Archivos adicionales:**
- Tests: ~50 archivos (unit tests + integration tests)
- Documentación: 2-3 archivos

**GRAN TOTAL: ~115-120 archivos**

---

## 📝 CHECKLIST DE IMPLEMENTACIÓN

### Por cada LOTE

- [ ] **Paso 1:** Crear interfaces en `Domain/Interfaces/Repositories/`
- [ ] **Paso 2:** Crear implementaciones en `Infrastructure/Persistence/Repositories/`
- [ ] **Paso 3:** Registrar en `DependencyInjection.cs`
- [ ] **Paso 4:** Actualizar Commands/Queries para usar repositorios
- [ ] **Paso 5:** Eliminar/comentar uso directo de `IApplicationDbContext`
- [ ] **Paso 6:** Compilar y verificar 0 errores
- [ ] **Paso 7:** Crear unit tests
- [ ] **Paso 8:** Ejecutar tests y verificar pasan
- [ ] **Paso 9:** Crear integration tests
- [ ] **Paso 10:** Documentar en `LOTE_X_REPOSITORIES_COMPLETADO.md`

---

## 🎯 CRITERIOS DE ÉXITO

### Al completar todos los LOTES

- ✅ **0 uso directo de `IApplicationDbContext`** en Commands/Queries
- ✅ **Todos los Commands/Queries usan repositorios**
- ✅ **Todos los repositorios registrados en DI**
- ✅ **Cobertura de tests >= 80%**
- ✅ **Documentación actualizada**
- ✅ **API ejecutándose sin errores**
- ✅ **Todos los endpoints funcionan correctamente**

---

## 🔄 PRÓXIMOS PASOS

### 1. Ejecutar LOTE 0 (Foundation)

```bash
# Crear carpetas
mkdir -p src/Core/MiGenteEnLinea.Domain/Interfaces/Repositories
mkdir -p src/Infrastructure/MiGenteEnLinea.Infrastructure/Persistence/Repositories/Specifications

# Iniciar implementación
```

### 2. Validar con compilación

```bash
dotnet build MiGenteEnLinea.Clean.sln
```

### 3. Crear branch específico

```bash
git checkout -b feature/repository-pattern-lote-0-foundation
```

### 4. Documentar progreso

Crear `LOTE_0_FOUNDATION_COMPLETADO.md` al finalizar.

---

## 📚 REFERENCIAS

- [Repository Pattern - Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design)
- [Unit of Work Pattern](https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application)
- [Specification Pattern](https://deviq.com/design-patterns/specification-pattern)
- [Clean Architecture - Jason Taylor](https://github.com/jasontaylordev/CleanArchitecture)

---

**Documento creado:** 16 de Octubre de 2025  
**Última actualización:** 16 de Octubre de 2025  
**Versión:** 1.0  
**Estado:** 📋 LISTO PARA EJECUCIÓN
