using System.Text.RegularExpressions;
using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.ValueObjects;

/// <summary>
/// Value Object para Email.
/// Representa un correo electrónico válido.
/// </summary>
public sealed class Email : ValueObject
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Crea un nuevo Email. Retorna null si no es válido.
    /// </summary>
    public static Email? Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var normalizedEmail = value.Trim().ToLowerInvariant();

        if (!EmailRegex.IsMatch(normalizedEmail))
            return null;

        if (normalizedEmail.Length > 100)
            return null;

        return new Email(normalizedEmail);
    }

    /// <summary>
    /// Crea un Email sin validación (para EF Core hydration)
    /// </summary>
    public static Email CreateUnsafe(string value) => new(value);

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;
}
