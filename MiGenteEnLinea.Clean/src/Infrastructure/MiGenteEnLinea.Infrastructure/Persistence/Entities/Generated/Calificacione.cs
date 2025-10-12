using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

public partial class Calificacione
{
    [Key]
    [Column("calificacionID")]
    public int CalificacionId { get; set; }

    [Column("fecha", TypeName = "datetime")]
    public DateTime? Fecha { get; set; }

    [Column("userID")]
    [StringLength(250)]
    [Unicode(false)]
    public string? UserId { get; set; }

    [Column("tipo")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Tipo { get; set; }

    [Column("identificacion")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Identificacion { get; set; }

    [Column("nombre")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Nombre { get; set; }

    [Column("puntualidad")]
    public int? Puntualidad { get; set; }

    [Column("cumplimiento")]
    public int? Cumplimiento { get; set; }

    [Column("conocimientos")]
    public int? Conocimientos { get; set; }

    [Column("recomendacion")]
    public int? Recomendacion { get; set; }
}
