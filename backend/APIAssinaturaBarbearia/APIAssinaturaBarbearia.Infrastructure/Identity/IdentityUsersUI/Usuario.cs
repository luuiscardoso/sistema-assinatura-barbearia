using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace APIAssinaturaBarbearia.Infrastructure.Identity.IdentityUsersUI
{
    public class Usuario : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenTempoExpiracao { get; set; }

        [StringLength(11)]
        public string? Cpf { get; set; }
    }
}
