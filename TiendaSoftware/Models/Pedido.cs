using System;
using System.Collections.Generic;

namespace TiendaSoftware.Models;

public partial class Pedido
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public DateTime? FechaPedido { get; set; }

    public decimal Total { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<PedidoProducto> PedidoProductos { get; set; } = new List<PedidoProducto>();

    public virtual ICollection<PedidosMetodosPago> PedidosMetodosPagos { get; set; } = new List<PedidosMetodosPago>();

    public virtual Usuario Usuario { get; set; } = null!;
}
