using APIAssinaturaBarbearia.Application.Interfaces;
using APIAssinaturaBarbearia.Application.Options;
using APIAssinaturaBarbearia.Application.Services;
using APIAssinaturaBarbearia.CrossCutting.IoC;
using APIAssinaturaBarbearia.CrossCutting.Middlewares;
using APIAssinaturaBarbearia.CrossCutting.Middlewares.GlobalExceptions;
using APIAssinaturaBarbearia.CrossCutting.Security;
using APIAssinaturaBarbearia.Domain.Interfaces;
using APIAssinaturaBarbearia.Filtros;
using APIAssinaturaBarbearia.Infrastructure.Data;
using APIAssinaturaBarbearia.Infrastructure.Email;
using APIAssinaturaBarbearia.Infrastructure.Identity;
using APIAssinaturaBarbearia.Infrastructure.Identity.IdentityUsersUI;
using APIAssinaturaBarbearia.Infrastructure.Identity.IdentityUserTokens;
using APIAssinaturaBarbearia.Infrastructure.Repositories;
using APIAssinaturaBarbearia.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAWSLambdaHosting(LambdaEventSource.ApplicationLoadBalancer);

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
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
#endregion

#region Swagger Configs
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "apibarbearia", Version = "v1" });

    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));

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

#region Infra
builder.Services.AddIdentity<Usuario, IdentityRole>()
                .AddEntityFrameworkStores<BdContext>()
                .AddDefaultTokenProviders();

string? conexao = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BdContext>(options => options.UseNpgsql(conexao));

builder.Services.Configure<SmtpConfigs>(builder.Configuration
                                         .GetSection("Email"));

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
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IResetSenhaTokenRepository<CustomIdentityUserTokens>, ResetSenhaTokenRepository>();
builder.Services
    .AddOptions<SeedOptions>()
    .Bind(builder.Configuration.GetSection("IdentitySeed"));
#endregion

builder.Services.ConfigureRateLimiter(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Staging"))
{
    app.ConfigureExceptionHandler();
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.RoutePrefix = "swagger";
        c.SwaggerEndpoint("v1/swagger.json", "Projeto Barbearia API v1");
    });

    await IdentitySeed.SeedAsync(app.Services);
}

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.UseWhen(
    context => context.Request.Path.StartsWithSegments("/search"),
    app =>
    {
        app.UseMiddleware<ValidateSearchFieldsMiddleware>();
        Console.WriteLine("entrou no middleware");
    }
);

app.MapControllers();
app.Run();


public partial class Program { }
