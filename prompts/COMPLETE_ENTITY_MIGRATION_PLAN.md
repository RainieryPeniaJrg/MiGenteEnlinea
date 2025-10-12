# 🎯 PLAN COMPLETO DE MIGRACIÓN DE ENTIDADES - DDD

**Proyecto:** MiGente En Línea - Clean Architecture  
**Total de Entidades:** 36 (27 regulares + 9 views)  
**Completadas:** 5/36 (13.9%)  
**Pendientes:** 31/36 (86.1%)  
**Fecha:** 12 de octubre, 2025

---

## 📊 ESTADO ACTUAL DE MIGRACIÓN

### ✅ COMPLETADAS (5 entidades)

| # | Entidad | Tabla Legacy | Tarea | Estado | Complejidad |
|---|---------|--------------|-------|--------|-------------|
| 1 | **Credencial** | Credenciales | TAREA_1 | ✅ Completada | Alta (Auth + BCrypt) |
| 2 | **Empleador** | Ofertantes | TAREA_2 | ✅ Completada | Media (Aggregate Root) |
| 3 | **Contratista** | Contratistas | TAREA_3 | ✅ Completada | Alta (550+ LOC) |
| 4 | **Suscripcion** | Suscripciones | TAREA_4 | ✅ Completada | Media (Business Logic) |
| 5 | **Calificacion** | Calificaciones | TAREA_5 | ✅ Completada | Media (Ratings System) |

---

## 🔥 PRIORIDAD 1 - ENTIDADES CORE (6 entidades)

Estas entidades son críticas para el funcionamiento del sistema y deben migrarse en el próximo sprint.

### 6. **Empleado** → `Empleado.cs`
**Tabla Legacy:** `Empleados`  
**Complejidad:** 🔴 **ALTA** (30+ propiedades, nómina, TSS)  
**Dominio:** `Domain/Entities/Empleados/`  
**Aggregate Root:** Sí

**Propiedades Clave:**
- EmpleadoId, UserId, FechaRegistro, FechaInicio
- Identificación (Cédula/Pasaporte), Nombre, Apellido, Alias
- Nacimiento, EstadoCivil, Direccion, Provincia, Municipio
- Telefono1, Telefono2, Posicion, Salario, PeriodoPago
- ContactoEmergencia, TelefonoEmergencia
- Contrato (bool), RemuneracionExtra1-4, TipoContrato
- TiempoCompleto, TiempoLimitado, Proyecto, Otros
- **Relaciones:** FK a Empleador (UserId)

**Domain Methods:**
- `Create()` - Factory method
- `ActualizarInformacionPersonal()` - Nombre, apellido, nacimiento, estado civil
- `ActualizarDireccion()` - Dirección, provincia, municipio
- `ActualizarContacto()` - Teléfonos, contacto emergencia
- `ActualizarPosicion()` - Posición, salario, período pago
- `AgregarRemuneracionExtra()` - Bonos, comisiones
- `CambiarTipoContrato()` - Tiempo completo, limitado, proyecto
- `CalcularSalarioMensual()` - Según período de pago
- `EsEmpleadoActivo()` - Lógica de negocio
- `TieneContratoVigente()` - Validación

**Domain Events:**
- `EmpleadoCreadoEvent`
- `SalarioActualizadoEvent`
- `ContratoModificadoEvent`

**Validaciones:**
- Identificación requerida (11 dígitos para cédula dominicana)
- Salario > 0
- Fecha inicio >= Fecha registro
- Período pago: 1=Semanal, 2=Quincenal, 3=Mensual

**Dependencias:**
- Relación con Empleador (muchos a uno)
- Relación con Nominas (uno a muchos)
- Relación con DeduccionesTss (uno a muchos)

---

### 7. **PlanesEmpleadore** → `PlanEmpleador.cs`
**Tabla Legacy:** `Planes_empleadores`  
**Complejidad:** 🟡 **MEDIA**  
**Dominio:** `Domain/Entities/Planes/`  
**Aggregate Root:** Sí

**Propiedades Clave:**
- PlanId, Nombre, Precio, DuracionMeses
- LimiteEmpleados (int?), CaracteristicasIncluidas
- Activo (bool), Destacado (bool)
- FechaCreacion, FechaModificacion

