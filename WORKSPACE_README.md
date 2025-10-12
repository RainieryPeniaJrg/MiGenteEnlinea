# 🏗️ MiGente En Línea - Multi-Root Workspace

Este workspace contiene **dos proyectos**:
1. **MiGente Legacy** - Proyecto original en ASP.NET Web Forms
2. **MiGente Clean** - Nueva implementación con Clean Architecture

---

## 📂 Estructura del Workspace

```
MiGenteEnLinea-Workspace/
├── 🔷 MiGente Legacy (Web Forms)
│   └── Codigo Fuente Mi Gente/
│       ├── MiGente.sln                          # Solución legacy
│       ├── MiGente_Front/                       # Proyecto Web Forms
│       │   ├── Data/                            # Entity Framework 6 (Database-First)
│       │   ├── Services/                        # Servicios de negocio
│       │   ├── Empleador/                       # Módulo empleadores
│       │   ├── Contratista/                     # Módulo contratistas
│       │   └── Web.config                       # Configuración IIS
│       ├── docs/                                # Documentación
│       │   └── MIGRATION_DATABASE_FIRST_TO_CODE_FIRST.md
│       ├── scripts/                             # Scripts de migración
│       ├── .github/                             # GitHub templates
│       ├── SECURITY.md
│       └── CONTRIBUTING.md
│
└── 🚀 MiGente Clean Architecture
    └── MiGenteEnLinea.Clean/
        ├── MiGenteEnLinea.Clean.sln             # Nueva solución
        ├── src/
        │   ├── Core/
        │   │   ├── MiGenteEnLinea.Domain/       # Entidades, Value Objects
        │   │   └── MiGenteEnLinea.Application/  # Use Cases, DTOs
        │   ├── Infrastructure/
        │   │   └── MiGenteEnLinea.Infrastructure/
        │   │       └── Persistence/
        │   │           ├── Contexts/
        │   │           │   └── MiGenteDbContext.cs
        │   │           ├── Entities/Generated/  # 36 entidades generadas
        │   │           └── Configurations/      # Fluent API configs
        │   └── Presentation/
        │       └── MiGenteEnLinea.API/          # ASP.NET Core Web API
        └── MIGRATION_SUCCESS_REPORT.md
```

---

## 🚀 Cómo Usar Este Workspace

### 1️⃣ Abrir el Workspace

```powershell
# Abrir VS Code con el workspace
code "C:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea-Workspace.code-workspace"
```

O desde VS Code:
- `File` → `Open Workspace from File...`
- Seleccionar `MiGenteEnLinea-Workspace.code-workspace`

### 2️⃣ Navegar Entre Proyectos

En el **Explorer** de VS Code verás dos carpetas raíz:
- 🔷 **MiGente Legacy (Web Forms)**
- 🚀 **MiGente Clean Architecture**

Puedes expandir y trabajar en ambas simultáneamente.

### 3️⃣ Ejecutar Proyectos

#### Opción A: Ejecutar Clean API (Recomendado)

**Desde VS Code:**
- `F5` o `Run` → `Start Debugging`
- Seleccionar: **"🚀 Launch Clean API"**
- Se abrirá Swagger en `https://localhost:5001/swagger`

**Desde Terminal:**
```powershell
cd "C:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea.Clean\src\Presentation\MiGenteEnLinea.API"
dotnet run
```

#### Opción B: Ejecutar Legacy Web Forms

**Desde VS Code:**
- `F5` o `Run` → `Start Debugging`
- Seleccionar: **"🔷 Launch Legacy Web Forms (IIS Express)"**
- Se abrirá en `https://localhost:44358/Login.aspx`

**Desde Visual Studio 2022:**
- Abrir `MiGente.sln`
- `F5` para ejecutar

#### Opción C: Ejecutar Ambos Simultáneamente

**Desde VS Code:**
- `F5` o `Run` → `Start Debugging`
- Seleccionar: **"🔥 Launch Both Projects"**

Esto ejecutará:
- Clean API en `https://localhost:5001`
- Legacy Web Forms en `https://localhost:44358`

---

## 🛠️ Tareas Disponibles

Presiona `Ctrl+Shift+P` (o `Cmd+Shift+P` en Mac) y escribe "Tasks: Run Task":

### Build Tasks
- ✅ **build-clean-api** - Compila la solución Clean Architecture
- ✅ **build-legacy** - Compila la solución legacy con MSBuild
- ✅ **restore-all** - Restaura paquetes de ambos proyectos

### Test Tasks
- ✅ **test-clean** - Ejecuta tests del proyecto Clean

### Entity Framework Tasks
- ✅ **ef-migrations-add** - Crea una nueva migración
- ✅ **ef-database-update** - Aplica migraciones pendientes

