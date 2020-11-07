using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transactions_Microservice.Models;

namespace Transactions_Microservice.Repository
{
    public interface IRepository
    {
       bool AddToTransactionHistory(TransactionStatus status,Account account);
       List<TransactionHistory> GetTransactionHistory(int CustomerId);
    }
}
