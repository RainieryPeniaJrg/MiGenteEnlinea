# 🔗 VALIDACIÓN Y CONFIGURACIÓN DE RELACIONES DE BASE DE DATOS

**Fecha de Creación:** 12 de octubre, 2025  
**Estado:** FASE CRÍTICA ANTES DE DEPLOYMENT  
**Prioridad:** 🔴 MÁXIMA  
**Agente Recomendado:** Claude Sonnet 4.5 (Modo Autónomo)

---

## 🎯 OBJETIVO

Asegurar que **TODAS** las relaciones de base de datos (FKs, navegación, constraints) configuradas en el proyecto Clean Architecture sean **100% IDÉNTICAS** a las del proyecto Legacy (EF6 Database-First con EDMX).

**⚠️ CRÍTICO:** Las relaciones de la base de datos son la columna vertebral del sistema. Una configuración incorrecta causará:
- ❌ Errores en tiempo de ejecución al cargar navegación
- ❌ Pérdida de datos en cascadas mal configuradas
- ❌ Inconsistencias entre legacy y clean al compartir la misma DB

---

## 📊 INVENTARIO DE RELACIONES EN LEGACY (EDMX)

### RELACIONES IDENTIFICADAS EN DataModel.edmx

#### 1. **Contratistas → Contratistas_Fotos** (1:N)
```xml
<Association Name="FK_Contratistas_Fotos_Contratistas">
  <End Role="Contratistas" Type="Self.Contratistas" Multiplicity="0..1" />
  <End Role="Contratistas_Fotos" Type="Self.Contratistas_Fotos" Multiplicity="*" />
  <ReferentialConstraint>
    <Principal Role="Contratistas">
      <PropertyRef Name="contratistaID" />
    </Principal>
    <Dependent Role="Contratistas_Fotos">
      <PropertyRef Name="contratistaID" />
    </Dependent>
  </ReferentialConstraint>
</Association>
```
**FK:** `Contratistas_Fotos.contratistaID` → `Contratistas.contratistaID`

#### 2. **Contratistas → Contratistas_Servicios** (1:N)
```xml
<Association Name="FK_Contratistas_Servicios_Contratistas">
  <End Role="Contratistas" Type="Self.Contratistas" Multiplicity="0..1" />
  <End Role="Contratistas_Servicios" Type="Self.Contratistas_Servicios" Multiplicity="*" />
  <ReferentialConstraint>
    <Principal Role="Contratistas">
      <PropertyRef Name="contratistaID" />
    </Principal>
    <Dependent Role="Contratistas_Servicios">
      <PropertyRef Name="contratistaID" />
    </Dependent>
  </ReferentialConstraint>
</Association>
```
**FK:** `Contratistas_Servicios.contratistaID` → `Contratistas.contratistaID`

#### 3. **EmpleadosTemporales → DetalleContrataciones** (1:N)
```xml
<Association Name="FK_DetalleContrataciones_EmpleadosTemporales">
  <End Role="EmpleadosTemporales" Type="Self.EmpleadosTemporales" Multiplicity="0..1" />
  <End Role="DetalleContrataciones" Type="Self.DetalleContrataciones" Multiplicity="*" />
  <ReferentialConstraint>
    <Principal Role="EmpleadosTemporales">
      <PropertyRef Name="contratacionID" />
    </Principal>
    <Dependent Role="DetalleContrataciones">
      <PropertyRef Name="contratacionID" />
    </Dependent>
  </ReferentialConstraint>
</Association>
```
**FK:** `DetalleContrataciones.contratacionID` → `EmpleadosTemporales.contratacionID`

#### 4. **Empleador_Recibos_Header_Contrataciones → Empleador_Recibos_Detalle_Contrataciones** (1:N)
```xml
<Association Name="FK_Empleador_Recibos_Detalle_Contrataciones_Empleador_Recibos_Header_Contrataciones">
  <End Role="Empleador_Recibos_Header_Contrataciones" Type="Self.Empleador_Recibos_Header_Contrataciones" Multiplicity="0..1" />
  <End Role="Empleador_Recibos_Detalle_Contrataciones" Type="Self.Empleador_Recibos_Detalle_Contrataciones" Multiplicity="*" />
  <ReferentialConstraint>
    <Principal Role="Empleador_Recibos_Header_Contrataciones">
      <PropertyRef Name="pagoID" />
    </Principal>
    <Dependent Role="Empleador_Recibos_Detalle_Contrataciones">
      <PropertyRef Name="pagoID" />
    </Dependent>
  </ReferentialConstraint>
</Association>
```
**FK:** `Empleador_Recibos_Detalle_Contrataciones.pagoID` → `Empleador_Recibos_Header_Contrataciones.pagoID`

