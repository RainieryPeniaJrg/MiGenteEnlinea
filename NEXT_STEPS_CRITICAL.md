# 🚨 PRÓXIMOS PASOS CRÍTICOS - MiGente En Línea

**Fecha:** 2025-01-21  
**Estado:** Listo para ejecutar Workflows 2 y 3

---

## ✅ LO QUE ACABAMOS DE COMPLETAR

### 1. ✅ Análisis Completo de Relaciones Legacy (EDMX)
- **Archivo analizado:** `Codigo Fuente Mi Gente/MiGente_Front/Data/DataModel.edmx` (2,432 líneas)
- **Relaciones extraídas:** 9 FK relationships con nombres de constraints, columnas FK, multiplicidad
- **Estado:** Documentado completamente en `DATABASE_RELATIONSHIPS_VALIDATION.md`

### 2. ✅ Evaluación de Clean Architecture
- **DbContext:** 36 DbSets registrados, assembly scanning habilitado
- **Configuraciones:** 27 archivos de configuración presentes en `Configurations/`
- **Estado:** 5 relaciones configuradas, 4 requieren validación

### 3. ✅ Creación de Prompts Comprehensivos
- **Prompt 1:** `DATABASE_RELATIONSHIPS_VALIDATION.md` (580+ líneas)
- **Prompt 2:** `PROGRAM_CS_AND_DI_CONFIGURATION.md` (680+ líneas)
- **Total:** 1,260+ líneas de instrucciones detalladas, código completo, validaciones

### 4. ✅ Actualización de Documentación
- **Archivo:** `prompts/README.md`
- **Añadido:** Workflow 2 (DB Relationships) y Workflow 3 (Program.cs Config)
- **Actualizado:** Estructura de prompts con nuevos archivos

---

## 🎯 PRÓXIMAS ACCIONES (EN ORDEN ESTRICTO)

### ⚠️ ACCIÓN 1: EJECUTAR DATABASE_RELATIONSHIPS_VALIDATION.md (CRÍTICO)

**⏱️ Duración estimada:** 1-2 horas  
**🤖 Agente recomendado:** Claude Sonnet 4.5 (Modo Agente)

**Por qué es CRÍTICO:**
- ❌ Ambos proyectos (Legacy y Clean) comparten la misma base de datos (`db_a9f8ff_migente`)
- ❌ Relaciones incorrectas pueden causar:
  - Errores de navegación en runtime
  - Pérdida de datos por cascadas mal configuradas
  - Discrepancias entre Legacy y Clean → comportamiento impredecible
- ❌ Sin validación de relaciones, el resto de la configuración no tiene sentido

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

**Validación de éxito:**
- ✅ `dotnet build` sin errores
- ✅ Migration temporal vacía (sin cambios en esquema)
- ✅ 9/9 relaciones configuradas con constraint names correctos
- ✅ Archivo `DATABASE_RELATIONSHIPS_REPORT.md` generado

---

### ⚙️ ACCIÓN 2: EJECUTAR PROGRAM_CS_AND_DI_CONFIGURATION.md

**⏱️ Duración estimada:** 1 hora  
**🤖 Agente recomendado:** Claude Sonnet 4.5 (Modo Agente)  
**⚠️ PREREQUISITO:** ACCIÓN 1 completada ✅

**Por qué ejecutar después de Acción 1:**
- ✅ Las relaciones de DB deben estar correctas ANTES de configurar servicios
- ✅ Si hay errores de relaciones, el API no arrancará correctamente
- ✅ Los tests de integración dependen de relaciones correctas

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

**Validación de éxito:**
- ✅ `dotnet build`: Success (0 errors)
- ✅ `dotnet run`: API ejecutándose en puerto 5001
- ✅ Swagger UI funcionando: `https://localhost:5001/swagger`
- ✅ Health check respondiendo: `https://localhost:5001/health`
- ✅ Logs generándose en `logs/` folder
- ✅ Archivo `PROGRAM_CS_CONFIGURATION_REPORT.md` generado

---

## 📊 ESTADO ACTUAL DEL PROYECTO

### ✅ Completado (100%)
- **Entidades:** 36/36 migradas
  - 24 Rich Domain Models
  - 9 Read Models
  - 3 Catálogos
