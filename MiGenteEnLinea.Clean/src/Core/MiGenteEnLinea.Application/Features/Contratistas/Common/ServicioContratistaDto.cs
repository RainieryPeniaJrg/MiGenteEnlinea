namespace MiGenteEnLinea.Application.Features.Contratistas.Common;

/// <summary>
/// DTO: Servicio de Contratista - Data Transfer Object para servicios
/// </summary>
/// <remarks>
/// Representa un servicio ofrecido por un contratista
/// Basado en tabla Contratistas_Servicios (Legacy)
/// </remarks>
public record ServicioContratistaDto
{
    /// <summary>
    /// ID único del servicio
    /// </summary>
    public int ServicioId { get; init; }

    /// <summary>
    /// ID del contratista que ofrece el servicio
    /// </summary>
    public int ContratistaId { get; init; }

    /// <summary>
    /// Descripción específica del servicio
    /// Ejemplo: "Instalación de tuberías, reparación de fugas"
    /// </summary>
    public string DetalleServicio { get; init; } = string.Empty;

    /// <summary>
    /// ¿Servicio activo en el perfil?
    /// </summary>
    public bool Activo { get; init; }

    /// <summary>
    /// Años de experiencia en este servicio específico
    /// </summary>
    public int? AniosExperiencia { get; init; }

    /// <summary>
    /// Tarifa base o rango de precios
    /// Ejemplo: "RD$500-1000/hora"
    /// </summary>
    public string? TarifaBase { get; init; }

    /// <summary>
    /// Orden de visualización
    /// </summary>
    public int Orden { get; init; }

    /// <summary>
    /// Certificaciones relacionadas con el servicio
    /// </summary>
    public string? Certificaciones { get; init; }
}
