using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RulesAPI.Exceptions
{
    public class AccountNotFound : Exception
    {
        public AccountNotFound(string message) : base(message)
        {

        }
    }
}
