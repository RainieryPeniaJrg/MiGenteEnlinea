# LOTE 3: CONTRATISTAS - INICIO DE IMPLEMENTACIÓN

**Fecha:** 2025-01-13  
**Estado:** EN PROGRESO  
**Módulo:** Application Layer - Contratistas (Contractors)

---

## 📊 ANÁLISIS DE LEGACY

### ContratistasService.cs (10 métodos)

```csharp
1. getTodasUltimos20() → SearchContratistasQuery (Top 20, activo=true)
2. getMiPerfil(userID) → GetContratistaByUserIdQuery
3. getServicios(contratistaID) → GetServiciosContratistaQuery
4. agregarServicio(servicio) → AddServicioCommand
5. removerServicio(servicioID, contratistaID) → RemoveServicioCommand
6. GuardarPerfil(ct, userID) → UpdateContratistaCommand
7. ActivarPerfil(userID) → ActivarPerfilCommand
8. DesactivarPerfil(userID) → DesactivarPerfilCommand
9. getConCriterio(palabrasClave, zona) → SearchContratistasQuery (con filtros)
```

### index_contratista.aspx.cs

**Funcionalidad Principal:**
- Editar perfil de contratista (persona física o empresa)
- Subir/cambiar foto de perfil
- Activar/Desactivar perfil público
- Agregar servicios ofrecidos
- Remover servicios
- Campos editables:
  - Información básica: Titulo, Sector, Tipo (persona/empresa)
  - Contacto: Email, Telefono1, Telefono2, WhatsApp
  - Ubicación: Provincia
  - Experiencia: Años de experiencia
  - Presentación: Descripción del contratista

### Entidad Domain: Contratista.cs

**Propiedades:**
- Id, FechaIngreso, UserId
- Titulo, Tipo (1=PersonaFísica, 2=Empresa)
- Identificacion, Nombre, Apellido
- Sector, Experiencia, Presentacion
- Telefono1, Whatsapp1, Telefono2, Whatsapp2
- Email, Activo, Provincia, NivelNacional
- ImagenUrl

**Domain Methods:**
- `Create()` - Factory method con validaciones
- `ActualizarPerfil()` - Actualiza información básica
- `ActualizarContacto()` - Actualiza teléfonos y email
- `ActualizarImagen()` / `EliminarImagen()` - Gestión de foto
- `Activar()` / `Desactivar()` - Cambiar visibilidad
- `PuedeRecibirTrabajos()` - Validación de negocio
- `PerfilCompleto()` - Validación de completitud
- `TieneWhatsApp()` - Validación de contacto

---

## 🎯 PLAN DE IMPLEMENTACIÓN

### FASE 1: Commands (5 Commands × 3 archivos = 15 archivos)

#### 1.1 CreateContratistaCommand
**Archivo:** `Features/Contratistas/Commands/CreateContratista/CreateContratistaCommand.cs`
**Lógica Legacy:** Al registrarse como contratista
**Request:**
```csharp
public record CreateContratistaCommand(
    string UserId,
    string Nombre,
    string Apellido,
    int Tipo = 1, // Default: Persona Física
    string? Titulo = null,
    string? Identificacion = null,
    string? Sector = null,
    int? Experiencia = null,
    string? Presentacion = null,
    string? Telefono1 = null,
    bool Whatsapp1 = false,
    string? Provincia = null
) : IRequest<int>; // Returns contratistaId
```
**Handler:**
- Validar userId existe en Credenciales
- Verificar no existe otro contratista con mismo userId (1:1)
- Crear con `Contratista.Create()`
- Guardar en DbContext
- Retornar contratistaId

**Validator:**
- UserId: Required, GUID format
- Nombre: Required, MaxLength(20)
- Apellido: Required, MaxLength(50)
- Tipo: Must be 1 or 2
- Titulo: MaxLength(70)
- Identificacion: MaxLength(20)
- Sector: MaxLength(40)
- Presentacion: MaxLength(250)
- Telefono1: MaxLength(16)
- Provincia: MaxLength(50)

#### 1.2 UpdateContratistaCommand
**Archivo:** `Features/Contratistas/Commands/UpdateContratista/UpdateContratistaCommand.cs`
**Lógica Legacy:** `ContratistasService.GuardarPerfil()`
**Request:**
```csharp
public record UpdateContratistaCommand(
    string UserId,
    string? Titulo = null,
    string? Sector = null,
    int? Experiencia = null,
    string? Presentacion = null,
    string? Provincia = null,
    bool? NivelNacional = null,
    string? Telefono1 = null,
    bool? Whatsapp1 = null,
    string? Telefono2 = null,
    bool? Whatsapp2 = null,
    string? Email = null
) : IRequest;
```
**Handler:**
- Buscar contratista por userId
- Si no existe → throw NotFoundException
- Si tiene cambios de perfil → llamar `contratista.ActualizarPerfil()`
- Si tiene cambios de contacto → llamar `contratista.ActualizarContacto()`
- Guardar cambios

