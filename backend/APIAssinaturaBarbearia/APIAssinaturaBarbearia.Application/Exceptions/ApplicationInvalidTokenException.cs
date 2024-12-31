using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAssinaturaBarbearia.Application.Exceptions
{
    public class ApplicationInvalidTokenException : ApplicationException
    {
        public ApplicationInvalidTokenException(string message) : base(message)
        {
        }
    }
}
