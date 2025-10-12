using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

public partial class DetalleContratacione
{
    [Key]
    [Column("detalleID")]
    public int DetalleId { get; set; }

    [Column("contratacionID")]
    public int? ContratacionId { get; set; }

    [Column("descripcionCorta")]
    [StringLength(60)]
    [Unicode(false)]
    public string? DescripcionCorta { get; set; }

    [Column("descripcionAmpliada")]
    [StringLength(250)]
    [Unicode(false)]
    public string? DescripcionAmpliada { get; set; }

    [Column("fechaInicio")]
    public DateOnly? FechaInicio { get; set; }

    [Column("fechaFinal")]
    public DateOnly? FechaFinal { get; set; }

    [Column("montoAcordado", TypeName = "decimal(10, 2)")]
    public decimal? MontoAcordado { get; set; }

    [Column("esquemaPagos")]
    [StringLength(50)]
    [Unicode(false)]
    public string? EsquemaPagos { get; set; }

    [Column("estatus")]
    public int? Estatus { get; set; }

    [Column("calificado")]
    public bool? Calificado { get; set; }

    [Column("calificacionID")]
    public int? CalificacionId { get; set; }

    [ForeignKey("ContratacionId")]
    [InverseProperty("DetalleContrataciones")]
    public virtual EmpleadosTemporale? Contratacion { get; set; }
}
