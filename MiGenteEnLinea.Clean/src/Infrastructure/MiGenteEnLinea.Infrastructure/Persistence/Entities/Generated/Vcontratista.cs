using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

[Keyless]
public partial class Vcontratista
{
    [Column("contratistaID")]
    public int ContratistaId { get; set; }

    [Column("fechaIngreso", TypeName = "datetime")]
    public DateTime? FechaIngreso { get; set; }

    [Column("userID")]
    [StringLength(250)]
    [Unicode(false)]
    public string? UserId { get; set; }

    [Column("titulo")]
    [StringLength(70)]
    [Unicode(false)]
    public string? Titulo { get; set; }

    [Column("tipo")]
    public int? Tipo { get; set; }

    [Column("identificacion")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Identificacion { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? Nombre { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Apellido { get; set; }

    [Column("sector")]
    [StringLength(40)]
    [Unicode(false)]
    public string? Sector { get; set; }

    [Column("experiencia")]
    public int? Experiencia { get; set; }

    [Column("presentacion")]
    [StringLength(250)]
    [Unicode(false)]
    public string? Presentacion { get; set; }

    [Column("telefono1")]
    [StringLength(16)]
    [Unicode(false)]
    public string? Telefono1 { get; set; }

    [Column("whatsapp1")]
    public bool? Whatsapp1 { get; set; }

    [Column("whatsapp2")]
    public bool? Whatsapp2 { get; set; }

    [Column("email")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Email { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; }

    [Column("provincia")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Provincia { get; set; }

    [Column("nivelNacional")]
    public bool? NivelNacional { get; set; }

    [Column("calificacion", TypeName = "decimal(10, 2)")]
    public decimal Calificacion { get; set; }

    [Column("total_registros")]
    public int TotalRegistros { get; set; }

    [Column("telefono2")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Telefono2 { get; set; }

    [Column("imagenURL")]
    [StringLength(150)]
    [Unicode(false)]
    public string? ImagenUrl { get; set; }
}
