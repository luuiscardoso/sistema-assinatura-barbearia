using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAssinaturaBarbearia.Infrastructure.Identity.IdentityUserTokens
{
    public class CustomIdentityUserTokens : IdentityUserToken<string>
    {
        public DateTime? Criacao { get; set; }
        public DateTime? Expiracao { get; set; }
    }
}
