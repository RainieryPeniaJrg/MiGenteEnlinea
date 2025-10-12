using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

[Keyless]
public partial class VcontratacionesTemporale
{
    [Column("contratacionID")]
    public int ContratacionId { get; set; }

    [Column("userID")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UserId { get; set; }

    [Column("fechaRegistro", TypeName = "datetime")]
    public DateTime? FechaRegistro { get; set; }

    [Column("tipo")]
    public int? Tipo { get; set; }

    [Column("nombreComercial")]
    [StringLength(50)]
    [Unicode(false)]
    public string? NombreComercial { get; set; }

    [Column("rnc")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Rnc { get; set; }

    [Column("identificacion")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Identificacion { get; set; }

    [Column("nombre")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Nombre { get; set; }

    [Column("apellido")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Apellido { get; set; }

    [Column("alias")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Alias { get; set; }

    [Column("direccion")]
    [StringLength(250)]
    [Unicode(false)]
    public string? Direccion { get; set; }

    [Column("provincia")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Provincia { get; set; }

    [Column("municipio")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Municipio { get; set; }

    [Column("telefono1")]
    [StringLength(18)]
    [Unicode(false)]
    public string? Telefono1 { get; set; }

    [Column("telefono2")]
    [StringLength(18)]
    [Unicode(false)]
    public string? Telefono2 { get; set; }

    [Column("detalleID")]
    public int DetalleId { get; set; }

    public int? Expr1 { get; set; }

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

    [Column("composicionNombre")]
    [StringLength(101)]
    [Unicode(false)]
    public string? ComposicionNombre { get; set; }

    [Column("composicionID")]
    [StringLength(20)]
    [Unicode(false)]
    public string? ComposicionId { get; set; }

    [Column("conocimientos")]
    public int? Conocimientos { get; set; }

    [Column("puntualidad")]
    public int? Puntualidad { get; set; }

    [Column("recomendacion")]
    public int? Recomendacion { get; set; }

    [Column("cumplimiento")]
    public int? Cumplimiento { get; set; }
}
