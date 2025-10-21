using MediatR;

namespace MiGenteEnLinea.Application.Features.Empleadores.Commands.UpdateEmpleador;

/// <summary>
/// Command: Actualizar perfil de Empleador
/// </summary>
/// <remarks>
/// RÉPLICA DE: MiPerfilEmpleador.aspx.cs→ActualizarPerfil()
/// LEGACY: Actualiza perfilesInfo + Cuentas (NO actualiza Ofertantes directamente)
/// 
/// CLEAN ARCHITECTURE: Actualiza Empleador entity con método de dominio
/// 
/// LÓGICA DE NEGOCIO:
/// - Buscar empleador por userId
/// - Actualizar con método de dominio ActualizarPerfil()
/// - Solo actualiza campos proporcionados (no null)
/// </remarks>
public record UpdateEmpleadorCommand(
    string UserId,
    string? Habilidades = null,
    string? Experiencia = null,
    string? Descripcion = null
) : IRequest<bool>; // Retorna true si actualización exitosa
