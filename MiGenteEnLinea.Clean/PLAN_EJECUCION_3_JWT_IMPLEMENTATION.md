# 🔐 PLAN DE EJECUCIÓN 3: JWT AUTHENTICATION IMPLEMENTATION

**Prioridad:** 🟡 **MEDIA** (Mejora de seguridad)  
**Esfuerzo Estimado:** 1-2 días (8-16 horas)  
**Estado:** ⏳ PENDIENTE  
**Dependencias:** Ninguna (puede iniciarse cuando EmailService esté completo)

---

## 🎯 OBJETIVO

Implementar autenticación JWT (JSON Web Tokens) con **refresh tokens** para reemplazar el sistema actual de autenticación basado en claims simples. Esto mejora la seguridad, permite revocación de sesiones, y habilita autenticación stateless.

---

## 📊 ANÁLISIS DEL ESTADO ACTUAL

### ✅ Lo que YA existe

**Archivo:** `Application/Features/Authentication/Commands/Login/LoginCommandHandler.cs`

```csharp
// ACTUAL: Solo valida credenciales y retorna UsuarioDto
var usuario = await _context.Credenciales
    .Include(c => c.Contratista)
    .Include(c => c.Empleador)
    .FirstOrDefaultAsync(c => c.Correo == request.Email && c.Activo, cancellationToken);

if (usuario == null)
    return null;

// Mapea a UsuarioDto y retorna
return _mapper.Map<UsuarioDto>(usuario);
```

**Problema:** No genera tokens JWT. La autenticación actual es manual (cliente debe enviar email/password en cada request o usar cookies no-seguras).

### ❌ Lo que FALTA implementar

1. **JwtTokenService** - Generar access tokens y refresh tokens
2. **RefreshToken Entity** - Almacenar refresh tokens en base de datos
3. **LoginCommand** - Modificar para retornar tokens JWT
4. **RefreshTokenCommand** - Renovar access token usando refresh token
5. **RevokeTokenCommand** - Revocar refresh token (logout seguro)
6. **JWT Configuration** - appsettings.json con secretos
7. **Authentication Middleware** - Validar tokens en cada request

---

## 🏗️ ARQUITECTURA JWT

```
┌─────────────────────────────────────────────────────────────────┐
│                     FLUJO DE AUTENTICACIÓN JWT                   │
└─────────────────────────────────────────────────────────────────┘

1. LOGIN INICIAL
   Cliente → POST /api/auth/login { email, password }
           ↓
   LoginCommand → Valida credenciales
           ↓
   JwtTokenService → Genera:
                      - AccessToken (exp: 15 min)
                      - RefreshToken (exp: 7 días, guardado en DB)
           ↓
   Cliente ← { accessToken, refreshToken, user }

2. REQUEST AUTENTICADO
   Cliente → GET /api/empleados { Authorization: Bearer <accessToken> }
           ↓
   JwtBearerMiddleware → Valida AccessToken
           ↓
   Controller → HttpContext.User.Claims (UserId, Role, etc.)

3. RENOVAR TOKEN (cuando accessToken expira)
   Cliente → POST /api/auth/refresh { refreshToken }
           ↓
   RefreshTokenCommand → Valida RefreshToken en DB
           ↓
   JwtTokenService → Genera nuevo AccessToken
           ↓
   Cliente ← { accessToken }

4. LOGOUT SEGURO
   Cliente → POST /api/auth/revoke { refreshToken }
           ↓
   RevokeTokenCommand → Marca RefreshToken como revocado en DB
           ↓
   Cliente ← { message: "Sesión cerrada" }
```

---

## 📁 ESTRUCTURA A CREAR

