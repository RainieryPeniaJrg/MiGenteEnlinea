using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

[Keyless]
public partial class Vsuscripcione
{
    [Column("suscripcionID")]
    public int SuscripcionId { get; set; }

    [Column("userID")]
    [Unicode(false)]
    public string? UserId { get; set; }

    [Column("planID")]
    public int? PlanId { get; set; }

    [Column("vencimiento")]
    public DateOnly? Vencimiento { get; set; }

    [Column("nombre")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Nombre { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ProximoPago { get; set; }

    [Column("fechaInicio")]
    public DateOnly? FechaInicio { get; set; }
}
