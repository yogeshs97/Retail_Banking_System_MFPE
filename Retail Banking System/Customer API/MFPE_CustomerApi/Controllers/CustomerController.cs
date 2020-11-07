using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using MFPE_CustomerApi.Models;
using MFPE_CustomerApi.Provider;
using MFPE_CustomerApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MFPE_CustomerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(CustomerController));
        private readonly IProvider<Customer> _provider;

        public CustomerController(IProvider<Customer> provider)
        {
            _provider = provider;
        }
        /// <summary>
        /// This method returns full Customer list.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get")]
        public IActionResult Get()
        {
            try
            {
                _log4net.Info("Get Api Initiated");
                return Ok(_provider.GetAll());
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// This method is returning customer details according to Customer Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getCustomerDetails/{id}")]
        public IActionResult getCustomerDetails([FromRoute]int id)
        {
            if (id == 0)
            {
                _log4net.Warn("User has sent some invalid CustomerId");
                return BadRequest();
            }
            Customer listCustomer = new Customer();
            try
            {
                listCustomer = _provider.Get(id);
                if (listCustomer == null)
                {
                    _log4net.Error("No record found for the user Id :" + id);
                    return NotFound();
                }
                else
                {
                    _log4net.Info("Customer's Details has been successfully returned");
                    return Ok(listCustomer);
                }
            }
            catch(Exception e)
            {
                _log4net.Error("Error occurred while calling get method" + e.Message);
                return new StatusCodeResult(500);
            }
        }
        
        /// <summary>
        /// This function returns customer creation status after creating customer account.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("createCustomer")]
        public IActionResult createCustomer([FromBody]Customer customer)
        {
            if(!ModelState.IsValid)
			{
                _log4net.Info("No Customer has been returned");
                return BadRequest();
            }
            try
            {
                bool result = _provider.Add(customer);

                if (result)
                {
                    _log4net.Info("Customer has been successfully created");
                    CustomerCreationStatus cts = new CustomerCreationStatus();
                    cts.Message = "Customer and its account has been successfully created.";
                    cts.CustomerId = customer.CustomerId;
                    return Ok(cts);
                }
                else
                {
                    return new StatusCodeResult(409);
                }
            }
            catch (Exception e)
            {
                _log4net.Error("Error occurred while calling post method" + e.Message);
                return new StatusCodeResult(500);
            }
        }
    }
}
