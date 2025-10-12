using MiGenteEnLinea.Domain.Common;
using MiGenteEnLinea.Domain.Events.Contratistas;

namespace MiGenteEnLinea.Domain.Entities.Contratistas;

/// <summary>
/// Representa la relación entre un contratista y los servicios que ofrece.
/// Es una entidad de asociación que permite a un contratista especificar qué servicios brinda,
/// con detalles adicionales específicos para cada servicio.
/// Ejemplo: Un contratista puede ofrecer "Plomería" con detalles como "Reparación de tuberías, instalación de lavamanos".
/// </summary>
public sealed class ContratistaServicio : AggregateRoot
{
    /// <summary>
    /// Identificador único de la relación contratista-servicio
    /// </summary>
    public int ServicioId { get; private set; }

    /// <summary>
    /// ID del contratista que ofrece el servicio
    /// </summary>
    public int ContratistaId { get; private set; }

    /// <summary>
    /// Detalle específico del servicio que ofrece este contratista.
    /// Permite al contratista especificar qué aspectos concretos del servicio domina.
    /// Ejemplos: 
    /// - Servicio "Electricidad" → Detalle: "Instalaciones residenciales, reparación de cortocircuitos"
    /// - Servicio "Construcción" → Detalle: "Remodelaciones, ampliaciones, acabados finos"
    /// </summary>
    public string DetalleServicio { get; private set; } = string.Empty;

    /// <summary>
    /// Indica si este servicio está activo en el perfil del contratista.
    /// Permite al contratista activar/desactivar servicios temporalmente sin eliminarlos.
    /// </summary>
    public bool Activo { get; private set; }

    /// <summary>
    /// Nivel de experiencia o experticia del contratista en este servicio específico (en años).
    /// </summary>
    public int? AniosExperiencia { get; private set; }

    /// <summary>
    /// Tarifa base o rango de precios para este servicio (opcional).
    /// Ejemplo: "RD$500-1000/hora" o "A convenir"
    /// </summary>
    public string? TarifaBase { get; private set; }

    /// <summary>
    /// Orden de prioridad del servicio en el perfil del contratista.
    /// Servicios con menor orden aparecen primero (destacados).
    /// </summary>
    public int Orden { get; private set; }

    /// <summary>
    /// Certificaciones o credenciales relacionadas con este servicio (opcional).
    /// Ejemplo: "Certificado TST, Licencia eléctrica Tipo A"
    /// </summary>
    public string? Certificaciones { get; private set; }

    // Constructor privado para EF Core
    private ContratistaServicio() { }

    /// <summary>
    /// Agrega un servicio al perfil de un contratista.
    /// </summary>
    /// <param name="contratistaId">ID del contratista</param>
    /// <param name="detalleServicio">Descripción específica del servicio (máx 250 caracteres)</param>
    /// <param name="aniosExperiencia">Años de experiencia en el servicio (opcional)</param>
    /// <param name="tarifaBase">Tarifa o rango de precios (opcional, máx 100 caracteres)</param>
    /// <param name="certificaciones">Certificaciones relacionadas (opcional, máx 500 caracteres)</param>
    /// <param name="orden">Orden de visualización (default 999)</param>
    /// <returns>Nueva instancia de ContratistaServicio</returns>
    /// <exception cref="ArgumentException">Si el detalle está vacío o excede los límites</exception>
    public static ContratistaServicio Agregar(
        int contratistaId,
        string detalleServicio,
        int? aniosExperiencia = null,
        string? tarifaBase = null,
        string? certificaciones = null,
        int orden = 999)
    {
        if (contratistaId <= 0)
            throw new ArgumentException("El ID del contratista debe ser mayor a 0", nameof(contratistaId));

        if (string.IsNullOrWhiteSpace(detalleServicio))
            throw new ArgumentException("El detalle del servicio es requerido", nameof(detalleServicio));

        if (detalleServicio.Length > 250)
            throw new ArgumentException("El detalle no puede exceder 250 caracteres", nameof(detalleServicio));

        if (aniosExperiencia.HasValue && aniosExperiencia.Value < 0)
            throw new ArgumentException("Los años de experiencia no pueden ser negativos", nameof(aniosExperiencia));

        if (!string.IsNullOrWhiteSpace(tarifaBase) && tarifaBase.Length > 100)
            throw new ArgumentException("La tarifa base no puede exceder 100 caracteres", nameof(tarifaBase));

        if (!string.IsNullOrWhiteSpace(certificaciones) && certificaciones.Length > 500)
            throw new ArgumentException("Las certificaciones no pueden exceder 500 caracteres", nameof(certificaciones));

        var contratistaServicio = new ContratistaServicio
        {
            ContratistaId = contratistaId,
            DetalleServicio = detalleServicio.Trim(),
            AniosExperiencia = aniosExperiencia,
            TarifaBase = tarifaBase?.Trim(),
            Certificaciones = certificaciones?.Trim(),
            Orden = orden,
            Activo = true // Por defecto activo al agregarse
        };

        contratistaServicio.RaiseDomainEvent(new ContratistaServicioAgregadoEvent(
            contratistaServicio.ServicioId,
            contratistaServicio.ContratistaId,
            contratistaServicio.DetalleServicio));

        return contratistaServicio;
    }

