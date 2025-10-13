using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Authentication.DTOs;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.Login;

/// <summary>
/// Handler para el comando de login
/// </summary>
/// <remarks>
/// LÓGICA COPIADA EXACTAMENTE DE: LoginService.asmx.cs -> login(string email, string pass)
/// 
/// Flujo Legacy:
/// 1. Buscar credencial por email y password encriptado
/// 2. Verificar si está activo (retorna -1 si inactivo)
/// 3. Obtener datos de cuenta (nombre, apellido, tipo)
/// 4. Obtener suscripción más reciente con plan
/// 5. Obtener perfil de VPerfiles
/// 6. Crear cookie con todos los datos
/// 7. Retornar código 2 (success), 0 (invalid), -1 (inactive)
/// </remarks>
public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        ILogger<LoginCommandHandler> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Intentando login para email: {Email}", request.Email);

        // PASO 1: Buscar credencial por email (IGUAL AL LEGACY)
        // Legacy: db.Credenciales.Where(x => x.email == email && x.password == crypted).FirstOrDefault()
        var credencial = await _context.Credenciales
            .Where(x => x.Email == request.Email)
            .FirstOrDefaultAsync(cancellationToken);

        if (credencial == null)
        {
            _logger.LogWarning("Login fallido - Email no encontrado: {Email}", request.Email);
            return new LoginResult { StatusCode = 0 }; // Credenciales inválidas
        }

        // PASO 2: Verificar password
        // Legacy usa: Crypt.Encrypt(pass) - custom encryption
        // Clean usa: BCrypt.VerifyPassword() - modern hashing
        // NOTA: Durante migración gradual, soportamos ambos formatos
        bool passwordValid = false;

        // Intentar BCrypt primero (nuevo estándar)
        if (credencial.PasswordHash.StartsWith("$2a$") || 
            credencial.PasswordHash.StartsWith("$2b$") ||
            credencial.PasswordHash.StartsWith("$2y$"))
        {
            // Es BCrypt
            passwordValid = _passwordHasher.VerifyPassword(request.Password, credencial.PasswordHash);
        }
        else
        {
            // Es legacy Crypt - TEMPORAL: comparar directo hasta migración completa
            // TODO: Implementar LegacyCryptService para compatibilidad
            _logger.LogWarning("Password en formato legacy detectado para {Email}. Requiere migración.", request.Email);
            // Por ahora, retornamos invalid para forzar reset de password
            passwordValid = false;
        }

        if (!passwordValid)
        {
            _logger.LogWarning("Login fallido - Password incorrecto para: {Email}", request.Email);
            return new LoginResult { StatusCode = 0 }; // Credenciales inválidas
        }

        // PASO 3: Verificar si está activo (IGUAL AL LEGACY)
        // Legacy: if (!(bool)result.activo) return -1;
        if (!credencial.Activo)
        {
            _logger.LogWarning("Login fallido - Cuenta inactiva: {Email}", request.Email);
            return new LoginResult { StatusCode = -1 }; // Cuenta inactiva
        }

        // PASO 4: Obtener datos básicos desde VistaPerfil (reemplaza Cuentas)
        // Legacy usaba Cuentas, pero usaremos VPerfiles que tiene toda la info
        var perfil = await _context.VPerfiles
            .Where(x => x.UserId == credencial.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (perfil == null)
        {
            _logger.LogError("Perfil no encontrado para userId: {UserId}", credencial.UserId);
            return new LoginResult { StatusCode = 0 }; // Error: perfil debe existir
        }

        // PASO 5: Obtener suscripción más reciente con plan (IGUAL AL LEGACY)
        // Legacy: db.Suscripciones.Where(x => x.userID == result.userID)
        //         .Include(a => a.Planes_empleadores).OrderByDescending(x => x.suscripcionID).FirstOrDefault()
        var suscripcion = await _context.Suscripciones
            .Where(x => x.UserId == credencial.UserId)
            .OrderByDescending(x => x.Id) // Cambio: SuscripcionId → Id
            .FirstOrDefaultAsync(cancellationToken);

        int? planId = 0;
        DateTime? vencimientoPlan = null;
        int? nomina = null;
        int? empleados = null;
        bool? historico = null;

        if (suscripcion != null)
        {
            planId = suscripcion.PlanId;
            vencimientoPlan = suscripcion.Vencimiento.ToDateTime(TimeOnly.MinValue); // Cambio: DateOnly → DateTime

            // Obtener detalles del plan (LEGACY: eager loading con Include)
            var plan = await _context.PlanesEmpleadores
                .Where(p => p.PlanId == suscripcion.PlanId)
                .FirstOrDefaultAsync(cancellationToken);

            if (plan != null)
            {
                nomina = plan.IncluyeNomina ? 1 : 0; // Cambio: Nomina → IncluyeNomina (bool → int)
                empleados = plan.LimiteEmpleados; // Cambio: Empleados → LimiteEmpleados
                historico = plan.MesesHistorico > 0; // Cambio: Historico → MesesHistorico (int → bool)
            }
        }

        // PASO 6: Obtener perfil de VPerfiles (IGUAL AL LEGACY)
        // Legacy: obtenerPerfil(result.userID) -> db.VPerfiles.Where(a => a.userID == userID).FirstOrDefault()
        var vPerfil = await _context.VPerfiles
            .Where(x => x.UserId == credencial.UserId)
            .AsNoTracking() // Read-only
            .FirstOrDefaultAsync(cancellationToken);

        PerfilDto? perfilDto = null;
        if (vPerfil != null)
        {
            perfilDto = new PerfilDto
            {
                UserId = vPerfil.UserId,
                Nombre = vPerfil.Nombre,
                Apellido = vPerfil.Apellido,
                Tipo = vPerfil.Tipo,
                Telefono1 = vPerfil.Telefono1,
                Telefono2 = vPerfil.Telefono2,
                FechaCreacion = vPerfil.FechaCreacion,
                Email = vPerfil.Email,
                PerfilId = vPerfil.PerfilId
            };
        }

        // PASO 7: Retornar resultado (ESTRUCTURA IGUAL AL LEGACY)
        // Legacy retorna código 2 para éxito y crea cookie
        _logger.LogInformation("Login exitoso para usuario: {UserId} - Email: {Email}", credencial.UserId, credencial.Email);

        return new LoginResult
        {
            StatusCode = 2, // Success (IGUAL AL LEGACY)
            UserId = credencial.UserId,
            Email = credencial.Email,
            Nombre = $"{perfil.Nombre} {perfil.Apellido}", // Cambio: cuenta → perfil
            Tipo = perfil.Tipo, // Cambio: cuenta → perfil (ya es int?)
            PlanId = planId,
            VencimientoPlan = vencimientoPlan,
            Nomina = nomina,
            Empleados = empleados,
            Historico = historico,
            Perfil = perfilDto
            // Token y RefreshToken se agregarán cuando se implemente IJwtTokenService
        };
    }
}
