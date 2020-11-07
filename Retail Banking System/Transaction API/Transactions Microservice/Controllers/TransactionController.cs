using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Transactions_Microservice.Helper;
using Transactions_Microservice.Models;
using Transactions_Microservice.Provider;
using Transactions_Microservice.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Transactions_Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(TransactionController));
        private IProvider _provider;
        public TransactionController(IProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Based on Customer Id displaying List of transaction history
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        // GET: api/<TransactionController>
        [HttpGet]
        [Route("getTransactions/{CustomerId}")]
        public IActionResult getTransactions(int CustomerId)
        {
            
            if (CustomerId == 0)
            {
                _log4net.Info("Invalid Customer Id");
                return NotFound();
            }

            
            try
            {
                List<TransactionHistory> Ts = _provider.GetTransactionHistory(CustomerId);
                _log4net.Info("Transaction history returned for Customer Id: "+CustomerId);
                return Ok(Ts);
            }
            catch(Exception e)
            {
                throw e;
            }
     
        }

        /// <summary>
        /// This method calls Account api's  deposit method based on AccountId and amount
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST api/<TransactionController>
        [HttpPost]
        [Route("deposit")]
        public IActionResult deposit([FromBody] dynamic model)
        {
            
            if (Convert.ToInt32(model.AccountId) == 0 || Convert.ToInt32(model.amount) == 0)
            {
                _log4net.Info("Either AccountId or amount is invalid");
                return NotFound(new TransactionStatus() { message = "Withdraw Not Allowed" });

            }
            _log4net.Info("getAccount Api called");


            try
            {
                Account account = _provider.GetAccount(Convert.ToInt32(model.AccountId));

                _log4net.Info("deposit Api called");

                TransactionStatus status = _provider.Deposit(Convert.ToInt32(model.AccountId), Convert.ToInt32(model.amount));
                _provider.AddToTransactionHistory(status, account);
                _log4net.Info("Deposit done for Account Id: " +Convert.ToInt32(model.AccountId));
                return Ok(status);

            }
            catch (Exception e)
            {
              _log4net.Error(e.Message+": In Trasaction Controller");
               throw e;
            }
            
        }

        /// <summary>
        /// This method calls Account api's  withdraw method based on AccounId and amount
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST api/<TransactionController>
        [HttpPost]
        [Route("withdraw")]
        public IActionResult withdraw([FromBody] dynamic model)
        {
            
            if (Convert.ToInt32(model.AccountId) == 0 || Convert.ToInt32(model.amount) == 0)
            {
                _log4net.Info("Either AccountId or amount is invalid");
                 return NotFound(new TransactionStatus() { message = "Withdraw Not Allowed" });
            }

            try
            {
                Account account = _provider.GetAccount(Convert.ToInt32(model.AccountId));

                RuleStatus rulestatus = _provider.KnowRuleStatus(Convert.ToInt32(model.AccountId), Convert.ToInt32(model.amount), account);


                if (rulestatus.Status == "allowed")
                {
                    _log4net.Info("withdraw Api called");


                    TransactionStatus status = _provider.Withdraw(Convert.ToInt32(model.AccountId), Convert.ToInt32(model.amount));

                    if (status.message == null)
                    {
                        return NotFound(new TransactionStatus() { message = "Record Not Found" });
                    }

                    _provider.AddToTransactionHistory(status, account);
                    _log4net.Info("Withdraw done for Account Id: " + Convert.ToInt32(model.AccountId));
                    return Ok(status);
                }
                return NotFound(new TransactionStatus() { message = "Withdraw Not Allowed" });
            }
            
            catch (Exception e)
            {
               _log4net.Error(e.Message + ": In Trasaction Controller");
                throw e;
            }
            
        }

        /// <summary>
        /// This method calls withdraw and deposit function for a successful transaction
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //POST api/<TransactionController>
        [HttpPost]
        [Route("transfer")]
        public IActionResult transfer([FromBody] dynamic model)
        {
            if (Convert.ToInt32(model.Source_AccountId) == 0 || Convert.ToInt32(model.Target_AccountId) == 0  || Convert.ToInt32(model.amount) == 0)
            {
                _log4net.Info("Invalid SourceAccountId or TargetAccountId or amount");
                return NotFound(new TransactionStatus() { message = "Transfer Not Allowed" });
            }

            try
            {
                TransactionStatus statusfinal = new TransactionStatus();
                statusfinal.message = "Transaction Not Allowed";
                Account account = _provider.GetAccount(Convert.ToInt32(model.Source_AccountId));
                RuleStatus rulestatus = _provider.KnowRuleStatus(Convert.ToInt32(model.Source_AccountId), Convert.ToInt32(model.amount), account);
                if (rulestatus.Status == "allowed")
                {

                    statusfinal.message = "Transfered from Account no. " + model.Source_AccountId + " To Account no. " + model.Target_AccountId;

                    TransactionStatus status = _provider.Withdraw(Convert.ToInt32(model.Source_AccountId), Convert.ToInt32(model.amount));

                    if (status.message == null)
                    {
                        return NotFound(new TransactionStatus() { message = "Record Not Found" });
                    }

                    _provider.AddToTransactionHistory(status, account);
                    statusfinal.source_balance = status.destination_balance;


                    account = _provider.GetAccount(Convert.ToInt32(model.Target_AccountId));
                    status = _provider.Deposit(Convert.ToInt32(model.Target_AccountId), Convert.ToInt32(model.amount));

                    if (status.message == null)
                    {
                        return NotFound(new TransactionStatus() { message = "Record Not Found" });
                    }

                    _provider.AddToTransactionHistory(status, account);
                    statusfinal.destination_balance = status.destination_balance;
                    _log4net.Info("Transfer done from Account Id: " +Convert.ToInt32(model.Source_AccountId)+"to Account Id"+ Convert.ToInt32(model.Target_AccountId));
                    return Ok(statusfinal);
                }
                return NotFound();
            }
            catch(Exception e)
            {
                _log4net.Error(e.Message + ": In Trasaction Controller");
                throw e;
            }
        }
    }
}