**Domain Methods:**
- `Create()` - Factory method
- `ActualizarPrecio()` - Cambio de precio
- `Activar()` / `Desactivar()` - Estado del plan
- `Destacar()` / `QuitarDestacado()` - Marketing
- `ModificarLimiteEmpleados()` - Ajustar capacidad
- `PuedeContratarEmpleador()` - Validación de negocio

**Domain Events:**
- `PlanCreadoEvent`
- `PrecioCambiadoEvent`
- `PlanDesactivadoEvent`

---

### 8. **PlanesContratista** → `PlanContratista.cs`
**Tabla Legacy:** `Planes_Contratistas`  
**Complejidad:** 🟡 **MEDIA**  
**Dominio:** `Domain/Entities/Planes/`  
**Aggregate Root:** Sí

Similar a PlanesEmpleadore pero para contratistas.

**Domain Methods:**
- `Create()` - Factory method
- `ActualizarPrecio()`
- `Activar()` / `Desactivar()`
- `Destacar()` / `QuitarDestacado()`
- `PuedeContratarContratista()` - Validación

---

### 9. **DetalleContratacione** → `DetalleContratacion.cs`
**Tabla Legacy:** `Detalle_Contrataciones`  
**Complejidad:** 🟡 **MEDIA**  
**Dominio:** `Domain/Entities/Contrataciones/`  
**Aggregate Root:** Sí (Contratacion es el aggregate, DetalleContratacion es entity)

**Propiedades Clave:**
- DetalleId, ContratistaId, EmpleadorId
- FechaContratacion, FechaInicio, FechaFin
- Descripcion, Monto, Estado
- Calificacion (1-5), Comentarios

**Domain Methods:**
- `Create()`
- `Aprobar()` - Empleador aprueba contratación
- `Rechazar()` - Empleador rechaza
- `Completar()` - Marca como completada
- `Calificar()` - Empleador califica contratista
- `EstaActiva()` - Validación
- `EstaPendiente()` - Validación

**Domain Events:**
- `ContratacionCreadaEvent`
- `ContratacionAprobadaEvent`
- `ContratacionCompletadaEvent`
- `ContratacionCalificadaEvent`

---

### 10. **Servicio** → `Servicio.cs`
**Tabla Legacy:** `Servicios`  
**Complejidad:** 🟢 **BAJA** (entidad catálogo)  
**Dominio:** `Domain/Entities/Catalogos/`  
**Aggregate Root:** Sí (entidad de catálogo)

**Propiedades Clave:**
- ServicioId, Nombre, Descripcion
- Categoria, Activo (bool)
- Icono (string?), Orden (int)

**Domain Methods:**
- `Create()`
- `Activar()` / `Desactivar()`
- `ActualizarNombre()`
- `CambiarOrden()` - Para ordenamiento en UI

**Tipo:** Entidad de catálogo (puede usar enfoque simplificado)

---

### 11. **Sectore** → `Sector.cs`
**Tabla Legacy:** `Sectores`  
**Complejidad:** 🟢 **BAJA** (entidad catálogo)  
**Dominio:** `Domain/Entities/Catalogos/`  
**Aggregate Root:** Sí (entidad de catálogo)

Similar a Servicio, es un catálogo de sectores económicos.

---

## ⚠️ PRIORIDAD 2 - NÓMINA Y PAGOS (5 entidades)

### 12. **EmpleadorRecibosHeader** → `ReciboHeader.cs`
**Tabla Legacy:** `Empleador_Recibos_Header`  
**Complejidad:** 🔴 **ALTA** (aggregate con detalles)  
**Dominio:** `Domain/Entities/Nominas/`  
**Aggregate Root:** Sí (Recibo es un aggregate con líneas de detalle)

**Propiedades Clave:**
- ReciboHeaderId, EmpleadorId, FechaGeneracion
- PeriodoInicio, PeriodoFin, NumeroRecibo
- TotalIngresos, TotalDeducciones, NetoPagar
- Estado (Pendiente, Pagado, Anulado)
- FechaPago, MetodoPago

