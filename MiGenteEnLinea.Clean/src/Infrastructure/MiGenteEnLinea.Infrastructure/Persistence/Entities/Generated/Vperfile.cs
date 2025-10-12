using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

[Keyless]
public partial class Vperfile
{
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

    [Column("id")]
    public int? Id { get; set; }

    [Column("tipoIdentificacion")]
    public int? TipoIdentificacion { get; set; }

    [Column("identificacion")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Identificacion { get; set; }

    [Column("direccion", TypeName = "text")]
    public string? Direccion { get; set; }

    [Column("fotoPerfil")]
    public byte[]? FotoPerfil { get; set; }

    [Column("presentacion", TypeName = "text")]
    public string? Presentacion { get; set; }

    [Column("nombreComercial")]
    [StringLength(50)]
    [Unicode(false)]
    public string? NombreComercial { get; set; }

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
}
