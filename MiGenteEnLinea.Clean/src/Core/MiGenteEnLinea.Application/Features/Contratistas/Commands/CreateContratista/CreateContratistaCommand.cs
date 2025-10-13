using MediatR;

namespace MiGenteEnLinea.Application.Features.Contratistas.Commands.CreateContratista;

/// <summary>
/// Command: Crear un nuevo perfil de contratista
/// </summary>
/// <remarks>
/// LÓGICA LEGACY: Al registrarse como contratista por primera vez
/// RELACIÓN: Un userId (Credencial) puede ser empleador O contratista (1:1)
/// VALIDACIÓN: Se valida que no exista otro contratista con el mismo userId
/// </remarks>
/// <param name="UserId">ID del usuario (GUID de Credenciales)</param>
/// <param name="Nombre">Nombre del contratista (max 20 caracteres)</param>
/// <param name="Apellido">Apellido del contratista (max 50 caracteres)</param>
/// <param name="Tipo">Tipo: 1=Persona Física (default), 2=Empresa</param>
/// <param name="Titulo">Título profesional (opcional, max 70 caracteres)</param>
/// <param name="Identificacion">Cédula o RNC (opcional, max 20 caracteres)</param>
/// <param name="Sector">Sector económico (opcional, max 40 caracteres)</param>
/// <param name="Experiencia">Años de experiencia (opcional)</param>
/// <param name="Presentacion">Descripción/biografía (opcional, max 250 caracteres)</param>
/// <param name="Telefono1">Teléfono principal (opcional, max 16 caracteres)</param>
/// <param name="Whatsapp1">¿Teléfono1 es WhatsApp? (default: false)</param>
/// <param name="Provincia">Provincia donde opera (opcional, max 50 caracteres)</param>
public record CreateContratistaCommand(
    string UserId,
    string Nombre,
    string Apellido,
    int Tipo = 1,
    string? Titulo = null,
    string? Identificacion = null,
    string? Sector = null,
    int? Experiencia = null,
    string? Presentacion = null,
    string? Telefono1 = null,
    bool Whatsapp1 = false,
    string? Provincia = null
) : IRequest<int>; // Retorna: contratistaId
