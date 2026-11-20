using System.ComponentModel.DataAnnotations;

namespace SistemaGestionEmpleados.Application.DTOs;

public sealed class EmpleadoDto
{
    public int EmpleadoId { get; set; }

    [Required(ErrorMessage = "El nombre del empleado es obligatorio.")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 80 caracteres.")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido del empleado es obligatorio.")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 80 caracteres.")]
    [Display(Name = "Apellido")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido.")]
    [StringLength(150, ErrorMessage = "El correo electrónico no puede superar los 150 caracteres.")]
    [Display(Name = "Correo electrónico")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [Phone(ErrorMessage = "Ingrese un número de teléfono válido.")]
    [StringLength(25, MinimumLength = 7, ErrorMessage = "El teléfono debe tener entre 7 y 25 caracteres.")]
    [RegularExpression(@"^[0-9+\-\s()]{7,25}$", ErrorMessage = "El teléfono contiene caracteres no válidos.")]
    [Display(Name = "Teléfono")]
    public string Telefono { get; set; } = string.Empty;

    public string NombreCompleto => $"{Nombre} {Apellido}".Trim();
    public string? DepartamentoPrincipal { get; set; }
    public int TotalAsignacionesActivas { get; set; }
}
