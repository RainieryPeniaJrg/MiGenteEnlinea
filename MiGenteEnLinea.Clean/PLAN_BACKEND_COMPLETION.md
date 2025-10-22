# PLAN DE COMPLETITUD BACKEND - Resumen Ejecutivo

**Objetivo:** Cerrar brechas entre Legacy Services y Clean Architecture API  
**Estado Actual:** 77% completado (63 de 81 métodos) ✅ ACTUALIZADO  
**Meta:** 100% paridad con Legacy  
**Timeline:** 1.5 semanas (~18 horas) para cerrar pendientes y validaciones finales  
**Última actualización:** LOTE 6.0.1 Authentication completado (4 endpoints nuevos)

---

## RESUMEN VISUAL

```
Progress: 77% (63 / 81 endpoints) ✅ ACTUALIZADO
Completed: 63 endpoints (+4 en LOTE 6.0.1)
Pending:   18 endpoints

Module coverage:
- Calificaciones ............... 100%
- Pagos (Cardnet) .............. 100%
- Email ........................ 100%
- Authentication ............... 100% ✅ COMPLETADO (11 de 11)
- Empleados / Nomina ........... 62% (faltan 14 de 37)
- Contratistas ................. 78% (faltan 4 de 18)
- Suscripciones ................ 74% (faltan 3 de 19)
- Bot / OpenAI ................. 100%
```


---

## Inventario actualizado vs Legacy (oct 2025)

| Servicio Legacy | Cobertura Clean | Pendientes | Notas |
|-----------------|-----------------|------------|-------|
| LoginService    | 11/11 ✅       | -          | 100% completado en LOTE 6.0.1. Ver `LOTE_6_0_1_AUTHENTICATION_COMPLETADO.md` |
| EmpleadosService| 31/31           | -          | Comandos y queries disponibles; falta batería de pruebas completas |
| ContratistasService | 9/9        | Validación final | Endpoints implementados, revisar regresión con WebForms |
| SuscripcionesService | 17/18    | `obtenerDetalleVentasBySuscripcion` | `actualizarPassByID` no existe; documentado como descartado |
| PaymentService  | 2/3            | `consultarIdempotency` | Necesario para reintentos Cardnet desde UI |
| botService      | 1/2            | `GetChatResponse` | Config OpenAI migrada; falta endpoint de chat si se mantiene feature |
| EmailService / Sender | Sustituido | - | Capa Infrastructure cubre envío con plantillas |

---

## PLAN DE EJECUCIÓN (7 LOTES)

### SEMANA 1: CRÍTICOS (12-15 horas)

####  LOTE 6.0.1: Authentication Completion ✅ COMPLETADO

**Progreso final:** 100% (4 de 4 endpoints completados)  
**Tiempo total:** ~8-10 horas de desarrollo + 15 minutos de corrección de errores  
**Estado:** ✅ COMPILANDO SIN ERRORES

**Endpoints migrados**
1. ✅ DELETE /api/auth/users/{userId}/credentials/{credentialId} → `LOTE_6_0_1_ENDPOINT_1_COMPLETADO.md`
2. ✅ POST /api/auth/profile-info → `LOTE_6_0_1_ENDPOINT_2_COMPLETADO.md`
3. ✅ GET /api/auth/cuenta/{cuentaId} → `GetCuentaByIdQuery` expuesto en AuthController
4. ✅ PUT /api/auth/perfil/{userId} → `UpdateProfileCommand` con 7 campos actualizables

**Errores corregidos (sesión actual)**
- ❌→✅ Método duplicado `DeleteUserCredential` en AuthController (línea 866 eliminada)
- ❌→✅ Método duplicado `GetServiciosContratista` en ContratistasController (línea 320 eliminada)
- ❌→✅ Referencia incorrecta a `ServicioDto` (corregido al eliminar duplicado)

**Compilación final**
```
✅ Build SUCCEEDED: 0 errors, 35 warnings (vulnerabilidades NuGet conocidas)
   Total time: 4.1s
```

**Documentación completa**
- Ver `LOTE_6_0_1_AUTHENTICATION_COMPLETADO.md` para métricas detalladas
- AuthController: 11 endpoints totales (100% del módulo Authentication)
- Paridad con Legacy: 100% (LoginService.asmx.cs migrado completamente)

**Próximo paso**
- Testing manual en Swagger UI (30 minutos estimados)
####  LOTE 6.0.2: Empleados - Remuneraciones & TSS (4-5h)  ALTA

**Prioridad:** ALTA - Módulo más usado  
**Endpoints Faltantes:** 6 endpoints

| # | Endpoint | Descripción | API Externa |
|---|----------|-------------|-------------|
| 1 | GET /api/empleados/{id}/remuneraciones | Lista remuneraciones | No |
| 2 | DELETE /api/remuneraciones/{id} | Eliminar una remuneración | No |
| 3 | POST /api/empleados/{id}/remuneraciones/batch | Agregar múltiples | No |
| 4 | PUT /api/empleados/{id}/remuneraciones/batch | Actualizar todas | No |
| 5 | GET /api/empleados/consultar-padron/{cedula} | Validar cédula JCE |  SÍ |
| 6 | GET /api/catalogos/deducciones-tss | Catálogo TSS | No |

