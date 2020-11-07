using RulesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RulesAPI.Repositories.Interfaces
{
    public interface IRulesRepository
    {
        int GetMinBalance(int AccountID);

        //int GetBalance(int AccountID);

        //void ApplyMonthlyCharge();

        List<Account> GetAccounts();

        //int GetServiceCharge(string TransactionType, int amount);
    }
}