---

## 📊 Comparación de Arquitecturas

| Característica | Legacy (Web Forms) | Clean Architecture |
|----------------|-------------------|-------------------|
| **Framework** | .NET Framework 4.7.2 | .NET 8.0 |
| **Patrón** | Web Forms (Code-Behind) | Clean Architecture + CQRS |
| **ORM** | Entity Framework 6 (Database-First) | Entity Framework Core 8 (Code-First) |
| **UI** | ASP.NET Web Forms | ASP.NET Core Web API (REST) |
| **Autenticación** | Forms Authentication (Cookies) | JWT + Refresh Tokens |
| **Passwords** | ⚠️ Texto plano | ✅ BCrypt (work factor 12) |
| **Inyección de Dependencias** | ❌ Manual | ✅ Built-in DI |
| **Testing** | ❌ Sin tests | ✅ Unit + Integration Tests |
| **Logging** | ❌ Básico | ✅ Serilog estructurado |
| **API** | SOAP Web Services | REST + Swagger |
| **Seguridad** | ⚠️ Múltiples vulnerabilidades | ✅ OWASP compliant |

---

## 🔐 Configuración de Base de Datos

Ambos proyectos se conectan a la misma base de datos:

**Legacy (Web.config):**
```xml
<connectionStrings>
  <add name="migenteEntities"
       connectionString="metadata=res://*/Data.DataModel.csdl|...;
       provider connection string='data source=localhost,1433;
       initial catalog=db_a9f8ff_migente;
       user id=sa;password=Volumen#1;...'" />
</connectionStrings>
```

**Clean (appsettings.json):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=db_a9f8ff_migente;User Id=sa;Password=Volumen#1;TrustServerCertificate=True"
  }
}
```

### ⚠️ Importante: Migración de Passwords

El proyecto legacy almacena passwords en **texto plano**. El proyecto Clean usa **BCrypt**.

**NO ejecutar ambos en producción simultáneamente** hasta completar la migración de passwords.

---

## 🎯 Workflow de Desarrollo Recomendado

### Fase 1: Análisis y Refactoring (Actual)

1. ✅ **Tener ambos proyectos abiertos** para comparar implementaciones
2. ✅ **Copiar lógica de negocio** del legacy al Clean
3. ✅ **Refactorizar con DDD** en el proyecto Clean
4. ✅ **Escribir tests** para la nueva implementación

### Fase 2: Migración Gradual (Próxima)

1. 🔄 **Migrar módulo por módulo** (empezar con Authentication)
2. 🔄 **Mantener legacy funcionando** mientras se migra
3. 🔄 **Feature flags** para cambiar entre legacy y Clean
4. 🔄 **Validar que ambos producen mismos resultados**

### Fase 3: Deprecación del Legacy

1. ⏳ **Redirigir todo el tráfico** al Clean API
2. ⏳ **Monitorear errores** y performance
3. ⏳ **Archivar proyecto legacy** después de 30 días sin issues
4. ⏳ **Celebrar** 🎉

---

## 🔍 Navegación Rápida

### Archivos Clave del Legacy

```
Codigo Fuente Mi Gente/
├── MiGente_Front/
│   ├── Login.aspx.cs                # Punto de entrada
│   ├── Comunity1.Master.cs          # Layout de Empleadores
│   ├── ContratistaM.Master.cs       # Layout de Contratistas
│   ├── Services/
│   │   ├── LoginService.cs          # ⚠️ SQL Injection
│   │   ├── EmpleadosService.cs      # Lógica de empleados
│   │   └── SuscripcionesService.cs  # Lógica de suscripciones
│   └── Data/
│       ├── DataModel.edmx           # Modelo EF6
│       ├── Credenciales.cs          # ⚠️ Password en texto plano
│       ├── Empleados.cs
│       └── Suscripciones.cs
```

### Archivos Clave del Clean

```
MiGenteEnLinea.Clean/
├── src/Core/MiGenteEnLinea.Domain/
│   └── Entities/Authentication/
│       └── Credencial.cs            # ✅ Entidad refactorizada con DDD
├── src/Infrastructure/
│   └── Persistence/
│       ├── Contexts/
│       │   └── MiGenteDbContext.cs  # ✅ DbContext Code-First
│       ├── Entities/Generated/      # 36 entidades scaffolded
│       │   ├── Credenciale.cs       # Generada desde DB
│       │   ├── Empleado.cs
│       │   └── Suscripcione.cs
│       └── Configurations/
│           └── CredencialConfiguration.cs  # Fluent API (pendiente)
└── src/Presentation/MiGenteEnLinea.API/
    └── Program.cs                   # Punto de entrada