#### 1.3 UpdateContratistaImagenCommand
**Archivo:** `Features/Contratistas/Commands/UpdateContratistaImagen/UpdateContratistaImagenCommand.cs`
**Lógica Legacy:** `guardarImagen()` en index_contratista.aspx.cs
**Request:**
```csharp
public record UpdateContratistaImagenCommand(
    string UserId,
    string ImagenUrl // URL después de subir a storage
) : IRequest;
```
**Handler:**
- Buscar contratista por userId
- Llamar `contratista.ActualizarImagen(imagenUrl)`
- Guardar cambios

#### 1.4 ActivarPerfilCommand / DesactivarPerfilCommand
**Archivos:** 
- `Features/Contratistas/Commands/ActivarPerfil/ActivarPerfilCommand.cs`
- `Features/Contratistas/Commands/DesactivarPerfil/DesactivarPerfilCommand.cs`

**Lógica Legacy:** 
- `ContratistasService.ActivarPerfil()`
- `ContratistasService.DesactivarPerfil()`

**Request:**
```csharp
public record ActivarPerfilCommand(string UserId) : IRequest;
public record DesactivarPerfilCommand(string UserId) : IRequest;
```
**Handler:**
- Buscar contratista por userId
- Llamar `contratista.Activar()` o `contratista.Desactivar()`
- Guardar cambios

#### 1.5 AddServicioCommand / RemoveServicioCommand
**Archivos:** 
- `Features/Contratistas/Commands/AddServicio/AddServicioCommand.cs`
- `Features/Contratistas/Commands/RemoveServicio/RemoveServicioCommand.cs`

**Lógica Legacy:** 
- `ContratistasService.agregarServicio()`
- `ContratistasService.removerServicio()`

**Request:**
```csharp
public record AddServicioCommand(
    int ContratistaId,
    string DetalleServicio
) : IRequest<int>; // Returns servicioId

public record RemoveServicioCommand(
    int ServicioId,
    int ContratistaId
) : IRequest;
```

**Handler (AddServicio):**
- Validar contratista existe
- Crear nueva entidad `ContratistaServicio`
- Agregar a DbContext
- Guardar cambios
- Retornar servicioId

**Handler (RemoveServicio):**
- Buscar servicio por servicioId y contratistaId
- Si no existe → throw NotFoundException
- Remover de DbContext
- Guardar cambios

---

### FASE 2: Queries (4 Queries × 2 archivos = 8 archivos)

#### 2.1 GetContratistaByUserIdQuery
**Archivo:** `Features/Contratistas/Queries/GetContratistaByUserId/GetContratistaByUserIdQuery.cs`
**Lógica Legacy:** `ContratistasService.getMiPerfil(userID)`
**Request:**
```csharp
public record GetContratistaByUserIdQuery(string UserId) : IRequest<ContratistaDto?>;
```
**Handler:**
- Query con AsNoTracking()
- Where userId == userId
- Proyectar a ContratistaDto
- Retornar null si no existe

#### 2.2 GetContratistaByIdQuery
**Archivo:** `Features/Contratistas/Queries/GetContratistaById/GetContratistaByIdQuery.cs`
**Request:**
```csharp
public record GetContratistaByIdQuery(int ContratistaId) : IRequest<ContratistaDto?>;
```
**Handler:**
- Query con AsNoTracking()
- Where id == contratistaId
- Proyectar a ContratistaDto
- Retornar null si no existe

#### 2.3 SearchContratistasQuery
**Archivo:** `Features/Contratistas/Queries/SearchContratistas/SearchContratistasQuery.cs`
**Lógica Legacy:** 
- `ContratistasService.getTodasUltimos20()` → sin filtros
- `ContratistasService.getConCriterio(palabrasClave, zona)` → con filtros

**Request:**
```csharp
public record SearchContratistasQuery(
    string? SearchTerm = null,      // Busca en Titulo, Presentacion, Sector
    string? Provincia = null,       // Filtro por provincia
    string? Sector = null,          // Filtro por sector
    int? ExperienciaMinima = null,  // Años de experiencia mínimos
    bool SoloActivos = true,        // Default: solo activos
    int PageIndex = 1,
    int PageSize = 10
) : IRequest<SearchContratistasResult>;

public record SearchContratistasResult(
    List<ContratistaDto> Contratistas,
    int TotalRecords,
    int PageIndex,
    int PageSize
)
{
    public int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);
}
```

