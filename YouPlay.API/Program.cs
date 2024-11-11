using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using YouPlay.Business;
using YouPlay.Business.DTOs.GameDTOs;
using YouPlay.Core.Entities;
using YouPlay.Data;
using YouPlay.Data.Contexts;

namespace YouPlay.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<GameGetDto>();


            //builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
            //{
            //    opt.User.RequireUniqueEmail = true;
            //    opt.Password.RequiredUniqueChars = 2;
            //    opt.Password.RequiredLength = 8;
            //}).AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,

                    ValidIssuer = builder.Configuration.GetSection("JWT:issuer").Value,
                    ValidAudience = builder.Configuration.GetSection("JWT:audience").Value,
                    ValidateLifetime = true,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:secretkey").Value)),
                    ClockSkew = TimeSpan.Zero
                };
            });




            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequiredUniqueChars = 2;
                opt.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddRepositories(builder.Configuration.GetConnectionString("default"));
            builder.Services.AddServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();
           

            app.MapControllers();

            app.Run();
        }
    }
}
