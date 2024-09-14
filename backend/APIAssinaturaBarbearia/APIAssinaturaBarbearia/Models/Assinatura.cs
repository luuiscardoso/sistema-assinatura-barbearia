namespace APIAssinaturaBarbearia.Models
{
    public class Assinatura
    {
        public int AssinaturaId { get; set; }
        public TimeSpan TempoRestante {  get; set; }

        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public bool Status { get; set; }
        public Cliente? Cliente { get; set; }
    }
}
