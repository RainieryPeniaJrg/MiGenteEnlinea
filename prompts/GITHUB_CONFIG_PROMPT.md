# 🔧 PROMPT: Configuración de GitHub para Workspace Multi-Root

---

## 📋 CONTEXTO

Tengo un **workspace multi-root en VS Code** que contiene dos proyectos:

1. **🔷 Proyecto Legacy** (`Codigo Fuente Mi Gente/`) - ASP.NET Web Forms
2. **🚀 Proyecto Clean** (`MiGenteEnLinea.Clean/`) - Clean Architecture

Actualmente, la configuración de GitHub (`.github/`, READMEs, documentación) está **dentro del proyecto Legacy**, pero necesito reorganizarla para que sirva a **todo el workspace**.

---

## 🎯 OBJETIVO

**Reorganizar la configuración de GitHub y documentación** para que:
- ✅ La configuración de GitHub esté en la **raíz del workspace**
- ✅ El README principal describa **ambos proyectos**
- ✅ Cada proyecto tenga su propio README específico
- ✅ La configuración de CI/CD maneje **ambas soluciones**
- ✅ Los issue templates y PR templates sean **reutilizables** para ambos

---

## 📂 ESTRUCTURA ACTUAL

```
ProyectoMigente/
├── Codigo Fuente Mi Gente/          # 🔷 LEGACY (repositorio Git)
│   ├── .git/                         # ⚠️ Repositorio Git AQUÍ
│   ├── .github/                      # ⚠️ Config GitHub AQUÍ
│   │   ├── ISSUE_TEMPLATE/
│   │   ├── PULL_REQUEST_TEMPLATE.md
│   │   ├── copilot-instructions.md
│   │   └── config.yml
│   ├── README.md                     # ⚠️ README del legacy
│   ├── SECURITY.md
│   ├── CONTRIBUTING.md
│   ├── CODE_OF_CONDUCT.md
│   ├── CHANGELOG.md
│   ├── docs/
│   ├── scripts/
│   └── MiGente.sln
│
├── MiGenteEnLinea.Clean/             # 🚀 CLEAN (no es repo Git)
│   ├── MiGenteEnLinea.Clean.sln
│   ├── src/
│   ├── tests/
│   └── MIGRATION_SUCCESS_REPORT.md
│
├── MiGenteEnLinea-Workspace.code-workspace
├── README.md                          # ✅ Ya existe (README del workspace)
├── WORKSPACE_README.md
├── DDD_MIGRATION_PROMPT.md
└── SESSION_SUMMARY.md
```

---

## 🎯 ESTRUCTURA OBJETIVO