#### 5. **Empleador_Recibos_Header → Empleador_Recibos_Detalle** (1:N)
```xml
<Association Name="FK_Empleador_Recibos_Detalle_Empleador_Recibos_Header">
  <End Role="Empleador_Recibos_Header" Type="Self.Empleador_Recibos_Header" Multiplicity="0..1" />
  <End Role="Empleador_Recibos_Detalle" Type="Self.Empleador_Recibos_Detalle" Multiplicity="*" />
  <ReferentialConstraint>
    <Principal Role="Empleador_Recibos_Header">
      <PropertyRef Name="pagoID" />
    </Principal>
    <Dependent Role="Empleador_Recibos_Detalle">
      <PropertyRef Name="pagoID" />
    </Dependent>
  </ReferentialConstraint>
</Association>
```
**FK:** `Empleador_Recibos_Detalle.pagoID` → `Empleador_Recibos_Header.pagoID`

#### 6. **EmpleadosTemporales → Empleador_Recibos_Header_Contrataciones** (1:N)
```xml
<Association Name="FK_Empleador_Recibos_Header_Contrataciones_EmpleadosTemporales">
  <End Role="EmpleadosTemporales" Type="Self.EmpleadosTemporales" Multiplicity="0..1" />
  <End Role="Empleador_Recibos_Header_Contrataciones" Type="Self.Empleador_Recibos_Header_Contrataciones" Multiplicity="*" />
  <ReferentialConstraint>
    <Principal Role="EmpleadosTemporales">
      <PropertyRef Name="contratacionID" />
    </Principal>
    <Dependent Role="Empleador_Recibos_Header_Contrataciones">
      <PropertyRef Name="contratacionID" />
    </Dependent>
  </ReferentialConstraint>
</Association>
```
**FK:** `Empleador_Recibos_Header_Contrataciones.contratacionID` → `EmpleadosTemporales.contratacionID`

#### 7. **Empleados → Empleador_Recibos_Header** (1:N)
```xml
<Association Name="FK_Empleador_Recibos_Header_Empleados">
  <End Role="Empleados" Type="Self.Empleados" Multiplicity="0..1" />
  <End Role="Empleador_Recibos_Header" Type="Self.Empleador_Recibos_Header" Multiplicity="*" />
  <ReferentialConstraint>
    <Principal Role="Empleados">
      <PropertyRef Name="empleadoID" />
    </Principal>
    <Dependent Role="Empleador_Recibos_Header">
      <PropertyRef Name="empleadoID" />
    </Dependent>
  </ReferentialConstraint>
</Association>
```
**FK:** `Empleador_Recibos_Header.empleadoID` → `Empleados.empleadoID`

#### 8. **Cuentas → perfilesInfo** (1:N) ⚠️ TABLA LEGACY DEPRECADA
```xml
<Association Name="FK_perfilesInfo_Cuentas">
  <End Role="Cuentas" Type="Self.Cuentas" Multiplicity="0..1" />
  <End Role="perfilesInfo" Type="Self.perfilesInfo" Multiplicity="*" />
  <ReferentialConstraint>
    <Principal Role="Cuentas">
      <PropertyRef Name="cuentaID" />
    </Principal>
    <Dependent Role="perfilesInfo">
      <PropertyRef Name="cuentaID" />
    </Dependent>
  </ReferentialConstraint>
</Association>
```
**FK:** `perfilesInfo.cuentaID` → `Cuentas.cuentaID`

**NOTA:** `Cuentas` y `perfilesInfo` son tablas legacy que NO se deben refactorizar (permanecen como tablas de compatibilidad).

