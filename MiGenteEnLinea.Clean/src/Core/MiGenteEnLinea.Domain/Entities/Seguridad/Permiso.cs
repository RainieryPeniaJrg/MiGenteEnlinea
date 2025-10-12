using MiGenteEnLinea.Domain.Common;
using MiGenteEnLinea.Domain.Events.Seguridad;

namespace MiGenteEnLinea.Domain.Entities.Seguridad;

/// <summary>
/// Representa los permisos de acceso de un usuario en el sistema.
/// Utiliza un sistema de atributos (flags) para permisos granulares.
/// </summary>
public class Permiso : AggregateRoot
{
    #region Properties

    /// <summary>
    /// Identificador único del permiso
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Identificador del usuario al que pertenecen los permisos
    /// </summary>
    public string UserId { get; private set; } = string.Empty;

    /// <summary>
    /// Atributos de permisos representados como flags binarios.
    /// Cada bit representa un permiso específico (lectura, escritura, eliminación, etc.)
    /// Ejemplo: 0001 = Lectura, 0010 = Escritura, 0100 = Eliminación, 1000 = Administración
    /// </summary>
    public int Atributos { get; private set; }

    /// <summary>
    /// Descripción legible de los permisos actuales (generada dinámicamente)
    /// </summary>
    public string DescripcionPermisos => ObtenerDescripcionPermisos();

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor privado para EF Core
    /// </summary>
    private Permiso() { }

    /// <summary>
    /// Constructor privado para creación controlada
    /// </summary>
    private Permiso(string userId, int atributos)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("El UserId no puede estar vacío", nameof(userId));

        if (atributos < 0)
            throw new ArgumentException("Los atributos de permisos deben ser un valor no negativo", nameof(atributos));

