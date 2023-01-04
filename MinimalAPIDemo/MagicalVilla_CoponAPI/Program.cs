using AutoMapper;
using FluentValidation;
using MagicalVilla_CoponAPI;
using MagicalVilla_CoponAPI.Data;
using MagicalVilla_CoponAPI.EndPoints.AuthEntPoints;
using MagicalVilla_CoponAPI.EndPoints.CouponEndPoints;
using MagicalVilla_CoponAPI.models;
using MagicalVilla_CoponAPI.Models;
using MagicalVilla_CoponAPI.Models.DTO;
using MagicalVilla_CoponAPI.Repository.AuthRepository;
using MagicalVilla_CoponAPI.Repository.CouponRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JTW Authorization header using the Bearer scheme. \r\n\r\n"+
                      "Enter 'Bearer' [space] and then your token in the text input below. \r\n\r\n"+
                      "Example: \"Bearer 123456abcdef\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});
builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata= false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("ApiSettings:Secret"))),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//app.UseAuthentication();
//app.UseAuthorization();

#region CouponEndPoints

app.ConfigureCouponGetEndPoints();
app.ConfigurePostCouponEndPoint();
app.ConfigureCouponUpdateEndPoint();
app.ConfigureDeleteCouponEndPoint();

#endregion

#region AuthEndPoints

app.ConfigureCreateUserEndPoint();
app.ConfigureLoginEndPoint();

#endregion

app.UseHttpsRedirection();

app.Run();