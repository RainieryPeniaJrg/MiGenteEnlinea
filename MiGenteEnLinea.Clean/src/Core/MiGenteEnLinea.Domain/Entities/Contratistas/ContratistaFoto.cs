using MiGenteEnLinea.Domain.Common;
using MiGenteEnLinea.Domain.Events.Contratistas;

namespace MiGenteEnLinea.Domain.Entities.Contratistas;

/// <summary>
/// Representa una fotografía en el portafolio de un contratista.
/// Permite a los contratistas mostrar su trabajo, proyectos completados, o foto de perfil.
/// </summary>
public sealed class ContratistaFoto : AggregateRoot
{
    /// <summary>
    /// Identificador único de la foto
    /// </summary>
    public int ImagenId { get; private set; }

    /// <summary>
    /// ID del contratista dueño de la foto
    /// </summary>
    public int ContratistaId { get; private set; }

    /// <summary>
    /// URL o path de la imagen almacenada.
    /// Puede ser URL absoluta (Azure Blob, AWS S3) o path relativo (almacenamiento local).
    /// </summary>
    public string ImagenUrl { get; private set; } = string.Empty;

    /// <summary>
    /// Tipo de foto para categorización.
    /// Ejemplos: "Perfil", "Portafolio", "Antes", "Después", "Certificado", "Trabajo"
    /// </summary>
    public string? TipoFoto { get; private set; }

    /// <summary>
    /// Descripción o título de la foto (opcional).
    /// Permite al contratista explicar qué muestra la imagen.
    /// Ejemplo: "Remodelación de cocina - Casa Los Prados 2024"
    /// </summary>
    public string? Descripcion { get; private set; }

    /// <summary>
    /// Orden de visualización en la galería del contratista.
    /// Valores menores aparecen primero. Útil para destacar mejores trabajos.
    /// </summary>
    public int Orden { get; private set; }

    /// <summary>
    /// Indica si la foto está activa y visible en el perfil público.
    /// Permite ocultar fotos temporalmente sin eliminarlas.
    /// </summary>
    public bool Activa { get; private set; }

    /// <summary>
    /// Indica si esta es la foto de perfil principal del contratista.
    /// Solo una foto puede ser principal a la vez.
    /// </summary>
    public bool EsPrincipal { get; private set; }

    /// <summary>
    /// Tags o etiquetas para clasificar la foto (opcional).
    /// Ejemplo: "cocina, remodelación, moderna, antes/después"
    /// </summary>
    public string? Tags { get; private set; }

    /// <summary>
    /// Fecha en que se tomó la foto o se completó el trabajo (opcional).
    /// </summary>
    public DateTime? FechaTrabajo { get; private set; }

    // Constructor privado para EF Core
    private ContratistaFoto() { }

    /// <summary>
    /// Agrega una nueva foto al portafolio del contratista.
    /// </summary>
    /// <param name="contratistaId">ID del contratista dueño</param>
    /// <param name="imagenUrl">URL o path de la imagen (máx 250 caracteres)</param>
    /// <param name="tipoFoto">Tipo de foto (opcional, máx 50 caracteres)</param>
    /// <param name="descripcion">Descripción de la foto (opcional, máx 500 caracteres)</param>
    /// <param name="tags">Tags separados por comas (opcional, máx 200 caracteres)</param>
    /// <param name="fechaTrabajo">Fecha del trabajo mostrado (opcional)</param>
    /// <param name="orden">Orden de visualización (default 999)</param>
    /// <param name="esPrincipal">Si es foto de perfil principal (default false)</param>
    /// <returns>Nueva instancia de ContratistaFoto</returns>
    public static ContratistaFoto Agregar(
        int contratistaId,
        string imagenUrl,
        string? tipoFoto = null,
        string? descripcion = null,
        string? tags = null,
        DateTime? fechaTrabajo = null,
        int orden = 999,
        bool esPrincipal = false)
    {
        if (contratistaId <= 0)
            throw new ArgumentException("El ID del contratista debe ser mayor a 0", nameof(contratistaId));

        if (string.IsNullOrWhiteSpace(imagenUrl))
            throw new ArgumentException("La URL de la imagen es requerida", nameof(imagenUrl));

        if (imagenUrl.Length > 250)
            throw new ArgumentException("La URL no puede exceder 250 caracteres", nameof(imagenUrl));

        if (!string.IsNullOrWhiteSpace(tipoFoto) && tipoFoto.Length > 50)
            throw new ArgumentException("El tipo de foto no puede exceder 50 caracteres", nameof(tipoFoto));

        if (!string.IsNullOrWhiteSpace(descripcion) && descripcion.Length > 500)
            throw new ArgumentException("La descripción no puede exceder 500 caracteres", nameof(descripcion));

        if (!string.IsNullOrWhiteSpace(tags) && tags.Length > 200)
            throw new ArgumentException("Los tags no pueden exceder 200 caracteres", nameof(tags));

        var foto = new ContratistaFoto
        {
            ContratistaId = contratistaId,
            ImagenUrl = imagenUrl.Trim(),
            TipoFoto = tipoFoto?.Trim(),
            Descripcion = descripcion?.Trim(),
            Tags = tags?.Trim(),
            FechaTrabajo = fechaTrabajo,
            Orden = orden,
            EsPrincipal = esPrincipal,
            Activa = true // Por defecto activa
        };

        foto.RaiseDomainEvent(new ContratistaFotoAgregadaEvent(
            foto.ImagenId,
            foto.ContratistaId,
            foto.ImagenUrl,
            foto.TipoFoto,
            foto.EsPrincipal));

        return foto;
    }

