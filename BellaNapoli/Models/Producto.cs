using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace BellaNapoli.Models;

public partial class Producto
{

    public int IdProducto { get; set; }

    [Required(ErrorMessage = "El codigo es obligatorio.")]
    [MaxLength(10, ErrorMessage = "El nombre no debe exceder los 10 caracteres.")]
    [RegularExpression("^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "Solo se permiten letras y números.")]
    public string Codigo { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(20, ErrorMessage = "El nombre no debe exceder los 20 caracteres.")]
    [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [MaxLength(50, ErrorMessage = "La descripción no debe exceder los 50 caracteres.")]
    [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "La descripción solo puede contener letras y espacios.")]
    public string Descripcion { get; set; }

    [Required(ErrorMessage = "Debe seleccionar una categoría")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría válida.")]
    public int IdCategoria { get; set; }
    
    [Required(ErrorMessage = "Agregar el stock es obligatorio.")]
    [Range(1, 50, ErrorMessage = "El stock debe ser un número entre 1 y 50.")]
    public int Stock { get; set; }

    public decimal? PrecioCompra { get; set; }

    public decimal? PrecioVenta { get; set; }

    [Required(ErrorMessage = "El campo es obligatorio")]
    public bool Estado { get; set; }= true;

    public DateTime? FechaRegistro { get; set; }
    public DateTime? FechaEdicion { get; set; }

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual ICollection<DetalleVentum> DetalleVenta { get; set; } = new List<DetalleVentum>();

    public virtual Categorium? IdCategoriaNavigation { get; set; }
}
