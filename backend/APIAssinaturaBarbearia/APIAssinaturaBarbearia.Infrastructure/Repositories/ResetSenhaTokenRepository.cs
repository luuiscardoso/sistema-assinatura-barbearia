using APIAssinaturaBarbearia.Infrastructure.Data;
using APIAssinaturaBarbearia.Infrastructure.Identity.IdentityUserTokens;
using APIAssinaturaBarbearia.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace APIAssinaturaBarbearia.Infrastructure.Repositories
{
    public class ResetSenhaTokenRepository : IResetSenhaTokenRepository<CustomIdentityUserTokens>
    {
        private readonly BdContext _context;
        public ResetSenhaTokenRepository(BdContext context)
        {
            _context = context;
        }
        public void Criar(CustomIdentityUserTokens entity)
        {
            _context.Set<CustomIdentityUserTokens>().Add(entity);
            _context.SaveChangesAsync();
        }

        public void Excluir(CustomIdentityUserTokens entity)
        {
            _context.Set<CustomIdentityUserTokens>().Remove(entity);
            _context.SaveChangesAsync();
        }

        public Task<CustomIdentityUserTokens?> ObterAsync(Expression<Func<CustomIdentityUserTokens, bool>> predicate)
        {
            return _context.Set<CustomIdentityUserTokens>().FirstOrDefaultAsync(predicate);
        }
    }
}
