using System;
using System.Collections.Generic;

namespace BellaNapoli.Models;

public partial class Proveedor
{
    public int IdProveedor { get; set; }

    public string? Documento { get; set; }

    public string? RazonSocial { get; set; }

    public string? Correo { get; set; }

    public string? Telefono { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaRegistro { get; set; }
    public DateTime? FechaEdicion { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
}