        UserId = userId;
        Atributos = atributos;
    }

    #endregion

    #region Factory Methods

    /// <summary>
    /// Crea un nuevo permiso para un usuario sin permisos iniciales
    /// </summary>
    public static Permiso Crear(string userId)
    {
        var permiso = new Permiso(userId, 0);
        permiso.RaiseDomainEvent(new PermisosCreadosEvent(permiso.Id, userId, 0));
        return permiso;
    }

    /// <summary>
    /// Crea un nuevo permiso para un usuario con atributos específicos
    /// </summary>
    public static Permiso Crear(string userId, int atributos)
    {
        var permiso = new Permiso(userId, atributos);
        permiso.RaiseDomainEvent(new PermisosCreadosEvent(permiso.Id, userId, atributos));
        return permiso;
    }

    /// <summary>
    /// Crea un nuevo permiso con permisos básicos de lectura
    /// </summary>
    public static Permiso CrearConPermisosBasicos(string userId)
    {
        var permiso = new Permiso(userId, PermisosFlags.Lectura);
        permiso.RaiseDomainEvent(new PermisosCreadosEvent(permiso.Id, userId, PermisosFlags.Lectura));
        return permiso;
    }

    /// <summary>
    /// Crea un nuevo permiso con permisos de administrador (todos los permisos)
    /// </summary>
    public static Permiso CrearAdministrador(string userId)
    {
        var atributos = PermisosFlags.Lectura | PermisosFlags.Escritura | 
                       PermisosFlags.Eliminacion | PermisosFlags.Administracion;
        var permiso = new Permiso(userId, atributos);
        permiso.RaiseDomainEvent(new PermisosCreadosEvent(permiso.Id, userId, atributos));
        return permiso;
    }

    #endregion

    #region Domain Methods

    /// <summary>
    /// Otorga un permiso específico al usuario
    /// </summary>
    public void OtorgarPermiso(int permiso)
    {
        if (permiso < 0)
            throw new ArgumentException("El permiso debe ser un valor no negativo", nameof(permiso));

        var atributosAnteriores = Atributos;
        Atributos |= permiso; // OR binario para agregar el permiso

        if (atributosAnteriores != Atributos)
        {
            RaiseDomainEvent(new PermisoOtorgadoEvent(Id, UserId, permiso, Atributos));
        }
    }

    /// <summary>
    /// Revoca un permiso específico del usuario
    /// </summary>
    public void RevocarPermiso(int permiso)
    {
        if (permiso < 0)
            throw new ArgumentException("El permiso debe ser un valor no negativo", nameof(permiso));

        var atributosAnteriores = Atributos;
        Atributos &= ~permiso; // AND con NOT binario para quitar el permiso

        if (atributosAnteriores != Atributos)
        {
            RaiseDomainEvent(new PermisoRevocadoEvent(Id, UserId, permiso, Atributos));
        }
    }

    /// <summary>
    /// Establece todos los permisos del usuario de una sola vez
    /// </summary>
    public void EstablecerPermisos(int atributos)
    {
        if (atributos < 0)
            throw new ArgumentException("Los atributos deben ser un valor no negativo", nameof(atributos));

        var atributosAnteriores = Atributos;
        Atributos = atributos;

        if (atributosAnteriores != Atributos)
        {
            RaiseDomainEvent(new PermisosActualizadosEvent(Id, UserId, atributosAnteriores, Atributos));
        }
    }

    /// <summary>
    /// Revoca todos los permisos del usuario
    /// </summary>
    public void RevocarTodosLosPermisos()
    {
        var atributosAnteriores = Atributos;
        Atributos = 0;

        if (atributosAnteriores != 0)
        {
            RaiseDomainEvent(new TodosLosPermisosRevocadosEvent(Id, UserId, atributosAnteriores));
        }
    }

    /// <summary>
    /// Verifica si el usuario tiene un permiso específico
    /// </summary>
    public bool TienePermiso(int permiso)
    {
        return (Atributos & permiso) == permiso;
    }

    /// <summary>
    /// Verifica si el usuario tiene al menos uno de los permisos especificados
    /// </summary>
    public bool TieneAlgunPermiso(params int[] permisos)
    {
        foreach (var permiso in permisos)
        {
            if (TienePermiso(permiso))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Verifica si el usuario tiene todos los permisos especificados
    /// </summary>
    public bool TieneTodosLosPermisos(params int[] permisos)
    {
        foreach (var permiso in permisos)
        {
            if (!TienePermiso(permiso))
                return false;
        }
        return true;
    }

    /// <summary>
    /// Verifica si el usuario tiene permisos de administrador
    /// </summary>
    public bool EsAdministrador()
    {
        return TienePermiso(PermisosFlags.Administracion);
    }

    /// <summary>
    /// Verifica si el usuario tiene algún permiso asignado
    /// </summary>
    public bool TieneAlgunPermisoAsignado()
    {
        return Atributos > 0;
    }

    /// <summary>
    /// Obtiene una descripción legible de los permisos actuales
    /// </summary>
    private string ObtenerDescripcionPermisos()
    {
        if (Atributos == 0)
            return "Sin permisos";

        var permisos = new List<string>();

        if (TienePermiso(PermisosFlags.Lectura))
            permisos.Add("Lectura");

        if (TienePermiso(PermisosFlags.Escritura))
            permisos.Add("Escritura");

        if (TienePermiso(PermisosFlags.Eliminacion))
            permisos.Add("Eliminación");

        if (TienePermiso(PermisosFlags.Administracion))
            permisos.Add("Administración");

        return string.Join(", ", permisos);
    }

    /// <summary>
    /// Obtiene el número de permisos activos
    /// </summary>
    public int ContarPermisosActivos()
    {
        int count = 0;
        int valor = Atributos;

        // Cuenta los bits activos (algoritmo Brian Kernighan)
        while (valor > 0)
        {
            valor &= valor - 1;
            count++;
        }

        return count;
    }

    #endregion
}

/// <summary>
/// Flags de permisos predefinidos usando potencias de 2 (bits)
/// </summary>
public static class PermisosFlags
{
    /// <summary>
    /// Permiso de lectura (bit 0)
    /// </summary>
    public const int Lectura = 1; // 0001

    /// <summary>
    /// Permiso de escritura/modificación (bit 1)
    /// </summary>
    public const int Escritura = 2; // 0010

    /// <summary>
    /// Permiso de eliminación (bit 2)
    /// </summary>
    public const int Eliminacion = 4; // 0100

    /// <summary>
    /// Permiso de administración completa (bit 3)
    /// </summary>
    public const int Administracion = 8; // 1000

    /// <summary>
    /// Permiso de gestión de usuarios (bit 4)
    /// </summary>
    public const int GestionUsuarios = 16; // 10000

    /// <summary>
    /// Permiso de reportes (bit 5)
    /// </summary>
    public const int Reportes = 32; // 100000

    /// <summary>
    /// Permiso de configuración del sistema (bit 6)
    /// </summary>
    public const int Configuracion = 64; // 1000000

    /// <summary>
    /// Permiso de auditoría (bit 7)
    /// </summary>
    public const int Auditoria = 128; // 10000000
}
