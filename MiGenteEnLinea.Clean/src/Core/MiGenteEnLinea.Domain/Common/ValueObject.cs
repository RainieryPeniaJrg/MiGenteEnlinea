namespace MiGenteEnLinea.Domain.Common;

/// <summary>
/// Objeto de valor según DDD.
/// Son inmutables y se comparan por valor, no por identidad.
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Componentes que definen la igualdad del value object
    /// </summary>
    protected abstract IEnumerable<object?> GetAtomicValues();

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var valueObject = (ValueObject)obj;
        return GetAtomicValues().SequenceEqual(valueObject.GetAtomicValues());
    }

    public override int GetHashCode()
    {
        return GetAtomicValues()
            .Where(x => x != null)
            .Aggregate(1, (current, value) => current * 23 + value!.GetHashCode());
    }

    protected static bool EqualOperator(ValueObject? left, ValueObject? right)
    {
        if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            return false;

        return ReferenceEquals(left, right) || left!.Equals(right);
    }

    protected static bool NotEqualOperator(ValueObject? left, ValueObject? right)
    {
        return !EqualOperator(left, right);
    }
}