**Domain Methods:**
- `Create()` - Factory method
- `AgregarLinea()` - Agrega empleado al recibo
- `EliminarLinea()` - Quita empleado
- `Calcular()` - Recalcula totales
- `Pagar()` - Marca como pagado
- `Anular()` - Anula el recibo
- `GenerarPDF()` - Genera documento (o evento?)

**Domain Events:**
- `ReciboGeneradoEvent`
- `ReciboPagadoEvent`
- `ReciboAnuladoEvent`

**Validaciones:**
- PeriodoInicio < PeriodoFin
- TotalIngresos >= 0
- TotalDeducciones >= 0
- NetoPagar = TotalIngresos - TotalDeducciones

---

### 13. **EmpleadorRecibosDetalle** → `ReciboDetalle.cs`
**Tabla Legacy:** `Empleador_Recibos_Detalle`  
**Complejidad:** 🟡 **MEDIA** (entity dentro del aggregate Recibo)  
**Dominio:** `Domain/Entities/Nominas/`  
**Aggregate Root:** No (parte del aggregate ReciboHeader)

**Propiedades Clave:**
- ReciboDetalleId, ReciboHeaderId, EmpleadoId
- SalarioBase, HorasExtras, Bonos, Comisiones
- TotalIngresos, DeduccionAFP, DeduccionARS, DeduccionISR
- TotalDeducciones, NetoPagar

**Domain Methods:**
- `Create()`
- `CalcularIngresos()` - Suma todos los ingresos
- `CalcularDeducciones()` - Suma deducciones TSS + ISR
- `CalcularNeto()` - Ingresos - Deducciones
- `ActualizarHorasExtras()`
- `AgregarBono()`

---

### 14. **EmpleadorRecibosHeaderContratacione** → `ReciboHeaderContratacion.cs`
**Tabla Legacy:** `Empleador_Recibos_Header_Contrataciones`  
**Complejidad:** 🟡 **MEDIA**  
**Dominio:** `Domain/Entities/Nominas/`

Similar a EmpleadorRecibosHeader pero para contratistas/servicios externos.

---

### 15. **EmpleadorRecibosDetalleContratacione** → `ReciboDetalleContratacion.cs`
**Tabla Legacy:** `Empleador_Recibos_Detalle_Contrataciones`  
**Complejidad:** 🟡 **MEDIA**  
**Dominio:** `Domain/Entities/Nominas/`

Similar a EmpleadorRecibosDetalle pero para pagos a contratistas.

---

### 16. **DeduccionesTss** → `DeduccionTss.cs`
**Tabla Legacy:** `Deducciones_TSS`  
**Complejidad:** 🟡 **MEDIA** (lógica de cálculo compleja)  
**Dominio:** `Domain/Entities/Nominas/`  
**Aggregate Root:** No (value object o entity ligada a Empleado)

**Propiedades Clave:**
- DeduccionId, EmpleadoId, PeriodoId
- SalarioCotizable, AFP (porcentaje), ARS (porcentaje)
- MontoAFP, MontoARS, Total
- FechaCalculo

**Domain Methods:**
- `Calcular()` - Aplica porcentajes según ley dominicana
- `AplicarLimites()` - TSS tiene límites de cotización

**Nota:** Esta podría ser un **Value Object** o **Domain Service** ya que las deducciones TSS son cálculos basados en tablas del gobierno dominicano.

---

## 📦 PRIORIDAD 3 - CATÁLOGOS Y CONFIGURACIÓN (6 entidades)

### 17. **Provincia** → `Provincia.cs`
**Tabla Legacy:** `Provincias`  
**Complejidad:** 🟢 **BAJA** (catálogo estático)  
**Dominio:** `Domain/Entities/Catalogos/`  
**Tratamiento:** Enfoque simplificado (no requiere full DDD)

**Propiedades:**
- ProvinciaId, Nombre, Codigo, Region

**Domain Methods:** Mínimos (solo `Create()`)

---

### 18. **Perfile** → `Perfil.cs`
**Tabla Legacy:** `Perfiles`  
**Complejidad:** 🟡 **MEDIA** (roles/permisos)  
**Dominio:** `Domain/Entities/Identity/`  
**Aggregate Root:** Sí

