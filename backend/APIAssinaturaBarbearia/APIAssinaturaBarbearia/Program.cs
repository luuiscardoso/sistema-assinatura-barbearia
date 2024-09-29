using APIAssinaturaBarbearia.Data;
using APIAssinaturaBarbearia.DTO.Mappings;
using APIAssinaturaBarbearia.Filtros;
using APIAssinaturaBarbearia.Repositories;
using APIAssinaturaBarbearia.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(FiltroExcecao));
})
.AddJsonOptions(options => 
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
)
.AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


string? conexao = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BdContext>(options => options.UseSqlServer(conexao));

builder.Services.AddAutoMapper(typeof(AssinaturaMappingProfile));
builder.Services.AddScoped<IAssinaturaRepositorie, AssinaturaRepositorie>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
