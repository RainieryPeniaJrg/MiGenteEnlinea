using MediatR;

namespace MiGenteEnLinea.Application.Features.Empleadores.Commands.CreateEmpleador;

/// <summary>
/// Command: Crear un nuevo perfil de Empleador
/// </summary>
/// <remarks>
/// RÉPLICA DE: Registro de usuario tipo Empleador (implícito en registro)
/// LEGACY: No hay método específico, se crea automáticamente al registrar usuario tipo=1
/// 
/// LÓGICA DE NEGOCIO:
/// - Validar que userId existe en Credenciales
/// - Validar que NO exista empleador para ese userId (relación 1:1)
/// - Crear empleador con factory method de dominio
/// - Habilidades, Experiencia, Descripcion son opcionales
/// </remarks>
public record CreateEmpleadorCommand(
    string UserId,
    string? Habilidades = null,
    string? Experiencia = null,
    string? Descripcion = null
) : IRequest<int>; // Retorna empleadorId
