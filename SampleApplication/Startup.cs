using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SampleApplication.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SampleApplication
{
    public class Startup
    {
        public const string DefaultPassword = "W0rdPass!";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                CreateRolesAndUsersAsync(roleManager, userManager).GetAwaiter().GetResult();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        private async Task CreateRolesAndUsersAsync(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            await CreateRolesAsync(roleManager);
            await CreateStartupUsersAsync(userManager);
        }

        private async Task CreateStartupUsersAsync(UserManager<IdentityUser> userManager)
        {
            List<IdentityUser> admins = new List<IdentityUser>
            {
                new IdentityUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    EmailConfirmed = true
                }
            };
            List<IdentityUser> employees = new List<IdentityUser>
            {
                new IdentityUser
                {
                    UserName = "employee1@admin.com",
                    Email = "employee1@admin.com",
                    EmailConfirmed = true
                },
                new IdentityUser
                {
                    UserName = "employee2@admin.com",
                    Email = "employee2@admin.com",
                    EmailConfirmed = true
                },
                new IdentityUser
                {
                    UserName = "employee3@admin.com",
                    Email = "employee3@admin.com",
                    EmailConfirmed = true
                }
            };
            //List<IdentityUser> customers = new List<IdentityUser>
            //{
            //    new IdentityUser
            //    {
            //        UserName = "customer1@example.net",
            //        Email = "customer1@example.net",
            //        EmailConfirmed = true
            //    },
            //    new IdentityUser
            //    {
            //        UserName = "customer2@example.net",
            //        Email = "customer2@example.net",
            //        EmailConfirmed = true
            //    }
            //};
            foreach (var admin in admins)
            {
                var foundUser = await userManager.FindByNameAsync(admin.UserName);
                if (foundUser != null)
                {
                    continue;
                }

                var userCreateResult = await userManager.CreateAsync(admin, DefaultPassword);
                if (userCreateResult.Succeeded)
                {
                    IdentityUser user = await userManager.FindByNameAsync(admin.UserName);
                    var AddUserRoleResult = await userManager.AddToRoleAsync(user, "Admin");
                    if (AddUserRoleResult.Succeeded)
                    {
                        continue;
                    }
                    else
                    {
                        throw new Exception($"Could not assign 'Admin' role to '{admin.UserName}' user.");
                    }
                }
                else
                {
                    throw new Exception($"Could not create '{admin.UserName}' user.");
                }

                
            }
            foreach (var employee in employees)
            {
                var foundUser = await userManager.FindByNameAsync(employee.UserName);
                if (foundUser != null)
                {
                    continue;
                }

                var userCreateResult = await userManager.CreateAsync(employee, DefaultPassword);
                if (userCreateResult.Succeeded)
                {
                    IdentityUser user = await userManager.FindByNameAsync(employee.UserName);
                    var AddUserRoleResult = await userManager.AddToRoleAsync(user, "Employee");
                    if (AddUserRoleResult.Succeeded)
                    {
                        continue;
                    }
                    else
                    {
                        throw new Exception($"Could not assign 'Employee' role to '{employee.UserName}' user.");
                    }
                }
                else
                {
                    throw new Exception($"Could not create '{employee.UserName}' user.");
                }
            }
            //foreach (var customer in customers)
            //{
            //    var foundUser = await userManager.FindByNameAsync(customer.UserName);
            //    if (foundUser != null)
            //    {
            //        continue;
            //    }

            //    var userCreateResult = await userManager.CreateAsync(customer, DefaultPassword);
            //    if (userCreateResult.Succeeded)
            //    {
            //        IdentityUser user = await userManager.FindByNameAsync(customer.UserName);
            //        var AddUserRoleResult = await userManager.AddToRoleAsync(user, "Customer");
            //        if (AddUserRoleResult.Succeeded)
            //        {
            //            continue;
            //        }
            //        else
            //        {
            //            throw new Exception($"Could not assign 'Customer' role to '{customer.UserName}' user.");
            //        }
            //    }
            //    else
            //    {
            //        throw new Exception($"Could not create '{customer.UserName}' user.");
            //    }
            //}
        }

        private async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin"
                },
                new IdentityRole
                {
                    Name = "Employee"
                }
                //new IdentityRole
                //{
                //    Name = "Customer"
                //}
            };

            foreach (var role in roles)
            {
                if (await roleManager.RoleExistsAsync(role.Name)) continue;
                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded) continue;

                throw new Exception($"Could not create '{role.Name}' role.");
            }
        }
    }
}
