using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

public partial class Perfile
{
    [Key]
    [Column("perfilID")]
    public int PerfilId { get; set; }

    [Column("fechaCreacion", TypeName = "datetime")]
    public DateTime? FechaCreacion { get; set; }

    [Column("userID")]
    [Unicode(false)]
    public string? UserId { get; set; }

    public int? Tipo { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? Nombre { get; set; }

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

    [InverseProperty("Perfil")]
    public virtual ICollection<PerfilesInfo> PerfilesInfos { get; set; } = new List<PerfilesInfo>();
}
