using MiGenteEnLinea.Domain.Common;
using MiGenteEnLinea.Domain.Events.Seguridad;

namespace MiGenteEnLinea.Domain.Entities.Seguridad;

/// <summary>
/// Representa información extendida de un perfil de usuario.
/// Contiene datos adicionales como identificación legal, foto de perfil,
/// presentación y datos del gerente/representante legal (para empresas).
/// </summary>
public class PerfilesInfo : AggregateRoot
{
    #region Properties

    /// <summary>
    /// Identificador único de la información del perfil
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Identificador del perfil al que pertenece esta información
    /// </summary>
    public int? PerfilId { get; private set; }

    /// <summary>
    /// Identificador del usuario propietario
    /// </summary>
    public string UserId { get; private set; } = string.Empty;

    /// <summary>
    /// Tipo de identificación: 1 = Cédula, 2 = Pasaporte, 3 = RNC
    /// </summary>
    public int? TipoIdentificacion { get; private set; }

    /// <summary>
    /// Nombre comercial de la empresa (si aplica)
    /// </summary>
    public string? NombreComercial { get; private set; }

    /// <summary>
    /// Número de identificación (Cédula, Pasaporte o RNC)
    /// </summary>
    public string Identificacion { get; private set; } = string.Empty;

    /// <summary>
    /// Dirección física del usuario/empresa
    /// </summary>
    public string? Direccion { get; private set; }

    /// <summary>
    /// Foto de perfil en formato binario
    /// </summary>
    public byte[]? FotoPerfil { get; private set; }

    /// <summary>
    /// Presentación o biografía del usuario/empresa
    /// </summary>
    public string? Presentacion { get; private set; }

    /// <summary>
    /// Cédula del gerente o representante legal
    /// </summary>
    public string? CedulaGerente { get; private set; }

    /// <summary>
    /// Nombre del gerente o representante legal
    /// </summary>
    public string? NombreGerente { get; private set; }

    /// <summary>
    /// Apellido del gerente o representante legal
    /// </summary>
    public string? ApellidoGerente { get; private set; }

    /// <summary>
    /// Dirección del gerente o representante legal
    /// </summary>
    public string? DireccionGerente { get; private set; }

    /// <summary>
    /// Nombre completo del gerente (calculado)
    /// </summary>
    public string? NombreCompletoGerente =>
        !string.IsNullOrWhiteSpace(NombreGerente) || !string.IsNullOrWhiteSpace(ApellidoGerente)
            ? $"{NombreGerente} {ApellidoGerente}".Trim()
            : null;

    /// <summary>
    /// Indica si el perfil tiene foto
    /// </summary>
    public bool TieneFotoPerfil => FotoPerfil != null && FotoPerfil.Length > 0;

    /// <summary>
    /// Indica si es una empresa (tiene nombre comercial)
    /// </summary>
    public bool EsEmpresa => !string.IsNullOrWhiteSpace(NombreComercial);

    /// <summary>
    /// Indica si tiene información del gerente/representante legal
    /// </summary>
    public bool TieneInformacionGerente =>
        !string.IsNullOrWhiteSpace(CedulaGerente) ||
        !string.IsNullOrWhiteSpace(NombreGerente) ||
        !string.IsNullOrWhiteSpace(ApellidoGerente);

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor privado para EF Core
    /// </summary>
    private PerfilesInfo() { }

    /// <summary>
    /// Constructor privado para creación controlada
    /// </summary>
    private PerfilesInfo(
        string userId,
        string identificacion,
        int? tipoIdentificacion = null,
        string? nombreComercial = null,
        string? direccion = null,
        string? presentacion = null)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("El UserId no puede estar vacío", nameof(userId));

        if (string.IsNullOrWhiteSpace(identificacion))
            throw new ArgumentException("La identificación no puede estar vacía", nameof(identificacion));

        if (identificacion.Length > 20)
            throw new ArgumentException("La identificación no puede exceder 20 caracteres", nameof(identificacion));

