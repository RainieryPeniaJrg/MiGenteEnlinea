using MiGenteEnLinea.Domain.Common;
using MiGenteEnLinea.Domain.Events.Catalogos;

namespace MiGenteEnLinea.Domain.Entities.Catalogos;

/// <summary>
/// Representa un servicio que puede ser ofrecido por contratistas en la plataforma.
/// Es una entidad de catálogo que define los tipos de servicios disponibles.
/// Ejemplos: Plomería, Electricidad, Carpintería, Pintura, Jardinería, etc.
/// </summary>
public sealed class Servicio : AggregateRoot
{
    /// <summary>
    /// Identificador único del servicio
    /// </summary>
    public int ServicioId { get; private set; }

    /// <summary>
    /// Nombre o descripción del servicio.
    /// Ejemplos: "Plomería", "Electricista", "Carpintero", "Pintor"
    /// </summary>
    public string Descripcion { get; private set; } = string.Empty;

    /// <summary>
    /// ID del usuario que creó el servicio (administrador del sistema).
    /// Nullable para servicios predefinidos del sistema.
    /// </summary>
    public string? UserId { get; private set; }

    /// <summary>
    /// Indica si el servicio está activo y disponible para ser seleccionado por contratistas.
    /// Los servicios inactivos no se muestran en la lista de selección.
    /// </summary>
    public bool Activo { get; private set; }

    /// <summary>
    /// Orden de visualización del servicio en las listas de la interfaz de usuario.
    /// Valores menores aparecen primero.
    /// </summary>
    public int Orden { get; private set; }

    /// <summary>
    /// Categoría o grupo al que pertenece el servicio para facilitar la organización.
    /// Ejemplos: "Construcción", "Mantenimiento", "Diseño", "Tecnología"
    /// </summary>
    public string? Categoria { get; private set; }

    /// <summary>
    /// Icono CSS o nombre de imagen para representar el servicio visualmente.
    /// Ejemplos: "fa-wrench", "fa-paint-brush", "fa-hammer"
    /// </summary>
    public string? Icono { get; private set; }

    // Constructor privado para EF Core
    private Servicio() { }

    /// <summary>
    /// Crea un nuevo servicio en el catálogo.
    /// </summary>
    /// <param name="descripcion">Nombre del servicio (máx 250 caracteres)</param>
    /// <param name="userId">ID del usuario administrador que crea el servicio (opcional)</param>
    /// <param name="categoria">Categoría del servicio (opcional)</param>
    /// <param name="icono">Icono del servicio (opcional)</param>
    /// <param name="orden">Orden de visualización (default 999)</param>
    /// <returns>Nueva instancia de Servicio</returns>
    /// <exception cref="ArgumentException">Si la descripción está vacía o excede 250 caracteres</exception>
    public static Servicio Create(
        string descripcion,
        string? userId = null,
        string? categoria = null,
        string? icono = null,
        int orden = 999)
    {
        if (string.IsNullOrWhiteSpace(descripcion))
            throw new ArgumentException("La descripción del servicio es requerida", nameof(descripcion));

        if (descripcion.Length > 250)
            throw new ArgumentException("La descripción no puede exceder 250 caracteres", nameof(descripcion));

        if (!string.IsNullOrWhiteSpace(categoria) && categoria.Length > 100)
            throw new ArgumentException("La categoría no puede exceder 100 caracteres", nameof(categoria));

        if (!string.IsNullOrWhiteSpace(icono) && icono.Length > 50)
            throw new ArgumentException("El icono no puede exceder 50 caracteres", nameof(icono));

        var servicio = new Servicio
        {
            Descripcion = descripcion.Trim(),
            UserId = userId,
            Categoria = categoria?.Trim(),
            Icono = icono?.Trim(),
            Orden = orden,
            Activo = true // Por defecto activo al crearse
        };

        servicio.RaiseDomainEvent(new ServicioCreadoEvent(
            servicio.ServicioId,
            servicio.Descripcion,
            servicio.Categoria));

        return servicio;
    }

