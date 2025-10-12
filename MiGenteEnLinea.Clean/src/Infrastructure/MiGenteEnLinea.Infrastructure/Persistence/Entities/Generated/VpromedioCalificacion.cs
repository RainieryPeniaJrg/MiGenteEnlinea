using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

[Keyless]
public partial class VpromedioCalificacion
{
    [Column("identificacion")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Identificacion { get; set; }

    [Column("calificacion_promedio", TypeName = "decimal(10, 2)")]
    public decimal? CalificacionPromedio { get; set; }

    [Column("total_registros")]
    public int? TotalRegistros { get; set; }
}
