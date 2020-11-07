using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MFPE_CustomerApi.Models
{
    public class CustomerCreationStatus
    {
        public string Message { get; set; }

        
        public int CustomerId { get; set; }

    }
}