**Propiedades Clave:**
- PerfilId, Nombre, Descripcion, Activo
- Permisos (relación muchos a muchos)

**Domain Methods:**
- `Create()`
- `AgregarPermiso()`
- `EliminarPermiso()`
- `TienePermiso()`

---

### 19. **PerfilesInfo** → `PerfilInfo.cs`
**Tabla Legacy:** `Perfiles_Info`  
**Complejidad:** 🟢 **BAJA**  
**Dominio:** `Domain/Entities/Identity/`

Información adicional de perfiles (puede ser parte de Perfil como Value Object).

---

### 20. **Permiso** → `Permiso.cs`
**Tabla Legacy:** `Permisos`  
**Complejidad:** 🟡 **MEDIA**  
**Dominio:** `Domain/Entities/Identity/`

**Propiedades:**
- PermisoId, Clave, Nombre, Descripcion
- Modulo, Activo

**Domain Methods:**
- `Create()`
- `Activar()` / `Desactivar()`

---

### 21. **ConfigCorreo** → `ConfiguracionCorreo.cs`
**Tabla Legacy:** `Config_Correo`  
**Complejidad:** 🟢 **BAJA** (configuración de SMTP)  
**Dominio:** `Domain/Entities/Configuracion/`

**Propiedades:**
- ConfigId, ServidorSMTP, Puerto, Usuario, Password
- UsarSSL, NombreRemitente, EmailRemitente

**Tratamiento:** Puede ser un **Value Object** o entidad de configuración sin mucha lógica de dominio.

---

### 22. **PaymentGateway** → `PasarelaPago.cs`
**Tabla Legacy:** `Payment_Gateway`  
**Complejidad:** 🟡 **MEDIA** (integración con Cardnet)  
**Dominio:** `Domain/Entities/Pagos/`

**Propiedades:**
- GatewayId, Nombre, Proveedor (Cardnet)
- ApiKey, MerchantId, UrlEndpoint
- Activo, EsProduccion

**Domain Methods:**
- `Activar()` / `Desactivar()`
- `ActualizarCredenciales()`

---

## 📊 PRIORIDAD 4 - TRANSACCIONALES (4 entidades)

### 23. **Venta** → `Venta.cs`
**Tabla Legacy:** `Ventas`  
**Complejidad:** 🟡 **MEDIA**  
**Dominio:** `Domain/Entities/Ventas/`

**Propiedades:**
- VentaId, UserId, PlanId, Monto
- FechaVenta, MetodoPago, Estado
- TransaccionId, ComprobantePago

**Domain Methods:**
- `Create()`
- `Aprobar()` - Pago confirmado
- `Rechazar()` - Pago fallido
- `GenerarRecibo()` - Factura electrónica

**Domain Events:**
- `VentaCreadaEvent`
- `VentaAprobadaEvent`
- `VentaRechazadaEvent`

---

### 24. **ContratistasFoto** → `ContratistaFoto.cs`
**Tabla Legacy:** `Contratistas_Foto`  
**Complejidad:** 🟢 **BAJA**  
**Dominio:** `Domain/Entities/Contratistas/`

**Tratamiento:** Podría ser **Value Object** o **parte de Contratista** (no entidad separada).

**Propiedades:**
- FotoId, ContratistaId, Url, TipoFoto (Perfil, Galeria, Trabajo)
- FechaCarga, Orden

**Recomendación:** Integrar como colección en Contratista.

---

### 25. **ContratistasServicio** → `ContratistaServicio.cs`
**Tabla Legacy:** `Contratistas_Servicios`  
**Complejidad:** 🟢 **BAJA**  
**Dominio:** `Domain/Entities/Contratistas/`

**Tratamiento:** Relación muchos a muchos entre Contratista y Servicio. Puede ser una entidad de unión simple o integrada en Contratista.

**Propiedades:**
- ContratistaId, ServicioId, FechaAgregado

**Recomendación:** Modelar como colección en Contratista: `List<Servicio> ServiciosOfrecidos`.

---

