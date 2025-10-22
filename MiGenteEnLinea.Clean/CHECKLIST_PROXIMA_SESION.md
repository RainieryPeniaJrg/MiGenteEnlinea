# ✅ CHECKLIST PRÓXIMA SESIÓN: Backend 100%

**Fecha inicio:** _________  
**Fecha fin:** _________  
**Total horas:** _________

---

## 📋 LOTE 6.0.2: Empleados - Remuneraciones & TSS

**Tiempo estimado:** 4-5 horas  
**Inicio:** ___:___ | **Fin:** ___:___

### Endpoint 1: GET /api/empleados/{empleadoId}/remuneraciones

- [ ] Crear `GetRemuneracionesByEmpleadoQuery`
- [ ] Crear `RemuneracionDto`
- [ ] Implementar Handler
- [ ] Agregar a `EmpleadosController`
- [ ] `dotnet build` → ✅ Compilado
- [ ] Probar en Swagger UI → ✅ Funciona
- [ ] **Tiempo real:** _____ min

---

### Endpoint 2: DELETE /api/remuneraciones/{remuneracionId}

- [ ] Crear `DeleteRemuneracionCommand`
- [ ] Implementar Handler (soft delete)
- [ ] Validar remuneración existe
- [ ] Agregar a `EmpleadosController`
- [ ] `dotnet build` → ✅ Compilado
- [ ] Probar en Swagger UI → ✅ Funciona
- [ ] **Tiempo real:** _____ min

---

### Endpoint 3: POST /api/empleados/{empleadoId}/remuneraciones/batch

- [ ] Crear `AddRemuneracionesBatchCommand`
- [ ] Crear `RemuneracionInputDto`
- [ ] Implementar Handler con AddRange
- [ ] Crear Validator (FluentValidation)
- [ ] Agregar a `EmpleadosController`
- [ ] `dotnet build` → ✅ Compilado
- [ ] Probar en Swagger UI (3-5 remuneraciones) → ✅ Funciona
- [ ] **Tiempo real:** _____ min

---

### Endpoint 4: PUT /api/empleados/{empleadoId}/remuneraciones/batch

- [ ] Crear `UpdateRemuneracionesBatchCommand`
- [ ] Implementar Handler con transacción:
  - [ ] Soft delete existentes (Activo = false)
  - [ ] AddRange nuevas
  - [ ] SaveChanges en transacción
- [ ] Crear Validator
- [ ] Agregar a `EmpleadosController`
- [ ] `dotnet build` → ✅ Compilado
- [ ] Probar en Swagger UI → ✅ Funciona
- [ ] ⚠️ **Verificar transacción funciona** (rollback si falla)
- [ ] **Tiempo real:** _____ min

---

### Endpoint 5: GET /api/empleados/consultar-padron/{cedula}

- [ ] Crear `ConsultarPadronQuery`
- [ ] Crear `PadronElectoralDto`
- [ ] Implementar `IPadronElectoralService` en Infrastructure
- [ ] Configurar HttpClient con Polly (retry 3x, timeout 10s)
- [ ] Agregar config en appsettings.json:
  ```json
  "PadronElectoral": {
    "ApiUrl": "https://abcportal.online/Sigeinfo/public/api",
    "ApiKey": "tu-api-key-aqui",
    "TimeoutSeconds": 10
  }
  ```
- [ ] Implementar Handler que llame al servicio
- [ ] Agregar a `EmpleadosController`
- [ ] `dotnet build` → ✅ Compilado
- [ ] ⚠️ **Probar con cédula real** → ✅ API responde
- [ ] ⚠️ **Probar retry logic** (desconectar red)
- [ ] **Tiempo real:** _____ min

---

### Endpoint 6: GET /api/catalogos/deducciones-tss

- [ ] Crear `GetDeduccionesTssQuery`
- [ ] Crear `DeduccionTssDto`
- [ ] Implementar Handler
- [ ] Crear `CatalogosController` (si no existe)
- [ ] Agregar endpoint
- [ ] `dotnet build` → ✅ Compilado
- [ ] Probar en Swagger UI → ✅ Funciona
- [ ] **Tiempo real:** _____ min

