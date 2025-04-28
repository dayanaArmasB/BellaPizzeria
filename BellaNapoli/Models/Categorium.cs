using System;
using System.Collections.Generic;

namespace BellaNapoli.Models;

public partial class Categorium
{
    public int IdCategoria { get; set; }

    public string? Descripcion { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaRegistro { get; set; }
    public DateTime? FechaEdicion { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
