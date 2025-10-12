using System;
using System.Collections.Generic;
using MiGenteEnLinea.Infrastructure.Persistence.Entities.Generated;
using MiGenteEnLinea.Domain.Entities.Authentication;
using MiGenteEnLinea.Domain.Entities.Empleadores;
using MiGenteEnLinea.Domain.Entities.Contratistas;
using MiGenteEnLinea.Domain.Entities.Suscripciones;
using MiGenteEnLinea.Domain.Entities.Calificaciones;
using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Infrastructure.Persistence.Contexts;

public partial class MiGenteDbContext : DbContext
{
    public MiGenteDbContext(DbContextOptions<MiGenteDbContext> options)
        : base(options)
    {
    }

    // Legacy scaffolded entity (kept for reference)
    // public virtual DbSet<Calificacione> CalificacionesLegacy { get; set; }

    // DDD Refactored entity (replaces Calificacione)
    public virtual DbSet<Calificacion> Calificaciones { get; set; }

    public virtual DbSet<ConfigCorreo> ConfigCorreos { get; set; }

    // Legacy scaffolded entity (kept for reference)
    // public virtual DbSet<Infrastructure.Persistence.Entities.Generated.Contratista> ContratistasLegacy { get; set; }

    // DDD Refactored entity (replaces legacy Contratista)
    public virtual DbSet<Domain.Entities.Contratistas.Contratista> Contratistas { get; set; }

    public virtual DbSet<ContratistasFoto> ContratistasFotos { get; set; }

    public virtual DbSet<ContratistasServicio> ContratistasServicios { get; set; }

    // Legacy scaffolded entity (kept for reference)
    // public virtual DbSet<Credenciale> Credenciales { get; set; }

    // DDD Refactored entity (replaces Credenciale)
    public virtual DbSet<Credencial> CredencialesRefactored { get; set; }

    public virtual DbSet<DeduccionesTss> DeduccionesTsses { get; set; }

    // Legacy scaffolded entity (kept for reference)
    // public virtual DbSet<Ofertante> OfertantesLegacy { get; set; }

    // DDD Refactored entity (replaces Ofertante)
    public virtual DbSet<Empleador> Empleadores { get; set; }

    public virtual DbSet<DetalleContratacione> DetalleContrataciones { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<EmpleadorRecibosDetalle> EmpleadorRecibosDetalles { get; set; }

    public virtual DbSet<EmpleadorRecibosDetalleContratacione> EmpleadorRecibosDetalleContrataciones { get; set; }

    public virtual DbSet<EmpleadorRecibosHeader> EmpleadorRecibosHeaders { get; set; }

    public virtual DbSet<EmpleadorRecibosHeaderContratacione> EmpleadorRecibosHeaderContrataciones { get; set; }

    public virtual DbSet<EmpleadosNota> EmpleadosNotas { get; set; }

    public virtual DbSet<EmpleadosTemporale> EmpleadosTemporales { get; set; }

    // Commented out - using Empleadores instead (DDD refactored)
    // public virtual DbSet<Ofertante> Ofertantes { get; set; }

    public virtual DbSet<PaymentGateway> PaymentGateways { get; set; }

    public virtual DbSet<Perfile> Perfiles { get; set; }

    public virtual DbSet<PerfilesInfo> PerfilesInfos { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<PlanesContratista> PlanesContratistas { get; set; }

    public virtual DbSet<PlanesEmpleadore> PlanesEmpleadores { get; set; }

    public virtual DbSet<Provincia> Provincias { get; set; }

    public virtual DbSet<Sectore> Sectores { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    // Legacy scaffolded entity (kept for reference)
    // public virtual DbSet<Suscripcione> SuscripcionesLegacy { get; set; }

    // DDD Refactored entity (replaces Suscripcione)
    public virtual DbSet<Suscripcion> Suscripciones { get; set; }

    public virtual DbSet<Vcalificacione> Vcalificaciones { get; set; }

    public virtual DbSet<VcontratacionesTemporale> VcontratacionesTemporales { get; set; }

    public virtual DbSet<Vcontratista> Vcontratistas { get; set; }

    public virtual DbSet<Vempleado> Vempleados { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    public virtual DbSet<Vpago> Vpagos { get; set; }

    public virtual DbSet<VpagosContratacione> VpagosContrataciones { get; set; }

    public virtual DbSet<Vperfile> Vperfiles { get; set; }

    public virtual DbSet<VpromedioCalificacion> VpromedioCalificacions { get; set; }

    public virtual DbSet<Vsuscripcione> Vsuscripciones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all configurations from assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MiGenteDbContext).Assembly);

        // Legacy Contratista relationships (commented out - using refactored version)
        // The refactored Contratista configuration handles these relationships
        /*
        modelBuilder.Entity<ContratistasFoto>(entity =>
        {
            entity.HasOne(d => d.Contratista).WithMany(p => p.ContratistasFotos).HasConstraintName("FK_Contratistas_Fotos_Contratistas");
        });

        modelBuilder.Entity<ContratistasServicio>(entity =>
        {
            entity.HasOne(d => d.Contratista).WithMany(p => p.ContratistasServicios).HasConstraintName("FK_Contratistas_Servicios_Contratistas");
        });
        */

        // Legacy Credenciale mapping (commented out - using refactored version)
        // modelBuilder.Entity<Credenciale>(entity =>
        // {
        //     entity.Property(e => e.Activo).HasDefaultValue(false);
        // });

        modelBuilder.Entity<DeduccionesTss>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Deducciones");
        });

