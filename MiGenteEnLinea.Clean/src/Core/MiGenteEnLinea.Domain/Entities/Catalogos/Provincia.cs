using MiGenteEnLinea.Domain.Common;
using MiGenteEnLinea.Domain.Events.Catalogos;

namespace MiGenteEnLinea.Domain.Entities.Catalogos;

/// <summary>
/// Representa una provincia de la República Dominicana.
/// Catálogo de las 32 provincias del país para uso en direcciones y filtros geográficos.
/// </summary>
public class Provincia : AggregateRoot
{
    #region Properties

    /// <summary>
    /// Identificador único de la provincia
    /// </summary>
    public int ProvinciaId { get; private set; }

    /// <summary>
    /// Nombre de la provincia
    /// </summary>
    public string Nombre { get; private set; } = string.Empty;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor privado para EF Core
    /// </summary>
    private Provincia() { }

    /// <summary>
    /// Constructor privado para creación controlada
    /// </summary>
    private Provincia(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre de la provincia no puede estar vacío", nameof(nombre));

        if (nombre.Length > 50)
            throw new ArgumentException("El nombre de la provincia no puede exceder 50 caracteres", nameof(nombre));

        Nombre = nombre;
    }

    #endregion

    #region Factory Methods

    /// <summary>
    /// Crea una nueva provincia
    /// </summary>
    public static Provincia Crear(string nombre)
    {
        var provincia = new Provincia(nombre);
        provincia.RaiseDomainEvent(new ProvinciaCreadaEvent(provincia.ProvinciaId, nombre));
        return provincia;
    }

    #endregion

    #region Domain Methods

    /// <summary>
    /// Actualiza el nombre de la provincia
    /// </summary>
    public void ActualizarNombre(string nuevoNombre)
    {
        if (string.IsNullOrWhiteSpace(nuevoNombre))
            throw new ArgumentException("El nombre de la provincia no puede estar vacío", nameof(nuevoNombre));

        if (nuevoNombre.Length > 50)
            throw new ArgumentException("El nombre de la provincia no puede exceder 50 caracteres", nameof(nuevoNombre));

        var nombreAnterior = Nombre;
        Nombre = nuevoNombre;

        if (nombreAnterior != nuevoNombre)
        {
            RaiseDomainEvent(new ProvinciaActualizadaEvent(ProvinciaId, nombreAnterior, nuevoNombre));
        }
    }

    /// <summary>
    /// Verifica si el nombre de la provincia coincide (ignorando mayúsculas/minúsculas)
    /// </summary>
    public bool TieneNombre(string nombre)
    {
        return Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase);
    }

    #endregion
}
