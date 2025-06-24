using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BellaNapoli.Models;

public partial class Permiso
{
    public int IdPermiso { get; set; }

    [Required(ErrorMessage = "Debe especificar un rol.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un rol válido.")]
    public int? IdRol { get; set; }

    [Required(ErrorMessage = "Debe indicar si el permiso está activo.")]
    public bool? Estado { get; set; }

    [Required(ErrorMessage = "El nombre del menú es obligatorio.")]
    [MaxLength(50, ErrorMessage = "El nombre del menú no debe exceder los 50 caracteres.")]
    [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "El nombre del menú solo puede contener letras y espacios.")]
    public string? NombreMenu { get; set; }

    public DateTime? FechaRegistro { get; set; }
    public DateTime? FechaEdicion { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }
}
