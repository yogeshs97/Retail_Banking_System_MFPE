using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Transactions_Microservice.Models
{
    public class TransactionStatus
    {
        public string message { get; set; }
        public int source_balance{ get; set; }
        public int destination_balance { get; set; }
    }
}
