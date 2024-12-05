namespace APIAssinaturaBarbearia.Domain.Interfaces
{
    public interface IUnityOfWork
    {
        IAssinaturaRepository AssinaturaRepository { get; }
        IClienteRepository ClienteRepository { get; }
        Task Commit();
    }
}
