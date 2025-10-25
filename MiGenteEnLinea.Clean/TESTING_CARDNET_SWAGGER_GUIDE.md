# 🧪 GUÍA DE TESTING: CardnetPaymentService con Swagger UI

**Fecha:** 24 de Octubre 2025, 20:13  
**API:** <http://localhost:5015>  
**Swagger UI:** <http://localhost:5015/swagger>  
**Estado:** ✅ API corriendo, TestPaymentController disponible  

---

## 📋 TESTS DISPONIBLES

El controlador `TestPaymentController` expone 3 endpoints para testing:

### TEST 1: Generar Idempotency Key

- **Endpoint:** `POST /api/testpayment/idempotency-key`
- **Método:** `GenerateIdempotencyKey`
- **Propósito:** Validar conectividad con Cardnet y generación de keys

### TEST 2: Procesar Pago Completo

- **Endpoint:** `POST /api/testpayment/process-payment`
- **Método:** `ProcessPayment`
- **Propósito:** Probar flujo completo de pago con tarjetas de prueba

### TEST 3: Obtener Configuración Gateway

- **Endpoint:** `GET /api/testpayment/gateway-config`
- **Método:** `GetGatewayConfig`
- **Propósito:** Verificar credenciales y URLs configuradas

---

## 🎯 TEST 1: Generar Idempotency Key

### Pasos en Swagger UI

1. **Abrir Swagger:** <http://localhost:5015/swagger>
2. **Buscar:** `TestPayment` en la lista de controladores
3. **Expandir:** `POST /api/testpayment/idempotency-key`
4. **Click:** "Try it out"
5. **Click:** "Execute"

### Resultado Esperado (200 OK)

```json
{
  "idempotencyKey": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
  "success": true,
  "message": "Idempotency key generada exitosamente"
}
```

### Validaciones

- ✅ Response code: `200 OK`
- ✅ `success`: `true`
- ✅ `idempotencyKey`: GUID válido (36 caracteres)
- ✅ Logs en consola: "Idempotency key generada exitosamente"

### Si falla

**Error 500:**

```json
{
  "success": false,
  "message": "Error al generar idempotency key",
  "error": "[mensaje de error]"
}
```

**Posibles causas:**

- ❌ Sin conectividad a lab.cardnet.com.do
- ❌ Credenciales inválidas en DB
- ❌ URL incorrecta en PaymentGateway table
- ❌ Firewall bloqueando puerto 443

**Solución:**

```powershell
# Verificar conectividad
Test-NetConnection lab.cardnet.com.do -Port 443

# Verificar credenciales en DB
sqlcmd -S localhost,1433 -d MiGenteDev -Q "SELECT * FROM PaymentGateways"
```

---

## 🎯 TEST 2: Procesar Pago con Tarjeta VISA (Aprobado)

### Pasos en Swagger UI

1. **Expandir:** `POST /api/testpayment/process-payment`
2. **Click:** "Try it out"
3. **Copiar JSON de Request Body:**

```json
{
  "amount": 100.50,
  "cardNumber": "4111111111111111",
  "cvv": "123",
  "expirationDate": "12/25"
}
```

4. **Pegar** en el editor de Request Body
5. **Click:** "Execute"

### Resultado Esperado (200 OK)

```json
{
  "success": true,
  "responseCode": "00",
  "responseDescription": "Transacción aprobada",
  "approvalCode": "123456",
  "transactionReference": "TXN-20251024001",
  "idempotencyKey": "a1b2c3d4-e5f6-7890-abcd-ef1234567890"
}
```

### Validaciones

- ✅ Response code: `200 OK`
- ✅ `success`: `true`
- ✅ `responseCode`: `"00"` (Aprobado)
- ✅ `approvalCode`: Valor no nulo (generado por Cardnet)
- ✅ `transactionReference` (pnRef): Valor no nulo
- ✅ Logs en consola: "Pago APROBADO"
- ✅ Card enmascarada en logs: `****-****-****-1111` (NO número completo)

