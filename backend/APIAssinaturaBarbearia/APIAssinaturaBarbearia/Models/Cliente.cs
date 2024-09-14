namespace APIAssinaturaBarbearia.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        private int Cpf { get; set; }

        public Assinatura? Assinatura { get; set; }

        public int AssinaturaId { get; set; }
    }
}
