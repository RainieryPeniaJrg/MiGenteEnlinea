using MiGenteEnLinea.Domain.Common;
using MiGenteEnLinea.Domain.Events.Nominas;

namespace MiGenteEnLinea.Domain.Entities.Nominas;

/// <summary>
/// Entidad que representa las deducciones de la Tesorería de Seguridad Social (TSS) de República Dominicana.
/// Incluye porcentajes y montos para AFP (Administradora de Fondos de Pensiones) y ARS (Administradora de Riesgos de Salud).
/// </summary>
public sealed class DeduccionTss : AuditableEntity
{
    /// <summary>
    /// Identificador único de la deducción TSS.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Descripción del tipo de deducción (ej: "AFP - Fondo de Pensiones", "ARS - Seguro de Salud").
    /// </summary>
    public string Descripcion { get; private set; } = null!;

    /// <summary>
    /// Porcentaje de deducción aplicable según la ley dominicana (ej: 2.87% para AFP, 3.04% para ARS del empleado).
    /// </summary>
    public decimal Porcentaje { get; private set; }

    /// <summary>
    /// Indica si esta deducción está activa y debe aplicarse en los cálculos de nómina.
    /// </summary>
    public bool Activa { get; private set; }

    /// <summary>
    /// Tope máximo de salario sujeto a esta deducción (según límites establecidos por ley).
    /// Null si no aplica tope.
    /// </summary>
    public decimal? TopeSalarial { get; private set; }

    // Constructor privado para EF Core
    private DeduccionTss()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de DeduccionTss.
    /// </summary>
    /// <param name="descripcion">Descripción del tipo de deducción.</param>
    /// <param name="porcentaje">Porcentaje de deducción (valor entre 0 y 100).</param>
    /// <param name="topeSalarial">Tope salarial opcional para la deducción.</param>
    /// <returns>Instancia de DeduccionTss.</returns>
    /// <exception cref="ArgumentException">Si la descripción está vacía o el porcentaje es inválido.</exception>
    public static DeduccionTss Create(string descripcion, decimal porcentaje, decimal? topeSalarial = null)
    {
        if (string.IsNullOrWhiteSpace(descripcion))
            throw new ArgumentException("La descripción de la deducción TSS es requerida", nameof(descripcion));

        if (porcentaje < 0 || porcentaje > 100)
            throw new ArgumentException("El porcentaje debe estar entre 0 y 100", nameof(porcentaje));

        if (topeSalarial.HasValue && topeSalarial.Value <= 0)
            throw new ArgumentException("El tope salarial debe ser mayor a cero", nameof(topeSalarial));

        var deduccion = new DeduccionTss
        {
            Descripcion = descripcion.Trim(),
            Porcentaje = porcentaje,
            TopeSalarial = topeSalarial,
            Activa = true
        };

        return deduccion;
    }

    /// <summary>
    /// Calcula el monto de la deducción basado en el salario cotizable.
    /// Aplica el tope salarial si está definido.
    /// </summary>
    /// <param name="salarioCotizable">Salario base sobre el cual aplicar la deducción.</param>
    /// <returns>Monto calculado de la deducción.</returns>
    /// <exception cref="ArgumentException">Si el salario cotizable es negativo.</exception>
    /// <exception cref="InvalidOperationException">Si la deducción está inactiva.</exception>
    public decimal CalcularMonto(decimal salarioCotizable)
    {
        if (salarioCotizable < 0)
            throw new ArgumentException("El salario cotizable no puede ser negativo", nameof(salarioCotizable));

        if (!Activa)
            throw new InvalidOperationException($"La deducción '{Descripcion}' está inactiva y no puede aplicarse");

        // Aplicar tope salarial si existe
        var salarioAplicable = TopeSalarial.HasValue && salarioCotizable > TopeSalarial.Value
            ? TopeSalarial.Value
            : salarioCotizable;

        var monto = salarioAplicable * (Porcentaje / 100);

        return Math.Round(monto, 2);
    }

    /// <summary>
    /// Actualiza el porcentaje de la deducción (cuando cambia la ley).
    /// </summary>
    /// <param name="nuevoPorcentaje">Nuevo porcentaje a aplicar.</param>
    /// <exception cref="ArgumentException">Si el porcentaje es inválido.</exception>
    public void ActualizarPorcentaje(decimal nuevoPorcentaje)
    {
        if (nuevoPorcentaje < 0 || nuevoPorcentaje > 100)
            throw new ArgumentException("El porcentaje debe estar entre 0 y 100", nameof(nuevoPorcentaje));

        if (Porcentaje == nuevoPorcentaje)
            return;

        var porcentajeAnterior = Porcentaje;
        Porcentaje = nuevoPorcentaje;
    }

    /// <summary>
    /// Actualiza el tope salarial de la deducción.
    /// </summary>
    /// <param name="nuevoTope">Nuevo tope salarial (null para eliminar el tope).</param>
    /// <exception cref="ArgumentException">Si el tope es menor o igual a cero.</exception>
    public void ActualizarTopeSalarial(decimal? nuevoTope)
    {
        if (nuevoTope.HasValue && nuevoTope.Value <= 0)
            throw new ArgumentException("El tope salarial debe ser mayor a cero", nameof(nuevoTope));

        TopeSalarial = nuevoTope;
    }

    /// <summary>
    /// Activa la deducción para que pueda ser aplicada en nóminas.
    /// </summary>
    public void Activar()
    {
        if (Activa)
            return;

        Activa = true;
    }

    /// <summary>
    /// Desactiva la deducción para que no se aplique en nóminas futuras.
    /// </summary>
    public void Desactivar()
    {
        if (!Activa)
            return;

        Activa = false;
    }

    /// <summary>
    /// Valida si la deducción puede aplicarse a un salario dado.
    /// </summary>
    /// <param name="salario">Salario a validar.</param>
    /// <returns>True si la deducción puede aplicarse, False en caso contrario.</returns>
    public bool PuedeAplicarse(decimal salario)
    {
        return Activa && salario > 0;
    }

    /// <summary>
    /// Actualiza la descripción de la deducción.
    /// </summary>
    /// <param name="nuevaDescripcion">Nueva descripción.</param>
    /// <exception cref="ArgumentException">Si la descripción está vacía.</exception>
    public void ActualizarDescripcion(string nuevaDescripcion)
    {
        if (string.IsNullOrWhiteSpace(nuevaDescripcion))
            throw new ArgumentException("La descripción no puede estar vacía", nameof(nuevaDescripcion));

        Descripcion = nuevaDescripcion.Trim();
    }
}
