using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAssinaturaBarbearia.Application.Exceptions
{
    public class ApplicationInvalidRefreshTokenException : ApplicationException
    {
        public ApplicationInvalidRefreshTokenException(string message) : base(message)
        {
        }
    }
}
