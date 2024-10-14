using System;
using System.Collections.Generic;

namespace TiendaSoftware1.Models;

public partial class UsuarioDetalle
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
