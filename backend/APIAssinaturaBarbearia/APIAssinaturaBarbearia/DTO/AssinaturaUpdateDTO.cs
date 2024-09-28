using System.ComponentModel.DataAnnotations;

namespace APIAssinaturaBarbearia.DTO
{
    public class AssinaturaUpdateDTO
    {
        public DateTime Fim { get; set; }

        public bool Status { get; set; }

        [StringLength(11)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter exatamente 11 números.")]
        public string? Cpf { get; set; }

        [StringLength(80)]
        public string? Nome { get; set; }
    }
}
