# 📋 Resumen de Sesión - Validación de Relaciones de Base de Datos

**Fecha:** 2025-01-21  
**Objetivo:** Crear prompts comprehensivos para validar relaciones de DB y configurar Program.cs  
**Estado:** ✅ COMPLETADO - Prompts listos para ejecución

---

## 🎯 OBJETIVO DE LA SESIÓN

El usuario identificó que después de migrar las 36 entidades (100% completado), los dos próximos pasos críticos son:

1. **CRÍTICO (Prioridad 1):** Validar que TODAS las relaciones de base de datos (FK, navegación, constraints) sean **100% idénticas** al proyecto Legacy (EDMX)
   - Por qué es crítico: Ambos proyectos comparten la misma base de datos
   - Riesgo: Relaciones incorrectas → errores en runtime, pérdida de datos, comportamiento impredecible

2. **Alto (Prioridad 2):** Configurar completamente `Program.cs`, `DependencyInjection.cs` y toda la infraestructura de la aplicación
   - Por qué después de #1: El API no arrancará correctamente si las relaciones están mal

---

## 📊 ANÁLISIS REALIZADO

### 1. Análisis del DataModel.edmx (Legacy)
**Archivo:** `Codigo Fuente Mi Gente/MiGente_Front/Data/DataModel.edmx`  
**Tamaño:** 2,432 líneas XML  
**Contenido extraído:**

#### 9 FK Relationships identificadas:

| # | Relación | Principal → Dependiente | FK Column | Constraint Name | Estado |
|---|----------|-------------------------|-----------|-----------------|--------|
| 1 | Contratistas → Contratistas_Fotos | 1:N | contratistaID | FK_Contratistas_Fotos_Contratistas | ✅ Configurado |
| 2 | Contratistas → Contratistas_Servicios | 1:N | contratistaID | FK_Contratistas_Servicios_Contratistas | ✅ Configurado |
| 3 | EmpleadosTemporales → DetalleContrataciones | 1:N | contratacionID | FK_DetalleContrataciones_EmpleadosTemporales | ⚠️ Requiere validación |
| 4 | Empleador_Recibos_Header_Contrataciones → Empleador_Recibos_Detalle_Contrataciones | 1:N | pagoID | FK_Empleador_Recibos_Detalle_Contrataciones_Empleador_Recibos_Header_Contrataciones | ⚠️ Requiere validación |
| 5 | Empleador_Recibos_Header → Empleador_Recibos_Detalle | 1:N | pagoID | FK_Empleador_Recibos_Detalle_Empleador_Recibos_Header | ✅ Configurado |
| 6 | EmpleadosTemporales → Empleador_Recibos_Header_Contrataciones | 1:N | contratacionID | FK_Empleador_Recibos_Header_Contrataciones_EmpleadosTemporales | ⚠️ Requiere validación |
| 7 | Empleados → Empleador_Recibos_Header | 1:N | empleadoID | FK_Empleador_Recibos_Header_Empleados | ✅ Configurado |
| 8 | Cuentas → perfilesInfo | 1:N | cuentaID | FK_perfilesInfo_Cuentas | ⚠️ Requiere configuración |
| 9 | Planes_empleadores → Suscripciones | 1:N | planID | FK_Suscripciones_Planes_empleadores | ✅ Configurado |

**Resumen:**
- ✅ **5 relaciones configuradas**
- ⚠️ **4 relaciones requieren validación/configuración**

### 2. Evaluación de Clean Architecture

**DbContext (`MiGenteDbContext.cs`):**
- ✅ 36 DbSets registrados (todos presentes)
- ✅ Assembly scanning habilitado: `modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())`
- ✅ Legacy mappings comentados (no interfieren)

**Configuraciones (`Configurations/` folder):**
- ✅ 27 archivos de configuración presentes
- ✅ Todos los archivos usan `IEntityTypeConfiguration<T>`
- ⚠️ Necesita validación: Asegurar que cada configuración tiene `.HasForeignKey()`, `.HasConstraintName()` correctos

**Program.cs:**
- ❌ Minimal (10 líneas)
- ❌ Falta: Serilog, CORS, Health checks, HttpContextAccessor, JSON config

