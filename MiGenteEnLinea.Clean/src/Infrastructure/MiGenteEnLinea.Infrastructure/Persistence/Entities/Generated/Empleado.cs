using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;

public partial class Empleado
{
    [Key]
    [Column("empleadoID")]
    public int EmpleadoId { get; set; }

    [Column("userID")]
    [Unicode(false)]
    public string? UserId { get; set; }

    [Column("fechaRegistro", TypeName = "datetime")]
    public DateTime? FechaRegistro { get; set; }

    [Column("fechaInicio")]
    public DateOnly? FechaInicio { get; set; }

    [Column("identificacion")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Identificacion { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Nombre { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Apellido { get; set; }

    [Column("alias")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Alias { get; set; }

    [Column("nacimiento")]
    public DateOnly? Nacimiento { get; set; }

    [Column("estadoCivil")]
    public int? EstadoCivil { get; set; }

    [Column("direccion")]
    [StringLength(250)]
    [Unicode(false)]
    public string? Direccion { get; set; }

    [Column("provincia")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Provincia { get; set; }

    [Column("municipio")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Municipio { get; set; }

    [Column("telefono1")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Telefono1 { get; set; }

    [Column("telefono2")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Telefono2 { get; set; }

    [Column("posicion")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Posicion { get; set; }

    [Column("salario", TypeName = "decimal(10, 2)")]
    public decimal? Salario { get; set; }

    [Column("periodoPago")]
    public int? PeriodoPago { get; set; }

    [Column("contactoEmergencia")]
    [StringLength(50)]
    [Unicode(false)]
    public string? ContactoEmergencia { get; set; }

    [Column("telefonoEmergencia")]
    [StringLength(20)]
    [Unicode(false)]
    public string? TelefonoEmergencia { get; set; }

    [Column("contrato")]
    public bool? Contrato { get; set; }

    [Column("remuneracionExtra1")]
    [StringLength(100)]
    [Unicode(false)]
    public string? RemuneracionExtra1 { get; set; }

    [Column("montoExtra1", TypeName = "decimal(10, 2)")]
    public decimal? MontoExtra1 { get; set; }

    [Column("remuneracionExtra2")]
    [StringLength(100)]
    [Unicode(false)]
    public string? RemuneracionExtra2 { get; set; }

    [Column("montoExtra2", TypeName = "decimal(10, 2)")]
    public decimal? MontoExtra2 { get; set; }

    [Column("remuneracionExtra3")]
    [StringLength(100)]
    [Unicode(false)]
    public string? RemuneracionExtra3 { get; set; }

    [Column("montoExtra3", TypeName = "decimal(10, 2)")]
    public decimal? MontoExtra3 { get; set; }

    [Column("tss")]
    public bool? Tss { get; set; }

    [Column("diasPago")]
    public int? DiasPago { get; set; }

    public bool? Activo { get; set; }

    [Column("fechaSalida", TypeName = "datetime")]
    public DateTime? FechaSalida { get; set; }

    [Column("motivoBaja")]
    [StringLength(20)]
    [Unicode(false)]
    public string? MotivoBaja { get; set; }

    [Column("prestaciones", TypeName = "decimal(10, 2)")]
    public decimal? Prestaciones { get; set; }

    [Column("foto")]
    [Unicode(false)]
    public string? Foto { get; set; }

    [InverseProperty("Empleado")]
    public virtual ICollection<EmpleadorRecibosHeader> EmpleadorRecibosHeaders { get; set; } = new List<EmpleadorRecibosHeader>();
}
