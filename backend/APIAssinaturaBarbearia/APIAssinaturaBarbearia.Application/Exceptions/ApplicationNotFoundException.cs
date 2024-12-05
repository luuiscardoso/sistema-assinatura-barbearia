namespace APIAssinaturaBarbearia.Application.Exceptions
{
    public class ApplicationNotFoundException : ApplicationException
    {
        public ApplicationNotFoundException(string mensage) : base(mensage)
        {
        }
    }
}
