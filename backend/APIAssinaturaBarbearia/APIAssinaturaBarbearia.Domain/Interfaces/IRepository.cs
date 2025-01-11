using System.Linq.Expressions;

namespace APIAssinaturaBarbearia.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> ObterAsync(Expression<Func<T, bool>> predicate, string propriedadeRelacionada);

        Task<IEnumerable<T>> TodosAsync(string propriedadeRelacionada);

        virtual void Criar(T entity) { }

        void Atualizar (T entity);

        void Excluir (T entity);
    }
}
