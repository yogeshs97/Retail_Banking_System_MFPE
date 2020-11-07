using Newtonsoft.Json;
using RulesAPI.Models.Results;
using RulesAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RulesAPI.Repositories
{
    public class ChargeRepository : IChargeRepository
    {
        private readonly Client _client;
        RulesRepository _rules;
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(ChargeRepository));

        public ChargeRepository()
        {
            _client = new Client();
            _rules = new RulesRepository();
        }
        /// <summary>
        /// It applies service charge for the account that doesn't maintain the minimum balance
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="Amount"></param>
        /// <returns>The transaction status if it is successful</returns>
        public TransactionStatus ApplyServiceCharge(int AccountID, int Amount)
        {
            try
            {
                HttpClient client = _client.TransactionClient();
                dynamic model = new { AccountId = AccountID, amount = Amount };
                var jsonString = JsonConvert.SerializeObject(model);
                var obj = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync("api/Transaction/withdraw", obj).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                TransactionStatus status = JsonConvert.DeserializeObject<TransactionStatus>(result);

                return status;
            }
            catch(Exception e)
            {
                _log4net.Error("Exception thrown while withdrawing Charge from transaction API");
                throw e;
            }

        }
    }
}
