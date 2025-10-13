using MiGenteEnLinea.Domain.Common;
using MiGenteEnLinea.Domain.Events.Seguridad;
using MiGenteEnLinea.Domain.ValueObjects;

namespace MiGenteEnLinea.Domain.Entities.Seguridad;

/// <summary>
/// Representa un perfil de usuario en el sistema.
/// Puede ser un perfil de Empleador (tipo 1) o Contratista (tipo 2).
/// Contiene información básica del usuario y puede tener información extendida en PerfilesInfo.
/// </summary>
public class Perfile : AggregateRoot
{
    #region Properties

    /// <summary>
    /// Identificador único del perfil
    /// </summary>
    public int PerfilId { get; private set; }

    /// <summary>
    /// Fecha de creación del perfil
    /// </summary>
    public DateTime FechaCreacion { get; private set; }

    /// <summary>
    /// Identificador del usuario asociado al perfil
    /// </summary>
    public string UserId { get; private set; } = string.Empty;

    /// <summary>
    /// Tipo de perfil: 1 = Empleador, 2 = Contratista
    /// </summary>
    public int Tipo { get; private set; }

    /// <summary>
    /// Nombre del usuario
    /// </summary>
    public string Nombre { get; private set; } = string.Empty;

    /// <summary>
    /// Apellido del usuario
    /// </summary>
    public string Apellido { get; private set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del usuario
    /// </summary>
    public string Email { get; private set; } = string.Empty;

    /// <summary>
    /// Teléfono principal del usuario
    /// </summary>
    public string? Telefono1 { get; private set; }

    /// <summary>
    /// Teléfono secundario del usuario
    /// </summary>
    public string? Telefono2 { get; private set; }

    /// <summary>
    /// Nombre de usuario para login
    /// </summary>
    public string? Usuario { get; private set; }

    /// <summary>
    /// Nombre completo del usuario (calculado)
    /// </summary>
    public string NombreCompleto => $"{Nombre} {Apellido}".Trim();

    /// <summary>
    /// Indica si es un perfil de empleador
    /// </summary>
    public bool EsEmpleador => Tipo == TipoPerfilEnum.Empleador;

    /// <summary>
    /// Indica si es un perfil de contratista
    /// </summary>
    public bool EsContratista => Tipo == TipoPerfilEnum.Contratista;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor privado para EF Core
    /// </summary>
    private Perfile() 
    {
        FechaCreacion = DateTime.UtcNow;
    }

    /// <summary>
    /// Constructor privado para creación controlada
    /// </summary>
    private Perfile(
        string userId,
        int tipo,
        string nombre,
        string apellido,
        string email,
        string? telefono1 = null,
        string? telefono2 = null,
        string? usuario = null)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("El UserId no puede estar vacío", nameof(userId));

        if (!EsTipoValido(tipo))
            throw new ArgumentException("El tipo de perfil debe ser 1 (Empleador) o 2 (Contratista)", nameof(tipo));

        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre no puede estar vacío", nameof(nombre));

        if (nombre.Length > 20)
            throw new ArgumentException("El nombre no puede exceder 20 caracteres", nameof(nombre));

        if (string.IsNullOrWhiteSpace(apellido))
            throw new ArgumentException("El apellido no puede estar vacío", nameof(apellido));

        if (apellido.Length > 50)
            throw new ArgumentException("El apellido no puede exceder 50 caracteres", nameof(apellido));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email no puede estar vacío", nameof(email));

        if (email.Length > 100)
            throw new ArgumentException("El email no puede exceder 100 caracteres", nameof(email));

        if (!string.IsNullOrWhiteSpace(telefono1) && telefono1.Length > 20)
            throw new ArgumentException("El teléfono 1 no puede exceder 20 caracteres", nameof(telefono1));

        if (!string.IsNullOrWhiteSpace(telefono2) && telefono2.Length > 20)
            throw new ArgumentException("El teléfono 2 no puede exceder 20 caracteres", nameof(telefono2));

        if (!string.IsNullOrWhiteSpace(usuario) && usuario.Length > 20)
            throw new ArgumentException("El usuario no puede exceder 20 caracteres", nameof(usuario));

        FechaCreacion = DateTime.UtcNow;
        UserId = userId;
        Tipo = tipo;
        Nombre = nombre;
        Apellido = apellido;
        Email = email;
        Telefono1 = telefono1;
        Telefono2 = telefono2;
        Usuario = usuario;
    }

