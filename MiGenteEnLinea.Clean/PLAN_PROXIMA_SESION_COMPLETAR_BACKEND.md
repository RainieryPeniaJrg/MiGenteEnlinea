# 🚀 PLAN PRÓXIMA SESIÓN: Completar Backend al 100%

**Fecha prevista:** 2025-10-22  
**Objetivo:** Cerrar los 18 endpoints faltantes y llegar al 100% de paridad con Legacy  
**Estado actual:** 77% (63/81 endpoints) ✅  
**Meta:** 100% (81/81 endpoints) 🎯  
**Tiempo estimado total:** 18-22 horas de desarrollo + 6-8 horas de testing

---

## 📊 RESUMEN EJECUTIVO

### Endpoints Pendientes por Módulo

```
TOTAL PENDIENTE: 18 endpoints

Distribución:
- Empleados/Nomina ........ 6 endpoints (LOTE 6.0.2) - 4-5h
- Contratistas ............ 4 endpoints (LOTE 6.0.4) - 3-4h  
- Contrataciones .......... 8 endpoints (LOTE 6.0.3) - 8-10h
- Suscripciones ........... 1 endpoint  (LOTE 6.0.5) - 1-2h
- Pagos/Bot ............... opcional (no crítico)
```

### Estrategia de Ejecución

**Prioridad 1 (CRÍTICO):** LOTE 6.0.2 Empleados - Base del sistema  
**Prioridad 2 (ALTA):** LOTE 6.0.4 Contratistas - Marketplace  
**Prioridad 3 (ALTA):** LOTE 6.0.3 Contrataciones - Complejidad alta  
**Prioridad 4 (MEDIA):** LOTE 6.0.5 Suscripciones - 1 endpoint simple  
**Prioridad 5 (OPCIONAL):** Testing exhaustivo y documentación final

---

## 🎯 LOTE 6.0.2: Empleados - Remuneraciones & TSS

**Tiempo estimado:** 4-5 horas  
**Prioridad:** 🔴 CRÍTICA (base del sistema de nómina)  
**Complejidad:** 🟡 MEDIA (API externa + batch operations)

### Endpoints a Implementar (6)

#### Endpoint 1: GET /api/empleados/{empleadoId}/remuneraciones

**Descripción:** Obtener lista de remuneraciones de un empleado  
**Legacy:** EmpleadosService.getRemuneraciones(int empleadoID)  
**Complejidad:** 🟢 Baja - Query simple con filtro

**Tareas:**

- [ ] Crear `GetRemuneracionesByEmpleadoQuery` (Query)
- [ ] Crear `RemuneracionDto` con campos: id, empleadoId, concepto, monto, activo
- [ ] Implementar Handler con LINQ a `Empleados_Remuneraciones`
- [ ] Agregar endpoint en `EmpleadosController`
- [ ] Probar con Swagger UI

**Tiempo:** 30 minutos

**Query ejemplo:**

```csharp
var remuneraciones = await _context.EmpleadosRemuneraciones
    .Where(r => r.EmpleadoID == empleadoId && r.Activo)
    .Select(r => new RemuneracionDto
    {
        Id = r.ID,
        EmpleadoId = r.EmpleadoID,
        Concepto = r.Concepto,
        Monto = r.Monto,
        Activo = r.Activo
    })
    .ToListAsync();
```

---

#### Endpoint 2: DELETE /api/remuneraciones/{remuneracionId}

**Descripción:** Eliminar una remuneración (soft delete)  
**Legacy:** EmpleadosService.eliminarRemuneracion(int remuneracionID)  
**Complejidad:** 🟢 Baja - Update simple

**Tareas:**

- [ ] Crear `DeleteRemuneracionCommand` (Command)
- [ ] Implementar Handler con soft delete (Activo = false)
- [ ] Validar que remuneración exista
- [ ] Agregar endpoint en `EmpleadosController`
- [ ] Probar con Swagger UI

**Tiempo:** 30 minutos

**Validaciones:**

- Remuneración existe
- No está ya eliminada (Activo = true)

---

#### Endpoint 3: POST /api/empleados/{empleadoId}/remuneraciones/batch

**Descripción:** Agregar múltiples remuneraciones a la vez  
**Legacy:** EmpleadosService.agregarRemuneracionesBatch(int empleadoID, List<Remuneracion>)  
**Complejidad:** 🟡 Media - Batch insert + validaciones

**Tareas:**

- [ ] Crear `AddRemuneracionesBatchCommand` (Command)
- [ ] Crear `RemuneracionInputDto` (request)
- [ ] Implementar Handler con AddRange
- [ ] Validar con FluentValidation:
  - Empleado existe
  - Conceptos no vacíos
  - Montos > 0
  - No duplicar conceptos existentes
- [ ] Agregar endpoint en `EmpleadosController`
- [ ] Probar con Swagger UI (3-5 remuneraciones)

**Tiempo:** 1 hora

**Request ejemplo:**

```json
{
  "empleadoId": 123,
  "remuneraciones": [
    { "concepto": "Salario Base", "monto": 35000 },
    { "concepto": "Bono Transporte", "monto": 5000 },
    { "concepto": "Bono Alimentación", "monto": 3000 }
  ]
}
```

---

#### Endpoint 4: PUT /api/empleados/{empleadoId}/remuneraciones/batch

**Descripción:** Actualizar todas las remuneraciones de un empleado (reemplazar)  
**Legacy:** EmpleadosService.actualizarRemuneracionesBatch(int empleadoID, List<Remuneracion>)  
**Complejidad:** 🟡 Media - Delete + Insert en transacción

