using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Transactions_Microservice.Models
{
    public class TransactionHistory
    {
        [Key]
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public int CustomerId { get; set; }
        public string message { get; set; }
        public int source_balance { get; set; }
        public int destination_balance { get; set; }
        public DateTime DateOfTransaction { get; set; }

    }
}
