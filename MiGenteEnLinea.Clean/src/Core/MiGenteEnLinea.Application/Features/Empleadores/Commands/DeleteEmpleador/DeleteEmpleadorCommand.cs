using MediatR;

namespace MiGenteEnLinea.Application.Features.Empleadores.Commands.DeleteEmpleador;

/// <summary>
/// Command: Eliminar (soft delete) perfil de Empleador
/// </summary>
/// <remarks>
/// LEGACY: No hay método específico de eliminación
/// CLEAN: Soft delete usando SoftDeletableEntity pattern
/// 
/// LÓGICA DE NEGOCIO:
/// - Buscar empleador por userId
/// - Marcar como eliminado (IsDeleted=true, DeletedAt=DateTime.UtcNow)
/// - NO eliminar físicamente de la base de datos
/// </remarks>
public record DeleteEmpleadorCommand(string UserId) : IRequest<bool>;
