using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaGestionEmpleados.Domain.Entities;

namespace SistemaGestionEmpleados.Infrastructure.Persistence.Configurations;

public sealed class AsignacionConfiguration : IEntityTypeConfiguration<Asignacion>
{
    public void Configure(EntityTypeBuilder<Asignacion> builder)
    {
        builder.ToTable("Asignaciones", table =>
        {
            table.HasCheckConstraint(
                "CK_Asignaciones_Fechas",
                "[fecha_fin] IS NULL OR [fecha_fin] >= [fecha_asignacion]");
        });

        builder.HasKey(a => a.AsignacionId);
        builder.Property(a => a.AsignacionId)
            .HasColumnName("asignacion_id")
            .UseIdentityColumn();

        builder.Property(a => a.EmpleadoId)
            .HasColumnName("empleado_id")
            .IsRequired();

        builder.Property(a => a.DepartamentoId)
            .HasColumnName("departamento_id")
            .IsRequired();

        builder.Property(a => a.Cargo)
            .HasColumnName("cargo")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.FechaAsignacion)
            .HasColumnName("fecha_asignacion")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(a => a.FechaFin)
            .HasColumnName("fecha_fin")
            .HasColumnType("date");

        builder.Property(a => a.EsPrincipal)
            .HasColumnName("es_principal")
            .HasDefaultValue(true);

        builder.Property(a => a.Activa)
            .HasColumnName("activa")
            .HasDefaultValue(true);

        builder.Property(a => a.Observaciones)
            .HasColumnName("observaciones")
            .HasMaxLength(300);

        builder.HasOne(a => a.Empleado)
            .WithMany(e => e.Asignaciones)
            .HasForeignKey(a => a.EmpleadoId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Asignaciones_Empleados");

        builder.HasOne(a => a.Departamento)
            .WithMany(d => d.Asignaciones)
            .HasForeignKey(a => a.DepartamentoId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Asignaciones_Departamentos");

        // Índice filtrado en SQL Server: evita duplicar una asignación ACTIVA del mismo empleado en el mismo departamento
        builder.HasIndex(a => new { a.EmpleadoId, a.DepartamentoId })
            .IsUnique()
            .HasFilter("[activa] = 1")
            .HasDatabaseName("UX_Asignaciones_Activa_Empleado_Departamento");
    }
}
