using System;
using System.Collections.Generic;

namespace TiendaSoftware1.Models;

public partial class PedidosMetodosPago
{
    public int Id { get; set; }

    public int PedidoId { get; set; }

    public int MetodoPagoId { get; set; }

    public virtual MetodosPago MetodoPago { get; set; } = null!;

    public virtual Pedido Pedido { get; set; } = null!;
}
