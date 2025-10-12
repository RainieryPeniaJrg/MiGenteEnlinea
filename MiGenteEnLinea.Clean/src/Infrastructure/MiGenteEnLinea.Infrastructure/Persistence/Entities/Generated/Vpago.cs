using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

[Keyless]
public partial class Vpago
{
    [Column("pagoID")]
    public int PagoId { get; set; }

    [Column("userID")]
    [Unicode(false)]
    public string? UserId { get; set; }

    [Column("empleadoID")]
    public int? EmpleadoId { get; set; }

    [Column("fechaRegistro", TypeName = "datetime")]
    public DateTime? FechaRegistro { get; set; }

    [Column("fechaPago", TypeName = "datetime")]
    public DateTime? FechaPago { get; set; }

    [StringLength(18)]
    [Unicode(false)]
    public string? Expr1 { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? Monto { get; set; }
}
