using Microsoft.EntityFrameworkCore;
using TiendaSoftware1.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar el servicio de sesi�n
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Establece el tiempo de espera de la sesi�n
    options.Cookie.HttpOnly = true; // Evita el acceso a las cookies desde JavaScript
    options.Cookie.IsEssential = true; // La sesi�n es esencial para la aplicaci�n
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

// A�adir el middleware de sesi�n
app.UseSession();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); //prueba
