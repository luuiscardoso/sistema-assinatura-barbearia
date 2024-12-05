using APIAssinaturaBarbearia.Filtros;
using APIAssinaturaBarbearia.Infrastructure.Data;
using APIAssinaturaBarbearia.Infrastructure.Identity.IdentityUsersUI;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using APIAssinaturaBarbearia.CrossCutting.IoC;
using APIAssinaturaBarbearia.CrossCutting.Security;

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
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


public partial class Program { }
