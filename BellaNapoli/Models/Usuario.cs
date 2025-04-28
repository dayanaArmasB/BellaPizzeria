using System;
using System.Collections.Generic;

namespace BellaNapoli.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? Documento { get; set; }

    public string? NombreCompleto { get; set; }

    public string? Correo { get; set; }

    public string? Clave { get; set; }

    public int? IdRol { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaRegistro { get; set; }
    public DateTime? FechaEdicion { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();

}
