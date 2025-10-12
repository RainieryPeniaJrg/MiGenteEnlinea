using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

public partial class Permiso
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("userID")]
    [Unicode(false)]
    public string? UserId { get; set; }

    [Column("atributos")]
    public int? Atributos { get; set; }
}