---

### 📝 Documentación LOTE 6.0.2

- [ ] Crear `LOTE_6_0_2_EMPLEADOS_COMPLETADO.md`
- [ ] Actualizar `PLAN_BACKEND_COMPLETION.md` (77% → 85%)
- [ ] Commit Git: "feat: LOTE 6.0.2 Empleados completado (6 endpoints)"

---

## 📋 LOTE 6.0.3: Contrataciones Temporales

**Tiempo estimado:** 10-12 horas (COMPLEJO)  
**Inicio:** ___:___ | **Fin:** ___:___

⚠️ **ADVERTENCIA:** Transacciones complejas, testing exhaustivo obligatorio

### Endpoint 1: GET /api/contrataciones/{contratacionId}/detalle/{detalleId}/pagos

- [ ] Crear `GetPagosByDetalleQuery`
- [ ] Crear `PagoContrataciónDto`
- [ ] Implementar Handler (LINQ + Include)
- [ ] Agregar a `ContratacionesController`
- [ ] `dotnet build` → ✅ Compilado
- [ ] Probar en Swagger UI → ✅ Funciona
- [ ] **Tiempo real:** _____ min

---

### Endpoint 2: GET /api/contrataciones/recibos/{pagoId}

- [ ] Crear `GetReciboByPagoIdQuery`
- [ ] Crear `ReciboCompletoDto` (header + detalles)
- [ ] Implementar Handler (LINQ + Include)
- [ ] Agregar a `ContratacionesController`
- [ ] `dotnet build` → ✅ Compilado
- [ ] Probar en Swagger UI → ✅ Funciona
- [ ] **Tiempo real:** _____ min

---

### Endpoint 3: POST /api/contrataciones/{contratacionId}/detalle/{detalleId}/cancelar

- [ ] Crear `CancelarDetalleContratacionCommand`
- [ ] Implementar Handler:
  - [ ] Validar no tiene pagos procesados
  - [ ] Actualizar Estado = "Cancelado"
  - [ ] Registrar motivo y fecha
- [ ] Crear Validator
- [ ] Agregar a `ContratacionesController`
- [ ] `dotnet build` → ✅ Compilado
- [ ] Probar en Swagger UI → ✅ Funciona
- [ ] **Tiempo real:** _____ min

---

### Endpoint 4: DELETE /api/contrataciones/recibos/{reciboId}

- [ ] Crear `DeleteReciboCommand`
- [ ] Implementar Handler con transacción:
  - [ ] Verificar estado (no "Cerrado")
  - [ ] Remove detalles
  - [ ] Remove header
  - [ ] SaveChanges en transacción
- [ ] Crear Validator
- [ ] Agregar a `ContratacionesController`
- [ ] `dotnet build` → ✅ Compilado
- [ ] ⚠️ **Testing en DB prueba** → ✅ Rollback funciona
- [ ] Probar en Swagger UI → ✅ Funciona
- [ ] **Tiempo real:** _____ min

---

### Endpoint 5: DELETE /api/contrataciones/{contratacionId} 🔴 CRÍTICO

- [ ] Crear `DeleteContratacionCommand`
- [ ] Implementar Handler con transacción:
  - [ ] Verificar NO tiene pagos procesados
  - [ ] Eliminar en orden:
    1. [ ] Recibos_Detalle
    2. [ ] Recibos_Header
    3. [ ] Pagos
    4. [ ] Detalle
    5. [ ] Contratación
  - [ ] SaveChanges en transacción con rollback
- [ ] Crear Validator
- [ ] Agregar a `ContratacionesController`
- [ ] `dotnet build` → ✅ Compilado
- [ ] 🔴 **TESTING EXHAUSTIVO en DB prueba**:
  - [ ] Probar con contratación SIN pagos → ✅ Elimina OK
  - [ ] Probar con contratación CON pagos → ❌ Rechaza (validación)
  - [ ] Probar rollback (simular error) → ✅ No deja inconsistencias