        modelBuilder.Entity<DetalleContratacione>(entity =>
        {
            entity.HasOne(d => d.Contratacion).WithMany(p => p.DetalleContrataciones).HasConstraintName("FK_DetalleContrataciones_EmpleadosTemporales");
        });

        modelBuilder.Entity<EmpleadorRecibosDetalle>(entity =>
        {
            entity.HasOne(d => d.Pago).WithMany(p => p.EmpleadorRecibosDetalles).HasConstraintName("FK_Empleador_Recibos_Detalle_Empleador_Recibos_Header");
        });

        modelBuilder.Entity<EmpleadorRecibosDetalleContratacione>(entity =>
        {
            entity.HasOne(d => d.Pago).WithMany(p => p.EmpleadorRecibosDetalleContrataciones).HasConstraintName("FK_Empleador_Recibos_Detalle_Contrataciones_Empleador_Recibos_Header_Contrataciones");
        });

        modelBuilder.Entity<EmpleadorRecibosHeader>(entity =>
        {
            entity.HasOne(d => d.Empleado).WithMany(p => p.EmpleadorRecibosHeaders).HasConstraintName("FK_Empleador_Recibos_Header_Empleados");
        });

        modelBuilder.Entity<EmpleadorRecibosHeaderContratacione>(entity =>
        {
            entity.HasOne(d => d.Contratacion).WithMany(p => p.EmpleadorRecibosHeaderContrataciones).HasConstraintName("FK_Empleador_Recibos_Header_Contrataciones_EmpleadosTemporales");
        });

        // Legacy Ofertante mapping (commented out - using refactored Empleador version)
        // modelBuilder.Entity<Ofertante>(entity =>
        // {
        //     entity.HasKey(e => e.OfertanteId).HasName("PK__Ofertant__B6039B8F8B329CD8");
        // });

        modelBuilder.Entity<Perfile>(entity =>
        {
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<PerfilesInfo>(entity =>
        {
            entity.HasOne(d => d.Perfil).WithMany(p => p.PerfilesInfos).HasConstraintName("FK_perfilesInfo_Perfiles");
        });

        modelBuilder.Entity<PlanesContratista>(entity =>
        {
            entity.Property(e => e.PlanId).ValueGeneratedNever();
        });

        modelBuilder.Entity<PlanesEmpleadore>(entity =>
        {
            entity.HasKey(e => e.PlanId).HasName("PK_Planes");

            entity.Property(e => e.Empleados).HasDefaultValue(0);
            entity.Property(e => e.Historico).HasDefaultValue(0);
            entity.Property(e => e.Nomina).HasDefaultValue(false);
        });

        modelBuilder.Entity<Vcalificacione>(entity =>
        {
            entity.ToView("VCalificaciones");
        });

        modelBuilder.Entity<VcontratacionesTemporale>(entity =>
        {
            entity.ToView("VContratacionesTemporales");
        });

        modelBuilder.Entity<Vcontratista>(entity =>
        {
            entity.ToView("VContratistas");
        });

        modelBuilder.Entity<Vempleado>(entity =>
        {
            entity.ToView("VEmpleados");

            entity.Property(e => e.EmpleadoId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Vpago>(entity =>
        {
            entity.ToView("VPagos");
        });

        modelBuilder.Entity<VpagosContratacione>(entity =>
        {
            entity.ToView("VPagosContrataciones");
        });

        modelBuilder.Entity<Vperfile>(entity =>
        {
            entity.ToView("VPerfiles");
        });

        modelBuilder.Entity<VpromedioCalificacion>(entity =>
        {
            entity.ToView("VPromedioCalificacion");
        });

        modelBuilder.Entity<Vsuscripcione>(entity =>
        {
            entity.ToView("VSuscripciones");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
