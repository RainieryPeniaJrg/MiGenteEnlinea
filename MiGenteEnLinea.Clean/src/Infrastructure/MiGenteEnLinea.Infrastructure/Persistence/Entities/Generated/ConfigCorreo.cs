using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

[Table("Config_Correo")]
public partial class ConfigCorreo
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("email")]
    [StringLength(70)]
    [Unicode(false)]
    public string? Email { get; set; }

    [Column("pass")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Pass { get; set; }

    [Column("servidor")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Servidor { get; set; }

    [Column("puerto")]
    public int? Puerto { get; set; }
}