```
ProyectoMigente/                     # ✅ RAÍZ DEL WORKSPACE = ROOT DEL REPOSITORIO
├── .git/                             # ✅ Mover repositorio Git AQUÍ
├── .github/                          # ✅ Mover config GitHub AQUÍ
│   ├── ISSUE_TEMPLATE/
│   │   ├── 1-bug_report.md
│   │   ├── 2-feature_request.md
│   │   └── 3-security_vulnerability.md
│   ├── workflows/                    # ✅ NUEVO - CI/CD
│   │   ├── legacy-build.yml          # Build del proyecto Legacy
│   │   ├── clean-build.yml           # Build del proyecto Clean
│   │   ├── clean-tests.yml           # Tests del proyecto Clean
│   │   └── security-scan.yml         # Security scanning
│   ├── PULL_REQUEST_TEMPLATE.md
│   ├── copilot-instructions.md       # ✅ Actualizado con contexto dual
│   └── config.yml
│
├── docs/                             # ✅ Documentación del workspace
│   ├── architecture/
│   │   ├── LEGACY_ARCHITECTURE.md
│   │   ├── CLEAN_ARCHITECTURE.md
│   │   └── COMPARISON.md
│   ├── migration/
│   │   ├── DATABASE_MIGRATION.md
│   │   ├── DDD_REFACTORING.md
│   │   └── MIGRATION_PHASES.md
│   ├── development/
│   │   ├── SETUP.md
│   │   ├── DEBUGGING.md
│   │   └── TESTING.md
│   └── security/
│       ├── VULNERABILITIES.md
│       └── REMEDIATION_PLAN.md
│
├── scripts/                          # ✅ Scripts del workspace
│   ├── setup-workspace.ps1
│   ├── setup-migration-simple.ps1
│   └── README.md
│
├── .gitignore                        # ✅ Gitignore del workspace
├── README.md                         # ✅ README principal del workspace
├── SECURITY.md                       # ✅ Política de seguridad
├── CONTRIBUTING.md                   # ✅ Guía de contribución
├── CODE_OF_CONDUCT.md                # ✅ Código de conducta
├── CHANGELOG.md                      # ✅ Changelog del workspace
├── LICENSE                           # ✅ Licencia
├── WORKSPACE_README.md               # ✅ Guía de uso del workspace
├── DDD_MIGRATION_PROMPT.md           # ✅ Prompt de migración DDD
├── SESSION_SUMMARY.md                # ✅ Resumen de sesiones
├── MiGenteEnLinea-Workspace.code-workspace  # ✅ Configuración del workspace
│
├── legacy/                           # ✅ RENOMBRAR: Codigo Fuente Mi Gente → legacy
│   ├── README.md                     # ✅ README específico del legacy
│   ├── MiGente.sln
│   ├── MiGente_Front/
│   ├── docs/                         # Docs específicos del legacy
│   └── scripts/                      # Scripts específicos del legacy
│
└── clean/                            # ✅ RENOMBRAR: MiGenteEnLinea.Clean → clean
    ├── README.md                     # ✅ README específico del clean
    ├── MiGenteEnLinea.Clean.sln
    ├── src/
    ├── tests/
    └── docs/                         # Docs específicos del clean
```

---

## 🚀 TAREAS ESPECÍFICAS

### ✅ Tarea 1: Preparar Repositorio Git

**IMPORTANTE:** Antes de mover nada, hacer backup y commit actual.

1. **Verificar estado actual**:
   ```powershell
   cd "C:\Users\ray\OneDrive\Documents\ProyectoMigente\Codigo Fuente Mi Gente"
   git status
   git log --oneline -5
   ```

2. **Hacer commit de cambios pendientes**:
   ```powershell
   git add .
   git commit -m "docs: preparar para reorganización de workspace"
   git push origin main
   ```

3. **Mover repositorio Git a la raíz**:
   ```powershell
   cd "C:\Users\ray\OneDrive\Documents\ProyectoMigente"
   
   # Mover .git a la raíz
   Move-Item -Path "Codigo Fuente Mi Gente\.git" -Destination ".git"
   
   # Verificar que el repo funciona
   git status
   ```

4. **Actualizar .gitignore en la raíz**:
   - [ ] Copiar `.gitignore` del legacy a la raíz
   - [ ] Agregar rutas específicas para ambos proyectos
   - [ ] Agregar patrones para archivos del workspace

---

### ✅ Tarea 2: Reorganizar Archivos de Configuración

1. **Mover configuración de GitHub**:
   ```powershell
   # Mover carpeta .github a la raíz
   Move-Item -Path "Codigo Fuente Mi Gente\.github" -Destination ".github"
   ```

2. **Mover documentación del workspace a la raíz**:
   ```powershell
   # Archivos que ya están en la raíz (verificar y mantener)
   # - README.md (ya existe, verificar contenido)
   # - WORKSPACE_README.md (ya existe)
   # - DDD_MIGRATION_PROMPT.md (ya existe)
   # - SESSION_SUMMARY.md (ya existe)
   
   # Mover archivos de políticas
   Move-Item -Path "Codigo Fuente Mi Gente\SECURITY.md" -Destination "SECURITY.md"
   Move-Item -Path "Codigo Fuente Mi Gente\CONTRIBUTING.md" -Destination "CONTRIBUTING.md"
   Move-Item -Path "Codigo Fuente Mi Gente\CODE_OF_CONDUCT.md" -Destination "CODE_OF_CONDUCT.md"
   Move-Item -Path "Codigo Fuente Mi Gente\CHANGELOG.md" -Destination "CHANGELOG.md"
   Move-Item -Path "Codigo Fuente Mi Gente\LICENSE" -Destination "LICENSE"
   ```

