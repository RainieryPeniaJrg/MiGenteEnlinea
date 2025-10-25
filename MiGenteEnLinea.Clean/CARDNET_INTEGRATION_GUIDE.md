# 🏦 Guía de Integración Cardnet Payment Gateway

**Fecha:** 24 de Octubre 2025  
**Proyecto:** MiGente En Línea - Payment Integration  
**Gateway:** Cardnet República Dominicana  
**Estado:** 📋 Documentación basada en código Legacy  

---

## 📊 Información Extraída del Legacy

### Configuración Cardnet (desde `PaymentService.cs`)

```csharp
// URL Endpoints
string testURL = "https://ecommerce.cardnet.com.do/api/payment/"; // Sandbox
string productionURL = "[URL a confirmar con Cardnet]";            // Producción

// Endpoints específicos
string salesEndpoint = testURL + "sales";                          // Procesar venta
string idempotencyEndpoint = testURL.Replace("/transactions/", "/idenpotency-keys"); // Generar key
```

### Parámetros de Configuración (tabla `PaymentGateway`)

| Campo | Descripción | Ejemplo |
|-------|-------------|---------|
| `merchantID` | ID del comercio en Cardnet | "349000001" (ejemplo Legacy) |
| `terminalID` | ID de la terminal | "[Por configurar]" |
| `test` | Flag modo sandbox | `true` para desarrollo |
| `testURL` | URL de sandbox | `https://ecommerce.cardnet.com.do/api/payment/` |
| `productionURL` | URL de producción | "[Por confirmar con Cardnet]" |

---

## 🔐 Flujo de Pago Completo

### PASO 1: Generar Idempotency Key

**Propósito:** Prevenir transacciones duplicadas  
**Endpoint:** `POST https://ecommerce.cardnet.com.do/api/payment/idenpotency-keys`  
**Headers:**

```
Accept: text/plain
Content-Type: application/json
```

**Request:**

```http
POST /api/payment/idenpotency-keys HTTP/1.1
Host: ecommerce.cardnet.com.do
Accept: text/plain
```

**Response (Success):**

```
ikey:a1b2c3d4-e5f6-7890-abcd-ef1234567890
```

**Código Legacy:**

```csharp
HttpResponseMessage response = await client.PostAsync(url.Replace("/transactions/", "/idenpotency-keys"), null);
string plainTextResponse = await response.Content.ReadAsStringAsync();
string idempotency = plainTextResponse?.Substring("ikey:".Length); // Remover prefijo "ikey:"
```

---

### PASO 2: Procesar Pago

**Endpoint:** `POST https://ecommerce.cardnet.com.do/api/payment/sales`  
**Headers:**

```
Content-Type: application/json
Authorization: [Confirmar si se requiere]
```

**Request Body (JSON):**

```json
{
    "amount": 1500.50,
    "card-number": "4111111111111111",
    "client-ip": "192.168.1.100",
    "currency": "214",
    "cvv": "123",
    "environment": "ECommerce",
    "expiration-date": "12/25",
    "idempotency-key": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    "merchant-id": "349000001",
    "reference-number": "REF-20251024-001",
    "terminal-id": "TERM-001",
    "token": "454500350001",
    "invoice-number": "INV-2025-1024-001"
}
```

**Campos Importantes:**

- `amount`: Monto decimal (ej: 1500.50)
- `card-number`: **ENCRIPTADO** con `Crypt.Decrypt()` antes de enviar
- `client-ip`: IP del cliente (para detección de fraude)
- `currency`: **"214"** = Peso Dominicano (DOP)
- `cvv`: CVV de la tarjeta (3-4 dígitos)
- `environment`: **"ECommerce"** para transacciones web
- `expiration-date`: Formato "MM/YY"
- `idempotency-key`: Del PASO 1
- `merchant-id`: ID del comercio
- `reference-number`: Referencia única de la transacción
- `terminal-id`: ID de la terminal
- `token`: Token estático "454500350001" (Legacy hardcoded)
- `invoice-number`: Número de factura único

**Response (Success):**

```json
{
    "idempotency-key": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    "response-code": "00",
    "internal-response-code": "00",
    "response-code-desc": "Transacción Aprobada",
    "response-code-source": "Emisor",
    "approval-code": "123456",
    "pnRef": "TXN-2025102401"
}
```

**Response Codes:**

