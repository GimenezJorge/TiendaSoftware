using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TiendaSoftware1.Models;

namespace TiendaSoftware1.Controllers
{
    public class HomeController : Controller
    {
        Bdg3Context db = new Bdg3Context();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var productos = db.Productos.ToList();
            return View(productos);
        }

        // --------- CRUD USUARIOS ---------


        // Crear usuario (Create) - GET
        public IActionResult CreateUsuario()
        {
            return View(); // Devolver el formulario de creación
        }

        // Crear usuario (Create) - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUsuario(Usuario usuario)
        {
            // Verificar si el correo electrónico ya existe
            if (db.Usuarios.Any(x => x.Email == usuario.Email))
            {
                ViewBag.Notification = "Este usuario ya existe";
                return View(); // Si existe, recargar el formulario de creación
            }
            else
            {
                // Asignar rol predeterminado
                usuario.RolId = 1;

                db.Usuarios.Add(usuario);
                db.SaveChanges();

                // Mensaje de éxito
                TempData["SuccessMessage"] = "Usuario creado con éxito";
                return RedirectToAction("GestionUsuarios"); // Redirigir a la lista de usuarios
            }
        }

        // Leer usuarios (Read)
        public IActionResult GestionUsuarios()
        {
            return View(db.Usuarios.ToList());
        }


        // Editar usuario (Update) - GET
        public async Task<IActionResult> EditUsuario(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await db.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // Editar usuario (Update) - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUsuario(int id, [Bind("Id,Nombre,Email,Password")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Usuarios.Update(usuario);
                    await db.SaveChangesAsync();
                    return RedirectToAction("GestionUsuarios"); // Redirige a la lista de usuarios
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Si hay errores de validación, volver a la vista de edición
            return View(usuario);
        }





        // Eliminar usuario (Delete) - GET
        [HttpGet]
        public async Task<IActionResult> DeleteUsuario(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await db.Usuarios.FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // Eliminar usuario (Delete) - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUsuarioConfirmed(int id)
        {
            var usuario = await db.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            db.Usuarios.Remove(usuario);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(GestionUsuarios));
        }









        private bool UsuarioExists(int id)
        {
            return db.Usuarios.Any(e => e.Id == id);
        }

        // Método auxiliar para verificar si el usuario existe

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(Usuario usuario)
        {
            // Verificar si el correo electrónico ya existe
            if (db.Usuarios.Any(x => x.Email == usuario.Email))
            {
                ViewBag.Notification = "Esta cuenta ya existe";
                return View();
            }
            else
            {
                // Asignar el rol predeterminado (por ejemplo, RolId = 1 para 'Usuario')
                usuario.RolId = 1;

                db.Usuarios.Add(usuario);
                db.SaveChanges();

                // Guardar información de sesión
                HttpContext.Session.SetString("email", usuario.Email.ToString());
                HttpContext.Session.SetString("clave", usuario.Password.ToString());

                // Almacenar el mensaje de éxito en TempData
                TempData["SuccessMessage"] = "Cuenta creada con éxito";

                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(Usuario usuario)
        {
            var checkLogin = db.Usuarios.Where(x => x.Email.Equals(usuario.Email)
            && x.Password.Equals(usuario.Password))
                .FirstOrDefault();

            if (checkLogin != null)
            {
                HttpContext.Session.SetString("email", checkLogin.Email);   
                HttpContext.Session.SetString("nombre", checkLogin.Nombre); 
                HttpContext.Session.SetString("clave", checkLogin.Password);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Notification = "Email o clave incorrectas";
            }
            return View();
        }


        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult DetailsProductos(int id)
        {
            var producto = db.Productos.FirstOrDefault(p => p.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        //---------------PRODUCTOS
        public IActionResult GestionProductos()
        {
            return View(db.Productos.ToList());
        }

        public IActionResult CreateProducto()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProducto(Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Productos.Add(producto);
                db.SaveChanges();
                return RedirectToAction("GestionProductos");
            }
            return View(producto);
        }
        // Editar producto (Update) - GET
        public async Task<IActionResult> EditProducto(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await db.Productos.FindAsync(id); // Buscar el producto por su ID
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto); // Retornar la vista para editar el producto
        }

        // Editar producto (Update) - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProducto(int id, [Bind("Id,Nombre,Descripcion,Precio,Stock,FechaLanzamiento,Tipo")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Productos.Update(producto); // Actualizar los datos del producto
                    await db.SaveChangesAsync(); // Guardar los cambios en la base de datos
                    return RedirectToAction("GestionProductos"); // Redirigir a la lista de productos
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(producto); // Si hay errores, volver a la vista con los datos actuales
        }



        // Eliminar producto (Delete) - GET
        [HttpGet]
        public async Task<IActionResult> DeleteProducto(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await db.Productos.FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto); // Mostrar la vista de confirmación
        }

        // Eliminar producto (Delete) - POST
        [HttpPost, ActionName("DeleteProducto")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await db.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            db.Productos.Remove(producto);
            await db.SaveChangesAsync();

            return RedirectToAction("GestionProductos"); // Redirigir a la lista de productos
        }

        // Método auxiliar para verificar si un producto existe
        private bool ProductoExists(int id)
        {
            return db.Productos.Any(e => e.Id == id);
        }

    }
}