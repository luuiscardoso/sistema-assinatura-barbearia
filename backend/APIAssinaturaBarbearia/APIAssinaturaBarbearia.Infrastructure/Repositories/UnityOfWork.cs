using APIAssinaturaBarbearia.Domain.Interfaces;
using APIAssinaturaBarbearia.Infrastructure.Data;
using APIAssinaturaBarbearia.Infrastructure.Repositories;

namespace APIAssinaturaBarbearia.Infrastructure.Repositories
{
    public class UnityOfWork : IUnityOfWork
    {
        private IAssinaturaRepository? _assinaturaRepository;
        private IClienteRepository? _clienteRepository;
        private BdContext _context;

        public UnityOfWork(BdContext bdContext)
        {
            _context = bdContext;
        }

        public IAssinaturaRepository AssinaturaRepository{
            get { return _assinaturaRepository = _assinaturaRepository ?? new AssinaturaRepository(_context); }
        }

        public IClienteRepository ClienteRepository
        {
            get { return _clienteRepository = _clienteRepository ?? new ClienteRepository(_context); }
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
