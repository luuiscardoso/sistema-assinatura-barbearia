using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAssinaturaBarbearia.Domain.Exceptions
{
    public class DomainRenewalNotPaidException : ApplicationException
    {
        public DomainRenewalNotPaidException(string message) : base(message) { }
    }
}