    /// <summary>
    /// Actualiza el detalle del servicio ofrecido.
    /// </summary>
    /// <param name="nuevoDetalle">Nuevo detalle (máx 250 caracteres)</param>
    public void ActualizarDetalle(string nuevoDetalle)
    {
        if (string.IsNullOrWhiteSpace(nuevoDetalle))
            throw new ArgumentException("El detalle del servicio es requerido", nameof(nuevoDetalle));

        if (nuevoDetalle.Length > 250)
            throw new ArgumentException("El detalle no puede exceder 250 caracteres", nameof(nuevoDetalle));

        DetalleServicio = nuevoDetalle.Trim();
    }

    /// <summary>
    /// Actualiza los años de experiencia en el servicio.
    /// </summary>
    /// <param name="anios">Años de experiencia (nullable)</param>
    public void ActualizarExperiencia(int? anios)
    {
        if (anios.HasValue && anios.Value < 0)
            throw new ArgumentException("Los años de experiencia no pueden ser negativos", nameof(anios));

        AniosExperiencia = anios;
    }

    /// <summary>
    /// Actualiza la tarifa base del servicio.
    /// </summary>
    /// <param name="nuevaTarifa">Nueva tarifa (máx 100 caracteres, nullable)</param>
    public void ActualizarTarifa(string? nuevaTarifa)
    {
        if (!string.IsNullOrWhiteSpace(nuevaTarifa) && nuevaTarifa.Length > 100)
            throw new ArgumentException("La tarifa base no puede exceder 100 caracteres", nameof(nuevaTarifa));

        TarifaBase = nuevaTarifa?.Trim();
    }

    /// <summary>
    /// Actualiza las certificaciones relacionadas con el servicio.
    /// </summary>
    /// <param name="nuevasCertificaciones">Nuevas certificaciones (máx 500 caracteres, nullable)</param>
    public void ActualizarCertificaciones(string? nuevasCertificaciones)
    {
        if (!string.IsNullOrWhiteSpace(nuevasCertificaciones) && nuevasCertificaciones.Length > 500)
            throw new ArgumentException("Las certificaciones no pueden exceder 500 caracteres", nameof(nuevasCertificaciones));

        Certificaciones = nuevasCertificaciones?.Trim();
    }

    /// <summary>
    /// Cambia el orden de visualización del servicio en el perfil.
    /// </summary>
    /// <param name="nuevoOrden">Nuevo orden (mayor o igual a 0)</param>
    public void CambiarOrden(int nuevoOrden)
    {
        if (nuevoOrden < 0)
            throw new ArgumentException("El orden debe ser mayor o igual a 0", nameof(nuevoOrden));

        Orden = nuevoOrden;
    }

    /// <summary>
    /// Activa el servicio en el perfil del contratista.
    /// </summary>
    public void Activar()
    {
        if (Activo) return;

        Activo = true;
        RaiseDomainEvent(new ContratistaServicioActivadoEvent(ServicioId, ContratistaId));
    }

    /// <summary>
    /// Desactiva el servicio temporalmente sin eliminarlo del perfil.
    /// </summary>
    public void Desactivar()
    {
        if (!Activo) return;

        Activo = false;
        RaiseDomainEvent(new ContratistaServicioDesactivadoEvent(ServicioId, ContratistaId));
    }

    /// <summary>
    /// Verifica si el servicio está activo.
    /// </summary>
    public bool EstaActivo() => Activo;

    /// <summary>
    /// Verifica si tiene años de experiencia especificados.
    /// </summary>
    public bool TieneExperienciaRegistrada() => AniosExperiencia.HasValue && AniosExperiencia.Value > 0;

    /// <summary>
    /// Verifica si tiene tarifa base especificada.
    /// </summary>
    public bool TieneTarifaDefinida() => !string.IsNullOrWhiteSpace(TarifaBase);

    /// <summary>
    /// Verifica si tiene certificaciones registradas.
    /// </summary>
    public bool TieneCertificaciones() => !string.IsNullOrWhiteSpace(Certificaciones);

    /// <summary>
    /// Obtiene un resumen del servicio para mostrar en el perfil.
    /// </summary>
    public string ObtenerResumen()
    {
        var resumen = DetalleServicio;

        if (TieneExperienciaRegistrada())
            resumen += $" ({AniosExperiencia} años de experiencia)";

        if (TieneTarifaDefinida())
            resumen += $" - {TarifaBase}";

        return resumen;
    }
}