```
src/Infrastructure/MiGenteEnLinea.Infrastructure/
├── Identity/
│   ├── JwtTokenService.cs (NUEVO - 200 líneas)
│   └── JwtSettings.cs (NUEVO - 50 líneas)
│
└── Persistence/Contexts/
    └── MiGenteDbContext.cs (MODIFICAR - agregar DbSet<RefreshToken>)

src/Core/MiGenteEnLinea.Domain/Entities/
└── Seguridad/
    └── RefreshToken.cs (NUEVO - 100 líneas)

src/Core/MiGenteEnLinea.Application/
└── Features/Authentication/
    ├── Commands/
    │   ├── Login/
    │   │   └── LoginCommandHandler.cs (MODIFICAR - retornar tokens)
    │   │
    │   ├── RefreshToken/
    │   │   ├── RefreshTokenCommand.cs (NUEVO)
    │   │   ├── RefreshTokenCommandHandler.cs (NUEVO)
    │   │   └── RefreshTokenCommandValidator.cs (NUEVO)
    │   │
    │   └── RevokeToken/
    │       ├── RevokeTokenCommand.cs (NUEVO)
    │       └── RevokeTokenCommandHandler.cs (NUEVO)
    │
    └── DTOs/
        └── LoginResponseDto.cs (MODIFICAR - agregar tokens)

src/Presentation/MiGenteEnLinea.API/
├── Program.cs (MODIFICAR - registrar JWT middleware)
└── Controllers/
    └── AuthController.cs (MODIFICAR - agregar endpoints refresh/revoke)

appsettings.json (MODIFICAR - agregar JWT config)
```

**Total Archivos:** 9 nuevos + 5 modificados = 14 archivos (~800 líneas de código)

---

## 📋 PLAN DE IMPLEMENTACIÓN

### ⏱️ FASE 1: Domain & Settings (2 horas)

#### Paso 1.1: RefreshToken Entity (1 hora)

**Ubicación:** `Domain/Entities/Seguridad/RefreshToken.cs`

```csharp
using System;

namespace MiGenteEnLinea.Domain.Entities.Seguridad;

/// <summary>
/// Entidad: Refresh Token para renovación de sesiones JWT
/// </summary>
public class RefreshToken
{
    /// <summary>
    /// ID único del refresh token
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Token aleatorio único (64 bytes en Base64)
    /// </summary>
    public string Token { get; private set; } = string.Empty;

    /// <summary>
    /// ID del usuario propietario del token
    /// </summary>
    public string UserId { get; private set; } = string.Empty;

    /// <summary>
    /// Fecha de creación
    /// </summary>
    public DateTime CreatedDate { get; private set; }

    /// <summary>
    /// Fecha de expiración (7 días desde creación)
    /// </summary>
    public DateTime ExpiryDate { get; private set; }

    /// <summary>
    /// Indica si el token ha sido revocado manualmente
    /// </summary>
    public bool IsRevoked { get; private set; }

    /// <summary>
    /// Fecha de revocación (si aplica)
    /// </summary>
    public DateTime? RevokedDate { get; private set; }

    /// <summary>
    /// IP del cliente que creó el token (para auditoría)
    /// </summary>
    public string? IpAddress { get; private set; }

    /// <summary>
    /// User Agent del cliente (para auditoría)
    /// </summary>
    public string? UserAgent { get; private set; }

    /// <summary>
    /// Token que reemplazó a este (cuando se usa refresh)
    /// </summary>
    public string? ReplacedByToken { get; private set; }

    // Constructor para EF Core
    private RefreshToken() { }

    /// <summary>
    /// Factory method: Crear nuevo refresh token
    /// </summary>
    public static RefreshToken Create(
        string token,
        string userId,
        DateTime expiryDate,
        string? ipAddress = null,
        string? userAgent = null)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token no puede estar vacío", nameof(token));

        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId no puede estar vacío", nameof(userId));

        if (expiryDate <= DateTime.UtcNow)
            throw new ArgumentException("ExpiryDate debe ser futuro", nameof(expiryDate));

        return new RefreshToken
        {
            Token = token,
            UserId = userId,
            CreatedDate = DateTime.UtcNow,
            ExpiryDate = expiryDate,
            IsRevoked = false,
            IpAddress = ipAddress,
            UserAgent = userAgent
        };
    }

    /// <summary>
    /// Revocar token manualmente
    /// </summary>
    public void Revoke(string? replacedByToken = null)
    {
        if (IsRevoked)
            throw new InvalidOperationException("Token ya está revocado");

        IsRevoked = true;
        RevokedDate = DateTime.UtcNow;
        ReplacedByToken = replacedByToken;
    }

    /// <summary>
    /// Verificar si el token está activo (no expirado y no revocado)
    /// </summary>
    public bool IsActive => !IsRevoked && ExpiryDate > DateTime.UtcNow;
}
```

#### Paso 1.2: JwtSettings Configuration Class (30 min)

**Ubicación:** `Infrastructure/Identity/JwtSettings.cs`

