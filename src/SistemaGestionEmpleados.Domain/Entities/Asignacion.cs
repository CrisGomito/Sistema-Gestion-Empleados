using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaGestionEmpleados.Domain.Entities;

/// <summary>
/// Representa la asignación de un empleado a un departamento específico,
/// conservando historial, cargo, fecha de inicio/fin y bandera de departamento principal.
/// Mapea a la tabla SQL Server [Asignaciones].
/// </summary>
[Table("Asignaciones")]
public class Asignacion : IValidatableObject
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("asignacion_id")]
    [Display(Name = "ID Asignación")]
    public int AsignacionId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un empleado.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un empleado válido.")]
    [Column("empleado_id")]
    [Display(Name = "Empleado")]
    public int EmpleadoId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un departamento.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un departamento válido.")]
    [Column("departamento_id")]
    [Display(Name = "Departamento")]
    public int DepartamentoId { get; set; }

    [Required(ErrorMessage = "El cargo o rol dentro del departamento es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El cargo debe tener entre 2 y 100 caracteres.")]
    [Column("cargo", TypeName = "nvarchar(100)")]
    [Display(Name = "Cargo / Rol", Prompt = "Ej. Arquitecto de Software")]
    public string Cargo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de asignación es obligatoria.")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Column("fecha_asignacion", TypeName = "date")]
    [Display(Name = "Fecha de asignación")]
    public DateTime FechaAsignacion { get; set; } = DateTime.Today;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Column("fecha_fin", TypeName = "date")]
    [Display(Name = "Fecha de finalización")]
    public DateTime? FechaFin { get; set; }

    [Column("es_principal")]
    [Display(Name = "Departamento principal")]
    public bool EsPrincipal { get; set; } = true;

    [Column("activa")]
    [Display(Name = "Asignación activa")]
    public bool Activa { get; set; } = true;

    [StringLength(300, ErrorMessage = "Las observaciones no pueden exceder 300 caracteres.")]
    [Column("observaciones", TypeName = "nvarchar(300)")]
    [Display(Name = "Observaciones", Prompt = "Notas opcionales sobre la asignación")]
    public string? Observaciones { get; set; }

    [ForeignKey(nameof(EmpleadoId))]
    public Empleado? Empleado { get; set; }

    [ForeignKey(nameof(DepartamentoId))]
    public Departamento? Departamento { get; set; }

    /// <summary>
    /// Validación de invariantes de dominio a nivel de entidad.
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FechaFin.HasValue && FechaFin.Value.Date < FechaAsignacion.Date)
        {
            yield return new ValidationResult(
                "La fecha de finalización no puede ser anterior a la fecha de asignación.",
                new[] { nameof(FechaFin) });
        }

        if (!Activa && !FechaFin.HasValue)
        {
            yield return new ValidationResult(
                "Una asignación finalizada debe registrar su fecha de finalización.",
                new[] { nameof(FechaFin) });
        }
    }
}
