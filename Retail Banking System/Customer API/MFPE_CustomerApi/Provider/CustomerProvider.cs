using MFPE_CustomerApi.Models;
using MFPE_CustomerApi.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MFPE_CustomerApi.Provider
{
    public class CustomerProvider : IProvider<Customer>
    {
        IRepository<Customer> _repository;

        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(CustomerProvider));
        public CustomerProvider(IRepository<Customer> repository)
        {
            _repository = repository;

        }
        /// <summary>
        /// This function returns true if account is getting created with the help of Customer Id.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(Customer model)
        {
            try
            {
                var customerExist = _repository.Get(model.CustomerId);
                if (customerExist == null)
                {
                    if (_repository.Add(model))
                    {
                        _log4net.Info("Customer Id has been successfully created");
                        var client = new HttpClient();
                        client.BaseAddress = new Uri("https://localhost:44369");
                        HttpResponseMessage response1 = client.PostAsJsonAsync("api/Account/createAccount", new { CustomerId = Convert.ToInt32(model.CustomerId) }).Result;
                        var result1 = response1.Content.ReadAsStringAsync().Result;
                        AccountCreationStatus st = JsonConvert.DeserializeObject<AccountCreationStatus>(result1);

                        return true;
                    }
                }
                else
                {
                    _log4net.Warn("User already exist with Id :" + model.CustomerId);

                }
                return false;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// This function returns customer details for particular id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Customer Get(int id)
        {
            try
            {
                _log4net.Info("Customer details has been successfully recieved.");
                return _repository.Get(id);
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// This function returns full customer list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> GetAll() 
        {
            try
            {
                
                var customer = _repository.GetAll().ToList();
                if (customer.Count == 0)
                {
                    _log4net.Info("List is empty");
                    throw new System.ArgumentNullException("List is empty");
                }
                else
                {
                    return customer;
                }
                
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
