using MFPE_CustomerApi.Controllers;
using MFPE_CustomerApi.Models;
using MFPE_CustomerApi.Provider;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NUnitTestCustomerApi
{
    class CustomerTest
    {
        private Mock<IProvider<Customer>> _config;
        private CustomerController _controller;


        [SetUp]
        public void Setup()
        {
            _config = new Mock<IProvider<Customer>>();
            _controller = new CustomerController(_config.Object);

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

            var result = _controller.Get();
            Assert.That(result, Is.InstanceOf<OkObjectResult>());

        }

        [Test]
        public void Called_When_Given_Valid_CustomerId()
        {
            _config.Setup(p => p.Get(1)).Returns(new Customer { });


            var result = _controller.getCustomerDetails(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void Called_When_Given_CustomerId_Notinthelist()
        {
            var result = _controller.getCustomerDetails(0);

            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public void returnValidCreateCustomer_Is_Valid()
        {
            Customer customer = new Customer();
            _config.Setup(p => p.Add(customer)).Returns(true);
            var result = _controller.createCustomer(customer);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void Called_When_CreateCustomer_Is_NULL()
        {
            
            var result = _controller.createCustomer(null) as StatusCodeResult;

            Assert.AreEqual(409,result.StatusCode);
        }

    }
}