### Logs Esperados

```
[20:13:45 INF] TEST: Procesando pago de prueba. Monto: 100.50, Últimos 4: ****-****-****-1111
[20:13:46 INF] Generando idempotency key. URL: https://lab.cardnet.com.do/api/payment/idenpotency-keys
[20:13:47 INF] Idempotency key generada exitosamente: a1b2c3d4-...
[20:13:47 INF] Procesando pago. Monto: 100.50, Referencia: 20251024201347-1234, Idempotency: a1b2c3d4-..., Últimos 4: ****-****-****-1111
[20:13:48 INF] Enviando pago a Cardnet. URL: https://lab.cardnet.com.do/api/payment/transactions/sales
[20:13:49 INF] Pago APROBADO. Referencia: 20251024201347-1234, ApprovalCode: 123456, PnRef: TXN-20251024001
[20:13:49 INF] TEST: Pago APROBADO. ApprovalCode: 123456, PnRef: TXN-20251024001
```

---

## 🎯 TEST 3: Procesar Pago con MasterCard (Aprobado)

### Request Body

```json
{
  "amount": 250.75,
  "cardNumber": "5500000000000004",
  "cvv": "456",
  "expirationDate": "06/26"
}
```

### Resultado Esperado

- Similar a TEST 2
- `responseCode`: `"00"` (Aprobado)
- Card enmascarada: `****-****-****-0004`

---

## 🎯 TEST 4: Procesar Pago con Tarjeta Rechazada

### Request Body

```json
{
  "amount": 500.00,
  "cardNumber": "4111111111111112",
  "cvv": "789",
  "expirationDate": "03/24"
}
```

### Resultado Esperado (200 OK pero pago rechazado)

```json
{
  "success": false,
  "responseCode": "01",
  "responseDescription": "Tarjeta rechazada",
  "approvalCode": null,
  "transactionReference": null,
  "idempotencyKey": "a1b2c3d4-..."
}
```

### Validaciones

- ✅ Response code: `200 OK` (la API funcionó)
- ❌ `success`: `false` (el pago fue rechazado)
- ✅ `responseCode`: `"01"` o `"14"` (Rechazada/Tarjeta inválida)
- ✅ `approvalCode`: `null` (no se aprobó)
- ✅ Logs en consola: "Pago RECHAZADO"

### Logs Esperados

```
[20:14:05 WRN] Pago RECHAZADO. Referencia: 20251024201405-5678, Código: 01, Descripción: Tarjeta rechazada
[20:14:05 WRN] TEST: Pago RECHAZADO. Código: 01, Descripción: Tarjeta rechazada
```

---

## 🎯 TEST 5: Fondos Insuficientes

### Request Body

```json
{
  "amount": 10000.00,
  "cardNumber": "5555555555554444",
  "cvv": "111",
  "expirationDate": "09/25"
}
```

### Resultado Esperado

```json
{
  "success": false,
  "responseCode": "51",
  "responseDescription": "Fondos insuficientes",
  "approvalCode": null,
  "transactionReference": null,
  "idempotencyKey": "..."
}
```

---

## 🎯 TEST 6: Obtener Configuración Gateway

### Pasos en Swagger UI

1. **Expandir:** `GET /api/testpayment/gateway-config`
2. **Click:** "Try it out"
3. **Click:** "Execute"

### Resultado Esperado (200 OK)

```json
{
  "merchantId": "349041263",
  "terminalId": "77777777",
  "baseUrl": "https://lab.cardnet.com.do/api/payment/transactions/",
  "isTest": true
}
```

### Validaciones

- ✅ Response code: `200 OK`
- ✅ `merchantId`: `"349041263"` (de DB)
- ✅ `terminalId`: `"77777777"` (de DB)
- ✅ `baseUrl`: `"https://lab.cardnet.com.do/api/payment/transactions/"`
- ✅ `isTest`: `true` (modo sandbox)

