using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BellaNapoli.Models;

public partial class DetalleVentum
{
    public int IdDetalleVenta { get; set; }

    [Required(ErrorMessage = "Debe especificar una venta.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un ID de venta válido.")]
    public int? IdVenta { get; set; }

    [Required(ErrorMessage = "Debe especificar un producto.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto válido.")]
    public int? IdProducto { get; set; }

    [Required(ErrorMessage = "El precio de venta es obligatorio.")]
    [Range(0.01, 100000, ErrorMessage = "El precio de venta debe ser mayor que 0.")]
    public decimal? PrecioVenta { get; set; }

    [Required(ErrorMessage = "Debe indicar la cantidad.")]
    [Range(1, 10000, ErrorMessage = "La cantidad debe ser mayor que 0.")]
    public int? Cantidad { get; set; }

    [Required(ErrorMessage = "El subtotal es obligatorio.")]
    [Range(0.01, 10000000, ErrorMessage = "El subtotal debe ser mayor que 0.")]
    public decimal? SubTotal { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }

    public virtual Ventum? IdVentaNavigation { get; set; }
}
