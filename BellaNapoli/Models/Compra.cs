using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BellaNapoli.Models;

public partial class Compra
{
    public int IdCompra { get; set; }

    [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un usuario válido.")]
    public int? IdUsuario { get; set; }

    [Required(ErrorMessage = "El ID del proveedor es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un proveedor válido.")]
    public int? IdProveedor { get; set; }

    [Required(ErrorMessage = "El tipo de documento es obligatorio.")]
    [MaxLength(20, ErrorMessage = "El tipo de documento no debe exceder los 20 caracteres.")]
    [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "Solo se permiten letras y espacios.")]
    public string? TipoDocumento { get; set; }

    [Required(ErrorMessage = "El número de documento es obligatorio.")]
    [MaxLength(20, ErrorMessage = "El número de documento no debe exceder los 20 caracteres.")]
    [RegularExpression("^[a-zA-Z0-9\\-]+$", ErrorMessage = "Solo se permiten letras, números y guiones.")]
    public string? NumeroDocumento { get; set; }

    [Required(ErrorMessage = "El monto total es obligatorio.")]
    [Range(0.01, 1000000, ErrorMessage = "El monto total debe ser mayor a 0.")]
    public decimal? MontoTotal { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual Proveedor? IdProveedorNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