    /// <summary>
    /// Actualiza el nombre/descripción del servicio.
    /// </summary>
    /// <param name="nuevaDescripcion">Nueva descripción (máx 250 caracteres)</param>
    /// <exception cref="ArgumentException">Si la descripción está vacía o excede 250 caracteres</exception>
    public void ActualizarDescripcion(string nuevaDescripcion)
    {
        if (string.IsNullOrWhiteSpace(nuevaDescripcion))
            throw new ArgumentException("La descripción del servicio es requerida", nameof(nuevaDescripcion));

        if (nuevaDescripcion.Length > 250)
            throw new ArgumentException("La descripción no puede exceder 250 caracteres", nameof(nuevaDescripcion));

        var descripcionAnterior = Descripcion;
        Descripcion = nuevaDescripcion.Trim();

        RaiseDomainEvent(new ServicioActualizadoEvent(
            ServicioId,
            descripcionAnterior,
            Descripcion));
    }

    /// <summary>
    /// Actualiza la categoría del servicio.
    /// </summary>
    /// <param name="nuevaCategoria">Nueva categoría (máx 100 caracteres, nullable)</param>
    public void ActualizarCategoria(string? nuevaCategoria)
    {
        if (!string.IsNullOrWhiteSpace(nuevaCategoria) && nuevaCategoria.Length > 100)
            throw new ArgumentException("La categoría no puede exceder 100 caracteres", nameof(nuevaCategoria));

        Categoria = nuevaCategoria?.Trim();
    }

    /// <summary>
    /// Actualiza el icono del servicio.
    /// </summary>
    /// <param name="nuevoIcono">Nuevo icono (máx 50 caracteres, nullable)</param>
    public void ActualizarIcono(string? nuevoIcono)
    {
        if (!string.IsNullOrWhiteSpace(nuevoIcono) && nuevoIcono.Length > 50)
            throw new ArgumentException("El icono no puede exceder 50 caracteres", nameof(nuevoIcono));

        Icono = nuevoIcono?.Trim();
    }

    /// <summary>
    /// Cambia el orden de visualización del servicio.
    /// </summary>
    /// <param name="nuevoOrden">Nuevo valor de orden (mayor o igual a 0)</param>
    public void CambiarOrden(int nuevoOrden)
    {
        if (nuevoOrden < 0)
            throw new ArgumentException("El orden debe ser mayor o igual a 0", nameof(nuevoOrden));

        Orden = nuevoOrden;
    }

    /// <summary>
    /// Activa el servicio para que esté disponible para los contratistas.
    /// </summary>
    public void Activar()
    {
        if (Activo) return; // Ya está activo

        Activo = true;
        RaiseDomainEvent(new ServicioActivadoEvent(ServicioId, Descripcion));
    }

    /// <summary>
    /// Desactiva el servicio para que no esté disponible en la selección.
    /// Los contratistas que ya lo tienen asignado no se ven afectados.
    /// </summary>
    public void Desactivar()
    {
        if (!Activo) return; // Ya está inactivo

        Activo = false;
        RaiseDomainEvent(new ServicioDesactivadoEvent(ServicioId, Descripcion));
    }

    /// <summary>
    /// Verifica si el servicio está activo y disponible.
    /// </summary>
    public bool EstaActivo() => Activo;

    /// <summary>
    /// Verifica si el servicio tiene categoría asignada.
    /// </summary>
    public bool TieneCategoria() => !string.IsNullOrWhiteSpace(Categoria);

    /// <summary>
    /// Verifica si el servicio tiene icono asignado.
    /// </summary>
    public bool TieneIcono() => !string.IsNullOrWhiteSpace(Icono);

    /// <summary>
    /// Obtiene una descripción completa del servicio incluyendo categoría si existe.
    /// </summary>
    public string ObtenerDescripcionCompleta()
    {
        return TieneCategoria()
            ? $"{Categoria}: {Descripcion}"
            : Descripcion;
    }
}