    #endregion

    #region Factory Methods

    /// <summary>
    /// Crea un nuevo perfil de empleador
    /// </summary>
    public static Perfile CrearPerfilEmpleador(
        string userId,
        string nombre,
        string apellido,
        string email,
        string? telefono1 = null,
        string? telefono2 = null,
        string? usuario = null)
    {
        var perfil = new Perfile(
            userId,
            TipoPerfilEnum.Empleador,
            nombre,
            apellido,
            email,
            telefono1,
            telefono2,
            usuario);

        perfil.RaiseDomainEvent(new PerfilCreadoEvent(
            perfil.PerfilId,
            userId,
            TipoPerfilEnum.Empleador,
            nombre,
            apellido,
            email));

        return perfil;
    }

    /// <summary>
    /// Crea un nuevo perfil de contratista
    /// </summary>
    public static Perfile CrearPerfilContratista(
        string userId,
        string nombre,
        string apellido,
        string email,
        string? telefono1 = null,
        string? telefono2 = null,
        string? usuario = null)
    {
        var perfil = new Perfile(
            userId,
            TipoPerfilEnum.Contratista,
            nombre,
            apellido,
            email,
            telefono1,
            telefono2,
            usuario);

        perfil.RaiseDomainEvent(new PerfilCreadoEvent(
            perfil.PerfilId,
            userId,
            TipoPerfilEnum.Contratista,
            nombre,
            apellido,
            email));

        return perfil;
    }

    #endregion

    #region Domain Methods

    /// <summary>
    /// Actualiza el nombre y apellido del perfil
    /// </summary>
    public void ActualizarNombreCompleto(string nombre, string apellido)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre no puede estar vacío", nameof(nombre));

        if (nombre.Length > 20)
            throw new ArgumentException("El nombre no puede exceder 20 caracteres", nameof(nombre));

        if (string.IsNullOrWhiteSpace(apellido))
            throw new ArgumentException("El apellido no puede estar vacío", nameof(apellido));

        if (apellido.Length > 50)
            throw new ArgumentException("El apellido no puede exceder 50 caracteres", nameof(apellido));

        var nombreAnterior = Nombre;
        var apellidoAnterior = Apellido;

        Nombre = nombre;
        Apellido = apellido;