3. **Reorganizar carpetas de documentación**:
   ```powershell
   # Crear estructura de docs en la raíz
   New-Item -ItemType Directory -Path "docs\architecture"
   New-Item -ItemType Directory -Path "docs\migration"
   New-Item -ItemType Directory -Path "docs\development"
   New-Item -ItemType Directory -Path "docs\security"
   
   # Mover documentación de migración
   Move-Item -Path "Codigo Fuente Mi Gente\docs\MIGRATION_DATABASE_FIRST_TO_CODE_FIRST.md" `
             -Destination "docs\migration\DATABASE_MIGRATION.md"
   
   # El DDD_MIGRATION_PROMPT.md ya está en la raíz, crear link en docs
   ```

4. **Reorganizar scripts**:
   ```powershell
   # Crear carpeta scripts en la raíz
   New-Item -ItemType Directory -Path "scripts" -Force
   
   # Mover scripts del workspace
   Copy-Item -Path "Codigo Fuente Mi Gente\scripts\setup-migration-simple.ps1" `
             -Destination "scripts\setup-migration-simple.ps1"
   Copy-Item -Path "Codigo Fuente Mi Gente\scripts\setup-codefirst-migration.ps1" `
             -Destination "scripts\setup-codefirst-migration.ps1"
   ```

---

### ✅ Tarea 3: Renombrar Carpetas de Proyectos

**PRECAUCIÓN:** Esto puede romper el workspace. Hacer después de commit.

1. **Renombrar carpeta legacy**:
   ```powershell
   # Opción 1: Renombrar (PELIGRO: rompe paths en Git history)
   # Rename-Item -Path "Codigo Fuente Mi Gente" -NewName "legacy"
   
   # Opción 2: Mantener nombre actual y actualizar workspace (RECOMENDADO)
   # No renombrar, solo actualizar referencias en workspace
   ```

2. **Renombrar carpeta clean**:
   ```powershell
   # Opción 1: Renombrar
   # Rename-Item -Path "MiGenteEnLinea.Clean" -NewName "clean"
   
   # Opción 2: Mantener nombre actual (RECOMENDADO)
   # No renombrar por ahora
   ```

**DECISIÓN:** Por ahora, **NO renombrar carpetas** para evitar problemas. Simplemente reorganizar contenido.

---

### ✅ Tarea 4: Crear READMEs Específicos

1. **README para proyecto Legacy** (`Codigo Fuente Mi Gente/README.md`):
   - [ ] Descripción del proyecto Legacy
   - [ ] Estado: Mantenimiento (no agregar features nuevos)
   - [ ] Tecnologías: ASP.NET Web Forms, EF6, DevExpress
   - [ ] Cómo ejecutar localmente
   - [ ] Link al README principal del workspace
   - [ ] Advertencia: Este proyecto está siendo reemplazado

2. **README para proyecto Clean** (`MiGenteEnLinea.Clean/README.md`):
   - [ ] Descripción del proyecto Clean Architecture
   - [ ] Estado: Desarrollo activo
   - [ ] Tecnologías: ASP.NET Core 8, EF Core 8, Clean Architecture
   - [ ] Estructura de capas (Domain, Application, Infrastructure, API)
   - [ ] Cómo ejecutar localmente
   - [ ] Cómo correr tests (cuando existan)
   - [ ] Link al README principal del workspace

---

### ✅ Tarea 5: Actualizar Referencias en Archivos

1. **Actualizar `MiGenteEnLinea-Workspace.code-workspace`**:
   - [ ] Paths de folders siguen funcionando
   - [ ] Launch configurations apuntan a los paths correctos
   - [ ] Tasks apuntan a los paths correctos

2. **Actualizar `.github/copilot-instructions.md`**:
   - [ ] Paths actualizados a la nueva estructura
   - [ ] Mantener contexto dual-project

3. **Actualizar READMEs**:
   - [ ] `README.md` principal - verificar links a archivos movidos
   - [ ] `WORKSPACE_README.md` - actualizar paths
   - [ ] `DDD_MIGRATION_PROMPT.md` - actualizar paths si necesario

4. **Actualizar `CONTRIBUTING.md`**:
   - [ ] Branch naming strategy
   - [ ] Commit message convention
   - [ ] Pull request process
   - [ ] Code review guidelines

---

### ✅ Tarea 6: Crear Workflows de CI/CD

**NOTA:** Esto es para el futuro, cuando esté listo para CI/CD.

1. **Crear `legacy-build.yml`**:
   ```yaml
   name: Legacy Build
   
   on:
     push:
       branches: [ main, develop ]
       paths:
         - 'Codigo Fuente Mi Gente/**'
     pull_request:
       branches: [ main ]
       paths:
         - 'Codigo Fuente Mi Gente/**'
   
   jobs:
     build:
       runs-on: windows-latest
       
       steps:
       - uses: actions/checkout@v3
       
       - name: Setup .NET Framework
         uses: microsoft/setup-msbuild@v1.1
       
       - name: Restore NuGet packages
         run: nuget restore "Codigo Fuente Mi Gente/MiGente.sln"
       
       - name: Build solution
         run: msbuild "Codigo Fuente Mi Gente/MiGente.sln" /p:Configuration=Release
   ```

2. **Crear `clean-build.yml`**:
   ```yaml
   name: Clean Architecture Build
   
   on:
     push:
       branches: [ main, develop ]
       paths:
         - 'MiGenteEnLinea.Clean/**'
     pull_request:
       branches: [ main ]
       paths:
         - 'MiGenteEnLinea.Clean/**'
   
   jobs:
     build:
       runs-on: ubuntu-latest
       
       steps:
       - uses: actions/checkout@v3
       
       - name: Setup .NET 8
         uses: actions/setup-dotnet@v3
         with:
           dotnet-version: '8.0.x'
       
       - name: Restore dependencies
         run: dotnet restore MiGenteEnLinea.Clean/MiGenteEnLinea.Clean.sln
       
       - name: Build
         run: dotnet build MiGenteEnLinea.Clean/MiGenteEnLinea.Clean.sln --configuration Release --no-restore
   ```

3. **Crear `clean-tests.yml`** (para cuando haya tests):
   ```yaml
   name: Clean Architecture Tests
   
   on:
     push:
       branches: [ main, develop ]
       paths:
         - 'MiGenteEnLinea.Clean/**'
     pull_request:
       branches: [ main ]
   
   jobs:
     test:
       runs-on: ubuntu-latest
       
       steps:
       - uses: actions/checkout@v3
       
       - name: Setup .NET 8
         uses: actions/setup-dotnet@v3
         with:
           dotnet-version: '8.0.x'
       
       - name: Restore dependencies
         run: dotnet restore MiGenteEnLinea.Clean/MiGenteEnLinea.Clean.sln
       
       - name: Build
         run: dotnet build MiGenteEnLinea.Clean/MiGenteEnLinea.Clean.sln --no-restore
       
       - name: Test
         run: dotnet test MiGenteEnLinea.Clean/MiGenteEnLinea.Clean.sln --no-build --verbosity normal --collect:"XPlat Code Coverage"
       
       - name: Upload coverage to Codecov
         uses: codecov/codecov-action@v3
   ```

4. **Crear `security-scan.yml`**:
   ```yaml
   name: Security Scan
   
   on:
     push:
       branches: [ main ]
     schedule:
       - cron: '0 0 * * 0'  # Weekly on Sunday
   
   jobs:
     security:
       runs-on: ubuntu-latest
       
       steps:
       - uses: actions/checkout@v3
       
       - name: Run Trivy vulnerability scanner
         uses: aquasecurity/trivy-action@master
         with:
           scan-type: 'fs'
           scan-ref: '.'
           format: 'sarif'
           output: 'trivy-results.sarif'
       
       - name: Upload Trivy results to GitHub Security tab
         uses: github/codeql-action/upload-sarif@v2
         with:
           sarif_file: 'trivy-results.sarif'
   ```

---

### ✅ Tarea 7: Actualizar .gitignore

Crear `.gitignore` en la raíz del workspace:

```gitignore
# ================================
# WORKSPACE FILES
# ================================
SESSION_SUMMARY.md
.vscode/
*.code-workspace.backup

