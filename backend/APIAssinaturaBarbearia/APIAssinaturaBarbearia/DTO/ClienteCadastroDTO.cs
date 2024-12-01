using System.ComponentModel.DataAnnotations;

namespace APIAssinaturaBarbearia.DTO
{
    public class ClienteCadastroDTO
    {
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF com tamanho inválido.")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(80)]
        public string Nome { get; set; }
    }
}
