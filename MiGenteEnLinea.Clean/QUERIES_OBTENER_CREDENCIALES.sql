-- ============================================================================
-- QUERIES PARA OBTENER CREDENCIALES DESDE BASE DE DATOS
-- Ejecutar estas queries en SQL Server Management Studio o Azure Data Studio
-- Conectar a: Server=mda-308, Database=MiGenteDev (o migenteV2 según Legacy)
-- ============================================================================

-- ============================================================================
-- 1. EMAIL SMTP CONFIGURATION (Config_Correo)
-- ============================================================================
SELECT TOP 1 
    id,
    Email,
    Usuario AS SmtpUsername,
    Clave AS SmtpPassword,
    Servidor AS SmtpServer,
    Puerto AS SmtpPort,
    SSL AS EnableSsl,
    FechaCreacion
FROM Config_Correo
ORDER BY id DESC;

-- Después de obtener el resultado, ejecutar en PowerShell:
-- cd "c:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea.Clean\src\Presentation\MiGenteEnLinea.API"
-- dotnet user-secrets set "EmailSettings:Password" "PEGAR_AQUI_CLAVE_OBTENIDA"

-- ============================================================================
-- 2. OPENAI API CONFIGURATION (OpenAi_Config)
-- ============================================================================
SELECT TOP 1 
    id,
    ApiKey,
    Model,
    MaxTokens,
    Temperature,
    SystemPrompt,
    FechaCreacion,
    Activo
FROM OpenAi_Config
WHERE Activo = 1
ORDER BY id DESC;

-- Después de obtener el resultado, ejecutar en PowerShell:
-- dotnet user-secrets set "OpenAI:ApiKey" "PEGAR_AQUI_API_KEY_OBTENIDA"

-- ============================================================================
-- 3. CARDNET PAYMENT GATEWAY (PaymentGateway)
-- ============================================================================
SELECT TOP 1 
    id,
    merchantID AS MerchantId,
    terminalID AS TerminalId,
    apiKey AS ApiKey,
    test AS IsTest,
    productionURL AS ProductionUrl,
    testURL AS TestUrl,
    FechaCreacion,
    Activo
FROM PaymentGateway
WHERE Activo = 1
ORDER BY id DESC;

-- Después de obtener el resultado, ejecutar en PowerShell:
-- dotnet user-secrets set "Cardnet:MerchantId" "PEGAR_AQUI_MERCHANT_ID"
-- dotnet user-secrets set "Cardnet:TerminalId" "PEGAR_AQUI_TERMINAL_ID"
-- dotnet user-secrets set "Cardnet:ApiKey" "PEGAR_AQUI_API_KEY"

-- ============================================================================
-- 4. VERIFICAR TODAS LAS TABLAS DE CONFIGURACIÓN
-- ============================================================================
-- Verificar que las tablas existen
SELECT 
    TABLE_NAME,
    TABLE_TYPE
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME IN ('Config_Correo', 'OpenAi_Config', 'PaymentGateway')
ORDER BY TABLE_NAME;

-- ============================================================================
-- 5. OBTENER ESTRUCTURA DE TABLAS (si las queries anteriores fallan)
-- ============================================================================

-- Config_Correo structure
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Config_Correo'
ORDER BY ORDINAL_POSITION;

-- OpenAi_Config structure
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'OpenAi_Config'
ORDER BY ORDINAL_POSITION;

-- PaymentGateway structure
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'PaymentGateway'
ORDER BY ORDINAL_POSITION;

-- ============================================================================
-- NOTAS IMPORTANTES
-- ============================================================================
-- 1. Si las tablas no existen en MiGenteDev, intentar con la base de datos migenteV2
-- 2. Los nombres de columnas pueden variar (PascalCase vs snake_case)
-- 3. Algunas configuraciones podrían estar encriptadas en la base de datos
-- 4. NUNCA commitear estas credenciales a Git - solo usar User Secrets
-- ============================================================================
