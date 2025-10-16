using AutoMapper;
using MiGenteEnLinea.Application.Features.Suscripciones.DTOs;
using MiGenteEnLinea.Domain.Entities.Pagos;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Mappings;

/// <summary>
/// Perfil de AutoMapper para mapear Venta a VentaDto.
/// </summary>
public class VentaMappingProfile : Profile
{
    public VentaMappingProfile()
    {
        CreateMap<Venta, VentaDto>()
            .ForMember(dest => dest.MetodoPagoTexto, opt => opt.MapFrom(src => MapMetodoPago(src.MetodoPago)))
            .ForMember(dest => dest.EstadoTexto, opt => opt.MapFrom(src => MapEstado(src.Estado)));
    }

    /// <summary>
    /// Mapea el código de método de pago a texto legible.
    /// </summary>
    /// <param name="metodoPago">Código del método de pago (1=Tarjeta, 4=Otro, 5=SinPago).</param>
    /// <returns>Descripción legible del método de pago.</returns>
    private static string MapMetodoPago(int metodoPago)
    {
        return metodoPago switch
        {
            1 => "Tarjeta de Crédito",
            4 => "Otro",
            5 => "Sin Pago",
            _ => "Desconocido"
        };
    }

    /// <summary>
    /// Mapea el código de estado de transacción a texto legible.
    /// </summary>
    /// <param name="estado">Código del estado (2=Aprobado, 3=Error, 4=Rechazado).</param>
    /// <returns>Descripción legible del estado.</returns>
    private static string MapEstado(int estado)
    {
        return estado switch
        {
            2 => "Aprobado",
            3 => "Error",
            4 => "Rechazado",
            _ => "Desconocido"
        };
    }
}
