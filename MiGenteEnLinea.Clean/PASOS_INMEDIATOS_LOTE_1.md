# 🚀 Pasos Inmediatos para LOTE 1 - Payment Gateway

**Fecha:** 24 de Octubre 2025, 19:50  
**Estado:** ⚠️ BLOQUEADORES IDENTIFICADOS - Acción requerida  
**Responsable:** Equipo de desarrollo  

---

## 📊 Resumen de Estado

✅ **COMPLETADO:**

- Análisis exhaustivo de código Legacy (9 servicios, 98 métodos)
- Mapeo completo Legacy → Clean (20 gaps identificados)
- Plan de integración detallado (5 LOTEs, 76 horas)
- Documentación Cardnet extraída del código Legacy
- API corriendo exitosamente en <http://localhost:5015>

⚠️ **BLOQUEADORES CRÍTICOS:**

1. **ClassLibrary_CSharp.Encryption.Crypt** - DLL externa no disponible
2. **Credenciales Cardnet Sandbox** - No disponibles en el código
3. **Validación de Endpoints Cardnet** - URLs pueden haber cambiado

---

## 🚨 BLOQUEADOR #1: Clase Crypt Missing (CRÍTICO)

### Problema

El código Legacy usa `ClassLibrary_CSharp.Encryption.Crypt` para encriptar/desencriptar números de tarjetas:

```csharp
using ClassLibrary_CSharp.Encryption;

Crypt crypt = new Crypt();
string decryptedCard = crypt.Decrypt(cardNumber); // Necesario para enviar a Cardnet
```

**Esta DLL NO está en el repositorio** y es referenciada desde:

```
..\..\Utility_Suite\Utility_POS\Utility_POS\bin\Debug\ClassLibrary CSharp.dll
```

### Opciones de Solución

#### OPCIÓN A: Buscar DLL Original (2 horas)

**Acción:**

1. Buscar en el servidor de desarrollo/producción Legacy
2. Ubicación probable: `[Servidor IIS]\MiGente\bin\ClassLibrary CSharp.dll`
3. Si se encuentra, decompile con **ILSpy** o **dnSpy**
4. Extraer código fuente de la clase `Crypt`

**Comando para buscar:**

```powershell
# En servidor de producción
Get-ChildItem -Path "C:\inetpub\wwwroot" -Filter "ClassLibrary*.dll" -Recurse
```

**Ventaja:** ✅ Compatibilidad 100% con tarjetas encriptadas existentes en DB
**Riesgo:** ❌ Si no se encuentra, bloquea todo el desarrollo

#### OPCIÓN B: Implementar Nuevo Sistema de Encriptación (8 horas)

**Acción:**

1. Implementar `EncryptionService` con AES-256-CBC
2. Crear script de migración para re-encriptar tarjetas existentes
3. Actualizar todas las tarjetas en DB con nuevo formato

**Código propuesto:**

```csharp
public class EncryptionService : IEncryptionService
{
    private readonly string _encryptionKey; // De appsettings.json
    
    public string Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Convert.FromBase64String(_encryptionKey);
            aes.GenerateIV();
            
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                msEncrypt.Write(aes.IV, 0, aes.IV.Length);
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }
                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }
    
    public string Decrypt(string cipherText)
    {
        byte[] buffer = Convert.FromBase64String(cipherText);
        
        using (Aes aes = Aes.Create())
        {
            aes.Key = Convert.FromBase64String(_encryptionKey);
            
            byte[] iv = new byte[aes.IV.Length];
            Array.Copy(buffer, 0, iv, 0, iv.Length);
            aes.IV = iv;
            
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            
            using (MemoryStream msDecrypt = new MemoryStream(buffer, iv.Length, buffer.Length - iv.Length))
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
            {
                return srDecrypt.ReadToEnd();
            }
        }
    }
}
```

**Ventaja:** ✅ Control total del algoritmo, más seguro (AES-256)
**Riesgo:** ❌ Requiere migración de todas las tarjetas (downtime)

#### OPCIÓN C: Hybrid Approach (4 horas + migración)

