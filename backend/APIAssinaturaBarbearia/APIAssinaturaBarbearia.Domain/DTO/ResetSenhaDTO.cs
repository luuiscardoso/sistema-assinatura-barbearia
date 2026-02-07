using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAssinaturaBarbearia.Domain.DTO
{
    public class ResetSenhaDTO
    {
        [Required(ErrorMessage = "Token obrigatório.")]
        public string? Token { get; set; }

        [Required(ErrorMessage = "E-mail obrigatório.")]
        [EmailAddress(ErrorMessage = "Insira um e-mail válido.")]
        public string? EmailConfirmacao { get; set; }

        [Required(ErrorMessage = "Nova senha obrigatória.")]
        public string? NovaSenha { get; set; }

        [Required(ErrorMessage = "Nova senha obrigatória.")]
        [Compare("NovaSenha", ErrorMessage = "As senhas não correspondem.")]
        public string? NovaSenhaConfirmacao { get; set; }
    }
}