| Code | Descripción | Acción |
|------|-------------|--------|
| `00` | Aprobada | ✅ Procesar venta |
| `01` | Rechazada | ❌ Informar al usuario |
| `05` | No autorizada | ❌ Informar al usuario |
| `14` | Tarjeta inválida | ❌ Verificar datos |
| `51` | Fondos insuficientes | ❌ Informar al usuario |
| `91` | Emisor no disponible | ⏳ Reintentar más tarde |

---

## 🔒 Encriptación de Tarjetas

### ClassLibrary_CSharp.Encryption.Crypt

**Problema:** La clase `Crypt` no está disponible en Clean Architecture  
**Solución:** Port manual de la lógica de encriptación

**Código Legacy:**

```csharp
using ClassLibrary_CSharp.Encryption;

Crypt crypt = new Crypt();
string encryptedCard = crypt.Encrypt(cardNumber); // Para almacenar en DB
string decryptedCard = crypt.Decrypt(encryptedCard); // Para enviar a Cardnet
```

### ⚠️ Tareas Pendientes

1. **Obtener código fuente de `ClassLibrary_CSharp.Encryption.Crypt`**
   - Ubicación Legacy: `..\..\Utility_Suite\Utility_POS\Utility_POS\bin\Debug\ClassLibrary CSharp.dll`
   - Acción: Decompile o solicitar código fuente

2. **Port a Clean Architecture:**
   - Crear `Infrastructure/Services/EncryptionService.cs`
   - Implementar `IEncryptionService` interface
   - Usar AES-256-CBC con salt/IV adecuados
   - **CRÍTICO:** Mantener compatibilidad con tarjetas encriptadas existentes en DB

3. **Alternativa (si no se puede decompile):**
   - Implementar nuevo sistema de encriptación AES-256
   - Migrar tarjetas existentes en DB al nuevo formato
   - **RIESGO:** Downtime durante migración

---

## 🧪 Testing con Tarjetas de Prueba

### Tarjetas de Prueba Cardnet (Sandbox)

**⚠️ NOTA:** Confirmar con Cardnet las tarjetas de prueba actuales

**Tarjetas Comunes de Prueba:**

| Número | Tipo | CVV | Fecha Exp | Resultado Esperado |
|--------|------|-----|-----------|-------------------|
| 4111111111111111 | VISA | 123 | 12/25 | ✅ Aprobada |
| 5500000000000004 | MasterCard | 123 | 12/25 | ✅ Aprobada |
| 4111111111111112 | VISA | 123 | 12/25 | ❌ Rechazada |
| 5555555555554444 | MasterCard | 123 | 12/25 | ❌ Fondos insuficientes |

**Montos Especiales (para testing):**

- `0.01` - `9.99`: Aprobado
- `10.00` - `99.99`: Rechazado por monto
- `100.00+`: Aprobado (si tarjeta válida)

---

## 📝 Checklist de Configuración

### Pre-requisitos para Cardnet

- [ ] **Cuenta activa en Cardnet República Dominicana**
  - Contacto: [URL/email de Cardnet]
  - Teléfono: [Por confirmar]

- [ ] **Credenciales Sandbox:**
  - [ ] Merchant ID de prueba
  - [ ] Terminal ID de prueba
  - [ ] URL de sandbox confirmada
  - [ ] Tarjetas de prueba validadas

- [ ] **Credenciales Producción:**
  - [ ] Merchant ID real
  - [ ] Terminal ID real
  - [ ] URL de producción confirmada
  - [ ] Certificados SSL (si aplica)

- [ ] **Configuración Base de Datos:**
  - [ ] Tabla `PaymentGateway` poblada con datos correctos
  - [ ] Test flag configurado correctamente

- [ ] **Código:**
  - [ ] EncryptionService implementado y probado
  - [ ] Compatibilidad con tarjetas encriptadas Legacy
  - [ ] Logging configurado (sin exponer datos sensibles)
  - [ ] Rate limiting en endpoints de pago

---

## 🚨 Seguridad PCI-DSS

### Requerimientos Obligatorios

#### ✅ DO (Hacer)

- ✅ Usar HTTPS para TODO el flujo de pago
- ✅ Encriptar números de tarjeta antes de almacenar (AES-256)
- ✅ **NUNCA** almacenar CVV en base de datos
- ✅ Validar formato de tarjeta en cliente Y servidor
- ✅ Usar idempotency keys para prevenir duplicados
- ✅ Logging detallado (sin datos sensibles)
- ✅ Rate limiting agresivo en endpoints de pago
- ✅ Tokenización de tarjetas si es posible

