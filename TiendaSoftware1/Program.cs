using Microsoft.EntityFrameworkCore;
using TiendaSoftware1.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar el servicio de sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Establece el tiempo de espera de la sesión
    options.Cookie.HttpOnly = true; // Evita el acceso a las cookies desde JavaScript
    options.Cookie.IsEssential = true; // La sesión es esencial para la aplicación
});

// Configuración de la autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Ruta para redirigir a la página de login
        options.AccessDeniedPath = "/Account/AccessDenied"; // Ruta para acceso denegado
    });

// Agregar el contexto de la base de datos
builder.Services.AddDbContext<Bdg3Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Añadir el middleware de sesión
app.UseSession();

// Añadir el middleware de autenticación
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); //prueba