1. Implementar nuevo `EncryptionService` (AES-256)
2. **NO migrar tarjetas Legacy** inicialmente
3. Crear `LegacyCryptAdapter` que intenta descifrar con DLL si existe, si no usa nuevo sistema
4. Nuevas tarjetas usan AES-256 nuevo
5. Tarjetas viejas se re-encriptan al primer uso

**Ventaja:** ✅ No requiere downtime, migración gradual
**Riesgo:** ⚠️ Requiere mantener DLL Legacy temporalmente

---

## 🚨 BLOQUEADOR #2: Credenciales Cardnet (CRÍTICO)

### Problema

Las credenciales de Cardnet están almacenadas en la tabla `PaymentGateway` de la base de datos Legacy:

```sql
SELECT merchantID, terminalID, testURL, productionURL, test 
FROM PaymentGateway;
```

**Acción inmediata requerida:**

### PASO 1: Extraer Credenciales de DB Legacy (15 minutos)

```powershell
# Conectar a SQL Server PersonalPC
sqlcmd -S PersonalPC -U sa -P Volumen#1 -d MiGenteDev -Q "SELECT * FROM PaymentGateway"
```

**O desde SSMS:**

```sql
USE MiGenteDev;
GO

SELECT 
    id,
    merchantID,
    terminalID,
    testURL,
    productionURL,
    test
FROM PaymentGateway;
```

**Guardar resultados en archivo seguro (NO commit a Git):**

- `config/cardnet-credentials.txt` (agregado a .gitignore)

### PASO 2: Contactar Cardnet (1-2 días)

**⚠️ ACCIÓN REQUERIDA:** Contactar soporte de Cardnet para:

1. ✅ Validar que `merchantID` y `terminalID` actuales son válidos
2. ✅ Confirmar URLs actuales (pueden haber cambiado):
   - Sandbox: `https://ecommerce.cardnet.com.do/api/payment/`
   - Producción: `[confirmar]`
3. ✅ Obtener credenciales de **SANDBOX** para testing (si las actuales son de producción)
4. ✅ Solicitar documentación oficial actualizada
5. ✅ Confirmar tarjetas de prueba vigentes
6. ✅ Verificar códigos de respuesta actuales

**Contacto Cardnet:**

- Website: <https://www.cardnet.com.do/>
- Email: `[buscar en website]`
- Teléfono: `[buscar en website]`
- Soporte técnico: `[solicitar contacto directo]`

**Documento a solicitar:**

- "Cardnet E-Commerce Integration Guide" (versión actualizada)
- Lista de códigos de respuesta
- Especificación JSON de request/response

---

## 🚨 BLOQUEADOR #3: Validación de Endpoints (MEDIA)

### URLs Extraídas del Legacy

```
Sandbox (test): https://ecommerce.cardnet.com.do/api/payment/
Producción:     [No especificado en código Legacy]

Endpoints específicos:
- Idempotency: /idenpotency-keys (POST)
- Sales:       /sales (POST)
```

### Acción Requerida (30 minutos)

**Probar conectividad a Cardnet Sandbox:**

```powershell
# Test 1: Verificar que el dominio existe
Test-NetConnection ecommerce.cardnet.com.do -Port 443

# Test 2: Intentar llamar endpoint de idempotency (probablemente falle sin credenciales)
Invoke-WebRequest -Uri "https://ecommerce.cardnet.com.do/api/payment/idenpotency-keys" -Method POST
```

**Resultado esperado:**

- ✅ `200 OK` o `401 Unauthorized` → Endpoint activo
- ❌ `404 Not Found` o timeout → URL cambió, contactar Cardnet

---

## 📋 Plan de Acción Inmediato (Próximas 4-6 horas)

### ⏰ AHORA MISMO (30 minutos)

1. **Extraer credenciales de DB Legacy:**

   ```powershell
   sqlcmd -S PersonalPC -U sa -P Volumen#1 -d MiGenteDev -Q "SELECT * FROM PaymentGateway" -o cardnet-creds.txt
   ```

