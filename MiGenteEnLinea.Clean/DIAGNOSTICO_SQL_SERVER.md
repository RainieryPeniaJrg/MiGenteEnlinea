# 🚨 Diagnóstico: Error de Autenticación SQL Server

## Error Actual
```
Microsoft.Data.SqlClient.SqlException (0x80131904): Login failed for user 'sa'.
Error Number: 18456, State: 1, Class: 14
```

## ⚠️ Causa Probable
El error **18456 State 1** típicamente indica uno de estos problemas:

1. **SQL Server NO está configurado en Modo de Autenticación Mixta** (solo Windows Auth)
2. La contraseña de 'sa' es incorrecta
3. El usuario 'sa' está deshabilitado
4. SQL Server no está escuchando en `localhost:1433`
5. SQL Server no está ejecutándose

## 🔧 Solución Paso a Paso

### PASO 1: Verificar si SQL Server está ejecutándose
```powershell
# Ejecuta este comando en PowerShell:
Get-Service -Name "MSSQL*" | Select-Object Name, Status, DisplayName
```

**Resultado esperado:** Debe mostrar servicio `MSSQLSERVER` con Status = `Running`

---

### PASO 2: Habilitar Autenticación Mixta (Mixed Mode)

#### Opción A: Usando SQL Server Management Studio (RECOMENDADO)
1. Abre **SQL Server Management Studio (SSMS)**
2. Conéctate usando **Windows Authentication**
3. Click derecho en el servidor → **Properties**
4. Ve a **Security** en el panel izquierdo
5. En "Server authentication" selecciona: **"SQL Server and Windows Authentication mode"**
6. Click **OK**
7. **REINICIA el servicio de SQL Server** (CRÍTICO)

#### Opción B: Usando T-SQL
```sql
-- Conecta con Windows Authentication y ejecuta:
USE [master]
GO
EXEC xp_instance_regwrite 
    N'HKEY_LOCAL_MACHINE', 
    N'Software\Microsoft\MSSQLServer\MSSQLServer',
    N'LoginMode', 
    REG_DWORD, 
    2
GO
```
**Luego REINICIA el servicio:**
```powershell
Restart-Service -Name MSSQLSERVER -Force
```

---

### PASO 3: Habilitar el usuario 'sa'

```sql
-- Conecta con Windows Authentication y ejecuta:
USE [master]
GO

-- Habilitar el login 'sa'
ALTER LOGIN [sa] ENABLE;
GO

-- Cambiar/establecer la contraseña
ALTER LOGIN [sa] WITH PASSWORD = N'Volumen#1';
GO

-- Verificar que el login existe y está habilitado
SELECT name, is_disabled, create_date, modify_date
FROM sys.sql_logins
WHERE name = 'sa';
GO
```

**Resultado esperado:** `is_disabled` debe ser `0` (habilitado)

---

### PASO 4: Verificar puerto y conexión TCP/IP

#### Habilitar TCP/IP:
1. Abre **SQL Server Configuration Manager**
2. Expande **SQL Server Network Configuration**
3. Click en **Protocols for MSSQLSERVER**
4. Click derecho en **TCP/IP** → **Enable**
5. Click derecho en **TCP/IP** → **Properties**
6. Ve a la pestaña **IP Addresses**
7. En **IPAll**, establece **TCP Port** a `1433`
8. **REINICIA el servicio de SQL Server**

#### Verificar el puerto:
```powershell
# Verifica que SQL Server esté escuchando en puerto 1433:
netstat -ano | findstr :1433
```

---

### PASO 5: Probar la conexión

#### Opción A: Usando SQL Server Management Studio
1. Abre SSMS
2. Server name: `localhost,1433`
3. Authentication: **SQL Server Authentication**
4. Login: `sa`
5. Password: `Volumen#1`
6. Click **Connect**

#### Opción B: Usando sqlcmd desde PowerShell
```powershell
sqlcmd -S localhost,1433 -U sa -P "Volumen#1" -Q "SELECT @@VERSION"
```

**Resultado esperado:** Debe mostrar la versión de SQL Server sin errores

---

## 🔄 Solución Alternativa (Mientras resuelves SQL Auth)

### Opción Temporal: Deshabilitar SQL Server Logging

Si necesitas que la API funcione YA mientras arreglas SQL Server, podemos deshabilitar temporalmente el logging a SQL Server:

**Modificar `Program.cs` línea 7-13:**
```csharp
// Comentar temporalmente el sink de SQL Server
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    // TODO: Descomentar cuando SQL Server esté configurado correctamente
    //.WriteTo.MSSqlServer(
    //    connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
    //    sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions
    //    {
    //        TableName = "Logs",
    //        AutoCreateSqlTable = true
    //    })
    .Enrich.FromLogContext()
    .CreateLogger();
```

### Opción Recomendada: Usar Windows Authentication

**Modificar el Connection String en `appsettings.json`:**
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=MiGenteDev;Integrated Security=true;TrustServerCertificate=True;MultipleActiveResultSets=true;Encrypt=false"
}
```

**Ventajas:**
- ✅ Más seguro (no contraseñas en texto)
- ✅ No requiere habilitar SQL Auth
- ✅ Funciona inmediatamente si tienes permisos en Windows

---

## 📋 Checklist de Verificación

Marca cada paso cuando lo completes:

- [ ] **SQL Server está ejecutándose** (Get-Service MSSQLSERVER)
- [ ] **Autenticación Mixta habilitada** (Server Properties → Security)
- [ ] **Servicio reiniciado después de cambiar a Mixed Mode**
- [ ] **Usuario 'sa' habilitado** (ALTER LOGIN [sa] ENABLE)
- [ ] **Contraseña 'sa' establecida** (ALTER LOGIN [sa] WITH PASSWORD)
- [ ] **TCP/IP habilitado** (SQL Server Configuration Manager)
- [ ] **Puerto 1433 configurado** (TCP/IP Properties → IPAll)
- [ ] **Servicio reiniciado después de habilitar TCP/IP**
- [ ] **Conexión probada con SSMS o sqlcmd**

---

## 🎯 Siguiente Paso Recomendado

**Opción 1 (RÁPIDA):** Usar Windows Authentication
- Cambiar Connection String a `Integrated Security=true`
- Ejecutar `dotnet run`
- Verificar que API inicia sin errores

**Opción 2 (COMPLETA):** Configurar SQL Authentication
- Seguir PASO 1-5 arriba
- Probar conexión con SSMS
- Una vez funcionando, ejecutar `dotnet run`

---

## 📞 ¿Qué opción prefieres?

1. **Usar Windows Authentication** (cambio rápido en connection string)
2. **Configurar SQL Server para SQL Authentication** (seguir guía paso a paso)
3. **Deshabilitar SQL logging temporalmente** (solo Console y File)

**Responde con el número de tu opción preferida (1, 2 o 3).**
