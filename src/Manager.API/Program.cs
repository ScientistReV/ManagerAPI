using System.Resources;
using System.Data;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Runtime.InteropServices;
using System.Buffers;
using System.Reflection.PortableExecutable;
using System.Collections.Immutable;
using Manager.Services.DTO;
using Manager.Services.Services;
using Manager.Services.Interfaces;
using Manager.Domain.Entities;
using Manager.API.ViewModels;
using Manager.Infra.Context;
using Manager.Infra.Repositories;
using Manager.Infra.Interfaces;
using AutoMapper;
using Manager.Infra.Mappings;
using Microsoft.EntityFrameworkCore;
using Manager.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Manager.API.Token;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using EscNet.IoC.Cryptography;
using EscNet.IoC.Hashers;
using Isopoh.Cryptography.Argon2;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Dependency Injection
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddSingleton(d => builder.Configuration);

#endregion
#region AutoMapper


var autoMapperConfig = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<User, UserDTO>().ReverseMap();
    cfg.CreateMap<CreateUserViewModel, UserDTO>().ReverseMap();
    cfg.CreateMap<UpdateUserViewModel, UserDTO>().ReverseMap();
});

builder.Services.AddSingleton(autoMapperConfig.CreateMapper());
#endregion
#region DbContext
builder.Services.AddDbContext<ManagerContext>(options => options.UseSqlServer(
builder.Configuration.GetConnectionString("MANAGER_USER")), ServiceLifetime.Transient);
#endregion
#region JWT

#region 
builder.Services.AddRijndaelCryptography(builder.Configuration["Cryptography"]);
#endregion

builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Manager API",
                    Version = "v1",
                    Description = "API ",
                    Contact = new OpenApiContact
                    {
                        Name = "Ygor Peixoto",
                        Email = "ygoor.rev0@gmail.com",
                        Url = new Uri("https://github.com/ScientistReV")
                    },
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please use the Bearer <TOKEN>",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
                });
            });
var secretKey = builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
    
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});