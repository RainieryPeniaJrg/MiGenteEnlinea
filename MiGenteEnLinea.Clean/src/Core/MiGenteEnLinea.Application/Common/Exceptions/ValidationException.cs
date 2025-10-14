namespace MiGenteEnLinea.Application.Common.Exceptions;

/// <summary>
/// Excepción lanzada cuando falla la validación de negocio.
/// </summary>
public class ValidationException : Exception
{
    public ValidationException()
        : base("Ocurrieron uno o más errores de validación.")
    {
    }

    public ValidationException(string message)
        : base(message)
    {
    }

    public ValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