**Estrategia:**

1. Soft delete todas las remuneraciones existentes (Activo = false)
2. Insertar nuevas remuneraciones
3. Todo en una transacción

**Tareas:**

- [ ] Crear `UpdateRemuneracionesBatchCommand` (Command)
- [ ] Implementar Handler con transacción:
  - Update existentes (Activo = false)
  - AddRange nuevas
  - SaveChanges en transacción
- [ ] Validar con FluentValidation
- [ ] Agregar endpoint en `EmpleadosController`
- [ ] Probar con Swagger UI

**Tiempo:** 1.5 horas

**⚠️ ADVERTENCIA:** Usar transacción para evitar estados inconsistentes

---

#### Endpoint 5: GET /api/empleados/consultar-padron/{cedula}

**Descripción:** Validar cédula contra Padrón Electoral de JCE (API externa)  
**Legacy:** EmpleadosService.consultarPadron(string cedula)  
**Complejidad:** 🔴 Alta - API externa + retry logic + manejo de errores

**API Externa:**

- **URL:** `https://abcportal.online/Sigeinfo/public/api/padron/{cedula}`
- **Método:** GET
- **Auth:** API Key en header `X-API-Key`
- **Response:**

```json
{
  "found": true,
  "cedula": "001-1234567-8",
  "nombre": "JUAN PEREZ GARCIA",
  "fechaNacimiento": "1990-01-15",
  "sexo": "M"
}
```

**Tareas:**

- [ ] Crear `ConsultarPadronQuery` (Query)
- [ ] Crear `PadronElectoralDto` (response)
- [ ] Implementar servicio `IPadronElectoralService` en Infrastructure
- [ ] Agregar HttpClient con Polly (retry 3 veces, timeout 10s)
- [ ] Agregar configuración en appsettings.json:

  ```json
  "PadronElectoral": {
    "ApiUrl": "https://abcportal.online/Sigeinfo/public/api",
    "ApiKey": "tu-api-key-aqui",
    "TimeoutSeconds": 10
  }
  ```

- [ ] Implementar Handler que llame al servicio
- [ ] Agregar endpoint en `EmpleadosController`
- [ ] Probar con cédula real en Swagger UI

**Tiempo:** 2 horas

**Manejo de errores:**

- 404: Cédula no encontrada en padrón
- 500: Error en API externa
- Timeout: Retry automático 3 veces

---

#### Endpoint 6: GET /api/catalogos/deducciones-tss

**Descripción:** Obtener catálogo de deducciones TSS (Seguridad Social)  
**Legacy:** EmpleadosService.getDeduccionesTSS()  
**Complejidad:** 🟢 Baja - Query simple a tabla catálogo

**Tabla:** `Deducciones_TSS`

**Tareas:**

- [ ] Crear `GetDeduccionesTssQuery` (Query)
- [ ] Crear `DeduccionTssDto` con campos: id, codigo, descripcion, porcentaje, aplicaEmpleado, aplicaEmpleador
- [ ] Implementar Handler con LINQ
- [ ] Agregar endpoint en `CatalogosController` (crear si no existe)
- [ ] Probar con Swagger UI

**Tiempo:** 30 minutos

**Response ejemplo:**

```json
[
  {
    "id": 1,
    "codigo": "SFS",
    "descripcion": "Seguro Familiar de Salud",
    "porcentaje": 3.04,
    "aplicaEmpleado": true,
    "aplicaEmpleador": true
  },
  {
    "id": 2,
    "codigo": "AFP",
    "descripcion": "Administradora de Fondos de Pensiones",
    "porcentaje": 2.87,
    "aplicaEmpleado": true,
    "aplicaEmpleador": false
  }
]
```

---

### Resumen LOTE 6.0.2

| # | Endpoint | Tiempo | Complejidad | Crítico |
|---|----------|--------|-------------|---------|
| 1 | GET remuneraciones | 30m | 🟢 Baja | No |
| 2 | DELETE remuneración | 30m | 🟢 Baja | No |
| 3 | POST batch agregar | 1h | 🟡 Media | Sí |
| 4 | PUT batch actualizar | 1.5h | 🟡 Media | Sí |
| 5 | GET consultar-padron | 2h | 🔴 Alta | Sí |
| 6 | GET deducciones-tss | 30m | 🟢 Baja | No |
| **TOTAL** | **6 endpoints** | **5-6h** | | |

**Documentación:** Crear `LOTE_6_0_2_EMPLEADOS_COMPLETADO.md` al finalizar

---

## 🎯 LOTE 6.0.4: Contratistas - Servicios & Activación

**Tiempo estimado:** 3-4 horas  
**Prioridad:** 🟠 ALTA (marketplace de servicios)  
**Complejidad:** 🟡 MEDIA (many-to-many relationship)

### Tabla Nueva: Contratistas_Servicios

**Verificar si existe o crear migración:**

```sql
CREATE TABLE Contratistas_Servicios (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    ContratistaID INT NOT NULL,
    ServicioID INT NOT NULL,
    DetalleServicio NVARCHAR(500),
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ContratistaID) REFERENCES Contratistas(ID),
    FOREIGN KEY (ServicioID) REFERENCES Servicios_Catalogo(ID)
)
```

### Endpoints a Implementar (4)

#### Endpoint 1: GET /api/contratistas/{contratistaId}/servicios

