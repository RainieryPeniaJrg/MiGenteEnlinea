using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

public partial class Ofertante
{
    [Key]
    [Column("ofertanteID")]
    public int OfertanteId { get; set; }

    [Column("fechaPublicacion", TypeName = "datetime")]
    public DateTime? FechaPublicacion { get; set; }

    [Column("userID")]
    [Unicode(false)]
    public string? UserId { get; set; }

    [Column("habilidades")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Habilidades { get; set; }

    [Column("experiencia")]
    [StringLength(200)]
    [Unicode(false)]
    public string? Experiencia { get; set; }

    [Column("descripcion")]
    [StringLength(500)]
    [Unicode(false)]
    public string? Descripcion { get; set; }

    [Column("foto")]
    public byte[]? Foto { get; set; }
}
