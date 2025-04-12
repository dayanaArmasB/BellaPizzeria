using System;
using System.Collections.Generic;

namespace BellaNapoli.Models;

public partial class Negocio
{
    public int IdNegocio { get; set; }

    public string? Nombre { get; set; }

    public string? Ruc { get; set; }

    public string? Direccion { get; set; }

    public byte[]? Logo { get; set; }
}
