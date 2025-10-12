# Script de Setup para Migracion a Code-First
# Este script automatiza la creacion de la nueva solucion Clean Architecture

param(
    [string]$SolutionPath = "C:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea.Clean",
    [string]$DbServer = ".",
    [string]$DbName = "migenteV2",
    [string]$DbUser = "sa",
    [string]$DbPassword = "1234"
)

Write-Host "Iniciando Setup de Migracion a Code-First..." -ForegroundColor Cyan
Write-Host ""

# Verificar que dotnet esta instalado
Write-Host "Verificando .NET SDK..." -ForegroundColor Yellow
$dotnetVersion = dotnet --version
if ($LASTEXITCODE -ne 0) {
    Write-Host "Error: .NET SDK no esta instalado" -ForegroundColor Red
    exit 1
}
Write-Host "  .NET SDK version: $dotnetVersion" -ForegroundColor Green
Write-Host ""

# Crear directorio de solucion
Write-Host "Creando estructura de directorios..." -ForegroundColor Yellow
if (-not (Test-Path $SolutionPath)) {
    New-Item -ItemType Directory -Path $SolutionPath | Out-Null
    Write-Host "  Directorio creado: $SolutionPath" -ForegroundColor Green
} else {
    Write-Host "  Directorio ya existe: $SolutionPath" -ForegroundColor Yellow
}
Set-Location $SolutionPath
Write-Host ""

# Crear solucion
Write-Host "Creando solucion..." -ForegroundColor Yellow
dotnet new sln -n MiGenteEnLinea.Clean -o . --force
Write-Host "  Solucion creada" -ForegroundColor Green
Write-Host ""

# Crear proyectos
Write-Host "Creando proyectos..." -ForegroundColor Yellow

Write-Host "  Domain (Core/Entities/ValueObjects)..." -ForegroundColor Cyan
dotnet new classlib -n MiGenteEnLinea.Domain -f net8.0 -o src/Core/MiGenteEnLinea.Domain
dotnet sln add src/Core/MiGenteEnLinea.Domain/MiGenteEnLinea.Domain.csproj

Write-Host "  Application (UseCases/DTOs/Validators)..." -ForegroundColor Cyan
dotnet new classlib -n MiGenteEnLinea.Application -f net8.0 -o src/Core/MiGenteEnLinea.Application
dotnet sln add src/Core/MiGenteEnLinea.Application/MiGenteEnLinea.Application.csproj

Write-Host "  Infrastructure (EF Core/Identity/Services)..." -ForegroundColor Cyan
dotnet new classlib -n MiGenteEnLinea.Infrastructure -f net8.0 -o src/Infrastructure/MiGenteEnLinea.Infrastructure
dotnet sln add src/Infrastructure/MiGenteEnLinea.Infrastructure/MiGenteEnLinea.Infrastructure.csproj

Write-Host "  API (Controllers/Middleware)..." -ForegroundColor Cyan
dotnet new webapi -n MiGenteEnLinea.API -f net8.0 -o src/Presentation/MiGenteEnLinea.API --use-controllers
dotnet sln add src/Presentation/MiGenteEnLinea.API/MiGenteEnLinea.API.csproj

Write-Host "  Proyectos creados" -ForegroundColor Green
Write-Host ""

# Configurar referencias entre proyectos
Write-Host "Configurando referencias entre proyectos..." -ForegroundColor Yellow
dotnet add src/Core/MiGenteEnLinea.Application/MiGenteEnLinea.Application.csproj reference src/Core/MiGenteEnLinea.Domain/MiGenteEnLinea.Domain.csproj
dotnet add src/Infrastructure/MiGenteEnLinea.Infrastructure/MiGenteEnLinea.Infrastructure.csproj reference src/Core/MiGenteEnLinea.Application/MiGenteEnLinea.Application.csproj
dotnet add src/Presentation/MiGenteEnLinea.API/MiGenteEnLinea.API.csproj reference src/Core/MiGenteEnLinea.Application/MiGenteEnLinea.Application.csproj
dotnet add src/Presentation/MiGenteEnLinea.API/MiGenteEnLinea.API.csproj reference src/Infrastructure/MiGenteEnLinea.Infrastructure/MiGenteEnLinea.Infrastructure.csproj
Write-Host "  Referencias configuradas" -ForegroundColor Green
Write-Host ""

# Instalar paquetes NuGet
Write-Host "Instalando paquetes NuGet..." -ForegroundColor Yellow

