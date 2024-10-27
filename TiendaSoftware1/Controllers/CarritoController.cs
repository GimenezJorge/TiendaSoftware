using Microsoft.AspNetCore.Mvc;
using TiendaSoftware1.Models;
using System.Collections.Generic;
using System.Linq;

namespace TiendaSoftware1.Controllers
{
    public class CarritoController : Controller
    {
        private readonly Bdg3Context _context;

        // Lista para almacenar productos en el carrito
        private static List<Producto> _carrito = new List<Producto>();

        public CarritoController(Bdg3Context context)
        {
            _context = context;
        }

        // Método para agregar un producto al carrito
        public IActionResult AgregarAlCarrito(int id)
        {
            var producto = _context.Productos.FirstOrDefault(p => p.Id == id);
            if (producto != null)
            {
                _carrito.Add(producto);
            }

            return RedirectToAction("Carrito");
        }

        // Método para mostrar el carrito
        public IActionResult Carrito()
        {
            // Cambia esto para buscar en la ubicación correcta
            return View("~/Views/Productos/Carrito.cshtml", _carrito); // Ruta completa a la vista
        }
    }
}