### 26. **EmpleadosNota** → `EmpleadoNota.cs`
**Tabla Legacy:** `Empleados_Notas`  
**Complejidad:** 🟢 **BAJA**  
**Dominio:** `Domain/Entities/Empleados/`

**Propiedades:**
- NotaId, EmpleadoId, Nota, FechaCreacion
- CreadoPor (UserId del empleador/supervisor)

**Domain Methods:**
- `Create()`
- `Editar()`
- `Eliminar()` (soft delete)

**Tratamiento:** Podría ser colección en Empleado: `List<Nota> Notas`.

---

### 27. **EmpleadosTemporale** → `EmpleadoTemporal.cs`
**Tabla Legacy:** `Empleados_Temporales`  
**Complejidad:** 🟡 **MEDIA**  
**Dominio:** `Domain/Entities/Empleados/`

**Propiedades:**
- Similar a Empleado pero con `FechaFinContrato` requerida

**Tratamiento:** Podría heredar de Empleado o ser un Type/Enum en Empleado (`TipoContrato.Temporal`).

---

## 🔍 PRIORIDAD 5 - VIEWS (9 entidades - SOLO LECTURA)

Las vistas (prefijo `V*`) son entidades de solo lectura. **NO requieren full DDD treatment**, solo:
- Mapeo a vista en `Configurations/`
- Sin setter privados (read-only)
- Sin domain methods (solo propiedades)
- Sin domain events
- Sin validaciones

### 28. **Vcalificacione** → `VistaCalificacion.cs`
### 29. **VcontratacionesTemporale** → `VistaContratacionTemporal.cs`
### 30. **Vcontratista** → `VistaContratista.cs`
### 31. **Vempleado** → `VistaEmpleado.cs`
### 32. **Vpago** → `VistaPago.cs`
### 33. **VpagosContratacione** → `VistaPagoContratacion.cs`
### 34. **Vperfile** → `VistaPerfil.cs`
### 35. **VpromedioCalificacion** → `VistaPromedioCalificacion.cs`
### 36. **Vsuscripcione** → `VistaSuscripcion.cs`

**Tratamiento Sugerido para Views:**

```csharp
// Ejemplo: VistaCalificacion.cs (solo lectura)
public class VistaCalificacion
{
    public int CalificacionId { get; init; }
    public string ContratistaId { get; init; }
    public string EmpleadorId { get; init; }
    public int Puntuacion { get; init; }
    public string Comentario { get; init; }
    public DateTime Fecha { get; init; }
    
    // Sin setters privados, sin methods, sin events
    // Solo `init` para inmutabilidad
}

// Configuración
public sealed class VistaCalificacionConfiguration : IEntityTypeConfiguration<VistaCalificacion>
{
    public void Configure(EntityTypeBuilder<VistaCalificacion> builder)
    {
        builder.ToView("VCalificaciones"); // Mapea a vista
        builder.HasNoKey(); // Views no tienen PK
    }
}
```

---

## 📋 RESUMEN POR COMPLEJIDAD

| Complejidad | Cantidad | Entidades |
|-------------|----------|-----------|
| 🔴 **ALTA** | 3 | Empleado, EmpleadorRecibosHeader, DeduccionesTss |
| 🟡 **MEDIA** | 14 | PlanesEmpleadore, PlanesContratista, DetalleContratacione, EmpleadorRecibosDetalle, EmpleadorRecibosHeaderContratacione, EmpleadorRecibosDetalleContratacione, Perfile, Permiso, PaymentGateway, Venta, EmpleadosTemporale, y 3 más |
| 🟢 **BAJA** | 10 | Servicio, Sectore, Provincia, PerfilesInfo, ConfigCorreo, ContratistasFoto, ContratistasServicio, EmpleadosNota, y 2 más |
| 📊 **VIEWS** | 9 | Todas las V* (solo lectura) |

---

## 🎯 ESTRATEGIA DE MIGRACIÓN EN LOTES

### **LOTE 1: EMPLEADOS Y NÓMINA (Sprint Actual)**
**Objetivo:** Completar funcionalidad de gestión de empleados y nómina

