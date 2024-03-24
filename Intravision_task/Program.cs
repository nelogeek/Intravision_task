using Intravision_task.Data;
using Intravision_task.Interfaces;
using Intravision_task.Models;
using Intravision_task.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Intravision_task
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DBConnection") ?? throw new InvalidOperationException("Connection string 'DBConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {

                // Disabling mail confirmation
                options.SignIn.RequireConfirmedEmail = false;

                // Do not require email uniqueness
                options.User.RequireUniqueEmail = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Password settings
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Disabling password length verification
                options.Password.RequiredLength = 0;
                options.Password.RequiredUniqueChars = 0;

                // Disabling checking for non-alphanumeric characters
                options.Password.RequireNonAlphanumeric = false;

                // Disabling checking for lowercase letters
                options.Password.RequireLowercase = false;

                // Disabling checking for uppercase letters
                options.Password.RequireUppercase = false;

            });


            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IDrinkService, DrinkService>();
            builder.Services.AddScoped<ICoinService, CoinService>();
            builder.Services.AddScoped<IMachineService, MachineService>();


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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            app.Run();
        }
    }
}
