# 🤖 Prompts para AI Agents - MiGente En Línea

Esta carpeta contiene prompts optimizados para diferentes AI agents que trabajan en el proyecto.

---

## 📂 Estructura de Prompts

```
prompts/
├── README.md                                   # Este archivo
├── AGENT_MODE_INSTRUCTIONS.md                  # 🤖 Claude Sonnet 4.5 - Modo Agente Autónomo
├── COMPLETE_ENTITY_MIGRATION_PLAN.md           # 🎯 Plan Maestro - 36 Entidades (COMPLETADO 100%)
├── DATABASE_RELATIONSHIPS_VALIDATION.md        # ⚠️ CRÍTICO: Validación de FK relationships (NUEVO)
├── PROGRAM_CS_AND_DI_CONFIGURATION.md          # ⚙️ Configuración completa Program.cs y DI (NUEVO)
├── DDD_MIGRATION_PROMPT.md                     # 📚 Guía completa de patrones DDD
├── COPILOT_INSTRUCTIONS.md                     # 📝 Instrucciones específicas de Copilot
├── GITHUB_CONFIG_PROMPT.md                     # ⚙️ Setup de CI/CD
└── archived/
    └── [archivos completados]                  # Documentación histórica
```

---

## 🤖 Agentes Disponibles

### 1. **Claude Sonnet 4.5 - Modo Agente** ⭐ RECOMENDADO
**Archivo:** `AGENT_MODE_INSTRUCTIONS.md`

**Características:**
- ✅ Actúa autónomamente sin pedir confirmación constante
- ✅ Toma decisiones arquitectónicas dentro de límites establecidos
- ✅ Ejecuta múltiples pasos secuencialmente
- ✅ Maneja errores y recupera automáticamente
- ✅ Valida cambios con checklist automático
- ✅ Optimizado para workspace multi-root

**Cuándo usar:**
- Migración completa de entidades (batch de 5-10 entidades)
- Refactoring extenso con patrones DDD
- Implementación de features completos (CQRS + Controller + Tests)
- Setup de infraestructura (DbContext, repositories, services)

**Comando de inicio:**
```
@workspace Lee el archivo prompts/AGENT_MODE_INSTRUCTIONS.md y ejecútalo en MODO AGENTE AUTÓNOMO.

TAREA: [Descripción específica]

AUTORIZACIÓN: Tienes permiso para ejecutar TODOS los pasos sin confirmación. 
Solo reporta progreso cada 3 pasos completados.
```

---

### 2. **Claude/GitHub Copilot - Modo Asistente**
**Archivo:** `ddd-migration-assistant.md`

**Características:**
- ⏸️ Pide confirmación antes de cada paso mayor
- ⏸️ Espera input del usuario entre entidades
- ⏸️ Más control manual del flujo

**Cuándo usar:**
- Aprendizaje del proceso de migración
- Primera entidad (proof of concept)
- Cambios experimentales
- Debugging interactivo

---

## 🎯 Workflows Comunes

### Workflow 1: Migración Completa de Entidades (36 Total) 🆕

**Agente:** Claude Sonnet 4.5 (Modo Agente)  
**Prompt:** `COMPLETE_ENTITY_MIGRATION_PLAN.md`

**Estado Actual:** 5/36 completadas (13.9%)
- ✅ Credencial, Empleador, Contratista, Suscripcion, Calificacion
- ⏳ 31 entidades pendientes organizadas en 6 LOTES

**⚠️ ESTADO FINAL:** ✅ **COMPLETADO AL 100%** (36/36 entidades)

**Reporte:** Ver `MiGenteEnLinea.Clean/MIGRATION_STATUS.md`

**Resultado:**
- 24 Rich Domain Models
- 9 Read Models  
- 3 Catálogos finales
- ~12,053 líneas de código
- 0 errores de compilación

---

### Workflow 2: 🔗 Validación de Relaciones de Base de Datos ⚠️ CRÍTICO

**Agente:** Claude Sonnet 4.5 (Modo Agente)  
**Prompt:** `DATABASE_RELATIONSHIPS_VALIDATION.md` (NUEVO)

**Estado:** ⚠️ **PENDIENTE - EJECUCIÓN REQUERIDA**

