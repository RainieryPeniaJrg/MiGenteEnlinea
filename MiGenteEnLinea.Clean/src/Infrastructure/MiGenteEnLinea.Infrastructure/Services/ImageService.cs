using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace MiGenteEnLinea.Infrastructure.Services;

/// <summary>
/// Implementación del servicio de procesamiento de imágenes
/// Usa SixLabors.ImageSharp para manipulación de imágenes
/// </summary>
public class ImageService : IImageService
{
    private readonly ILogger<ImageService> _logger;

    public ImageService(ILogger<ImageService> logger)
    {
        _logger = logger;
    }

    public byte[] Resize(byte[] imageBytes, int maxWidth, int maxHeight)
    {
        try
        {
            using var image = Image.Load(imageBytes);
            _logger.LogInformation("Redimensionando imagen de {Width}x{Height} a máximo {MaxWidth}x{MaxHeight}", 
                image.Width, image.Height, maxWidth, maxHeight);

            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(maxWidth, maxHeight)
            }));

            using var outputStream = new MemoryStream();
            image.SaveAsJpeg(outputStream, new JpegEncoder { Quality = 85 });
            
            var result = outputStream.ToArray();
            _logger.LogInformation("Imagen redimensionada. Tamaño original: {Original} bytes, Nuevo: {New} bytes",
                imageBytes.Length, result.Length);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al redimensionar imagen");
            throw;
        }
    }

    public byte[] Compress(byte[] imageBytes, int quality = 75)
    {
        try
        {
            _logger.LogInformation("Comprimiendo imagen con calidad {Quality}", quality);
            
            using var image = Image.Load(imageBytes);
            using var outputStream = new MemoryStream();
            
            image.SaveAsJpeg(outputStream, new JpegEncoder { Quality = quality });
            
            var result = outputStream.ToArray();
            _logger.LogInformation("Imagen comprimida. Tamaño original: {Original} bytes, Comprimido: {Compressed} bytes",
                imageBytes.Length, result.Length);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al comprimir imagen");
            throw;
        }
    }

    public byte[] ConvertFormat(byte[] imageBytes, string format)
    {
        try
        {
            _logger.LogInformation("Convirtiendo imagen a formato {Format}", format);
            
            using var image = Image.Load(imageBytes);
            using var outputStream = new MemoryStream();
            
            switch (format.ToLower())
            {
                case "jpg":
                case "jpeg":
                    image.SaveAsJpeg(outputStream);
                    break;
                case "png":
                    image.SaveAsPng(outputStream);
                    break;
                default:
                    throw new NotSupportedException($"Formato {format} no soportado");
            }
            
            return outputStream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al convertir formato de imagen");
            throw;
        }
    }

    public byte[] AddWatermark(byte[] imageBytes, string watermarkText, float opacity = 0.3f)
    {
        try
        {
            _logger.LogInformation("Agregando watermark '{Text}' con opacidad {Opacity}", watermarkText, opacity);
            
            using var image = Image.Load(imageBytes);
            
            // Note: Watermark text requires additional font libraries
            // For now, this is a placeholder implementation
            _logger.LogWarning("Watermark implementation requires additional font configuration");
            
            using var outputStream = new MemoryStream();
            image.SaveAsJpeg(outputStream);
            
            return outputStream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al agregar watermark");
            throw;
        }
    }
}
