using RulesAPI.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RulesAPI.Repositories.Interfaces
{
    public interface IChargeRepository
    {
        TransactionStatus ApplyServiceCharge(int AccountID, int Amount);
    }
}
