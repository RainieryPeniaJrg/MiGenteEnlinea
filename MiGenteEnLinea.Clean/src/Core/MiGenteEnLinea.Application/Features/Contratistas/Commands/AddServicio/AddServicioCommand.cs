using MediatR;

namespace MiGenteEnLinea.Application.Features.Contratistas.Commands.AddServicio;

/// <summary>
/// Command: Agrega un nuevo servicio al perfil de un contratista
/// </summary>
/// <remarks>
/// LÓGICA LEGACY: ContratistasService.agregarServicio()
/// REPLICACIÓN: btnAgregar_Click() en index_contratista.aspx.cs
/// TABLA LEGACY: Contratistas_Servicios
/// RELACIÓN: Un contratista puede tener múltiples servicios (1:N)
/// </remarks>
/// <param name="ContratistaId">ID del contratista</param>
/// <param name="DetalleServicio">Descripción del servicio (max 250 caracteres)</param>
public record AddServicioCommand(
    int ContratistaId,
    string DetalleServicio
) : IRequest<int>; // Retorna: servicioId
