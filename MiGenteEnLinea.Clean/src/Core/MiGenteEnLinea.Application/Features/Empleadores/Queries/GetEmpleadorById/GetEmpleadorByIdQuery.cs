using MediatR;
using MiGenteEnLinea.Application.Features.Empleadores.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleadores.Queries.GetEmpleadorById;

/// <summary>
/// Query: Obtener Empleador por EmpleadorId
/// </summary>
public record GetEmpleadorByIdQuery(int EmpleadorId) : IRequest<EmpleadorDto?>;
