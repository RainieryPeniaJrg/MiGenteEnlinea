using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

[Table("Contratistas_Fotos")]
public partial class ContratistasFoto
{
    [Key]
    [Column("imagenID")]
    public int ImagenId { get; set; }

    [Column("contratistaID")]
    public int? ContratistaId { get; set; }

    [Column("imagenURL")]
    [StringLength(250)]
    [Unicode(false)]
    public string? ImagenUrl { get; set; }

    [ForeignKey("ContratistaId")]
    [InverseProperty("ContratistasFotos")]
    public virtual Contratista? Contratista { get; set; }
}
