using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

[Table("Empleador_Recibos_Detalle")]
public partial class EmpleadorRecibosDetalle
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
    [InverseProperty("EmpleadorRecibosDetalles")]
    public virtual EmpleadorRecibosHeader? Pago { get; set; }
}