#### ❌ DON'T (No hacer)

- ❌ Almacenar CVV en logs, DB, o cualquier lugar
- ❌ Loggear números de tarjeta completos
- ❌ Enviar datos de tarjeta por email/SMS
- ❌ Usar HTTP para transacciones de pago
- ❌ Hardcodear merchant IDs en código (usar config)
- ❌ Exponer stack traces con datos de pago al cliente

### Logging Seguro

**✅ Ejemplo BUENO:**

```csharp
_logger.LogInformation(
    "Procesando pago. Referencia: {Reference}, Monto: {Amount}, Últimos 4 dígitos: {Last4}",
    referenceNumber,
    amount,
    cardNumber.Substring(cardNumber.Length - 4)); // Solo últimos 4
```

**❌ Ejemplo MALO:**

```csharp
_logger.LogInformation("Pago: {CardNumber}, CVV: {CVV}", cardNumber, cvv); // ¡NUNCA!
```

---

## 🔧 Implementación en Clean Architecture

### Estructura de Archivos a Crear

```
MiGenteEnLinea.Clean/
├── src/
│   ├── Application/
│   │   └── Common/
│   │       └── Interfaces/
│   │           ├── IPaymentService.cs                    [YA EXISTE]
│   │           └── IEncryptionService.cs                 [CREAR]
│   │
│   ├── Infrastructure/
│   │   └── MiGenteEnLinea.Infrastructure/
│   │       └── Services/
│   │           ├── CardnetPaymentService.cs              [CREAR - reemplaza Mock]
│   │           ├── EncryptionService.cs                  [CREAR]
│   │           └── MockPaymentService.cs                 [ELIMINAR después]
│   │
│   └── Presentation/
│       └── MiGenteEnLinea.API/
│           ├── appsettings.json
│           │   └── Cardnet:
│           │       ├── MerchantId: "[configurar]"
│           │       ├── TerminalId: "[configurar]"
│           │       ├── BaseUrl: "[configurar]"
│           │       └── IsTest: true
│           │
│           └── appsettings.Production.json
│               └── Cardnet:
│                   ├── MerchantId: "[PRODUCCIÓN]"
│                   ├── TerminalId: "[PRODUCCIÓN]"
│                   ├── BaseUrl: "[PRODUCCIÓN]"
│                   └── IsTest: false
```

---

## 📞 Contactos y Recursos

### Cardnet República Dominicana

**⚠️ ACCIÓN REQUERIDA:** Contactar a Cardnet para:

1. Confirmar URL actual de API (sandbox y producción)
2. Obtener credenciales de sandbox actualizadas
3. Validar formato de request (JSON structure)
4. Obtener tarjetas de prueba actuales
5. Confirmar códigos de respuesta
6. Verificar si hay headers adicionales requeridos
7. Obtener documentación oficial actualizada

**Información de Contacto:**

- Website: <https://www.cardnet.com.do/>
- Email: [Por confirmar]
- Teléfono: [Por confirmar]
- Soporte Técnico: [Por confirmar]

### Documentación Externa

- [x] Código Legacy analizado (`PaymentService.cs`)
- [ ] Documentación oficial de Cardnet API
- [ ] Especificación de códigos de respuesta
- [ ] Guía de integración para E-Commerce
- [ ] Certificación PCI-DSS requirements

---

## 🎯 Próximos Pasos

### INMEDIATO (Antes de codificar)

1. **Contactar Cardnet** (1 hora)
   - Solicitar documentación actualizada
   - Confirmar URLs de API
   - Obtener credenciales de sandbox

2. **Obtener Crypt Class** (1 hora)
   - Decompile ClassLibrary_CSharp.dll
   - O solicitar código fuente al equipo Legacy

3. **Configurar appsettings.json** (30 min)
   - Agregar sección `Cardnet`
   - Configurar con datos de sandbox

### DESARROLLO (32 horas - LOTE 1)

Ver `PLAN_INTEGRACION_API_COMPLETO.md` sección "LOTE 1: Payment Gateway Integration"

---

**Última actualización:** 2025-10-24 19:45 UTC-4  
**Estado:** 📋 Documentación lista - Esperando validación de Cardnet