- [ ] Probar en Swagger UI → ✅ Funciona
- [ ] **Tiempo real:** _____ min

---

### Endpoint 6: POST /api/contrataciones/{contratacionId}/calificar

- [ ] Crear `CalificarContratacionCommand`
- [ ] Implementar Handler:
  - [ ] Verificar contratación completada
  - [ ] Insert en Calificaciones
  - [ ] Update flag Calificada = true
  - [ ] Actualizar promedio en Contratista
- [ ] Crear Validator (puntuación 1-5)
- [ ] Agregar a `ContratacionesController`
- [ ] `dotnet build` → ✅ Compilado
- [ ] Probar en Swagger UI → ✅ Funciona
- [ ] **Tiempo real:** _____ min

---

### Endpoint 7: GET /api/contrataciones/{contratacionId}/vista

- [ ] Crear `GetContratacionVistaCompletaQuery`
- [ ] Crear `ContratacionVistaCompletaDto` (anidado)
- [ ] Implementar Handler (múltiples Include)
- [ ] Agregar a `ContratacionesController`
- [ ] `dotnet build` → ✅ Compilado
- [ ] Probar en Swagger UI → ✅ Funciona
- [ ] **Tiempo real:** _____ min

---

### Endpoint 8: POST /api/contrataciones/procesar-pago 🔴 CRÍTICO

- [ ] Crear `ProcesarPagoContratacionCommand`
- [ ] Implementar Handler con transacción:
  - [ ] Validar saldo disponible
  - [ ] Crear registro Contrataciones_Pagos
  - [ ] Calcular deducciones TSS (usar tabla Deducciones_TSS)
  - [ ] Generar Recibo Header
  - [ ] Generar Recibo Detalle (con desglose TSS)
  - [ ] Actualizar estado Detalle
  - [ ] SaveChanges en transacción
- [ ] Crear Validator
- [ ] Agregar a `ContratacionesController`
- [ ] `dotnet build` → ✅ Compilado
- [ ] 🔴 **TESTING EXHAUSTIVO**:
  - [ ] Comparar cálculos TSS con Legacy (mismos valores)
  - [ ] Probar rollback si falla generación recibo
  - [ ] Verificar integridad de datos
- [ ] Probar en Swagger UI → ✅ Funciona
- [ ] **Tiempo real:** _____ min

---

### 📝 Documentación LOTE 6.0.3

- [ ] Crear `LOTE_6_0_3_CONTRATACIONES_COMPLETADO.md`
- [ ] Actualizar `PLAN_BACKEND_COMPLETION.md` (85% → 95%)
- [ ] Commit Git: "feat: LOTE 6.0.3 Contrataciones completado (8 endpoints CRÍTICOS)"

---

## 📋 LOTE 6.0.4: Contratistas - Servicios & Activación

**Tiempo estimado:** 2-3 horas  
**Inicio:** ___:___ | **Fin:** ___:___

⚠️ **NOTA:** 3 endpoints YA EXISTEN, solo verificar

### Verificación de Endpoints Existentes

#### Endpoint 1: GET /api/contratistas/{contratistaId}/servicios

- [ ] Verificar existe en `ContratistasController` línea 151
- [ ] `dotnet build` → ✅ Compila sin errores
- [ ] Probar en Swagger UI → ✅ Funciona correctamente
- [ ] **Estado:** ✅ OK / ⚠️ Necesita ajustes

---

#### Endpoint 2: POST /api/contratistas/{contratistaId}/servicios

- [ ] Verificar existe en `ContratistasController` línea ~350
- [ ] `dotnet build` → ✅ Compila sin errores
- [ ] Probar en Swagger UI → ✅ Funciona correctamente
- [ ] **Estado:** ✅ OK / ⚠️ Necesita ajustes

---

#### Endpoint 3: DELETE /api/contratistas/{contratistaId}/servicios/{servicioId}

