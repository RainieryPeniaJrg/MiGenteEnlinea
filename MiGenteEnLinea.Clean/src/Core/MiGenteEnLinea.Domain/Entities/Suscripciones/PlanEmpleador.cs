using MiGenteEnLinea.Domain.Common;
using MiGenteEnLinea.Domain.Events.Suscripciones;

namespace MiGenteEnLinea.Domain.Entities.Suscripciones;

/// <summary>
/// Representa un plan de suscripción para empleadores con sus límites y características.
/// Los planes definen cuántos empleados puede gestionar, acceso a nómina, y retención de histórico.
/// </summary>
public sealed class PlanEmpleador : AggregateRoot
{
    /// <summary>
    /// Identificador único del plan.
    /// </summary>
    public int PlanId { get; private set; }

    /// <summary>
    /// Nombre del plan (ej: "Básico", "Premium", "Empresarial").
    /// </summary>
    public string Nombre { get; private set; } = string.Empty;

    /// <summary>
    /// Precio mensual del plan en DOP (Pesos Dominicanos).
    /// </summary>
    public decimal Precio { get; private set; }

    /// <summary>
    /// Cantidad máxima de empleados que el empleador puede gestionar con este plan.
    /// 0 = ilimitado.
    /// </summary>
    public int LimiteEmpleados { get; private set; }

    /// <summary>
    /// Meses de histórico de nóminas que se conservan (ej: 12 meses, 24 meses).
    /// 0 = ilimitado.
    /// </summary>
    public int MesesHistorico { get; private set; }

    /// <summary>
    /// Indica si el plan incluye acceso al módulo de generación de nómina.
    /// </summary>
    public bool IncluyeNomina { get; private set; }

    /// <summary>
    /// Indica si el plan está activo y disponible para compra.
    /// </summary>
    public bool Activo { get; private set; }

    // Constructor privado para EF Core
    private PlanEmpleador() { }

    /// <summary>
    /// Crea un nuevo plan de empleador.
    /// </summary>
    /// <param name="nombre">Nombre del plan</param>
    /// <param name="precio">Precio mensual en DOP</param>
    /// <param name="limiteEmpleados">Cantidad máxima de empleados (0 = ilimitado)</param>
    /// <param name="mesesHistorico">Meses de histórico (0 = ilimitado)</param>
    /// <param name="incluyeNomina">Si incluye módulo de nómina</param>
    /// <returns>Nueva instancia de PlanEmpleador</returns>
    /// <exception cref="ArgumentException">Si los parámetros no son válidos</exception>
    public static PlanEmpleador Create(
        string nombre,
        decimal precio,
        int limiteEmpleados,
        int mesesHistorico,
        bool incluyeNomina)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del plan es requerido", nameof(nombre));

        if (nombre.Length > 20)
            throw new ArgumentException("El nombre del plan no puede exceder 20 caracteres", nameof(nombre));

        if (precio < 0)
            throw new ArgumentException("El precio no puede ser negativo", nameof(precio));

        if (limiteEmpleados < 0)
            throw new ArgumentException("El límite de empleados no puede ser negativo", nameof(limiteEmpleados));

        if (mesesHistorico < 0)
            throw new ArgumentException("Los meses de histórico no pueden ser negativos", nameof(mesesHistorico));

        var plan = new PlanEmpleador
        {
            Nombre = nombre.Trim(),
            Precio = precio,
            LimiteEmpleados = limiteEmpleados,
            MesesHistorico = mesesHistorico,
            IncluyeNomina = incluyeNomina,
            Activo = true
        };

        plan.RaiseDomainEvent(new PlanEmpleadorCreadoEvent(
            plan.PlanId,
            plan.Nombre,
            plan.Precio,
            plan.LimiteEmpleados));

