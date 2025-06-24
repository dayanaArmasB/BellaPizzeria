using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BellaNapoli.Models;

public partial class DetalleCompra
{
    public int IdDetalleCompra { get; set; }

    [Required(ErrorMessage = "Debe especificar una compra.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un ID de compra válido.")]
    public int? IdCompra { get; set; }

    [Required(ErrorMessage = "Debe especificar un producto.")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto válido.")]
    public int? IdProducto { get; set; }

    [Required(ErrorMessage = "El precio de compra es obligatorio.")]
    [Range(0.01, 100000, ErrorMessage = "El precio de compra debe ser mayor que 0.")]
    public decimal? PrecioCompra { get; set; }

    [Required(ErrorMessage = "El precio de venta es obligatorio.")]
    [Range(0.01, 100000, ErrorMessage = "El precio de venta debe ser mayor que 0.")]
    public decimal? PrecioVenta { get; set; }

    [Required(ErrorMessage = "Debe indicar la cantidad.")]
    [Range(1, 10000, ErrorMessage = "La cantidad debe ser mayor que 0.")]
    public int? Cantidad { get; set; }

    [Required(ErrorMessage = "El monto total es obligatorio.")]
    [Range(0.01, 10000000, ErrorMessage = "El monto total debe ser mayor que 0.")]
    public decimal? MontoTotal { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual Compra? IdCompraNavigation { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
}
