using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account_Microservice.Models;
using Account_Microservice.Provider;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Account_Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(AccountController));
        private readonly IAccountProvider _Provider;

        public AccountController(IAccountProvider Provider)
        {
            _Provider = Provider;
        }
        
       /// <summary>
       /// this method creates current and savings account for given customerid
       /// </summary>
       /// <param name="obj"></param>
       /// <returns></returns>
        [HttpPost("createAccount")]
        public IActionResult createAccount([FromBody]dynamic obj)
       {
            if (Convert.ToInt32(obj.CustomerId) == 0)
            {
                _log4net.Warn("User has sent some invalid CustomerId "+ Convert.ToInt32(obj.CustomerId));
                return NotFound();
            }
            try
            {
                AccountCreationStatus acs, acs1 = new AccountCreationStatus();


                acs = _Provider.AddAccount(Convert.ToInt32(obj.CustomerId), "Savings");
                _log4net.Info("Savings account has been successfully created");
                acs1 = _Provider.AddAccount(Convert.ToInt32(obj.CustomerId), "Current");
                _log4net.Info("Current account has been successfully created");
                return Ok(acs1);
            }
            catch (Exception e)
            {
                _log4net.Error(e.Message);
                throw e;
            }
        }

        /// <summary>
        /// this method calls the provder's and returns the accounts list for the given customer id 
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getCustomerAccounts/{CustomerId}")]
        public IActionResult getCustomerAccounts(int CustomerId )
        {
            if (CustomerId == 0)
            {
                _log4net.Warn("invalid accountid "+CustomerId);
                return NotFound();
            }
            try
            {
          
             var  Listaccount = _Provider.getAllAccounts(CustomerId);
                _log4net.Info("Customer's account has been successfully returned");
                return Ok(Listaccount);

            }
            catch (Exception e)
            {
                _log4net.Error(e.Message);
                throw e;
            }
        }
        /// <summary>
        /// this method calls provider's method and returns the all list of accounts 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getAllCustomerAccounts")]
        public IActionResult getAllCustomerAccounts()
        {
            List<Account> Listaccount = new List<Account>();
            try
            {
                Listaccount = _Provider.getCustomersAllAccounts().ToList();
             
            }
            catch(Exception e)
            {
                _log4net.Error(e.Message);
                throw e;
            }
          
            _log4net.Info("Customer's account has been successfully returned");
            return Ok(Listaccount);
        }


        /// <summary>
        /// this method calls provder's method and returns the single account based on account id
        /// </summary>
        /// <param name="AccountId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getAccount/{AccountId}")]
        public IActionResult getAccount(int AccountId)
        {
            if (AccountId == 0)
            {
                _log4net.Warn("invalid accountid "+AccountId);
                return NotFound();

            }
            try
            { 
                Account a = new Account();
                a = _Provider.getCustomerAccount(AccountId);
                _log4net.Info("successfully returned acccount model");
                return Ok(a);
            }
             catch (Exception e)
            {
                _log4net.Error(e.Message);
                throw e;

            }
            
        }


        /// <summary>
        /// this method calls provder's method and returns the list of statement based on account id and from date and to date
        /// and in this method without from date and to date it gives the statement for current method
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("getAccountStatement")]
        //[Route("getAccountStatement")]
        public IActionResult getAccountStatement([FromBody]dynamic obj)
        {
            if (Convert.ToInt32(obj.AccountId) == 0)
            {
                _log4net.Warn("invalid accountid : "+Convert.ToInt32(obj.AccountId));
                return NotFound();
            }
            try
            {
                List<Statement> statements = new List<Statement>();


                DateTime? temp = null;

                if ((obj.from_date == temp) && (obj.to_date == temp))
                {
                    statements = _Provider.getStatement(Convert.ToInt32(obj.AccountId), Convert.ToDateTime(obj.from_date), Convert.ToDateTime(obj.to_date)).ToList();
                   
                }
                else
                {
                    statements = _Provider.getStatement(Convert.ToInt32(obj.AccountId), Convert.ToDateTime(obj.from_date),Convert.ToDateTime(obj.to_date));
                   
                }
                _log4net.Info("statement returned for given accountid");

                return Ok(statements);
            }
            catch(Exception e)
            {
                _log4net.Error(e.Message);
                throw e;
            }
        }



        /// <summary>
        /// this method calls provder's method and deposit given amount on given account id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("deposit")]
        public IActionResult deposit([FromBody] dynamic obj)
        {
            if (Convert.ToInt32(obj.AccountId) == 0 || Convert.ToInt32(obj.amount) == 0)
            {
                _log4net.Warn("invalid accountid or amount 0 for id: "+Convert.ToInt32(obj.AccountId));
                return NotFound();
            }

            try
            {
                TransactionStatus ts = new TransactionStatus();
                ts = _Provider.depositAccount(Convert.ToInt32(obj.AccountId), Convert.ToInt32(obj.amount));
                _log4net.Info("account has been credited successfully");
                return Ok(ts);
            }
            catch(Exception e)
            {
                _log4net.Error(e.Message);
                throw e;
            }
           
        }

        /// <summary>
        /// this method calls provder's method and withdraw given amount on given account id
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("withdraw")]
        public IActionResult withdraw([FromBody] dynamic obj)
        {

            if (Convert.ToInt32(obj.AccountId) == 0 || Convert.ToInt32(obj.amount) == 0)
            {
                _log4net.Warn("invalid accountid or amount 0");
                return NotFound();
            }
            try
            {
                TransactionStatus ts = new TransactionStatus();
                ts = _Provider.withdrawAccount(Convert.ToInt32(obj.AccountId), Convert.ToInt32(obj.amount));
              
                _log4net.Info("account has been debited successfully");
                return Ok(ts);

            }
          
            catch (Exception e)
            {
                _log4net.Error(e.Message);
                throw e;
            }


        }


    }
}