```csharp
namespace MiGenteEnLinea.Infrastructure.Identity;

/// <summary>
/// Configuración JWT desde appsettings.json
/// </summary>
public class JwtSettings
{
    public const string SectionName = "Jwt";

    /// <summary>
    /// Clave secreta para firmar tokens (mínimo 256 bits)
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Emisor del token (issuer claim)
    /// </summary>
    public string Issuer { get; set; } = "MiGenteEnLinea.API";

    /// <summary>
    /// Audiencia del token (audience claim)
    /// </summary>
    public string Audience { get; set; } = "MiGenteEnLinea.Client";

    /// <summary>
    /// Tiempo de expiración del Access Token en minutos (default: 15)
    /// </summary>
    public int AccessTokenExpirationMinutes { get; set; } = 15;

    /// <summary>
    /// Tiempo de expiración del Refresh Token en días (default: 7)
    /// </summary>
    public int RefreshTokenExpirationDays { get; set; } = 7;

    /// <summary>
    /// Validar si la configuración es válida
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(SecretKey))
            throw new InvalidOperationException("JWT SecretKey no configurada");

        if (SecretKey.Length < 32)
            throw new InvalidOperationException("JWT SecretKey debe tener al menos 32 caracteres (256 bits)");

        if (AccessTokenExpirationMinutes < 1)
            throw new InvalidOperationException("AccessTokenExpirationMinutes debe ser al menos 1");

        if (RefreshTokenExpirationDays < 1)
            throw new InvalidOperationException("RefreshTokenExpirationDays debe ser al menos 1");
    }
}
```

#### Paso 1.3: Actualizar appsettings.json (30 min)

**Ubicación:** `Presentation/MiGenteEnLinea.API/appsettings.json`

```json
{
  "Jwt": {
    "SecretKey": "TU_CLAVE_SECRETA_SUPER_SEGURA_MINIMO_32_CARACTERES_BASE64_ENCODED",
    "Issuer": "MiGenteEnLinea.API",
    "Audience": "MiGenteEnLinea.Client",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  }
}
```

**⚠️ IMPORTANTE:** Generar clave secreta segura:

```powershell
# PowerShell: Generar clave aleatoria de 256 bits (32 bytes)
[Convert]::ToBase64String([System.Security.Cryptography.RandomNumberGenerator]::GetBytes(32))
```

**Para Production (appsettings.Production.json):**

```json
{
  "Jwt": {
    "SecretKey": "${JWT_SECRET_KEY}", // Desde variable de entorno
    "AccessTokenExpirationMinutes": 30,
    "RefreshTokenExpirationDays": 14
  }
}
```

---

### ⏱️ FASE 2: JwtTokenService Implementation (4 horas)

#### Paso 2.1: Interface IJwtTokenService (30 min)

**Ubicación:** `Application/Common/Interfaces/IJwtTokenService.cs`

```csharp
using MiGenteEnLinea.Application.Features.Authentication.DTOs;
using MiGenteEnLinea.Domain.Entities.Seguridad;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MiGenteEnLinea.Application.Common.Interfaces;

/// <summary>
/// Servicio para generación y validación de tokens JWT
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Generar Access Token (JWT corta duración)
    /// </summary>
    string GenerateAccessToken(UsuarioDto usuario);

    /// <summary>
    /// Generar Refresh Token (token largo duración almacenado en DB)
    /// </summary>
    Task<RefreshToken> GenerateRefreshTokenAsync(
        string userId,
        string? ipAddress = null,
        string? userAgent = null);

    /// <summary>
    /// Extraer ClaimsPrincipal desde Access Token (validación)
    /// </summary>
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);

    /// <summary>
    /// Validar estructura del token (sin validar expiración)
    /// </summary>
    bool ValidateTokenStructure(string token);
}
```

#### Paso 2.2: JwtTokenService Implementation (3.5 horas)

**Ubicación:** `Infrastructure/Identity/JwtTokenService.cs`

