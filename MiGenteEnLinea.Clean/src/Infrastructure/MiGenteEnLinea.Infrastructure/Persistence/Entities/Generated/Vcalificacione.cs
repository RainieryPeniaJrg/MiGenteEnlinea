using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

[Keyless]
public partial class Vcalificacione
{
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

    [Column("perfilID")]
    public int? PerfilId { get; set; }

    [Column("fechaCreacion", TypeName = "datetime")]
    public DateTime? FechaCreacion { get; set; }

    [Unicode(false)]
    public string? Expr1 { get; set; }

    public int? Expr2 { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? Expr3 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Apellido { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Email { get; set; }

    [Column("telefono1")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Telefono1 { get; set; }

    [Column("telefono2")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Telefono2 { get; set; }

    [Column("usuario")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Usuario { get; set; }
}
