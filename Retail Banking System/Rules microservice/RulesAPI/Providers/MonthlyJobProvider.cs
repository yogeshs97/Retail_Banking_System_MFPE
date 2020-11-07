using RulesAPI.Models;
using RulesAPI.Providers.Interfaces;
using RulesAPI.Repositories;
using RulesAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RulesAPI.Providers
{
    public class MonthlyJobProvider : IMonthlyJobProvider
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(MonthlyJobProvider));
        public readonly IRulesRepository _rules;
        public readonly IChargeRepository _charge;

        public MonthlyJobProvider()
        {
            _rules = new RulesRepository();
            _charge = new ChargeRepository();
        }

        /// <summary>
        /// It is taking all accounts from Account API
        /// and applying the service charge to those accounts who don't maintain miniumum balance
        /// </summary>
        public void RunMonthlyJob()
        {
            try
            {
                List<Account> AllAcc = _rules.GetAccounts();
                foreach (var x in AllAcc)
                {
                    if (x.Balance < x.minBalance)
                    {
                        float ServiceCharge = GetServiceCharge(x.AccountType);
                        var status = _charge.ApplyServiceCharge(x.AccountId, (int)ServiceCharge);
                        if (status.Message == "Your account has been credited")
                        {
                            _log4net.Info("Service charge deducted for the AccountID = " + x.AccountId);
                        }
                        else
                        {
                            _log4net.Info("Some Issue occured while deducting service charge for the AccountID = " + x.AccountId);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                _log4net.Error("Exception in RunMonthlyJob() in MonthlyJobProvider");
                _log4net.Error(e.Message);
                throw e;
            }
        }

        /// <summary>
        /// It gives service charge for account type
        /// </summary>
        /// <param name="AccountType"></param>
        /// <returns></returns>
        public float GetServiceCharge(string AccountType)
        {
            if (String.Equals(AccountType,"Savings"))
            {
                return 100;
            }
            else if (String.Equals(AccountType,"Current"))
            {
                return 200;
            }
            else
            {
                return 0;
            }
        }
    }
}
