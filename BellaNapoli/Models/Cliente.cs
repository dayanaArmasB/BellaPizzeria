using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BellaNapoli.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    [Required(ErrorMessage = "El documento es obligatorio.")]
    [MaxLength(8, ErrorMessage = "El documento no debe exceder los 8 caracteres.")]
    [MinLength(8, ErrorMessage = "El documento debe tener al menos 8 caracteres.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "El documento solo puede contener números.")]
    public string? Documento { get; set; }

    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
    public string? NombreCompleto { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
    [StringLength(100, ErrorMessage = "El correo no debe exceder los 100 caracteres.")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.(com|pe)$", ErrorMessage = "El correo debe terminar en .com o .pe.")]
    public string? Correo { get; set; }

    [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
    [StringLength(9, MinimumLength = 9, ErrorMessage = "El teléfono debe tener exactamente 9 dígitos.")]
    [RegularExpression(@"^\d{9}$", ErrorMessage = "El teléfono solo puede contener 9 dígitos numéricos.")]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "Debe indicar si el cliente está activo o no.")]
    public bool? Estado { get; set; }

    public DateTime? FechaRegistro { get; set; }
    public DateTime? FechaEdicion { get; set; }
}
