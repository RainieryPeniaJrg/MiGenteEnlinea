# ✅ CONFIGURACIÓN COMPLETADA: Prompts para Modo Agente

**Fecha:** 12 de octubre, 2025  
**Objetivo:** Actualizar instrucciones para Claude Sonnet 4.5 en modo agente autónomo

---

## 🎯 RESPUESTA A TU PREGUNTA

### ¿Dónde crear los prompts?

**✅ RECOMENDACIÓN: Crear carpeta `/prompts/` en la raíz del workspace**

**Estructura implementada:**

```
C:\Users\ray\OneDrive\Documents\ProyectoMigente\
│
├── .github/
│   └── copilot-instructions.md         # ✅ Para GitHub Copilot (auto-cargado por VS Code)
│
├── prompts/                             # ✅ NUEVO - Para agentes externos
│   ├── README.md                        # ✅ Guía completa de uso
│   └── AGENT_MODE_INSTRUCTIONS.md       # ✅ Claude Sonnet 4.5 - Modo Agente
│
├── Codigo Fuente Mi Gente/             # 🔷 Proyecto Legacy
├── MiGenteEnLinea.Clean/               # 🚀 Proyecto Clean
└── ...
```

---

## 📄 Archivos Creados

### 1. `/prompts/README.md` ✅

**Propósito:** Guía maestra para todos los prompts del workspace

**Contiene:**
- 📂 Estructura de prompts y casos de uso
- 🤖 Diferencia entre Modo Agente vs Modo Asistente
- 🎯 3 workflows comunes (Migración DDD, Feature CQRS, Setup Infraestructura)
- 📋 Checklist pre-ejecución
- 🔒 Límites de autoridad del agente
- 📊 Formato de reportes de progreso
- 🆘 Troubleshooting (problemas comunes y soluciones)

---

### 2. `/prompts/AGENT_MODE_INSTRUCTIONS.md` ✅

**Propósito:** Prompt optimizado para Claude Sonnet 4.5 en modo autónomo

**Características clave:**

#### 🚀 Modo de Operación: AGENTE AUTÓNOMO

**DEBES (sin pedir confirmación):**
- ✅ Ejecutar tareas completas sin pausas
- ✅ Tomar decisiones arquitectónicas dentro de límites
- ✅ Corregir errores automáticamente
- ✅ Reportar progreso cada 3 pasos
- ✅ Validar con build automáticamente

**NO DEBES (pedir confirmación SOLO si):**
- ⛔ Vas a modificar base de datos
- ⛔ Vas a tocar código Legacy
- ⛔ Detectas conflicto arquitectónico mayor
- ⛔ Encuentras error que no puedes resolver

#### 📂 Contexto Completo

- Estructura detallada de ambos proyectos
- 36 entidades scaffolded referenciadas
- Paths absolutos (no ambigüedad)
- Base de datos y connection strings
- Estado actual de migración

#### 🎯 Tareas Prioritarias

1. **Credencial** (CRÍTICO - passwords sin hash)
2. **Empleador** (core business entity)
3. **Contratista** (core business entity)
4. Infraestructura de auditoría
5. CQRS con MediatR

#### 🛠️ Patrones con Código Completo

- Rich Domain Model (ejemplo completo de Credencial)
- Fluent API Configuration (template reutilizable)
- BCrypt Password Hasher (implementación completa)
- FluentValidation (reglas con ejemplos)

#### 🚨 Manejo de Errores Autónomo

**Errores que el agente DEBE resolver solo:**
- Missing using statements
- NuGet packages faltantes
- DbContext not registered
- Errores de sintaxis

**Errores que DEBE reportar:**
- Conflictos arquitectónicos
- Conexión a base de datos
- Decisiones de diseño ambiguas

---

### 3. `.github/copilot-instructions.md` ✅ ACTUALIZADO

**Cambios aplicados:**

- ✅ Agregado header con workspace location
- ✅ Nueva sección **"🤖 AI Agent Resources"**
- ✅ Diferenciación clara: GitHub Copilot (IDE) vs Claude (Agente)
- ✅ Referencias a `/prompts/` folder
- ✅ Quick reference a la estructura

**Ahora incluye:**

```markdown
> **📍 Workspace Location:** `C:\Users\ray\OneDrive\Documents\ProyectoMigente\`  
> **🤖 AI Agent Mode:** GitHub Copilot (IDE Integration)  
> **📚 Advanced Prompts:** See `/prompts/` folder for Claude Sonnet 4.5

## 🤖 AI Agent Resources

### For GitHub Copilot (This File)
- **Mode:** IDE Integration (autocomplete, chat)
- **Purpose:** Quick suggestions, code completion
- **Scope:** Small to medium tasks

### For Claude Sonnet 4.5 / External Agents
- **Mode:** Autonomous Agent (batch execution)
- **Purpose:** Large refactoring, multi-file changes, DDD migration
- **Scope:** Complex architectural tasks
- **Location:** `/prompts/AGENT_MODE_INSTRUCTIONS.md`
```

---

## 🚀 CÓMO USAR EL MODO AGENTE

### Comando Copy-Paste Ready

En tu chat con Claude Sonnet 4.5, copia y pega:

```
@workspace Lee y ejecuta el archivo prompts/AGENT_MODE_INSTRUCTIONS.md

TAREA: Refactorizar entidades [Credencial, Empleador, Contratista] con patrón DDD

AUTORIZACIÓN COMPLETA:
✅ Crear/modificar archivos en Domain, Application, Infrastructure, API
✅ Configurar DbContext y Fluent API
✅ Implementar servicios de seguridad (BCrypt, JWT)
✅ Ejecutar dotnet build para validación
✅ Corregir errores de compilación automáticamente
✅ Registrar servicios en DI
✅ Reportar solo cuando completes cada entidad

LÍMITES:
⛔ NO ejecutar migraciones (dotnet ef database update)
⛔ NO modificar código en "Codigo Fuente Mi Gente/"
⛔ NO crear tests aún (fase posterior)

WORKSPACE ROOT: C:\Users\ray\OneDrive\Documents\ProyectoMigente\

INICIO: Entidad Credencial (passwords en texto plano - CRÍTICO)
```

