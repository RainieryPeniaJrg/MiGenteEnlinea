using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

public partial class Venta
{
    [Key]
    [Column("ventaID")]
    public int VentaId { get; set; }

    [Column("userID")]
    [Unicode(false)]
    public string? UserId { get; set; }

    [Column("fecha", TypeName = "datetime")]
    public DateTime? Fecha { get; set; }

    [Column("metodo")]
    public int? Metodo { get; set; }

    [Column("planID")]
    public int? PlanId { get; set; }

    [Column("precio", TypeName = "decimal(18, 2)")]
    public decimal? Precio { get; set; }

    [Column("comentario")]
    [Unicode(false)]
    public string? Comentario { get; set; }

    [Column("idTransaccion")]
    [StringLength(100)]
    [Unicode(false)]
    public string? IdTransaccion { get; set; }

    [Column("idempotencyKey")]
    [StringLength(100)]
    [Unicode(false)]
    public string? IdempotencyKey { get; set; }

    [Column("card")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Card { get; set; }

    [Column("ip")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Ip { get; set; }
}
