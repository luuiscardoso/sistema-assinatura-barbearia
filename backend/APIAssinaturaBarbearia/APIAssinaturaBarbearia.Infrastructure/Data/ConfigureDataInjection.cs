using APIAssinaturaBarbearia.Infrastructure.Identity.IdentityUsersUI;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAssinaturaBarbearia.Infrastructure.Data
{
    public static class ConfigureDataInjection
    {
        public static void AddData(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<BdContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
        }
    }
}