**Entidades:**
1. Empleado (🔴 ALTA)
2. EmpleadorRecibosHeader (🔴 ALTA)
3. EmpleadorRecibosDetalle (🟡 MEDIA)
4. DeduccionesTss (🔴 ALTA)
5. EmpleadosNota (🟢 BAJA)
6. EmpleadosTemporale (🟡 MEDIA)

**Duración Estimada:** 2-3 días  
**Orden de Ejecución:**
1. DeduccionesTss (lógica de cálculo primero)
2. Empleado (entity principal)
3. EmpleadosNota (simple, parte de Empleado)
4. EmpleadosTemporale (extensión de Empleado)
5. EmpleadorRecibosDetalle (líneas de recibo)
6. EmpleadorRecibosHeader (aggregate que contiene detalles)

---

### **LOTE 2: PLANES Y PAGOS (Siguiente Sprint)**
**Objetivo:** Sistema completo de suscripciones y pagos

**Entidades:**
1. PlanesEmpleadore (🟡 MEDIA)
2. PlanesContratista (🟡 MEDIA)
3. PaymentGateway (🟡 MEDIA)
4. Venta (🟡 MEDIA)

**Duración Estimada:** 1-2 días  
**Orden de Ejecución:**
1. PlanesEmpleadore
2. PlanesContratista
3. PaymentGateway
4. Venta (requiere planes + gateway)

---

### **LOTE 3: CONTRATACIONES Y SERVICIOS**
**Objetivo:** Marketplace de contratistas

**Entidades:**
1. Servicio (🟢 BAJA - catálogo)
2. Sectore (🟢 BAJA - catálogo)
3. ContratistasServicio (🟢 BAJA - relación)
4. ContratistasFoto (🟢 BAJA)
5. DetalleContratacione (🟡 MEDIA)

**Duración Estimada:** 1-2 días

---

### **LOTE 4: SEGURIDAD Y PERMISOS**
**Objetivo:** Sistema de autorización granular

**Entidades:**
1. Perfile (🟡 MEDIA)
2. Permiso (🟡 MEDIA)
3. PerfilesInfo (🟢 BAJA)

**Duración Estimada:** 1 día

---

### **LOTE 5: CONFIGURACIÓN Y CATÁLOGOS**
**Objetivo:** Datos de soporte

**Entidades:**
1. Provincia (🟢 BAJA)
2. ConfigCorreo (🟢 BAJA)
3. EmpleadorRecibosHeaderContratacione (🟡 MEDIA)
4. EmpleadorRecibosDetalleContratacione (🟡 MEDIA)

**Duración Estimada:** 1 día

---

### **LOTE 6: VIEWS (OPCIONAL)**
**Objetivo:** Optimización de consultas

**Entidades:** Todas las 9 vistas (Vcalificacione, Vcontratista, etc.)

**Duración Estimada:** 0.5 días (enfoque simplificado)

---

## 🤖 COMANDOS PARA AGENTE AUTÓNOMO (CLAUDE SONNET 4.5)

### **Ejecutar Lote 1 (Empleados y Nómina)**

