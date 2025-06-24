using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BellaNapoli.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }
    [Required(ErrorMessage = "El DOCUMENTO es obligatorio")]
    [MaxLength(8, ErrorMessage = "El documento no debe exceder los 8 caracteres.")]
    [MinLength(8, ErrorMessage = "El documento debe tener al menos 8 caracteres.")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El documento debe tener exactamente 8 dígitos")]
    public string? Documento { get; set; }
    [Required(ErrorMessage = "El nombre completo es obligatorio")]
    [MaxLength(100, ErrorMessage = "La descripción no debe exceder los 50 caracteres.")]
    [MinLength(5, ErrorMessage = "La descripción debe tener al menos 5 caracteres.")]
    public string? NombreCompleto { get; set; }

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "Ingrese un correo válido")]
    [MaxLength(100, ErrorMessage = "La descripción no debe exceder los 100 caracteres.")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.(com|pe)$", ErrorMessage = "El correo debe tener formato usuario@dominio.com o usuario@dominio.pe")]
    public string Correo { get; set; }

    public string? Clave { get; set; }

    [Required(ErrorMessage = "El rol es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un rol válido.")]
    public int IdRol { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }

    [Required(ErrorMessage = "Debe seleccionar el estado del usuario.")]
    public bool? Estado { get; set; }

    public DateTime? FechaRegistro { get; set; }
    public DateTime? FechaEdicion { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();

}
