using MiGenteEnLinea.Domain.Common;
using MiGenteEnLinea.Domain.Events.Configuracion;

namespace MiGenteEnLinea.Domain.Entities.Configuracion;

/// <summary>
/// Representa la configuración del servidor SMTP para envío de correos electrónicos.
/// Almacena credenciales y parámetros de conexión del servidor de correo.
/// </summary>
public class ConfigCorreo : AggregateRoot
{
    #region Properties

    /// <summary>
    /// Identificador único de la configuración
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Dirección de correo electrónico del remitente
    /// </summary>
    public string Email { get; private set; } = string.Empty;

    /// <summary>
    /// Contraseña del correo electrónico (debe estar encriptada)
    /// </summary>
    public string Pass { get; private set; } = string.Empty;

    /// <summary>
    /// Dirección del servidor SMTP (ejemplo: smtp.gmail.com)
    /// </summary>
    public string Servidor { get; private set; } = string.Empty;

    /// <summary>
    /// Puerto del servidor SMTP (comúnmente 587 para TLS, 465 para SSL, 25 sin encriptación)
    /// </summary>
    public int Puerto { get; private set; }

    /// <summary>
    /// Indica si la configuración está completa y lista para usar
    /// </summary>
    public bool EstaConfigurada => !string.IsNullOrWhiteSpace(Email) &&
                                   !string.IsNullOrWhiteSpace(Pass) &&
                                   !string.IsNullOrWhiteSpace(Servidor) &&
                                   Puerto > 0;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor privado para EF Core
    /// </summary>
    private ConfigCorreo() { }

    /// <summary>
    /// Constructor privado para creación controlada
    /// </summary>
    private ConfigCorreo(string email, string pass, string servidor, int puerto)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email no puede estar vacío", nameof(email));

        if (email.Length > 70)
            throw new ArgumentException("El email no puede exceder 70 caracteres", nameof(email));

        if (string.IsNullOrWhiteSpace(pass))
            throw new ArgumentException("La contraseña no puede estar vacía", nameof(pass));

        if (pass.Length > 50)
            throw new ArgumentException("La contraseña no puede exceder 50 caracteres", nameof(pass));

        if (string.IsNullOrWhiteSpace(servidor))
            throw new ArgumentException("El servidor no puede estar vacío", nameof(servidor));

        if (servidor.Length > 50)
            throw new ArgumentException("El servidor no puede exceder 50 caracteres", nameof(servidor));

        if (puerto <= 0 || puerto > 65535)
            throw new ArgumentException("El puerto debe estar entre 1 y 65535", nameof(puerto));

