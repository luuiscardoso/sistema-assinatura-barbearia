namespace APIAssinaturaBarbearia.Exceptions
{
    public class AlreadyHasSubscriptionException : ApplicationException
    {
        public AlreadyHasSubscriptionException(string mensage) : base(mensage) 
        {    
        }
    }
}
