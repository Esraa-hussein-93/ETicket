using Microsoft.EntityFrameworkCore;
using New_Eticket.DataAccess;
using New_Eticket.Repositories;
using New_Eticket.Repositories.IRepositories;

namespace New_Eticket
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IActorRepository,    ActorRepository>();
            builder.Services.AddScoped<ICinemaRepository,   CinemaRepository>();
            builder.Services.AddScoped<IMovieRepository,    MovieRepository>();

            //builder.Services.AddTransient<IMovieRepository,MovieRepository>();

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
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=User}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