2. **Probar conectividad Cardnet:**

   ```powershell
   Test-NetConnection ecommerce.cardnet.com.do -Port 443
   Invoke-WebRequest -Uri "https://ecommerce.cardnet.com.do/api/payment/idenpotency-keys" -Method POST
   ```

3. **Buscar DLL Crypt en servidor Legacy:**
   - Si tienes acceso: buscar en `C:\inetpub\wwwroot\MiGente\bin\`
   - Si no: solicitar al administrador del servidor

### ⏰ HOY/MAÑANA (2-4 horas)

4. **Contactar Cardnet:**
   - Email/llamada solicitando validación de credenciales
   - Pedir acceso a sandbox + documentación actualizada

5. **Decidir estrategia para Crypt:**
   - ¿Se encontró DLL? → Decompile con ILSpy
   - ¿No se encontró? → Implementar Opción B o C

6. **Configurar `appsettings.Development.json`:**

   ```json
   "Cardnet": {
     "MerchantId": "[extraído de DB]",
     "TerminalId": "[extraído de DB]",
     "BaseUrl": "https://ecommerce.cardnet.com.do/api/payment/",
     "IsTest": true
   },
   "Encryption": {
     "Key": "[generar nuevo AES-256 key]"
   }
   ```

### ⏰ DESPUÉS DE DESBLOQUEAR (32 horas)

7. **Implementar LOTE 1 completo** (ver `PLAN_INTEGRACION_API_COMPLETO.md`)

---

## 🎯 Preguntas para el Equipo

**Responder lo antes posible para desbloquear desarrollo:**

1. ⚠️ **¿Tienes acceso al servidor donde corre el Legacy actualmente?**
   - Si SÍ: Buscar `ClassLibrary CSharp.dll` en `bin\`
   - Si NO: ¿Quién puede extraer el DLL?

2. ⚠️ **¿Las credenciales en DB son de SANDBOX o PRODUCCIÓN?**
   - Si PRODUCCIÓN: **NO USAR** para testing, solicitar sandbox a Cardnet
   - Si SANDBOX: Validar que siguen vigentes

3. ⚠️ **¿Hay contacto directo con soporte técnico de Cardnet?**
   - Si SÍ: Contactar hoy mismo
   - Si NO: Usar formulario web (puede tardar 1-2 días)

4. ⚠️ **¿Preferencia para solución de Crypt?**
   - Opción A: Buscar DLL original (más rápido si se encuentra)
   - Opción B: Nuevo sistema AES-256 (más seguro, requiere migración)
   - Opción C: Hybrid (sin downtime, más complejo)

---

## 📊 Impacto en el Cronograma

**LOTE 1 Original:** 32 horas  
**Bloqueadores agregados:** +6-10 horas (dependiendo de soluciones)

**Nuevo cronograma estimado:**

- Resolver bloqueadores: 6-10 horas
- LOTE 1 implementación: 32 horas
- **Total: 38-42 horas (5-6 días de trabajo)**

**Fecha estimada de completado:**

- Inicio: Hoy (24 Oct 2025)
- Completado: ~30 Oct - 1 Nov 2025

---

## ✅ Checklist de Desbloqueo

Marcar cuando se complete cada item:

- [ ] Credenciales extraídas de DB Legacy
- [ ] Conectividad a Cardnet validada (endpoints activos)
- [ ] DLL Crypt encontrado O estrategia alternativa decidida
- [ ] Contacto con Cardnet iniciado
- [ ] Documentación oficial de Cardnet recibida
- [ ] Credenciales de Sandbox confirmadas/obtenidas
- [ ] `appsettings.json` configurado con datos reales
- [ ] Encryption key generado y almacenado de forma segura

**Una vez marcados todos los items anteriores, podemos comenzar la implementación de LOTE 1.**

---

**Última actualización:** 2025-10-24 19:50 UTC-4  
**Estado:** ⚠️ Bloqueadores identificados - Esperando acciones del equipo  
**Próximo paso:** Ejecutar acciones de "AHORA MISMO" (30 minutos)