**Descripción:** Obtener servicios ofrecidos por un contratista  
**Legacy:** ContratistasService.getServicios(int contratistaID)  
**Complejidad:** 🟢 Baja - Query con join

**⚠️ NOTA:** Este endpoint YA EXISTE en ContratistasController línea 151  
**Acción:** VERIFICAR si funciona correctamente, NO duplicar

**Tiempo:** 15 minutos (solo verificación)

---

#### Endpoint 2: POST /api/contratistas/{contratistaId}/servicios

**Descripción:** Agregar un servicio al perfil del contratista  
**Legacy:** ContratistasService.agregarServicio(int contratistaID, int servicioID, string detalle)  
**Complejidad:** 🟢 Baja - Insert simple

**⚠️ NOTA:** Este endpoint YA EXISTE en ContratistasController línea ~350  
**Acción:** VERIFICAR si funciona correctamente, NO duplicar

**Tiempo:** 15 minutos (solo verificación)

---

#### Endpoint 3: DELETE /api/contratistas/{contratistaId}/servicios/{servicioId}

**Descripción:** Remover un servicio del perfil del contratista  
**Legacy:** ContratistasService.removerServicio(int contratistaID, int servicioID)  
**Complejidad:** 🟢 Baja - Soft delete

**⚠️ NOTA:** Este endpoint YA EXISTE en ContratistasController línea ~380  
**Acción:** VERIFICAR si funciona correctamente, NO duplicar

**Tiempo:** 15 minutos (solo verificación)

---

#### Endpoint 4: POST /api/contratistas/{contratistaId}/activar

**Descripción:** Activar perfil de contratista (después de verificación)  
**Legacy:** ContratistasService.ActivarPerfil(int contratistaID)  
**Complejidad:** 🟢 Baja - Update booleano

**Tareas:**

- [ ] Crear `ActivarPerfilContratistaCommand` (Command)
- [ ] Implementar Handler:
  - Buscar contratista
  - Actualizar `Activo = true`
  - Actualizar `FechaActivacion = DateTime.UtcNow`
  - SaveChanges
- [ ] Agregar endpoint en `ContratistasController`
- [ ] Probar con Swagger UI

**Tiempo:** 45 minutos

**Request:**

```http
POST /api/contratistas/123/activar
Authorization: Bearer {token}
```

**Response:**

```json
{
  "message": "Perfil activado exitosamente",
  "contratistaId": 123,
  "fechaActivacion": "2025-10-22T10:30:00Z"
}
```

---

#### Endpoint 5: POST /api/contratistas/{contratistaId}/desactivar

**Descripción:** Desactivar perfil de contratista (suspensión temporal)  
**Legacy:** ContratistasService.DesactivarPerfil(int contratistaID, string motivo)  
**Complejidad:** 🟢 Baja - Update booleano

**Tareas:**

- [ ] Crear `DesactivarPerfilContratistaCommand` (Command)
- [ ] Implementar Handler:
  - Buscar contratista
  - Actualizar `Activo = false`
  - Registrar motivo si aplica
  - SaveChanges
- [ ] Agregar endpoint en `ContratistasController`
- [ ] Probar con Swagger UI

**Tiempo:** 45 minutos

**Request:**

```json
{
  "contratistaId": 123,
  "motivo": "Suspensión temporal por revisión de documentos"
}
```

---

### Resumen LOTE 6.0.4

| # | Endpoint | Tiempo | Estado | Acción |
|---|----------|--------|--------|--------|
| 1 | GET servicios | 15m | ✅ Existe | Verificar |
| 2 | POST agregar servicio | 15m | ✅ Existe | Verificar |
| 3 | DELETE remover servicio | 15m | ✅ Existe | Verificar |
| 4 | POST activar perfil | 45m | ❌ Falta | Implementar |
| 5 | POST desactivar perfil | 45m | ❌ Falta | Implementar |
| **TOTAL** | **5 endpoints** | **2-3h** | 3/5 OK | |

**⚠️ IMPORTANTE:** Los primeros 3 endpoints YA EXISTEN. Solo faltan 2 nuevos.

**Documentación:** Crear `LOTE_6_0_4_CONTRATISTAS_COMPLETADO.md` al finalizar

---

## 🎯 LOTE 6.0.3: Contrataciones Temporales (COMPLEJO)

**Tiempo estimado:** 8-10 horas  
**Prioridad:** 🔴 CRÍTICA (lógica más compleja del sistema)  
**Complejidad:** 🔴 ALTA (múltiples tablas, transacciones, cascade deletes)

### ⚠️ ADVERTENCIA CRÍTICA

Este módulo tiene:

- 5 tablas relacionadas (FK en cascada)
- Lógica transaccional compleja
- Cálculos de nómina y TSS
- Generación de documentos PDF
- **REQUIERE testing exhaustivo antes de producción**

### Tablas Involucradas

```
Contrataciones (header)
├── Contrataciones_Detalle (líneas de servicios)
│   └── Contrataciones_Pagos (pagos por servicio)
│       ├── Contrataciones_Recibos_Header (comprobante)
│       └── Contrataciones_Recibos_Detalle (desglose)
└── Calificaciones (opcional)
```

### Endpoints a Implementar (8)

#### Endpoint 1: GET /api/contrataciones/{contratacionId}/detalle/{detalleId}/pagos

**Descripción:** Listar pagos de un servicio específico en una contratación  
**Legacy:** ContratacionesService.getPagosByDetalle(int detalleID)  
**Complejidad:** 🟡 Media - Query con joins

**Tareas:**