- **Código generado:** ~12,053 líneas
- **Errores de compilación:** 0
- **Documentación:** `MiGenteEnLinea.Clean/MIGRATION_STATUS.md`

### ⏳ En Progreso
- **Relaciones de DB:** Análisis completo, validación pendiente
- **Configuración de App:** Prompts creados, ejecución pendiente

### 🚫 Bloqueado (hasta completar Acción 1 y 2)
- Implementación de CQRS Commands/Queries
- Creación de Controllers REST
- Tests de integración
- Migración de Business Logic de Services

---

## 🔗 ARCHIVOS CLAVE CREADOS

### Prompts
1. `prompts/DATABASE_RELATIONSHIPS_VALIDATION.md` (580+ líneas)
   - Inventario completo de 9 FK relationships del EDMX
   - Patrones de Fluent API con ejemplos
   - Guías de DeleteBehavior
   - Workflow de validación con comandos dotnet
   - Template de documentación

2. `prompts/PROGRAM_CS_AND_DI_CONFIGURATION.md` (680+ líneas)
   - Reemplazo completo de Program.cs (200+ líneas)
   - DependencyInjection.cs actualizado (Infrastructure)
   - Nuevo DependencyInjection.cs (Application) con MediatR
   - Templates de appsettings.json
   - Comandos de instalación de NuGet packages
   - Workflow de validación
   - Guía de troubleshooting

### Documentación
3. `prompts/README.md` (actualizado)
   - Workflow 2: Database Relationships Validation
   - Workflow 3: Program.cs and DI Configuration
   - Estructura de prompts actualizada

4. `NEXT_STEPS_CRITICAL.md` (este archivo)
   - Roadmap de próximos pasos
   - Validaciones de éxito
   - Comandos de ejecución

---

## ⚠️ NOTAS IMPORTANTES

### Sobre la Base de Datos
- **Nombre:** `db_a9f8ff_migente`
- **Servidor:** `localhost,1433`
- **Estado:** Compartida entre Legacy (Web Forms) y Clean (API)
- **Constraint names:** Deben coincidir EXACTAMENTE con el EDMX para evitar conflictos

### Sobre las Migraciones
- **NO aplicar migraciones** hasta validar relaciones 100%
- **Usar migrations temporales** solo para validar diferencias
- **Eliminar migration temporal** después de validación
- **Primera migration real:** Se creará después de validar todo

### Sobre los NuGet Packages
Paquetes a instalar en Acción 2:
- **Application:** MediatR, FluentValidation, AutoMapper
- **Infrastructure:** Serilog.Sinks.MSSqlServer, Serilog.Sinks.File
- **API:** Serilog.AspNetCore, Microsoft.AspNetCore.Authentication.JwtBearer

---

## 🎯 OBJETIVO FINAL (después de Acción 1 y 2)

Al completar ambas acciones, el proyecto estará en estado:

✅ **Database Layer:**
- 36 entidades con relaciones correctas
- Fluent API configurations completas
- Paridad 100% con Legacy EDMX

✅ **Application Layer:**
- MediatR configurado (CQRS ready)
- FluentValidation configurado
- AutoMapper configurado
- Listo para implementar Commands/Queries

✅ **API Layer:**
- Program.cs completo con logging
- Swagger con documentación
- Health checks funcionando
- CORS policies configuradas
- Listo para recibir Controllers

✅ **Infrastructure Layer:**
- DbContext configurado
- Repositories listos (interfaces definidas)
- Servicios externos (stubs comentados)
- Listo para implementar lógica

---

## 📞 CONTACTO Y SOPORTE

**Si encuentras problemas durante la ejecución:**

1. **Errores de compilación:** Verificar que las 9 relaciones estén configuradas correctamente
2. **Errores de migration:** Comparar constraint names con EDMX
3. **Errores de NuGet:** Verificar que los packages se instalaron en los proyectos correctos
4. **API no arranca:** Revisar logs en carpeta `logs/`, verificar connection string

**Archivos de reporte:**
- `DATABASE_RELATIONSHIPS_REPORT.md` (generado por Acción 1)
- `PROGRAM_CS_CONFIGURATION_REPORT.md` (generado por Acción 2)

---

**🚀 COMENCEMOS CON ACCIÓN 1 (CRÍTICO)**

Copia el comando de la Acción 1 y ejecútalo en Claude Sonnet 4.5.
