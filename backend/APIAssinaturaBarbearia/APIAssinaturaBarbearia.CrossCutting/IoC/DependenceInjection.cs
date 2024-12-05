using APIAssinaturaBarbearia.Application.DTO.Mappings;
using APIAssinaturaBarbearia.Application.Interfaces;
using APIAssinaturaBarbearia.Application.Services;
using APIAssinaturaBarbearia.Domain.Interfaces;
using APIAssinaturaBarbearia.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAssinaturaBarbearia.CrossCutting.IoC
{
    public static class DependenceInjection
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AssinaturaMappingProfile));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IAssinaturaRepository, AssinaturaRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUnityOfWork, UnityOfWork>();
            services.AddScoped<IAssinaturaService, AssinaturaService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IAssinaturaClienteHandlerService, AssinaturaClienteHandlerService>();
        }
    }
}
