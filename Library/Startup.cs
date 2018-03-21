using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Library.Data;
using Library.Models;
using Library.Services;

namespace Library
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LibraryContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<LibraryContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 2;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 1;

                options.User.RequireUniqueEmail = true;
            });
            
            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            CreateRoles(serviceProvider).Wait();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roles = {"Administrator", "User"};

            foreach (var role in roles)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var admin = new ApplicationUser
            {
                UserName = "admin@adm.in",
                Email = "admin@adm.in",
                PhoneNumber = "123987456",
                FirstName = "Administrator",
                LastName = "Admin"
            };
            var adminPassword = "admin";

            var adminExists = await userManager.FindByEmailAsync(admin.Email);
            if (adminExists == null)
            {
                var adminCreate = await userManager.CreateAsync(admin, adminPassword);
                if (adminCreate.Succeeded)
                {
                    await userManager.AddToRolesAsync(admin, new[] {"Administrator", "User"});
                }
            }


            var user = new ApplicationUser
            {
                UserName = "user@us.er",
                Email = "user@us.er",
                PhoneNumber = "987123654",
                FirstName = "User",
                LastName = "Use"
            };
            var userPassword = "user";

            var userExists = await userManager.FindByEmailAsync(user.Email);
            if (userExists == null)
            {
                var userCreate = await userManager.CreateAsync(user, userPassword);
                if (userCreate.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
            }
        }
    }
}
