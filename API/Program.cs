using Application.Interfaces;
using Application.UseCases;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Base de datos 
builder.Services.AddDbContext<EcommerceDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Repositorios
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ICatalogoLecturaRepository, CatalogoLecturaRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Servicios de infraestructura
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddHttpClient<IBackofficeService, BackofficeService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["BackofficeApi:BaseUrl"]
            ?? throw new InvalidOperationException(
                "BackofficeApi:BaseUrl no configurada."));
    client.Timeout = TimeSpan.FromSeconds(10);
});

// Casos de uso 
builder.Services.AddScoped<RegistrarClienteUseCase>();
builder.Services.AddScoped<LoginClienteUseCase>();
builder.Services.AddScoped<ObtenerCatalogoUseCase>();
builder.Services.AddScoped<BuscarProductosUseCase>();
builder.Services.AddScoped<SincronizarProductoUseCase>();
builder.Services.AddScoped<CrearPedidoUseCase>();
builder.Services.AddScoped<AgregarDetallePedidoUseCase>();
builder.Services.AddScoped<ConfirmarPedidoUseCase>();
builder.Services.AddScoped<ObtenerPedidosClienteUseCase>();

// Autenticación JWT
var claveJwt = builder.Configuration["Jwt:Clave"]
    ?? throw new InvalidOperationException("Jwt:Clave no configurada.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Emisor"],
            ValidAudience = builder.Configuration["Jwt:Audiencia"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(claveJwt))
        };
    });

builder.Services.AddAuthorization();

// Swagger 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ecommerce API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa: Bearer {tu token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers();

var app = builder.Build();

// Pipeline HTTP 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();