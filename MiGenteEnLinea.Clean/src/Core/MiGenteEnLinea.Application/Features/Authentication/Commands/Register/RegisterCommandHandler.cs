using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Authentication.DTOs;
using MiGenteEnLinea.Domain.Entities.Authentication;
using MiGenteEnLinea.Domain.Entities.Contratistas;
using MiGenteEnLinea.Domain.Entities.Seguridad;
using MiGenteEnLinea.Domain.Entities.Suscripciones;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.Register;

/// <summary>
/// Handler para RegisterCommand
/// Réplica EXACTA de SuscripcionesService.GuardarPerfil() del Legacy
/// </summary>
public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailService _emailService;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IEmailService emailService,
        ILogger<RegisterCommandHandler> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<RegisterResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // ================================================================================
        // PASO 1: VALIDAR QUE EL EMAIL NO EXISTA
        // ================================================================================
        // Legacy usa Cuentas.Email, Clean usa Credenciales.Email (mismo objetivo)
        var emailLower = request.Email.ToLowerInvariant();
        var emailExists = await _context.Credenciales
            .AnyAsync(c => c.Email.Value.ToLowerInvariant() == emailLower, cancellationToken);

        if (emailExists)
        {
            _logger.LogWarning("Intento de registro con email duplicado: {Email}", request.Email);
            return new RegisterResult
            {
                Success = false,
                UserId = null,
                Message = "El correo electrónico ya está registrado en el sistema"
            };
        }

        // ================================================================================
        // PASO 2: CREAR PERFIL (equivalente a Cuenta en Legacy)
        // ================================================================================
        // Legacy: db.Cuentas.Add(p);
        // Clean: Usamos Perfile (mapea a tabla Perfiles en DB)
        var userId = Guid.NewGuid().ToString();
        
        Perfile perfil;
        if (request.Tipo == 1)
        {
            perfil = Perfile.CrearPerfilEmpleador(
                userId: userId,
                nombre: request.Nombre,
                apellido: request.Apellido,
                email: request.Email,
                telefono1: request.Telefono1,
                telefono2: request.Telefono2
            );
        }
        else // Tipo == 2
        {
            perfil = Perfile.CrearPerfilContratista(
                userId: userId,
                nombre: request.Nombre,
                apellido: request.Apellido,
                email: request.Email,
                telefono1: request.Telefono1,
                telefono2: request.Telefono2
            );
        }
        
        await _context.Perfiles.AddAsync(perfil, cancellationToken);

        // ================================================================================
        // PASO 3: CREAR CREDENCIAL CON PASSWORD HASHEADO
        // ================================================================================
        // Legacy: guardarCredenciales() crea registro en Credenciales con password encriptado
        // Clean: Usamos BCrypt en lugar de Crypt.Encrypt()
        // Nota: Credencial.Create() ya pone activo=false por defecto
        var email = Domain.ValueObjects.Email.Create(request.Email);
        var credencial = Credencial.Create(
            userId: userId,
            email: email,
            passwordHash: _passwordHasher.HashPassword(request.Password)
        );

        await _context.Credenciales.AddAsync(credencial, cancellationToken);

        // ================================================================================
        // PASO 4: CREAR CONTRATISTA SI ES TIPO 2
        // ================================================================================
        // Legacy: if (tipo == 2) → guardarNuevoContratista(c)
        if (request.Tipo == 2)
        {
            var contratista = Contratista.Create(
                userId: userId,
                nombre: request.Nombre,
                apellido: request.Apellido,
                tipo: 1, // ⚠️ IMPORTANTE: tipo=1 por defecto (Persona Física) - igual que Legacy
                telefono1: request.Telefono1
            );

            await _context.Contratistas.AddAsync(contratista, cancellationToken);
        }

        // ================================================================================
        // PASO 5: CREAR SUSCRIPCIÓN INICIAL CON PLANID=0
        // ================================================================================
        // Legacy: No crea suscripción en GuardarPerfil, pero en flujo de registro siempre
        //         debe haber una suscripción inicial sin plan (planID=0)
        // TODO: Descomentar cuando se implemente un factory method para suscripción sin plan
        // Por ahora, la suscripción se creará cuando el usuario compre su primer plan
        /*
        var suscripcion = Suscripcion.Create(
            userId: userId,
            planId: 0, // ⚠️ PROBLEMA: Create() valida planId > 0
            duracionMeses: 1
        );

        await _context.Suscripciones.AddAsync(suscripcion, cancellationToken);
        */

        // ================================================================================
        // PASO 6: GUARDAR CAMBIOS EN LA BASE DE DATOS
        // ================================================================================
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Usuario registrado exitosamente. UserId: {UserId}, Email: {Email}, Tipo: {Tipo}",
            userId, request.Email, request.Tipo);

        // ================================================================================
        // PASO 7: ENVIAR EMAIL DE ACTIVACIÓN
        // ================================================================================
        // Legacy: enviarCorreoActivacion(host, email, p)
        //         Genera URL: host + "/Activar.aspx?userID=" + userID + "&email=" + email
        try
        {
            // Nueva firma: SendActivationEmailAsync(toEmail, toName, activationUrl)
            var nombreCompleto = $"{request.Nombre} {request.Apellido}";
            var activationUrl = $"{request.Host}/Activar.aspx?userID={userId}&email={request.Email}";
            
            await _emailService.SendActivationEmailAsync(
                toEmail: request.Email,
                toName: nombreCompleto,
                activationUrl: activationUrl
            );

            _logger.LogInformation("Email de activación enviado a: {Email}", request.Email);
        }
        catch (Exception ex)
        {
            // NO fallar el registro si el email falla
            // El usuario ya está creado, solo el email falló
            _logger.LogError(ex, "Error al enviar email de activación a {Email}", request.Email);
        }

        // ================================================================================
        // RETORNAR RESULTADO EXITOSO
        // ================================================================================
        return new RegisterResult
        {
            Success = true,
            UserId = userId,
            Email = request.Email,
            Message = "Registro exitoso. Por favor revisa tu correo electrónico para activar tu cuenta."
        };
    }
}
