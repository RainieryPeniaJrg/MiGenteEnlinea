using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

[Table("Empleador_Recibos_Header_Contrataciones")]
public partial class EmpleadorRecibosHeaderContratacione
{
    [Key]
    [Column("pagoID")]
    public int PagoId { get; set; }

    [Column("userID")]
    [Unicode(false)]
    public string? UserId { get; set; }

    [Column("contratacionID")]
    public int? ContratacionId { get; set; }

    [Column("fechaRegistro", TypeName = "datetime")]
    public DateTime? FechaRegistro { get; set; }

    [Column("fechaPago", TypeName = "datetime")]
    public DateTime? FechaPago { get; set; }

    [Column("conceptoPago")]
    [StringLength(50)]
    [Unicode(false)]
    public string? ConceptoPago { get; set; }

    [Column("tipo")]
    public int? Tipo { get; set; }

    [ForeignKey("ContratacionId")]
    [InverseProperty("EmpleadorRecibosHeaderContrataciones")]
    public virtual EmpleadosTemporale? Contratacion { get; set; }

    [InverseProperty("Pago")]
    public virtual ICollection<EmpleadorRecibosDetalleContratacione> EmpleadorRecibosDetalleContrataciones { get; set; } = new List<EmpleadorRecibosDetalleContratacione>();
}