# ================================
# LEGACY PROJECT (Web Forms)
# ================================
Codigo Fuente Mi Gente/bin/
Codigo Fuente Mi Gente/obj/
Codigo Fuente Mi Gente/.vs/
Codigo Fuente Mi Gente/packages/
Codigo Fuente Mi Gente/MiGente_Front/bin/
Codigo Fuente Mi Gente/MiGente_Front/obj/

# ================================
# CLEAN PROJECT (ASP.NET Core)
# ================================
MiGenteEnLinea.Clean/bin/
MiGenteEnLinea.Clean/obj/
MiGenteEnLinea.Clean/.vs/
MiGenteEnLinea.Clean/**/*.user
MiGenteEnLinea.Clean/**/appsettings.Development.json
MiGenteEnLinea.Clean/**/appsettings.Local.json

# ================================
# COMMON
# ================================
*.suo
*.user
*.userosscache
*.sln.docstates
*.userprefs

# Build results
[Dd]ebug/
[Dd]ebugPublic/
[Rr]elease/
[Rr]eleases/
x64/
x86/
[Aa][Rr][Mm]/
[Aa][Rr][Mm]64/
bld/
[Bb]in/
[Oo]bj/
[Ll]og/
[Ll]ogs/

# Visual Studio cache/options
.vs/
.vscode/
*.vsidx
*.vssscc
$tf/