```csharp
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Authentication.DTOs;
using MiGenteEnLinea.Domain.Entities.Seguridad;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MiGenteEnLinea.Infrastructure.Identity;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public JwtTokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
        _jwtSettings.Validate(); // Validar configuración al inicio

        // Configurar parámetros de validación reutilizables
        _tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _jwtSettings.Issuer,

            ValidateAudience = true,
            ValidAudience = _jwtSettings.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),

            ValidateLifetime = true, // Validar expiración
            ClockSkew = TimeSpan.Zero // Sin tolerancia de tiempo
        };
    }

    /// <summary>
    /// Generar Access Token JWT (15 minutos de duración)
    /// </summary>
    public string GenerateAccessToken(UsuarioDto usuario)
    {
        if (usuario == null)
            throw new ArgumentNullException(nameof(usuario));

        // CLAIMS: Información del usuario en el token
        var claims = new[]
        {
            // Standard JWT claims
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID único
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()), // Issued At

            // Custom claims
            new Claim(ClaimTypes.NameIdentifier, usuario.Id),
            new Claim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}"),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim("TipoUsuario", usuario.TipoUsuario), // "Empleador" o "Contratista"
            new Claim("Activo", usuario.Activo.ToString())
        };

        // Agregar claim de Plan si existe
        if (!string.IsNullOrWhiteSpace(usuario.PlanId))
        {
            claims = claims.Append(new Claim("PlanId", usuario.PlanId)).ToArray();
        }

        // FIRMAR TOKEN
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

        var signingCredentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256); // HMAC-SHA256

        // CREAR TOKEN
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = signingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Generar Refresh Token (token aleatorio de 64 bytes)
    /// </summary>
    public Task<RefreshToken> GenerateRefreshTokenAsync(
        string userId,
        string? ipAddress = null,
        string? userAgent = null)
    {
        // Generar token aleatorio criptográficamente seguro
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        var token = Convert.ToBase64String(randomBytes);

        var expiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);

        var refreshToken = RefreshToken.Create(
            token: token,
            userId: userId,
            expiryDate: expiryDate,
            ipAddress: ipAddress,
            userAgent: userAgent);

        return Task.FromResult(refreshToken);
    }

    /// <summary>
    /// Extraer ClaimsPrincipal desde token expirado (para refresh)
    /// </summary>
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        try
        {
            // Crear parámetros SIN validar expiración
            var validationParameters = _tokenValidationParameters.Clone();
            validationParameters.ValidateLifetime = false; // Ignorar expiración

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(
                token,
                validationParameters,
                out SecurityToken securityToken);

            // Verificar que sea JWT válido
            if (securityToken is not JwtSecurityToken jwtToken ||
                !jwtToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Validar estructura del token (sin validar expiración)
    /// </summary>
    public bool ValidateTokenStructure(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return false;

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            // Verificar que sea JWT parseable
            var jwtToken = tokenHandler.ReadJwtToken(token);
            
            return jwtToken != null;
        }
        catch
        {
            return false;
        }
    }
}
```

---

### ⏱️ FASE 3: Commands Implementation (4 horas)

#### Paso 3.1: Modificar LoginCommand (1.5 horas)

**3.1.1: Actualizar LoginResponseDto**

**Ubicación:** `Application/Features/Authentication/DTOs/LoginResponseDto.cs`

```csharp
namespace MiGenteEnLinea.Application.Features.Authentication.DTOs;

/// <summary>
/// Response DTO para login exitoso con tokens JWT
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// Access Token (JWT de corta duración)
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Refresh Token (token de larga duración)
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de expiración del Access Token
    /// </summary>
    public DateTime AccessTokenExpiration { get; set; }

    /// <summary>
    /// Información del usuario autenticado
    /// </summary>
    public UsuarioDto Usuario { get; set; } = null!;
}
```

**3.1.2: Modificar LoginCommandHandler**