**Handler:**
- Base query: `_context.Contratistas.AsNoTracking()`
- Si SoloActivos → Where Activo == true
- Si SearchTerm → Where Titulo/Presentacion/Sector contains searchTerm (case-insensitive)
- Si Provincia != "Cualquier Ubicacion" → Where Provincia == provincia
- Si Sector → Where Sector == sector
- Si ExperienciaMinima → Where Experiencia >= experienciaMinima
- Count total
- OrderByDescending(FechaIngreso)
- Skip/Take para paginación
- Proyectar a ContratistaDto

#### 2.4 GetServiciosContratistaQuery
**Archivo:** `Features/Contratistas/Queries/GetServiciosContratista/GetServiciosContratistaQuery.cs`
**Lógica Legacy:** `ContratistasService.getServicios(contratistaID)`
**Request:**
```csharp
public record GetServiciosContratistaQuery(int ContratistaId) : IRequest<List<ServicioContratistaDto>>;
```
**Handler:**
- Query con AsNoTracking()
- Where contratistaId == contratistaId
- Proyectar a List<ServicioContratistaDto>

---

### FASE 3: DTOs (2 archivos)

#### 3.1 ContratistaDto
**Archivo:** `Features/Contratistas/Common/ContratistaDto.cs`
```csharp
public record ContratistaDto
{
    public int ContratistaId { get; init; }
    public string UserId { get; init; } = string.Empty;
    public DateTime? FechaIngreso { get; init; }
    public string? Titulo { get; init; }
    public int Tipo { get; init; } // 1=Persona, 2=Empresa
    public string? Identificacion { get; init; }
    public string? Nombre { get; init; }
    public string? Apellido { get; init; }
    public string? NombreCompleto { get; init; } // Calculado
    public string? Sector { get; init; }
    public int? Experiencia { get; init; }
    public string? Presentacion { get; init; }
    public string? Telefono1 { get; init; }
    public bool Whatsapp1 { get; init; }
    public string? Telefono2 { get; init; }
    public bool Whatsapp2 { get; init; }
    public string? Email { get; init; }
    public bool Activo { get; init; }
    public string? Provincia { get; init; }
    public bool NivelNacional { get; init; }
    public string? ImagenUrl { get; init; }
    public bool TieneImagen => !string.IsNullOrWhiteSpace(ImagenUrl);
    public bool TieneWhatsApp { get; init; } // Calculado
    public bool PerfilCompleto { get; init; } // Calculado
}
```

#### 3.2 ServicioContratistaDto
**Archivo:** `Features/Contratistas/Common/ServicioContratistaDto.cs`
```csharp
public record ServicioContratistaDto
{
    public int ServicioId { get; init; }
    public int ContratistaId { get; init; }
    public string DetalleServicio { get; init; } = string.Empty;
}
```

---

### FASE 4: Controller (1 archivo)

#### ContratistasController
**Archivo:** `Presentation/MiGenteEnLinea.API/Controllers/ContratistasController.cs`

**Endpoints:**

1. **POST /api/contratistas** → CreateContratistaCommand
   - Request: CreateContratistaRequest (JSON)
   - Response: 201 Created con { contratistaId, message }

2. **GET /api/contratistas/{contratistaId}** → GetContratistaByIdQuery
   - Response: 200 OK con ContratistaDto | 404 Not Found

3. **GET /api/contratistas/by-user/{userId}** → GetContratistaByUserIdQuery
   - Response: 200 OK con ContratistaDto | 404 Not Found

4. **GET /api/contratistas** → SearchContratistasQuery
   - Query params: searchTerm, provincia, sector, experienciaMinima, soloActivos, pageIndex, pageSize
   - Response: 200 OK con SearchContratistasResult

5. **GET /api/contratistas/{contratistaId}/servicios** → GetServiciosContratistaQuery
   - Response: 200 OK con List<ServicioContratistaDto>

6. **PUT /api/contratistas/{userId}** → UpdateContratistaCommand
   - Request: UpdateContratistaRequest (JSON)
   - Response: 200 OK | 400 Bad Request | 404 Not Found

7. **PUT /api/contratistas/{userId}/imagen** → UpdateContratistaImagenCommand
   - Request: multipart/form-data con ImagenUrl
   - Response: 200 OK | 400 Bad Request | 404 Not Found

8. **POST /api/contratistas/{userId}/activar** → ActivarPerfilCommand
   - Response: 200 OK | 404 Not Found

9. **POST /api/contratistas/{userId}/desactivar** → DesactivarPerfilCommand
   - Response: 200 OK | 404 Not Found

10. **POST /api/contratistas/{contratistaId}/servicios** → AddServicioCommand
    - Request: { detalleServicio }
    - Response: 201 Created con { servicioId, message }

