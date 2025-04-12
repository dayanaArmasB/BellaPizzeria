using System;
using System.Collections.Generic;

namespace BellaNapoli.Models;

public partial class Rol
{
    public int IdRol { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
