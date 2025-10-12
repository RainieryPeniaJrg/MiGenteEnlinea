using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Seguridad;

/// <summary>
/// Evento que se dispara cuando se actualiza el nombre completo de un perfil
/// </summary>
public sealed class PerfilActualizadoEvent : DomainEvent
{
    public int PerfilId { get; }
    public string UserId { get; }
    public string NombreCompletoAnterior { get; }
    public string NombreCompletoNuevo { get; }

    public PerfilActualizadoEvent(
        int perfilId,
        string userId,
        string nombreCompletoAnterior,
        string nombreCompletoNuevo)
    {
        PerfilId = perfilId;
        UserId = userId;
        NombreCompletoAnterior = nombreCompletoAnterior;
        NombreCompletoNuevo = nombreCompletoNuevo;
    }
}
