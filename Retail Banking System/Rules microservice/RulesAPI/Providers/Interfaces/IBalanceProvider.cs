using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RulesAPI.Providers.Interfaces
{
    public interface IBalanceProvider
    {
        int GetMinBalance(int AccountID);
    }
}
