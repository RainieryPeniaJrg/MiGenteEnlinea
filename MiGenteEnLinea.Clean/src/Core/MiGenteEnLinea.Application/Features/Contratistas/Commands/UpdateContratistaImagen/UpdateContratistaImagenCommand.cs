using MediatR;

namespace MiGenteEnLinea.Application.Features.Contratistas.Commands.UpdateContratistaImagen;

/// <summary>
/// Command: Actualiza la imagen de perfil de un contratista
/// </summary>
/// <remarks>
/// LÓGICA LEGACY: guardarImagen() en index_contratista.aspx.cs
/// ALMACENAMIENTO: Se espera que el ImagenUrl ya esté procesado (subido a storage)
/// NOTA: Este Command NO sube el archivo, solo guarda la URL en la base de datos
/// TODO: Crear un FileUploadService que suba a Azure Blob Storage
/// </remarks>
/// <param name="UserId">ID del usuario (identifica al contratista)</param>
/// <param name="ImagenUrl">URL de la imagen ya subida (max 150 caracteres)</param>
public record UpdateContratistaImagenCommand(
    string UserId,
    string ImagenUrl
) : IRequest;
