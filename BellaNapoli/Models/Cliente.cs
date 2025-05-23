﻿using System;
using System.Collections.Generic;

namespace BellaNapoli.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string? Documento { get; set; }

    public string? NombreCompleto { get; set; }

    public string? Correo { get; set; }

    public string? Telefono { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaRegistro { get; set; }
    public DateTime? FechaEdicion { get; set; }
}
