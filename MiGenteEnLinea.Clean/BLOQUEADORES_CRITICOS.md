# 🚨 BLOQUEADORES CRÍTICOS - LOTE 1 Payment Gateway

**Fecha:** 24 de Octubre 2025, 20:10  
**Estado:** ⛔ DESARROLLO BLOQUEADO - Acción urgente requerida  
**Impacto:** No se puede implementar payment gateway real sin resolver estos bloqueadores  

---

## ⛔ BLOQUEADOR #1: Tabla PaymentGateway VACÍA (CRÍTICO)

### Estado Actual

```sql
-- Conectado a: Docker SQL Server (localhost:1433)
-- Base de datos: MiGenteDev

SELECT * FROM PaymentGateway;
-- RESULTADO: (0 rows) - Tabla VACÍA ❌
```

### Impacto

**SIN credenciales de Cardnet NO SE PUEDE:**
- ❌ Generar idempotency keys
- ❌ Procesar pagos reales
- ❌ Configurar ambiente de testing
- ❌ Probar con Cardnet Sandbox
- ❌ Completar LOTE 1 (32 horas bloqueadas)

### Acciones Inmediatas Requeridas

#### OPCIÓN A: Buscar en Servidor de Producción Legacy (1 hora)

**¿Dónde buscar?**

1. **Servidor IIS donde corre Legacy Web Forms:**
   ```sql
   -- Conectarse a base de datos de producción
   USE [db_a9f8ff_migente] -- O el nombre real
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

2. **Backup de base de datos Legacy:**
   - Si existe backup reciente, restaurar y extraer datos
   - Ubicación típica: `C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\Backup\`

3. **Archivos de configuración Legacy:**
   ```powershell
   # Buscar en servidor de producción
   Get-ChildItem -Path "C:\inetpub\wwwroot\MiGente" -Recurse -Filter "*.config" | 
       Select-String -Pattern "cardnet|merchant|gateway" -CaseSensitive:$false
   ```

**⚠️ PREGUNTA PARA EL EQUIPO:**
- ¿Tienes acceso al servidor donde está corriendo el Legacy actualmente?
- ¿Hay una base de datos de producción con datos reales?
- ¿Quién administra el servidor Legacy?

#### OPCIÓN B: Contactar Cardnet Directamente (1-3 días)

**Información de contacto:**
- Website: https://www.cardnet.com.do/
- Email comercial: [buscar en website - sección "Contáctanos"]
- Teléfono: [buscar en website]

**Qué solicitar:**

1. ✅ **Credenciales de SANDBOX para desarrollo:**
   - Merchant ID de prueba
   - Terminal ID de prueba
   - URL de API sandbox
   - API Key (si aplica)
   - Tarjetas de prueba

2. ✅ **Documentación oficial:**
   - "E-Commerce Integration Guide"
   - Especificación de API REST
   - Lista de códigos de respuesta
   - Ejemplos de request/response JSON

3. ✅ **Validación de implementación existente:**
   - Confirmar si MiGente En Línea ya tiene cuenta activa
   - Verificar Merchant ID de producción (si existe)
   - Status de la cuenta (activa/suspendida)

**Template de email:**

```
Asunto: Solicitud Credenciales Sandbox - MiGente En Línea

Estimado equipo de Cardnet,

Somos el equipo de desarrollo de MiGente En Línea, una plataforma de 
gestión de empleo en República Dominicana.

Estamos migrando nuestra plataforma a una nueva arquitectura y necesitamos:

1. Credenciales de SANDBOX para ambiente de testing:
   - Merchant ID de prueba
   - Terminal ID de prueba
   - URL de API (sandbox)

2. Documentación técnica actualizada:
   - Guía de integración E-Commerce API
   - Especificación de endpoints REST
   - Lista de códigos de respuesta

3. Validar cuenta existente:
   - Confirmar si ya tenemos cuenta activa (Merchant ID: 349000001)
   - Status actual de la cuenta

Nuestro contacto técnico: [TU EMAIL Y TELÉFONO]

Quedamos atentos a su respuesta.