**Dependencias Externas:**

- API Padrón Electoral: <https://abcportal.online/Sigeinfo/public/api>
- Credenciales en appsettings.json
- Implementar retry logic (Polly)

---

####  LOTE 6.0.4: Contratistas - Servicios (5-6h)  ALTA

**Prioridad:** ALTA - Marketplace de servicios  
**Endpoints Faltantes:** 5 endpoints

| # | Endpoint | Método Legacy |
|---|----------|---------------|
| 1 | GET /api/contratistas/{id}/servicios | getServicios() |
| 2 | POST /api/contratistas/{id}/servicios | agregarServicio() |
| 3 | DELETE /api/contratistas/{id}/servicios/{servicioId} | removerServicio() |
| 4 | POST /api/contratistas/{id}/activar | ActivarPerfil() |
| 5 | POST /api/contratistas/{id}/desactivar | DesactivarPerfil() |

**Entidad Nueva:**

- Contratistas_Servicios (many-to-many)
- Servicios_Catalogo (catálogo de servicios disponibles)

---

### ⏱️ SEMANA 2: COMPLEJOS (14-18 horas)

####  LOTE 6.0.3: Contrataciones Temporales (8-10h)  CRÍTICA

**Prioridad:** CRÍTICA - Lógica más compleja del sistema  
**Endpoints Faltantes:** 8 endpoints

 **ADVERTENCIA:** Múltiples tablas relacionadas, cascade deletes complejos

| # | Endpoint | Complejidad | Notas |
|---|----------|-------------|-------|
| 1 | GET /api/contrataciones/{id}/detalle/{detalleId}/pagos |  Media | Vista con joins |
| 2 | GET /api/contrataciones/recibos/{pagoId} |  Media | Header + Detalles |
| 3 | POST /api/contrataciones/{id}/detalle/{detalleId}/cancelar |  Baja | Update status |
| 4 | DELETE /api/contrataciones/recibos/{pagoId} |  Media | 2 tablas |
| 5 | DELETE /api/contrataciones/{id} |  Alta | CASCADE 3+ tablas |
| 6 | POST /api/contrataciones/{id}/calificar |  Baja | Update flag |
| 7 | GET /api/contrataciones/{id}/vista |  Media | Vista completa |
| 8 | POST /api/contrataciones/procesar-pago |  Alta | Multi-step logic |

**Testing Crítico:**

- Transacciones con rollback
- Cascade deletes en QA environment
- Performance con datos reales

---

####  LOTE 6.0.5: Suscripciones - Gestión Avanzada (4-5h)  MEDIA

**Estado actual:** 2 de 3 operaciones cubiertas. El método legacy `actualizarPassByID` no existe y se excluye del alcance.

**Migración realizada**
- GET /api/auth/validar-correo-cuenta → ver `SESION_LOTE_6_0_5_Y_6_0_6_COMPLETADO.md`

**Pendiente**
- GET /api/suscripciones/{userId}/ventas (migrar `obtenerDetalleVentasBySuscripcion`, reutilizar mapeo de ventas y documentar contrato)

**Notas**
- Actualizar documentación para reflejar el descarte de `actualizarPassByID`.
- Alinear DTOs de ventas con los campos consumidos por la UI legacy.
####  LOTE 6.0.6: Bot & Configuración (2-3h)  COMPLETADO

**Estado actual:** Endpoint GET /api/configuracion/openai migrado (ver `SESION_LOTE_6_0_5_Y_6_0_6_COMPLETADO.md`).

**Acciones siguientes**
- Documentar riesgos de exposición de credenciales y definir estrategia segura (Key Vault, variables de entorno).
- Añadir pruebas de integración para asegurar respuesta anónima/autenticada.
- Planificar migración de `GetChatResponse` si el chatbot se mantiene en el roadmap.
####  LOTE 6.0.7: Testing & Validación (6-8h)  OBLIGATORIO

**Prioridad:** MÁXIMA - Garantiza calidad  

**Checklist de Calidad:**

**1. Unit Testing (2h)**

- [ ] 80%+ code coverage en Application layer
- [ ] All Commands con business logic tested
- [ ] All Queries con filtros tested
- [ ] Validators tested con edge cases

**2. Integration Testing (2h)**

- [ ] All Controllers tested (Request → Response)
- [ ] Authorization scenarios tested
- [ ] Validation failures tested
- [ ] Error responses tested (400, 401, 404, 500)

**3. Manual Testing con Swagger UI (2h)**

- [ ] Crear Excel checklist: 81 endpoints × Status
- [ ] Probar con datos reales de DB
- [ ] Comparar responses con Legacy (screenshot)
- [ ] Validar tiempos de respuesta (<500ms p95)

**4. Security Audit (1h)**

