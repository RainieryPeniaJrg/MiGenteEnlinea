using MiGenteEnLinea.Domain.Common;
using MiGenteEnLinea.Domain.Events.Calificaciones;

namespace MiGenteEnLinea.Domain.Entities.Calificaciones;

/// <summary>
/// Entidad Calificacion - Representa las calificaciones que los empleadores dan a contratistas
/// 
/// CONTEXTO DE NEGOCIO:
/// - Un empleador califica a un contratista después de contratarlo
/// - Se evalúan 4 dimensiones: Puntualidad, Cumplimiento, Conocimientos, Recomendación
/// - Cada dimensión se califica de 1 a 5 estrellas
/// - El campo "identificacion" almacena la cédula del contratista calificado
/// - El campo "nombre" es el nombre completo del contratista (desnormalizado)
/// - El campo "tipo" indica el tipo de calificación (ej: "Contratista")
/// 
/// MAPEO CON LEGACY:
/// - Tabla: Calificaciones (nombre legacy plural)
/// - Columnas: calificacionID, fecha, userID, tipo, identificacion, nombre,
///             puntualidad, cumplimiento, conocimientos, recomendacion
/// 
/// REGLAS DE NEGOCIO:
/// - Las calificaciones son de 1 a 5 (estrellas)
/// - Un empleador puede calificar múltiples veces al mismo contratista
/// - Las calificaciones NO se pueden editar (inmutables)
/// - El promedio de calificaciones se calcula en la vista VPromedioCalificacion
/// </summary>
public sealed class Calificacion : AggregateRoot
{
    /// <summary>
    /// Identificador único de la calificación
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Fecha en que se realizó la calificación
    /// </summary>
    public DateTime Fecha { get; private set; }

    /// <summary>
    /// ID del usuario que califica (empleador)
    /// FK a Credencial.UserId
    /// </summary>
    public string EmpleadorUserId { get; private set; }

    /// <summary>
    /// Tipo de calificación (ej: "Contratista")
    /// Campo legacy, se mantiene por compatibilidad
    /// </summary>
    public string Tipo { get; private set; }

    /// <summary>
    /// Identificación (cédula) del contratista calificado
    /// </summary>
    public string ContratistaIdentificacion { get; private set; }

    /// <summary>
    /// Nombre completo del contratista (desnormalizado)
    /// Se guarda para evitar joins en queries de lectura
    /// </summary>
    public string ContratistaNombre { get; private set; }

    /// <summary>
    /// Calificación de puntualidad (1-5 estrellas)
    /// ¿Llegó a tiempo el contratista?
    /// </summary>
    public int Puntualidad { get; private set; }

    /// <summary>
    /// Calificación de cumplimiento (1-5 estrellas)
    /// ¿Cumplió con lo acordado?
    /// </summary>
    public int Cumplimiento { get; private set; }

    /// <summary>
    /// Calificación de conocimientos (1-5 estrellas)
    /// ¿Tenía las habilidades necesarias?
    /// </summary>
    public int Conocimientos { get; private set; }

    /// <summary>
    /// Calificación de recomendación (1-5 estrellas)
    /// ¿Lo recomendaría a otros empleadores?
    /// </summary>
    public int Recomendacion { get; private set; }

    // Constantes para validación
    private const int CALIFICACION_MINIMA = 1;
    private const int CALIFICACION_MAXIMA = 5;
    private const string TIPO_CONTRATISTA = "Contratista";

    /// <summary>
    /// Constructor privado para EF Core
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor
    private Calificacion() { }
#pragma warning restore CS8618

    /// <summary>
    /// Constructor privado para lógica de creación
    /// </summary>
    private Calificacion(
        string empleadorUserId,
        string contratistaIdentificacion,
        string contratistaNombre,
        int puntualidad,
        int cumplimiento,
        int conocimientos,
        int recomendacion,
        string tipo = TIPO_CONTRATISTA)
    {
        EmpleadorUserId = empleadorUserId ?? throw new ArgumentNullException(nameof(empleadorUserId));
        ContratistaIdentificacion = contratistaIdentificacion ?? throw new ArgumentNullException(nameof(contratistaIdentificacion));
        ContratistaNombre = contratistaNombre ?? throw new ArgumentNullException(nameof(contratistaNombre));
        Tipo = tipo;
        Fecha = DateTime.UtcNow;

        // Validar y asignar calificaciones
        ValidarCalificacion(puntualidad, nameof(puntualidad));
        ValidarCalificacion(cumplimiento, nameof(cumplimiento));
        ValidarCalificacion(conocimientos, nameof(conocimientos));
        ValidarCalificacion(recomendacion, nameof(recomendacion));

        Puntualidad = puntualidad;
        Cumplimiento = cumplimiento;
        Conocimientos = conocimientos;
        Recomendacion = recomendacion;
    }