- [ ] Verificar existe en `ContratistasController` línea ~380
- [ ] `dotnet build` → ✅ Compila sin errores
- [ ] Probar en Swagger UI → ✅ Funciona correctamente
- [ ] **Estado:** ✅ OK / ⚠️ Necesita ajustes

---

### Endpoints Nuevos a Implementar

#### Endpoint 4: POST /api/contratistas/{contratistaId}/activar

- [ ] Crear `ActivarPerfilContratistaCommand`
- [ ] Implementar Handler:
  - [ ] Buscar contratista
  - [ ] Actualizar Activo = true
  - [ ] Actualizar FechaActivacion = DateTime.UtcNow
  - [ ] SaveChanges
- [ ] Agregar a `ContratistasController`
- [ ] `dotnet build` → ✅ Compilado
- [ ] Probar en Swagger UI → ✅ Funciona
- [ ] **Tiempo real:** _____ min

---

#### Endpoint 5: POST /api/contratistas/{contratistaId}/desactivar

- [ ] Crear `DesactivarPerfilContratistaCommand`
- [ ] Implementar Handler:
  - [ ] Buscar contratista
  - [ ] Actualizar Activo = false
  - [ ] Registrar motivo (opcional)
  - [ ] SaveChanges
- [ ] Agregar a `ContratistasController`
- [ ] `dotnet build` → ✅ Compilado
- [ ] Probar en Swagger UI → ✅ Funciona
- [ ] **Tiempo real:** _____ min

---

### 📝 Documentación LOTE 6.0.4

- [ ] Crear `LOTE_6_0_4_CONTRATISTAS_COMPLETADO.md`
- [ ] Actualizar `PLAN_BACKEND_COMPLETION.md` (95% → 97%)
- [ ] Commit Git: "feat: LOTE 6.0.4 Contratistas completado (2 endpoints nuevos)"

---

## 📋 LOTE 6.0.5: Suscripciones - 1 Endpoint

**Tiempo estimado:** 1 hora  
**Inicio:** ___:___ | **Fin:** ___:___

### Endpoint: GET /api/suscripciones/{userId}/ventas

- [ ] Crear `GetVentasBySuscripcionQuery`
- [ ] Crear `VentaSuscripcionDto`
- [ ] Implementar Handler (LINQ a tabla Ventas)
- [ ] Agregar a `SuscripcionesController`
- [ ] `dotnet build` → ✅ Compilado
- [ ] Probar en Swagger UI → ✅ Funciona
- [ ] **Tiempo real:** _____ min

---

### 📝 Documentación LOTE 6.0.5

- [ ] Actualizar `PLAN_BACKEND_COMPLETION.md` (97% → 100%) 🎉
- [ ] Commit Git: "feat: LOTE 6.0.5 Suscripciones completado - BACKEND 100%!"

---

## 🧪 TESTING & VALIDACIÓN FINAL

**Tiempo estimado:** 6-8 horas  
**Inicio:** ___:___ | **Fin:** ___:___

### 1. Unit Testing (2 horas)

- [ ] `DeleteContratacionCommandHandlerTests.cs` (CRÍTICO)
- [ ] `ProcesarPagoContratacionCommandHandlerTests.cs` (CRÍTICO)
- [ ] `AddRemuneracionesBatchCommandHandlerTests.cs`
- [ ] `ConsultarPadronQueryHandlerTests.cs`
- [ ] `ActivarPerfilContratistaCommandHandlerTests.cs`
- [ ] Ejecutar: `dotnet test`
- [ ] **Code coverage:** _____% (objetivo: 80%+)

---

### 2. Integration Testing (2 horas)

- [ ] `ContratacionesControllerTests.cs` (endpoints críticos)
- [ ] `EmpleadosControllerTests.cs` (batch operations)
- [ ] Ejecutar: `dotnet test`
- [ ] **Tests passed:** _____ / _____

---

### 3. Manual Testing - Swagger UI (2 horas)