**Objetivo:**  
Asegurar que TODAS las relaciones de base de datos (FKs, navegación, constraints) sean **100% IDÉNTICAS** al proyecto Legacy (EDMX).

**Por qué es CRÍTICO:**
- ❌ Relaciones incorrectas → Errores en runtime al cargar navegación
- ❌ Cascadas mal configuradas → Pérdida de datos
- ❌ Discrepancias con Legacy → Comportamiento impredecible al compartir DB

**9 Relaciones a Validar:**
1. Contratistas → Contratistas_Fotos (1:N)
2. Contratistas → Contratistas_Servicios (1:N)
3. EmpleadosTemporales → DetalleContrataciones (1:N)
4. Empleador_Recibos_Header_Contrataciones → Empleador_Recibos_Detalle_Contrataciones (1:N)
5. Empleador_Recibos_Header → Empleador_Recibos_Detalle (1:N)
6. EmpleadosTemporales → Empleador_Recibos_Header_Contrataciones (1:N)
7. Empleados → Empleador_Recibos_Header (1:N)
8. Cuentas → perfilesInfo (1:N) - Legacy
9. Planes_empleadores → Suscripciones (1:N)

**Comando de ejecución:**
```
@workspace Lee prompts/DATABASE_RELATIONSHIPS_VALIDATION.md

FASE CRÍTICA: Validar y configurar TODAS las relaciones de base de datos.

OBJETIVO: Asegurar paridad 100% entre Clean Architecture y Legacy (EDMX).

AUTORIZACIÓN COMPLETA: 
- Leer todas las configuraciones en Configurations/
- Modificar archivos de configuración existentes
- Crear nuevos archivos de configuración si falta
- Ejecutar dotnet build para validar
- Generar migrations temporales (NO aplicarlas) solo para validar

WORKFLOW:
1. Leer todas las configuraciones existentes
2. Comparar con las 9 relaciones del EDMX
3. Identificar faltantes o incorrectas
4. Corregir/Crear configuraciones con Fluent API
5. Validar con dotnet build (0 errors)
6. Generar migration temporal para ver diferencias
7. Eliminar migration temporal
8. Reportar en DATABASE_RELATIONSHIPS_REPORT.md

DURACIÓN ESTIMADA: 1-2 horas

COMENZAR EJECUCIÓN AUTOMÁTICA AHORA.
```

**Resultado esperado:**
- ✅ 9/9 relaciones configuradas correctamente
- ✅ dotnet build sin errores
- ✅ Migration temporal vacía (sin cambios detectados)
- ✅ Tests de navegación pasando

---

### Workflow 3: ⚙️ Configuración de Program.cs y Dependency Injection

**Agente:** Claude Sonnet 4.5 (Modo Agente)  
**Prompt:** `PROGRAM_CS_AND_DI_CONFIGURATION.md` (NUEVO)

**Estado:** ⚠️ **PENDIENTE - EJECUTAR DESPUÉS DE WORKFLOW 2**

**Prerequisito:** Workflow 2 completado ✅

**Objetivo:**  
Configurar completamente `Program.cs`, `DependencyInjection.cs` (Infrastructure y Application) para tener la API lista para ejecutar.

**Qué se configura:**
- ✅ DbContext con connection string correcto
- ✅ Assembly Scanning para Fluent API configurations
- ✅ Serilog para logging estructurado (archivo + consola + DB)
- ✅ MediatR para CQRS (Application layer)
- ✅ FluentValidation y AutoMapper
- ✅ ICurrentUserService, IPasswordHasher (BCrypt)
- ✅ Audit Interceptor
- ✅ CORS policies (Development y Production)
- ✅ Swagger con documentación
- ✅ Health check endpoint
- ✅ appsettings.json con todos los settings

