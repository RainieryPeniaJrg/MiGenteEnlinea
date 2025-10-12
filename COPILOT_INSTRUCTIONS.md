# 📋 INSTRUCCIONES PARA GITHUB COPILOT CHAT

---

## 🎯 Dos Prompts Disponibles

Tienes **dos prompts completos** listos para usar con GitHub Copilot Chat:

1. **DDD_MIGRATION_PROMPT.md** - Para migrar entidades con Domain-Driven Design
2. **GITHUB_CONFIG_PROMPT.md** - Para reorganizar configuración de GitHub

---

## 🔷 PROMPT 1: Migración de Entidades con DDD

### 📄 Archivo
`DDD_MIGRATION_PROMPT.md`

### 🎯 Para Qué Sirve
Refactorizar las 36 entidades scaffolded desde Database-First a Code-First aplicando:
- ✅ Domain-Driven Design (Rich Domain Models)
- ✅ Auditable Entity Pattern
- ✅ Fluent API Configuration
- ✅ BCrypt Password Hashing
- ✅ CQRS con MediatR
- ✅ FluentValidation
- ⚠️ **SIN tests** (se agregarán después)

### 🚀 Comando para Copilot Chat

Copia y pega este comando completo en **GitHub Copilot Chat**:

```
@workspace Lee el archivo DDD_MIGRATION_PROMPT.md completo y úsalo como guía paso a paso.

CONTEXTO:
- Workspace ROOT: C:\Users\ray\OneDrive\Documents\ProyectoMigente\
- Proyecto Legacy: Codigo Fuente Mi Gente/
- Proyecto Clean: MiGenteEnLinea.Clean/
- Ya existe la solución creada con setup-codefirst-migration.ps1
- 36 entidades ya están scaffolded en MiGenteEnLinea.Clean/src/Infrastructure/Persistence/Entities/Generated/
- Database: db_a9f8ff_migente en SQL Server (localhost,1433)
- NO crear tests aún (fase posterior)
- Enfoque: Migración Code-First + Lógica de negocio con DDD + Controllers

TAREA:
Ejecuta la Tarea 1: Refactorizar Entidad Credencial

Sigue TODOS los pasos en orden:
1. Crear clases base en MiGenteEnLinea.Clean/src/Core/MiGenteEnLinea.Domain/Common/
   (AuditableEntity, SoftDeletableEntity, AggregateRoot, ValueObject)
   
2. Copiar Infrastructure/Persistence/Entities/Generated/Credenciale.cs 
   a Domain/Entities/Authentication/Credencial.cs
   
3. Refactorizar Credencial con patrón DDD:
   - Heredar de AuditableEntity
   - Encapsular propiedades (setters privados)
   - Constructor privado para EF Core
   - Factory method Create()
   - Domain methods: Activar(), Desactivar(), ActualizarPasswordHash(), RegistrarAcceso()
   - Renombrar Password → PasswordHash
   - Agregar FechaActivacion, UltimoAcceso
   
4. Crear CredencialConfiguration.cs en Infrastructure/Persistence/Configurations/
   - Mapear a tabla Credenciales existente
   - Mantener nombres de columnas legacy (id, userID, email, password, activo)
   - Índices únicos en UserId y Email
   
5. Implementar BCrypt Password Hasher:
   - IPasswordHasher en Domain/Interfaces/
   - BCryptPasswordHasher en Infrastructure/Identity/Services/
   
6. Crear AuditableEntityInterceptor en Infrastructure/Persistence/Interceptors/

7. Actualizar DbContext con configuración y interceptor

Después de completar Credencial, continuar con Empleador y Contratista usando el mismo patrón.

IMPORTANTE: 
- Ejecuta paso por paso, esperando mi confirmación
- NO crear tests unitarios ni de integración (fase posterior)
- Enfócate en la lógica de negocio y arquitectura limpia
- Mantener compatibilidad con nombres de tabla/columna legacy
- TODOS los paths son relativos a: C:\Users\ray\OneDrive\Documents\ProyectoMigente\
```

---

## ✅ PROMPT 2: Configuración de GitHub (COMPLETADO)

