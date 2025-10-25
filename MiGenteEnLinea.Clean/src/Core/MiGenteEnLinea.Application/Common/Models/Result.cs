namespace MiGenteEnLinea.Application.Common.Models;

/// <summary>
/// Representa el resultado de una operación.
/// </summary>
public class Result
{
    protected Result(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }

    /// <summary>
    /// Indica si la operación fue exitosa.
    /// </summary>
    public bool Succeeded { get; init; }

    /// <summary>
    /// Lista de errores si la operación falló.
    /// </summary>
    public string[] Errors { get; init; }

    /// <summary>
    /// Crea un resultado exitoso.
    /// </summary>
    public static Result Success()
    {
        return new Result(true, Array.Empty<string>());
    }

    /// <summary>
    /// Crea un resultado fallido con errores.
    /// </summary>
    public static Result Failure(IEnumerable<string> errors)
    {
        return new Result(false, errors);
    }
}

/// <summary>
/// Representa el resultado de una operación con un valor de retorno.
/// </summary>
public class Result<T> : Result
{
    private Result(bool succeeded, T? value, IEnumerable<string> errors)
        : base(succeeded, errors)
    {
        Value = value;
    }

    /// <summary>
    /// Valor retornado si la operación fue exitosa.
    /// </summary>
    public T? Value { get; init; }

    /// <summary>
    /// Crea un resultado exitoso con un valor.
    /// </summary>
    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, Array.Empty<string>());
    }

    /// <summary>
    /// Crea un resultado fallido con errores.
    /// </summary>
    public new static Result<T> Failure(IEnumerable<string> errors)
    {
        return new Result<T>(false, default, errors);
    }
}