        Email = email;
        Pass = pass;
        Servidor = servidor;
        Puerto = puerto;
    }

    #endregion

    #region Factory Methods

    /// <summary>
    /// Crea una nueva configuración de correo para Gmail
    /// </summary>
    public static ConfigCorreo CrearGmail(string email, string pass)
    {
        var config = new ConfigCorreo(email, pass, "smtp.gmail.com", 587);
        config.RaiseDomainEvent(new ConfigCorreoCreadaEvent(config.Id, email, "smtp.gmail.com", 587));
        return config;
    }

    /// <summary>
    /// Crea una nueva configuración de correo para Outlook/Hotmail
    /// </summary>
    public static ConfigCorreo CrearOutlook(string email, string pass)
    {
        var config = new ConfigCorreo(email, pass, "smtp-mail.outlook.com", 587);
        config.RaiseDomainEvent(new ConfigCorreoCreadaEvent(config.Id, email, "smtp-mail.outlook.com", 587));
        return config;
    }

    /// <summary>
    /// Crea una nueva configuración de correo personalizada
    /// </summary>
    public static ConfigCorreo Crear(string email, string pass, string servidor, int puerto)
    {
        var config = new ConfigCorreo(email, pass, servidor, puerto);
        config.RaiseDomainEvent(new ConfigCorreoCreadaEvent(config.Id, email, servidor, puerto));
        return config;
    }

    #endregion

    #region Domain Methods

    /// <summary>
    /// Actualiza el email del remitente
    /// </summary>
    public void ActualizarEmail(string nuevoEmail)
    {
        if (string.IsNullOrWhiteSpace(nuevoEmail))
            throw new ArgumentException("El email no puede estar vacío", nameof(nuevoEmail));

        if (nuevoEmail.Length > 70)
            throw new ArgumentException("El email no puede exceder 70 caracteres", nameof(nuevoEmail));

        var emailAnterior = Email;
        Email = nuevoEmail;

        if (emailAnterior != nuevoEmail)
        {
            RaiseDomainEvent(new ConfigCorreoActualizadaEvent(Id, "Email", emailAnterior, nuevoEmail));
        }
    }

    /// <summary>
    /// Actualiza la contraseña del correo (debe estar encriptada antes de llamar este método)
    /// </summary>
    public void ActualizarPassword(string nuevaPass)
    {
        if (string.IsNullOrWhiteSpace(nuevaPass))
            throw new ArgumentException("La contraseña no puede estar vacía", nameof(nuevaPass));

        if (nuevaPass.Length > 50)
            throw new ArgumentException("La contraseña no puede exceder 50 caracteres", nameof(nuevaPass));

        Pass = nuevaPass;

        RaiseDomainEvent(new ConfigCorreoActualizadaEvent(Id, "Password", "****", "****"));
    }

    /// <summary>
    /// Actualiza el servidor SMTP
    /// </summary>
    public void ActualizarServidor(string nuevoServidor)
    {
        if (string.IsNullOrWhiteSpace(nuevoServidor))
            throw new ArgumentException("El servidor no puede estar vacío", nameof(nuevoServidor));

        if (nuevoServidor.Length > 50)
            throw new ArgumentException("El servidor no puede exceder 50 caracteres", nameof(nuevoServidor));

        var servidorAnterior = Servidor;
        Servidor = nuevoServidor;

        if (servidorAnterior != nuevoServidor)
        {
            RaiseDomainEvent(new ConfigCorreoActualizadaEvent(Id, "Servidor", servidorAnterior, nuevoServidor));
        }
    }

    /// <summary>
    /// Actualiza el puerto del servidor SMTP
    /// </summary>
    public void ActualizarPuerto(int nuevoPuerto)
    {
        if (nuevoPuerto <= 0 || nuevoPuerto > 65535)
            throw new ArgumentException("El puerto debe estar entre 1 y 65535", nameof(nuevoPuerto));

        var puertoAnterior = Puerto;
        Puerto = nuevoPuerto;

        if (puertoAnterior != nuevoPuerto)
        {
            RaiseDomainEvent(new ConfigCorreoActualizadaEvent(
                Id, 
                "Puerto", 
                puertoAnterior.ToString(), 
                nuevoPuerto.ToString()));
        }
    }

    /// <summary>
    /// Actualiza toda la configuración de una vez
    /// </summary>
    public void ActualizarConfiguracion(string email, string pass, string servidor, int puerto)
    {
        ActualizarEmail(email);
        ActualizarPassword(pass);
        ActualizarServidor(servidor);
        ActualizarPuerto(puerto);
    }

    /// <summary>
    /// Verifica si el puerto es estándar para TLS
    /// </summary>
    public bool UsaTLS()
    {
        return Puerto == 587;
    }

    /// <summary>
    /// Verifica si el puerto es estándar para SSL
    /// </summary>
    public bool UsaSSL()
    {
        return Puerto == 465;
    }

    /// <summary>
    /// Obtiene una descripción del tipo de encriptación según el puerto
    /// </summary>
    public string ObtenerTipoEncriptacion()
    {
        return Puerto switch
        {
            587 => "TLS",
            465 => "SSL",
            25 => "Sin encriptación",
            _ => "Desconocido"
        };
    }

    #endregion
}
