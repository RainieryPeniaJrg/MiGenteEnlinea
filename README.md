# 🏢 MiGente En Línea - Sistema de Gestión de Relaciones Laborales

[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.7.2-blue)](https://dotnet.microsoft.com/)
[![.NET Core](https://img.shields.io/badge/.NET%20Core-8.0-purple)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-Proprietary-red)](LICENSE)
[![Security Audit](https://img.shields.io/badge/Security-In%20Remediation-yellow)](SECURITY.md)
[![Migration Status](https://img.shields.io/badge/Migration-In%20Progress-orange)](docs/MIGRATION_DATABASE_FIRST_TO_CODE_FIRST.md)

---

## 📋 Descripción

**MiGente En Línea** es una plataforma integral para la gestión de relaciones laborales en la República Dominicana. Conecta **Empleadores** y **Contratistas** (proveedores de servicios) mediante un sistema de suscripciones con procesamiento de pagos integrado.

### ⚡ Características Principales

- 👥 **Gestión de Empleadores y Contratistas** - Perfiles completos con validación de documentos
- 💼 **Administración de Empleados** - Contratos, nómina, deducciones TSS
- 💰 **Sistema de Suscripciones** - Planes diferenciados con pagos recurrentes
- 🔐 **Autenticación Segura** - Sistema de usuarios con roles y permisos
- 📄 **Generación de Documentos** - Contratos, recibos de pago, certificaciones en PDF
- 💳 **Procesamiento de Pagos** - Integración con Cardnet (República Dominicana)
- ⭐ **Sistema de Calificaciones** - Reviews y ratings para contratistas
- 🤖 **Asistente Legal Virtual** - Integración con OpenAI para consultas legales

---

## 🚨 Estado Actual: Migración Dual-Project

Este workspace contiene **DOS proyectos ejecutándose simultáneamente** durante la migración:

### 🔷 Proyecto Legacy (Modo Mantenimiento)
**Ubicación:** `Codigo Fuente Mi Gente/`

- **Framework:** ASP.NET Web Forms (.NET Framework 4.7.2)
- **ORM:** Entity Framework 6 (Database-First con EDMX)
- **Base de Datos:** `db_a9f8ff_migente` en SQL Server
- **Estado:** Sistema en producción siendo reemplazado
- **Desarrollo:** Solo correcciones críticas de bugs y parches de seguridad

**⚠️ ADVERTENCIA:** Múltiples vulnerabilidades de seguridad identificadas en auditoría de septiembre 2025.

### 🚀 Proyecto Clean Architecture (Desarrollo Activo)
**Ubicación:** `../MiGenteEnLinea.Clean/`

- **Framework:** ASP.NET Core 8.0 Web API
- **Arquitectura:** Clean Architecture (Onion Pattern) con DDD
- **ORM:** Entity Framework Core 8 (Code-First)
- **Base de Datos:** `db_a9f8ff_migente` (misma base de datos, migración gradual)
- **Estado:** En construcción activa
- **Desarrollo:** Todo el desarrollo nuevo y refactorización DDD

---

## 🏗️ Arquitectura del Workspace

```
MiGenteEnLinea-Workspace/
│
├── 📂 Codigo Fuente Mi Gente/           # 🔷 LEGACY PROJECT
│   ├── MiGente.sln                       # Solución .NET Framework 4.7.2
│   ├── MiGente_Front/                    # Aplicación ASP.NET Web Forms
│   │   ├── Data/                         # Entity Framework 6 (Database-First)
│   │   ├── Services/                     # Lógica de negocio (Services layer)
│   │   ├── Empleador/                    # Módulo de empleadores
│   │   ├── Contratista/                  # Módulo de contratistas
│   │   ├── Login.aspx                    # Punto de entrada
│   │   └── Web.config                    # Configuración IIS
│   ├── docs/                             # Documentación de migración
│   ├── scripts/                          # Scripts de automatización
│   ├── .github/                          # Templates y configuración GitHub
│   ├── SECURITY.md                       # Política de seguridad
│   ├── CONTRIBUTING.md                   # Guía de contribución
│   └── README.md                         # Este archivo
│
├── 📂 MiGenteEnLinea.Clean/             # 🚀 CLEAN ARCHITECTURE PROJECT
│   ├── MiGenteEnLinea.Clean.sln          # Solución .NET 8.0
│   ├── src/
│   │   ├── Core/
│   │   │   ├── MiGenteEnLinea.Domain/              # Capa de Dominio
│   │   │   │   ├── Entities/                       # Entidades DDD (Rich Models)
│   │   │   │   ├── ValueObjects/                   # Objetos de valor inmutables
│   │   │   │   ├── Events/                         # Domain Events
│   │   │   │   └── Common/                         # Clases base (AuditableEntity, etc.)
│   │   │   │
│   │   │   └── MiGenteEnLinea.Application/         # Capa de Aplicación
│   │   │       ├── Features/                       # Use Cases (CQRS)
│   │   │       │   ├── Authentication/             # Autenticación
│   │   │       │   ├── Empleadores/                # Empleadores
│   │   │       │   └── Contratistas/               # Contratistas
│   │   │       ├── Common/                         # Interfaces, DTOs
│   │   │       └── Behaviors/                      # Pipelines MediatR
│   │   │
│   │   ├── Infrastructure/
│   │   │   └── MiGenteEnLinea.Infrastructure/      # Capa de Infraestructura
│   │   │       ├── Persistence/
│   │   │       │   ├── Contexts/                   # DbContext EF Core
│   │   │       │   ├── Entities/Generated/         # 36 entidades scaffolded
│   │   │       │   ├── Configurations/             # Fluent API
│   │   │       │   └── Migrations/                 # Migraciones EF Core
│   │   │       ├── Identity/                       # JWT, Password Hashing
│   │   │       └── Services/                       # Email, Payments, PDF
│   │   │
│   │   └── Presentation/
│   │       └── MiGenteEnLinea.API/                 # API REST
│   │           ├── Controllers/                    # Endpoints REST
│   │           ├── Middleware/                     # Middleware custom
│   │           └── Program.cs                      # Punto de entrada
│   │
│   ├── tests/                                      # Proyectos de tests
│   │   ├── MiGenteEnLinea.Domain.Tests/
│   │   ├── MiGenteEnLinea.Application.Tests/
│   │   └── MiGenteEnLinea.Infrastructure.Tests/
│   │
│   ├── MIGRATION_SUCCESS_REPORT.md                 # Reporte de migración
│   └── README.md                                   # Documentación del proyecto Clean
│
├── MiGenteEnLinea-Workspace.code-workspace         # Configuración del workspace
├── WORKSPACE_README.md                             # Guía de uso del workspace
└── DDD_MIGRATION_PROMPT.md                         # Prompt para migración DDD

```

---

## 🚀 Inicio Rápido

### Prerrequisitos

#### Para Proyecto Legacy:
- Visual Studio 2019/2022
- .NET Framework 4.7.2 SDK
- IIS Express
- SQL Server 2017+
- DevExpress v23.1 (licencia comercial requerida)

#### Para Proyecto Clean:
- Visual Studio 2022 / VS Code
- .NET 8.0 SDK
- SQL Server 2017+
- Entity Framework Core CLI tools

### 1️⃣ Clonar el Repositorio

```bash
git clone https://github.com/RainieryPeniaJrg/MiGenteEnlinea.git
cd MiGenteEnlinea
```

### 2️⃣ Configurar Base de Datos

La base de datos `db_a9f8ff_migente` debe existir y estar accesible. Verificar conexión en:

**Legacy:** `Codigo Fuente Mi Gente/MiGente_Front/Web.config`
```xml
<connectionStrings>
  <add name="migenteEntities"
       connectionString="...data source=localhost,1433;
       initial catalog=db_a9f8ff_migente;
       user id=sa;password=YOUR_PASSWORD;..." />
</connectionStrings>
```

**Clean:** `MiGenteEnLinea.Clean/src/Presentation/MiGenteEnLinea.API/appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=db_a9f8ff_migente;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True"
  }
}
```

### 3️⃣ Ejecutar Proyecto Legacy

#### Desde Visual Studio:
1. Abrir `MiGente.sln`
2. Presionar `F5` para ejecutar
3. Navegador abrirá en `https://localhost:44358/Login.aspx`

#### Desde VS Code (con workspace):
1. Abrir `MiGenteEnLinea-Workspace.code-workspace`
2. `F5` → Seleccionar "🔷 Launch Legacy Web Forms (IIS Express)"

### 4️⃣ Ejecutar Proyecto Clean

```bash
cd MiGenteEnLinea.Clean/src/Presentation/MiGenteEnLinea.API
dotnet run
```

O desde VS Code:
1. Abrir `MiGenteEnLinea-Workspace.code-workspace`
2. `F5` → Seleccionar "🚀 Launch Clean API"
3. Swagger UI abrirá en `https://localhost:5001/swagger`

### 5️⃣ Ejecutar Ambos Simultáneamente

Desde VS Code con el workspace:
- `F5` → Seleccionar "🔥 Launch Both Projects"

Esto ejecutará:
- Legacy Web Forms en `https://localhost:44358`
- Clean API en `https://localhost:5001`

---

## 📚 Documentación

### Documentación General
- [📖 Guía de Uso del Workspace](WORKSPACE_README.md) - Cómo trabajar con ambos proyectos
- [🔒 Política de Seguridad](Codigo%20Fuente%20Mi%20Gente/SECURITY.md) - Vulnerabilidades y remediación
- [🤝 Guía de Contribución](Codigo%20Fuente%20Mi%20Gente/CONTRIBUTING.md) - Estándares de código
- [📜 Código de Conducta](Codigo%20Fuente%20Mi%20Gente/CODE_OF_CONDUCT.md)

### Documentación Técnica
- [🏗️ Guía de Migración Database-First a Code-First](Codigo%20Fuente%20Mi%20Gente/docs/MIGRATION_DATABASE_FIRST_TO_CODE_FIRST.md)
- [✅ Reporte de Migración Exitosa](MiGenteEnLinea.Clean/MIGRATION_SUCCESS_REPORT.md)
- [🎯 Prompt de Migración DDD](DDD_MIGRATION_PROMPT.md) - Guía para refactorización con DDD
- [📋 Instrucciones para GitHub Copilot](Codigo%20Fuente%20Mi%20Gente/.github/copilot-instructions.md)

### Documentación de API
- Swagger UI: `https://localhost:5001/swagger` (cuando Clean API esté corriendo)

---

## 🧪 Testing

### Proyecto Clean Architecture

```bash
# Ejecutar todos los tests
cd MiGenteEnLinea.Clean
dotnet test

# Tests con cobertura
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# Tests de una capa específica
dotnet test tests/MiGenteEnLinea.Domain.Tests/
```

### Proyecto Legacy
⚠️ El proyecto legacy no tiene tests automatizados. Testing manual requerido.

---

## 🔐 Seguridad

### ⚠️ Vulnerabilidades Conocidas (Legacy)

**15 vulnerabilidades críticas identificadas en auditoría de septiembre 2025:**

#### 🔴 CRÍTICO (Remediación Inmediata)
1. **SQL Injection** - Concatenación de strings SQL
2. **Passwords en Texto Plano** - Almacenados sin hash en DB
3. **Falta de Autenticación** - Endpoints críticos sin protección
4. **Divulgación de Información** - Stack traces expuestos a clientes
5. **Credenciales Hardcodeadas** - En `Web.config`

#### 🟡 ALTO (Este Sprint)
6. **CORS Permisivo** - Política allow-all en producción
7. **Sin Rate Limiting** - Ataques de fuerza bruta posibles
8. **Validación de Inputs Faltante** - Sin framework sistemático
9. **Sin Logging de Auditoría** - Eventos de seguridad no registrados
10. **Gestión de Sesiones Insegura** - Configuración de cookies débil

Ver detalles completos en [SECURITY.md](Codigo%20Fuente%20Mi%20Gente/SECURITY.md)

### ✅ Mejoras de Seguridad (Clean Architecture)

- ✅ Passwords hasheados con BCrypt (work factor 12)
- ✅ JWT authentication con refresh tokens
- ✅ Validación de inputs con FluentValidation
- ✅ Rate limiting en endpoints críticos
- ✅ Logging estructurado con Serilog
- ✅ Manejo global de excepciones sin exponer detalles
- ✅ Secrets en User Secrets / Azure Key Vault
- ✅ CORS configurado por ambiente
- ✅ Auditoría automática con interceptors

---

## 🛠️ Stack Tecnológico

### Proyecto Legacy 🔷

| Categoría | Tecnología | Versión |
|-----------|-----------|---------|
| **Framework** | ASP.NET Web Forms | .NET 4.7.2 |
| **ORM** | Entity Framework | 6.4.4 |
| **Base de Datos** | SQL Server | 2017+ |
| **UI Components** | DevExpress | 23.1 |
| **PDF Generation** | iText | 8.0.5 |
| **Payments** | Cardnet Gateway | - |
| **AI** | OpenAI API | GPT-3.5 |
| **HTTP Client** | RestSharp | 112.1.0 |

### Proyecto Clean 🚀

| Categoría | Tecnología | Versión |
|-----------|-----------|---------|
| **Framework** | ASP.NET Core | 8.0 |
| **ORM** | Entity Framework Core | 8.0.0 |
| **Base de Datos** | SQL Server | 2017+ |
| **CQRS** | MediatR | 12.2.0 |
| **Mapping** | AutoMapper | 12.0.1 |
| **Validation** | FluentValidation | 11.9.0 |
| **Password Hashing** | BCrypt.Net-Next | 4.0.3 |
| **Logging** | Serilog | 8.0.0 |
| **Authentication** | JWT Bearer | 8.0.0 |
| **Rate Limiting** | AspNetCoreRateLimit | 5.0.0 |
| **API Docs** | Swashbuckle (Swagger) | 6.5.0 |
| **Testing** | xUnit + Moq + FluentAssertions | Latest |

---

## 📅 Timeline de Migración

### ✅ Fase 1: Preparación (Semanas 1-2) - COMPLETADO
- [x] Análisis de arquitectura legacy
- [x] Identificación de vulnerabilidades de seguridad
- [x] Creación de estructura Clean Architecture
- [x] Scaffolding de 36 entidades desde DB
- [x] Instalación de NuGet packages
- [x] Configuración de workspace multi-root

### 🔄 Fase 2: Refactorización DDD (Semanas 3-4) - EN PROGRESO
- [ ] Refactorizar entidad Credencial (🔥 Prioridad 1)
- [ ] Refactorizar entidades Empleador y Contratista
- [ ] Implementar Value Objects (Email, Money, etc.)
- [ ] Crear Fluent API configurations
- [ ] Implementar BCrypt password hasher
- [ ] Unit tests (cobertura > 80%)

### ⏳ Fase 3: CQRS & Application Layer (Semanas 5-6)
- [ ] Implementar Commands y Queries para Authentication
- [ ] Implementar Commands y Queries para Empleadores
- [ ] Implementar Commands y Queries para Contratistas
- [ ] FluentValidation para todos los inputs
- [ ] Integration tests

### ⏳ Fase 4: API & Middleware (Semanas 7-8)
- [ ] Crear controllers REST
- [ ] Implementar global exception handler
- [ ] Configurar rate limiting
- [ ] Implementar request logging
- [ ] Swagger documentation

### ⏳ Fase 5: Testing & Deployment (Semanas 9-10)
- [ ] Security testing (OWASP compliance)
- [ ] Performance testing
- [ ] Migration script para passwords
- [ ] CI/CD pipeline
- [ ] Deployment a staging

### ⏳ Fase 6: Go-Live (Semana 11+)
- [ ] Feature flags para gradual rollout
- [ ] Monitoreo en producción
- [ ] Deprecación gradual del legacy
- [ ] Documentación completa

**Estimación Total:** 11-12 semanas (~3 meses)

---

## 🤝 Contribución

### Workflow de Desarrollo

1. **Trabajar en ambos proyectos simultáneamente**
   ```bash
   code MiGenteEnLinea-Workspace.code-workspace
   ```

2. **Para bug fixes en Legacy:**
   - Branch: `hotfix/nombre-descriptivo`
   - Solo correcciones críticas
   - No agregar features nuevos

3. **Para desarrollo nuevo en Clean:**
   - Branch: `feature/nombre-descriptivo`
   - Seguir principios DDD y Clean Architecture
   - Incluir tests unitarios e integración

4. **Commit Message Convention:**
   ```
   type(scope): subject

   body (opcional)

   footer (opcional)
   ```

   **Types:** `feat`, `fix`, `refactor`, `test`, `docs`, `chore`, `security`

   **Examples:**
   ```
   feat(clean/auth): implement BCrypt password hashing
   fix(legacy): patch SQL injection in LoginService
   security(clean): add rate limiting to auth endpoints
   refactor(clean/domain): convert Credencial to rich domain model
   ```

Ver guía completa en [CONTRIBUTING.md](Codigo%20Fuente%20Mi%20Gente/CONTRIBUTING.md)

---

## 📞 Soporte

### Reportar Issues

- **Bugs en Legacy:** [Issue Template - Bug](https://github.com/RainieryPeniaJrg/MiGenteEnlinea/issues/new?template=bug_report.md)
- **Features en Clean:** [Issue Template - Feature](https://github.com/RainieryPeniaJrg/MiGenteEnlinea/issues/new?template=feature_request.md)
- **Vulnerabilidades:** [Issue Template - Security](https://github.com/RainieryPeniaJrg/MiGenteEnlinea/issues/new?template=security_vulnerability.md)

### Contacto

- **Email:** [Tu email del proyecto]
- **GitHub:** [@RainieryPeniaJrg](https://github.com/RainieryPeniaJrg)

---

## 📄 Licencia

Este proyecto es propietario. Ver [LICENSE](LICENSE) para más detalles.

---

## 🎯 Objetivos del Proyecto

### Objetivos de Negocio
- ✅ Reducir costos de infraestructura con arquitectura moderna
- ✅ Mejorar experiencia de usuario con API REST
- ✅ Facilitar integraciones con terceros
- ✅ Cumplir con estándares de seguridad internacionales

### Objetivos Técnicos
- ✅ Eliminar todas las vulnerabilidades de seguridad conocidas
- ✅ Implementar arquitectura testeable (coverage > 80%)
- ✅ Mejorar performance (reducir latencia en 50%)
- ✅ Facilitar mantenimiento con Clean Architecture y DDD

### Objetivos de Equipo
- ✅ Capacitar al equipo en Clean Architecture y DDD
- ✅ Establecer estándares de código y testing
- ✅ Implementar CI/CD pipeline
- ✅ Documentación completa para nuevos desarrolladores

---

## ⭐ Estado del Proyecto

- **Repositorio:** [MiGenteEnlinea](https://github.com/RainieryPeniaJrg/MiGenteEnlinea)
- **Estado Legacy:** 🟡 Mantenimiento
- **Estado Clean:** 🟢 Desarrollo Activo
- **Cobertura de Tests:** 0% → Target: 80%
- **Vulnerabilidades Conocidas:** 15 → Target: 0
- **Progreso de Migración:** 15% → Target: 100%

---

## 📖 Recursos Adicionales

### Aprendizaje
- [Clean Architecture - Jason Taylor](https://github.com/jasontaylordev/CleanArchitecture)
- [Domain-Driven Design Reference](https://www.domainlanguage.com/ddd/reference/)
- [CQRS Pattern - Microsoft](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs)
- [EF Core Best Practices](https://docs.microsoft.com/en-us/ef/core/performance/)

### Herramientas
- [VS Code Extensions Recomendadas](WORKSPACE_README.md#-extensiones-recomendadas-de-vs-code)
- [dotnet-ef CLI](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)
- [Swagger UI](https://localhost:5001/swagger)

---

## 📝 Changelog

Ver [CHANGELOG.md](Codigo%20Fuente%20Mi%20Gente/CHANGELOG.md) para historial detallado de cambios.

---

**Última actualización:** 12 de octubre, 2025  
**Mantenido por:** Equipo de Desarrollo MiGente  
**GitHub:** [@RainieryPeniaJrg](https://github.com/RainieryPeniaJrg)

---

<div align="center">

**🚀 Construyendo el futuro de la gestión laboral en República Dominicana 🚀**

</div>