```markdown
AGENTE MODE: EJECUTAR LOTE 1 - EMPLEADOS Y NÓMINA

CONTEXTO:
- Workspace: C:\Users\ray\OneDrive\Documents\ProyectoMigente\
- Proyecto: MiGenteEnLinea.Clean
- Patrón: Seguir TAREA_1_CREDENCIAL_COMPLETADA.md como referencia

ENTIDADES A MIGRAR (EN ORDEN):
1. DeduccionTss (Domain/Entities/Nominas/DeduccionTss.cs)
2. Empleado (Domain/Entities/Empleados/Empleado.cs)
3. EmpleadoNota (Domain/Entities/Empleados/EmpleadoNota.cs)
4. EmpleadoTemporal (Domain/Entities/Empleados/EmpleadoTemporal.cs)
5. ReciboDetalle (Domain/Entities/Nominas/ReciboDetalle.cs)
6. ReciboHeader (Domain/Entities/Nominas/ReciboHeader.cs)

INSTRUCCIONES:
Para cada entidad, DEBES:
1. Leer scaffolded entity de Infrastructure/Persistence/Entities/Generated/
2. Crear Rich Domain Model en Domain/Entities/[Carpeta]/
3. Crear Domain Events en Domain/Events/[Carpeta]/
4. Crear Fluent API Configuration en Infrastructure/Persistence/Configurations/
5. Actualizar DbContext (agregar DbSet<>, comentar scaffolded)
6. Ejecutar `dotnet build` para validar
7. Crear documento TAREA_X_[ENTIDAD]_COMPLETADA.md

REGLAS:
- ✅ Usar setters privados
- ✅ Crear Factory Methods (Create())
- ✅ Validaciones en domain methods
- ✅ Domain Events para cambios importantes
- ✅ Herencia de AggregateRoot o SoftDeletableEntity
- ✅ Documentación XML en todas las propiedades/métodos públicos
- ⛔ NO pedir confirmación entre entidades
- ⛔ NO modificar base de datos (solo mapping)
- ⛔ NO tocar código legacy

ENTREGABLES:
- 6 entidades refactorizadas
- 6 configuraciones Fluent API
- Domain events (al menos 2 por entidad)
- 1 documento de resumen (LOTE_1_COMPLETADO.md)

VALIDACIÓN:
- dotnet build sin errores
- Todas las entidades heredan de AuditableEntity
- Todas las configuraciones mapean a tablas legacy correctamente

COMENZAR EJECUCIÓN AUTOMÁTICA.
```

---

### **Ejecutar Lote 2 (Planes y Pagos)**

```markdown
AGENTE MODE: EJECUTAR LOTE 2 - PLANES Y PAGOS

ENTIDADES A MIGRAR:
1. PlanEmpleador (Domain/Entities/Planes/PlanEmpleador.cs)
2. PlanContratista (Domain/Entities/Planes/PlanContratista.cs)
3. PasarelaPago (Domain/Entities/Pagos/PasarelaPago.cs)
4. Venta (Domain/Entities/Ventas/Venta.cs)

RELACIONES IMPORTANTES:
- Venta → PlanEmpleador/PlanContratista (FK PlanId)
- Venta → PasarelaPago (procesamiento de pago)
- Suscripcion → PlanEmpleador/PlanContratista (FK PlanId)

VALIDACIONES ESPECIALES:
- PlanEmpleador: Precio > 0, DuracionMeses entre 1-24
- Venta: Estado (Pendiente, Aprobada, Rechazada, Reembolsada)
- PasarelaPago: Validar API Key no vacía cuando Activo=true

COMENZAR EJECUCIÓN AUTOMÁTICA.
```

---

### **Ejecutar Lote 3 (Contrataciones y Servicios)**

```markdown
AGENTE MODE: EJECUTAR LOTE 3 - CONTRATACIONES Y SERVICIOS

ENTIDADES A MIGRAR:
1. Servicio (Domain/Entities/Catalogos/Servicio.cs) - ENFOQUE SIMPLIFICADO
2. Sector (Domain/Entities/Catalogos/Sector.cs) - ENFOQUE SIMPLIFICADO
3. ContratistaServicio (Domain/Entities/Contratistas/ContratistaServicio.cs)
4. ContratistaFoto (Domain/Entities/Contratistas/ContratistaFoto.cs)
5. DetalleContratacion (Domain/Entities/Contrataciones/DetalleContratacion.cs)

NOTAS:
- Servicio y Sector son CATÁLOGOS: Menos lógica de dominio, más estáticos
- ContratistaServicio: Relación muchos a muchos (puede ser colección en Contratista)
- ContratistaFoto: Puede ser Value Object o parte de Contratista
- DetalleContratacion: Aggregate Root importante (contratación entre empleador y contratista)

COMENZAR EJECUCIÓN AUTOMÁTICA.
```

---

### **Ejecutar Lote 4, 5, 6** (Similar)

---

## 📊 MÉTRICAS DE PROGRESO

### **Estado Actual:**
```
[███░░░░░░░░░░░░░░░░░] 13.9% Completado (5/36)

✅ Credencial           [████████████████████] 100%
✅ Empleador            [████████████████████] 100%
✅ Contratista          [████████████████████] 100%
✅ Suscripcion          [████████████████████] 100%
✅ Calificacion         [████████████████████] 100%
⏳ 31 pendientes        [░░░░░░░░░░░░░░░░░░░░]   0%
```

