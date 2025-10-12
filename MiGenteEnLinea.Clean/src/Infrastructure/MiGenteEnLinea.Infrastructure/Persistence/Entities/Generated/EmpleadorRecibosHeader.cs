using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

[Table("Empleador_Recibos_Header")]
public partial class EmpleadorRecibosHeader
{
    [Key]
    [Column("pagoID")]
    public int PagoId { get; set; }

    [Column("userID")]
    [Unicode(false)]
    public string? UserId { get; set; }

    [Column("empleadoID")]
    public int? EmpleadoId { get; set; }

    [Column("fechaRegistro", TypeName = "datetime")]
    public DateTime? FechaRegistro { get; set; }

    [Column("fechaPago", TypeName = "datetime")]
    public DateTime? FechaPago { get; set; }

    [Column("conceptoPago")]
    [StringLength(50)]
    [Unicode(false)]
    public string? ConceptoPago { get; set; }

    [Column("tipo")]
    public int? Tipo { get; set; }

    [ForeignKey("EmpleadoId")]
    [InverseProperty("EmpleadorRecibosHeaders")]
    public virtual Empleado? Empleado { get; set; }

    [InverseProperty("Pago")]
    public virtual ICollection<EmpleadorRecibosDetalle> EmpleadorRecibosDetalles { get; set; } = new List<EmpleadorRecibosDetalle>();
}
