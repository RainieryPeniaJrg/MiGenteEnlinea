using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

[Table("Empleador_Recibos_Detalle_Contrataciones")]
public partial class EmpleadorRecibosDetalleContratacione
{
    [Key]
    [Column("detalleID")]
    public int DetalleId { get; set; }

    [Column("pagoID")]
    public int? PagoId { get; set; }

    [StringLength(90)]
    [Unicode(false)]
    public string? Concepto { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Monto { get; set; }

    [ForeignKey("PagoId")]
    [InverseProperty("EmpleadorRecibosDetalleContrataciones")]
    public virtual EmpleadorRecibosHeaderContratacione? Pago { get; set; }
}
