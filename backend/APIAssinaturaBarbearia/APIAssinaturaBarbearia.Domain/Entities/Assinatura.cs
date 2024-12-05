using APIAssinaturaBarbearia.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace APIAssinaturaBarbearia.Domain.Entities
{
    public class Assinatura
    {
        public Assinatura(DateTime inicio, DateTime datafinal, bool status)
        {
            VerificaDatas(inicio, datafinal);
            Status = status;
        }

        public Assinatura() { }
        public int AssinaturaId { get; set; }

        public DateTime Inicio { get; private set; }

        public DateTime Fim { get; private set; }

        public bool Status { get; private set; }

        [JsonIgnore]
        public Cliente? Cliente { get; set; }

        private void VerificaDatas(DateTime inicio, DateTime novaDatafinal)
        {
            if (novaDatafinal <= inicio) throw new DomainPeriodOfInvalidDatesException("A data final da assinatura não pode ser menor que a data de inicio.");
            Inicio = inicio;
            Fim = novaDatafinal;
        }
        public void ValidarAtualizacao(bool status, DateTime novaDatafinal)
        {
            if (novaDatafinal >= Fim && !status) throw new DomainRenewalNotPaidException("A data final da assinatura não pode ser extendida pois ela não está ativa/paga.");
            Status = status;
            Fim = novaDatafinal;
        }
    }
}
