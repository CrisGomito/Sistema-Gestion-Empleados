using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaGestionEmpleados.Domain.Entities;

namespace SistemaGestionEmpleados.Infrastructure.Persistence.Configurations;

public sealed class EmpleadoConfiguration : IEntityTypeConfiguration<Empleado>
{
    public void Configure(EntityTypeBuilder<Empleado> builder)
    {
        builder.ToTable("Empleados");

        builder.HasKey(e => e.EmpleadoId);
        builder.Property(e => e.EmpleadoId)
            .HasColumnName("empleado_id")
            .UseIdentityColumn();

        builder.Property(e => e.Nombre)
            .HasColumnName("nombre")
            .HasMaxLength(80)
            .IsRequired();

        builder.Property(e => e.Apellido)
            .HasColumnName("apellido")
            .HasMaxLength(80)
            .IsRequired();

        builder.Property(e => e.Email)
            .HasColumnName("email")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(e => e.Telefono)
            .HasColumnName("telefono")
            .HasMaxLength(25)
            .IsUnicode(false)
            .IsRequired();

        builder.HasIndex(e => e.Email)
            .IsUnique()
            .HasDatabaseName("UX_Empleados_Email");

        builder.HasIndex(e => new { e.Apellido, e.Nombre })
            .HasDatabaseName("IX_Empleados_Apellido_Nombre");
    }
}
