using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAssinaturaBarbearia.Infrastructure.Email
{
    public class SmtpConfigs
    {
        public string? Remetente { get; set; }
        public string? Nome { get; set; }
        public string? Senha { get; set; }
        public string? Server { get; set; }
        public int Port { get; set; }
        public string? Security { get; set; }
    }
}
