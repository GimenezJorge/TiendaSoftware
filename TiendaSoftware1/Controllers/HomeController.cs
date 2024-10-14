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

            var usuario = await db.Usuarios.FindAsync(id); // Buscar el usuario por su ID usando 'db'
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario); // Devolver el formulario de edición
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
                    db.Usuarios.Update(usuario); // Actualizar los datos del usuario
                    await db.SaveChangesAsync();  // Guardar los cambios en la base de datos
                    return RedirectToAction("GestionUsuarios"); // Redirigir a la lista de usuarios
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

            return View(usuario); // Si hay errores, volver a la vista con los datos actuales
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


 
    }
}