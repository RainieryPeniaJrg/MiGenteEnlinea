using MiGenteEnLinea.Domain.Common;
using MiGenteEnLinea.Domain.Events.Catalogos;

namespace MiGenteEnLinea.Domain.Entities.Catalogos;

/// <summary>
/// Representa un sector económico o industrial en el que operan los empleadores.
/// Es una entidad de catálogo que clasifica las empresas por tipo de industria.
/// Ejemplos: Tecnología, Construcción, Salud, Educación, Comercio, Manufactura, etc.
/// </summary>
public sealed class Sector : AggregateRoot
{
    /// <summary>
    /// Identificador único del sector
    /// </summary>
    public int SectorId { get; private set; }

    /// <summary>
    /// Nombre del sector económico.
    /// Ejemplos: "Tecnología", "Construcción", "Salud", "Comercio", "Servicios Financieros"
    /// </summary>
    public string Nombre { get; private set; } = string.Empty;

    /// <summary>
    /// Código abreviado del sector (opcional).
    /// Útil para reportes y clasificaciones estándar.
    /// Ejemplos: "TEC", "CONST", "SAL", "EDU"
    /// </summary>
    public string? Codigo { get; private set; }

    /// <summary>
    /// Descripción detallada del sector (opcional).
    /// Proporciona más contexto sobre qué industrias incluye este sector.
    /// </summary>
    public string? Descripcion { get; private set; }

    /// <summary>
    /// Indica si el sector está activo y disponible para ser seleccionado por empleadores.
    /// Los sectores inactivos no se muestran en la lista de selección.
    /// </summary>
    public bool Activo { get; private set; }

    /// <summary>
    /// Orden de visualización del sector en las listas de la interfaz de usuario.
    /// Valores menores aparecen primero.
    /// </summary>
    public int Orden { get; private set; }

    /// <summary>
    /// Grupo o categoría superior del sector para agrupación jerárquica (opcional).
    /// Ejemplos: "Servicios", "Industria", "Comercio"
    /// </summary>
    public string? Grupo { get; private set; }

    // Constructor privado para EF Core
    private Sector() { }

    /// <summary>
    /// Crea un nuevo sector económico en el catálogo.
    /// </summary>
    /// <param name="nombre">Nombre del sector (máx 60 caracteres)</param>
    /// <param name="codigo">Código abreviado del sector (opcional, máx 10 caracteres)</param>
    /// <param name="descripcion">Descripción detallada (opcional, máx 500 caracteres)</param>
    /// <param name="grupo">Grupo o categoría superior (opcional, máx 100 caracteres)</param>
    /// <param name="orden">Orden de visualización (default 999)</param>
    /// <returns>Nueva instancia de Sector</returns>
    /// <exception cref="ArgumentException">Si el nombre está vacío o excede los límites</exception>
    public static Sector Create(
        string nombre,
        string? codigo = null,
        string? descripcion = null,
        string? grupo = null,
        int orden = 999)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del sector es requerido", nameof(nombre));

        if (nombre.Length > 60)
            throw new ArgumentException("El nombre no puede exceder 60 caracteres", nameof(nombre));

        if (!string.IsNullOrWhiteSpace(codigo) && codigo.Length > 10)
            throw new ArgumentException("El código no puede exceder 10 caracteres", nameof(codigo));

        if (!string.IsNullOrWhiteSpace(descripcion) && descripcion.Length > 500)
            throw new ArgumentException("La descripción no puede exceder 500 caracteres", nameof(descripcion));

        if (!string.IsNullOrWhiteSpace(grupo) && grupo.Length > 100)
            throw new ArgumentException("El grupo no puede exceder 100 caracteres", nameof(grupo));

        var sector = new Sector
        {
            Nombre = nombre.Trim(),
            Codigo = codigo?.Trim().ToUpperInvariant(),
            Descripcion = descripcion?.Trim(),
            Grupo = grupo?.Trim(),
            Orden = orden,
            Activo = true // Por defecto activo al crearse
        };

        sector.RaiseDomainEvent(new SectorCreadoEvent(
            sector.SectorId,
            sector.Nombre,
            sector.Codigo,
            sector.Grupo));

