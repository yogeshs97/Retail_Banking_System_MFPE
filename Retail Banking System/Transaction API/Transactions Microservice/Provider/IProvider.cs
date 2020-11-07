using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transactions_Microservice.Models;

namespace Transactions_Microservice.Provider
{
    public interface IProvider
    {
        bool AddToTransactionHistory(TransactionStatus status,Account account);
        List<TransactionHistory> GetTransactionHistory(int CustomerId);
        Account  GetAccount(int AccountId);
        TransactionStatus Deposit(int AccountId,int amount);
        TransactionStatus Withdraw(int AccountId, int amount);
        RuleStatus KnowRuleStatus(int AccountId, int amount, Account account);
    }
}
