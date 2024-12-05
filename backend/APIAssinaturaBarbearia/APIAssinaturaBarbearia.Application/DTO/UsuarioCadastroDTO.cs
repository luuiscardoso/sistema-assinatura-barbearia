using System.ComponentModel.DataAnnotations;

namespace APIAssinaturaBarbearia.Application.DTO
{
    public class UsuarioCadastroDTO
    {
        [Required(ErrorMessage = "E-mail obrigatório.")]
        [EmailAddress(ErrorMessage = "Insira um e-mail válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha obrigatória.")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF com tamanho inválido.")]
        public string? Cpf { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string? Nome { get; set; }
    }
}
