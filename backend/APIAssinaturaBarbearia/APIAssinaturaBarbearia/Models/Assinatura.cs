using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace APIAssinaturaBarbearia.Models
{
    public class Assinatura
    {
        public int AssinaturaId { get; set; }

        [Required(ErrorMessage = "A data de inicio é obrigatória.")]
        public DateTime Inicio { get; set; }

        [Required(ErrorMessage = "A data final é obrigatória.")]
        public DateTime Fim { get; set; }

        [Required(ErrorMessage = "O status da assinatura é obrigatória.")]
        public bool Status { get; set; }

        [JsonIgnore]
        public Cliente? Cliente { get; set; }
    }
}
