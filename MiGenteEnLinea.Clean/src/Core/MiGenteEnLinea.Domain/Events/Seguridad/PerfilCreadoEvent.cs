using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Events.Seguridad;

/// <summary>
/// Evento que se dispara cuando se crea un nuevo perfil de usuario
/// </summary>
public sealed class PerfilCreadoEvent : DomainEvent
{
    public int PerfilId { get; }
    public string UserId { get; }
    public int Tipo { get; }
    public string Nombre { get; }
    public string Apellido { get; }
    public string Email { get; }

    public PerfilCreadoEvent(
        int perfilId,
        string userId,
        int tipo,
        string nombre,
        string apellido,
        string email)
    {
        PerfilId = perfilId;
        UserId = userId;
        Tipo = tipo;
        Nombre = nombre;
        Apellido = apellido;
        Email = email;
    }
}
