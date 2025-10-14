namespace MiGenteEnLinea.Application.Features.Empleados.DTOs;

/// <summary>
/// DTO para una remuneración extra de un empleado.
/// Representa uno de los 3 slots de remuneraciones disponibles.
/// </summary>
public class RemuneracionDto
{
    /// <summary>
    /// Número del slot de remuneración (1, 2 o 3).
    /// </summary>
    public int Numero { get; set; }

    /// <summary>
    /// Descripción de la remuneración (ej: "Bono productividad", "Comisión ventas").
    /// Puede ser null si el slot está vacío.
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Monto de la remuneración extra.
    /// Puede ser null si el slot está vacío.
    /// </summary>
    public decimal? Monto { get; set; }

    /// <summary>
    /// Indica si el slot tiene una remuneración activa.
    /// </summary>
    public bool TieneValor => !string.IsNullOrWhiteSpace(Descripcion) && Monto.HasValue;
}
