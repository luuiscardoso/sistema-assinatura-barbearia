using APIAssinaturaBarbearia.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace APIAssinaturaBarbearia.CrossCutting.IoC
{
    public static class DependenceInjection
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AssinaturaMappingProfile));
        }
    }
}
