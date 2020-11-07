using Account_Microservice.Controllers;
using Account_Microservice.Models;
using Account_Microservice.Models.ViewModel;
using Account_Microservice.Provider;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace AccountTests
{
    [TestFixture]
    public class Tests
    {
        private Mock<IAccountProvider> config;
        private AccountController TokenObj;
        [SetUp]
        public void Setup()
        {
            config = new Mock<IAccountProvider>();
            TokenObj = new AccountController(config.Object);
        }

        [Test]
        public void returnValidCreateCustomer()
        {
            config.Setup(p => p.AddAccount(1, "Savings")).Returns(new AccountCreationStatus {
                Message = "Account has been successfully created",
                AccountId = 1
            });

            var result = TokenObj.createAccount(new Customer { CustomerId = 1 });

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void return_notfound_when_invaid_customerid()
        {
            config.Setup(p => p.AddAccount(1, "Savings")).Returns(new AccountCreationStatus
            {
            });

            var result = TokenObj.createAccount(new Customer { CustomerId = 0 });

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }


        [Test]
        public void getaccount_Called_When_Given_Valid_AccountId()
        {
            config.Setup(p => p.getCustomerAccount(1)).Returns(new Account
            {
                AccountId=1,
                CustomerId=1,
                AccountType="Savings",
                Balance=1000,
                minBalance=1000
             });

               var result = TokenObj.getAccount(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }


        [Test]
        public void getaccount_Called_When_Given_InValid_AccountId()
        {
            config.Setup(p => p.getCustomerAccount(1)).Returns(new Account
            {
            });

            var result = TokenObj.getAccount(0);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }



        [Test]
        public void getCutomerAccounts_Called_When_Given_Valid_CustomerId()
        {
            config.Setup(p => p.getAllAccounts(1)).Returns(new List<AccountViewModel>{new AccountViewModel()
            {
                Id = 1,
                Balance = 1000,
            } });

            var result = TokenObj.getCustomerAccounts(1);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void getCutomerAccounts_Called_When_Given_InValid_CustomerId()
        {
            config.Setup(p => p.getAllAccounts(1)).Returns(new List<AccountViewModel>{new AccountViewModel()
            {
                Id = 1,
                Balance = 1000,
            } });

            var result = TokenObj.getCustomerAccounts(0);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }


        [Test]
        public void getAllCusotmerAccounts_Called_When_ListisnotEmpty()
        {
            config.Setup(p => p.getCustomersAllAccounts()).Returns(new List<Account>{new Account()
            {
                AccountId = 1,
                CustomerId = 1,
                AccountType = "Savings",
                Balance = 1000,
                minBalance = 1000
            } });

            var result = TokenObj.getAllCustomerAccounts();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }



        [Test]
         public void getstatements_Called_When_Given_Valid_AccountId()
         {

             config.Setup(p => p.getStatement(1,It.IsAny<DateTime>(),It.IsAny<DateTime>())).Returns(new List<Statement>{ new Statement() {

             } });

             var result = TokenObj.getAccountStatement(new StatementViewModel1() { AccountId = 1, from_date = DateTime.Now, to_date = DateTime.Now });

             Assert.That(result, Is.InstanceOf<OkObjectResult>());
         }
        

         [Test]
         public void getstatements_Called_When_Given_InvalidValid_AccountId()
         {
 
             var result = TokenObj.getAccountStatement(new StatementViewModel1() { AccountId = 0, to_date = DateTime.Now, from_date= DateTime.Now });

             Assert.That(result, Is.InstanceOf<NotFoundResult>());
         }


        [Test]
        public void deposit_Called_When_Given_Valid_AccountId()
        {

            config.Setup(p => p.depositAccount(1,200)).Returns( new TransactionStatus() {

            } );

            var result = TokenObj.deposit(new AccountViewModel1{AccountId=1,amount=200});

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void deposit_Called_When_Given_InValid_AccountId()
        {

            config.Setup(p => p.depositAccount(1, 200)).Returns(new TransactionStatus()
            {

            });

            var result = TokenObj.deposit(new AccountViewModel1 { AccountId = 0, amount = 200 });

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }







        [Test]
        public void withdraw_Called_When_Given_Valid_AccountId()
        {

            config.Setup(p => p.withdrawAccount(1, 200)).Returns(new TransactionStatus()
            {

            });

            var result = TokenObj.withdraw(new AccountViewModel1 { AccountId = 1, amount = 200 });

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public void withdraw_Called_When_Given_InValid_AccountId()
        {

            config.Setup(p => p.withdrawAccount(1, 200)).Returns(new TransactionStatus()
            {

            });

            var result = TokenObj.withdraw(new AccountViewModel1 { AccountId = 0, amount = 200 });

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }







    }
}
