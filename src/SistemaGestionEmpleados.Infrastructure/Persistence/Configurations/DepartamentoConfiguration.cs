using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaGestionEmpleados.Domain.Entities;

namespace SistemaGestionEmpleados.Infrastructure.Persistence.Configurations;

public sealed class DepartamentoConfiguration : IEntityTypeConfiguration<Departamento>
{
    public void Configure(EntityTypeBuilder<Departamento> builder)
    {
        builder.ToTable("Departamentos");

        builder.HasKey(d => d.DepartamentoId);
        builder.Property(d => d.DepartamentoId)
            .HasColumnName("departamento_id")
            .UseIdentityColumn();

        builder.Property(d => d.Nombre)
            .HasColumnName("nombre")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.Ubicacion)
            .HasColumnName("ubicacion")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(d => d.JefeDepartamento)
            .HasColumnName("jefe_departamento")
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(d => d.Extension)
            .HasColumnName("extension")
            .HasMaxLength(15)
            .IsUnicode(false)
            .IsRequired();

        builder.HasIndex(d => d.Nombre)
            .IsUnique()
            .HasDatabaseName("UX_Departamentos_Nombre");

        builder.HasIndex(d => d.Extension)
            .IsUnique()
            .HasDatabaseName("UX_Departamentos_Extension");
    }
}
