using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

[Table("Planes_Contratistas")]
public partial class PlanesContratista
{
    [Key]
    [Column("planID")]
    public int PlanId { get; set; }

    [Column("nombrePlan")]
    [StringLength(50)]
    [Unicode(false)]
    public string? NombrePlan { get; set; }

    [Column("precio", TypeName = "decimal(10, 2)")]
    public decimal? Precio { get; set; }
}
