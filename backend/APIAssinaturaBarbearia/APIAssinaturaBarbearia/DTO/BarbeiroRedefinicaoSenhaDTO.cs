using System.ComponentModel.DataAnnotations;

namespace APIAssinaturaBarbearia.DTO
{
    public class BarbeiroRedefinicaoSenhaDTO
    {
        [Required(ErrorMessage = "E-mail obrigatório.")]
        [EmailAddress(ErrorMessage = "Insira um e-mail válido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Senha atual obrigatória.")]
        public string SenhaAtual { get; set; }

        [Required(ErrorMessage = "Nova senha obrigatória.")]
        public string NovaSenha { get; set; }
    }
}
