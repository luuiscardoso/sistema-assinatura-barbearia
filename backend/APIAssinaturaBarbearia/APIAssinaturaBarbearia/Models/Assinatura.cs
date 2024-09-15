using System.ComponentModel.DataAnnotations.Schema;

namespace APIAssinaturaBarbearia.Models
{
    public class Assinatura
    {
        public int AssinaturaId { get; set; }

        [Column(TypeName = "time(0)")]
        public TimeSpan TempoRestante {  get; set; }

        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public bool Status { get; set; }
        public Cliente? Cliente { get; set; }
    }
}
