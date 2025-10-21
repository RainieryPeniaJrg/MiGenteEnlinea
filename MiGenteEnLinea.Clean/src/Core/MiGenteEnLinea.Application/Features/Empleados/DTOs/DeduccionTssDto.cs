namespace MiGenteEnLinea.Application.Features.Empleados.DTOs;

/// <summary>
/// DTO para deducción de TSS (Tesorería de la Seguridad Social).
/// Representa un catálogo de deducciones con su porcentaje correspondiente.
/// </summary>
public class DeduccionTssDto
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Porcentaje { get; set; }
}
