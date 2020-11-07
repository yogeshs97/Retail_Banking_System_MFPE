using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Account_Microservice.Models
{
    public class Statement
    {
        [Key]
        public int StatementId { get; set; }
        public int AccountId { get; set; }
        public DateTime date { get; set; }
        public string refno {get;set;}
        public DateTime ValueDate { get; set; }
        public int Withdrawal { get; set; }
        public int Deposit { get; set; }
        public int ClosingBalance { get; set;}
    }
}
