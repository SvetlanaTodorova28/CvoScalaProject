using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scala.StockSimulation.Core.Entities;
using Scala.StockSimulation.Web.Areas.Admin.Services;
using Scala.StockSimulation.Web.Data;
using SmartBreadcrumbs.Extensions;
using System.Reflection;
using Scala.StockSimulation.Web.Services;
using Scala.StockSimulation.Web.Services.Interfaces;

namespace Scala.StockSimulation.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddScoped<IClaimsService, ClaimsService>();
            builder.Services.AddScoped<IFormBuilder, FormBuilder>();
            builder.Services.AddDbContext<ScalaStockSimulationDbContext>
            (options => options.UseSqlServer(builder.Configuration.GetConnectionString("Scala-Stock-Simulation-Db")));
            
            builder.Services.AddBreadcrumbs(Assembly.GetExecutingAssembly(), options =>
            {
                options.TagName = "nav";
                options.DontLookForDefaultNode = true;
                options.TagClasses = " ";
                options.OlClasses = "breadcrumb";
                options.LiClasses = "breadcrumb-item";
                options.ActiveLiClasses = "breadcrumb-item active";
                options.SeparatorElement = "<li class=\"separator\">>></li>";
            });

            // Add services to the container.

            //add identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            }).AddEntityFrameworkStores<ScalaStockSimulationDbContext>()
            .AddDefaultTokenProviders();
            builder.Services.AddAuthorization(options => {
                options.AddPolicy("TeacherPolicy", policy => policy.RequireRole("Teacher"));
                options.AddPolicy("StudentPolicy", policy => policy.RequireRole("Student"));
               
            });
            builder.Services.AddControllersWithViews();

            builder.Services.AddSession();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
            name: "areaDefault",
            pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
    }
}