**DependencyInjection.cs (Infrastructure):**
- ✅ DbContext registrado con AuditInterceptor
- ✅ BCrypt, CurrentUserService registrados
- ❌ Falta: Repositories, External Services

**DependencyInjection.cs (Application):**
- ❌ No existe
- ❌ Falta: MediatR, FluentValidation, AutoMapper

---

## 📝 PROMPTS CREADOS

### Prompt 1: DATABASE_RELATIONSHIPS_VALIDATION.md
**Ubicación:** `prompts/DATABASE_RELATIONSHIPS_VALIDATION.md`  
**Tamaño:** 580+ líneas  
**Propósito:** Validar y configurar todas las relaciones de DB para paridad 100% con Legacy

**Contenido incluido:**
1. **Inventario completo de EDMX relationships:**
   - 9 asociaciones con XML snippets completos
   - Nombres de constraint exactos
   - Columnas FK
   - Multiplicidad (1:N, 1:0..1, etc.)
   - Roles Principal/Dependiente

2. **Tabla de estado de relaciones:**
   - 5 configuradas ✅
   - 4 requieren validación ⚠️

3. **Patrones de Fluent API:**
   - `.HasOne()` / `.WithMany()` / `.WithOptional()`
   - `.HasForeignKey()`
   - `.HasConstraintName()` (CRÍTICO para paridad con EDMX)
   - `.OnDelete()` (DeleteBehavior)

4. **Guías de DeleteBehavior:**
   - `Cascade` - Eliminar registros relacionados automáticamente
   - `Restrict` - Prevenir eliminación si hay registros relacionados
   - `SetNull` - Setear FK a NULL en registros relacionados
   - `NoAction` - No hacer nada (default SQL Server)

5. **Workflow de validación:**
   ```
   1. Leer todas las configuraciones existentes
   2. Comparar con las 9 relaciones del EDMX
   3. Identificar faltantes o incorrectas
   4. Corregir/Crear configuraciones con Fluent API
   5. Validar con dotnet build (0 errors)
   6. Generar migration temporal para ver diferencias
   7. Eliminar migration temporal
   8. Reportar en DATABASE_RELATIONSHIPS_REPORT.md
   ```

6. **Comandos dotnet:**
   - `dotnet build`
   - `dotnet ef migrations add ValidationCheck --project Infrastructure --startup-project API`
   - `dotnet ef migrations remove --project Infrastructure --startup-project API`

7. **Template de documentación:**
   - Estructura de `DATABASE_RELATIONSHIPS_REPORT.md`
   - Qué reportar: relaciones validadas, cambios hechos, constraint names verificados

8. **Autorización del agente:**
   - ✅ Leer configuraciones
   - ✅ Modificar configuraciones
   - ✅ Crear configuraciones nuevas
   - ✅ Ejecutar dotnet build
   - ✅ Generar migrations temporales
   - ❌ NO aplicar migraciones a la DB

---

### Prompt 2: PROGRAM_CS_AND_DI_CONFIGURATION.md
**Ubicación:** `prompts/PROGRAM_CS_AND_DI_CONFIGURATION.md`  
**Tamaño:** 680+ líneas  
**Propósito:** Configurar completamente Program.cs y todos los layers de Dependency Injection

**Contenido incluido:**
1. **Program.cs completo (reemplazo total - 200+ líneas):**
   - Serilog (consola, archivo, base de datos)
   - CORS policies (Development y Production)
   - Swagger con JWT authorization
   - Health checks
   - HttpContextAccessor
   - JSON serialization config (camelCase, enums, referencias circulares)
   - Global Exception Handler
   - HTTPS Redirection
   - Authentication & Authorization

2. **Infrastructure/DependencyInjection.cs actualizado:**
   - DbContext con AuditInterceptor (ya presente)
   - Repositories (interfaces definidas, implementaciones comentadas como TODO)
   - External Services (Email, Cardnet, PDF, Storage - comentados como TODO)

3. **Application/DependencyInjection.cs (NUEVO):**
   - MediatR con assembly scanning
   - FluentValidation con assembly scanning
   - AutoMapper con assembly scanning
   - MediatR Behaviors (ValidationBehavior, LoggingBehavior - opcional)