Saludos cordiales,
Equipo de Desarrollo - MiGente En Línea
```

#### OPCIÓN C: Usar Credenciales del Código Legacy (30 minutos)

**Del análisis del código encontramos:**

```csharp
// PaymentService.cs líneas 56-65
var gatewayParams = getPaymentParameters();
// Extrae de tabla PaymentGateway:
// - merchantID
// - terminalID
// - testURL
// - productionURL
// - test (flag)
```

**Valores hardcoded encontrados:**

```csharp
// Token (línea 88 de PaymentService.cs)
"token": "454500350001"  // ¿Es estático o configurable?

// Merchant ID en INSTRUCTIONS_IMPROVED.md (ejemplo)
"CardnetMerchantId": "349000001"  // ¿Es el real?
```

**⚠️ PREGUNTA PARA EL EQUIPO:**
- ¿El Merchant ID "349000001" es real o solo un ejemplo?
- ¿Hay algún documento con las credenciales reales?
- ¿Quién configuró originalmente Cardnet en el Legacy?

### Solución Temporal (Para desarrollo local)

**MIENTRAS se obtienen credenciales reales, podemos:**

1. **Insertar datos de prueba en PaymentGateway:**

```sql
USE MiGenteDev;
GO

INSERT INTO PaymentGateway (
    merchantID, 
    terminalID, 
    testURL, 
    productionURL, 
    test
)
VALUES (
    '999999999',  -- FAKE merchant ID (reemplazar cuando tengamos el real)
    'TEST-TERM-001',  -- FAKE terminal ID
    'https://ecommerce.cardnet.com.do/api/payment/',  -- URL confirmada del código Legacy
    'https://ecommerce.cardnet.com.do/api/payment/',  -- Asumir misma URL hasta confirmar
    1  -- test = true (sandbox mode)
);

SELECT * FROM PaymentGateway;
GO
```

2. **Continuar desarrollo con MockPaymentService:**
   - Implementar toda la lógica EXCEPTO la llamada real a Cardnet
   - Crear CardnetPaymentService con método stub
   - Unit tests con mocks

3. **Feature flag para payment:**
   ```json
   // appsettings.Development.json
   {
     "Features": {
       "UseRealPaymentGateway": false,  // false = usa MOCK
       "MockPaymentAlwaysSucceed": true
     }
   }
   ```

**⚠️ LIMITACIÓN:** No se pueden hacer pruebas reales con tarjetas hasta tener credenciales

---

## ⛔ BLOQUEADOR #2: ClassLibrary_CSharp.Encryption.Crypt (CRÍTICO)

### Estado Actual

```
REFERENCIA EN CÓDIGO LEGACY:
using ClassLibrary_CSharp.Encryption;

Crypt crypt = new Crypt();
string decryptedCard = crypt.Decrypt(cardNumber);

UBICACIÓN DLL (según código):
..\..\Utility_Suite\Utility_POS\Utility_POS\bin\Debug\ClassLibrary CSharp.dll

ESTADO: ❌ No disponible en repositorio
```

### Impacto

**SIN clase Crypt NO SE PUEDE:**
- ❌ Desencriptar números de tarjeta almacenados en DB
- ❌ Enviar números de tarjeta válidos a Cardnet
- ❌ Procesar pagos reales
- ❌ Migrar a nuevo sistema de encriptación (sin conocer el algoritmo actual)

### Acciones Inmediatas Requeridas

#### OPCIÓN A: Buscar DLL en Servidor Legacy (30 minutos)

**Ubicaciones posibles:**

1. **Servidor IIS - bin folder:**
   ```powershell
   # En servidor de producción
   Get-ChildItem -Path "C:\inetpub\wwwroot\MiGente\bin" -Filter "ClassLibrary*.dll" -Recurse
   
   # O en desarrollo
   Get-ChildItem -Path "C:\[PATH_TO_LEGACY_PROJECT]\MiGente_Front\bin" -Filter "ClassLibrary*.dll"
   ```

2. **Utility Suite folder (según referencia del código):**
   ```powershell
   # Buscar en servidor/máquina donde se desarrolló Legacy
   Get-ChildItem -Path "C:\" -Filter "Utility_Suite" -Directory -Recurse -Depth 3
   Get-ChildItem -Path "D:\" -Filter "Utility_Suite" -Directory -Recurse -Depth 3
   ```

3. **Backup del proyecto:**
   - ¿Hay backup completo del proyecto Legacy con todas las dependencias?

**SI SE ENCUENTRA LA DLL:**

```powershell
# Decompile con ILSpy (descargar de https://github.com/icsharpcode/ILSpy/releases)
ilspy ClassLibrary_CSharp.dll /out:.\Decompiled\