- [ ] Crear `GetPagosByDetalleQuery` (Query)
- [ ] Crear `PagoContrataciónDto` con campos completos
- [ ] Implementar Handler con LINQ + Include
- [ ] Agregar endpoint en `ContratacionesController`
- [ ] Probar con Swagger UI

**Tiempo:** 1 hora

**Query LINQ:**

```csharp
var pagos = await _context.ContratacionesPagos
    .Include(p => p.Recibo)
    .Where(p => p.DetalleID == detalleId && p.Activo)
    .Select(p => new PagoContrataciónDto
    {
        Id = p.ID,
        DetalleId = p.DetalleID,
        Monto = p.Monto,
        FechaPago = p.FechaPago,
        ReciboId = p.ReciboID,
        Estado = p.Estado
    })
    .ToListAsync();
```

---

#### Endpoint 2: GET /api/contrataciones/recibos/{pagoId}

**Descripción:** Obtener detalle completo de un recibo (header + detalles)  
**Legacy:** ContratacionesService.getReciboByPagoID(int pagoID)  
**Complejidad:** 🟡 Media - Query con 2 tablas

**Tareas:**

- [ ] Crear `GetReciboByPagoIdQuery` (Query)
- [ ] Crear `ReciboCompletoDto` con header + detalles
- [ ] Implementar Handler con LINQ + Include
- [ ] Agregar endpoint en `ContratacionesController`
- [ ] Probar con Swagger UI

**Tiempo:** 1 hora

**Response ejemplo:**

```json
{
  "header": {
    "id": 789,
    "pagoId": 456,
    "numeroRecibo": "REC-2025-001234",
    "fecha": "2025-10-22",
    "montoTotal": 45000,
    "empleadorNombre": "Empresa ABC",
    "contratistaNombre": "Juan Pérez"
  },
  "detalles": [
    { "concepto": "Servicio de plomería", "monto": 40000 },
    { "concepto": "TSS Contratista", "monto": 5000 }
  ]
}
```

---

#### Endpoint 3: POST /api/contrataciones/{contratacionId}/detalle/{detalleId}/cancelar

**Descripción:** Cancelar un servicio específico de una contratación  
**Legacy:** ContratacionesService.cancelarDetalle(int detalleID, string motivo)  
**Complejidad:** 🟢 Baja - Update status

**Tareas:**

- [ ] Crear `CancelarDetalleContratacionCommand` (Command)
- [ ] Implementar Handler:
  - Validar que detalle no tenga pagos procesados
  - Actualizar `Estado = "Cancelado"`
  - Registrar motivo y fecha
  - SaveChanges
- [ ] Validar con FluentValidation
- [ ] Agregar endpoint en `ContratacionesController`
- [ ] Probar con Swagger UI

**Tiempo:** 1 hora

**Validación crítica:** No permitir cancelar si hay pagos ya realizados

---

#### Endpoint 4: DELETE /api/contrataciones/recibos/{reciboId}

**Descripción:** Eliminar un recibo (header + detalles en cascada)  
**Legacy:** ContratacionesService.eliminarRecibo(int reciboID)  
**Complejidad:** 🟡 Media - Delete en 2 tablas con transacción

**Estrategia:**

1. Verificar que recibo no esté "cerrado" (proceso contable)
2. Eliminar detalles (Contrataciones_Recibos_Detalle)
3. Eliminar header (Contrataciones_Recibos_Header)
4. Todo en transacción

**Tareas:**

- [ ] Crear `DeleteReciboCommand` (Command)
- [ ] Implementar Handler con transacción:
  - Verificar estado (no "Cerrado")
  - Remove detalles
  - Remove header
  - SaveChanges en transacción
- [ ] Validar con FluentValidation
- [ ] Agregar endpoint en `ContratacionesController`
- [ ] Probar con Swagger UI

**Tiempo:** 1.5 horas

**⚠️ CRÍTICO:** Usar transacción explícita (`BeginTransaction`)

---

#### Endpoint 5: DELETE /api/contrataciones/{contratacionId}

**Descripción:** Eliminar una contratación completa (CASCADE 3+ tablas)  
**Legacy:** ContratacionesService.eliminarContratacion(int contratacionID)  
**Complejidad:** 🔴 Alta - Cascade delete complejo

**Orden de eliminación:**

1. Contrataciones_Recibos_Detalle (nietos)
2. Contrataciones_Recibos_Header (nietos)
3. Contrataciones_Pagos (hijos)
4. Contrataciones_Detalle (hijos)
5. Contrataciones (padre)

**Tareas:**

- [ ] Crear `DeleteContratacionCommand` (Command)
- [ ] Implementar Handler con transacción:
  - Verificar que NO tenga pagos procesados (validación de negocio)
  - Eliminar en orden correcto
  - SaveChanges en transacción con rollback
- [ ] Validar con FluentValidation
- [ ] Agregar endpoint en `ContratacionesController`
- [ ] **TESTING CRÍTICO en base de datos de prueba**

**Tiempo:** 2 horas

**⚠️ PELIGRO:** Este endpoint puede destruir datos. Implementar soft delete si es posible.

---

#### Endpoint 6: POST /api/contrataciones/{contratacionId}/calificar

**Descripción:** Agregar calificación a una contratación (empleador califica contratista)  
**Legacy:** ContratacionesService.calificarContratacion(int contratacionID, int puntuacion, string comentario)  
**Complejidad:** 🟢 Baja - Insert en tabla Calificaciones

**Tareas:**

