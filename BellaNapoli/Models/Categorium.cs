using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BellaNapoli.Models;

public partial class Categorium
{
    public int IdCategoria { get; set; }


    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [MaxLength(50, ErrorMessage = "La descripción no debe exceder los 50 caracteres.")]
    [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "La descripción solo puede contener letras y espacios.")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "Debe indicar si el cliente está activo o no.")]
    public bool? Estado { get; set; }

    public DateTime? FechaRegistro { get; set; }
    public DateTime? FechaEdicion { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
