# 🛠️ Scripts de Migración - MiGente En Línea

Este directorio contiene scripts automatizados para facilitar la migración de Database-First a Code-First con Clean Architecture.

## 📜 Scripts Disponibles

### `setup-codefirst-migration.ps1`

Script principal que automatiza la creación de la nueva solución Clean Architecture.

#### ¿Qué hace este script?

1. ✅ Verifica instalación de .NET SDK
2. 📁 Crea estructura de directorios para Clean Architecture
3. 🏗️ Genera solución y proyectos (.NET 8)
4. 🔗 Configura referencias entre proyectos
5. 📦 Instala todos los paquetes NuGet necesarios
6. 🔧 Instala Entity Framework Core Tools (dotnet-ef)
7. 📂 Crea estructura de carpetas por capas (Domain, Application, Infrastructure)
8. 🔨 Compila la solución
9. 📋 Muestra próximos pasos

#### Uso Básico

```powershell
# Ejecutar con configuración por defecto
.\scripts\setup-codefirst-migration.ps1
```

#### Uso Avanzado (Parámetros Personalizados)

```powershell
# Cambiar ruta de solución
.\scripts\setup-codefirst-migration.ps1 -SolutionPath "C:\MiProyecto\NuevaSolucion"

# Cambiar credenciales de base de datos
.\scripts\setup-codefirst-migration.ps1 `
    -DbServer "localhost" `
    -DbName "migenteV2" `
    -DbUser "mi_usuario" `
    -DbPassword "mi_password"

# Combinar múltiples parámetros
.\scripts\setup-codefirst-migration.ps1 `
    -SolutionPath "C:\Custom\Path" `
    -DbServer "192.168.1.100" `
    -DbName "produccion_db" `
    -DbUser "admin" `
    -DbPassword "SecurePass123!"
```

#### Parámetros Disponibles

| Parámetro | Descripción | Valor por Defecto |
|-----------|-------------|-------------------|
| `-SolutionPath` | Ruta donde se creará la nueva solución | `C:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea.Clean` |
| `-DbServer` | Servidor SQL Server | `.` (localhost) |
| `-DbName` | Nombre de la base de datos | `migenteV2` |
| `-DbUser` | Usuario SQL Server | `sa` |
| `-DbPassword` | Contraseña SQL Server | `1234` |

## 🏗️ Estructura Creada

```
MiGenteEnLinea.Clean/
├── MiGenteEnLinea.Clean.sln
├── src/
│   ├── Core/
│   │   ├── MiGenteEnLinea.Domain/            # ✅ Entidades, Value Objects, Interfaces
│   │   │   ├── Entities/
│   │   │   │   ├── Authentication/
│   │   │   │   ├── Empleadores/
│   │   │   │   ├── Contratistas/
│   │   │   │   ├── Nomina/
│   │   │   │   ├── Contrataciones/
│   │   │   │   ├── Calificaciones/
│   │   │   │   ├── Suscripciones/
│   │   │   │   ├── Pagos/
│   │   │   │   └── Catalogos/
│   │   │   ├── ValueObjects/
│   │   │   ├── Enums/
│   │   │   └── Interfaces/
│   │   │
│   │   └── MiGenteEnLinea.Application/       # ✅ Use Cases, DTOs, Validators
│   │       ├── Common/
│   │       │   ├── Interfaces/
│   │       │   ├── Behaviors/
│   │       │   └── Exceptions/
│   │       └── Features/
│   │           ├── Authentication/
│   │           │   ├── Commands/
│   │           │   ├── Queries/
│   │           │   ├── DTOs/
│   │           │   └── Validators/
│   │           ├── Empleadores/
│   │           ├── Contratistas/
│   │           ├── Nominas/
│   │           └── Suscripciones/
│   │
│   ├── Infrastructure/
│   │   └── MiGenteEnLinea.Infrastructure/    # ✅ EF Core, Identity, External Services
│   │       ├── Persistence/
│   │       │   ├── Contexts/
│   │       │   ├── Configurations/
│   │       │   ├── Repositories/
│   │       │   ├── Migrations/
│   │       │   └── Entities/
│   │       │       └── Generated/            # 👈 Aquí se generarán las entidades
│   │       ├── Identity/
│   │       │   └── Services/
│   │       └── Services/
│   │
│   └── Presentation/
│       └── MiGenteEnLinea.API/               # ✅ Controllers, Middleware
│           ├── Controllers/
│           ├── Middleware/
│           └── Program.cs
```

## 📦 Paquetes NuGet Instalados

### Domain Layer
- `FluentValidation` (11.9.0)

### Application Layer
- `MediatR` (12.2.0) - CQRS pattern
- `AutoMapper` (12.0.1) - Object mapping
- `FluentValidation.DependencyInjectionExtensions` (11.9.0)

### Infrastructure Layer
- `Microsoft.EntityFrameworkCore.SqlServer` (8.0.0)
- `Microsoft.EntityFrameworkCore.Tools` (8.0.0)
- `Microsoft.EntityFrameworkCore.Design` (8.0.0)
- `BCrypt.Net-Next` (4.0.3) - Password hashing
- `Serilog.AspNetCore` (8.0.0) - Logging
- `Serilog.Sinks.MSSqlServer` (6.5.0)

### API Layer
- `Microsoft.AspNetCore.Authentication.JwtBearer` (8.0.0)
- `AspNetCoreRateLimit` (5.0.0)
- `Swashbuckle.AspNetCore` (6.5.0) - API documentation

## 📋 Próximos Pasos Después del Script

### 1️⃣ Ejecutar Scaffolding

```powershell
cd src/Infrastructure/MiGenteEnLinea.Infrastructure

