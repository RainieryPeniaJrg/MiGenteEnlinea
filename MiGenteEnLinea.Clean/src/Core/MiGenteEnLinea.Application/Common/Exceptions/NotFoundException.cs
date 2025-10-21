namespace MiGenteEnLinea.Application.Common.Exceptions;

/// <summary>
/// Excepción lanzada cuando una entidad no se encuentra en la base de datos.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException()
        : base()
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public NotFoundException(string name, object key)
        : base($"Entidad \"{name}\" ({key}) no fue encontrada.")
    {
    }
}
