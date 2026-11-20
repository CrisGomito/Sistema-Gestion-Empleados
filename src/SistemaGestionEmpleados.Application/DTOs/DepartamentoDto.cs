using System.ComponentModel.DataAnnotations;

namespace SistemaGestionEmpleados.Application.DTOs;

public sealed class DepartamentoDto
{
    public int DepartamentoId { get; set; }

    [Required(ErrorMessage = "El nombre del departamento es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    [Display(Name = "Nombre del departamento")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La ubicación es obligatoria.")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "La ubicación debe tener entre 2 y 150 caracteres.")]
    [Display(Name = "Ubicación")]
    public string Ubicacion { get; set; } = string.Empty;

    [Required(ErrorMessage = "El jefe de departamento es obligatorio.")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "El nombre del jefe debe tener entre 3 y 120 caracteres.")]
    [Display(Name = "Jefe de departamento")]
    public string JefeDepartamento { get; set; } = string.Empty;

    [Required(ErrorMessage = "La extensión telefónica es obligatoria.")]
    [StringLength(15, MinimumLength = 2, ErrorMessage = "La extensión debe tener entre 2 y 15 caracteres.")]
    [RegularExpression(@"^[0-9+\-\s#]{2,15}$", ErrorMessage = "La extensión solo admite dígitos y símbolos (+, -, #).")]
    [Display(Name = "Extensión")]
    public string Extension { get; set; } = string.Empty;

    public int TotalAsignacionesActivas { get; set; }
    public int TotalAsignacionesHistoricas { get; set; }
}
