using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace APIAssinaturaBarbearia.Models
{
    public class Assinatura
    {
        public Assinatura(DateTime inicio, DateTime fim, bool status)
        {
            Inicio = inicio;
            Fim = fim;
            Status = status;
        }

        public int AssinaturaId { get; set; }

        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public bool Status { get; set; }
        public Cliente? Cliente { get; set; }
    }
}