4. **appsettings.json template:**
   - Connection string para `db_a9f8ff_migente`
   - Serilog configuration (MinimumLevel, Sinks)
   - JWT settings (SecretKey, Issuer, Audience, Expiration)
   - Cardnet payment gateway config
   - Email SMTP settings

5. **appsettings.Development.json template:**
   - Debug logging level
   - Detailed errors enabled
   - Sensitive data logging (solo Development)

6. **NuGet packages a instalar:**
   - **Application:**
     - MediatR 12.2.0
     - MediatR.Extensions.Microsoft.DependencyInjection 11.1.0
     - FluentValidation.DependencyInjectionExtensions 11.9.0
     - AutoMapper.Extensions.Microsoft.DependencyInjection 12.0.1
   - **Infrastructure:**
     - Serilog.Sinks.MSSqlServer 6.5.0
     - Serilog.Sinks.File 5.0.0
   - **API:**
     - Serilog.AspNetCore 8.0.0
     - Microsoft.AspNetCore.Authentication.JwtBearer 8.0.0

7. **Comandos de instalación:**
   ```powershell
   # Application layer
   cd MiGenteEnLinea.Clean\src\Core\MiGenteEnLinea.Application
   dotnet add package MediatR --version 12.2.0
   dotnet add package MediatR.Extensions.Microsoft.DependencyInjection --version 11.1.0
   dotnet add package FluentValidation.DependencyInjectionExtensions --version 11.9.0
   dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1

   # Infrastructure layer
   cd ..\..\Infrastructure\MiGenteEnLinea.Infrastructure
   dotnet add package Serilog.Sinks.MSSqlServer --version 6.5.0
   dotnet add package Serilog.Sinks.File --version 5.0.0

   # API layer
   cd ..\..\Presentation\MiGenteEnLinea.API
   dotnet add package Serilog.AspNetCore --version 8.0.0
   dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0
   ```

8. **Workflow de validación:**
   ```
   1. Instalar packages faltantes
   2. Crear Application/DependencyInjection.cs
   3. Reemplazar Program.cs completo
   4. Actualizar Infrastructure/DependencyInjection.cs
   5. Configurar appsettings.json
   6. Validar compilación (dotnet build)
   7. Ejecutar API (dotnet run)
   8. Verificar Swagger en https://localhost:5001/swagger
   9. Verificar Health Check en https://localhost:5001/health
   10. Verificar logs en carpeta logs/
   11. Reportar en PROGRAM_CS_CONFIGURATION_REPORT.md
   ```

9. **Troubleshooting guide:**
   - Error: "Cannot resolve service X" → Verificar registro en DI
   - Error: "Connection string not found" → Verificar appsettings.json
   - Error: "Health check endpoint not found" → Verificar MapHealthChecks en Program.cs
   - Error: "Swagger not loading" → Verificar AddSwaggerGen y UseSwagger

10. **Autorización del agente:**
    - ✅ Instalar NuGet packages
    - ✅ Crear archivos nuevos
    - ✅ Modificar Program.cs
    - ✅ Modificar DependencyInjection.cs
    - ✅ Modificar appsettings.json
    - ✅ Ejecutar dotnet build
    - ✅ Ejecutar dotnet run (para validación)
    - ❌ NO ejecutar migraciones

---

## 📂 DOCUMENTACIÓN ACTUALIZADA

### prompts/README.md
**Cambios realizados:**

1. **Estructura de Prompts actualizada:**
   ```
   prompts/
   ├── README.md                                   # Este archivo
   ├── AGENT_MODE_INSTRUCTIONS.md                  # 🤖 Claude Sonnet 4.5
   ├── COMPLETE_ENTITY_MIGRATION_PLAN.md           # 🎯 COMPLETADO 100%
   ├── DATABASE_RELATIONSHIPS_VALIDATION.md        # ⚠️ CRÍTICO: FK relationships (NUEVO)
   ├── PROGRAM_CS_AND_DI_CONFIGURATION.md          # ⚙️ Program.cs y DI (NUEVO)
   ├── DDD_MIGRATION_PROMPT.md                     # 📚 Guía DDD
   ├── COPILOT_INSTRUCTIONS.md                     # 📝 Copilot
   ├── GITHUB_CONFIG_PROMPT.md                     # ⚙️ CI/CD
   └── archived/                                   # Archivos completados
   ```

