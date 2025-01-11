using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace APIAssinaturaBarbearia.Domain.Interfaces
{
    public interface ITokenRepository<T> where T : class
    {
        Task<T?> ObterAsync(Expression<Func<T, bool>> predicate);
        void Criar(T entity);
        void Excluir(T entity);
    }
}
