using APIAssinaturaBarbearia.Domain.Interfaces;
using APIAssinaturaBarbearia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace APIAssinaturaBarbearia.Infrastructure.Repositories
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

        public virtual void Criar(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Excluir(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<T?> ObterAsync(Expression<Func<T, bool>> predicate, string propriedadeRelacionada)
        {
            return await _context.Set<T>().Include(propriedadeRelacionada).FirstOrDefaultAsync(predicate);
        }

        public async Task <IEnumerable<T>> TodosAsync(string propriedadeRelacionada)      
        {
            return await _context.Set<T>().Include(propriedadeRelacionada).AsNoTracking().ToListAsync();
        }
    }
}
