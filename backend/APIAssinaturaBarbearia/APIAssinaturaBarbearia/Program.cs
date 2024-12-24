using APIAssinaturaBarbearia.Filtros;
using APIAssinaturaBarbearia.Infrastructure.Data;
using APIAssinaturaBarbearia.Infrastructure.Identity.IdentityUsersUI;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using APIAssinaturaBarbearia.CrossCutting.IoC;
using APIAssinaturaBarbearia.CrossCutting.Security;
using APIAssinaturaBarbearia.CrossCutting.GlobalExceptions;
using APIAssinaturaBarbearia.Application.Interfaces;
using APIAssinaturaBarbearia.Application.Services;
using APIAssinaturaBarbearia.Domain.Interfaces;
using APIAssinaturaBarbearia.Infrastructure.Identity;
using APIAssinaturaBarbearia.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Controllers Configs
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(FiltroExcecao));
})
.AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
)
.AddNewtonsoftJson(options =>
{
    //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
#endregion

#region Swagger Configs
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "apibarbearia", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer JWT ",
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
                         new string[] {}
                    }
                });
});
#endregion

#region Data
builder.Services.AddIdentity<Usuario, IdentityRole>()
                .AddEntityFrameworkStores<BdContext>()
                .AddDefaultTokenProviders();

string? conexao = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BdContext>(options => options.UseSqlServer(conexao));
#endregion

#region Autenticação/JWT Configs
builder.Services.ConfigureAuthenticationAndAuthorization(builder.Configuration);
#endregion

#region Injecao de Dependencia
builder.Services.AddServices();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IAssinaturaRepository, AssinaturaRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUnityOfWork, UnityOfWork>();
builder.Services.AddScoped<IAssinaturaService, AssinaturaService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IAssinaturaClienteHandlerService, AssinaturaClienteHandlerService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAdminService, AdminService>();
#endregion

builder.Services.ConfigureRateLimiter(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


public partial class Program { }
