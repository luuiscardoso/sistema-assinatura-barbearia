namespace APIAssinaturaBarbearia.Repositories.Interfaces
{
    public interface IUnityOfWork
    {
        IAssinaturaRepository AssinaturaRepository { get; }

        Task Commit();
    }
}
