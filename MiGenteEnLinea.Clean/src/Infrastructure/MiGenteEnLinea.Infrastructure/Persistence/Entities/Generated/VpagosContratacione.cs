using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

[Keyless]
public partial class VpagosContratacione
{
    [Column("pagoID")]
    public int PagoId { get; set; }

    [Column("userID")]
    [Unicode(false)]
    public string? UserId { get; set; }

    [Column("fechaRegistro", TypeName = "datetime")]
    public DateTime? FechaRegistro { get; set; }

    [Column("fechaPago", TypeName = "datetime")]
    public DateTime? FechaPago { get; set; }

    [StringLength(18)]
    [Unicode(false)]
    public string? Expr1 { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? Monto { get; set; }

    [Column("contratacionID")]
    public int? ContratacionId { get; set; }

    [Column("detalleID")]
    public int? DetalleId { get; set; }
}
