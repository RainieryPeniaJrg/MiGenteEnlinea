namespace MiGenteEnLinea.Domain.Entities.Empleados;

/// <summary>
/// Entidad que representa una remuneración adicional al salario base de un empleado.
/// 
/// CONTEXTO DE NEGOCIO:
/// - Representa ingresos adicionales como bonos, comisiones, horas extras, incentivos
/// - Se asocia a un Empleado específico
/// - Pertenece a un Empleador (UserId)
/// - Se usa para cálculo de nómina completa (Salario Base + Remuneraciones)
/// </summary>
/// <remarks>
/// CARACTERÍSTICAS:
/// - Entity (no AggregateRoot): Es parte del agregado Empleado
/// - Validaciones: Descripción requerida, Monto positivo
/// - Inmutabilidad: Propiedades con private set
/// 
/// EJEMPLOS DE USO:
/// - Bono mensual por desempeño
/// - Comisión por ventas realizadas
/// - Horas extras trabajadas
/// - Incentivos por cumplimiento de metas
/// </remarks>
public sealed class Remuneracion
{
    /// <summary>
    /// ID único de la remuneración
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// ID del usuario empleador (FK a Credencial.UserId)
    /// </summary>
    public string UserId { get; private set; }

    /// <summary>
    /// ID del empleado asociado (FK a Empleado.EmpleadoId)
    /// </summary>
    public int EmpleadoId { get; private set; }

    /// <summary>
    /// Descripción del concepto de remuneración
    /// Ejemplos: "Bono mensual", "Comisión ventas", "Horas extras"
    /// </summary>
    public string Descripcion { get; private set; }

    /// <summary>
    /// Monto de la remuneración adicional
    /// </summary>
    public decimal Monto { get; private set; }

    // Constructor privado para EF Core
    private Remuneracion()
    {
        UserId = string.Empty;
        Descripcion = string.Empty;
    }

    /// <summary>
    /// Factory method para crear una nueva remuneración (DDD Pattern).
    /// </summary>
    /// <param name="userId">ID del usuario empleador</param>
    /// <param name="empleadoId">ID del empleado</param>
    /// <param name="descripcion">Descripción del concepto</param>
    /// <param name="monto">Monto de la remuneración</param>
    /// <returns>Nueva instancia de Remuneracion</returns>
    /// <exception cref="ArgumentException">Si los parámetros son inválidos</exception>
    public static Remuneracion Crear(
        string userId,
        int empleadoId,
        string descripcion,
        decimal monto)
    {
        // Validaciones de negocio
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("El UserID es requerido", nameof(userId));

        if (empleadoId <= 0)
            throw new ArgumentException("El EmpleadoId debe ser mayor a 0", nameof(empleadoId));

        if (string.IsNullOrWhiteSpace(descripcion))
            throw new ArgumentException("La descripción es requerida", nameof(descripcion));

        if (monto <= 0)
            throw new ArgumentException("El monto debe ser mayor a 0", nameof(monto));

        return new Remuneracion
        {
            UserId = userId,
            EmpleadoId = empleadoId,
            Descripcion = descripcion.Trim(),
            Monto = monto
        };
    }

    /// <summary>
    /// Actualiza los datos de la remuneración.
    /// </summary>
    /// <param name="descripcion">Nueva descripción</param>
    /// <param name="monto">Nuevo monto</param>
    public void Actualizar(string descripcion, decimal monto)
    {
        if (string.IsNullOrWhiteSpace(descripcion))
            throw new ArgumentException("La descripción es requerida", nameof(descripcion));

        if (monto <= 0)
            throw new ArgumentException("El monto debe ser mayor a 0", nameof(monto));

        Descripcion = descripcion.Trim();
        Monto = monto;
    }
}
