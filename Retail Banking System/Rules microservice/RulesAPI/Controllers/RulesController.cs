using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RulesAPI.Exceptions;
using RulesAPI.Models;
using RulesAPI.Providers.Interfaces;
using RulesAPI.Repositories;
using RulesAPI.Repositories.Interfaces;

namespace RulesAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/rules/")]
    [ApiController]
    public class RulesController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(RulesController));
        public readonly IBalanceProvider _balance;
        public readonly IMonthlyJobProvider _MonthlyJob;

        public RulesController(IBalanceProvider balance, IMonthlyJobProvider monthlyJob)
        {
            this._balance = balance;
            this._MonthlyJob = monthlyJob;
        }

        /// <summary>
        /// This method runs and checks if the date is 1 or not
        /// if the day is 1, it applies service charge to those who are not maintaining minimum balance
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Route("api/Rules/MonthlyBatchJob")]
        [Route("MonthlyBatchJob")]
        public IActionResult MonthlyBatchJob()
        {
            try
            {
                if (DateTime.Now.Day == 1)
                {
                    _log4net.Info("Monthly Checking Started");
                    _MonthlyJob.RunMonthlyJob();
                    _log4net.Info("Monthly Service Charge Deduction Completed");
                }
                return StatusCode(200);
            }
            catch(Exception e)
            {
                _log4net.Error("Monthy Charge Couldn't be applied due to exception");
                _log4net.Error(e.Message);
                return StatusCode(500);
            }
        }
        
        [HttpGet]
        [Route("api/Rules/GetServiceCharge")]
        public float GetServiceCharge(string AccountType)
        {
            if (AccountType == "Savings")
            {
                return 100;
            }
            else if(AccountType == "Current")
            {
                return 200;
            }
            else
            {
                return 0;
            }
        }
        
        /// <summary>
        /// This function take account id and balance(might left after withdraw)
        /// It evaluates if the transaction can be done or not.
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="Balance"></param>
        /// <returns></returns>

        [HttpGet]
        //[Route("api/Rules/EvaluateMinBal")]
        [Route("EvaluateMinBal/{AccountID}/{Balance}")]
        public RuleStatus EvaluateMinBal(int AccountID,int Balance)
        {
            _log4net.Info("Evaluating Minimum Balance");
            try
            {
                int MinBalance = _balance.GetMinBalance(AccountID);

                if (Balance >= MinBalance)
                {
                    return new RuleStatus { Status = "allowed" };
                }
                else
                {
                    return new RuleStatus { Status = "denied" };
                }
            }
            catch(NullReferenceException e)
            {
                _log4net.Error("NullReferenceException caught. Issue in calling Account API");
                throw e;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}