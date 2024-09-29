using APIAssinaturaBarbearia.Models;

namespace APIAssinaturaBarbearia.Repositories.Interfaces
{
    public interface IAssinaturaRepositorie
    {
        Assinatura? Obter(int id);

        IEnumerable<Assinatura> ObterTodas();

        void Criar(Cliente cliente);

        void Atualizar(Assinatura assinatura);

        void Excluir(Assinatura assinatura);   
    }
}
