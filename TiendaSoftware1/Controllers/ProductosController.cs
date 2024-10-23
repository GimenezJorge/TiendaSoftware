using Microsoft.AspNetCore.Mvc;
using TiendaSoftware1.Models;
using System.Linq;

namespace TiendaSoftware1.Controllers
{
    public class ProductosController : Controller
    {
        private readonly Bdg3Context _context;

        public ProductosController(Bdg3Context context)
        {
            _context = context;
        }

        // Página principal que podría mostrar todos los productos o redirigir a una categoría
        public IActionResult Index()
        {
            // Opcional: redirigir a una categoría específica o mostrar todos los productos
            return RedirectToAction("Nintendo"); // Redirige a la categoría Nintendo como ejemplo
        }

        // Ruta para productos de Nintendo
        public IActionResult Nintendo()
        {
            var productos = _context.Productos.Where(p => p.Tipo == "nintendo").ToList();
            return View(productos);
        }

        // Ruta para productos de PlayStation
        public IActionResult PlayStation() // Cambiado de Playstation a PlayStation
        {
            var productos = _context.Productos.Where(p => p.Tipo == "playstation").ToList();
            return View(productos);
        }

        // Ruta para productos de Xbox
        public IActionResult Xbox()
        {
            var productos = _context.Productos.Where(p => p.Tipo == "xbox").ToList();
            return View(productos);
        }

        // Ruta para productos de Accesorios
        public IActionResult Accesorios()
        {
            var productos = _context.Productos.Where(p => p.Tipo == "accesorios").ToList();
            return View(productos);
        }
    }
}
