using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

[Table("Contratistas_Servicios")]
public partial class ContratistasServicio
{
    [Key]
    [Column("servicioID")]
    public int ServicioId { get; set; }

    [Column("contratistaID")]
    public int? ContratistaId { get; set; }

    [Column("detalleServicio")]
    [StringLength(250)]
    [Unicode(false)]
    public string? DetalleServicio { get; set; }

    [ForeignKey("ContratistaId")]
    [InverseProperty("ContratistasServicios")]
    public virtual Contratista? Contratista { get; set; }
}
