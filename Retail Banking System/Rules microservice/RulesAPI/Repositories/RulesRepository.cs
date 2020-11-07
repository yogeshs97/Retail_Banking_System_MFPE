using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Newtonsoft.Json;
using RulesAPI.Exceptions;
using RulesAPI.Models;
using RulesAPI.Models.Results;
using RulesAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace RulesAPI.Repositories
{
    public class RulesRepository : IRulesRepository
    {
        ///private List<Account> accounts;
        private Client _client;
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(RulesRepository));
        public RulesRepository()
        {
            _client = new Client();
        }

        /// <summary>
        /// It takes all the accounts from Account API
        /// </summary>
        /// <returns></returns>
        public List<Account> GetAccounts()
        {
            try
            {
                List<Account> acc = null;
                
                HttpClient client = _client.AccountClient();
                HttpResponseMessage response = client.GetAsync("api/Account/getAllCustomerAccounts").Result;
                var result = response.Content.ReadAsStringAsync().Result;
                acc = JsonConvert.DeserializeObject<List<Account>>(result);
                
                return acc;
            }
            catch(Exception e)
            {
                _log4net.Error("Exception in getting accounts from Account API");
                throw e;
            }
            
            
        }
       
        /// <summary>
        /// It takes account from Account api by giving it Account ID
        /// Then it returns the minimum balance of that account
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public int GetMinBalance(int AccountID)
        {
            try
            {
                HttpClient client = _client.AccountClient();
                HttpResponseMessage response = client.GetAsync("api/Account/getAccount/" + AccountID).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                Account acc = JsonConvert.DeserializeObject<Account>(result);
                return acc.minBalance;
            }
            catch(NullReferenceException e)
            {
                _log4net.Error("The account is not returned from Account API.",e);
                throw e;
            }
            catch(Exception e)
            {
                _log4net.Error("Exception occured while getting Account by ID");
                throw e;
            }
        }


        
    }
}
