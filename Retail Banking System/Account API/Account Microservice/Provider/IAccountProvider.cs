using Account_Microservice.Models;
using Account_Microservice.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account_Microservice.Provider
{
    public interface IAccountProvider
    {
        AccountCreationStatus AddAccount(int CustomerId, string AccountType);
        List<AccountViewModel> getAllAccounts(int CustomerId);
        Account getCustomerAccount(int AccountId);
        IEnumerable<Statement> getStatement(int AccountId, DateTime from_date, DateTime to_date);
        TransactionStatus depositAccount(int AccountId, int amount);
        TransactionStatus withdrawAccount(int AccountId, int amount);
        IEnumerable<Account>getCustomersAllAccounts();
    }
}