Write-Host "  Domain packages..." -ForegroundColor Cyan
dotnet add src/Core/MiGenteEnLinea.Domain/MiGenteEnLinea.Domain.csproj package FluentValidation --version 11.9.0

Write-Host "  Application packages..." -ForegroundColor Cyan
dotnet add src/Core/MiGenteEnLinea.Application/MiGenteEnLinea.Application.csproj package MediatR --version 12.2.0
dotnet add src/Core/MiGenteEnLinea.Application/MiGenteEnLinea.Application.csproj package AutoMapper --version 12.0.1
dotnet add src/Core/MiGenteEnLinea.Application/MiGenteEnLinea.Application.csproj package FluentValidation.DependencyInjectionExtensions --version 11.9.0

Write-Host "  Infrastructure packages..." -ForegroundColor Cyan
dotnet add src/Infrastructure/MiGenteEnLinea.Infrastructure/MiGenteEnLinea.Infrastructure.csproj package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add src/Infrastructure/MiGenteEnLinea.Infrastructure/MiGenteEnLinea.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Tools --version 8.0.0
dotnet add src/Infrastructure/MiGenteEnLinea.Infrastructure/MiGenteEnLinea.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Design --version 8.0.0
dotnet add src/Infrastructure/MiGenteEnLinea.Infrastructure/MiGenteEnLinea.Infrastructure.csproj package BCrypt.Net-Next --version 4.0.3
dotnet add src/Infrastructure/MiGenteEnLinea.Infrastructure/MiGenteEnLinea.Infrastructure.csproj package Serilog.AspNetCore --version 8.0.0
dotnet add src/Infrastructure/MiGenteEnLinea.Infrastructure/MiGenteEnLinea.Infrastructure.csproj package Serilog.Sinks.MSSqlServer --version 6.5.0

Write-Host "  API packages..." -ForegroundColor Cyan
dotnet add src/Presentation/MiGenteEnLinea.API/MiGenteEnLinea.API.csproj package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0
dotnet add src/Presentation/MiGenteEnLinea.API/MiGenteEnLinea.API.csproj package AspNetCoreRateLimit --version 5.0.0
dotnet add src/Presentation/MiGenteEnLinea.API/MiGenteEnLinea.API.csproj package Swashbuckle.AspNetCore --version 6.5.0

Write-Host "  Paquetes instalados" -ForegroundColor Green
Write-Host ""

# Instalar EF Core Tools global
Write-Host "Instalando Entity Framework Core Tools..." -ForegroundColor Yellow
dotnet tool install --global dotnet-ef --version 8.0.0 2>&1 | Out-Null
if ($LASTEXITCODE -eq 0) {
    Write-Host "  dotnet-ef instalado globalmente" -ForegroundColor Green
} else {
    Write-Host "  dotnet-ef ya esta instalado (puedes ignorar esto)" -ForegroundColor Yellow
}
Write-Host ""

# Crear estructura de carpetas en Domain
Write-Host "Creando estructura de carpetas en Domain..." -ForegroundColor Yellow
$domainFolders = @(
    "src/Core/MiGenteEnLinea.Domain/Entities",
    "src/Core/MiGenteEnLinea.Domain/Entities/Authentication",
    "src/Core/MiGenteEnLinea.Domain/Entities/Empleadores",
    "src/Core/MiGenteEnLinea.Domain/Entities/Contratistas",
    "src/Core/MiGenteEnLinea.Domain/Entities/Nomina",
    "src/Core/MiGenteEnLinea.Domain/Entities/Contrataciones",
    "src/Core/MiGenteEnLinea.Domain/Entities/Calificaciones",
    "src/Core/MiGenteEnLinea.Domain/Entities/Suscripciones",
    "src/Core/MiGenteEnLinea.Domain/Entities/Pagos",
    "src/Core/MiGenteEnLinea.Domain/Entities/Catalogos",
    "src/Core/MiGenteEnLinea.Domain/ValueObjects",
    "src/Core/MiGenteEnLinea.Domain/Enums",
    "src/Core/MiGenteEnLinea.Domain/Interfaces"
)

foreach ($folder in $domainFolders) {
    if (-not (Test-Path $folder)) {
        New-Item -ItemType Directory -Path $folder -Force | Out-Null
    }
}
Write-Host "  Estructura de Domain creada" -ForegroundColor Green
Write-Host ""

