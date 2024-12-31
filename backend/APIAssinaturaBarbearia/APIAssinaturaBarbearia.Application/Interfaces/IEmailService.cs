using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAssinaturaBarbearia.Application.Interfaces
{
    public interface IEmailService
    {
        Task EnviarEmailAsync(string email, string titulo, string corpo);
    }
}
