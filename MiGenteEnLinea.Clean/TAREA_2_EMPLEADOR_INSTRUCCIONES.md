# 🚀 TAREA 2: Refactorizar Entidad Empleador (Ofertante) con DDD

**Fecha:** 12 de octubre, 2025  
**Estado:** ⏳ **PENDIENTE DE EJECUCIÓN**  
**Prerequisitos:** ✅ Tarea 1 (Credencial) completada exitosamente

---

## 🎯 COMANDO PARA CLAUDE SONNET 4.5 - MODO AGENTE

Copia y pega este comando en tu chat con Claude:

```
@workspace Lee y ejecuta el archivo prompts/AGENT_MODE_INSTRUCTIONS.md

TAREA 2: Refactorizar Entidad Empleador con patrón DDD

CONTEXTO:
- ✅ Tarea 1 (Credencial) COMPLETADA
- ✅ Clases base DDD ya existen (AuditableEntity, AggregateRoot, etc.)
- ✅ BCryptPasswordHasher implementado
- ✅ AuditableEntityInterceptor funcionando
- 📚 Entidad scaffolded: Infrastructure/Persistence/Entities/Generated/Ofertante.cs
- 📚 Tabla legacy: "Ofertantes" (nombre plural)
- 🎯 Nuevo nombre de dominio: "Empleador" (español, más claro)

ENTIDAD LEGACY (Referencia - NO MODIFICAR):
Ubicación: Codigo Fuente Mi Gente/MiGente_Front/Data/Ofertantes.cs

Estructura:
- ofertanteID (int)
- fechaPublicacion (datetime?)
- userID (string) - FK a Credenciales
- habilidades (string 200)
- experiencia (string 200)
- descripcion (string 500)
- foto (byte[])

OBJETIVO: Crear entidad Empleador como Aggregate Root con Rich Domain Model

AUTORIZACIÓN COMPLETA para ejecutar SIN CONFIRMACIÓN:

PASO 1: Crear Entidad Empleador (Domain)
✅ Archivo: src/Core/MiGenteEnLinea.Domain/Entities/Empleadores/Empleador.cs
✅ Heredar de: AggregateRoot (NO de AuditableEntity - AggregateRoot ya hereda de ella)
✅ Propiedades privadas con encapsulación
✅ Factory Method: Empleador.Create(string userId, string? habilidades, string? experiencia, string? descripcion)
✅ Domain Methods:
   - ActualizarPerfil(string? habilidades, string? experiencia, string? descripcion)
   - ActualizarFoto(byte[] foto)
   - EliminarFoto()
   - PuedePublicarOfertas() - bool
✅ Value Objects recomendados (si aplica):
   - UserId puede ser string por ahora (FK a Credencial.UserId)
✅ Validaciones en métodos:
   - Habilidades: max 200 caracteres
   - Experiencia: max 200 caracteres
   - Descripcion: max 500 caracteres
   - Foto: validar tamaño (max 5MB recomendado)
✅ Domain Events:
   - EmpleadorCreadoEvent(int empleadorId, string userId)
   - PerfilActualizadoEvent(int empleadorId)
   - FotoActualizadaEvent(int empleadorId)

PASO 2: Crear Fluent API Configuration
✅ Archivo: src/Infrastructure/Persistence/Configurations/EmpleadorConfiguration.cs
✅ Implementar: IEntityTypeConfiguration<Empleador>
✅ Mapear a tabla: "Ofertantes" (legacy plural)
✅ Mapeos de columnas:
   - Id → "ofertanteID"
   - FechaPublicacion → "fechaPublicacion"
   - UserId → "userID"
   - Habilidades → "habilidades"
   - Experiencia → "experiencia"
   - Descripcion → "descripcion"
   - Foto → "foto"
   - CreatedAt → "created_at" (nueva columna auditoría)
   - CreatedBy → "created_by" (nueva columna auditoría)
   - UpdatedAt → "updated_at" (nueva columna auditoría)
   - UpdatedBy → "updated_by" (nueva columna auditoría)
✅ Índices:
   - HasIndex(e => e.UserId).IsUnique() - Un empleador por userId
✅ Relaciones:
   - HasOne(Credencial) con UserId (si Credencial está en DbContext)
✅ Configuración:
   - MaxLength en strings (200, 200, 500)
   - Foto como byte[] sin restricción
✅ Ignorar:
   - Ignore(e => e.Events) - Domain events no se persisten

PASO 3: Actualizar DbContext
✅ Archivo: src/Infrastructure/Persistence/Contexts/MiGenteDbContext.cs
✅ Agregar:
   - DbSet<Empleador> Empleadores { get; set; }
   - modelBuilder.ApplyConfiguration(new EmpleadorConfiguration());

PASO 4: Crear Domain Events
✅ Carpeta: src/Core/MiGenteEnLinea.Domain/Events/Empleadores/
✅ Archivos:
   - EmpleadorCreadoEvent.cs
   - PerfilActualizadoEvent.cs
   - FotoActualizadaEvent.cs
✅ Cada evento debe:
   - Heredar de DomainEvent
   - Tener propiedades: EmpleadorId (y otras relevantes)
   - Constructor con parámetros

PASO 5: Validación y Build
✅ Ejecutar: cd "C:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea.Clean"
✅ Ejecutar: dotnet build
✅ Corregir errores de compilación si los hay
✅ Reportar resultado

LÍMITES (NO HACER):
⛔ NO ejecutar migraciones (dotnet ef database update)
⛔ NO modificar proyecto Legacy (Codigo Fuente Mi Gente/)
⛔ NO crear tests aún (fase posterior)
⛔ NO crear Commands/Queries/Controllers aún (Tarea 3)
⛔ NO modificar entidad scaffolded (Ofertante.cs en Generated/)

CONSIDERACIONES ESPECIALES:

1. NOMBRE DE DOMINIO:
   - En Domain: "Empleador" (español, singular, más claro)
   - En DB: "Ofertantes" (legacy, plural, mantener para compatibilidad)
   - En código legacy: "Ofertantes" (no tocar)

2. RELACIÓN CON CREDENCIAL:
   - Empleador tiene FK UserId que apunta a Credencial.UserId
   - Es una relación 1:1 (un usuario puede ser empleador O contratista)
   - Configurar en Fluent API si Credencial está en DbContext

3. FOTO COMO BYTE ARRAY:
   - Mantener como byte[] por ahora (legacy usa esto)
   - TODO futuro: Migrar a Azure Blob Storage y guardar solo URL
   - Validación de tamaño en el domain method

4. FECHAS:
   - FechaPublicacion: DateTime? nullable (puede no estar publicado aún)
   - Usar DateTime.UtcNow para consistencia

5. CAMPOS OPCIONALES:
   - Habilidades, Experiencia, Descripcion son nullable
   - Permitir null pero validar longitud si tiene valor

PATRÓN DE CÓDIGO (seguir ejemplo de Credencial):

```csharp
public sealed class Empleador : AggregateRoot
{
    // Properties (private setters)
    public int Id { get; private set; }
    public DateTime? FechaPublicacion { get; private set; }
    public string UserId { get; private set; }
    public string? Habilidades { get; private set; }
    public string? Experiencia { get; private set; }
    public string? Descripcion { get; private set; }
    public byte[]? Foto { get; private set; }

