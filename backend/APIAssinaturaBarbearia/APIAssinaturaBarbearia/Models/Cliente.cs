using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APIAssinaturaBarbearia.Models
{
    public class Cliente
    {
        public Cliente(string? cpf, string? nome, int assinaturaId)
        {
            Cpf = cpf;
            Nome = nome;
            AssinaturaId = assinaturaId;
        }

        public int ClienteId { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter exatamente 11 números.")]
        public string? Cpf { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(80)]
        public string? Nome { get; set; }

        [JsonIgnore]
        public Assinatura? Assinatura { get; set; }

        public int AssinaturaId { get; set; }
    }
}
