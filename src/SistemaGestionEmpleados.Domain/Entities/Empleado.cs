using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaGestionEmpleados.Domain.Entities;

/// <summary>
/// Representa a un empleado registrado en el sistema.
/// Mapea directamente a la tabla SQL Server [Empleados].
/// </summary>
[Table("Empleados")]
public class Empleado
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("empleado_id")]
    [Display(Name = "ID Empleado")]
    public int EmpleadoId { get; set; }

    [Required(ErrorMessage = "El nombre del empleado es obligatorio.")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 80 caracteres.")]
    [Column("nombre", TypeName = "nvarchar(80)")]
    [Display(Name = "Nombre", Prompt = "Ej. Mateo")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido del empleado es obligatorio.")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 80 caracteres.")]
    [Column("apellido", TypeName = "nvarchar(80)")]
    [Display(Name = "Apellido", Prompt = "Ej. Castellanos")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido.")]
    [StringLength(150, ErrorMessage = "El correo electrónico no puede superar los 150 caracteres.")]
    [Column("email", TypeName = "nvarchar(150)")]
    [Display(Name = "Correo electrónico", Prompt = "Ej. m.castellanos@empresa.com")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [Phone(ErrorMessage = "Ingrese un número de teléfono válido.")]
    [StringLength(25, MinimumLength = 7, ErrorMessage = "El teléfono debe tener entre 7 y 25 caracteres.")]
    [RegularExpression(@"^[0-9+\-\s()]{7,25}$", ErrorMessage = "El teléfono contiene caracteres no válidos.")]
    [Column("telefono", TypeName = "varchar(25)")]
    [Display(Name = "Teléfono", Prompt = "Ej. +593 99 482 1104")]
    public string Telefono { get; set; } = string.Empty;

    /// <summary>
    /// Propiedad calculada de solo lectura para vistas y reportes.
    /// </summary>
    [NotMapped]
    [Display(Name = "Nombre completo")]
    public string NombreCompleto => $"{Nombre} {Apellido}".Trim();

    /// <summary>
    /// Colección de asignaciones del empleado en uno o varios departamentos.
    /// </summary>
    public ICollection<Asignacion> Asignaciones { get; set; } = new List<Asignacion>();
}