- [ ] Crear `CalificarContratacionCommand` (Command)
- [ ] Implementar Handler:
  - Verificar que contratación esté completada
  - Insertar en tabla Calificaciones
  - Actualizar flag `Calificada = true` en Contrataciones
  - Actualizar promedio en perfil Contratista
  - SaveChanges
- [ ] Validar con FluentValidation (puntuación 1-5)
- [ ] Agregar endpoint en `ContratacionesController`
- [ ] Probar con Swagger UI

**Tiempo:** 1 hora

**Request:**

```json
{
  "contratacionId": 123,
  "puntuacion": 5,
  "comentario": "Excelente trabajo, muy profesional"
}
```

---

#### Endpoint 7: GET /api/contrataciones/{contratacionId}/vista

**Descripción:** Obtener vista completa de una contratación (todos los datos relacionados)  
**Legacy:** ContratacionesService.getVistaCompleta(int contratacionID)  
**Complejidad:** 🟡 Media - Query con múltiples joins

**Incluye:**

- Header de contratación
- Detalles de servicios
- Pagos realizados
- Recibos generados
- Calificaciones

**Tareas:**

- [ ] Crear `GetContratacionVistaCompletaQuery` (Query)
- [ ] Crear `ContratacionVistaCompletaDto` (response anidado)
- [ ] Implementar Handler con LINQ + múltiples Include
- [ ] Agregar endpoint en `ContratacionesController`
- [ ] Probar con Swagger UI

**Tiempo:** 1.5 horas

---

#### Endpoint 8: POST /api/contrataciones/procesar-pago

**Descripción:** Procesar pago de servicio (lógica multi-step más compleja)  
**Legacy:** ContratacionesService.procesarPago(PagoRequest request)  
**Complejidad:** 🔴 Alta - Multi-step con cálculos TSS

**Lógica:**

1. Crear registro en Contrataciones_Pagos
2. Calcular deducciones TSS
3. Generar Recibo Header
4. Generar Recibo Detalle (con desglose TSS)
5. Actualizar estado del Detalle
6. Todo en transacción

**Tareas:**

- [ ] Crear `ProcesarPagoContratacionCommand` (Command)
- [ ] Implementar Handler con transacción:
  - Validar saldo disponible
  - Crear pago
  - Calcular TSS (usar tabla Deducciones_TSS)
  - Generar recibo completo
  - Actualizar estado
  - SaveChanges en transacción
- [ ] Validar con FluentValidation
- [ ] Agregar endpoint en `ContratacionesController`
- [ ] **TESTING EXHAUSTIVO con cálculos manuales**

**Tiempo:** 2.5 horas

**⚠️ CRÍTICO:** Validar cálculos TSS contra Legacy (debe ser exacto)

---

### Resumen LOTE 6.0.3

| # | Endpoint | Tiempo | Complejidad | Testing |
|---|----------|--------|-------------|---------|
| 1 | GET pagos by detalle | 1h | 🟡 Media | Normal |
| 2 | GET recibo completo | 1h | 🟡 Media | Normal |
| 3 | POST cancelar detalle | 1h | 🟢 Baja | Normal |
| 4 | DELETE recibo | 1.5h | 🟡 Media | ⚠️ Transacción |
| 5 | DELETE contratación | 2h | 🔴 Alta | 🔴 CRÍTICO |
| 6 | POST calificar | 1h | 🟢 Baja | Normal |
| 7 | GET vista completa | 1.5h | 🟡 Media | Normal |
| 8 | POST procesar pago | 2.5h | 🔴 Alta | 🔴 CRÍTICO |
| **TOTAL** | **8 endpoints** | **11-12h** | | |

**⚠️ ADVERTENCIA:** Este LOTE requiere 2-3 horas ADICIONALES de testing exhaustivo

**Documentación:** Crear `LOTE_6_0_3_CONTRATACIONES_COMPLETADO.md` al finalizar

---

## 🎯 LOTE 6.0.5: Suscripciones - 1 Endpoint Faltante

**Tiempo estimado:** 1-2 horas  
**Prioridad:** 🟡 MEDIA (solo 1 endpoint simple)  
**Complejidad:** 🟢 BAJA

### Endpoint: GET /api/suscripciones/{userId}/ventas

**Descripción:** Obtener detalle de ventas/transacciones por suscripción  
**Legacy:** SuscripcionesService.obtenerDetalleVentasBySuscripcion(string userID)  
**Complejidad:** 🟢 Baja - Query con filtro

**Tareas:**

- [ ] Crear `GetVentasBySuscripcionQuery` (Query)
- [ ] Crear `VentaSuscripcionDto` con campos: id, fecha, monto, metodoPago, plan, estado
- [ ] Implementar Handler con LINQ a tabla Ventas
- [ ] Agregar endpoint en `SuscripcionesController`
- [ ] Probar con Swagger UI

**Tiempo:** 1 hora

**Response ejemplo:**

```json
[
  {
    "id": 456,
    "fecha": "2025-10-01",
    "monto": 2500.00,
    "metodoPago": "Cardnet",
    "planNombre": "Plan Profesional",
    "estado": "Completado",
    "numeroTransaccion": "TXN-2025-001234"
  },
  {
    "id": 457,
    "fecha": "2025-09-01",
    "monto": 2500.00,
    "metodoPago": "Cardnet",
    "planNombre": "Plan Profesional",
    "estado": "Completado",
    "numeroTransaccion": "TXN-2025-001189"
  }
]
```