```csharp
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Authentication.DTOs;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IMapper _mapper;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        IMapper mapper,
        ILogger<LoginCommandHandler> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<LoginResponseDto?> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Intento de login para: {Email}", request.Email);

        // PASO 1: Buscar usuario por email
        var credencial = await _context.Credenciales
            .Include(c => c.Contratista)
            .Include(c => c.Empleador)
            .FirstOrDefaultAsync(c => c.Correo == request.Email, cancellationToken);

        if (credencial == null)
        {
            _logger.LogWarning("Login fallido: Email no encontrado - {Email}", request.Email);
            return null;
        }

        // PASO 2: Verificar password
        if (!_passwordHasher.VerifyPassword(request.Password, credencial.Clave))
        {
            _logger.LogWarning("Login fallido: Password incorrecto - {Email}", request.Email);
            return null;
        }

        // PASO 3: Verificar que esté activo
        if (!credencial.Activo)
        {
            _logger.LogWarning("Login fallido: Usuario inactivo - {Email}", request.Email);
            return null;
        }

        // PASO 4: Mapear a UsuarioDto
        var usuarioDto = _mapper.Map<UsuarioDto>(credencial);

        // PASO 5: Generar Access Token (JWT)
        var accessToken = _jwtTokenService.GenerateAccessToken(usuarioDto);

        // PASO 6: Generar Refresh Token
        var refreshToken = await _jwtTokenService.GenerateRefreshTokenAsync(
            userId: credencial.Id.ToString(),
            ipAddress: request.IpAddress,
            userAgent: request.UserAgent);

        // PASO 7: Guardar Refresh Token en base de datos
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Login exitoso: {Email} - RefreshTokenId: {TokenId}",
            request.Email,
            refreshToken.Id);

        // PASO 8: Retornar response con tokens
        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            AccessTokenExpiration = DateTime.UtcNow.AddMinutes(15),
            Usuario = usuarioDto
        };
    }
}
```

**3.1.3: Actualizar LoginCommand** (agregar IpAddress y UserAgent)

```csharp
public record LoginCommand : IRequest<LoginResponseDto?>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    
    // NUEVOS: Para auditoría de refresh tokens
    public string? IpAddress { get; init; }
    public string? UserAgent { get; init; }
}
```

---

#### Paso 3.2: RefreshTokenCommand (1.5 horas)

**3.2.1: Command Class**

```csharp
using MediatR;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.RefreshToken;

/// <summary>
/// Comando: Renovar Access Token usando Refresh Token
/// </summary>
public record RefreshTokenCommand : IRequest<string>
{
    public string RefreshToken { get; init; } = string.Empty;
    public string? IpAddress { get; init; }
    public string? UserAgent { get; init; }
}
```

**3.2.2: Command Handler**

```csharp
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(
        IApplicationDbContext context,
        IJwtTokenService jwtTokenService,
        ILogger<RefreshTokenCommandHandler> logger)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }

    public async Task<string> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Intentando renovar token");

        // PASO 1: Buscar Refresh Token en DB
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken, cancellationToken);

        if (refreshToken == null)
        {
            _logger.LogWarning("Refresh token no encontrado");
            throw new UnauthorizedAccessException("Refresh token inválido");
        }

        // PASO 2: Validar que esté activo (no expirado, no revocado)
        if (!refreshToken.IsActive)
        {
            _logger.LogWarning(
                "Refresh token inactivo - IsRevoked: {IsRevoked}, Expired: {Expired}",
                refreshToken.IsRevoked,
                refreshToken.ExpiryDate < DateTime.UtcNow);

            throw new UnauthorizedAccessException("Refresh token expirado o revocado");
        }

        // PASO 3: Obtener usuario desde UserId
        var usuario = await _context.Credenciales
            .Include(c => c.Contratista)
            .Include(c => c.Empleador)
            .FirstOrDefaultAsync(c => c.Id.ToString() == refreshToken.UserId, cancellationToken);

        if (usuario == null || !usuario.Activo)
        {
            _logger.LogWarning("Usuario no encontrado o inactivo - UserId: {UserId}", refreshToken.UserId);
            throw new UnauthorizedAccessException("Usuario inválido");
        }

        // PASO 4: Generar nuevo Access Token
        var usuarioDto = _mapper.Map<UsuarioDto>(usuario);
        var newAccessToken = _jwtTokenService.GenerateAccessToken(usuarioDto);

        // PASO 5 (OPCIONAL): Rotar Refresh Token (buena práctica de seguridad)
        // Crear nuevo refresh token y revocar el anterior
        var newRefreshToken = await _jwtTokenService.GenerateRefreshTokenAsync(
            userId: refreshToken.UserId,
            ipAddress: request.IpAddress,
            userAgent: request.UserAgent);

        refreshToken.Revoke(replacedByToken: newRefreshToken.Token);

        _context.RefreshTokens.Add(newRefreshToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Token renovado exitosamente - UserId: {UserId}",
            refreshToken.UserId);

        return newAccessToken;
    }
}
```

---

#### Paso 3.3: RevokeTokenCommand (1 hora)

