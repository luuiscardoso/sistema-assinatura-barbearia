using System.ComponentModel.DataAnnotations;

namespace APIAssinaturaBarbearia.Application.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "E-mail obrigatório.")]
        [EmailAddress(ErrorMessage = "Insira um e-mail válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha obrigatória.")]
        public string Senha { get; set; }
    }
}
