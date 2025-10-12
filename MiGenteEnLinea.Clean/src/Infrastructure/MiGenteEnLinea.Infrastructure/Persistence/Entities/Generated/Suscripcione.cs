using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

public partial class Suscripcione
{
    [Key]
    [Column("suscripcionID")]
    public int SuscripcionId { get; set; }

    [Column("userID")]
    [Unicode(false)]
    public string? UserId { get; set; }

    [Column("planID")]
    public int? PlanId { get; set; }

    [Column("vencimiento")]
    public DateOnly? Vencimiento { get; set; }
}
