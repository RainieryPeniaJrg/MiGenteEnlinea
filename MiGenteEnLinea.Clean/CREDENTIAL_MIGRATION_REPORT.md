# 🔐 Credential Migration Report - Legacy to Clean Architecture

**Generated**: 2025-10-XX  
**Purpose**: Compare ALL credentials from Legacy Web Forms project to Clean Architecture project  
**Security Status**: ⚠️ CRITICAL - Some credentials hardcoded in source control

---

## 📊 Comparison Summary

| Credential Type | Legacy Source | Clean Source | Status | Action Needed |
|----------------|---------------|--------------|--------|---------------|
| **SQL Server** | Web.config | appsettings.json | ⚠️ **DIFFERENT** | Update server/DB name |
| **Cardnet Payment** | Web.config | appsettings.json | ⚠️ **INCOMPLETE** | Migrate actual API key |
| **Email (SMTP)** | Database (Config_Correo) | appsettings.json | ⚠️ **INCOMPLETE** | Migrate password to User Secrets |
| **OpenAI API** | Database (OpenAi_Config) | ❌ **MISSING** | 🔴 **NOT CONFIGURED** | Add OpenAI configuration |
| **Padrón API** | ❌ **NOT FOUND** | appsettings.json | ✅ **NEW** | Verify if Legacy uses it |
| **JWT Secret** | N/A (Legacy uses Forms Auth) | appsettings.json | ⚠️ **PLACEHOLDER** | Generate production key |

---

## 🔷 Legacy Project Credentials (Web Forms)

### 1. SQL Server Connection (Web.config)
```xml
<connectionStrings>
  <add name="migenteEntities" 
       connectionString="metadata=res://*/Data.DataModel.csdl|res://*/Data.DataModel.ssdl|res://*/Data.DataModel.msl;
       provider=System.Data.SqlClient;
       provider connection string=&quot;
         data source=.;
         initial catalog=migenteV2;
         user id=sa;
         password=1234;
         multipleactiveresultsets=True;
         App=EntityFramework&quot;" 
       providerName="System.Data.EntityClient"/>
</connectionStrings>
```

**Analysis**:
- Server: `.` (localhost)
- Database: `migenteV2`
- User: `sa`
- Password: `1234` (⚠️ Hardcoded, weak password)

---

### 2. Cardnet Payment Gateway (Web.config)
```xml
<appSettings>
  <add key="CardnetMerchantId" value="349000001"/>
  <add key="CardnetApiKey" value="TU_API_KEY"/>
  <add key="CardnetApiUrlSales" value="https://ecommerce.cardnet.com.do/api/payment/transactions/sales"/>
  <add key="CardnetApiUrlIdempotency" value="https://ecommerce.cardnet.com.do/api/payment/idenpotency-keys"/>
</appSettings>
```

**Analysis**:
- MerchantId: `349000001` ✅ (actual production value)
- ApiKey: `TU_API_KEY` ⚠️ (placeholder - actual key NOT in source control, likely in production Web.config)
- Production URL: `ecommerce.cardnet.com.do` (different from Clean's lab.cardnet.com.do)

**🔴 CRITICAL**: Legacy uses **PRODUCTION** Cardnet URL, Clean uses **TEST/LAB** URL!

---

### 3. Email SMTP Configuration (Database)
**Table**: `Config_Correo`  
**Service**: `Services/EmailService.cs` → `db.Config_Correo.FirstOrDefault()`

**Analysis**:
- Email credentials stored in database, NOT in Web.config
- Clean project has hardcoded SMTP in appsettings.json (`mail.intdosystem.com`, `develop@intdosystem.com`)
- **Need to verify if Clean's hardcoded values match Legacy's database values**

**TODO**: Query `Config_Correo` table to extract actual SMTP settings

---

### 4. OpenAI API (Database)
**Table**: `OpenAi_Config`  
**Service**: `Services/BotServices.cs` → `db.OpenAi_Config.FirstOrDefault()`

**Analysis**:
- OpenAI API key stored in database, NOT in Web.config
- Clean project has **NO OpenAI configuration** in appsettings.json
- Used for "abogado virtual" (virtual legal assistant) feature

**🔴 CRITICAL**: OpenAI integration completely missing from Clean Architecture!

**TODO**: 
1. Query `OpenAi_Config` table to extract API key
2. Add OpenAI configuration section to appsettings.json
3. Implement OpenAI service in Infrastructure layer (if needed)

---

### 5. Authentication
**Legacy**: ASP.NET Forms Authentication (cookies)
```xml
<authentication mode="Forms">
  <forms loginUrl="~/login.aspx" name="sqlAuthCookie" timeout="30"></forms>
</authentication>
```

**Clean**: JWT tokens (stateless)
```json
"Jwt": {
  "SecretKey": "TU_SECRET_KEY_DE_AL_MENOS_32_CARACTERES_AQUI_CAMBIAR_EN_PRODUCCION",
  "Issuer": "MiGenteEnLinea.API",
  "Audience": "MiGenteEnLinea.Client"
}
```

**Analysis**: No migration needed (different auth strategies by design)

---

## 🚀 Clean Architecture Credentials (ASP.NET Core)

### 1. SQL Server Connection (appsettings.json)
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=mda-308;Database=MiGenteDev;User Id=sa;Password=Volumen#1;TrustServerCertificate=True;MultipleActiveResultSets=true",
  "PCDEXTRA": "Server=mda-308;Database=MiGenteDev;User Id=sa;Password=Volumen#1;...",
  "PERSONALPC": "Server=localhost,1433;Database=MiGenteDev;User Id=sa;Password=Volumen#1;..."
}
```

**Analysis**:
- Server: `mda-308` (different from Legacy's localhost)
- Database: `MiGenteDev` (different from Legacy's `migenteV2`)
- Password: `Volumen#1` (different from Legacy's `1234`)

