namespace APIAssinaturaBarbearia.Application.Exceptions
{
    public class ApplicationAlreadyHasSubscriptionException : ApplicationException
    {
        public ApplicationAlreadyHasSubscriptionException(string mensage) : base(mensage) 
        {    
        }
    }
}
