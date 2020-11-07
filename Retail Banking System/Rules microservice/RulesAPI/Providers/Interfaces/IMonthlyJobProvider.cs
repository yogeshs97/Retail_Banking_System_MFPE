using RulesAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RulesAPI.Providers.Interfaces
{
    public interface IMonthlyJobProvider
    {
        void RunMonthlyJob();
    }
}
