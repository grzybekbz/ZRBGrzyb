using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using ZRBGrzyb.Models;
using ZRBGrzyb.Infrastructure;
using System.IO;

namespace ZRBGrzyb {

    public class Startup {

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {

        services.AddDbContext<ApplicationDbContext>(options => 
                    options.UseSqlServer(
                    Configuration["Data:Grzyb:ConnectionString"]));

            services.AddDbContext<AppIdentityDbContext>(options => 
                    options.UseSqlServer(
                    Configuration["Data:Grzyb:ConnectionString"]));

            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(),
                    Configuration["Data:Grzyb:PhotosDirectory"])));

            services.AddTransient<IUserValidator<AppUser>, CustomUserValidator>();

            services.AddIdentity<AppUser, IdentityRole>(opts => {
                    opts.Password.RequiredLength = 6;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireLowercase = false;
                    opts.Password.RequireUppercase = true;
                    opts.Password.RequireDigit = true; })
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders()
                .AddPasswordValidator<PasswordValidator<AppUser>>()
                .AddErrorDescriber<CustomIdentityErrorDescriber>();

            services.AddTransient<IRepository, EFRepository>();
            services.AddSession();
            services.AddMvc(/*options => {
                options.Filters.Add(new RequireHttpsAttribute());
            }*/);
            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            } else {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseMvc(routes => {

                routes.MapRoute(
                    name: null,
                    template: "Realizacje/{category}/Strona{photoPage:int}",
                    defaults: new { controller = "Photo", action = "Gallery" });

                routes.MapRoute(
                    name: null,
                    template: "Realizacje/{category}/Strona{photoPage:int}",
                    defaults: new { controller = "Photo", action = "Gallery", photoPage = 1 });

                routes.MapRoute(
                    name: null,
                    template: "Realizacje/{category}",
                    defaults: new { controller = "Photo", action = "Gallery", photoPage = 1 });

                routes.MapRoute(
                    name: null,
                    template: "Realizacje",
                    defaults: new { controller = "Photo", action = "Gallery" });

                routes.MapRoute(
                    name: null,
                    template: "",
                    defaults: new { controller = "Home", action = "Index", photoPage = 1 });

                routes.MapRoute(
                    name: null,
                    template: "Oferta",
                    defaults: new { controller = "Home", action = "Offer", photoPage = 1 });

                routes.MapRoute(
                    name: null,
                    template: "Praca",
                    defaults: new { controller = "Home", action = "Job", photoPage = 1 });

                routes.MapRoute(
                    name: null,
                    template: "Kontakt",
                    defaults: new { controller = "Home", action = "Contact", photoPage = 1 });

                routes.MapRoute(
                    name: null,
                    template: "Admin",
                    defaults: new { controller = "Admin", action = "Index" });

                routes.MapRoute(
                    name: null, 
                    template: "{controller}/{action}/{id?}");
            });
        }
    }
}