```csharp
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.RevokeToken;

public record RevokeTokenCommand : IRequest<Unit>
{
    public string RefreshToken { get; init; } = string.Empty;
}

public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<RevokeTokenCommandHandler> _logger;

    public RevokeTokenCommandHandler(
        IApplicationDbContext context,
        ILogger<RevokeTokenCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken, cancellationToken);

        if (refreshToken == null)
            throw new KeyNotFoundException("Refresh token no encontrado");

        if (!refreshToken.IsActive)
            throw new InvalidOperationException("Refresh token ya está inactivo");

        refreshToken.Revoke();
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Token revocado - UserId: {UserId}", refreshToken.UserId);

        return Unit.Value;
    }
}
```

---

### ⏱️ FASE 4: Controller & Middleware (4 horas)

#### Paso 4.1: Actualizar AuthController (2 horas)

```csharp
/// <summary>
/// Login con generación de JWT tokens
/// </summary>
[HttpPost("login")]
[AllowAnonymous]
[ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginCommand command)
{
    // Capturar IP y User-Agent para auditoría
    command = command with
    {
        IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
        UserAgent = HttpContext.Request.Headers["User-Agent"].ToString()
    };

    var response = await _mediator.Send(command);

    if (response == null)
        return Unauthorized(new { message = "Credenciales inválidas" });

    return Ok(response);
}

/// <summary>
/// Renovar Access Token usando Refresh Token
/// </summary>
[HttpPost("refresh")]
[AllowAnonymous]
[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public async Task<ActionResult<string>> RefreshToken([FromBody] RefreshTokenCommand command)
{
    command = command with
    {
        IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
        UserAgent = HttpContext.Request.Headers["User-Agent"].ToString()
    };

    var newAccessToken = await _mediator.Send(command);

    return Ok(new { accessToken = newAccessToken });
}

/// <summary>
/// Revocar Refresh Token (logout seguro)
/// </summary>
[HttpPost("revoke")]
[Authorize]
[ProducesResponseType(StatusCodes.Status204NoContent)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenCommand command)
{
    await _mediator.Send(command);
    return NoContent();
}
```

---

#### Paso 4.2: Configurar JWT Middleware en Program.cs (2 horas)

```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MiGenteEnLinea.Infrastructure.Identity;
using System.Text;

// ... (después de builder.Services)

// PASO 1: Registrar JwtSettings desde appsettings.json
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection(JwtSettings.SectionName));

// PASO 2: Registrar IJwtTokenService
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// PASO 3: Configurar JWT Authentication
var jwtSettings = builder.Configuration
    .GetSection(JwtSettings.SectionName)
    .Get<JwtSettings>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,

        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero // Sin tolerancia
    };

    // Eventos para logging
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception is SecurityTokenExpiredException)
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }

            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<Program>>();

            var userId = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            logger.LogInformation("Token validado para usuario: {UserId}", userId);

            return Task.CompletedTask;
        }
    };
});

// ... (después de app.Build())

app.UseAuthentication(); // ANTES de UseAuthorization
app.UseAuthorization();
```

---

#### Paso 4.3: Agregar DbSet en DbContext (15 min)

```csharp
// MiGenteDbContext.cs
public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
```

---

#### Paso 4.4: Crear Migration (15 min)

```powershell
# Crear migración para RefreshTokens
dotnet ef migrations add AddRefreshTokens `
  --startup-project src/Presentation/MiGenteEnLinea.API `
  --project src/Infrastructure/MiGenteEnLinea.Infrastructure `
  --context MiGenteDbContext `
  --output-dir Persistence/Migrations

# Aplicar migración
dotnet ef database update `
  --startup-project src/Presentation/MiGenteEnLinea.API `
  --project src/Infrastructure/MiGenteEnLinea.Infrastructure
```

---

### ⏱️ FASE 5: Testing (2 horas)

#### Paso 5.1: Testing con Swagger UI (1 hora)

**Flujo Completo:**

1. **Login** → POST `/api/auth/login`
   - Input: `{ "email": "test@example.com", "password": "password123" }`
   - Output: `{ "accessToken": "eyJ...", "refreshToken": "base64...", "usuario": {...} }`

2. **Request Autenticado** → GET `/api/empleadores`
   - Header: `Authorization: Bearer eyJ...`
   - Verificar que funciona

3. **Esperar 15 minutos** (o modificar temporalmente `AccessTokenExpirationMinutes` a 1)

4. **Refresh Token** → POST `/api/auth/refresh`
   - Input: `{ "refreshToken": "base64..." }`
   - Output: `{ "accessToken": "eyJ..." }` (nuevo token)

