using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

[Table("perfilesInfo")]
public partial class PerfilesInfo
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("perfilID")]
    public int? PerfilId { get; set; }

    [Column("userID")]
    [Unicode(false)]
    public string? UserId { get; set; }

    [Column("tipoIdentificacion")]
    public int? TipoIdentificacion { get; set; }

    [Column("nombreComercial")]
    [StringLength(50)]
    [Unicode(false)]
    public string? NombreComercial { get; set; }

    [Column("identificacion")]
    [StringLength(20)]
    [Unicode(false)]
    public string Identificacion { get; set; } = null!;

    [Column("direccion", TypeName = "text")]
    public string? Direccion { get; set; }

    [Column("fotoPerfil")]
    public byte[]? FotoPerfil { get; set; }

    [Column("presentacion", TypeName = "text")]
    public string? Presentacion { get; set; }

    [Column("cedulaGerente")]
    [StringLength(20)]
    [Unicode(false)]
    public string? CedulaGerente { get; set; }

    [Column("nombreGerente")]
    [StringLength(50)]
    [Unicode(false)]
    public string? NombreGerente { get; set; }

    [Column("apellidoGerente")]
    [StringLength(50)]
    [Unicode(false)]
    public string? ApellidoGerente { get; set; }

    [Column("direccionGerente")]
    [StringLength(250)]
    [Unicode(false)]
    public string? DireccionGerente { get; set; }

    [ForeignKey("PerfilId")]
    [InverseProperty("PerfilesInfos")]
    public virtual Perfile? Perfil { get; set; }
}