#### 9. **Planes_empleadores → Suscripciones** (1:N)
```xml
<Association Name="FK_Suscripciones_Planes_empleadores">
  <End Role="Planes_empleadores" Type="Self.Planes_empleadores" Multiplicity="0..1" />
  <End Role="Suscripciones" Type="Self.Suscripciones" Multiplicity="*" />
  <ReferentialConstraint>
    <Principal Role="Planes_empleadores">
      <PropertyRef Name="planID" />
    </Principal>
    <Dependent Role="Suscripciones">
      <PropertyRef Name="planID" />
    </Dependent>
  </ReferentialConstraint>
</Association>
```
**FK:** `Suscripciones.planID` → `Planes_empleadores.planID`

---

## 📋 RESUMEN: 9 RELACIONES A VALIDAR

| # | Tabla Principal | Tabla Dependiente | FK Column | Estado Clean |
|---|----------------|-------------------|-----------|--------------|
| 1 | Contratistas | Contratistas_Fotos | contratistaID | ✅ Configurado |
| 2 | Contratistas | Contratistas_Servicios | contratistaID | ✅ Configurado |
| 3 | EmpleadosTemporales | DetalleContrataciones | contratacionID | ⚠️ Validar |
| 4 | Empleador_Recibos_Header_Contrataciones | Empleador_Recibos_Detalle_Contrataciones | pagoID | ⚠️ Validar |
| 5 | Empleador_Recibos_Header | Empleador_Recibos_Detalle | pagoID | ✅ Configurado |
| 6 | EmpleadosTemporales | Empleador_Recibos_Header_Contrataciones | contratacionID | ⚠️ Validar |
| 7 | Empleados | Empleador_Recibos_Header | empleadoID | ✅ Configurado |
| 8 | Cuentas (legacy) | perfilesInfo (legacy) | cuentaID | ⚠️ Configurar |
| 9 | Planes_empleadores | Suscripciones | planID | ✅ Configurado |

---

## 🔍 TAREAS DE VALIDACIÓN

### TAREA 1: Auditar Configuraciones Existentes

**Ubicación:** `MiGenteEnLinea.Clean/src/Infrastructure/MiGenteEnLinea.Infrastructure/Persistence/Configurations/`

**Archivos a revisar:**
```
✅ ContratistaConfiguration.cs
✅ ContratistaFotoConfiguration.cs
✅ ContratistaServicioConfiguration.cs
⚠️ DetalleContratacionConfiguration.cs
⚠️ EmpleadorRecibosDetalleContratacioneConfiguration.cs
⚠️ EmpleadorRecibosHeaderContratacioneConfiguration.cs
✅ ReciboDetalleConfiguration.cs
✅ ReciboHeaderConfiguration.cs
✅ EmpleadoConfiguration.cs
⚠️ EmpleadoTemporalConfiguration.cs
⚠️ PerfilesInfoConfiguration.cs (falta Perfile/Cuentas FK)
✅ SuscripcionConfiguration.cs
✅ PlanEmpleadorConfiguration.cs
```

**Acción:**
1. Leer cada archivo de configuración
2. Verificar que las relaciones de navegación estén **explícitamente configuradas** con `.HasOne()`, `.WithMany()`, `.HasForeignKey()`, `.OnDelete()`
3. Comparar con el EDMX de arriba para asegurar paridad 100%

---

### TAREA 2: Configurar Relaciones Faltantes o Incorrectas

#### Ejemplo de Configuración Correcta (ContratistaConfiguration.cs):

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Contratistas;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

public class ContratistaConfiguration : IEntityTypeConfiguration<Contratista>
{
    public void Configure(EntityTypeBuilder<Contratista> builder)
    {
        builder.ToTable("Contratistas");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("contratistaID")
            .ValueGeneratedOnAdd();

        // ... otras propiedades ...

        // ✅ RELACIÓN 1: Contratista → ContratistasFotos (1:N)
        builder.HasMany(c => c.Fotos)
            .WithOne(f => f.Contratista)
            .HasForeignKey(f => f.ContratistaId)
            .HasConstraintName("FK_Contratistas_Fotos_Contratistas")
            .OnDelete(DeleteBehavior.Cascade); // O Restrict según regla de negocio

        // ✅ RELACIÓN 2: Contratista → ContratistasServicios (1:N)
        builder.HasMany(c => c.Servicios)
            .WithOne(s => s.Contratista)
            .HasForeignKey(s => s.ContratistaId)
            .HasConstraintName("FK_Contratistas_Servicios_Contratistas")
            .OnDelete(DeleteBehavior.Cascade); // O Restrict según regla de negocio
    }
}
```

#### Patrón a Seguir para TODAS las Relaciones:

```csharp
// En la configuración de la entidad PRINCIPAL (que tiene la PK)
builder.HasMany(principal => principal.CollectionNavigation)
    .WithOne(dependent => dependent.ReferenceNavigation)
    .HasForeignKey(dependent => dependent.ForeignKeyProperty)
    .HasConstraintName("FK_TablaDependiente_TablaPrincipal") // Nombre EXACTO del constraint en DB
    .OnDelete(DeleteBehavior.Cascade); // O Restrict/SetNull según el caso

