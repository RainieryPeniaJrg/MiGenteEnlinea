using MediatR;

namespace MiGenteEnLinea.Application.Features.Empleadores.Commands.UpdateEmpleadorFoto;

/// <summary>
/// Command: Actualizar foto/logo del Empleador
/// </summary>
/// <remarks>
/// LEGACY: No hay método específico en Legacy
/// CLEAN: Método de dominio ActualizarFoto() con validación de tamaño (max 5MB)
/// 
/// LÓGICA DE NEGOCIO:
/// - Buscar empleador por userId
/// - Validar tamaño máximo (5MB)
/// - Actualizar foto con método de dominio
/// </remarks>
public record UpdateEmpleadorFotoCommand(
    string UserId,
    byte[] Foto
) : IRequest<bool>; // Retorna true si actualización exitosa
