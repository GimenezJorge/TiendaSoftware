using Microsoft.AspNetCore.Mvc;
using TiendaSoftware1.Models;
using System.Collections.Generic;
using System.Linq;

namespace TiendaSoftware1.Controllers
{
    public class CarritoController : Controller
    {
        private readonly Bdg3Context _context;

        // Carrito de compras, ahora utilizando un objeto Carrito en vez de una lista de productos.
        private static Carrito _carrito = new Carrito
        {
            CarritoProductos = new List<CarritoProducto>()
        };

        public CarritoController(Bdg3Context context)
        {
            _context = context;
        }

        // Método para agregar un producto al carrito
        public IActionResult AgregarAlCarrito(int id, int cantidad)
        {
            var producto = _context.Productos.FirstOrDefault(p => p.Id == id);
            if (producto != null && cantidad > 0)
            {
                var carritoProducto = _carrito.CarritoProductos.FirstOrDefault(cp => cp.ProductoId == producto.Id);

                if (carritoProducto != null)
                {
                    // Si el producto ya está en el carrito, simplemente incrementamos la cantidad
                    carritoProducto.Cantidad += cantidad;
                }
                else
                {
                    // Si no está en el carrito, lo agregamos
                    _carrito.CarritoProductos.Add(new CarritoProducto
                    {
                        ProductoId = producto.Id,
                        Producto = producto,
                        Cantidad = cantidad
                    });
                }
            }

            return RedirectToAction("Carrito");
        }

        // Método para mostrar el carrito
        public IActionResult Carrito()
        {
            return View("~/Views/Productos/Carrito.cshtml", _carrito); // Ruta completa a la vista
        }
    }
}