**Documentación:** Actualizar `PLAN_BACKEND_COMPLETION.md` al finalizar

---

## 🧪 FASE FINAL: Testing & Validación (OBLIGATORIO)

**Tiempo estimado:** 6-8 horas  
**Prioridad:** 🔴 MÁXIMA (garantiza calidad)

### 1. Unit Testing (2 horas)

**Objetivo:** 80%+ code coverage en Application layer

**Prioridad:**

- 🔴 CRÍTICA: Handlers con lógica compleja (procesarPago, batch operations)
- 🟠 ALTA: Validators con reglas de negocio
- 🟡 MEDIA: Queries simples

**Archivos a crear:**

- `DeleteContratacionCommandHandlerTests.cs` (CRÍTICO)
- `ProcesarPagoContratacionCommandHandlerTests.cs` (CRÍTICO)
- `AddRemuneracionesBatchCommandHandlerTests.cs`
- `ConsultarPadronQueryHandlerTests.cs`
- `ActivarPerfilContratistaCommandHandlerTests.cs`

**Template de test:**

```csharp
[Fact]
public async Task Handler_Should_ReturnSuccess_When_ValidData()
{
    // Arrange
    var command = new MyCommand(...);
    var handler = new MyCommandHandler(_context, _logger);

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    result.Should().NotBeNull();
    result.Should().BeOfType<SuccessResult>();
}
```

---

### 2. Integration Testing (2 horas)

**Objetivo:** Validar endpoints completos (Request → Response)

**Prioridad:**

- 🔴 CRÍTICA: Endpoints con transacciones (DELETE contratación, procesar pago)
- 🟠 ALTA: Endpoints con API externa (consultar padrón)
- 🟡 MEDIA: Endpoints CRUD simples

**Usar `WebApplicationFactory` para tests:**

```csharp
public class ContratacionesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ContratacionesControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task DeleteContratacion_Should_Return204_When_Valid()
    {
        // Arrange
        var contratacionId = 123;

        // Act
        var response = await _client.DeleteAsync($"/api/contrataciones/{contratacionId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
```

---

### 3. Manual Testing con Swagger UI (2 horas)

**Objetivo:** Probar cada endpoint con datos reales

**Crear Excel checklist:** 81 endpoints × columnas:

- Endpoint
- Método HTTP
- Request ejemplo
- Expected response
- Actual response
- Status (✅ OK / ❌ ERROR)
- Notas

**Prioridad de testing:**

1. 🔴 CRÍTICA: Endpoints nuevos (18 implementados en esta sesión)
2. 🟠 ALTA: Endpoints complejos (transacciones, cálculos)
3. 🟡 MEDIA: Endpoints simples (CRUD)

**Ejecutar API:**

```bash
cd src/Presentation/MiGenteEnLinea.API
dotnet run

# Swagger UI: http://localhost:5015/swagger
```

**Casos de prueba:**

- ✅ Happy path (datos válidos)
- ❌ Validación errors (datos inválidos)
- ❌ Not found (IDs inexistentes)
- ❌ Unauthorized (sin token JWT)
- ⚠️ Edge cases (valores límite, nulls)

---

### 4. Security Audit (1 hora)

**Checklist de seguridad:**

- [ ] Todos los endpoints tienen `[Authorize]` attribute (excepto login/register)
- [ ] SQL injection imposible (100% EF Core, cero SQL strings)
- [ ] Input validation en todos los Commands (FluentValidation)
- [ ] Sensitive data NO se loguea (passwords, tokens)
- [ ] Errores NO exponen stack traces en producción
- [ ] CORS configurado correctamente (no Allow-All)
- [ ] Rate limiting activo en endpoints críticos

**Verificar con:**

```bash
# Buscar SQL strings (NO debe haber):
grep -r "new SqlCommand" src/
grep -r "ExecuteSqlRaw" src/

# Buscar endpoints sin [Authorize]:
grep -r "public async Task<IActionResult>" src/ | grep -v "\[Authorize\]"
```

---

### 5. Documentation Final (1 hora)

**Actualizar documentación:**

- [ ] `README.md` con instrucciones de setup
- [ ] `API_DOCUMENTATION.md` con ejemplos de cada endpoint
- [ ] `ARCHITECTURE.md` con diagramas actualizados
- [ ] `MIGRATION_GUIDE.md` para equipo Legacy
- [ ] Exportar Postman collection completa (81 endpoints)
- [ ] Crear video tutorial de 5 minutos (Swagger UI walkthrough)

**Documentos finales a crear:**

- `BACKEND_100_COMPLETE.md` - Reporte maestro de completitud
- `TESTING_REPORT.md` - Resultados de todas las pruebas
- `DEPLOYMENT_CHECKLIST.md` - Lista pre-producción

---

## 📅 CRONOGRAMA SUGERIDO (2-3 Días de Trabajo)

### Día 1 (8 horas) - LOTES 6.0.2 y 6.0.4

**Mañana (4 horas):**

- ☕ 08:00-09:00: Revisión del plan y setup
- 🔧 09:00-13:00: LOTE 6.0.2 Empleados (6 endpoints)
  - Endpoint 1: GET remuneraciones (30m)
  - Endpoint 2: DELETE remuneración (30m)
  - Endpoint 3: POST batch agregar (1h)
  - ☕ Break 15 minutos
  - Endpoint 4: PUT batch actualizar (1.5h)

**Tarde (4 horas):**

