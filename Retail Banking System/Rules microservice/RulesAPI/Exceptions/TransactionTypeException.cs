using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RulesAPI.Exceptions
{
    public class TransactionTypeException : Exception
    {
        public TransactionTypeException(string message) : base(message) { }
    }
}
