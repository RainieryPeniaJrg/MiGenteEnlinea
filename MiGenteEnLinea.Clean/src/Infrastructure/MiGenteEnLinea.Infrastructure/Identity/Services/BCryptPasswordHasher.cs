using BCrypt.Net;
using MiGenteEnLinea.Domain.Interfaces;
using ApplicationPasswordHasher = MiGenteEnLinea.Application.Common.Interfaces.IPasswordHasher;

namespace MiGenteEnLinea.Infrastructure.Identity.Services;

/// <summary>
/// Implementación de IPasswordHasher usando BCrypt.
/// BCrypt es el estándar recomendado para hashing de contraseñas (work factor 12).
/// Implementa tanto la interfaz de Domain como de Application para compatibilidad.
/// </summary>
public sealed class BCryptPasswordHasher : IPasswordHasher, ApplicationPasswordHasher
{
    private const int WorkFactor = 12;

    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("La contraseña no puede estar vacía", nameof(password));

        return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hashedPassword))
            return false;

        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        catch
        {
            // Si el hash no es válido, retornar false
            return false;
        }
    }
}
