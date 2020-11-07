using MFPE_CustomerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace MFPE_CustomerApi.Repository
{
    public class CustomerRepository : IRepository<Customer>
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(CustomerRepository));
        public static List<Customer> ListCustomer = new List<Customer>()
        {
            new Customer {CustomerId = 1, Name = "Riya", Address = "Ballupur Road, Dehradun",  PANno = "DCRP124" , DOB = Convert.ToDateTime("1998-11-20 01:02:01 AM")},
            new Customer {CustomerId = 2, Name = "Reema", Address = "Vijay Colony, Delhi", PANno = "DCRP23456", DOB = Convert.ToDateTime("1999-11-10 02:02:01 AM")}
        };
        /// <summary>
        /// This function returns added customer list and returning boolean value is customer id is getting created.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Add(Customer item)
        {
            try
            {
                ListCustomer.Add(item);
                _log4net.Info("Customer details has been successfully entered.");
                return true;
            }
            catch(Exception e)
            {
                _log4net.Error("Error" + e.Message);
            }
            return false;
        }
        /// <summary>
        /// This function returns all the customer details based on id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Customer Get(int id)
        {
            try
            {
                _log4net.Info("Customer details  has been successfully retrieved");
                return ListCustomer.Find(p => p.CustomerId == id);
            }
            catch(Exception e)
            {
                _log4net.Error("Error " + e.Message);
                throw e;
            }
            
        }
        /// <summary>
        /// This function returns all the customer lists.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> GetAll()
        {
           
                _log4net.Info("Customer details is finally recieved.");
                return ListCustomer.ToList();
            
        }

    }
}