---

## 📊 MATRIZ DE CASOS DE PRUEBA

| # | Test Case | Tarjeta | Monto | Código Esperado | Estado Esperado |
|---|-----------|---------|-------|-----------------|-----------------|
| 1 | Visa Aprobada | 4111111111111111 | 100.50 | 00 | ✅ Aprobado |
| 2 | MasterCard Aprobada | 5500000000000004 | 250.75 | 00 | ✅ Aprobado |
| 3 | Visa Rechazada | 4111111111111112 | 500.00 | 01/14 | ❌ Rechazado |
| 4 | Fondos Insuficientes | 5555555555554444 | 10000.00 | 51 | ❌ Rechazado |
| 5 | Monto bajo | 4111111111111111 | 0.50 | 00 | ✅ Aprobado |
| 6 | Monto alto | 4111111111111111 | 9999.99 | 00 | ✅ Aprobado |

---

## 🔒 VALIDACIÓN DE SEGURIDAD (PCI-DSS)

### ✅ Lo que DEBE aparecer en logs

```
[INF] Procesando pago. Monto: 100.50, Últimos 4: ****-****-****-1111
[INF] Pago APROBADO. ApprovalCode: 123456
```

### ❌ Lo que NUNCA debe aparecer en logs

```
❌ [INF] CardNumber: 4111111111111111  <-- VIOLACIÓN PCI-DSS
❌ [INF] CVV: 123                       <-- VIOLACIÓN PCI-DSS
❌ [INF] Sending: {"card-number": "4111111111111111"}  <-- VIOLACIÓN
```

### Verificar Logs

```powershell
# Ver logs en tiempo real
Get-Content -Path "C:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea.Clean\src\Presentation\MiGenteEnLinea.API\bin\Debug\net8.0\logs\log.txt" -Wait

# Buscar si hay números de tarjeta en logs (NO debe haber matches)
Select-String -Path "logs\*.txt" -Pattern "\d{16}" -SimpleMatch
```

**Resultado esperado:** 0 matches (no debe haber tarjetas completas en logs)

---

## ⏱️ VALIDACIÓN DE PERFORMANCE

### Objetivo: Transacción completa < 5 segundos

**Medición manual:**

1. Anotar tiempo de "Execute" en Swagger
2. Esperar respuesta
3. Verificar que el tiempo total < 5000ms

**Componentes del tiempo:**

- Idempotency key: ~1-2 segundos
- Payment processing: ~2-3 segundos
- **Total esperado:** 3-5 segundos

**Si tarda más de 5 segundos:**

- Verificar conectividad a Cardnet
- Verificar logs para timeouts
- Verificar retry policy (3 intentos con backoff)

---

## 🚨 TROUBLESHOOTING

### Error 500: "No se encontró configuración de PaymentGateway"

**Causa:** Tabla `PaymentGateways` vacía en DB

**Solución:**

```sql
USE MiGenteDev;
GO

SELECT * FROM PaymentGateways;
-- Si está vacía, insertar credenciales:

INSERT INTO PaymentGateways (MerchantId, TerminalId, UrlTest, UrlProduccion, ModoTest, Activa)
VALUES (
    '349041263',
    '77777777',
    'https://lab.cardnet.com.do/api/payment/transactions/',
    'https://lab.cardnet.com.do/api/payment/transactions/',
    1,
    1
);
```

### Error 500: "Error al generar idempotency key: 404"

**Causa:** URL incorrecta en DB

**Solución:**

```sql
UPDATE PaymentGateways
SET UrlTest = 'https://lab.cardnet.com.do/api/payment/transactions/',
    UrlProduccion = 'https://lab.cardnet.com.do/api/payment/transactions/'
WHERE Id = 1;
```

### Error: "Formato de idempotency key inválido"

**Causa:** Respuesta de Cardnet no tiene formato "ikey:XXXXX"