5. **Revoke Token** → POST `/api/auth/revoke`
   - Input: `{ "refreshToken": "base64..." }`
   - Verificar que intento posterior de refresh falle

#### Paso 5.2: Unit Tests (1 hora)

```csharp
[Fact]
public async Task GenerateAccessToken_WithValidUser_ReturnsValidJwt()
{
    // Arrange
    var usuario = new UsuarioDto { Id = "123", Email = "test@test.com", ... };
    var service = CreateJwtTokenService();

    // Act
    var token = service.GenerateAccessToken(usuario);

    // Assert
    Assert.NotNull(token);
    Assert.True(service.ValidateTokenStructure(token));
}

[Fact]
public async Task RefreshToken_WithExpiredToken_Fails()
{
    // Arrange
    var expiredRefreshToken = CreateExpiredRefreshToken();

    // Act & Assert
    await Assert.ThrowsAsync<UnauthorizedAccessException>(
        () => _handler.Handle(new RefreshTokenCommand { RefreshToken = expiredRefreshToken.Token }, CancellationToken.None));
}
```

---

## ✅ CHECKLIST DE COMPLETADO

### Fase 1: Domain & Settings (2 horas)

- [ ] RefreshToken entity creada
- [ ] JwtSettings configuration class creada
- [ ] appsettings.json actualizado con JWT config
- [ ] Clave secreta generada (256 bits mínimo)

### Fase 2: JwtTokenService (4 horas)

- [ ] IJwtTokenService interface creada
- [ ] JwtTokenService implementado (GenerateAccessToken, GenerateRefreshToken, GetPrincipalFromExpiredToken)
- [ ] Compilación sin errores

### Fase 3: Commands (4 horas)

- [ ] LoginCommand modificado (retorna LoginResponseDto con tokens)
- [ ] RefreshTokenCommand + Handler creados
- [ ] RevokeTokenCommand + Handler creados
- [ ] Compilación sin errores

### Fase 4: Controller & Middleware (4 horas)

- [ ] AuthController actualizado (3 endpoints: login, refresh, revoke)
- [ ] Program.cs configurado (AddAuthentication, AddJwtBearer)
- [ ] DbSet<RefreshToken> agregado a DbContext
- [ ] Migration creada y aplicada
- [ ] API ejecutándose sin errores

### Fase 5: Testing (2 horas)

- [ ] Login retorna tokens JWT
- [ ] Request autenticado con Bearer token funciona
- [ ] Refresh token renueva access token correctamente
- [ ] Revoke token invalida refresh token
- [ ] Unit tests pasan (80%+ coverage)

---

## 📊 ENDPOINTS REST FINALES

| Método | Endpoint | Auth | Descripción |
|--------|----------|------|-------------|
| POST | `/api/auth/login` | ❌ Público | Login con JWT tokens |
| POST | `/api/auth/refresh` | ❌ Público | Renovar access token |
| POST | `/api/auth/revoke` | ✅ Required | Revocar refresh token (logout) |

---

## 🔒 SEGURIDAD ADICIONAL (Post-MVP)

Después de completar el plan base, considerar:

1. **IP Binding**: Validar que refresh token solo funcione desde misma IP
2. **User-Agent Binding**: Validar que refresh token solo funcione desde mismo dispositivo
3. **Token Rotation**: Rotar refresh token en cada uso (implementado en Paso 3.2)
4. **Limpieza Automática**: Job para eliminar refresh tokens expirados (> 30 días)
5. **Multi-Device Support**: Permitir múltiples refresh tokens por usuario
6. **Device Management**: Endpoint para listar/revocar dispositivos activos

---

## 📈 MÉTRICAS DE ÉXITO

| Métrica | Objetivo | Cómo Verificar |
|---------|----------|----------------|
| **Compilación** | 0 errores | `dotnet build` |
| **Swagger Functional** | 3/3 endpoints | Swagger UI |
| **Access Token Valid** | 15 min | JWT.io decode |
| **Refresh Token Works** | Renueva sin re-login | Testing manual |
| **Revoke Works** | Token queda inválido | Testing manual |

---

**Elaborado por:** GitHub Copilot AI Agent  
**Fecha:** 2025-01-13  
**Versión:** 1.0  
**Estado:** ⏳ PENDIENTE DE EJECUCIÓN
