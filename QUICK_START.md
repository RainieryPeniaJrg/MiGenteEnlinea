# 🚀 GUÍA RÁPIDA: Ejecutar Migración de Entidades

**Última actualización:** 12 de octubre, 2025  
**Progreso Actual:** 5/36 entidades (13.9%)  
**Próximo Objetivo:** LOTE 1 - Empleados y Nómina (6 entidades)

---

## 📋 COMANDOS RÁPIDOS

### Ver Progreso Actual

```bash
# Ver estado de migración
cat MIGRATION_STATUS.md

# Ver plan completo de 36 entidades
cat prompts/COMPLETE_ENTITY_MIGRATION_PLAN.md
```

---

### Ejecutar LOTE 1 (Empleados y Nómina)

**Para Claude Sonnet 4.5 / GPT-4 Turbo:**

```
@workspace Lee prompts/COMPLETE_ENTITY_MIGRATION_PLAN.md

EJECUTAR: LOTE 1 completo en modo autónomo

ENTIDADES A MIGRAR (EN ORDEN):
1. DeduccionTss (Domain/Entities/Nominas/DeduccionTss.cs)
2. Empleado (Domain/Entities/Empleados/Empleado.cs)
3. EmpleadoNota (Domain/Entities/Empleados/EmpleadoNota.cs)
4. EmpleadoTemporal (Domain/Entities/Empleados/EmpleadoTemporal.cs)
5. ReciboDetalle (Domain/Entities/Nominas/ReciboDetalle.cs)
6. ReciboHeader (Domain/Entities/Nominas/ReciboHeader.cs)

PATRÓN A SEGUIR:
- Lee Generated/[Entidad].cs
- Crea Rich Domain Model en Domain/Entities/[Carpeta]/
- Crea Domain Events en Domain/Events/[Carpeta]/
- Crea Fluent API Configuration en Infrastructure/Configurations/
- Actualiza DbContext (DbSet + comentar scaffolded)
- Ejecuta `dotnet build` para validar
- Crea documento LOTE_1_[ENTIDAD]_COMPLETADA.md

AUTORIZACIÓN COMPLETA:
- Ejecuta TODOS los pasos sin pedir confirmación
- Reporta progreso cada 2 entidades completadas
- Solo pregunta si encuentras error que no puedas resolver

REFERENCIA:
Sigue exactamente el patrón de MiGenteEnLinea.Clean/TAREA_1_CREDENCIAL_COMPLETADA.md

COMENZAR EJECUCIÓN AUTOMÁTICA AHORA.
```

---

### Validar LOTE 1 Completado

```bash
# Cambiar al proyecto Clean
cd MiGenteEnLinea.Clean

# Compilar
dotnet build

# Esperado: Build succeeded. 0 Error(s)

# Ver archivos creados
git status
```

---

### Ejecutar LOTE 2 (Planes y Pagos)

**Después de validar LOTE 1:**

```
@workspace Lee prompts/COMPLETE_ENTITY_MIGRATION_PLAN.md

EJECUTAR: LOTE 2 completo en modo autónomo

ENTIDADES A MIGRAR:
1. PlanEmpleador (Domain/Entities/Planes/PlanEmpleador.cs)
2. PlanContratista (Domain/Entities/Planes/PlanContratista.cs)
3. PasarelaPago (Domain/Entities/Pagos/PasarelaPago.cs)
4. Venta (Domain/Entities/Ventas/Venta.cs)

AUTORIZACIÓN: Modo autónomo. Reporta cada 2 entidades.

COMENZAR EJECUCIÓN.
```

---

## 📁 DOCUMENTOS IMPORTANTES

| Documento | Ubicación | Propósito |
|-----------|-----------|-----------|
| **Plan Maestro** | `prompts/COMPLETE_ENTITY_MIGRATION_PLAN.md` | Todas las 36 entidades organizadas |
| **Estado Actual** | `MIGRATION_STATUS.md` | Progreso visual y próximos pasos |
| **Instrucciones Agente** | `prompts/AGENT_MODE_INSTRUCTIONS.md` | Modo autónomo de Claude Sonnet 4.5 |
| **Guía DDD** | `prompts/DDD_MIGRATION_PROMPT.md` | Patrones y ejemplos de código |
| **Copilot IDE** | `.github/copilot-instructions.md` | Contexto para GitHub Copilot |

---

## 🎯 CHECKLIST PRE-EJECUCIÓN

Antes de ejecutar LOTE 1, verifica:

- [ ] Workspace abierto: `MiGenteEnLinea-Workspace.code-workspace`
- [ ] Base de datos disponible: `localhost,1433`
- [ ] Compilación limpia: `cd MiGenteEnLinea.Clean && dotnet build`
- [ ] Git limpio o cambios commiteados
- [ ] NuGet packages restaurados

---

## 📊 PROGRESO ESPERADO

### Después de LOTE 1

```
█████████░░░░░░░░░░░░░░░░░░░░░░░░░░░ 30.6% Completado

✅ 11/36 entidades completadas
⏳ 25/36 entidades pendientes
```

### Después de LOTE 2

```
█████████████░░░░░░░░░░░░░░░░░░░░░░░ 41.7% Completado

✅ 15/36 entidades completadas
⏳ 21/36 entidades pendientes
```

### Después de LOTE 3-6

```
████████████████████████████████████ 100% Completado

✅ 36/36 entidades completadas
🎉 MIGRACIÓN COMPLETA
```

---

## 🔧 SOLUCIÓN DE PROBLEMAS

### Error de Compilación

```bash
# Ver errores detallados
dotnet build --verbosity detailed

# Limpiar y recompilar
dotnet clean
dotnet build
```

### Entidad No Compila

**Problema:** Error en Fluent API Configuration

**Solución:**
1. Verifica que el nombre de tabla legacy sea correcto
2. Verifica que todas las columnas estén mapeadas
3. Verifica que Domain Events estén ignorados: `builder.Ignore(e => e.Events);`

### DbContext No Reconoce Nueva Entidad

**Problema:** `DbSet<Entidad>` no aparece

**Solución:**
1. Verifica que `DbSet<Entidad>` esté declarado en `MiGenteDbContext.cs`
2. Verifica que Configuration esté en namespace correcto
3. Ejecuta `dotnet build` para regenerar IntelliSense

---

## 🎉 DESPUÉS DE COMPLETAR TODO

### Validación Final

```bash
# Compilar todo
dotnet build

# Ejecutar tests (cuando se creen)
dotnet test

# Ver cobertura
dotnet test /p:CollectCoverage=true
```

### Generar Reporte

```bash
# Actualizar MIGRATION_STATUS.md con progreso 100%
# Crear documento MIGRACION_COMPLETA_SUMMARY.md
# Commitear todos los cambios
git add .
git commit -m "feat: Migración completa de 36 entidades a Clean Architecture"
```

---

## 📞 SOPORTE

**¿Tienes dudas?**

1. Lee `prompts/COMPLETE_ENTITY_MIGRATION_PLAN.md` (detalles de cada entidad)
2. Lee `prompts/DDD_MIGRATION_PROMPT.md` (patrones DDD)
3. Revisa ejemplos completados en `MiGenteEnLinea.Clean/TAREA_*.md`
4. Pregunta a Claude Sonnet 4.5 con contexto del workspace

---

_Última actualización: 12 de octubre, 2025_  
_Versión: 1.0_