**Comando de ejecución:**
```
@workspace Lee prompts/PROGRAM_CS_AND_DI_CONFIGURATION.md

FASE 2: Configurar Program.cs y Dependency Injection completo.

PREREQUISITO VERIFICADO: DATABASE_RELATIONSHIPS_VALIDATION.md completado.

AUTORIZACIÓN COMPLETA:
- Instalar packages NuGet (MediatR, Serilog, etc)
- Crear Application/DependencyInjection.cs
- Reemplazar Program.cs completo
- Actualizar Infrastructure/DependencyInjection.cs
- Modificar appsettings.json
- Ejecutar dotnet build y dotnet run para validar

WORKFLOW:
1. Instalar packages faltantes
2. Crear DependencyInjection.cs en Application
3. Reemplazar Program.cs con configuración completa
4. Actualizar Infrastructure/DependencyInjection.cs
5. Configurar appsettings.json
6. Validar compilación (dotnet build)
7. Ejecutar API (dotnet run)
8. Verificar Swagger en https://localhost:5001/
9. Verificar Health Check en https://localhost:5001/health
10. Reportar en PROGRAM_CS_CONFIGURATION_REPORT.md

DURACIÓN ESTIMADA: 1 hora

COMENZAR EJECUCIÓN AUTOMÁTICA AHORA.
```

**Resultado esperado:**
- ✅ dotnet build: Success (0 errors)
- ✅ dotnet run: API ejecutándose en puerto 5001
- ✅ Swagger UI funcionando correctamente
- ✅ Health check endpoint respondiendo
- ✅ Logs generándose en archivo y consola
- ✅ Todos los servicios registrados en DI

**Comando para ver progreso general:**
```
@workspace Lee prompts/COMPLETE_ENTITY_MIGRATION_PLAN.md

TAREA: Genera reporte de progreso actual
- Entidades completadas vs pendientes
- Próximo LOTE a ejecutar
- Estimación de tiempo restante
```

---

### Workflow 2: Migrar Entidades con DDD (Batch)

**Agente:** Claude Sonnet 4.5 (Modo Agente)  
**Prompt:** `AGENT_MODE_INSTRUCTIONS.md`

**Comando:**
```
@workspace Lee prompts/AGENT_MODE_INSTRUCTIONS.md y ejecuta:

TAREA: Migrar entidades [Credencial, Empleador, Contratista] con patrón DDD

AUTORIZACIÓN COMPLETA:
- Crear/modificar archivos en Domain, Infrastructure, Application
- Configurar DbContext y Fluent API
- Implementar servicios de seguridad (BCrypt)
- Ejecutar build y validar errores de compilación
- Reportar solo cuando completes cada entidad

LÍMITES:
- NO modificar base de datos
- NO modificar proyecto Legacy
- NO crear tests aún (fase posterior)
```

---

### Workflow 3: Implementar Feature con CQRS

**Agente:** Claude Sonnet 4.5 (Modo Agente)  
**Prompt:** `AGENT_MODE_INSTRUCTIONS.md` + Feature specification

**Comando:**
```
@workspace Lee prompts/AGENT_MODE_INSTRUCTIONS.md

TAREA: Implementar feature completo "Registro de Usuario"

COMPONENTES A CREAR:
1. Command: RegistrarUsuarioCommand + Handler
2. Validator: RegistrarUsuarioCommandValidator
3. Controller: AuthController con endpoint POST /api/auth/register
4. DTOs: UsuarioDto, CredencialDto
5. Mappers: AutoMapper profiles

AUTORIZACIÓN: Ejecuta todo el ciclo sin confirmación.
Reporta cuando esté listo para testing.
```

---

### Workflow 3: Setup de Infraestructura

**Agente:** Claude Sonnet 4.5 (Modo Agente)  
**Prompt:** `AGENT_MODE_INSTRUCTIONS.md`

**Comando:**
```
@workspace Lee prompts/AGENT_MODE_INSTRUCTIONS.md

TAREA: Setup completo de infraestructura para autenticación

COMPONENTES:
1. IPasswordHasher interface (Domain)
2. BCryptPasswordHasher implementation (Infrastructure)
3. JwtTokenService (Infrastructure/Identity)
4. ICurrentUserService + CurrentUserService
5. AuditableEntityInterceptor
6. Registro en DependencyInjection.cs

AUTORIZACIÓN: Ejecuta setup completo.
```

---

## 📋 Checklist Pre-Ejecución

Antes de iniciar un agente en modo autónomo, verifica:

- [ ] ✅ Workspace multi-root abierto (`MiGenteEnLinea-Workspace.code-workspace`)
- [ ] ✅ Base de datos disponible (`localhost,1433` / `db_a9f8ff_migente`)
- [ ] ✅ No hay cambios sin commitear importantes
- [ ] ✅ Branch correcto (`main` o feature branch)
- [ ] ✅ NuGet packages restaurados
- [ ] ✅ Proyecto compila antes de iniciar (`dotnet build`)

---

## 🔒 Límites de Autoridad del Agente

### ✅ EL AGENTE PUEDE (sin confirmación):

**Código:**
- Crear/modificar entidades en `Domain/`
- Crear/modificar commands/queries en `Application/`
- Crear/modificar configurations en `Infrastructure/`
- Crear/modificar controllers en `API/`
- Implementar interfaces y servicios
- Actualizar `DbContext` y registros de DI

**Build & Validación:**
- Ejecutar `dotnet build`
- Ejecutar `dotnet test`
- Validar errores de compilación
- Corregir errores de sintaxis automáticamente

**Git:**
- Ejecutar `git status` para ver cambios
- Crear commits con mensajes descriptivos

### ⛔ EL AGENTE NO PUEDE (requiere confirmación):

**Base de Datos:**
- Ejecutar migraciones (`dotnet ef database update`)
- Modificar datos en la base de datos
- Drop/recreate database

**Proyecto Legacy:**
- Modificar código en `Codigo Fuente Mi Gente/`
- Cambiar configuración de Web.config
- Tocar entidades de EF6

**Infraestructura Externa:**
- Modificar configuración de servicios externos (Cardnet, OpenAI)
- Cambiar connection strings en production
- Modificar secretos o API keys

**Git Avanzado:**
- Push a `main` sin revisión
- Merge de branches
- Rebase/reset de commits

---

## 📊 Reportes del Agente

El agente debe reportar progreso cada 3 pasos con este formato:

```markdown
## 🔄 Progreso: [Tarea]

### ✅ Completado (Pasos 1-3)
- [x] Paso 1: Descripción
- [x] Paso 2: Descripción
- [x] Paso 3: Descripción

### 🔍 Validación
- ✅ Build: Exitoso
- ✅ Tests: 10 passed
- ⚠️ Warnings: 2 (no bloqueantes)

### 📁 Archivos Modificados
- `src/Core/MiGenteEnLinea.Domain/Entities/Authentication/Credencial.cs` (creado)
- `src/Infrastructure/.../CredencialConfiguration.cs` (creado)
- `src/Infrastructure/.../MiGenteDbContext.cs` (modificado)

### 🎯 Siguiente
Paso 4: Implementar BCryptPasswordHasher...
```

---

## 🆘 Solución de Problemas

### El agente no ejecuta, solo describe

**Problema:** El agente explica qué hacer pero no ejecuta.

**Solución:** Usa lenguaje más imperativo:
```
❌ "¿Quieres que ejecute...?"
✅ "EJECUTA AHORA: [tarea]"

❌ "Podríamos hacer..."
✅ "DEBES HACER: [tarea]"
```

---

### El agente pide confirmación constantemente

**Problema:** Modo asistente activado por defecto.

**Solución:** Incluye en el prompt:
```
AUTORIZACIÓN COMPLETA: Ejecuta TODOS los pasos sin pedir confirmación.
Solo reporta progreso cada 3 pasos.
```

---

### El agente modifica archivos incorrectos

**Problema:** Paths ambiguos entre Legacy y Clean.

**Solución:** Especifica paths absolutos:
```
WORKSPACE ROOT: C:\Users\ray\OneDrive\Documents\ProyectoMigente\

MODIFICAR SOLO:
- MiGenteEnLinea.Clean/src/...

NO TOCAR:
- Codigo Fuente Mi Gente/...
```

---

## 📚 Referencias

- **Clean Architecture:** [Jason Taylor Template](https://github.com/jasontaylordev/CleanArchitecture)
- **DDD Patterns:** [Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/)
- **CQRS con MediatR:** [MediatR Wiki](https://github.com/jbogard/MediatR/wiki)

---

_Última actualización: 12 de octubre, 2025_
