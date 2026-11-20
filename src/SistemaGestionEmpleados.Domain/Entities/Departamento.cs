using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaGestionEmpleados.Domain.Entities;

/// <summary>
/// Representa un departamento organizacional dentro de la empresa.
/// Mapea directamente a la tabla SQL Server [Departamentos].
/// </summary>
[Table("Departamentos")]
public class Departamento
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("departamento_id")]
    [Display(Name = "ID Departamento")]
    public int DepartamentoId { get; set; }

    [Required(ErrorMessage = "El nombre del departamento es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    [Column("nombre", TypeName = "nvarchar(100)")]
    [Display(Name = "Nombre del departamento", Prompt = "Ej. Tecnología e Ingeniería")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La ubicación es obligatoria.")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "La ubicación debe tener entre 2 y 150 caracteres.")]
    [Column("ubicacion", TypeName = "nvarchar(150)")]
    [Display(Name = "Ubicación", Prompt = "Ej. Edificio Norte, Piso 4")]
    public string Ubicacion { get; set; } = string.Empty;

    [Required(ErrorMessage = "El jefe de departamento es obligatorio.")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "El nombre del jefe debe tener entre 3 y 120 caracteres.")]
    [Column("jefe_departamento", TypeName = "nvarchar(120)")]
    [Display(Name = "Jefe de departamento", Prompt = "Ej. Ing. Valeria Mendoza")]
    public string JefeDepartamento { get; set; } = string.Empty;

    [Required(ErrorMessage = "La extensión telefónica es obligatoria.")]
    [StringLength(15, MinimumLength = 2, ErrorMessage = "La extensión debe tener entre 2 y 15 caracteres.")]
    [RegularExpression(@"^[0-9+\-\s#]{2,15}$", ErrorMessage = "La extensión solo admite dígitos y símbolos (+, -, #).")]
    [Column("extension", TypeName = "varchar(15)")]
    [Display(Name = "Extensión", Prompt = "Ej. 4100")]
    public string Extension { get; set; } = string.Empty;

    /// <summary>
    /// Colección de asignaciones de empleados asociadas a este departamento.
    /// </summary>
    public ICollection<Asignacion> Asignaciones { get; set; } = new List<Asignacion>();
}