# O usar dnSpy (más interactivo)
# https://github.com/dnSpy/dnSpy/releases
```

**⚠️ PREGUNTA PARA EL EQUIPO:**
- ¿Tienes acceso a la máquina donde se desarrolló el Legacy originalmente?
- ¿Hay un repositorio de control de versiones con el proyecto Utility_Suite?
- ¿Quién escribió esa clase originalmente? (Contactar al desarrollador)

#### OPCIÓN B: Implementar Nuevo Sistema de Encriptación (8 horas + migración)

**SI NO SE PUEDE RECUPERAR LA DLL ORIGINAL:**

**⚠️ PROBLEMA GRAVE:** No podemos desencriptar tarjetas existentes en DB sin conocer el algoritmo

**Pasos necesarios:**

1. **Verificar si hay tarjetas encriptadas en DB:**
   ```sql
   USE MiGenteDev;
   GO
   
   -- ¿Hay alguna tabla con números de tarjeta?
   SELECT TABLE_NAME, COLUMN_NAME
   FROM INFORMATION_SCHEMA.COLUMNS
   WHERE COLUMN_NAME LIKE '%card%' 
      OR COLUMN_NAME LIKE '%tarjeta%'
      OR COLUMN_NAME LIKE '%numero%';
   ```

2. **SI HAY TARJETAS ENCRIPTADAS:**
   - ❌ **NO SE PUEDEN MIGRAR** sin la clase Crypt original
   - Opciones:
     - Eliminar tarjetas antiguas (usuarios deben re-ingresar)
     - Contactar a quien tenga acceso al código fuente original
     - Recuperar del backup de producción

3. **SI NO HAY TARJETAS ENCRIPTADAS:**
   - ✅ Implementar nuevo `EncryptionService` con AES-256-GCM
   - Seguir estándares PCI-DSS
   - No hay problema de compatibilidad

**Implementación propuesta (si no hay tarjetas):**

```csharp
// Infrastructure/Services/EncryptionService.cs
public class EncryptionService : IEncryptionService
{
    private readonly byte[] _key;
    
    public EncryptionService(IConfiguration configuration)
    {
        // Key desde Azure Key Vault o User Secrets
        _key = Convert.FromBase64String(
            configuration["Encryption:AesKey"] ?? throw new ArgumentNullException("Encryption key not configured")
        );
    }
    
    public string Encrypt(string plainText)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = _key;
            aes.Mode = CipherMode.GCM;  // Más seguro que CBC
            aes.GenerateIV();
            
            // ... implementación completa
        }
    }
    
    public string Decrypt(string cipherText)
    {
        // ... implementación completa
    }
}
```

**Tiempo estimado:** 8 horas de desarrollo + testing

#### OPCIÓN C: Contactar al Desarrollador Original (???)

**Si el código Legacy fue desarrollado por un equipo externo o freelancer:**

1. Buscar en commits de Git (si hay)
2. Revisar documentación del proyecto
3. Contactar a quien hizo el deployment original

**⚠️ PREGUNTA PARA EL EQUIPO:**
- ¿Quién desarrolló el sistema Legacy original?
- ¿Hay contrato o SLA con soporte técnico?
- ¿Hay documentación técnica con algoritmos de encriptación?

---

## 📊 Resumen de Estado

### Bloqueadores Activos

| Bloqueador | Criticidad | Impacto | Tiempo para resolver | Opciones disponibles |
|------------|------------|---------|----------------------|---------------------|
| PaymentGateway vacía | 🔴 CRÍTICO | Sin credenciales = No payment gateway | 1 hora - 3 días | A: Buscar en Legacy<br>B: Contactar Cardnet<br>C: Usar datos fake temporales |
| Crypt.dll missing | 🔴 CRÍTICO | No desencriptar tarjetas = No pagos | 30 min - 8 horas | A: Buscar DLL y decompile<br>B: Nuevo sistema (si no hay tarjetas)<br>C: Contactar dev original |

### Dependencias entre Bloqueadores

```
BLOQUEADOR #1 (Credenciales)
    ↓
PASO 4: Configurar Testing Environment
    ↓
