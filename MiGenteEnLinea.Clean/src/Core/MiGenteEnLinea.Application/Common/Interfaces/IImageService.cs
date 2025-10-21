namespace MiGenteEnLinea.Application.Common.Interfaces;

/// <summary>
/// Servicio para procesamiento de imágenes
/// 
/// CONTEXTO DE NEGOCIO:
/// - Procesa imágenes de perfil de usuarios
/// - Redimensiona imágenes para optimizar almacenamiento
/// - Comprime imágenes para mejorar performance
/// - Agrega watermarks a imágenes corporativas
/// 
/// TECNOLOGÍA: SixLabors.ImageSharp 3.1.5
/// </summary>
public interface IImageService
{
    /// <summary>
    /// Redimensiona una imagen manteniendo aspect ratio
    /// </summary>
    /// <param name="imageBytes">Imagen original</param>
    /// <param name="maxWidth">Ancho máximo</param>
    /// <param name="maxHeight">Alto máximo</param>
    /// <returns>Imagen redimensionada</returns>
    byte[] Resize(byte[] imageBytes, int maxWidth, int maxHeight);

    /// <summary>
    /// Comprime una imagen reduciendo calidad
    /// </summary>
    /// <param name="imageBytes">Imagen original</param>
    /// <param name="quality">Calidad (1-100)</param>
    /// <returns>Imagen comprimida</returns>
    byte[] Compress(byte[] imageBytes, int quality = 75);

    /// <summary>
    /// Convierte imagen a formato específico
    /// </summary>
    /// <param name="imageBytes">Imagen original</param>
    /// <param name="format">Formato destino (jpg, png, webp)</param>
    /// <returns>Imagen convertida</returns>
    byte[] ConvertFormat(byte[] imageBytes, string format);

    /// <summary>
    /// Agrega watermark de texto a imagen
    /// </summary>
    /// <param name="imageBytes">Imagen original</param>
    /// <param name="watermarkText">Texto del watermark</param>
    /// <param name="opacity">Opacidad (0.0-1.0)</param>
    /// <returns>Imagen con watermark</returns>
    byte[] AddWatermark(byte[] imageBytes, string watermarkText, float opacity = 0.3f);
}
