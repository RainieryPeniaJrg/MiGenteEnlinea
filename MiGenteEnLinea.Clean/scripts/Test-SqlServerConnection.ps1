# Test-SqlServerConnection.ps1
# Script para diagnosticar problemas de conexión con SQL Server

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "🔍 DIAGNÓSTICO SQL SERVER" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

# PASO 1: Verificar servicios
Write-Host "📌 PASO 1: Verificando servicios de SQL Server..." -ForegroundColor Yellow
$services = Get-Service -Name "MSSQL*" -ErrorAction SilentlyContinue
if ($services) {
    $services | Select-Object Name, Status, DisplayName | Format-Table
    $sqlServerService = $services | Where-Object { $_.Name -eq "MSSQLSERVER" }
    if ($sqlServerService -and $sqlServerService.Status -eq "Running") {
        Write-Host "✅ SQL Server está ejecutándose`n" -ForegroundColor Green
    } else {
        Write-Host "❌ SQL Server NO está ejecutándose" -ForegroundColor Red
        Write-Host "   Ejecuta: Start-Service MSSQLSERVER`n" -ForegroundColor Yellow
    }
} else {
    Write-Host "❌ No se encontró SQL Server instalado`n" -ForegroundColor Red
}

# PASO 2: Verificar puerto 1433
Write-Host "📌 PASO 2: Verificando puerto 1433..." -ForegroundColor Yellow
$listening = netstat -ano | Select-String ":1433"
if ($listening) {
    Write-Host "✅ SQL Server está escuchando en puerto 1433:" -ForegroundColor Green
    $listening | ForEach-Object { Write-Host "   $_" -ForegroundColor Gray }
    Write-Host ""
} else {
    Write-Host "❌ SQL Server NO está escuchando en puerto 1433" -ForegroundColor Red
    Write-Host "   Verifica TCP/IP en SQL Server Configuration Manager`n" -ForegroundColor Yellow
}

# PASO 3: Probar conexión con Windows Authentication
Write-Host "📌 PASO 3: Probando conexión con Windows Authentication..." -ForegroundColor Yellow
$connectionString = "Server=localhost,1433;Database=master;Integrated Security=true;TrustServerCertificate=True;Encrypt=false"
try {
    $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
    $connection.Open()
    $command = $connection.CreateCommand()
    $command.CommandText = "SELECT @@VERSION"
    $version = $command.ExecuteScalar()
    Write-Host "✅ Conexión exitosa con Windows Authentication" -ForegroundColor Green
    Write-Host "   Versión: $($version.ToString().Substring(0, 100))..." -ForegroundColor Gray
    $connection.Close()
    Write-Host ""
} catch {
    Write-Host "❌ Fallo la conexión con Windows Authentication" -ForegroundColor Red
    Write-Host "   Error: $($_.Exception.Message)`n" -ForegroundColor Red
}

# PASO 4: Verificar si existe la base de datos MiGenteDev
Write-Host "📌 PASO 4: Verificando base de datos MiGenteDev..." -ForegroundColor Yellow
try {
    $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
    $connection.Open()
    $command = $connection.CreateCommand()
    $command.CommandText = "SELECT name FROM sys.databases WHERE name = 'MiGenteDev'"
    $dbName = $command.ExecuteScalar()
    if ($dbName) {
        Write-Host "✅ Base de datos 'MiGenteDev' existe" -ForegroundColor Green
    } else {
        Write-Host "⚠️  Base de datos 'MiGenteDev' NO existe" -ForegroundColor Yellow
        Write-Host "   Puede que necesites crearla o usar 'db_a9f8ff_migente'" -ForegroundColor Yellow
    }
    $connection.Close()
    Write-Host ""
} catch {
    Write-Host "⚠️  No se pudo verificar la base de datos" -ForegroundColor Yellow
    Write-Host "   Error: $($_.Exception.Message)`n" -ForegroundColor Red
}

# PASO 5: Verificar modo de autenticación
Write-Host "📌 PASO 5: Verificando modo de autenticación..." -ForegroundColor Yellow
try {
    $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
    $connection.Open()
    $command = $connection.CreateCommand()
    $command.CommandText = @"
SELECT CASE SERVERPROPERTY('IsIntegratedSecurityOnly')
    WHEN 1 THEN 'Windows Authentication Only'
    WHEN 0 THEN 'SQL Server and Windows Authentication (Mixed Mode)'
END AS [Authentication Mode]
"@
    $authMode = $command.ExecuteScalar()
    Write-Host "   Modo actual: $authMode" -ForegroundColor Gray
    
    if ($authMode -like "*Mixed Mode*") {
        Write-Host "✅ Modo Mixto habilitado (SQL + Windows Auth)" -ForegroundColor Green
    } else {
        Write-Host "⚠️  Solo Windows Authentication está habilitado" -ForegroundColor Yellow
        Write-Host "   Para usar 'sa', debes habilitar Mixed Mode" -ForegroundColor Yellow
    }
    $connection.Close()
    Write-Host ""
} catch {
    Write-Host "⚠️  No se pudo verificar el modo de autenticación" -ForegroundColor Yellow
    Write-Host "   Error: $($_.Exception.Message)`n" -ForegroundColor Red
}