LOTE 1.2: Idempotency Key Generation
    ↓
BLOQUEADOR #2 (Crypt.dll)
    ↓
LOTE 1.1: EncryptionService
    ↓
LOTE 1.3: Payment Processing
    ↓
LOTE 1.4: Integración en ProcesarVenta
    ↓
LOTE 1.5: Testing Sandbox
```

**⚠️ CONCLUSIÓN:** Ambos bloqueadores deben resolverse antes de continuar con LOTE 1

---

## 🎯 Plan de Acción Inmediato

### HOY (Próximas 2-4 horas)

**Acción 1: Investigar credenciales (1-2 horas)**

- [ ] Contactar administrador del servidor Legacy
- [ ] Buscar backup de base de datos de producción
- [ ] Revisar documentación/contratos con Cardnet
- [ ] Buscar emails antiguos con setup de Cardnet

**Acción 2: Investigar Crypt.dll (1-2 horas)**

- [ ] Buscar DLL en servidor de producción
- [ ] Buscar en máquina de desarrollo original
- [ ] Contactar desarrolladores del equipo Legacy
- [ ] Verificar si hay tarjetas encriptadas en DB actual

**Acción 3: Crear issue de seguimiento**

```markdown
# GitHub Issue Template

## 🚨 BLOQUEADORES LOTE 1 - Payment Gateway

**Prioridad:** 🔴 CRÍTICA  
**Impacto:** 32 horas de desarrollo bloqueadas  

### Bloqueador #1: Credenciales Cardnet
- Tabla `PaymentGateway` vacía en base de datos
- No se pueden procesar pagos reales
- **Acción requerida:** [lista de opciones A/B/C]

### Bloqueador #2: Clase Crypt.dll
- Encriptación de tarjetas no disponible
- DLL externa faltante
- **Acción requerida:** [lista de opciones A/B/C]

### Dependencias
- LOTE 1.2-1.5 dependen de ambos bloqueadores
- Sin resolver: +3 días de retraso

### Asignado a
- @[TEAM_LEAD] - Coordinar con administradores
- @[DEV_ORIGINAL] - Contactar desarrollador Legacy (si disponible)
```

### MAÑANA (Si no se resuelve hoy)

**Plan B: Desarrollo parcial con mocks**

1. ✅ Implementar estructura de CardnetPaymentService (sin lógica real)
2. ✅ Crear interfaces y DTOs
3. ✅ Unit tests con mocks
4. ✅ Documentar lo que falta para completar
5. ⏸️ PAUSAR LOTE 1 y continuar con LOTE 2 (User Management)

**Ventaja:** No bloquear todo el desarrollo  
**Riesgo:** Payment gateway queda incompleto más tiempo

---

## 📞 Contactos Clave

**Para resolver bloqueadores, contactar:**

- [ ] **Administrador de Sistemas Legacy:** [NOMBRE/EMAIL]
- [ ] **DBA - Base de Datos:** [NOMBRE/EMAIL]
- [ ] **Desarrollador Lead Legacy:** [NOMBRE/EMAIL]
- [ ] **Contacto comercial Cardnet:** [buscar en website]
- [ ] **Soporte técnico Cardnet:** [buscar en website]

---

## ✅ Checklist de Desbloqueo

**BLOQUEADOR #1 - Credenciales:**

- [ ] Credenciales encontradas en servidor Legacy
- [ ] O credenciales obtenidas de Cardnet
- [ ] O credenciales fake insertadas para desarrollo local
- [ ] Tabla `PaymentGateway` poblada con datos
- [ ] Conectividad a Cardnet sandbox confirmada

**BLOQUEADOR #2 - Crypt.dll:**

- [ ] DLL encontrada y decompilada
- [ ] O nuevo EncryptionService implementado
- [ ] O confirmado que no hay tarjetas encriptadas existentes
- [ ] Testing de encrypt/decrypt funcional

**Una vez marcados todos los items, podemos reanudar LOTE 1.**

---

**Última actualización:** 2025-10-24 20:10 UTC-4  
**Estado:** ⛔ BLOQUEADO - Esperando resolución de 2 bloqueadores críticos  
**Impacto:** LOTE 1 (32 horas) completamente bloqueado  
**Acción inmediata:** Ejecutar "Plan de Acción Inmediato" (2-4 horas)
