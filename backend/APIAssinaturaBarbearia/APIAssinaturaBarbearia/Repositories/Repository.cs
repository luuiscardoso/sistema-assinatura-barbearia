using APIAssinaturaBarbearia.Data;
using APIAssinaturaBarbearia.Models;
using APIAssinaturaBarbearia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace APIAssinaturaBarbearia.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly BdContext _context;

        public Repository(BdContext bdContext)
        {
            _context = bdContext;
        }
        public void Atualizar(T entity)
        {
            _context.Set<T>().Update(entity);
        }


        public void Excluir(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public T? Obter(Expression<Func<T, bool>> predicate, string propriedadeRelacionada)
        {
            return _context.Set<T>().Include(propriedadeRelacionada).FirstOrDefault(predicate);
        }

        public IEnumerable<T> Todos(string propriedadeRelacionada)      
        {
            return _context.Set<T>().Include(propriedadeRelacionada).AsNoTracking().ToList();
        }
    }
}