// Ejemplo: Empleado (1) → RecibosHeader (N)
builder.HasMany(e => e.RecibosHeader) // Colección en Empleado
    .WithOne(r => r.Empleado)          // Referencia en ReciboHeader
    .HasForeignKey(r => r.EmpleadoId)  // FK en ReciboHeader
    .HasConstraintName("FK_Empleador_Recibos_Header_Empleados")
    .OnDelete(DeleteBehavior.Restrict); // No borrar empleado si tiene recibos
```

---

### TAREA 3: Validar DeleteBehavior

**⚠️ IMPORTANTE:** El `OnDelete()` debe reflejar la lógica de negocio:

| Escenario | DeleteBehavior | Explicación |
|-----------|---------------|-------------|
| **Cascade** | `.OnDelete(DeleteBehavior.Cascade)` | Si se borra el principal, se borran TODOS los dependientes (ej: Contratista → Fotos) |
| **Restrict** | `.OnDelete(DeleteBehavior.Restrict)` | NO permite borrar el principal si existen dependientes (ej: Empleado → Recibos) |
| **SetNull** | `.OnDelete(DeleteBehavior.SetNull)` | Si se borra el principal, el FK del dependiente se pone en NULL (requiere FK nullable) |
| **NoAction** | `.OnDelete(DeleteBehavior.NoAction)` | No hace nada (delega a la DB) |

**Revisar cada relación y aplicar el comportamiento correcto según las reglas de negocio.**

---

### TAREA 4: Validar Constraint Names

**CRÍTICO:** Los nombres de constraints deben ser **EXACTOS** a los de la base de datos legacy para evitar conflictos cuando Clean Architecture y Legacy compartan la DB.

```csharp
.HasConstraintName("FK_Contratistas_Fotos_Contratistas") // Nombre EXACTO del EDMX
```

**Acción:** Revisar cada configuración y asegurar que los nombres coincidan con los del EDMX listado arriba.

---

## ✅ CHECKLIST DE VALIDACIÓN

Ejecutar estas validaciones después de configurar todas las relaciones:

### 1. Compilación Limpia
```bash
cd "C:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea.Clean"
dotnet build
# Debe resultar: Build succeeded. 0 Error(s)
```

### 2. Validar Migration (SIN APLICARLA)
```bash
cd "C:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea.Clean\src\Infrastructure\MiGenteEnLinea.Infrastructure"

# Generar una migration temporal para ver diferencias
dotnet ef migrations add RelationshipsValidation --startup-project ..\..\Presentation\MiGenteEnLinea.API\ --no-build

# Revisar el archivo generado en Migrations/ para ver si detecta cambios
# SI DETECTA CAMBIOS: Hay discrepancias entre Clean y la DB actual