dotnet ef dbcontext scaffold "Server=.;Database=migenteV2;User Id=sa;Password=1234;TrustServerCertificate=True" `
    Microsoft.EntityFrameworkCore.SqlServer `
    --output-dir Persistence/Entities/Generated `
    --context-dir Persistence/Contexts `
    --context MiGenteDbContext `
    --force `
    --data-annotations `
    --no-onconfiguring `
    --startup-project ../../Presentation/MiGenteEnLinea.API
```

### 2️⃣ Revisar Entidades Generadas

```powershell
cd Persistence/Entities/Generated
ls
```

Deberías ver ~45 archivos `.cs` con las entidades generadas.

### 3️⃣ Comenzar Refactoring (POC con Credencial)

**Copiar entidad generada:**
```powershell
Copy-Item "Persistence/Entities/Generated/Credenciale.cs" `
    "../../Core/MiGenteEnLinea.Domain/Entities/Authentication/Credencial.cs"
```

**Refactorizar según guía DDD** (ver `docs/MIGRATION_DATABASE_FIRST_TO_CODE_FIRST.md`)

### 4️⃣ Crear Configuración Fluent API

Crear `Infrastructure/Persistence/Configurations/CredencialConfiguration.cs`

### 5️⃣ Ejecutar Tests

```powershell
dotnet test
```

## 🔧 Troubleshooting

### Error: ".NET SDK no está instalado"

**Solución:**
```powershell
# Descargar e instalar .NET 8 SDK
# https://dotnet.microsoft.com/download/dotnet/8.0
```

### Error: "dotnet-ef no se reconoce como comando"

**Solución:**
```powershell
# Instalar herramienta global
dotnet tool install --global dotnet-ef --version 8.0.0

# Actualizar PATH
$env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User")
```

### Error al compilar: "No se encontró el proyecto"

**Solución:**
```powershell
# Verificar que estás en el directorio correcto
cd C:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea.Clean

# Restaurar paquetes
dotnet restore

# Compilar de nuevo
dotnet build
```

### Error de conexión a SQL Server

**Solución:**
```powershell
# Verificar que SQL Server está ejecutándose
Get-Service -Name "MSSQL*"

# Probar conexión
sqlcmd -S . -U sa -P 1234 -Q "SELECT @@VERSION"

# Si usas autenticación Windows, cambiar connection string:
# Server=.;Database=migenteV2;Integrated Security=True;TrustServerCertificate=True
```

### Error: "TrustServerCertificate=True" requerido

**Contexto:** SQL Server con certificados auto-firmados necesita esta opción.

**Es seguro en desarrollo local**, pero en producción debes usar certificados válidos.

## 📚 Recursos Adicionales

- **Guía Completa de Migración:** `docs/MIGRATION_DATABASE_FIRST_TO_CODE_FIRST.md`
- **Instrucciones para AI:** `.github/copilot-instructions.md`
- **Política de Seguridad:** `SECURITY.md`
- **Guía de Contribución:** `CONTRIBUTING.md`

## 🆘 Soporte

Si encuentras problemas:

1. Revisa el archivo de log generado por el script
2. Consulta la guía de migración detallada
3. Verifica los requisitos previos (SQL Server, .NET 8)
4. Crea un issue en GitHub: https://github.com/RainieryPeniaJrg/MiGenteEnlinea/issues

## 📝 Notas Importantes

- ⚠️ **NO ejecutar en producción** sin backup previo
- ⚠️ El script crea una **nueva solución separada** (no modifica el código actual)
- ✅ Puedes ejecutar el script múltiples veces (usa `--force`)
- ✅ Todos los cambios son reversibles

## 🎯 Checklist Post-Ejecución

- [ ] Script ejecutado sin errores
- [ ] Solución compila correctamente (`dotnet build`)
- [ ] Estructura de carpetas creada
- [ ] Paquetes NuGet instalados
- [ ] dotnet-ef funciona (`dotnet ef --version`)
- [ ] Listo para ejecutar scaffolding

---

**Última actualización:** 2025-10-12  
**Versión del script:** 1.0.0  
**Compatible con:** .NET 8.0, EF Core 8.0, PowerShell 5.1+
