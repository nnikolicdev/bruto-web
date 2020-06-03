using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bruto_web.Data;
using bruto_web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace bruto_web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {

            // Ovo sluzi da ucita trenutne configuracije
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // Samo kada se pokrece
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // Dozvoljava promene da se vide u real time.
            services.AddMvc().AddRazorRuntimeCompilation();


            // Dodajem ovo da bi posle mogli biti prosledjeno dalje
            services.Configure<ConfigModel>(Configuration.GetSection("DbHosts"));

            // Kreacija konteksta za db msssql
            services.AddDbContext<DbUserContext>();

            // Session manager za login/logout
            services.AddDistributedMemoryCache();
            services.AddSession();
        }

        // Butno za Route HTTP pipe
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Bitno je ovo dodati pre MVC
            app.UseSession();

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
