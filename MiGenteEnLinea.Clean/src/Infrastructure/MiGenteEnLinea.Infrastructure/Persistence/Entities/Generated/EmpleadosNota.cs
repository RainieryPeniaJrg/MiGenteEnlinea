using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

[Table("Empleados_Notas")]
public partial class EmpleadosNota
{
    [Key]
    [Column("notaID")]
    public int NotaId { get; set; }

    [Column("userID")]
    [StringLength(150)]
    [Unicode(false)]
    public string? UserId { get; set; }

    [Column("empleadoID")]
    public int? EmpleadoId { get; set; }

    [Column("fecha", TypeName = "datetime")]
    public DateTime? Fecha { get; set; }

    [Column("nota")]
    [StringLength(250)]
    [Unicode(false)]
    public string? Nota { get; set; }
}
