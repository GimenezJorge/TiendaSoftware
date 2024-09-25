using System;
using System.Collections.Generic;

namespace TiendaSoftware.Models;

public partial class MetodosPago
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<PedidosMetodosPago> PedidosMetodosPagos { get; set; } = new List<PedidosMetodosPago>();
}
