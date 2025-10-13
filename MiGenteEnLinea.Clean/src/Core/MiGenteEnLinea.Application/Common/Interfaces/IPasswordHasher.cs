namespace MiGenteEnLinea.Application.Common.Interfaces;

/// <summary>
/// Interfaz para el servicio de hashing de contraseñas
/// </summary>
/// <remarks>
/// Implementación en Infrastructure usando BCrypt con work factor 12
/// </remarks>
public interface IPasswordHasher
{
    /// <summary>
    /// Genera un hash seguro de la contraseña usando BCrypt
    /// </summary>
    /// <param name="password">Contraseña en texto plano</param>
    /// <returns>Hash de la contraseña</returns>
    string HashPassword(string password);

    /// <summary>
    /// Verifica si una contraseña coincide con un hash
    /// </summary>
    /// <param name="password">Contraseña en texto plano</param>
    /// <param name="hashedPassword">Hash almacenado</param>
    /// <returns>True si la contraseña es correcta</returns>
    bool VerifyPassword(string password, string hashedPassword);
}