# Eliminar la migration (NO aplicarla):
dotnet ef migrations remove --startup-project ..\..\Presentation\MiGenteEnLinea.API\ --force
```

**Resultado Esperado:** La migration debe estar **vacía** o solo tener cambios menores en índices, porque las relaciones YA existen en la DB legacy.

### 3. Test de Navegación

Crear un test simple en `tests/MiGenteEnLinea.Infrastructure.Tests/`:

```csharp
[Fact]
public async Task Should_Load_Contratista_WithRelatedData()
{
    // Arrange
    var options = new DbContextOptionsBuilder<MiGenteDbContext>()
        .UseSqlServer("TU_CONNECTION_STRING")
        .Options;

    using var context = new MiGenteDbContext(options);

    // Act
    var contratista = await context.Contratistas
        .Include(c => c.Fotos)
        .Include(c => c.Servicios)
        .FirstOrDefaultAsync();

    // Assert
    Assert.NotNull(contratista);
    Assert.NotNull(contratista.Fotos); // No debe ser null
    Assert.NotNull(contratista.Servicios); // No debe ser null
}
```

**Ejecutar:**
```bash
dotnet test
```

**Resultado Esperado:** Test pasa ✅ sin errores de navegación.

---

## 🚨 CASOS ESPECIALES

### Relaciones con Tablas Legacy (Cuentas, perfilesInfo)

Estas tablas **NO deben refactorizarse** a DDD. Configurarlas en el DbContext directamente:

```csharp
// En MiGenteDbContext.OnModelCreating()
modelBuilder.Entity<Perfile>(entity =>
{
    entity.ToTable("Perfiles");
    entity.HasKey(e => e.PerfilId).HasColumnName("perfilID");

    entity.HasMany(e => e.PerfilesInfos)
        .WithOne(e => e.Perfil)
        .HasForeignKey(e => e.PerfilId)
        .HasConstraintName("FK_perfilesInfo_Perfiles")
        .OnDelete(DeleteBehavior.Restrict);
});

modelBuilder.Entity<PerfilesInfo>(entity =>
{
    entity.ToTable("perfilesInfo");
    entity.HasKey(e => e.Id).HasColumnName("id");
});
```

---

## 📄 DOCUMENTACIÓN REQUERIDA

Después de validar y configurar todas las relaciones, crear archivo:

**`MiGenteEnLinea.Clean/DATABASE_RELATIONSHIPS_REPORT.md`**

```markdown
# Reporte de Validación de Relaciones de Base de Datos

**Fecha:** [FECHA]
**Ejecutado por:** [AGENTE/DEVELOPER]

## Resumen

- ✅ Relaciones configuradas: X/9
- ⚠️ Relaciones con warnings: X
- ❌ Relaciones faltantes: X

## Detalles por Relación

### 1. Contratistas → Contratistas_Fotos
- **Estado:** ✅ Configurado
- **Archivo:** ContratistaConfiguration.cs
- **DeleteBehavior:** Cascade
- **Constraint Name:** FK_Contratistas_Fotos_Contratistas

[... repetir para todas las 9 relaciones ...]

## Tests Ejecutados

- ✅ dotnet build: Success
- ✅ dotnet ef migrations add: Sin cambios detectados
- ✅ Navigation tests: 9/9 pasaron

## Problemas Encontrados

[Lista de cualquier problema encontrado y cómo se resolvió]
```

---

## 🤖 INSTRUCCIONES PARA AGENTE AUTÓNOMO

**CONTEXTO:**
- Proyecto: MiGente En Línea (Clean Architecture)
- Database: SQL Server (`db_a9f8ff_migente` en localhost,1433)
- Workspace: `C:\Users\ray\OneDrive\Documents\ProyectoMigente\`

**AUTORIZACIÓN:**
- ✅ LEER todos los archivos de configuración en `Configurations/`
- ✅ MODIFICAR archivos de configuración existentes para corregir relaciones
- ✅ CREAR nuevos archivos de configuración si falta alguna relación
- ✅ EJECUTAR `dotnet build` para validar
- ✅ GENERAR migrations temporales SOLO para validación (NO aplicarlas)
- ❌ NO APLICAR migrations (`dotnet ef database update`) sin autorización explícita

**WORKFLOW:**

1. **LEER** todas las configuraciones en `Configurations/` folder
2. **COMPARAR** con las 9 relaciones listadas en este documento
3. **IDENTIFICAR** relaciones faltantes o incorrectas
4. **CORREGIR/CREAR** configuraciones usando el patrón mostrado arriba
5. **VALIDAR** con `dotnet build` (debe ser 0 errors)
6. **GENERAR** migration temporal para ver diferencias: `dotnet ef migrations add RelationshipsValidation`
7. **REVISAR** el archivo de migration generado
8. **ELIMINAR** la migration: `dotnet ef migrations remove --force`
9. **REPORTAR** resultados en `DATABASE_RELATIONSHIPS_REPORT.md`

**COMENZAR EJECUCIÓN AUTOMÁTICA AHORA.**

---

## 📞 CONTACTO

Si encuentras errores o necesitas clarificación, detener ejecución y reportar:
- Archivo con problema
- Línea de código específica
- Error message completo
- Tu análisis del problema

---

_Última actualización: 12 de octubre, 2025_
