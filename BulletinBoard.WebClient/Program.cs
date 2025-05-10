using BulletinBoard.WebClient.Configurations;
using BulletinBoard.WebClient.Services.Interfaces;
using BulletinBoard.WebClient.Services;
using Microsoft.Extensions.Options;

namespace BulletinBoard.WebClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            builder.Services.Configure<ApiSettings>(
                builder.Configuration.GetSection("ApiSettings"));

            builder.Services.AddHttpClient("ApiClient", (sp, client) =>
            {
                var apiSettings = sp.GetRequiredService<IOptions<ApiSettings>>().Value;
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
            });

            // Services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPostService, PostService>();


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
                pattern: "{controller=Posts}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
