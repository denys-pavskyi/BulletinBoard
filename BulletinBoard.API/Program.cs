
using AutoMapper;
using BulletinBoard.BLL.Interfaces;
using BulletinBoard.BLL.Other;
using BulletinBoard.BLL.Services;
using BulletinBoard.DAL.Configurations;
using BulletinBoard.DAL.Repositories;
using BulletinBoard.DAL.Repositories.Interfaces;
using BulletinBoardAPI.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoardAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration["ConnectionStrings:MsSqlServer"]));


            //mapper config
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MapperProfile()); });

            var mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);

            // Repositories

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();

            // Services

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