**⚠️ ISSUE**: Clean Architecture points to **DIFFERENT database** than Legacy!

**Questions**:
1. Is `MiGenteDev` a separate dev database?
2. Should Clean use the same database as Legacy during migration?
3. Are the schemas compatible?

---

### 2. Cardnet Payment Gateway (appsettings.json)
```json
"Cardnet": {
  "ProductionUrl": "https://lab.cardnet.com.do/api/payment/transactions/",
  "TestUrl": "https://lab.cardnet.com.do/api/payment/transactions/",
  "MerchantId": "PONER_EN_USER_SECRETS",
  "TerminalId": "PONER_EN_USER_SECRETS",
  "IsTest": true
}
```

**Analysis**:
- MerchantId: `PONER_EN_USER_SECRETS` ❌ (placeholder, should be `349000001` from Legacy)
- TerminalId: `PONER_EN_USER_SECRETS` ❌ (not found in Legacy, need to ask)
- URL: `lab.cardnet.com.do` ⚠️ (TEST environment, Legacy uses `ecommerce.cardnet.com.do` PRODUCTION)

**🔴 CRITICAL MISMATCH**: 
- Legacy: Production Cardnet URL
- Clean: Test/Lab Cardnet URL

**TODO**: 
1. Update MerchantId to `349000001` (from Legacy)
2. Find actual TerminalId (not in Legacy Web.config, may be in code or production server)
3. Update URLs to match Legacy production settings

---

### 3. Email SMTP Configuration (appsettings.json)
```json
"EmailSettings": {
  "SmtpServer": "mail.intdosystem.com",
  "SmtpPort": 465,
  "Username": "develop@intdosystem.com",
  "Password": "PONER_EN_USER_SECRETS",
  "FromEmail": "develop@intdosystem.com",
  "FromName": "MiGente En Línea",
  "EnableSsl": true
}
```

**Analysis**:
- SMTP Server: `mail.intdosystem.com` ✅ (seems correct)
- Username: `develop@intdosystem.com` ✅ (seems correct)
- Password: `PONER_EN_USER_SECRETS` ❌ (placeholder)

**TODO**: 
1. Query `Config_Correo` table from Legacy database to get actual password
2. Move password to User Secrets (NOT appsettings.json)

---

### 4. Padrón API (appsettings.json)
```json
"PadronAPI": {
  "BaseUrl": "https://abcportal.online/Sigeinfo/public/api/",
  "Username": "131345042",
  "Password": "1313450422022@*SRL"
}
```

**Analysis**:
- ⚠️ **HARDCODED PASSWORD IN SOURCE CONTROL** (security violation!)
- Not found in Legacy Web.config (may be a new integration)
- Likely Dominican national registry API

**🔴 SECURITY VIOLATION**: Password exposed in Git repository!

**TODO**: 
1. Move password to User Secrets immediately
2. Verify if Legacy project uses this API (search for "padron" or "131345042" in Legacy code)
3. Rotate password if compromised

---

### 5. OpenAI API (appsettings.json)
```json
❌ NOT CONFIGURED
```

**Analysis**:
- Legacy has OpenAI integration (stored in database)
- Clean Architecture has **NO OpenAI configuration**
- Feature: "abogado virtual" (virtual legal assistant)

**TODO**: 
1. Query `OpenAi_Config` table from database
2. Add OpenAI section to appsettings.json:
```json
"OpenAI": {
  "ApiKey": "MOVE_TO_USER_SECRETS",
  "Model": "gpt-4",
  "MaxTokens": 2000,
  "Temperature": 0.7
}
```
3. Implement OpenAI service in Infrastructure layer

---

## 🔴 CRITICAL ACTION ITEMS

### Priority 1: Security Fixes (Immediate)
1. **Move PadronAPI password to User Secrets** (currently exposed in Git)
2. **Verify SQL Server connection** (Clean points to different database than Legacy)
3. **Fix Cardnet URL mismatch** (Clean uses TEST, Legacy uses PRODUCTION)

### Priority 2: Missing Credentials (High)
4. **Add OpenAI configuration** (completely missing from Clean)
5. **Migrate Cardnet MerchantId** (`349000001` from Legacy to Clean)
6. **Get Cardnet TerminalId** (not in Legacy Web.config, need to find)
7. **Get Email SMTP password** (query `Config_Correo` table)

