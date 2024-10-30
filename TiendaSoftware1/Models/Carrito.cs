using System;
using System.Collections.Generic;
using System.Linq; // Agregar para usar LINQ

namespace TiendaSoftware1.Models
{
    public partial class Carrito
    {
        public int Id { get; set; } // Identificador del carrito

        public int UsuarioId { get; set; } // Identificador del usuario

        public DateTime? FechaCreacion { get; set; } // Fecha de creación del carrito

        // Colección de productos en el carrito
        public virtual ICollection<CarritoProducto> CarritoProductos { get; set; } = new List<CarritoProducto>();

        public virtual Usuario Usuario { get; set; } = null!; // Relación con el usuario

        // Método para agregar un producto al carrito
        public void AgregarProducto(Producto producto, int cantidad)
        {
            // Verifica si el producto ya está en el carrito
            var itemExistente = CarritoProductos.FirstOrDefault(item => item.ProductoId == producto.Id);
            if (itemExistente != null)
            {
                // Si ya existe, incrementa la cantidad
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                // Si no existe, agrega un nuevo producto al carrito
                CarritoProductos.Add(new CarritoProducto { ProductoId = producto.Id, Cantidad = cantidad, Producto = producto });
            }
        }

        // Método para obtener el total del carrito
        public decimal ObtenerTotal()
        {
            return CarritoProductos.Sum(item => item.Producto.Precio * item.Cantidad); // Suma total de los productos
        }
    }
}