    // Private constructor for EF Core
    private Empleador() { }

    // Factory Method
    public static Empleador Create(
        string userId,
        string? habilidades = null,
        string? experiencia = null,
        string? descripcion = null)
    {
        // Validaciones
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId es requerido", nameof(userId));

        if (habilidades?.Length > 200)
            throw new ArgumentException("Habilidades no puede exceder 200 caracteres");

        // ... más validaciones

        var empleador = new Empleador
        {
            UserId = userId,
            Habilidades = habilidades?.Trim(),
            Experiencia = experiencia?.Trim(),
            Descripcion = descripcion?.Trim(),
            FechaPublicacion = DateTime.UtcNow
        };

        // Domain Event
        empleador.RaiseDomainEvent(new EmpleadorCreadoEvent(empleador.Id, userId));

        return empleador;
    }

    // Domain Methods
    public void ActualizarPerfil(
        string? habilidades,
        string? experiencia,
        string? descripcion)
    {
        // Validaciones
        // ...

        Habilidades = habilidades?.Trim();
        Experiencia = experiencia?.Trim();
        Descripcion = descripcion?.Trim();

        RaiseDomainEvent(new PerfilActualizadoEvent(Id));
    }

    public void ActualizarFoto(byte[] foto)
    {
        if (foto == null || foto.Length == 0)
            throw new ArgumentException("Foto no puede estar vacía");

        // Validar tamaño (max 5MB)
        const int maxSize = 5 * 1024 * 1024;
        if (foto.Length > maxSize)
            throw new ArgumentException($"Foto no puede exceder {maxSize / (1024 * 1024)}MB");

        Foto = foto;
        RaiseDomainEvent(new FotoActualizadaEvent(Id));
    }

    public void EliminarFoto()
    {
        Foto = null;
    }