```

---

## 📚 Documentación Relacionada

### En el Proyecto Legacy
- 📄 `docs/MIGRATION_DATABASE_FIRST_TO_CODE_FIRST.md` - Guía completa de migración
- 📄 `scripts/README.md` - Documentación de scripts de automatización
- 📄 `SECURITY.md` - Vulnerabilidades identificadas y plan de remediación
- 📄 `CONTRIBUTING.md` - Guía de contribución con estándares DDD

### En el Proyecto Clean
- 📄 `MIGRATION_SUCCESS_REPORT.md` - Reporte de migración completada
- 📄 `README.md` - Documentación del proyecto Clean (pendiente)

---

## 🎨 Extensiones Recomendadas de VS Code

El workspace recomienda automáticamente estas extensiones:

### Esenciales
- ✅ **C# Dev Kit** - Soporte completo para C#
- ✅ **NuGet Gallery** - Gestión de paquetes
- ✅ **GitLens** - Git supercharged
- ✅ **GitHub Copilot** - AI coding assistant

### Productividad
- ✅ **Todo Tree** - Visualizar TODOs en el código
- ✅ **Coverage Gutters** - Cobertura de tests visual
- ✅ **Material Icon Theme** - Iconos mejorados
- ✅ **Indent Rainbow** - Indentación visual

### Testing
- ✅ **.NET Test Explorer** - Ejecutar tests desde UI

---

## 🐛 Debugging Tips

### Debugging en Clean API

**Breakpoints en:**
- Controllers: `MiGenteEnLinea.API/Controllers/`
- Handlers: `MiGenteEnLinea.Application/Features/*/Handlers/`
- Repositories: `MiGenteEnLinea.Infrastructure/Persistence/Repositories/`
- Entities: `MiGenteEnLinea.Domain/Entities/`

### Debugging en Legacy

**Breakpoints en:**
- Code-behind: `*.aspx.cs` files
- Services: `Services/*.cs`
- Master Pages: `*.Master.cs`

### Variables de Entorno

**Clean API (appsettings.Development.json):**
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "EnableSensitiveDataLogging": true
}
```

**Legacy (Web.config):**
```xml
<system.web>
  <compilation debug="true" targetFramework="4.7.2"/>
  <customErrors mode="Off"/>
</system.web>
```

---

## 🚨 Advertencias Importantes

### ⚠️ NO Hacer en Producción (Aún)

- ❌ **NO ejecutar Clean API en producción** hasta completar testing
- ❌ **NO migrar passwords** hasta tener backup completo
- ❌ **NO eliminar legacy** hasta validar Clean funciona 100%
- ❌ **NO compartir credentials** en código (usar secrets)

### ✅ SÍ Hacer

- ✅ **Mantener ambos proyectos sincronizados** en desarrollo
- ✅ **Ejecutar tests** antes de cualquier commit
- ✅ **Documentar cambios** en CHANGELOG.md
- ✅ **Seguir convenciones** de código establecidas

---

## 🎯 Próximos Pasos

### Esta Semana (Sprint 1)
- [ ] Refactorizar entidad `Credencial` con DDD
- [ ] Crear `CredencialConfiguration` con Fluent API
- [ ] Implementar `BCryptPasswordHasher` service
- [ ] Escribir tests de integración para Credencial
- [ ] Crear endpoint `/api/auth/register` en Clean API

### Próxima Semana (Sprint 2)
- [ ] Migrar módulo de autenticación completo
- [ ] Implementar JWT authentication
- [ ] Crear endpoints `/api/auth/login` y `/api/auth/refresh`
- [ ] Migrar passwords existentes con script SQL
- [ ] Validar ambos proyectos usan misma DB sin conflictos

### Mes 1
- [ ] Migrar módulo de Empleadores
- [ ] Migrar módulo de Contratistas
- [ ] Migrar módulo de Suscripciones
- [ ] Deprecar endpoints legacy uno por uno
- [ ] Cobertura de tests > 80%

---

## 📞 Soporte

Si encuentras problemas:

1. **Revisar documentación** en `/docs`
2. **Consultar MIGRATION_SUCCESS_REPORT.md**
3. **Buscar en issues de GitHub**
4. **Crear nuevo issue** con etiqueta apropiada

---

## 📝 Notas Finales

Este workspace está diseñado para facilitar:
- ✅ **Comparación** entre arquitecturas
- ✅ **Migración gradual** sin downtime
- ✅ **Aprendizaje** de Clean Architecture
- ✅ **Refactoring** sistemático con DDD
- ✅ **Testing** exhaustivo antes de deployment

**¡Feliz coding!** 🚀

---

**Última actualización:** 12 de octubre, 2025  
**Mantenido por:** Equipo de Desarrollo MiGente