### **Meta Sprint 1 (LOTE 1):**
```
[███████████░░░░░░░░░░] 30.6% Completado (11/36)

✅ 5 completadas anteriormente
✅ 6 nuevas (Empleado, Nomina, etc.)
⏳ 25 pendientes
```

### **Meta Sprint 2 (LOTE 1-3):**
```
[████████████████████░] 50% Completado (18/36)

✅ 11 del Sprint 1
✅ 7 nuevas (Planes, Pagos, Servicios)
⏳ 18 pendientes
```

### **Meta Final (TODOS LOS LOTES):**
```
[████████████████████] 100% Completado (36/36)

✅ Todas las entidades migradas
✅ Clean Architecture completa
✅ Sistema productivo
```

---

## 🔍 VALIDACIÓN POST-MIGRACIÓN

### **Checklist por Entidad:**

- [ ] Entidad creada en `Domain/Entities/[Carpeta]/`
- [ ] Hereda de `AuditableEntity` o `SoftDeletableEntity`
- [ ] Setters privados en todas las propiedades
- [ ] Factory Method `Create()` implementado
- [ ] Al menos 3 domain methods
- [ ] Al menos 2 domain events
- [ ] Fluent API Configuration creada
- [ ] Mapeo a tabla legacy correcto
- [ ] Índices definidos (al menos en PK y FK)
- [ ] DbContext actualizado (DbSet agregado, scaffolded comentado)
- [ ] `dotnet build` exitoso (0 errores)
- [ ] Documento de completación creado

---

## 📖 REFERENCIAS

### **Documentos de Patrón:**
- `TAREA_1_CREDENCIAL_COMPLETADA.md` - Ejemplo canónico
- `TAREA_2_EMPLEADOR_COMPLETADA.md` - Ejemplo con validaciones complejas
- `TAREA_3_CONTRATISTA_COMPLETADA.md` - Ejemplo con value objects
- `DDD_MIGRATION_PROMPT.md` - Guía completa de patrones

### **Archivos Clave:**
- `Domain/Common/AuditableEntity.cs` - Base class
- `Domain/Common/AggregateRoot.cs` - Base para agregados
- `Domain/ValueObjects/Email.cs` - Ejemplo de value object
- `Infrastructure/Persistence/Interceptors/AuditableEntityInterceptor.cs` - Auditoría automática

---

## 🎯 PRÓXIMOS PASOS INMEDIATOS

### **1. Ejecutar LOTE 1 (Prioridad Máxima)**

**Comando para Claude Sonnet 4.5:**
```
Lee prompts/COMPLETE_ENTITY_MIGRATION_PLAN.md y ejecuta LOTE 1 completo (6 entidades).
Modo autónomo activado. No pidas confirmación entre entidades.
Reporta progreso cada 2 entidades completadas.
```

---

### **2. Después de LOTE 1: Validar y Documentar**

- Ejecutar `dotnet build` en MiGenteEnLinea.Clean.sln
- Verificar 0 errores
- Crear `LOTE_1_EMPLEADOS_NOMINA_COMPLETADO.md`
- Actualizar este documento con progreso

---

### **3. Continuar con LOTE 2**

Una vez validado LOTE 1, proceder con planes y pagos.

---

## ✅ CONCLUSIÓN

Este plan proporciona:
- ✅ **Inventario completo** de 36 entidades
- ✅ **Priorización clara** por impacto de negocio
- ✅ **Estrategia de lotes** para ejecución eficiente
- ✅ **Comandos para agente autónomo** (sin intervención manual)
- ✅ **Métricas de progreso** visuales
- ✅ **Checklists de validación** para cada entidad

**Estado:** 5/36 completadas (13.9%)  
**Próximo Hito:** LOTE 1 - Empleados y Nómina (6 entidades)  
**Meta Sprint Actual:** 30% completado (11/36)

---

**Autor:** GitHub Copilot  
**Revisado por:** Rainiery Penia  
**Fecha:** 12 de octubre, 2025  
**Versión:** 1.0