    public bool PuedePublicarOfertas()
    {
        // Lógica de negocio: ¿qué requiere un empleador para publicar?
        // Por ahora: solo tener perfil básico
        return !string.IsNullOrWhiteSpace(UserId);
    }
}
```

VALIDACIÓN POST-EJECUCIÓN:

Al completar, verifica:
- [ ] Empleador.cs creado en Domain/Entities/Empleadores/
- [ ] Hereda de AggregateRoot
- [ ] Tiene Factory Method: Create()
- [ ] Tiene Domain Methods: ActualizarPerfil(), ActualizarFoto(), EliminarFoto()
- [ ] Validaciones en métodos (no en setters)
- [ ] EmpleadorConfiguration.cs creado
- [ ] Mapea a tabla "Ofertantes" (plural legacy)
- [ ] Índice único en UserId
- [ ] 3 Domain Events creados
- [ ] DbContext actualizado con DbSet<Empleador>
- [ ] dotnet build exitoso (0 errores)

REPORTAR:

Cada 3 pasos completados, reporta con este formato:

## 🔄 PROGRESO: Tarea 2 - Empleador

### ✅ Completado (Pasos 1-3)
- [x] **Paso 1:** Creada entidad Empleador.cs en Domain/Entities/Empleadores/
- [x] **Paso 2:** Creada configuración EmpleadorConfiguration.cs
- [x] **Paso 3:** Actualizado MiGenteDbContext.cs con DbSet<Empleador>

### 🔍 Validación Automática
- ✅ **Build:** Exitoso (0 errors)
- ✅ **Nomenclatura:** "Empleador" en dominio, "Ofertantes" en DB
- ✅ **Relación:** FK UserId configurado

### 📁 Archivos Creados
- `src/Core/MiGenteEnLinea.Domain/Entities/Empleadores/Empleador.cs`
- `src/Infrastructure/Persistence/Configurations/EmpleadorConfiguration.cs`
- `src/Core/MiGenteEnLinea.Domain/Events/Empleadores/EmpleadorCreadoEvent.cs`
- `src/Core/MiGenteEnLinea.Domain/Events/Empleadores/PerfilActualizadoEvent.cs`
- `src/Core/MiGenteEnLinea.Domain/Events/Empleadores/FotoActualizadaEvent.cs`

### 📁 Archivos Modificados
- `src/Infrastructure/Persistence/Contexts/MiGenteDbContext.cs` (+2 líneas)

### 🎯 Resultado
✅ Entidad Empleador completamente refactorizada con DDD
✅ Compatible con tabla legacy "Ofertantes"
✅ Listo para Tarea 3 (Contratista)

INICIO AHORA: Ejecuta Paso 1 - Crear Empleador.cs
```

---

## 📚 REFERENCIAS

### Entidad Scaffolded (Consulta - NO MODIFICAR)
- `MiGenteEnLinea.Clean/src/Infrastructure/Persistence/Entities/Generated/Ofertante.cs`

### Entidad Legacy (Consulta - NO MODIFICAR)
- `Codigo Fuente Mi Gente/MiGente_Front/Data/Ofertantes.cs`

### Ejemplo a Seguir (Tarea 1 Completada)
- `MiGenteEnLinea.Clean/src/Core/MiGenteEnLinea.Domain/Entities/Authentication/Credencial.cs`
- `MiGenteEnLinea.Clean/src/Infrastructure/Persistence/Configurations/CredencialConfiguration.cs`
- `MiGenteEnLinea.Clean/TAREA_1_CREDENCIAL_COMPLETADA.md`

### Clases Base Disponibles
- `AggregateRoot` (hereda de AuditableEntity)
- `DomainEvent`
- `ValueObject` (si necesitas crear value objects)

---

## 🎯 RESULTADO ESPERADO

Al completar esta tarea, tendrás:

1. ✅ Entidad **Empleador** como Rich Domain Model
2. ✅ Mapeo a tabla legacy **"Ofertantes"**
3. ✅ Lógica de negocio encapsulada en la entidad
4. ✅ Domain Events para comunicación
5. ✅ Auditoría automática (heredado de AggregateRoot)
6. ✅ Validaciones en métodos de dominio
7. ✅ Proyecto compilando sin errores
8. ✅ Listo para Tarea 3 (Contratista)

**Tiempo estimado:** 10-15 minutos de ejecución autónoma

---

_Creado: 12 de octubre, 2025_  
_Prerequisito: Tarea 1 (Credencial) ✅_  
_Siguiente: Tarea 3 (Contratista)_
