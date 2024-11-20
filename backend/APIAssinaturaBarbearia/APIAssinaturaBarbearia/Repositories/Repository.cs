using APIAssinaturaBarbearia.Data;
using APIAssinaturaBarbearia.Models;
using APIAssinaturaBarbearia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        public void Criar(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Excluir(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<T?> Obter(Expression<Func<T, bool>> predicate, string propriedadeRelacionada)
        {
            return await _context.Set<T>().Include(propriedadeRelacionada).FirstOrDefaultAsync(predicate);
        }

        public async Task <IEnumerable<T>> Todos(string propriedadeRelacionada)      
        {
            return await _context.Set<T>().Include(propriedadeRelacionada).AsNoTracking().ToListAsync();
        }
    }
}