### 📄 Archivo
`GITHUB_CONFIG_PROMPT.md`

### 🎯 Para Qué Sirve
Reorganizar la estructura del repositorio para que:
- ✅ `.git/` y `.github/` estén en la raíz del workspace ✅ **COMPLETADO**
- ✅ Documentación esté organizada en `docs/` ⏳ Pendiente
- ✅ Cada proyecto tenga su README específico ⏳ Pendiente
- ✅ CI/CD workflows estén preparados ⏳ Pendiente
- ✅ `.gitignore` cubra ambos proyectos ✅ **COMPLETADO**

### ✅ Estado: REORGANIZACIÓN BÁSICA COMPLETADA

**Fecha de completación:** 12 de octubre, 2025

**Cambios aplicados:**
- ✅ `.git/` movido a raíz del workspace
- ✅ `.github/` movido a raíz del workspace
- ✅ `.gitignore` del workspace creado
- ✅ Proyecto `MiGenteEnLinea.Clean/` añadido al repositorio
- ✅ Documentación del workspace añadida (README.md, WORKSPACE_README.md)
- ✅ Commit y push exitoso a GitHub

**Pendientes (opcional):**
- ⏳ Reorganizar documentación en `docs/`
- ⏳ Crear READMEs específicos por proyecto
- ⏳ Configurar GitHub Actions workflows

### 🚀 Comando Anterior (Ya NO necesario)

~~Copia y pega este comando completo en **GitHub Copilot Chat**:~~

```
@workspace Lee el archivo GITHUB_CONFIG_PROMPT.md completo y úsalo como guía paso a paso.

CONTEXTO:
- Workspace multi-root con dos proyectos:
  * Codigo Fuente Mi Gente/ (Legacy Web Forms) - TIENE .git/ y .github/
  * MiGenteEnLinea.Clean/ (Clean Architecture) - NO tiene Git
- Repositorio actual: https://github.com/RainieryPeniaJrg/MiGenteEnlinea
- Branch actual: main
- Objetivo: Reorganizar para que Git y GitHub config estén en la raíz del workspace

ADVERTENCIAS IMPORTANTES:
⚠️ Antes de CUALQUIER cambio:
1. Hacer backup completo del directorio
2. Commit y push de cambios pendientes: git add . && git commit -m "backup antes de reorganización" && git push
3. Confirmar conmigo antes de mover .git/

TAREA:
Ejecuta paso a paso siguiendo el prompt:

1. PRIMERO: Verificar estado actual de Git
   cd "Codigo Fuente Mi Gente"
   git status
   git log --oneline -5

2. Hacer commit de seguridad:
   git add .
   git commit -m "docs: backup antes de reorganización de workspace"
   git push origin main

3. Mover repositorio Git a la raíz del workspace:
   cd ..
   Move-Item -Path "Codigo Fuente Mi Gente\.git" -Destination ".git"
   git status  # Verificar que funciona

4. Mover configuración de GitHub:
   Move-Item -Path "Codigo Fuente Mi Gente\.github" -Destination ".github"

5. Reorganizar documentación y archivos de política

6. Crear READMEs específicos para cada proyecto

7. Actualizar referencias en workspace y copilot-instructions

8. Crear .gitignore del workspace

9. Commit final de la reorganización

IMPORTANTE:
- Ejecuta TAREA POR TAREA, esperando mi confirmación
- Haz commit después de cada tarea mayor
- Si algo falla, podré revertir al último commit
- NO renombrar carpetas (mantener nombres actuales)
- Probar que workspace abre después de cada cambio
```

---

## 📊 Comparación de Prompts

| Aspecto | DDD_MIGRATION_PROMPT | GITHUB_CONFIG_PROMPT |
|---------|---------------------|---------------------|
| **Objetivo** | Refactorizar código | Reorganizar estructura |
| **Prioridad** | 🔥 Alta (desarrollo activo) | ⚠️ Media (organización) |
| **Riesgo** | 🟢 Bajo (no toca Git) | 🔴 Alto (mueve .git/) |
| **Tiempo** | 2-3 horas | 30-60 minutos |
| **Reversible** | ✅ Fácil (Git revert) | ⚠️ Medio (requiere backup) |
| **Dependencies** | Ninguna | Requiere backup previo |