        return sector;
    }

    /// <summary>
    /// Actualiza el nombre del sector.
    /// </summary>
    /// <param name="nuevoNombre">Nuevo nombre (máx 60 caracteres)</param>
    /// <exception cref="ArgumentException">Si el nombre está vacío o excede 60 caracteres</exception>
    public void ActualizarNombre(string nuevoNombre)
    {
        if (string.IsNullOrWhiteSpace(nuevoNombre))
            throw new ArgumentException("El nombre del sector es requerido", nameof(nuevoNombre));

        if (nuevoNombre.Length > 60)
            throw new ArgumentException("El nombre no puede exceder 60 caracteres", nameof(nuevoNombre));

        var nombreAnterior = Nombre;
        Nombre = nuevoNombre.Trim();

        RaiseDomainEvent(new SectorActualizadoEvent(
            SectorId,
            nombreAnterior,
            Nombre));
    }

    /// <summary>
    /// Actualiza el código del sector.
    /// </summary>
    /// <param name="nuevoCodigo">Nuevo código (máx 10 caracteres, nullable)</param>
    public void ActualizarCodigo(string? nuevoCodigo)
    {
        if (!string.IsNullOrWhiteSpace(nuevoCodigo) && nuevoCodigo.Length > 10)
            throw new ArgumentException("El código no puede exceder 10 caracteres", nameof(nuevoCodigo));

        Codigo = nuevoCodigo?.Trim().ToUpperInvariant();
    }

    /// <summary>
    /// Actualiza la descripción del sector.
    /// </summary>
    /// <param name="nuevaDescripcion">Nueva descripción (máx 500 caracteres, nullable)</param>
    public void ActualizarDescripcion(string? nuevaDescripcion)
    {
        if (!string.IsNullOrWhiteSpace(nuevaDescripcion) && nuevaDescripcion.Length > 500)
            throw new ArgumentException("La descripción no puede exceder 500 caracteres", nameof(nuevaDescripcion));

        Descripcion = nuevaDescripcion?.Trim();
    }

    /// <summary>
    /// Actualiza el grupo del sector.
    /// </summary>
    /// <param name="nuevoGrupo">Nuevo grupo (máx 100 caracteres, nullable)</param>
    public void ActualizarGrupo(string? nuevoGrupo)
    {
        if (!string.IsNullOrWhiteSpace(nuevoGrupo) && nuevoGrupo.Length > 100)
            throw new ArgumentException("El grupo no puede exceder 100 caracteres", nameof(nuevoGrupo));

        Grupo = nuevoGrupo?.Trim();
    }

    /// <summary>
    /// Cambia el orden de visualización del sector.
    /// </summary>
    /// <param name="nuevoOrden">Nuevo valor de orden (mayor o igual a 0)</param>
    public void CambiarOrden(int nuevoOrden)
    {
        if (nuevoOrden < 0)
            throw new ArgumentException("El orden debe ser mayor o igual a 0", nameof(nuevoOrden));

        Orden = nuevoOrden;
    }

    /// <summary>
    /// Activa el sector para que esté disponible para los empleadores.
    /// </summary>
    public void Activar()
    {
        if (Activo) return; // Ya está activo

        Activo = true;
        RaiseDomainEvent(new SectorActivadoEvent(SectorId, Nombre));
    }

    /// <summary>
    /// Desactiva el sector para que no esté disponible en la selección.
    /// Los empleadores que ya lo tienen asignado no se ven afectados.
    /// </summary>
    public void Desactivar()
    {
        if (!Activo) return; // Ya está inactivo

        Activo = false;
        RaiseDomainEvent(new SectorDesactivadoEvent(SectorId, Nombre));
    }

    /// <summary>
    /// Verifica si el sector está activo y disponible.
    /// </summary>
    public bool EstaActivo() => Activo;

    /// <summary>
    /// Verifica si el sector tiene código asignado.
    /// </summary>
    public bool TieneCodigo() => !string.IsNullOrWhiteSpace(Codigo);

    /// <summary>
    /// Verifica si el sector tiene descripción asignada.
    /// </summary>
    public bool TieneDescripcion() => !string.IsNullOrWhiteSpace(Descripcion);

    /// <summary>
    /// Verifica si el sector pertenece a un grupo.
    /// </summary>
    public bool TieneGrupo() => !string.IsNullOrWhiteSpace(Grupo);

    /// <summary>
    /// Obtiene una representación completa del sector incluyendo código si existe.
    /// </summary>
    public string ObtenerNombreCompleto()
    {
        return TieneCodigo()
            ? $"[{Codigo}] {Nombre}"
            : Nombre;
    }

    /// <summary>
    /// Obtiene una descripción completa del sector incluyendo grupo si existe.
    /// </summary>
    public string ObtenerDescripcionCompleta()
    {
        return TieneGrupo()
            ? $"{Grupo} - {Nombre}"
            : Nombre;
    }
}
