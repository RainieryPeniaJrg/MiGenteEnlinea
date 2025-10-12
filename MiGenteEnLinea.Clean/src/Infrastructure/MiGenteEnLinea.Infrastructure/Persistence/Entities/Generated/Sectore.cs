using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

public partial class Sectore
{
    [Key]
    [Column("sectorID")]
    public int SectorId { get; set; }

    [Column("sector")]
    [StringLength(60)]
    [Unicode(false)]
    public string? Sector { get; set; }
}