# Crear estructura de carpetas en Application
Write-Host "Creando estructura de carpetas en Application..." -ForegroundColor Yellow
$appFolders = @(
    "src/Core/MiGenteEnLinea.Application/Common/Interfaces",
    "src/Core/MiGenteEnLinea.Application/Common/Behaviors",
    "src/Core/MiGenteEnLinea.Application/Common/Exceptions",
    "src/Core/MiGenteEnLinea.Application/Features/Authentication/Commands",
    "src/Core/MiGenteEnLinea.Application/Features/Authentication/Queries",
    "src/Core/MiGenteEnLinea.Application/Features/Authentication/DTOs",
    "src/Core/MiGenteEnLinea.Application/Features/Authentication/Validators",
    "src/Core/MiGenteEnLinea.Application/Features/Empleadores",
    "src/Core/MiGenteEnLinea.Application/Features/Contratistas",
    "src/Core/MiGenteEnLinea.Application/Features/Nominas",
    "src/Core/MiGenteEnLinea.Application/Features/Suscripciones"
)

foreach ($folder in $appFolders) {
    if (-not (Test-Path $folder)) {
        New-Item -ItemType Directory -Path $folder -Force | Out-Null
    }
}
Write-Host "  Estructura de Application creada" -ForegroundColor Green
Write-Host ""

# Crear estructura de carpetas en Infrastructure
Write-Host "Creando estructura de carpetas en Infrastructure..." -ForegroundColor Yellow
$infraFolders = @(
    "src/Infrastructure/MiGenteEnLinea.Infrastructure/Persistence/Contexts",
    "src/Infrastructure/MiGenteEnLinea.Infrastructure/Persistence/Configurations",
    "src/Infrastructure/MiGenteEnLinea.Infrastructure/Persistence/Repositories",
    "src/Infrastructure/MiGenteEnLinea.Infrastructure/Persistence/Migrations",
    "src/Infrastructure/MiGenteEnLinea.Infrastructure/Persistence/Entities/Generated",
    "src/Infrastructure/MiGenteEnLinea.Infrastructure/Identity/Services",
    "src/Infrastructure/MiGenteEnLinea.Infrastructure/Services"
)

foreach ($folder in $infraFolders) {
    if (-not (Test-Path $folder)) {
        New-Item -ItemType Directory -Path $folder -Force | Out-Null
    }
}
Write-Host "  Estructura de Infrastructure creada" -ForegroundColor Green
Write-Host ""

# Compilar solucion
Write-Host "Compilando solucion..." -ForegroundColor Yellow
dotnet build --nologo --verbosity quiet
if ($LASTEXITCODE -eq 0) {
    Write-Host "  Solucion compilada exitosamente" -ForegroundColor Green
} else {
    Write-Host "  Error compilando la solucion" -ForegroundColor Red
    exit 1
}
Write-Host ""

# Preparar comando de scaffolding
Write-Host "Preparando comando de scaffolding..." -ForegroundColor Yellow
$connectionString = "Server=$DbServer;Database=$DbName;User Id=$DbUser;Password=$DbPassword;TrustServerCertificate=True"
$scaffoldCommand = "dotnet ef dbcontext scaffold `"$connectionString`" Microsoft.EntityFrameworkCore.SqlServer --output-dir Persistence/Entities/Generated --context-dir Persistence/Contexts --context MiGenteDbContext --force --data-annotations --no-onconfiguring --startup-project ../../Presentation/MiGenteEnLinea.API"

Write-Host ""
Write-Host "=============================================================" -ForegroundColor Cyan
Write-Host "Setup completado exitosamente!" -ForegroundColor Green
Write-Host "=============================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "PROXIMOS PASOS:" -ForegroundColor Yellow
Write-Host "=============================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "1. Ejecutar Scaffolding de Base de Datos:" -ForegroundColor Yellow
Write-Host "   cd src/Infrastructure/MiGenteEnLinea.Infrastructure" -ForegroundColor White
Write-Host "   $scaffoldCommand" -ForegroundColor Cyan
Write-Host ""
Write-Host "2. Revisar Entidades Generadas:" -ForegroundColor Yellow
Write-Host "   cd Persistence/Entities/Generated" -ForegroundColor White
Write-Host "   ls" -ForegroundColor White
Write-Host ""
Write-Host "3. Comenzar Refactoring con Credencial:" -ForegroundColor Yellow
Write-Host "   Copiar Generated/Credenciale.cs a Domain/Entities/Authentication/Credencial.cs" -ForegroundColor White
Write-Host "   Aplicar encapsulacion y DDD" -ForegroundColor White
Write-Host "   Crear CredencialConfiguration en Infrastructure/Persistence/Configurations/" -ForegroundColor White
Write-Host ""
Write-Host "=============================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Estructura creada en: $SolutionPath" -ForegroundColor Green
Write-Host ""
