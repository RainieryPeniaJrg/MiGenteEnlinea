using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

public partial class Servicio
{
    [Key]
    [Column("servicioID")]
    public int ServicioId { get; set; }

    [Column("userID")]
    [StringLength(250)]
    [Unicode(false)]
    public string? UserId { get; set; }

    [Column("descripcion")]
    [StringLength(250)]
    [Unicode(false)]
    public string? Descripcion { get; set; }
}