    /// <summary>
    /// Factory Method: Crea una nueva calificación
    /// </summary>
    /// <param name="empleadorUserId">ID del empleador que califica</param>
    /// <param name="contratistaIdentificacion">Cédula del contratista</param>
    /// <param name="contratistaNombre">Nombre completo del contratista</param>
    /// <param name="puntualidad">Calificación de puntualidad (1-5)</param>
    /// <param name="cumplimiento">Calificación de cumplimiento (1-5)</param>
    /// <param name="conocimientos">Calificación de conocimientos (1-5)</param>
    /// <param name="recomendacion">Calificación de recomendación (1-5)</param>
    /// <returns>Nueva instancia de Calificacion</returns>
    /// <exception cref="ArgumentException">Si los datos no son válidos</exception>
    public static Calificacion Create(
        string empleadorUserId,
        string contratistaIdentificacion,
        string contratistaNombre,
        int puntualidad,
        int cumplimiento,
        int conocimientos,
        int recomendacion)
    {
        // Validaciones de negocio
        if (string.IsNullOrWhiteSpace(empleadorUserId))
            throw new ArgumentException("EmpleadorUserId es requerido", nameof(empleadorUserId));

        if (string.IsNullOrWhiteSpace(contratistaIdentificacion))
            throw new ArgumentException("ContratistaIdentificacion es requerida", nameof(contratistaIdentificacion));

        if (string.IsNullOrWhiteSpace(contratistaNombre))
            throw new ArgumentException("ContratistaNombre es requerido", nameof(contratistaNombre));

        // Validar longitudes
        if (contratistaIdentificacion.Length > 20)
            throw new ArgumentException("ContratistaIdentificacion no puede exceder 20 caracteres", nameof(contratistaIdentificacion));

        if (contratistaNombre.Length > 100)
            throw new ArgumentException("ContratistaNombre no puede exceder 100 caracteres", nameof(contratistaNombre));

        var calificacion = new Calificacion(
            empleadorUserId,
            contratistaIdentificacion,
            contratistaNombre,
            puntualidad,
            cumplimiento,
            conocimientos,
            recomendacion);

        // Levantar evento de dominio
        calificacion.RaiseDomainEvent(new CalificacionCreadaEvent(
            calificacion.Id,
            empleadorUserId,
            contratistaIdentificacion,
            contratistaNombre,
            puntualidad,
            cumplimiento,
            conocimientos,
            recomendacion,
            calificacion.ObtenerPromedioGeneral()));

        return calificacion;
    }

    /// <summary>
    /// DOMAIN METHOD: Valida que una calificación esté en el rango permitido (1-5)
    /// </summary>
    private static void ValidarCalificacion(int valor, string nombreCampo)
    {
        if (valor < CALIFICACION_MINIMA || valor > CALIFICACION_MAXIMA)
        {
            throw new ArgumentException(
                $"{nombreCampo} debe estar entre {CALIFICACION_MINIMA} y {CALIFICACION_MAXIMA}",
                nombreCampo);
        }
    }

    /// <summary>
    /// DOMAIN METHOD: Calcula el promedio general de la calificación
    /// </summary>
    /// <returns>Promedio de las 4 dimensiones (1-5)</returns>
    public decimal ObtenerPromedioGeneral()
    {
        return (Puntualidad + Cumplimiento + Conocimientos + Recomendacion) / 4.0m;
    }

    /// <summary>
    /// DOMAIN METHOD: Verifica si la calificación es excelente (promedio >= 4.5)
    /// </summary>
    public bool EsExcelente()
    {
        return ObtenerPromedioGeneral() >= 4.5m;
    }

