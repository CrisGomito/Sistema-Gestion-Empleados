using System.ComponentModel.DataAnnotations;

namespace SistemaGestionEmpleados.Application.DTOs;

public sealed class AsignacionDto : IValidatableObject
{
    public int AsignacionId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un empleado.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un empleado válido.")]
    [Display(Name = "Empleado")]
    public int EmpleadoId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un departamento.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un departamento válido.")]
    [Display(Name = "Departamento")]
    public int DepartamentoId { get; set; }

    [Required(ErrorMessage = "El cargo o rol es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El cargo debe tener entre 2 y 100 caracteres.")]
    [Display(Name = "Cargo / Rol")]
    public string Cargo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de asignación es obligatoria.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fecha de asignación")]
    public DateTime FechaAsignacion { get; set; } = DateTime.Today;

    [DataType(DataType.Date)]
    [Display(Name = "Fecha de finalización")]
    public DateTime? FechaFin { get; set; }

    [Display(Name = "Departamento principal")]
    public bool EsPrincipal { get; set; } = true;

    [Display(Name = "Asignación activa")]
    public bool Activa { get; set; } = true;

    [StringLength(300, ErrorMessage = "Las observaciones no pueden exceder 300 caracteres.")]
    [Display(Name = "Observaciones")]
    public string? Observaciones { get; set; }

    // Datos de proyección para tablas y vistas de detalle
    public string EmpleadoNombreCompleto { get; set; } = string.Empty;
    public string EmpleadoEmail { get; set; } = string.Empty;
    public string DepartamentoNombre { get; set; } = string.Empty;
    public string DepartamentoUbicacion { get; set; } = string.Empty;
    public string DepartamentoExtension { get; set; } = string.Empty;

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
                "Para finalizar una asignación debe indicar la fecha de finalización.",
                new[] { nameof(FechaFin) });
        }
    }
}
