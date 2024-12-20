using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAssinaturaBarbearia.Application.Exceptions
{
    public class ApplicationSearchPeriodOfInvalidDatesException : ApplicationException
    {
        public ApplicationSearchPeriodOfInvalidDatesException(string mensage) : base(mensage)
        {
        }
    }
}