**Solución:**

- Verificar que Cardnet sandbox esté funcionando
- Contactar soporte Cardnet para validar endpoint
- Revisar logs para ver respuesta completa

### Error: Circuit Breaker abierto

**Causa:** 5+ fallos consecutivos → circuito abierto por 30s

**Solución:**

- Esperar 30 segundos
- Verificar conectividad a Cardnet
- Revisar logs para identificar causa de fallos

---

## ✅ CHECKLIST DE VALIDACIÓN

### Antes de ejecutar tests

- [ ] API corriendo en <http://localhost:5015>
- [ ] Swagger UI accesible en <http://localhost:5015/swagger>
- [ ] TestPaymentController visible en Swagger
- [ ] Credenciales en DB (`SELECT * FROM PaymentGateways`)
- [ ] Conectividad Cardnet (`Test-NetConnection lab.cardnet.com.do -Port 443`)

### Durante testing

- [ ] Test 1: Idempotency key generada (200 OK)
- [ ] Test 2: Pago Visa aprobado (responseCode: "00")
- [ ] Test 3: Pago MasterCard aprobado (responseCode: "00")
- [ ] Test 4: Pago rechazado (responseCode: "01" o "14")
- [ ] Test 5: Fondos insuficientes (responseCode: "51")
- [ ] Test 6: Configuración gateway obtenida

### Validaciones de seguridad

- [ ] Logs NO contienen números de tarjeta completos
- [ ] Logs NO contienen CVV
- [ ] Cards enmascaradas en logs: `****-****-****-XXXX`
- [ ] Approval codes visibles en logs

### Validaciones de performance

- [ ] Idempotency key < 2 segundos
- [ ] Payment processing < 5 segundos total
- [ ] Retry policy funciona en caso de error

---

## 📝 REPORTE DE RESULTADOS

### Formato de Reporte

```markdown
## Test Execution Report
**Fecha:** 2025-10-24
**Ejecutor:** [Nombre]
**Ambiente:** Development (Sandbox Cardnet)

### Test 1: Idempotency Key
- ✅ Status: PASSED
- Response Code: 200 OK
- Idempotency Key: a1b2c3d4-e5f6-7890-abcd-ef1234567890
- Tiempo: 1.2s

### Test 2: Pago Visa Aprobado
- ✅ Status: PASSED
- Response Code: 200 OK
- Payment Response Code: 00 (Aprobado)
- Approval Code: 123456
- Transaction Reference: TXN-20251024001
- Tiempo: 4.5s

### Test 3: Pago Rechazado
- ✅ Status: PASSED
- Response Code: 200 OK
- Payment Response Code: 01 (Rechazado)
- Tiempo: 3.8s

### Seguridad (PCI-DSS)
- ✅ No card numbers en logs
- ✅ No CVV en logs
- ✅ Cards enmascaradas correctamente

### Performance
- ✅ Todas las transacciones < 5s
- ✅ Retry policy funciona

### Conclusión
✅ Todos los tests PASSED
CardnetPaymentService listo para integración en features reales.
```

---

## 🚀 PRÓXIMOS PASOS

### Después de validar todos los tests

1. **Documentar resultados** en `LOTE_1_PAYMENT_GATEWAY_COMPLETADO.md`
2. **Eliminar TestPaymentController** (solo para testing)
3. **Integrar IPaymentService** en Commands reales:
   - `CreateSuscripcionCommand` → Procesar pago al crear suscripción
   - `RenovarSuscripcionCommand` → Procesar pago al renovar
4. **Crear tabla Ventas** para almacenar historial de pagos
5. **Continuar con LOTE 2** (User Management gaps)

---

**Última actualización:** 2025-10-24 20:13 UTC-4  
**Estado:** ✅ API corriendo, TestPaymentController disponible  
**Swagger UI:** <http://localhost:5015/swagger>  
**Ready for testing!** 🚀
