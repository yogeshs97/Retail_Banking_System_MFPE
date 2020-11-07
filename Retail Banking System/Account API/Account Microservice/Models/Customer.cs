using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Account_Microservice.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set;}
        public string Name { get; set; }
        public string Address { get; set;}
        public DateTime DOB { get; set; }
        public string PANno{get;set;}
    }
}