11. **DELETE /api/contratistas/{contratistaId}/servicios/{servicioId}** → RemoveServicioCommand
    - Response: 200 OK | 404 Not Found

---

## 📝 RESUMEN DE ARCHIVOS A CREAR

### Commands (15 archivos)
1. `CreateContratistaCommand.cs` + `Handler.cs` + `Validator.cs` (3 archivos)
2. `UpdateContratistaCommand.cs` + `Handler.cs` + `Validator.cs` (3 archivos)
3. `UpdateContratistaImagenCommand.cs` + `Handler.cs` + `Validator.cs` (3 archivos)
4. `ActivarPerfilCommand.cs` + `Handler.cs` + `Validator.cs` (3 archivos)
5. `DesactivarPerfilCommand.cs` + `Handler.cs` + `Validator.cs` (3 archivos)
6. `AddServicioCommand.cs` + `Handler.cs` + `Validator.cs` (3 archivos)
7. `RemoveServicioCommand.cs` + `Handler.cs` + `Validator.cs` (3 archivos)

**NOTA:** Algunos Commands se pueden combinar:
- ActivarPerfil y DesactivarPerfil → `ToggleActivoCommand`
- AddServicio y RemoveServicio → Mantener separados (diferentes responsabilidades)

### Queries (8 archivos)
1. `GetContratistaByUserIdQuery.cs` + `Handler.cs` (2 archivos)
2. `GetContratistaByIdQuery.cs` + `Handler.cs` (2 archivos)
3. `SearchContratistasQuery.cs` + `Handler.cs` (2 archivos)
4. `GetServiciosContratistaQuery.cs` + `Handler.cs` (2 archivos)

### DTOs (2 archivos)
1. `ContratistaDto.cs`
2. `ServicioContratistaDto.cs`

### Controller (1 archivo)
1. `ContratistasController.cs`

**TOTAL:** ~26 archivos (dependiendo de si combinamos algunos Commands)

---

## ⏱️ ESTIMACIÓN DE TIEMPO

- **FASE 1 (Commands):** 4-5 horas (~20-25 min por Command × 7)
- **FASE 2 (Queries):** 2-3 horas (~30-45 min por Query × 4)
- **FASE 3 (DTOs):** 30 minutos
- **FASE 4 (Controller):** 1.5-2 horas (11 endpoints con documentación)
- **Compilación y Testing:** 1 hora

**TOTAL ESTIMADO:** 9-11 horas

---

## 🚨 DECISIONES DE ARQUITECTURA

### 1. ¿Combinar ActivarPerfil y DesactivarPerfil?

**OPCIÓN A: Dos Commands separados** (RECOMENDADO)
```csharp
ActivarPerfilCommand(userId)
DesactivarPerfilCommand(userId)
```
**Ventajas:**
- Más explícito y claro
- Mejor documentación en Swagger (dos endpoints claros)
- Fácil agregar validaciones específicas (ej: no activar si falta info)

**OPCIÓN B: Un Command con flag**
```csharp
ToggleActivoCommand(userId, bool activar)
```
**Ventajas:**
- Menos archivos (3 en lugar de 6)
- Menos duplicación de código

**DECISIÓN:** Usar OPCIÓN A (dos Commands) para claridad y semántica HTTP (POST para acciones).

### 2. Gestión de Servicios

Los servicios de contratista se manejan en tabla separada `Contratistas_Servicios` (relación 1:N).

**Estructura:**
- `servicioID` (PK, int, identity)
- `contratistaID` (FK, int)
- `detalleServicio` (nvarchar(50))

**Decisión:** Mantener AddServicioCommand y RemoveServicioCommand separados (CRUD estándar).

### 3. DTO Projection

ContratistaDto incluirá campos calculados:
- `NombreCompleto = $"{Nombre} {Apellido}"`
- `TieneWhatsApp = (Whatsapp1 && Telefono1 != null) || (Whatsapp2 && Telefono2 != null)`
- `PerfilCompleto = contratista.PerfilCompleto()` (método de dominio)

Esto reduce lógica en el frontend.

---

## 📖 NEXT STEPS

1. ✅ **Documentar plan** (este archivo)
2. ⏳ **Implementar Commands** (FASE 1)
3. ⏳ **Implementar Queries** (FASE 2)
4. ⏳ **Crear DTOs** (FASE 3)
5. ⏳ **Implementar Controller** (FASE 4)
6. ⏳ **Compilar y verificar** (0 errores esperado)
7. ⏳ **Testing con Swagger UI**
8. ⏳ **Documentar completado** (LOTE_3_CONTRATISTAS_COMPLETADO.md)

---

**Iniciado por:** GitHub Copilot (AI Agent)  
**Fecha:** 2025-01-13  
**Contexto:** Migración de ASP.NET Web Forms a Clean Architecture .NET 8
