using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

public partial class EmpleadosTemporale
{
    [Key]
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

    [Column("nombreRepresentante")]
    [StringLength(50)]
    [Unicode(false)]
    public string? NombreRepresentante { get; set; }

    [Column("cedulaRepresentante")]
    [StringLength(20)]
    [Unicode(false)]
    public string? CedulaRepresentante { get; set; }

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

    [Column("foto")]
    [Unicode(false)]
    public string? Foto { get; set; }

    [InverseProperty("Contratacion")]
    public virtual ICollection<DetalleContratacione> DetalleContrataciones { get; set; } = new List<DetalleContratacione>();

    [InverseProperty("Contratacion")]
    public virtual ICollection<EmpleadorRecibosHeaderContratacione> EmpleadorRecibosHeaderContrataciones { get; set; } = new List<EmpleadorRecibosHeaderContratacione>();
}
