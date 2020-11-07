using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Transactions_Microservice.Helper
{
    public class Client
    {
        public HttpClient AccountDetails()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44369");
            return client;
        }

        public HttpClient RuleApi()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44315");
            return client;
        }
    }
}