### Priority 3: Validation (Medium)
8. **Verify database schemas match** (Legacy: `migenteV2`, Clean: `MiGenteDev`)
9. **Test Padrón API** (verify credentials work)
10. **Generate production JWT SecretKey** (currently placeholder)

---

## 📝 SQL Queries to Run (Get Database-Stored Credentials)

### 1. Get Email SMTP Configuration
```sql
-- Connect to Legacy database (migenteV2 or MiGenteDev)
SELECT TOP 1 
    Email,
    Usuario AS SmtpUsername,
    Clave AS SmtpPassword,
    Servidor AS SmtpServer,
    Puerto AS SmtpPort,
    SSL AS EnableSsl
FROM Config_Correo
ORDER BY id DESC;
```

### 2. Get OpenAI API Configuration
```sql
-- Connect to Legacy database
SELECT TOP 1 
    ApiKey,
    Model,
    MaxTokens,
    Temperature,
    SystemPrompt
FROM OpenAi_Config
ORDER BY id DESC;
```

---

## 🛠️ Implementation Steps

### Step 1: Secure Sensitive Data (15 minutes)
```powershell
# Navigate to API project
cd "c:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea.Clean\src\Presentation\MiGenteEnLinea.API"

# Initialize User Secrets
dotnet user-secrets init

# Move PadronAPI password (CRITICAL - exposed in Git)
dotnet user-secrets set "PadronAPI:Password" "1313450422022@*SRL"

# Add placeholders for other secrets
dotnet user-secrets set "EmailSettings:Password" "REPLACE_WITH_ACTUAL_FROM_DB"
dotnet user-secrets set "Cardnet:MerchantId" "349000001"
dotnet user-secrets set "Cardnet:TerminalId" "REPLACE_WITH_ACTUAL"
dotnet user-secrets set "Cardnet:ApiKey" "REPLACE_WITH_ACTUAL"
dotnet user-secrets set "OpenAI:ApiKey" "REPLACE_WITH_ACTUAL_FROM_DB"
dotnet user-secrets set "Jwt:SecretKey" "GENERATE_STRONG_32_CHAR_KEY_FOR_PRODUCTION"
```

### Step 2: Update appsettings.json (10 minutes)
Remove hardcoded passwords, replace with User Secrets references:
```json
"PadronAPI": {
  "BaseUrl": "https://abcportal.online/Sigeinfo/public/api/",
  "Username": "131345042",
  "Password": ""  // REMOVED - Now in User Secrets
}
```

### Step 3: Add OpenAI Configuration (5 minutes)
Add new section to appsettings.json:
```json
"OpenAI": {
  "ApiKey": "",  // User Secrets
  "BaseUrl": "https://api.openai.com/v1/",
  "Model": "gpt-4",
  "MaxTokens": 2000,
  "Temperature": 0.7,
  "SystemPrompt": "Eres un asistente legal virtual para MiGente En Línea..."
}
```

### Step 4: Fix Cardnet Configuration (5 minutes)
Update Cardnet URLs to match Legacy production:
```json
"Cardnet": {
  "ProductionUrl": "https://ecommerce.cardnet.com.do/api/payment/transactions/sales",
  "TestUrl": "https://lab.cardnet.com.do/api/payment/transactions/",
  "IdempotencyUrl": "https://ecommerce.cardnet.com.do/api/payment/idenpotency-keys",
  "MerchantId": "",  // User Secrets
  "TerminalId": "",  // User Secrets
  "ApiKey": "",      // User Secrets
  "IsTest": false    // Production mode
}
```

### Step 5: Query Database for Credentials (15 minutes)
Run SQL queries above to populate User Secrets with actual values.

---

## ✅ Validation Checklist

After migration, verify:
- [ ] PadronAPI password NOT in appsettings.json (moved to User Secrets)
- [ ] Email SMTP password populated from `Config_Correo` table
- [ ] OpenAI API key populated from `OpenAi_Config` table
- [ ] Cardnet MerchantId matches Legacy (`349000001`)
- [ ] Cardnet URLs match Legacy production
- [ ] SQL Server connection points to correct database
- [ ] All User Secrets set: `dotnet user-secrets list`
- [ ] API starts without configuration errors
- [ ] Payment gateway test succeeds
- [ ] Email sending test succeeds
- [ ] OpenAI virtual assistant test succeeds

---

## 📄 Files Modified

1. `appsettings.json` - Remove hardcoded passwords, add OpenAI section
2. `secrets.json` (via `dotnet user-secrets`) - Store all sensitive credentials
3. `Program.cs` - Verify User Secrets loaded in Development mode ✅ (already configured)

---

## 🔐 Security Recommendations

1. **Rotate PadronAPI password** (exposed in Git history)
2. **Add appsettings.Production.json** to .gitignore
3. **Use Azure Key Vault** for production deployment
4. **Enable auditing** for credential access
5. **Document credential rotation process**

---

**Generated by**: GitHub Copilot Agent  
**Last Updated**: 2025-10-XX  
**Status**: 🔴 PENDING IMPLEMENTATION
