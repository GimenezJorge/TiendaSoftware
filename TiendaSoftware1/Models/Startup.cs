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