---

## 🎯 Recomendación de Orden

### Opción 1: Empezar con Desarrollo (RECOMENDADO)
```
1. DDD_MIGRATION_PROMPT (empezar a desarrollar)
2. GITHUB_CONFIG_PROMPT (cuando tengas tiempo, no es urgente)
```

**Razón:** Puedes empezar a desarrollar inmediatamente con DDD. La reorganización de GitHub es cosmética y puede hacerse después.

### Opción 2: Empezar con Organización
```
1. GITHUB_CONFIG_PROMPT (organizar primero)
2. DDD_MIGRATION_PROMPT (desarrollar con estructura limpia)
```

**Razón:** Tendrás la estructura perfecta desde el inicio, pero pierdes tiempo antes de empezar a codear.

---

## 💡 Tips para Usar los Prompts

### ✅ Mejores Prácticas

1. **Abrir el archivo del prompt** antes de enviar el comando:
   ```
   code DDD_MIGRATION_PROMPT.md
   ```

2. **Leer el prompt completo** para entender el contexto

3. **Copiar el comando exacto** de estas instrucciones

4. **No modificar el comando** a menos que entiendas las consecuencias

5. **Esperar confirmación** entre tareas grandes

6. **Hacer commits frecuentes** para poder revertir

### ⚠️ Qué NO Hacer

1. ❌ **NO ejecutar ambos prompts simultáneamente**
2. ❌ **NO saltar pasos** en los prompts
3. ❌ **NO mover .git/** sin hacer backup primero
4. ❌ **NO renombrar carpetas** sin actualizar workspace
5. ❌ **NO ignorar advertencias** de los prompts

---

## 🔧 Troubleshooting

### Si GitHub Copilot no responde correctamente:

1. **Verificar que tiene acceso al archivo**:
   ```
   @workspace list all markdown files in workspace
   ```

2. **Abrir el archivo explícitamente**:
   ```
   code DDD_MIGRATION_PROMPT.md
   ```

3. **Simplificar el comando**:
   ```
   @workspace Read DDD_MIGRATION_PROMPT.md and start with Task 1: Refactor Credencial entity
   ```

### Si algo sale mal con Git:

1. **Revertir último commit**:
   ```powershell
   git reset --soft HEAD~1
   ```

2. **Restaurar desde GitHub**:
   ```powershell
   git fetch origin
   git reset --hard origin/main
   ```

3. **Usar backup** (si hiciste backup previo):
   ```powershell
   # Restaurar desde backup manual
   ```

---

## 📋 Checklist Final

Antes de ejecutar cualquier prompt:

### ✅ Preparación
- [ ] He leído el prompt completo
- [ ] Entiendo qué va a hacer
- [ ] He hecho commit de cambios pendientes
- [ ] Tengo backup (si es GITHUB_CONFIG_PROMPT)
- [ ] Tengo tiempo para completar la tarea

### ✅ Durante Ejecución
- [ ] Estoy leyendo cada respuesta de Copilot
- [ ] Estoy verificando el código generado
- [ ] Estoy haciendo commits incrementales
- [ ] Estoy probando que todo compila

### ✅ Después de Completar
- [ ] Todo compila sin errores
- [ ] He hecho commit final
- [ ] He actualizado documentación si necesario
- [ ] He probado que el workspace funciona

---

## 🎊 ¡Listo para Empezar!

**Opción Recomendada:** Empezar con `DDD_MIGRATION_PROMPT`

**Comando:**
```
@workspace Lee el archivo DDD_MIGRATION_PROMPT.md completo y ejecuta la Tarea 1: Refactorizar Entidad Credencial. NO crear tests aún. Enfócate en DDD + Code-First + Controllers.
```

**Duración estimada:** 2-3 horas

**Resultado esperado:**
- ✅ Entidad Credencial refactorizada con DDD
- ✅ Fluent API configuration
- ✅ BCrypt password hasher
- ✅ Auditoría automática
- ✅ Controllers de Authentication

---

**¡Manos a la obra!** 💪🚀