# NuGet Packages
*.nupkg
*.snupkg
**/packages/*
!**/packages/build/
*.nuget.props
*.nuget.targets

# Test Results
[Tt]est[Rr]esult*/
[Bb]uild[Ll]og.*
*.trx
*.coverage
*.coveragexml

# User-specific files
*.rsuser
*.sln.iml
.idea/

# Sensitive files
appsettings.Local.json
appsettings.*.Local.json
*.pfx
*.key
secrets.json
.env
.env.local

# OS files
.DS_Store
Thumbs.db
desktop.ini

# Backup files
*.bak
*.backup
*~
```

---

### ✅ Tarea 8: Commit y Push Final

1. **Hacer commit de la reorganización**:
   ```powershell
   cd "C:\Users\ray\OneDrive\Documents\ProyectoMigente"
   
   git add .
   git status  # Revisar cambios
   
   git commit -m "refactor: reorganizar workspace para multi-root structure

   - Mover .git a la raíz del workspace
   - Mover .github/ a la raíz
   - Reorganizar documentación en docs/
   - Crear READMEs específicos por proyecto
   - Actualizar paths en workspace configuration
   - Crear .gitignore del workspace
   - Preparar estructura para CI/CD
   
   BREAKING CHANGE: La estructura del repositorio cambió.
   El proyecto legacy ahora está en 'Codigo Fuente Mi Gente/'.
   El proyecto clean está en 'MiGenteEnLinea.Clean/'."
   
   git push origin main
   ```

2. **Verificar que todo funciona**:
   ```powershell
   # Verificar que el workspace abre correctamente
   code MiGenteEnLinea-Workspace.code-workspace
   
   # Verificar que git funciona
   git status
   git log --oneline -5
   
   # Verificar que los proyectos compilan
   cd "Codigo Fuente Mi Gente"
   msbuild MiGente.sln /p:Configuration=Debug
   
   cd "..\MiGenteEnLinea.Clean"
   dotnet build
   ```

---

## 📝 CHECKLIST DE VALIDACIÓN

Cuando termines, verifica:

### ✅ Estructura del Repositorio
- [ ] `.git/` está en la raíz del workspace
- [ ] `.github/` está en la raíz del workspace
- [ ] `docs/` está organizado por categorías
- [ ] `scripts/` contiene scripts del workspace
- [ ] Archivos de política (SECURITY.md, etc.) están en la raíz

### ✅ Proyectos
- [ ] Proyecto Legacy tiene su propio README
- [ ] Proyecto Clean tiene su propio README
- [ ] Ambos proyectos compilan sin errores
- [ ] Paths en workspace configuration funcionan

### ✅ Git
- [ ] `git status` funciona correctamente
- [ ] `git log` muestra historial completo
- [ ] `.gitignore` cubre ambos proyectos
- [ ] Commit y push funcionan

### ✅ Documentación
- [ ] README principal describe el workspace
- [ ] Links entre documentos funcionan
- [ ] Copilot instructions actualizado
- [ ] Contributing guide tiene instrucciones para ambos proyectos

---

## 🎯 RESULTADO ESPERADO

Al final de esta tarea, deberías tener:

1. ✅ **Repositorio Git en la raíz del workspace**
2. ✅ **Configuración de GitHub unificada** (`.github/` en raíz)
3. ✅ **Documentación organizada** (`docs/` con subcarpetas)
4. ✅ **READMEs específicos** para cada proyecto
5. ✅ **.gitignore del workspace** cubriendo ambos proyectos
6. ✅ **Workflows de CI/CD preparados** (opcional, para futuro)
7. ✅ **Todo funcionando** (workspace, git, builds)

---

## 🚨 ADVERTENCIAS IMPORTANTES

### ⚠️ Antes de Empezar

1. **Hacer backup completo** del directorio
2. **Commit y push** todos los cambios pendientes
3. **Verificar** que tienes backup remoto en GitHub

### ⚠️ Durante el Proceso

1. **NO renombrar carpetas** hasta estar seguro que todo funciona
2. **Verificar cada comando** antes de ejecutarlo
3. **Hacer commits incrementales** después de cada tarea mayor
4. **Probar que el workspace abre** después de cada cambio

### ⚠️ Si Algo Sale Mal

```powershell
# Restaurar desde GitHub
git reset --hard origin/main

# O desde backup local
# Copiar backup del directorio completo
```

---

## 💡 COMANDOS ÚTILES

### Verificar estado de Git
```powershell
git status
git log --oneline --graph --all -10
git remote -v
```

### Mover archivos preservando historial de Git
```powershell
# Git automáticamente detecta movimientos si el contenido es similar
git mv "ruta/origen/archivo.md" "ruta/destino/archivo.md"
```

### Verificar workspace
```powershell
# Abrir workspace
code MiGenteEnLinea-Workspace.code-workspace

# Verificar compilación Legacy
cd "Codigo Fuente Mi Gente"
msbuild MiGente.sln

# Verificar compilación Clean
cd "..\MiGenteEnLinea.Clean"
dotnet build
```

---

## 🚀 ¡EMPECEMOS!

Por favor, comienza con la **Tarea 1: Preparar Repositorio Git**.

Sigue el orden de las tareas y **haz commit después de cada tarea completada** para poder revertir si algo sale mal.

**Pregunta si tienes dudas** antes de ejecutar comandos que muevan o eliminen archivos.

¡Manos a la obra! 💪
