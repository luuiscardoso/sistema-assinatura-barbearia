using System.Linq.Expressions;

namespace APIAssinaturaBarbearia.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T? Obter(Expression<Func<T, bool>> predicate, string propriedadeRelacionada);

        IEnumerable<T> Todos(string propriedadeRelacionada);

        void Atualizar (T entity);

        void Excluir (T entity);
    }
}