2. **Workflow 2 documentado:**
   - Título: "🔗 Validación de Relaciones de Base de Datos ⚠️ CRÍTICO"
   - Estado: Pendiente - Ejecución requerida
   - Objetivo: Paridad 100% con Legacy EDMX
   - Por qué es crítico: Ambos proyectos comparten DB
   - 9 relaciones listadas
   - Comando de ejecución incluido
   - Resultado esperado: 9/9 relaciones ✅, dotnet build sin errores

3. **Workflow 3 documentado:**
   - Título: "⚙️ Configuración de Program.cs y Dependency Injection"
   - Estado: Pendiente - Ejecutar después de Workflow 2
   - Prerequisito: Workflow 2 completado ✅
   - Objetivo: API lista para ejecutar
   - Qué se configura: DbContext, Serilog, MediatR, FluentValidation, AutoMapper, CORS, Swagger, Health checks
   - Comando de ejecución incluido
   - Resultado esperado: API ejecutándose en puerto 5001, Swagger funcionando, Health check OK

---

## 📋 ARCHIVOS CREADOS EN ESTA SESIÓN

### 1. Prompts
- ✅ `prompts/DATABASE_RELATIONSHIPS_VALIDATION.md` (580+ líneas)
- ✅ `prompts/PROGRAM_CS_AND_DI_CONFIGURATION.md` (680+ líneas)

### 2. Documentación
- ✅ `prompts/README.md` (actualizado con Workflow 2 y 3)
- ✅ `NEXT_STEPS_CRITICAL.md` (roadmap de próximos pasos)
- ✅ `SESSION_SUMMARY_DB_RELATIONSHIPS.md` (este archivo)

### 3. Archivos que serán generados por los prompts
- ⏳ `DATABASE_RELATIONSHIPS_REPORT.md` (generado por Workflow 2)
- ⏳ `PROGRAM_CS_CONFIGURATION_REPORT.md` (generado por Workflow 3)

---

## ✅ VALIDACIONES DE ÉXITO

### Para Workflow 2 (DATABASE_RELATIONSHIPS_VALIDATION.md):
```bash
# Compilación sin errores
dotnet build
# Output esperado: Build succeeded. 0 Error(s)

# Migration temporal vacía (sin cambios)
dotnet ef migrations add ValidationCheck --project Infrastructure --startup-project API
# Output esperado: No changes detected (o cambios mínimos en índices)

# Eliminar migration temporal
dotnet ef migrations remove --project Infrastructure --startup-project API

# Verificar archivo generado
ls DATABASE_RELATIONSHIPS_REPORT.md
```

**Criterios de éxito:**
- ✅ 9/9 relaciones configuradas correctamente
- ✅ Todos los constraint names coinciden con EDMX
- ✅ DeleteBehavior apropiado para cada relación
- ✅ Navigation properties en ambas direcciones
- ✅ dotnet build sin errores
- ✅ Migration temporal sin cambios o solo índices

### Para Workflow 3 (PROGRAM_CS_AND_DI_CONFIGURATION.md):
```bash
# Compilación sin errores
dotnet build
# Output esperado: Build succeeded. 0 Error(s)

# Ejecutar API
dotnet run --project src/Presentation/MiGenteEnLinea.API
# Output esperado: Now listening on: https://localhost:5001

# Verificar Swagger (en navegador)
# URL: https://localhost:5001/swagger
# Esperado: Swagger UI carga correctamente

# Verificar Health Check (en navegador o PowerShell)
curl https://localhost:5001/health
# Output esperado: Healthy

# Verificar logs
ls logs/
# Esperado: Archivos .txt con logs del día actual

# Verificar archivo generado
ls PROGRAM_CS_CONFIGURATION_REPORT.md
```

**Criterios de éxito:**
- ✅ Todos los NuGet packages instalados
- ✅ Application/DependencyInjection.cs creado
- ✅ Program.cs reemplazado completamente
- ✅ Infrastructure/DependencyInjection.cs actualizado
- ✅ appsettings.json configurado
- ✅ dotnet build sin errores
- ✅ API ejecutándose en puerto 5001
- ✅ Swagger UI funcionando
- ✅ Health check respondiendo 200 OK
- ✅ Logs generándose correctamente

