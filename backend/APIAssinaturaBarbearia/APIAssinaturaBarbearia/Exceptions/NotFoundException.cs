namespace APIAssinaturaBarbearia.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string mensage) : base(mensage)
        {
        }
    }
}
