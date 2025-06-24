using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BellaNapoli.Models
{
    public partial class Proveedor
    {
        public int IdProveedor { get; set; }

        [Required(ErrorMessage = "El documento es obligatorio.")]
        [StringLength(12, MinimumLength = 8, ErrorMessage = "El documento debe tener entre 8 y 11 dígitos si es RUC.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El documento solo puede contener números.")]
        public string? Documento { get; set; }

        [Required(ErrorMessage = "La razón social es obligatoria.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La razón social debe tener entre 3 y 100 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9 .,'\-&()]+$", ErrorMessage = "La razón social contiene caracteres no válidos.")]
        public string? RazonSocial { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        [StringLength(100, ErrorMessage = "El correo no debe exceder los 100 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.(com|pe)$", ErrorMessage = "El correo debe terminar en .com o .pe.")]
        public string? Correo { get; set; }

        [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "El teléfono debe tener exactamente 9 dígitos.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "El teléfono solo puede contener 9 dígitos numéricos.")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "Debe indicar si el proveedor está activo o no.")]
        public bool? Estado { get; set; }

        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaEdicion { get; set; }

        public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
    }
}
