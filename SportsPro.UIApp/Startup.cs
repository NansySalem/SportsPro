using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

namespace SportsPro
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<SportsProContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("SportsPro")));

            services.AddRouting(options => {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = true;
            });
        }

        // Use this method to configure the HTTP request pipeline.
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //customize url --Doris 
                endpoints.MapControllerRoute(
                    name: "customers",
                    pattern: "customers/",
                    new { controller ="Customers", action ="Details"});
                
                endpoints.MapControllerRoute(
                    name: "incidents",
                    pattern: "incidents/",
                    new { controller = "Incidents", action = "Details" });

                endpoints.MapControllerRoute(
                    name: "technicians",
                    pattern: "technicians/",
                    new { controller = "Technicians", action = "Details" });
               
                endpoints.MapControllerRoute(
                   name: "products",
                   pattern: "products/",
                   new { controller = "Products", action = "Details" });
                
                //!!!Haven't created a about page yet!!!
                //endpoints.MapControllerRoute(
                //   name: "about",
                //   pattern: "about/",
                //   new { controller = "Home", action = "About" });


                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");


            });
          
           
        }
    }
}