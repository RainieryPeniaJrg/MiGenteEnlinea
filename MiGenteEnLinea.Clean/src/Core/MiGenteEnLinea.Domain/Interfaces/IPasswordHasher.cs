namespace MiGenteEnLinea.Domain.Interfaces;

/// <summary>
/// Interface para servicios de hashing de contraseñas.
/// Abstracción para permitir múltiples implementaciones (BCrypt, Crypt legacy, etc.)
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hashea una contraseña en texto plano
    /// </summary>
    /// <param name="password">Contraseña en texto plano</param>
    /// <returns>Hash de la contraseña</returns>
    string HashPassword(string password);

    /// <summary>
    /// Verifica si una contraseña coincide con un hash
    /// </summary>
    /// <param name="password">Contraseña en texto plano</param>
    /// <param name="hashedPassword">Hash almacenado</param>
    /// <returns>True si coincide, False si no</returns>
    bool VerifyPassword(string password, string hashedPassword);
}