---

## 📊 Diferencia: Modo Asistente vs Modo Agente

### Modo Asistente (Anterior)
- ⏸️ Pide confirmación para cada archivo
- ⏸️ Explica cada paso antes de ejecutar
- ⏸️ Pausa entre entidades
- ⏸️ Pregunta opciones de diseño
- ⏸️ **Tiempo:** 3-4 horas para 3 entidades
- ⏸️ **Interrupciones:** ~30-40 confirmaciones

### Modo Agente (Nuevo) ✅
- 🚀 Ejecuta sin pausas innecesarias
- 🚀 Reporta cada 3 pasos completados
- 🚀 Toma decisiones arquitectónicas estándar
- 🚀 Corrige errores automáticamente
- 🚀 **Tiempo:** 30-45 minutos para 3 entidades
- 🚀 **Interrupciones:** 3-5 (solo decisiones críticas)

---

## ✅ Beneficios de la Nueva Estructura

### 1. Separación Clara
- `.github/copilot-instructions.md` → Auto-cargado por GitHub Copilot en VS Code
- `/prompts/` → Para agentes externos (Claude, ChatGPT, etc.)

### 2. Optimizado para Ejecución Autónoma
- Lenguaje imperativo (DEBES, EJECUTA, NO PIDAS)
- Autorización explícita
- Límites claros
- Manejo de errores autónomo

### 3. Contexto Completo
- Paths absolutos
- Estructura visual de proyectos
- Estado actual de migración
- Referencias a entidades scaffolded

### 4. Copy-Paste Ready
- Comandos listos para usar
- Workflows completos
- Templates de código funcionales

### 5. Troubleshooting Incluido
- Problemas comunes documentados
- Soluciones paso a paso

---

## 🎯 Próximo Paso Recomendado

### Probar el Modo Agente con Credencial

**Ejecuta este comando en Claude:**

```
@workspace Lee prompts/AGENT_MODE_INSTRUCTIONS.md

TAREA: Refactorizar entidad Credencial con patrón DDD

AUTORIZACIÓN COMPLETA para ejecutar TODOS los pasos sin confirmación.

PASOS ESPERADOS:
1. Crear Credencial.cs en Domain/Entities/Authentication/
2. Crear CredencialConfiguration.cs en Infrastructure/Configurations/
3. Actualizar MiGenteDbContext.cs
4. Implementar BCryptPasswordHasher
5. Registrar servicios en DI
6. Ejecutar dotnet build
7. Reportar resultado

INICIO: Credencial (CRÍTICO - passwords en texto plano)
```

**Tiempo estimado:** 8-12 minutos  
**Resultado esperado:** Entidad Credencial completamente refactorizada con DDD + BCrypt

---

## 📁 Archivos de Referencia

### Creados en esta sesión
- ✅ `/prompts/README.md`
- ✅ `/prompts/AGENT_MODE_INSTRUCTIONS.md`
- ✅ `.github/copilot-instructions.md` (actualizado)
- ✅ `PROMPTS_REORGANIZATION_SUMMARY.md` (este archivo)

### Mantener (compatibilidad)
- ✅ `DDD_MIGRATION_PROMPT.md` (prompt detallado original)
- ✅ `COPILOT_INSTRUCTIONS.md` (guía rápida de inicio)
- ✅ `GITHUB_CONFIG_PROMPT.md` (configuración GitHub completada)

---

## 📚 Documentación Completa

### Para usar con GitHub Copilot (VS Code)
1. Abrir VS Code con el workspace
2. GitHub Copilot carga `.github/copilot-instructions.md` automáticamente
3. Usar `Ctrl+I` para inline chat
4. Copilot tiene contexto del workspace dual-project

### Para usar con Claude/ChatGPT (Modo Agente)
1. Abrir chat con Claude Sonnet 4.5
2. Copiar comando de `prompts/README.md`
3. Pegar y especificar tarea
4. Claude ejecuta autónomamente
5. Reporta progreso cada 3 pasos

---

## ✅ Checklist Final

- [x] ✅ Carpeta `/prompts/` creada en raíz
- [x] ✅ `prompts/README.md` creado (guía completa)
- [x] ✅ `prompts/AGENT_MODE_INSTRUCTIONS.md` creado (Claude agent)
- [x] ✅ `.github/copilot-instructions.md` actualizado
- [x] ✅ Diferenciación clara: Copilot vs Agent
- [x] ✅ Modo agente optimizado (lenguaje imperativo)
- [x] ✅ Contexto completo del workspace
- [x] ✅ Workflows copy-paste ready
- [x] ✅ Troubleshooting incluido
- [x] ✅ Límites de autoridad documentados
- [x] ✅ Este resumen creado

---

## 🎉 LISTO PARA USAR

Tu workspace ahora tiene:

✅ **Prompts organizados** en `/prompts/`  
✅ **Modo agente optimizado** para Claude Sonnet 4.5  
✅ **GitHub Copilot configurado** con referencias a prompts externos  
✅ **Comandos copy-paste ready** para workflows comunes  
✅ **Contexto completo** del workspace dual-project  
✅ **Troubleshooting guide** incluido  

**Próxima acción:** Probar el modo agente con la entidad Credencial  
**Tiempo estimado:** 8-12 minutos de ejecución autónoma

---

_Última actualización: 12 de octubre, 2025_
