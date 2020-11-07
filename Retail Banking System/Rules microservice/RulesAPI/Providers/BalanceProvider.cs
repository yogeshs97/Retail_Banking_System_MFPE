using RulesAPI.Providers.Interfaces;
using RulesAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RulesAPI.Providers
{
    public class BalanceProvider : IBalanceProvider
    {
        RulesRepository _rules;
        public BalanceProvider()
        {
            _rules = new RulesRepository();
        }
        /// <summary>
        /// it transfers control from controller to repository
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public int GetMinBalance(int AccountID)
        {
            try
            {
                return _rules.GetMinBalance(AccountID);
            }
            catch(NullReferenceException e)
            {
                throw e;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
