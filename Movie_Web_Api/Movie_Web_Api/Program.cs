using Movie_Web_Api.Models;
using Microsoft.EntityFrameworkCore;
using Movie_Web_Api.Repository;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using Movie_Web_Api.Dto;
using Microsoft.AspNetCore.Identity;
using Movie_Web_Api.Helpers;
using Movie_Web_Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
//using AutoMapper.Execution.Microsoft.Dependencyinjection;
namespace Movie_Web_Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddScoped<IMovieRepository, MovieRepository>();

            builder.Services.AddScoped<IActorReposatory, ActorReposatory>();
            builder.Services.AddScoped<ICinamasReprosatory, CinamasReprosatory>();
            builder.Services.AddScoped<IProducersRepository, ProducersRepository>();

            //////////////////////////////////////////////////////
            
            //builder.Services.AddControllers();
            //builder.Services.AddSession();
            //builder.Services.AddHttpContextAccessor();


            builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            });


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(o =>
              {
                  o.RequireHttpsMetadata = false;
                  o.SaveToken = false;
                  o.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = true,
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidIssuer = builder.Configuration["JWT:Issuer"],
                      ValidAudience = builder.Configuration["JWT:Audience"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                  };
              });


            builder.Services.AddDbContext<AppDbContext>(options => {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Movie_Api"));
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("default", (builder) =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("default");
            app.UseAuthorization();
         //   app.UseSession();

            app.MapControllers();

            app.Run();
        }
    }
}