- [ ] Crear Excel checklist: 81 endpoints
- [ ] Ejecutar API: `dotnet run`
- [ ] Abrir Swagger: http://localhost:5015/swagger
- [ ] Probar TODOS los endpoints:
  - [ ] 18 endpoints nuevos (prioridad ALTA)
  - [ ] 63 endpoints existentes (smoke test)
- [ ] Comparar responses con Legacy (screenshot)
- [ ] Validar tiempos de respuesta (<500ms p95)
- [ ] **Endpoints OK:** _____ / 81

---

### 4. Security Audit (1 hora)

- [ ] Todos los endpoints tienen `[Authorize]` (excepto login/register)
- [ ] Buscar SQL injection: `grep -r "new SqlCommand" src/` → 0 resultados
- [ ] Buscar endpoints sin auth: `grep -r "HttpGet\|HttpPost" src/ | grep -v "Authorize"` → revisar
- [ ] Input validation en todos los Commands (FluentValidation)
- [ ] Sensitive data NO en logs
- [ ] Errores NO exponen stack traces
- [ ] CORS configurado (no Allow-All)
- [ ] **Vulnerabilidades encontradas:** _____

---

### 5. Documentación Final (1 hora)

- [ ] Crear `BACKEND_100_COMPLETE.md` ⭐ REPORTE MAESTRO
- [ ] Crear `TESTING_REPORT.md`
- [ ] Actualizar `README.md`
- [ ] Actualizar `API_DOCUMENTATION.md`
- [ ] Exportar Postman collection (81 endpoints)
- [ ] Grabar video tutorial Swagger UI (5-10 min)
- [ ] Commit Git: "docs: Backend 100% completo - Documentación final"

---

## 🎯 VERIFICACIÓN FINAL - Backend 100%

### Métricas de Éxito

- [ ] **81/81 endpoints implementados** (100%)
- [ ] **0 errores de compilación**
- [ ] **0 errores de testing críticos**
- [ ] **80%+ code coverage**
- [ ] **Security audit pasado**
- [ ] **Documentación completa**
- [ ] **Postman collection exportada**

---

### Módulos 100%

- [ ] Authentication .......... 11/11 ✅
- [ ] Empleados ............... 37/37 ✅
- [ ] Contratistas ............ 18/18 ✅
- [ ] Contrataciones .......... 8/8 ✅
- [ ] Suscripciones ........... 19/19 ✅
- [ ] Calificaciones .......... 100% ✅
- [ ] Pagos ................... 100% ✅
- [ ] Email ................... 100% ✅
- [ ] Bot ..................... 100% ✅

---

## 🎉 CELEBRACIÓN

- [ ] **Backend 100% completado** 🎊
- [ ] **Paridad completa con Legacy** ✅
- [ ] **Clean Architecture implementada** ✅
- [ ] **Mejoras de seguridad sobre Legacy** ✅
- [ ] **Testing exhaustivo realizado** ✅

**Progreso:** 77/100 → 100/100 ⬛⬛⬛⬛⬛⬛⬛⬛⬛⬛

---

## 📝 NOTAS Y LECCIONES APRENDIDAS

**Tiempo real total:** _____ horas (estimado: 18-22h)

**Bloqueos encontrados:**
1. _____________________________________________
2. _____________________________________________
3. _____________________________________________

**Soluciones implementadas:**
1. _____________________________________________
2. _____________________________________________
3. _____________________________________________

**Mejoras sobre estimación:**
- _____________________________________________
- _____________________________________________

**Próximos pasos (después del 100%):**
- [ ] Frontend integration testing
- [ ] Load testing (1000 usuarios concurrentes)
- [ ] Security penetration testing
- [ ] Database optimization
- [ ] Deployment a staging
- [ ] UAT con usuarios reales
- [ ] Production deployment 🚀

---

**Preparado por:** GitHub Copilot + OpenAI ChatGPT Codex 5  
**Fecha creación:** 2025-10-21  
**Para sesión:** 2025-10-22 (próxima)