---

## 🎯 PRÓXIMOS PASOS (EN ORDEN ESTRICTO)

### Paso 1: ⚠️ EJECUTAR WORKFLOW 2 (CRÍTICO)
**Comando:**
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

**⏱️ Duración estimada:** 1-2 horas  
**🤖 Agente:** Claude Sonnet 4.5 (Modo Agente)

---

### Paso 2: ⚙️ EJECUTAR WORKFLOW 3 (DESPUÉS DE PASO 1)
**Prerequisito:** ✅ Workflow 2 completado

**Comando:**
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
8. Verificar Swagger en https://localhost:5001/swagger
9. Verificar Health Check en https://localhost:5001/health
10. Reportar en PROGRAM_CS_CONFIGURATION_REPORT.md

DURACIÓN ESTIMADA: 1 hora

COMENZAR EJECUCIÓN AUTOMÁTICA AHORA.
```

**⏱️ Duración estimada:** 1 hora  
**🤖 Agente:** Claude Sonnet 4.5 (Modo Agente)

---

### Paso 3: 🔄 Implementar CQRS Commands y Queries (FUTURO)
**Prerequisito:** ✅ Workflow 2 y 3 completados

**Qué implementar:**
- Commands para operaciones de escritura (Create, Update, Delete)
- Queries para operaciones de lectura (GetById, GetAll, Search)
- Handlers para cada Command/Query
- Validators para cada Command/Query

**Orden sugerido:**
1. Authentication (Login, Register, ChangePassword, ResetPassword)
2. Empleadores (Create, Update, GetById, GetAll)
3. Contratistas (Create, Update, GetById, GetAll)
4. Empleados (Create, Update, Delete, GetById, GetAll)
5. Nominas (Create, GetById, GetAll, GetByEmpleado)

---

### Paso 4: 🎮 Implementar Controllers REST (FUTURO)
**Prerequisito:** ✅ CQRS Commands/Queries implementados

**Qué implementar:**
- AuthController (`/api/auth`)
- EmpleadoresController (`/api/empleadores`)
- ContratistasController (`/api/contratistas`)
- EmpleadosController (`/api/empleados`)
- NominasController (`/api/nominas`)

---

### Paso 5: 🧪 Implementar Tests (FUTURO)
**Prerequisito:** ✅ Controllers implementados

**Qué implementar:**
- Unit tests para Domain entities
- Unit tests para Handlers
- Integration tests para Controllers
- Unit tests para Validators

---

## 📊 ESTADO FINAL DEL PROYECTO (DESPUÉS DE ESTA SESIÓN)

### Completado (100%):
- ✅ **Entidades:** 36/36 migradas
  - 24 Rich Domain Models
  - 9 Read Models
  - 3 Catálogos
- ✅ **Código:** ~12,053 líneas
- ✅ **Errores:** 0 errores de compilación
- ✅ **Documentación:** `MiGenteEnLinea.Clean/MIGRATION_STATUS.md`
- ✅ **Prompts creados:** DATABASE_RELATIONSHIPS_VALIDATION.md, PROGRAM_CS_AND_DI_CONFIGURATION.md
- ✅ **Documentación actualizada:** prompts/README.md con Workflows 2 y 3

### En Progreso:
- ⏳ **Relaciones de DB:** Análisis completo ✅, prompts creados ✅, ejecución pendiente ⏳
- ⏳ **Configuración App:** Análisis completo ✅, prompts creados ✅, ejecución pendiente ⏳

### Bloqueado (hasta completar Workflows 2 y 3):
- 🚫 Implementación de CQRS
- 🚫 Implementación de Controllers
- 🚫 Tests de integración
- 🚫 Migración de Business Logic

---

## 🔗 ENLACES RÁPIDOS

### Archivos de esta sesión:
- [`prompts/DATABASE_RELATIONSHIPS_VALIDATION.md`](prompts/DATABASE_RELATIONSHIPS_VALIDATION.md) - 580+ líneas
- [`prompts/PROGRAM_CS_AND_DI_CONFIGURATION.md`](prompts/PROGRAM_CS_AND_DI_CONFIGURATION.md) - 680+ líneas
- [`prompts/README.md`](prompts/README.md) - Actualizado con Workflows 2 y 3
- [`NEXT_STEPS_CRITICAL.md`](NEXT_STEPS_CRITICAL.md) - Roadmap de próximos pasos
- [`SESSION_SUMMARY_DB_RELATIONSHIPS.md`](SESSION_SUMMARY_DB_RELATIONSHIPS.md) - Este archivo

### Archivos de referencia:
- [`Codigo Fuente Mi Gente/MiGente_Front/Data/DataModel.edmx`](Codigo%20Fuente%20Mi%20Gente/MiGente_Front/Data/DataModel.edmx) - EDMX Legacy (2,432 líneas)
- [`MiGenteEnLinea.Clean/src/Infrastructure/MiGenteEnLinea.Infrastructure/Persistence/Contexts/MiGenteDbContext.cs`](MiGenteEnLinea.Clean/src/Infrastructure/MiGenteEnLinea.Infrastructure/Persistence/Contexts/MiGenteDbContext.cs) - DbContext actual
- [`MiGenteEnLinea.Clean/src/Infrastructure/MiGenteEnLinea.Infrastructure/Persistence/Configurations/`](MiGenteEnLinea.Clean/src/Infrastructure/MiGenteEnLinea.Infrastructure/Persistence/Configurations/) - 27 configuraciones existentes

### Documentación del proyecto:
- [`MiGenteEnLinea.Clean/MIGRATION_STATUS.md`](MiGenteEnLinea.Clean/MIGRATION_STATUS.md) - Estado de migración de entidades
- [`.github/copilot-instructions.md`](.github/copilot-instructions.md) - Instrucciones principales del proyecto

---

## ⚠️ NOTAS IMPORTANTES

### Base de Datos Compartida
- **Nombre:** `db_a9f8ff_migente`
- **Servidor:** `localhost,1433`
- **Estado:** Compartida entre Legacy (Web Forms) y Clean (API)
- **CRÍTICO:** Cualquier cambio en el esquema afecta ambos proyectos
- **Constraint names:** DEBEN coincidir exactamente con EDMX para evitar conflictos

### Migraciones
- ❌ **NO aplicar migraciones** hasta validar relaciones 100%
- ✅ **Usar migrations temporales** solo para validar diferencias (luego eliminarlas)
- ✅ **Primera migration real** se creará después de validar todo

### NuGet Packages
Paquetes que se instalarán en Workflow 3:
- **Application:** MediatR 12.2.0, FluentValidation 11.9.0, AutoMapper 12.0.1
- **Infrastructure:** Serilog.Sinks.MSSqlServer 6.5.0, Serilog.Sinks.File 5.0.0
- **API:** Serilog.AspNetCore 8.0.0, JWT Bearer 8.0.0

---

## 📞 SOPORTE Y TROUBLESHOOTING

### Si hay errores en Workflow 2:
1. **Errores de compilación:** Verificar sintaxis de Fluent API, nombres de propiedades
2. **Errores de migration:** Comparar constraint names con EDMX (deben ser idénticos)
3. **Navegación no funciona:** Verificar `.HasOne()`, `.WithMany()`, `.HasForeignKey()`

### Si hay errores en Workflow 3:
1. **NuGet no instala:** Verificar conexión a internet, limpiar cache (`dotnet nuget locals all --clear`)
2. **Errores de compilación:** Verificar que packages se instalaron en proyectos correctos
3. **API no arranca:** Revisar logs en `logs/`, verificar connection string en appsettings.json
4. **Swagger no carga:** Verificar que `AddSwaggerGen` y `UseSwagger` están en Program.cs

### Archivos de reporte:
- `DATABASE_RELATIONSHIPS_REPORT.md` - Generado por Workflow 2 con resultados de validación
- `PROGRAM_CS_CONFIGURATION_REPORT.md` - Generado por Workflow 3 con resultados de configuración

---

**🚀 LISTO PARA COMENZAR**

Los prompts están listos para ejecución. Copia el comando de Workflow 2 y ejecútalo en Claude Sonnet 4.5 en modo agente.

---

_Sesión completada: 2025-01-21_  
_Total de líneas de prompts creados: 1,260+_  
_Total de archivos creados/modificados: 4_