    /// <summary>
    /// DOMAIN METHOD: Verifica si la calificación es buena (promedio >= 3.5)
    /// </summary>
    public bool EsBuena()
    {
        return ObtenerPromedioGeneral() >= 3.5m;
    }

    /// <summary>
    /// DOMAIN METHOD: Verifica si la calificación es regular (promedio >= 2.5)
    /// </summary>
    public bool EsRegular()
    {
        var promedio = ObtenerPromedioGeneral();
        return promedio >= 2.5m && promedio < 3.5m;
    }

    /// <summary>
    /// DOMAIN METHOD: Verifica si la calificación es mala (promedio < 2.5)
    /// </summary>
    public bool EsMala()
    {
        return ObtenerPromedioGeneral() < 2.5m;
    }

    /// <summary>
    /// DOMAIN METHOD: Obtiene la categoría de la calificación
    /// </summary>
    /// <returns>"Excelente", "Buena", "Regular" o "Mala"</returns>
    public string ObtenerCategoria()
    {
        if (EsExcelente()) return "Excelente";
        if (EsBuena()) return "Buena";
        if (EsRegular()) return "Regular";
        return "Mala";
    }

    /// <summary>
    /// DOMAIN METHOD: Verifica si hay unanimidad en las calificaciones
    /// (todas las dimensiones tienen la misma puntuación)
    /// </summary>
    public bool TieneUnanimidad()
    {
        return Puntualidad == Cumplimiento &&
               Cumplimiento == Conocimientos &&
               Conocimientos == Recomendacion;
    }

    /// <summary>
    /// DOMAIN METHOD: Obtiene la dimensión mejor calificada
    /// </summary>
    public string ObtenerDimensionMejorCalificada()
    {
        var max = new[] { Puntualidad, Cumplimiento, Conocimientos, Recomendacion }.Max();

        if (Puntualidad == max) return nameof(Puntualidad);
        if (Cumplimiento == max) return nameof(Cumplimiento);
        if (Conocimientos == max) return nameof(Conocimientos);
        return nameof(Recomendacion);
    }

    /// <summary>
    /// DOMAIN METHOD: Obtiene la dimensión peor calificada
    /// </summary>
    public string ObtenerDimensionPeorCalificada()
    {
        var min = new[] { Puntualidad, Cumplimiento, Conocimientos, Recomendacion }.Min();

        if (Puntualidad == min) return nameof(Puntualidad);
        if (Cumplimiento == min) return nameof(Cumplimiento);
        if (Conocimientos == min) return nameof(Conocimientos);
        return nameof(Recomendacion);
    }

    /// <summary>
    /// DOMAIN METHOD: Verifica si el empleador recomendaría al contratista
    /// (recomendación >= 4)
    /// </summary>
    public bool LoRecomendaria()
    {
        return Recomendacion >= 4;
    }

    /// <summary>
    /// DOMAIN METHOD: Calcula desviación estándar de las calificaciones
    /// Útil para detectar inconsistencias en la evaluación
    /// </summary>
    public double CalcularDesviacionEstandar()
    {
        var promedio = (double)ObtenerPromedioGeneral();
        var calificaciones = new[] { (double)Puntualidad, Cumplimiento, Conocimientos, Recomendacion };
        var sumaCuadrados = calificaciones.Sum(c => Math.Pow(c - promedio, 2));
        return Math.Sqrt(sumaCuadrados / 4);
    }

    /// <summary>
    /// DOMAIN METHOD: Verifica si la calificación es consistente
    /// (desviación estándar baja, indica coherencia en la evaluación)
    /// </summary>
    public bool EsConsistente()
    {
        return CalcularDesviacionEstandar() <= 1.0; // Desviación <= 1 es consistente
    }

    /// <summary>
    /// DOMAIN METHOD: Obtiene un resumen textual de la calificación
    /// </summary>
    public string ObtenerResumen()
    {
        var promedio = ObtenerPromedioGeneral();
        var categoria = ObtenerCategoria();
        var consistencia = EsConsistente() ? "consistente" : "inconsistente";

        return $"Calificación {categoria} ({promedio:F2}/5.00) - Evaluación {consistencia}";
    }
}