    /// <summary>
    /// Actualiza la URL de la imagen (útil si se cambia el sistema de almacenamiento).
    /// </summary>
    /// <param name="nuevaUrl">Nueva URL (máx 250 caracteres)</param>
    public void ActualizarUrl(string nuevaUrl)
    {
        if (string.IsNullOrWhiteSpace(nuevaUrl))
            throw new ArgumentException("La URL de la imagen es requerida", nameof(nuevaUrl));

        if (nuevaUrl.Length > 250)
            throw new ArgumentException("La URL no puede exceder 250 caracteres", nameof(nuevaUrl));

        ImagenUrl = nuevaUrl.Trim();
    }

    /// <summary>
    /// Actualiza el tipo de foto.
    /// </summary>
    /// <param name="nuevoTipo">Nuevo tipo (máx 50 caracteres, nullable)</param>
    public void ActualizarTipo(string? nuevoTipo)
    {
        if (!string.IsNullOrWhiteSpace(nuevoTipo) && nuevoTipo.Length > 50)
            throw new ArgumentException("El tipo de foto no puede exceder 50 caracteres", nameof(nuevoTipo));

        TipoFoto = nuevoTipo?.Trim();
    }

    /// <summary>
    /// Actualiza la descripción de la foto.
    /// </summary>
    /// <param name="nuevaDescripcion">Nueva descripción (máx 500 caracteres, nullable)</param>
    public void ActualizarDescripcion(string? nuevaDescripcion)
    {
        if (!string.IsNullOrWhiteSpace(nuevaDescripcion) && nuevaDescripcion.Length > 500)
            throw new ArgumentException("La descripción no puede exceder 500 caracteres", nameof(nuevaDescripcion));

        Descripcion = nuevaDescripcion?.Trim();
    }

    /// <summary>
    /// Actualiza los tags de la foto.
    /// </summary>
    /// <param name="nuevosTags">Nuevos tags separados por comas (máx 200 caracteres, nullable)</param>
    public void ActualizarTags(string? nuevosTags)
    {
        if (!string.IsNullOrWhiteSpace(nuevosTags) && nuevosTags.Length > 200)
            throw new ArgumentException("Los tags no pueden exceder 200 caracteres", nameof(nuevosTags));

        Tags = nuevosTags?.Trim();
    }

    /// <summary>
    /// Actualiza la fecha del trabajo mostrado en la foto.
    /// </summary>
    /// <param name="nuevaFecha">Nueva fecha (nullable)</param>
    public void ActualizarFechaTrabajo(DateTime? nuevaFecha)
    {
        if (nuevaFecha.HasValue && nuevaFecha.Value > DateTime.Now)
            throw new ArgumentException("La fecha del trabajo no puede ser futura", nameof(nuevaFecha));

        FechaTrabajo = nuevaFecha;
    }

    /// <summary>
    /// Cambia el orden de visualización de la foto.
    /// </summary>
    /// <param name="nuevoOrden">Nuevo orden (mayor o igual a 0)</param>
    public void CambiarOrden(int nuevoOrden)
    {
        if (nuevoOrden < 0)
            throw new ArgumentException("El orden debe ser mayor o igual a 0", nameof(nuevoOrden));

        Orden = nuevoOrden;
    }

    /// <summary>
    /// Marca esta foto como la foto de perfil principal del contratista.
    /// IMPORTANTE: Antes de llamar este método, debe desmarcar la foto principal anterior.
    /// </summary>
    public void MarcarComoPrincipal()
    {
        if (EsPrincipal) return;

        EsPrincipal = true;
        RaiseDomainEvent(new ContratistaFotoPrincipalCambiadaEvent(ImagenId, ContratistaId, ImagenUrl));
    }

    /// <summary>
    /// Desmarca esta foto como principal.
    /// </summary>
    public void DesmarcarComoPrincipal()
    {
        if (!EsPrincipal) return;

        EsPrincipal = false;
    }

    /// <summary>
    /// Activa la foto para que sea visible en el perfil público.
    /// </summary>
    public void Activar()
    {
        if (Activa) return;

        Activa = true;
        RaiseDomainEvent(new ContratistaFotoActivadaEvent(ImagenId, ContratistaId));
    }

    /// <summary>
    /// Desactiva la foto para ocultarla temporalmente sin eliminarla.
    /// </summary>
    public void Desactivar()
    {
        if (!Activa) return;

        Activa = false;
        RaiseDomainEvent(new ContratistaFotoDesactivadaEvent(ImagenId, ContratistaId));
    }

    /// <summary>
    /// Verifica si la foto está activa.
    /// </summary>
    public bool EstaActiva() => Activa;

    /// <summary>
    /// Verifica si es la foto de perfil principal.
    /// </summary>
    public bool EsFotoPrincipal() => EsPrincipal;

    /// <summary>
    /// Verifica si tiene descripción.
    /// </summary>
    public bool TieneDescripcion() => !string.IsNullOrWhiteSpace(Descripcion);

    /// <summary>
    /// Verifica si tiene tags.
    /// </summary>
    public bool TieneTags() => !string.IsNullOrWhiteSpace(Tags);

    /// <summary>
    /// Verifica si tiene fecha de trabajo registrada.
    /// </summary>
    public bool TieneFechaTrabajo() => FechaTrabajo.HasValue;

    /// <summary>
    /// Obtiene los tags como lista.
    /// </summary>
    public List<string> ObtenerTagsComoLista()
    {
        if (!TieneTags())
            return new List<string>();

        return Tags!
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();
    }

    /// <summary>
    /// Verifica si la foto contiene un tag específico.
    /// </summary>
    public bool ContieneTag(string tag)
    {
        if (string.IsNullOrWhiteSpace(tag) || !TieneTags())
            return false;

        var tags = ObtenerTagsComoLista();
        return tags.Any(t => t.Equals(tag, StringComparison.OrdinalIgnoreCase));
    }
}