# PASO 6: Verificar estado del usuario 'sa'
Write-Host "📌 PASO 6: Verificando usuario 'sa'..." -ForegroundColor Yellow
try {
    $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
    $connection.Open()
    $command = $connection.CreateCommand()
    $command.CommandText = @"
SELECT name, is_disabled, 
       CASE WHEN is_disabled = 0 THEN 'Habilitado' ELSE 'Deshabilitado' END AS Estado
FROM sys.sql_logins
WHERE name = 'sa'
"@
    $reader = $command.ExecuteReader()
    if ($reader.Read()) {
        $isDisabled = $reader["is_disabled"]
        $estado = $reader["Estado"]
        Write-Host "   Usuario 'sa': $estado" -ForegroundColor Gray
        
        if ($isDisabled -eq $false) {
            Write-Host "✅ Usuario 'sa' está habilitado" -ForegroundColor Green
        } else {
            Write-Host "❌ Usuario 'sa' está DESHABILITADO" -ForegroundColor Red
            Write-Host "   Ejecuta: ALTER LOGIN [sa] ENABLE;" -ForegroundColor Yellow
        }
    } else {
        Write-Host "❌ No se encontró el usuario 'sa'" -ForegroundColor Red
    }
    $reader.Close()
    $connection.Close()
    Write-Host ""
} catch {
    Write-Host "⚠️  No se pudo verificar el usuario 'sa'" -ForegroundColor Yellow
    Write-Host "   Error: $($_.Exception.Message)`n" -ForegroundColor Red
}

# PASO 7: Probar conexión con SQL Authentication
Write-Host "📌 PASO 7: Probando conexión con SQL Authentication (sa)..." -ForegroundColor Yellow
$sqlAuthConnectionString = "Server=localhost,1433;Database=master;User Id=sa;Password=Volumen#1;TrustServerCertificate=True;Encrypt=false"
try {
    $connection = New-Object System.Data.SqlClient.SqlConnection($sqlAuthConnectionString)
    $connection.Open()
    Write-Host "✅ ¡Conexión exitosa con SQL Authentication (sa/Volumen#1)!" -ForegroundColor Green
    Write-Host "   Tu connection string está CORRECTO" -ForegroundColor Green
    $connection.Close()
    Write-Host ""
} catch {
    Write-Host "❌ Fallo la conexión con SQL Authentication" -ForegroundColor Red
    Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "   Esto confirma el problema de autenticación`n" -ForegroundColor Yellow
}

# RESUMEN Y RECOMENDACIONES
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "📋 RESUMEN Y RECOMENDACIONES" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

Write-Host "Opciones para resolver el problema:`n" -ForegroundColor White

Write-Host "OPCIÓN 1 (RECOMENDADA - MÁS RÁPIDA):" -ForegroundColor Green
Write-Host "Usar Windows Authentication en el connection string" -ForegroundColor White
Write-Host "Cambia appsettings.json a:" -ForegroundColor Gray
Write-Host '  "Server=localhost,1433;Database=MiGenteDev;Integrated Security=true;TrustServerCertificate=True;Encrypt=false"' -ForegroundColor Yellow
Write-Host ""

Write-Host "OPCIÓN 2 (SI NECESITAS SQL AUTH):" -ForegroundColor Yellow
Write-Host "Habilitar Mixed Mode y configurar 'sa'" -ForegroundColor White
Write-Host "1. Abre SQL Server Management Studio" -ForegroundColor Gray
Write-Host "2. Click derecho en servidor → Properties → Security" -ForegroundColor Gray
Write-Host "3. Selecciona 'SQL Server and Windows Authentication mode'" -ForegroundColor Gray
Write-Host "4. Ejecuta: ALTER LOGIN [sa] ENABLE; ALTER LOGIN [sa] WITH PASSWORD = 'Volumen#1';" -ForegroundColor Gray
Write-Host "5. Reinicia el servicio: Restart-Service MSSQLSERVER" -ForegroundColor Gray
Write-Host ""

Write-Host "OPCIÓN 3 (TEMPORAL):" -ForegroundColor Cyan
Write-Host "Deshabilitar SQL Server logging en Program.cs" -ForegroundColor White
Write-Host "Comenta la sección .WriteTo.MSSqlServer() temporalmente" -ForegroundColor Gray
Write-Host ""

Write-Host "========================================`n" -ForegroundColor Cyan

# Pregunta al usuario qué desea hacer
Write-Host "¿Deseas aplicar alguna solución automáticamente?" -ForegroundColor White
Write-Host "1 - Modificar appsettings.json para usar Windows Authentication" -ForegroundColor Gray
Write-Host "2 - Abrir guía completa de diagnóstico (DIAGNOSTICO_SQL_SERVER.md)" -ForegroundColor Gray
Write-Host "3 - No hacer nada (revisar manualmente)" -ForegroundColor Gray
Write-Host ""
$choice = Read-Host "Selecciona una opción (1-3)"

switch ($choice) {
    "1" {
        Write-Host "`n✅ Modificando appsettings.json..." -ForegroundColor Green
        # Aquí se podría implementar la modificación automática del archivo
        Write-Host "⚠️  Implementación pendiente. Por favor, modifica manualmente el archivo." -ForegroundColor Yellow
    }
    "2" {
        $diagnosticFile = Join-Path (Get-Location) "DIAGNOSTICO_SQL_SERVER.md"
        if (Test-Path $diagnosticFile) {
            Write-Host "`n📖 Abriendo guía de diagnóstico..." -ForegroundColor Green
            Start-Process $diagnosticFile
        } else {
            Write-Host "`n❌ No se encontró el archivo DIAGNOSTICO_SQL_SERVER.md" -ForegroundColor Red
        }
    }
    "3" {
        Write-Host "`nℹ️  Revisa los resultados arriba y aplica las soluciones manualmente." -ForegroundColor Cyan
    }
    default {
        Write-Host "`n⚠️  Opción no válida." -ForegroundColor Yellow
    }
}

Write-Host "`n🏁 Diagnóstico completado." -ForegroundColor Cyan