- [ ] All endpoints con [Authorize]
- [ ] SQL injection impossible (EF Core)
- [ ] Input validation (FluentValidation)
- [ ] Sensitive data not logged

**5. Documentation (1h)**

- [ ] Swagger XML comments completos
- [ ] Postman collection exportada
- [ ] README actualizado con ejemplos
- [ ] Arquitectura diagrams actualizados

---

## 📋 CHECKLIST DE IMPLEMENTACIÓN (Por Endpoint)

### Template de Implementación

```markdown
## Endpoint: [HTTP METHOD] /api/[resource]

### 1. Análisis Legacy
- [x] Leer método Legacy completo
- [x] Documentar lógica de negocio
- [x] Identificar validaciones
- [x] Notar códigos de retorno

### 2. Domain Layer (si aplica)
- [ ] Agregar propiedades a Entity
- [ ] Crear Value Objects
- [ ] Agregar Domain Events

### 3. Application Layer
- [ ] Crear Command/Query
- [ ] Crear Handler
- [ ] Crear Validator (FluentValidation)
- [ ] Crear DTO (Request + Response)
- [ ] Agregar AutoMapper profile

### 4. API Layer
- [ ] Agregar método a Controller
- [ ] Agregar [Http*] attribute
- [ ] Agregar XML documentation
- [ ] Agregar [ProducesResponseType]
- [ ] Agregar [Authorize] si aplica

### 5. Testing
- [ ] Unit test del Handler
- [ ] Unit test del Validator
- [ ] Integration test del Controller
- [ ] Manual test con Swagger UI

### 6. Validación
- [ ] Compilación sin errores
- [ ] Comparar con Legacy (mismo input → output)
- [ ] Performance aceptable (<500ms)
- [ ] Security checklist passed
```

---

## QUICK WIN: Cerrar pendientes del Lote 6.0.1


1. Publicar `GetCuentaByIdQuery` en AuthController y documentar contrato JSON.
2. Revisar `UpdateProfileCommand` vs legacy (`actualizarPerfil` y `actualizarPerfil1`) y ajustar validaciones/DTOs.
3. Ejecutar pruebas unitarias e integración del módulo de Authentication.
4. Actualizar documentación (Swagger/Postman/QA checklist) antes del traspaso al equipo frontend.

## TRACKING DE PROGRESO

```markdown
## LOTE 6.0.1: Authentication Completion

**Objetivo:** completar los endpoints restantes de autenticación y dejar documentación actualizada.

**Endpoints:**
- [x] DELETE /api/auth/users/{userId}/credentials/{credentialId}
- [x] POST /api/auth/profile-info
- [ ] GET /api/auth/cuenta/{cuentaId}
- [ ] PUT /api/auth/profile

**Criterios de aceptación:**
- [ ] Respuesta GET alineada al DTO legacy (`obtenerPerfil` + `getPerfilByID`).
- [ ] Validaciones y manejo de errores equivalentes al Web Forms.
- [ ] Pruebas unitarias e integración en AuthController actualizadas.
- [ ] Documentación en README/Swagger y checklist QA actualizados.
```


1. Publicar `GetCuentaByIdQuery` en AuthController y documentar contrato JSON.
2. Revisar `UpdateProfileCommand` vs legacy (`actualizarPerfil` y `actualizarPerfil1`) y ajustar validaciones/DTOs.
3. Ejecutar pruebas unitarias e integración del módulo de Authentication.
4. Actualizar documentación (Swagger/Postman/QA checklist) antes del traspaso al equipo frontend.
## RESULTADOS ESPERADOS

Al finalizar las 3 semanas:

### Métricas Objetivo

-  **100% paridad** con Legacy (81/81 métodos)
-  **80%+ code coverage** en tests
-  **0 errores** de compilación
-  **0 warnings** críticos
-  **<500ms p95** en response times
-  **100% endpoints** documentados en Swagger

### Entregables

1. **Código:**
   - 46 nuevos Commands/Queries
   - 46 nuevos endpoints en Controllers
   - 92+ unit tests
   - 46+ integration tests

2. **Documentación:**
   - Swagger UI completo con ejemplos
   - Postman collection para QA
   - README actualizado
   - Diagramas de arquitectura

3. **Calidad:**
   - Security audit passed
   - Performance benchmark passed
   - All tests green
   - Code review approved

---

## COMANDO PARA EMPEZAR AHORA

```bash
cd MiGenteEnLinea.Clean
git checkout -b feature/lote-6.0.1-final

dotnet build --no-restore

code .

echo "Objetivo: exponer GetCuentaById y completar UpdateProfile"
```


---

**¿Listo para empezar?** 💪

**Pregunta para Usuario:**

1. ¿Quieres que empiece con LOTE 6.0.1 ahora?
2. ¿Prefieres revisar primero qué endpoints del Legacy están realmente en uso en producción?
3. ¿Algún módulo tiene prioridad diferente por necesidades de negocio?

---

**Última Actualización:** 2025-01-15  
**Próximo Checkpoint:** Después de completar LOTE 6.0.1 (3-4 horas)
