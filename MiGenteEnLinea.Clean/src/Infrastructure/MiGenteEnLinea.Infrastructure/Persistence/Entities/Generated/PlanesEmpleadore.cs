using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

[Table("Planes_empleadores")]
public partial class PlanesEmpleadore
{
    [Key]
    [Column("planID")]
    public int PlanId { get; set; }

    [Column("nombre")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Nombre { get; set; }

    [Column("precio", TypeName = "decimal(18, 2)")]
    public decimal? Precio { get; set; }

    [Column("empleados")]
    public int? Empleados { get; set; }

    [Column("historico")]
    public int? Historico { get; set; }

    [Column("nomina")]
    public bool? Nomina { get; set; }
}
