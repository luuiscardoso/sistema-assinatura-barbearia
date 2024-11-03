using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APIAssinaturaBarbearia.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }

        public string? Cpf { get; set; }

        public string? Nome { get; set; }

        [JsonIgnore]
        public Assinatura? Assinatura { get; set; }

        public int AssinaturaId { get; set; }
    }
}
