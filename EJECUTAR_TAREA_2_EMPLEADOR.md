# ✅ LISTO PARA EJECUTAR: Tarea 2 - Empleador

**Fecha:** 12 de octubre, 2025  
**Estado:** ⏳ **ESPERANDO EJECUCIÓN**

---

## 🎯 RESUMEN RÁPIDO

**✅ Tarea 1 (Credencial):** COMPLETADA  
**🚀 Tarea 2 (Empleador):** LISTA PARA EJECUTAR

---

## 📋 QUÉ VAMOS A HACER

Refactorizar la entidad **Empleador** (tabla legacy "Ofertantes") siguiendo el mismo patrón DDD de la Tarea 1:

1. ✅ Crear entidad `Empleador.cs` en Domain (Rich Model)
2. ✅ Crear configuración `EmpleadorConfiguration.cs` (Fluent API)
3. ✅ Crear 3 Domain Events
4. ✅ Actualizar `DbContext`
5. ✅ Validar con `dotnet build`

---

## 🚀 COMANDO PARA CLAUDE (COPY-PASTE)

Abre tu chat con Claude Sonnet 4.5 y pega esto:

```
@workspace Lee y ejecuta: MiGenteEnLinea.Clean/TAREA_2_EMPLEADOR_INSTRUCCIONES.md

CONTEXTO:
- ✅ Tarea 1 (Credencial) completada
- ✅ Clases base DDD disponibles
- 📚 Tabla legacy: "Ofertantes" (plural)
- 🎯 Nombre dominio: "Empleador" (singular, español)

TAREA 2: Refactorizar Entidad Empleador con DDD

AUTORIZACIÓN COMPLETA:
✅ Crear Empleador.cs en Domain/Entities/Empleadores/
✅ Crear EmpleadorConfiguration.cs en Infrastructure/Configurations/
✅ Crear 3 Domain Events (EmpleadorCreado, PerfilActualizado, FotoActualizada)
✅ Actualizar MiGenteDbContext.cs
✅ Ejecutar dotnet build y corregir errores
✅ Reportar resultado cuando termine

LÍMITES:
⛔ NO ejecutar migraciones
⛔ NO modificar proyecto Legacy
⛔ NO crear tests aún

INICIO AHORA: Ejecuta los 5 pasos sin pausas
```

---

## 📊 DIFERENCIAS CON CREDENCIAL

### Credencial (Tarea 1 ✅)
- **Propósito:** Autenticación y seguridad
- **Tabla:** "Credenciales"
- **Relaciones:** 1:1 con Usuario
- **Complejidad:** Alta (BCrypt, eventos de seguridad)

### Empleador (Tarea 2 🚀)
- **Propósito:** Perfil de empleador
- **Tabla:** "Ofertantes" (legacy plural)
- **Relaciones:** 1:1 con Credencial (FK UserId)
- **Complejidad:** Media (perfil, foto, validaciones)

---

## 🎯 ESTRUCTURA DE EMPLEADOR

### Propiedades
- `Id` (int)
- `FechaPublicacion` (DateTime?)
- `UserId` (string) - FK a Credencial
- `Habilidades` (string? max 200)
- `Experiencia` (string? max 200)
- `Descripcion` (string? max 500)
- `Foto` (byte[]?)

### Domain Methods
- `Create()` - Factory method
- `ActualizarPerfil()` - Actualiza habilidades, experiencia, descripción
- `ActualizarFoto()` - Sube nueva foto (validar tamaño)
- `EliminarFoto()` - Elimina foto actual
- `PuedePublicarOfertas()` - Validación de negocio

### Domain Events
- `EmpleadorCreadoEvent` - Nuevo empleador registrado
- `PerfilActualizadoEvent` - Perfil modificado
- `FotoActualizadaEvent` - Foto cambiada

---

## ✅ PREREQUISITOS (YA COMPLETADOS)

- [x] Tarea 1 (Credencial) completada
- [x] Clases base DDD creadas (AggregateRoot, DomainEvent, etc.)
- [x] BCryptPasswordHasher implementado
- [x] AuditableEntityInterceptor funcionando
- [x] DbContext configurado con interceptor
- [x] Dependency Injection configurado

---

## 📁 ARCHIVOS QUE SE CREARÁN

```
MiGenteEnLinea.Clean/
├── src/Core/MiGenteEnLinea.Domain/
│   ├── Entities/Empleadores/
│   │   └── Empleador.cs                        # ✅ NUEVO
│   └── Events/Empleadores/
│       ├── EmpleadorCreadoEvent.cs             # ✅ NUEVO
│       ├── PerfilActualizadoEvent.cs           # ✅ NUEVO
│       └── FotoActualizadaEvent.cs             # ✅ NUEVO
│
└── src/Infrastructure/MiGenteEnLinea.Infrastructure/
    ├── Persistence/Configurations/
    │   └── EmpleadorConfiguration.cs           # ✅ NUEVO
    └── Persistence/Contexts/
        └── MiGenteDbContext.cs                 # ✏️ MODIFICAR (+ 2 líneas)
```

---

## 🔍 VALIDACIÓN POST-EJECUCIÓN

Al terminar, Claude debe reportar:

- [ ] ✅ Empleador.cs creado (heredando de AggregateRoot)
- [ ] ✅ EmpleadorConfiguration.cs creado (mapeo a "Ofertantes")
- [ ] ✅ 3 Domain Events creados
- [ ] ✅ DbContext actualizado con `DbSet<Empleador>`
- [ ] ✅ `dotnet build` exitoso (0 errores)
- [ ] ✅ Auditoría automática configurada (heredada)

---

## ⏱️ TIEMPO ESTIMADO

**10-15 minutos** de ejecución autónoma (sin interrupciones)

---

## 📚 ARCHIVOS DE REFERENCIA

### Para Consultar (Claude leerá estos)
- ✅ `MiGenteEnLinea.Clean/TAREA_1_CREDENCIAL_COMPLETADA.md` - Ejemplo exitoso
- ✅ `MiGenteEnLinea.Clean/src/Core/MiGenteEnLinea.Domain/Entities/Authentication/Credencial.cs` - Patrón
- ✅ `prompts/AGENT_MODE_INSTRUCTIONS.md` - Instrucciones modo agente

### Entidades Legacy (Solo consulta - NO MODIFICAR)
- 📚 `Codigo Fuente Mi Gente/MiGente_Front/Data/Ofertantes.cs`
- 📚 `MiGenteEnLinea.Clean/src/Infrastructure/Persistence/Entities/Generated/Ofertante.cs`

---

## 🎉 DESPUÉS DE COMPLETAR

Una vez que Claude reporte éxito:

1. ✅ Verifica que compile: `dotnet build`
2. ✅ Revisa los archivos creados
3. ✅ Procede con **Tarea 3: Contratista** (mismo patrón)

---

## 🚀 ¡EJECUTA AHORA!

**Copia el comando de arriba y pégalo en Claude.** 

El agente ejecutará todos los pasos autónomamente y reportará cuando termine.

---

_Creado: 12 de octubre, 2025_  
_Prerequisito: Tarea 1 ✅_  
_Siguiente: Tarea 3 (Contratista)_