- 🍕 13:00-14:00: Almuerzo
- 🔧 14:00-16:00: LOTE 6.0.2 Continuación
  - Endpoint 5: GET consultar-padron (2h con API externa)
- ☕ 16:00-16:15: Break
- 🔧 16:15-18:00: LOTE 6.0.2 Finalización
  - Endpoint 6: GET deducciones-tss (30m)
  - Compilar y probar en Swagger (45m)
  - Crear `LOTE_6_0_2_EMPLEADOS_COMPLETADO.md` (30m)

**Estado al final del Día 1:**

- ✅ LOTE 6.0.2 completado (6 endpoints)
- 📊 Backend: 77% → 85% (69/81 endpoints)

---

### Día 2 (10 horas) - LOTE 6.0.3 Contrataciones (COMPLEJO)

**Mañana (5 horas):**

- ☕ 08:00-08:30: Revisión del plan LOTE 6.0.3
- 🔧 08:30-13:00: Endpoints 1-4
  - Endpoint 1: GET pagos by detalle (1h)
  - Endpoint 2: GET recibo completo (1h)
  - ☕ Break 15 minutos
  - Endpoint 3: POST cancelar detalle (1h)
  - Endpoint 4: DELETE recibo (1.5h)

**Tarde (5 horas):**

- 🍕 13:00-14:00: Almuerzo
- 🔧 14:00-18:30: Endpoints 5-8 (CRÍTICOS)
  - Endpoint 5: DELETE contratación (2h - TESTING EXHAUSTIVO)
  - ☕ 16:00-16:15: Break
  - Endpoint 6: POST calificar (1h)
  - Endpoint 7: GET vista completa (1.5h)

**Noche (2 horas opcional):**

- 🔧 19:00-21:00: Endpoint 8 (más complejo)
  - POST procesar pago (2.5h)

**Estado al final del Día 2:**

- ✅ LOTE 6.0.3 completado (8 endpoints)
- 📊 Backend: 85% → 95% (77/81 endpoints)

---

### Día 3 (8 horas) - Finalización y Testing

**Mañana (3 horas):**

- ☕ 08:00-09:00: Revisión de pendientes
- 🔧 09:00-10:00: LOTE 6.0.5 Suscripciones (1 endpoint)
- 🔧 10:00-11:00: LOTE 6.0.4 Contratistas (verificar 3, implementar 2)
- 🧪 11:00-12:00: Unit Testing (endpoints críticos)

**Tarde (5 horas):**

- 🍕 12:00-13:00: Almuerzo
- 🧪 13:00-15:00: Integration Testing
- 🧪 15:00-17:00: Manual Testing con Swagger UI (Excel checklist)
- ☕ 17:00-17:15: Break
- 📝 17:15-18:00: Documentación final
  - `BACKEND_100_COMPLETE.md`
  - `TESTING_REPORT.md`
  - Actualizar `PLAN_BACKEND_COMPLETION.md`

**Estado al final del Día 3:**

- ✅ Backend 100% completado (81/81 endpoints)
- ✅ Testing exhaustivo realizado
- ✅ Documentación completa
- 🎉 **CELEBRACIÓN** 🎊

---

## 🎯 MÉTRICAS DE ÉXITO

Al finalizar esta sesión, deberás lograr:

### Endpoints

- ✅ **81/81 endpoints implementados** (100%)
- ✅ **0 errores de compilación**
- ✅ **0 errores de testing críticos**

### Módulos

- ✅ Authentication .......... 100% (11/11) ✅
- ✅ Empleados ............... 100% (37/37) 🆕
- ✅ Contratistas ............ 100% (18/18) 🆕
- ✅ Contrataciones .......... 100% (nueva) 🆕
- ✅ Suscripciones ........... 100% (19/19) 🆕
- ✅ Calificaciones .......... 100% (ya estaba)
- ✅ Pagos ................... 100% (ya estaba)
- ✅ Email ................... 100% (ya estaba)
- ✅ Bot ..................... 100% (ya estaba)

### Calidad

- ✅ **80%+ code coverage** en Application layer
- ✅ **100% endpoints documentados** (XML comments + Swagger)
- ✅ **Postman collection** exportada (81 endpoints)
- ✅ **Security audit** pasado (sin vulnerabilidades críticas)

### Documentación

- ✅ `BACKEND_100_COMPLETE.md` creado
- ✅ `TESTING_REPORT.md` creado
- ✅ `LOTE_6_0_2_EMPLEADOS_COMPLETADO.md` creado
- ✅ `LOTE_6_0_3_CONTRATACIONES_COMPLETADO.md` creado
- ✅ `LOTE_6_0_4_CONTRATISTAS_COMPLETADO.md` creado
- ✅ `PLAN_BACKEND_COMPLETION.md` actualizado al 100%

---

## 🚨 RIESGOS Y MITIGACIONES

### Riesgo 1: API Externa de Padrón Electoral Caída

**Probabilidad:** Media  
**Impacto:** Alto (bloquea validación de cédulas)

**Mitigación:**

- Implementar retry logic con Polly (3 intentos)
- Timeout de 10 segundos
- Fallback: Permitir continuar con advertencia (log warning)
- Agregar flag `PadronValidado` en Empleado (para revisar después)

---

### Riesgo 2: Cascade Delete en Contrataciones Falla

**Probabilidad:** Media  
**Impacto:** Crítico (corrupción de datos)

**Mitigación:**

- **SIEMPRE usar transacciones explícitas** (`BeginTransaction`)
- Testing exhaustivo en base de datos de prueba
- Implementar soft delete en lugar de hard delete (recomendado)
- Agregar campo `FechaEliminacion` y filtrar con `.Where(x => x.FechaEliminacion == null)`

