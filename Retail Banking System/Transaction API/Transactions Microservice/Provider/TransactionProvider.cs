using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Transactions_Microservice.Helper;
using Transactions_Microservice.Models;
using Transactions_Microservice.Repository;

namespace Transactions_Microservice.Provider
{
    public class TransactionProvider : IProvider
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(TransactionProvider));
        private IRepository _repo;
        public TransactionProvider(IRepository repo)
        {
            _repo = repo;
        }
        /// <summary>
        /// From here we are calling repository  method to add a transaction history after successful transaction
        /// </summary>
        /// <param name="status"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool AddToTransactionHistory(TransactionStatus status,Account account)
        {

            try
            {
                bool output = _repo.AddToTransactionHistory(status, account);

                if (output == false)
                {
                    throw new System.ArgumentNullException("Not able to add data to transaction history for: "+account.AccountId);
                }
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message+": In Provider method");
                throw e;
            }
            
            return true;
        }

        /// <summary>
        /// Here we are communicating with Account api to actually deposit to a given AccountId
        /// </summary>
        /// <param name="AccountId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public TransactionStatus Deposit(int AccountId, int amount)
        {
            TransactionStatus status = new TransactionStatus();
            try
            {
                Client obj = new Client();
                HttpClient client = obj.AccountDetails();
                HttpResponseMessage response = client.PostAsJsonAsync("api/Account/deposit", new { AccountId = AccountId, amount = amount }).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                status = JsonConvert.DeserializeObject<TransactionStatus>(result);
            }
            catch(Exception e)
            {
                _log4net.Error("Not able to deposit in account with account id " +AccountId+ " and amount " +amount);
                throw e;
            }
            
            return status;
        }

        /// <summary>
        /// From Here we are calling Account api to get Account object
        /// </summary>
        /// <param name="AccountId"></param>
        /// <returns></returns>
        public Account GetAccount(int AccountId)
        {
            Account account = new Account();
            try
            {
                Client obj = new Client();
                HttpClient client = obj.AccountDetails();

                HttpResponseMessage response = client.GetAsync("api/Account/getAccount/" + AccountId).Result;

                var result = response.Content.ReadAsStringAsync().Result;
                account = JsonConvert.DeserializeObject<Account>(result);
            }
            
            catch (Exception e) {
               _log4net.Error("Exception occured while getting Account details for account id: "+ AccountId);
                throw e;
            }

            return account;
        }

        /// <summary>
        /// This method returns Transaction History for a particular CustomerId
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<TransactionHistory> GetTransactionHistory(int CustomerId)
        {
            try
            {
                List<TransactionHistory> list = _repo.GetTransactionHistory(CustomerId);
                return list;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Here we are calling Rule api to know whether we can actually 
        /// withdraw from an account means balance is sufficient or not
        /// </summary>
        /// <param name="AccountId"></param>
        /// <param name="amount"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public RuleStatus KnowRuleStatus(int AccountId, int amount,Account account)
        {
            RuleStatus rulestatus = new RuleStatus();
            try
            {
                Client obj = new Client();
                HttpClient client = obj.RuleApi();
                int balance = account.Balance - amount;
                HttpResponseMessage response = client.GetAsync("api/Rules/EvaluateMinBal/" + AccountId + "/" + balance).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                rulestatus = JsonConvert.DeserializeObject<RuleStatus>(result);
            }
            catch(Exception e)
            {
               _log4net.Error("Insufficient Balance for account Id: " +AccountId);
                throw e;
            }

            return rulestatus;
        }

        /// <summary>
        /// Here we are communicating with Account api to actually 
        /// withdraw from a given AccountId
        /// </summary>
        /// <param name="AccountId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public TransactionStatus Withdraw(int AccountId, int amount)
        {
            TransactionStatus status = new TransactionStatus();
            try
            {
                Client obj = new Client();
                HttpClient client = obj.AccountDetails();
                HttpResponseMessage response = client.PostAsJsonAsync("api/Account/withdraw", new { AccountId = AccountId, amount = amount }).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                status = JsonConvert.DeserializeObject<TransactionStatus>(result);
            }
            catch(Exception e)
            {
              _log4net.Error("Not able to withdraw from account with account id " + AccountId + " and amount " + amount);
                throw e;
            }
            return status;
        }
    }
}