        return plan;
    }

    /// <summary>
    /// Actualiza la información básica del plan.
    /// </summary>
    public void ActualizarInformacion(string nombre, decimal precio)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del plan es requerido", nameof(nombre));

        if (nombre.Length > 20)
            throw new ArgumentException("El nombre del plan no puede exceder 20 caracteres", nameof(nombre));

        if (precio < 0)
            throw new ArgumentException("El precio no puede ser negativo", nameof(precio));

        var precioAnterior = Precio;
        Nombre = nombre.Trim();
        Precio = precio;

        if (precioAnterior != precio)
        {
            RaiseDomainEvent(new PrecioPlanActualizadoEvent(PlanId, Nombre, precioAnterior, precio));
        }
    }

    /// <summary>
    /// Actualiza los límites y características del plan.
    /// </summary>
    public void ActualizarCaracteristicas(int limiteEmpleados, int mesesHistorico, bool incluyeNomina)
    {
        if (limiteEmpleados < 0)
            throw new ArgumentException("El límite de empleados no puede ser negativo", nameof(limiteEmpleados));

        if (mesesHistorico < 0)
            throw new ArgumentException("Los meses de histórico no pueden ser negativos", nameof(mesesHistorico));

        LimiteEmpleados = limiteEmpleados;
        MesesHistorico = mesesHistorico;
        IncluyeNomina = incluyeNomina;
    }

    /// <summary>
    /// Activa el plan haciéndolo disponible para compra.
    /// </summary>
    public void Activar()
    {
        if (Activo)
            throw new InvalidOperationException("El plan ya está activo");

        Activo = true;
    }

    /// <summary>
    /// Desactiva el plan impidiendo nuevas compras (suscripciones existentes no se afectan).
    /// </summary>
    public void Desactivar()
    {
        if (!Activo)
            throw new InvalidOperationException("El plan ya está desactivado");

        Activo = false;
        RaiseDomainEvent(new PlanEmpleadorDesactivadoEvent(PlanId, Nombre));
    }

    /// <summary>
    /// Verifica si el plan permite gestionar la cantidad especificada de empleados.
    /// </summary>
    public bool PermiteEmpleados(int cantidadEmpleados)
    {
        if (LimiteEmpleados == 0) // Ilimitado
            return true;

        return cantidadEmpleados <= LimiteEmpleados;
    }

    /// <summary>
    /// Indica si el plan tiene límite de empleados o es ilimitado.
    /// </summary>
    public bool TieneLimiteEmpleados() => LimiteEmpleados > 0;

    /// <summary>
    /// Indica si el plan tiene límite de histórico o es ilimitado.
    /// </summary>
    public bool TieneLimiteHistorico() => MesesHistorico > 0;

    /// <summary>
    /// Obtiene la descripción completa del plan con todas sus características.
    /// </summary>
    public string ObtenerDescripcion()
    {
        var empleados = LimiteEmpleados == 0 ? "Ilimitados" : $"{LimiteEmpleados}";
        var historico = MesesHistorico == 0 ? "Ilimitado" : $"{MesesHistorico} meses";
        var nomina = IncluyeNomina ? "Sí" : "No";

        return $"{Nombre} - RD${Precio:N2}/mes | Empleados: {empleados} | Histórico: {historico} | Nómina: {nomina}";
    }

    /// <summary>
    /// Calcula el precio anual del plan (12 meses).
    /// </summary>
    public decimal CalcularPrecioAnual() => Precio * 12;

    /// <summary>
    /// Calcula el precio con descuento por cantidad de meses.
    /// </summary>
    public decimal CalcularPrecioConDescuento(int meses, decimal porcentajeDescuento)
    {
        if (meses <= 0)
            throw new ArgumentException("Los meses deben ser mayor a 0", nameof(meses));

        if (porcentajeDescuento < 0 || porcentajeDescuento > 100)
            throw new ArgumentException("El descuento debe estar entre 0 y 100", nameof(porcentajeDescuento));

        var precioTotal = Precio * meses;
        var descuento = precioTotal * (porcentajeDescuento / 100);
        return precioTotal - descuento;
    }
}
