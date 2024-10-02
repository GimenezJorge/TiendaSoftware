using System;
using System.Collections.Generic;

namespace TiendaSoftware1.Models;

public partial class Carrito
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<CarritoProducto> CarritoProductos { get; set; } = new List<CarritoProducto>();

    public virtual Usuario Usuario { get; set; } = null!;
}
