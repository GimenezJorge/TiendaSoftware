using System;
using System.Collections.Generic;

namespace TiendaSoftware1.Models
{
    public partial class CarritoProducto
    {
        public int Id { get; set; } // Identificador único del carrito del producto

        public int CarritoId { get; set; } // Identificador del carrito

        public int ProductoId { get; set; } // Identificador del producto

        public int Cantidad { get; set; } // Cantidad del producto en el carrito

        public virtual Carrito Carrito { get; set; } = null!; // Relación con la clase Carrito

        public virtual Producto Producto { get; set; } = null!; // Relación con la clase Producto
    }
}
