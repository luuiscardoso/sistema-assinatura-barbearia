using APIAssinaturaBarbearia.Data;
using APIAssinaturaBarbearia.Repositories.Interfaces;

namespace APIAssinaturaBarbearia.Repositories
{
    public class UnityOfWork : IUnityOfWork
    {
        private IAssinaturaRepository? _assinaturaRepository;
        private BdContext _context;

        public UnityOfWork(BdContext bdContext)
        {
            _context = bdContext;
        }

        public IAssinaturaRepository AssinaturaRepository{
            get { return _assinaturaRepository = _assinaturaRepository ?? new AssinaturaRepository(_context); }
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
