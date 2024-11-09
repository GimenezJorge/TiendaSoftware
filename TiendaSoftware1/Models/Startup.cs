using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TiendaSoftware1.Models;

namespace TiendaSoftware1
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Configuración del contexto de la base de datos
            services.AddDbContext<Bdg3Context>(options =>
                options.UseSqlServer("Server=DESKTOP-O0OIP5E;Database=TiendaSoftware;Trusted_Connection=True;"));

            // Agregar controladores y vistas
            services.AddControllersWithViews();

            // Configuración de sesiones
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Duración de la sesión
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Habilitar el uso de sesiones
            app.UseSession();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
