using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MFPE_CustomerApi.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]

        public string Name { get; set; }
        [Required]

        public string Address { get; set; }
        [Required]

        public DateTime DOB { get; set; }
        [Required]

        public string PANno { get; set; }
        



    }
}
