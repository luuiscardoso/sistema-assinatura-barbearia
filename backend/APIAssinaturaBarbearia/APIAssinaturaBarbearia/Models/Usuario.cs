using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace APIAssinaturaBarbearia.Models
{
    public class Usuario : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenTempoExpiracao { get; set; }

        [StringLength(11)]
        public string? Cpf { get; set; }
    }
}