        RaiseDomainEvent(new PerfilActualizadoEvent(
            PerfilId,
            UserId,
            $"{nombreAnterior} {apellidoAnterior}",
            NombreCompleto));
    }

    /// <summary>
    /// Actualiza el correo electrónico del perfil
    /// </summary>
    public void ActualizarEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email no puede estar vacío", nameof(email));

        if (email.Length > 100)
            throw new ArgumentException("El email no puede exceder 100 caracteres", nameof(email));

        var emailAnterior = Email;
        Email = email;

        RaiseDomainEvent(new EmailPerfilActualizadoEvent(
            PerfilId,
            UserId,
            emailAnterior,
            email));
    }

    /// <summary>
    /// Actualiza los teléfonos del perfil
    /// </summary>
    public void ActualizarTelefonos(string? telefono1, string? telefono2)
    {
        if (!string.IsNullOrWhiteSpace(telefono1) && telefono1.Length > 20)
            throw new ArgumentException("El teléfono 1 no puede exceder 20 caracteres", nameof(telefono1));

        if (!string.IsNullOrWhiteSpace(telefono2) && telefono2.Length > 20)
            throw new ArgumentException("El teléfono 2 no puede exceder 20 caracteres", nameof(telefono2));

        Telefono1 = telefono1;
        Telefono2 = telefono2;

        RaiseDomainEvent(new TelefonosPerfilActualizadosEvent(
            PerfilId,
            UserId,
            telefono1,
            telefono2));
    }

    /// <summary>
    /// Actualiza el nombre de usuario para login
    /// </summary>
    public void ActualizarUsuario(string? usuario)
    {
        if (!string.IsNullOrWhiteSpace(usuario) && usuario.Length > 20)
            throw new ArgumentException("El usuario no puede exceder 20 caracteres", nameof(usuario));

        var usuarioAnterior = Usuario;
        Usuario = usuario;

        if (usuarioAnterior != usuario)
        {
            RaiseDomainEvent(new UsuarioPerfilActualizadoEvent(
                PerfilId,
                UserId,
                usuarioAnterior,
                usuario));
        }
    }

    /// <summary>
    /// Actualiza toda la información básica del perfil en una sola operación
    /// Réplica de LoginService.actualizarPerfil() del Legacy
    /// </summary>
    public void ActualizarInformacionBasica(
        string nombre,
        string apellido,
        string email,
        string? telefono1 = null,
        string? telefono2 = null,
        string? usuario = null)
    {
        // Validaciones
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre no puede estar vacío", nameof(nombre));
        
        if (nombre.Length > 100)
            throw new ArgumentException("El nombre no puede exceder 100 caracteres", nameof(nombre));

        if (string.IsNullOrWhiteSpace(apellido))
            throw new ArgumentException("El apellido no puede estar vacío", nameof(apellido));
        
        if (apellido.Length > 100)
            throw new ArgumentException("El apellido no puede exceder 100 caracteres", nameof(apellido));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email no puede estar vacío", nameof(email));
        
        if (email.Length > 100)
            throw new ArgumentException("El email no puede exceder 100 caracteres", nameof(email));

        if (!string.IsNullOrWhiteSpace(telefono1) && telefono1.Length > 20)
            throw new ArgumentException("El teléfono 1 no puede exceder 20 caracteres", nameof(telefono1));

        if (!string.IsNullOrWhiteSpace(telefono2) && telefono2.Length > 20)
            throw new ArgumentException("El teléfono 2 no puede exceder 20 caracteres", nameof(telefono2));

        if (!string.IsNullOrWhiteSpace(usuario) && usuario.Length > 50)
            throw new ArgumentException("El usuario no puede exceder 50 caracteres", nameof(usuario));

        // Actualizar campos
        var nombreAnterior = $"{Nombre} {Apellido}";
        Nombre = nombre;
        Apellido = apellido;
        Email = email;
        Telefono1 = telefono1;
        Telefono2 = telefono2;
        Usuario = usuario;

        // Emitir evento de actualización
        RaiseDomainEvent(new PerfilActualizadoEvent(
            PerfilId,
            UserId,
            nombreAnterior,
            NombreCompleto));
    }

    /// <summary>
    /// Verifica si el perfil tiene información de contacto completa
    /// </summary>
    public bool TieneContactoCompleto()
    {
        return !string.IsNullOrWhiteSpace(Email) &&
               !string.IsNullOrWhiteSpace(Telefono1);
    }

    /// <summary>
    /// Verifica si el perfil tiene al menos un teléfono registrado
    /// </summary>
    public bool TieneTelefono()
    {
        return !string.IsNullOrWhiteSpace(Telefono1) ||
               !string.IsNullOrWhiteSpace(Telefono2);
    }

    /// <summary>
    /// Obtiene el teléfono principal o secundario si el principal no existe
    /// </summary>
    public string? ObtenerTelefonoPrincipal()
    {
        return !string.IsNullOrWhiteSpace(Telefono1) ? Telefono1 : Telefono2;
    }

    /// <summary>
    /// Obtiene una descripción del tipo de perfil
    /// </summary>
    public string ObtenerDescripcionTipo()
    {
        return Tipo switch
        {
            TipoPerfilEnum.Empleador => "Empleador",
            TipoPerfilEnum.Contratista => "Contratista",
            _ => "Desconocido"
        };
    }

    /// <summary>
    /// Verifica si el tipo de perfil es válido
    /// </summary>
    private static bool EsTipoValido(int tipo)
    {
        return tipo == TipoPerfilEnum.Empleador ||
               tipo == TipoPerfilEnum.Contratista;
    }

    #endregion
}

/// <summary>
/// Enumeración de tipos de perfil
/// </summary>
public static class TipoPerfilEnum
{
    /// <summary>
    /// Perfil de empleador
    /// </summary>
    public const int Empleador = 1;

    /// <summary>
    /// Perfil de contratista
    /// </summary>
    public const int Contratista = 2;
}
