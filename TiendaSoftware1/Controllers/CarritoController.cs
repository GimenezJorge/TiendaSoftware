using Microsoft.AspNetCore.Mvc;
using TiendaSoftware1.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TiendaSoftware1.Controllers
{
    public class CarritoController : Controller
    {
        private readonly Bdg3Context _context;
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
                int cantidadActual = carritoProducto != null ? carritoProducto.Cantidad : 0;

                // Verifica si la cantidad total supera el stock
                if (cantidadActual + cantidad > producto.Stock)
                {
                    return RedirectToAction("Carrito", new { mensaje = "No se puede agregar más productos al carrito, excede el stock disponible." });
                }

                if (carritoProducto != null)
                {
                    carritoProducto.Cantidad += cantidad;
                }
                else
                {
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

        // Método para quitar un producto del carrito
        [HttpPost]
        public IActionResult QuitarDelCarrito(int productoId)
        {
            var carritoProducto = _carrito.CarritoProductos.FirstOrDefault(cp => cp.ProductoId == productoId);
            if (carritoProducto != null)
            {
                _carrito.CarritoProductos.Remove(carritoProducto);
            }

            return RedirectToAction("Carrito");
        }

        // Método para mostrar el carrito
        public IActionResult Carrito(string? mensaje = null)
        {
            ViewBag.Mensaje = mensaje;
            return View("~/Views/Productos/Carrito.cshtml", _carrito);
        }

        // Método para generar el PDF del carrito usando iTextSharp
        [HttpGet("Carrito/GenerarPDF")]
        public IActionResult GenerarPDF()
        {
            using (var ms = new MemoryStream())
            {
                // Crear documento PDF
                var document = new Document(PageSize.A4);
                PdfWriter.GetInstance(document, ms).CloseStream = false;
                document.Open();

                // Título del reporte
                document.Add(new Paragraph("Carrito de Compras"));
                document.Add(new Paragraph(" ")); // Espacio en blanco

                // Tabla de productos
                PdfPTable table = new PdfPTable(4); // 4 columnas: Producto, Cantidad, Precio Unitario, Precio Total
                table.AddCell("Producto");
                table.AddCell("Cantidad");
                table.AddCell("Precio Unitario");
                table.AddCell("Precio Total");

                foreach (var item in _carrito.CarritoProductos)
                {
                    table.AddCell(item.Producto.Nombre);
                    table.AddCell(item.Cantidad.ToString());
                    table.AddCell($"${item.Producto.Precio}");
                    table.AddCell($"${item.Cantidad * item.Producto.Precio}");
                }

                // Total del carrito
                decimal total = _carrito.CarritoProductos.Sum(cp => cp.Cantidad * cp.Producto.Precio);
                document.Add(table);
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph($"Total: ${total}"));

                document.Close();
                var pdfStream = new MemoryStream(ms.ToArray());

                // Descargar el PDF
                return File(pdfStream, "application/pdf", "Carrito.pdf");
            }
        }

        // Método para finalizar la compra
        [HttpPost]
        public IActionResult FinalizarCompra()
        {
            if (_carrito.CarritoProductos.Any())
            {
                foreach (var carritoProducto in _carrito.CarritoProductos)
                {
                    var producto = _context.Productos.FirstOrDefault(p => p.Id == carritoProducto.ProductoId);
                    if (producto != null)
                    {
                        // Descontar el stock de cada producto comprado
                        if (producto.Stock >= carritoProducto.Cantidad)
                        {
                            producto.Stock -= carritoProducto.Cantidad;
                            _context.SaveChanges();
                        }
                        else
                        {
                            // Si no hay suficiente stock, mostrar un mensaje de error
                            return RedirectToAction("Carrito", new { mensaje = "No hay suficiente stock para algunos productos." });
                        }
                    }
                }

                // Vaciar el carrito después de la compra
                _carrito.CarritoProductos.Clear();

                return RedirectToAction("CompraFinalizada");
            }
            return RedirectToAction("Carrito", new { mensaje = "Tu carrito está vacío." });
        }

        // Acción para mostrar que la compra fue finalizada
        public IActionResult CompraFinalizada()
        {
            return View("~/Views/Productos/CompraFinalizada.cshtml");
        }
    }
}