        if (!string.IsNullOrWhiteSpace(nombreComercial) && nombreComercial.Length > 50)
            throw new ArgumentException("El nombre comercial no puede exceder 50 caracteres", nameof(nombreComercial));

        UserId = userId;
        Identificacion = identificacion;
        TipoIdentificacion = tipoIdentificacion;
        NombreComercial = nombreComercial;
        Direccion = direccion;
        Presentacion = presentacion;
    }

    #endregion

    #region Factory Methods

    /// <summary>
    /// Crea información de perfil para una persona física
    /// </summary>
    public static PerfilesInfo CrearPerfilPersonaFisica(
        string userId,
        string cedula,
        string? direccion = null,
        string? presentacion = null)
    {
        var perfilInfo = new PerfilesInfo(
            userId,
            cedula,
            TipoIdentificacionEnum.Cedula,
            null,
            direccion,
            presentacion);

        perfilInfo.RaiseDomainEvent(new PerfilesInfoCreadoEvent(
            perfilInfo.Id,
            userId,
            cedula,
            TipoIdentificacionEnum.Cedula));

        return perfilInfo;
    }

    /// <summary>
    /// Crea información de perfil para una empresa
    /// </summary>
    public static PerfilesInfo CrearPerfilEmpresa(
        string userId,
        string rnc,
        string nombreComercial,
        string? direccion = null,
        string? presentacion = null)
    {
        if (string.IsNullOrWhiteSpace(nombreComercial))
            throw new ArgumentException("El nombre comercial es requerido para empresas", nameof(nombreComercial));

        var perfilInfo = new PerfilesInfo(
            userId,
            rnc,
            TipoIdentificacionEnum.RNC,
            nombreComercial,
            direccion,
            presentacion);

        perfilInfo.RaiseDomainEvent(new PerfilesInfoCreadoEvent(
            perfilInfo.Id,
            userId,
            rnc,
            TipoIdentificacionEnum.RNC));

        return perfilInfo;
    }

    #endregion

    #region Domain Methods

    /// <summary>
    /// Asocia este registro de información con un perfil específico
    /// </summary>
    public void AsociarAPerfil(int perfilId)
    {
        if (perfilId <= 0)
            throw new ArgumentException("El PerfilId debe ser mayor a cero", nameof(perfilId));

        PerfilId = perfilId;
    }

    /// <summary>
    /// Actualiza la identificación del perfil
    /// </summary>
    public void ActualizarIdentificacion(string identificacion, int? tipoIdentificacion = null)
    {
        if (string.IsNullOrWhiteSpace(identificacion))
            throw new ArgumentException("La identificación no puede estar vacía", nameof(identificacion));

        if (identificacion.Length > 20)
            throw new ArgumentException("La identificación no puede exceder 20 caracteres", nameof(identificacion));

        var identificacionAnterior = Identificacion;
        Identificacion = identificacion;
        TipoIdentificacion = tipoIdentificacion;

        if (identificacionAnterior != identificacion)
        {
            RaiseDomainEvent(new IdentificacionActualizadaEvent(
                Id,
                UserId,
                identificacionAnterior,
                identificacion,
                tipoIdentificacion));
        }
    }

    /// <summary>
    /// Actualiza el nombre comercial (para empresas)
    /// </summary>
    public void ActualizarNombreComercial(string? nombreComercial)
    {
        if (!string.IsNullOrWhiteSpace(nombreComercial) && nombreComercial.Length > 50)
            throw new ArgumentException("El nombre comercial no puede exceder 50 caracteres", nameof(nombreComercial));

        var nombreAnterior = NombreComercial;
        NombreComercial = nombreComercial;

        if (nombreAnterior != nombreComercial)
        {
            RaiseDomainEvent(new NombreComercialActualizadoEvent(
                Id,
                UserId,
                nombreAnterior,
                nombreComercial));
        }
    }

    /// <summary>
    /// Actualiza la dirección del perfil
    /// </summary>
    public void ActualizarDireccion(string? direccion)
    {
        Direccion = direccion;

        RaiseDomainEvent(new DireccionPerfilActualizadaEvent(
            Id,
            UserId,
            direccion));
    }

    /// <summary>
    /// Actualiza la presentación o biografía del perfil
    /// </summary>
    public void ActualizarPresentacion(string? presentacion)
    {
        Presentacion = presentacion;

        RaiseDomainEvent(new PresentacionPerfilActualizadaEvent(
            Id,
            UserId,
            presentacion));
    }

    /// <summary>
    /// Actualiza la foto de perfil
    /// </summary>
    public void ActualizarFotoPerfil(byte[]? fotoPerfil)
    {
        FotoPerfil = fotoPerfil;

        RaiseDomainEvent(new FotoPerfilActualizadaEvent(
            Id,
            UserId,
            fotoPerfil != null && fotoPerfil.Length > 0));
    }

    /// <summary>
    /// Elimina la foto de perfil
    /// </summary>
    public void EliminarFotoPerfil()
    {
        FotoPerfil = null;

        RaiseDomainEvent(new FotoPerfilEliminadaEvent(
            Id,
            UserId));
    }

    /// <summary>
    /// Actualiza la información del gerente o representante legal
    /// </summary>
    public void ActualizarInformacionGerente(
        string? cedulaGerente,
        string? nombreGerente,
        string? apellidoGerente,
        string? direccionGerente)
    {
        if (!string.IsNullOrWhiteSpace(cedulaGerente) && cedulaGerente.Length > 20)
            throw new ArgumentException("La cédula del gerente no puede exceder 20 caracteres", nameof(cedulaGerente));

        if (!string.IsNullOrWhiteSpace(nombreGerente) && nombreGerente.Length > 50)
            throw new ArgumentException("El nombre del gerente no puede exceder 50 caracteres", nameof(nombreGerente));

        if (!string.IsNullOrWhiteSpace(apellidoGerente) && apellidoGerente.Length > 50)
            throw new ArgumentException("El apellido del gerente no puede exceder 50 caracteres", nameof(apellidoGerente));

        if (!string.IsNullOrWhiteSpace(direccionGerente) && direccionGerente.Length > 250)
            throw new ArgumentException("La dirección del gerente no puede exceder 250 caracteres", nameof(direccionGerente));

        CedulaGerente = cedulaGerente;
        NombreGerente = nombreGerente;
        ApellidoGerente = apellidoGerente;
        DireccionGerente = direccionGerente;

        RaiseDomainEvent(new InformacionGerenteActualizadaEvent(
            Id,
            UserId,
            cedulaGerente,
            nombreGerente,
            apellidoGerente,
            direccionGerente));
    }

    /// <summary>
    /// Verifica si el perfil tiene información completa
    /// </summary>
    public bool TieneInformacionCompleta()
    {
        return !string.IsNullOrWhiteSpace(Identificacion) &&
               !string.IsNullOrWhiteSpace(Direccion) &&
               TieneInformacionBasica();
    }

    /// <summary>
    /// Verifica si el perfil tiene información básica
    /// </summary>
    public bool TieneInformacionBasica()
    {
        return !string.IsNullOrWhiteSpace(Identificacion);
    }

    /// <summary>
    /// Obtiene la descripción del tipo de identificación
    /// </summary>
    public string ObtenerDescripcionTipoIdentificacion()
    {
        return TipoIdentificacion switch
        {
            TipoIdentificacionEnum.Cedula => "Cédula",
            TipoIdentificacionEnum.Pasaporte => "Pasaporte",
            TipoIdentificacionEnum.RNC => "RNC",
            _ => "No especificado"
        };
    }

    #endregion
}

/// <summary>
/// Enumeración de tipos de identificación
/// </summary>
public static class TipoIdentificacionEnum
{
    /// <summary>
    /// Cédula de identidad
    /// </summary>
    public const int Cedula = 1;

    /// <summary>
    /// Pasaporte
    /// </summary>
    public const int Pasaporte = 2;

    /// <summary>
    /// Registro Nacional de Contribuyentes (RNC)
    /// </summary>
    public const int RNC = 3;
}
