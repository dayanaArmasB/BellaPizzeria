using System;
using System.Collections.Generic;

namespace BellaNapoli.Models;

public partial class Compra
{
    public int IdCompra { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdProveedor { get; set; }

    public string? TipoDocumento { get; set; }

    public string? NumeroDocumento { get; set; }

    public decimal? MontoTotal { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual Proveedor? IdProveedorNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
