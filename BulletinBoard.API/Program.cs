
using AutoMapper;
using BulletinBoard.BLL.Interfaces;
using BulletinBoard.BLL.Other;
using BulletinBoard.BLL.Services;
using BulletinBoard.DAL.Configurations;
using BulletinBoard.DAL.Repositories;
using BulletinBoard.DAL.Repositories.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using BulletinBoard.API.Extensions;
using BulletinBoard.BLL.Models.Requests;
using BulletinBoard.BLL.Validations.Posts;
using BulletinBoard.API.Middlewares;
using BulletinBoard.BLL.Other.Hashers;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace BulletinBoard.API
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
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            // Services

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            // Validators

            builder.Services.AddTransient<IPostCollectionValidators, PostCollectionValidators>();


            // Jwt

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidAudience = builder.Configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chatHub"))
                                context.Token = accessToken;
                            return Task.CompletedTask;
                        }
                    };
                });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGetWithAuth();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
