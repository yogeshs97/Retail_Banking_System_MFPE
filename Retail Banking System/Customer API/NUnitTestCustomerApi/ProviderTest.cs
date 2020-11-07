using MFPE_CustomerApi.Models;
using MFPE_CustomerApi.Provider;
using MFPE_CustomerApi.Repository;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NUnitTestCustomerApi
{
    class ProviderTest
    {
        private Mock<IRepository<Customer>> _config;
        private CustomerProvider TokenObj;


        [SetUp]
        public void Setup()
        {
            _config = new Mock<IRepository<Customer>>();
            TokenObj = new CustomerProvider(_config.Object);

        }
        [Test]
        public void getCutomerAccounts_Called_When_Given_Valid_CustomerId()
        {
            _config.Setup(p => p.Get(1)).Returns(new Customer { });

            var result = TokenObj.Get(1);

            Assert.That(result, Is.Not.Null);
        }

        
        [Test]
        public void Get_WhenCalled_ReturnsListOfCustomerDetails()
        {

            _config.Setup(repo => repo.GetAll()).Returns(new List<Customer> {new Customer()
                {
                    CustomerId = 1,
                    Name = "Mansi",
                    Address = "Dehradun",
                    DOB = Convert.ToDateTime("1998-11-20 01:02:01 AM"),
                    PANno = "DCRP4536",
                } });

            var result = TokenObj.GetAll();
            Assert.That(result.Count, Is.EqualTo(1));
        }
        [Test]
        public void GetAll_Called_When_Throws_Exception()
        {
            _config.Setup(repo => repo.GetAll()).Returns((new List<Customer> { }));
            Assert.That(() => TokenObj.GetAll(), Throws.ArgumentNullException);
        }
    }
}
