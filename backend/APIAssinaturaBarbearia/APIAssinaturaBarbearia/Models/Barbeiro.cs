using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace APIAssinaturaBarbearia.Models
{
    public class Barbeiro : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenTempoExpiracao { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11)]
        public string? Cpf { get; set; }
    }
}
