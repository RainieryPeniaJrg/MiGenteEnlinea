using AutoMapper;
using MiGenteEnLinea.Application.Features.Calificaciones.DTOs;
using MiGenteEnLinea.Domain.Entities.Calificaciones;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Mappings;

/// <summary>
/// AutoMapper Profile: Mapeos de Calificaciones (4 dimensiones)
/// </summary>
public class CalificacionesMappingProfile : Profile
{
    public CalificacionesMappingProfile()
    {
        // Calificacion (Domain) → CalificacionDto (Application)
        CreateMap<Calificacion, CalificacionDto>()
            .ForMember(dest => dest.CalificacionId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.EmpleadorUserId, opt => opt.MapFrom(src => src.EmpleadorUserId))
            .ForMember(dest => dest.ContratistaIdentificacion, opt => opt.MapFrom(src => src.ContratistaIdentificacion))
            .ForMember(dest => dest.ContratistaNombre, opt => opt.MapFrom(src => src.ContratistaNombre))
            .ForMember(dest => dest.Puntualidad, opt => opt.MapFrom(src => src.Puntualidad))
            .ForMember(dest => dest.Cumplimiento, opt => opt.MapFrom(src => src.Cumplimiento))
            .ForMember(dest => dest.Conocimientos, opt => opt.MapFrom(src => src.Conocimientos))
            .ForMember(dest => dest.Recomendacion, opt => opt.MapFrom(src => src.Recomendacion))
            .ForMember(dest => dest.PromedioGeneral, opt => opt.MapFrom(src => src.ObtenerPromedioGeneral()))
            .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.ObtenerCategoria()))
            .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.Fecha));
        // Las propiedades computed (EsReciente, TiempoTranscurrido) se calculan automáticamente en el DTO getter
    }
}
