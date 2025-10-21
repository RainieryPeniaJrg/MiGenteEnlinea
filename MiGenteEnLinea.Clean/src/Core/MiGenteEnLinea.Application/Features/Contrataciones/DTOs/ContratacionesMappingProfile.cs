using AutoMapper;
using MiGenteEnLinea.Domain.Entities.Contrataciones;

namespace MiGenteEnLinea.Application.Features.Contrataciones.DTOs;

/// <summary>
/// Configuración de AutoMapper para Contrataciones.
/// </summary>
public class ContratacionesMappingProfile : Profile
{
    public ContratacionesMappingProfile()
    {
        // DetalleContratacion → ContratacionDto (simplificado para listados)
        CreateMap<DetalleContratacion, ContratacionDto>()
            .ForMember(dest => dest.NombreEstado, opt => opt.MapFrom(src => src.ObtenerNombreEstado()));

        // DetalleContratacion → ContratacionDetalleDto (completo para detalle)
        CreateMap<DetalleContratacion, ContratacionDetalleDto>()
            .ForMember(dest => dest.NombreEstado, opt => opt.MapFrom(src => src.ObtenerNombreEstado()))
            .ForMember(dest => dest.DuracionEstimadaDias, opt => opt.MapFrom(src => src.CalcularDuracionEstimadaDias()))
            .ForMember(dest => dest.DuracionRealDias, opt => opt.MapFrom(src => src.CalcularDuracionRealDias()))
            .ForMember(dest => dest.EstaRetrasada, opt => opt.MapFrom(src => src.EstaRetrasada()))
            .ForMember(dest => dest.PuedeSerCalificada, opt => opt.MapFrom(src => src.PuedeSerCalificada()))
            .ForMember(dest => dest.PuedeSerCancelada, opt => opt.MapFrom(src => src.PuedeSerCancelada()))
            .ForMember(dest => dest.PuedeSerModificada, opt => opt.MapFrom(src => src.PuedeSerModificada()));
    }
}