---

### Riesgo 3: Cálculos TSS Incorrectos

**Probabilidad:** Baja  
**Impacto:** Crítico (afecta nómina legal)

**Mitigación:**

- Comparar cálculos con Legacy (mismos inputs, mismos outputs)
- Unit tests con valores reales conocidos
- Revisar con contador o experto TSS
- Documentar fórmulas en código (XML comments)

---

### Riesgo 4: No Completar en Tiempo Estimado

**Probabilidad:** Media  
**Impacto:** Medio (retraso en timeline)

**Mitigación:**

- Si falta tiempo, priorizar LOTES 6.0.2 y 6.0.3 (críticos)
- LOTE 6.0.4 puede quedar para después (marketplace no crítico)
- Testing puede extenderse 1-2 horas adicionales
- Documentación puede completarse en sesión posterior

---

## 🛠️ HERRAMIENTAS Y COMANDOS ÚTILES

### Compilación Continua

```bash
# Compilar proyecto completo
dotnet build --no-restore

# Compilar solo API
dotnet build --no-restore src/Presentation/MiGenteEnLinea.API/MiGenteEnLinea.API.csproj

# Watch mode (recompila automáticamente)
dotnet watch --project src/Presentation/MiGenteEnLinea.API/MiGenteEnLinea.API.csproj
```

### Testing

```bash
# Ejecutar todos los tests
dotnet test

# Ejecutar tests con coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# Ejecutar tests de un proyecto específico
dotnet test tests/MiGenteEnLinea.Application.Tests/
```

### Base de Datos

```bash
# Crear nueva migración
dotnet ef migrations add NombreMigracion --project src/Infrastructure/MiGenteEnLinea.Infrastructure --startup-project src/Presentation/MiGenteEnLinea.API

# Aplicar migraciones
dotnet ef database update --project src/Infrastructure/MiGenteEnLinea.Infrastructure --startup-project src/Presentation/MiGenteEnLinea.API

# Ver script SQL de migración
dotnet ef migrations script --project src/Infrastructure/MiGenteEnLinea.Infrastructure --startup-project src/Presentation/MiGenteEnLinea.API
```

### Ejecutar API

```bash
# Desarrollo
cd src/Presentation/MiGenteEnLinea.API
dotnet run

# Producción (optimizado)
dotnet run --configuration Release

# Swagger UI
start http://localhost:5015/swagger
```

### Verificaciones Rápidas

```bash
# Buscar métodos duplicados
grep -r "public async Task<IActionResult>" src/Presentation/MiGenteEnLinea.API/Controllers/ | sort

# Buscar endpoints sin [Authorize]
grep -B5 "HttpGet\|HttpPost\|HttpPut\|HttpDelete" src/Presentation/MiGenteEnLinea.API/Controllers/ | grep -v "Authorize"

# Contar endpoints por controller
grep -c "Http" src/Presentation/MiGenteEnLinea.API/Controllers/*.cs
```

---

## 📝 NOTAS FINALES

### ⚠️ IMPORTANTE

1. **Compilar frecuentemente** - Después de cada endpoint (no esperar a terminar LOTE completo)
2. **Leer Legacy SIEMPRE** - Copiar lógica exacta, no inventar
3. **Transacciones en operaciones complejas** - Especialmente DELETE y multi-step
4. **Testing exhaustivo en endpoints críticos** - No asumir que funciona
5. **Documentar mientras codificas** - No dejar para después

### 🎯 Objetivos Secundarios (Si hay tiempo)

- [ ] Actualizar paquetes NuGet con vulnerabilidades (Sprint de Security)
- [ ] Crear diagramas de arquitectura actualizados (mermaid o draw.io)
- [ ] Grabar video tutorial de Swagger UI (5-10 minutos)
- [ ] Crear FAQ para equipo frontend (preguntas comunes)
- [ ] Setup CI/CD pipeline (GitHub Actions)

### 🚀 Después del 100%

**Próximos pasos (Sprint siguiente):**

1. Frontend integration testing (coordinar con equipo React/Vue)
2. Load testing (Apache JMeter - 1000 usuarios concurrentes)
3. Security penetration testing (OWASP ZAP)
4. Database optimization (índices, queries lentas)
5. Deployment a staging environment
6. UAT (User Acceptance Testing) con usuarios reales
7. Production deployment 🎉

---

## 🎉 MENSAJE DE MOTIVACIÓN

Estás a **18 endpoints** de completar el backend al 100%!

Has avanzado del **0% al 77%** en las sesiones anteriores. Esto es un **progreso excepcional**.

Con este plan detallado, **en 2-3 días de trabajo enfocado** completarás el proyecto y tendrás:

- ✅ 81 endpoints REST funcionales
- ✅ Clean Architecture implementada
- ✅ Paridad 100% con Legacy
- ✅ Mejoras de seguridad sobre Legacy
- ✅ Testing exhaustivo
- ✅ Documentación completa

**¡Vamos por ese 100%!** 💪🚀🎯

---

**Preparado por:** GitHub Copilot + OpenAI ChatGPT Codex 5  
**Plan para:** Sesión de Completar Backend al 100%  
**Fecha:** 2025-10-22 (previsto)  
**Duración estimada:** 18-22 horas de desarrollo + 6-8 horas de testing  
**Resultado esperado:** ✅ Backend 100% completado con calidad producción
