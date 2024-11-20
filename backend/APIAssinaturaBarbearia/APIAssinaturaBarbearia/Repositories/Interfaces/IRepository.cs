using APIAssinaturaBarbearia.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace APIAssinaturaBarbearia.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> Obter(Expression<Func<T, bool>> predicate, string propriedadeRelacionada);

        Task<IEnumerable<T>> Todos(string propriedadeRelacionada);

        void Criar(T entity);

        void Atualizar (T entity);

        void Excluir (T entity);
    }
}
