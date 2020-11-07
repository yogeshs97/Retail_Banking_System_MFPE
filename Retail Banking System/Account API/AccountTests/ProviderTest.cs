using Account_Microservice.Provider;
using Account_Microservice.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Account_Microservice.Models;
using System.Linq;

namespace AccountTests
{
    [TestFixture]
    class ProviderTest
    {
        private Mock<IAccountRepository> config;
        private AccountProvider TokenObj;

        [SetUp]
        public void Setup()
        {
            config = new Mock<IAccountRepository>();
            TokenObj = new AccountProvider(config.Object);
        }

        [Test]
        public void returnValidCreateCustomer()
        {
            config.Setup(p => p.AddAccount(1, "Savings")).Returns(new AccountCreationStatus
            {
               
            });

            var result = TokenObj.AddAccount(1,"Savings");

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void returnInvalidCreateCustomer()
        {
            config.Setup(p => p.AddAccount(1, "Savings")).Returns(()=>null);

            var result = TokenObj.AddAccount(1, "Saving");

            Assert.That(result, Is.Null);
        }
    

        [Test]
        public void Called_When_Given_Valid_AccountId()
        {
            config.Setup(p => p.getAccount(1)).Returns(new Account
            {
             
            });

            var result = TokenObj.getCustomerAccount(1);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Called_When_Given_Invalid_AccountId()
        {
            config.Setup(p => p.getAccount(1)).Returns(()=>null);

            var result = TokenObj.getCustomerAccount(1);
            Assert.That(result, Is.Null);
        }
       
       


    [Test]
    public void getCutomerAccounts_Called_When_Given_Valid_CustomerId()
    {
        config.Setup(p => p.getAllAccounts(1)).Returns(new List<Account>{new Account()
        {
        } });

        var result = TokenObj.getAllAccounts(1);

            Assert.That(result, Is.Not.Null);
       }

        [Test]
        public void getCustomerAccounts_Called_When_Given_Invalid_CustomerId()
        {
            config.Setup(p => p.getAllAccounts(1)).Returns(new List<Account>{});

            var result = TokenObj.getAllAccounts(1);

            Assert.That(result.Count,Is.EqualTo(0));
        }
        


           [Test]
           public void getAllCusotmerAccounts_Called_When_ListisnotEmpty()
           {
               config.Setup(p => p.getCustomersAllAccounts()).Returns(new List<Account>{new Account()
               {} });

               var result = TokenObj.getCustomersAllAccounts();

            Assert.That(result, Is.Not.Null);
        }


   


        [Test]
           public void getstatements_Called_When_Given_Valid_AccountId()
           {

               config.Setup(p => p.getStatement(1, It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(new List<Statement>{ new Statement() {

               } });

               var result = TokenObj.getStatement(1,DateTime.Now.AddMonths(-1),DateTime.Now);

            Assert.That(result, Is.Not.Null);
        }

           [Test]
           public void getstatements_Called_When_Given_InvalidValid_AccountId()
           {
            config.Setup(p => p.getStatement(1, It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(new List<Statement>{ });

            var result = TokenObj.getStatement(1, DateTime.Now.AddMonths(-1), DateTime.Now);

            Assert.That(result.Count,Is.EqualTo(0));
        }

        

           [Test]
           public void deposit_Called_When_Given_Valid_AccountId()
           {

               config.Setup(p => p.depositAccount(1,200)).Returns( new TransactionStatus() {

               } );

               var result = TokenObj.depositAccount(1,200);

            Assert.That(result, Is.Not.Null);
        }

           [Test]
           public void deposit_Called_When_Given_InvalidValid_AccountId()
           {
            config.Setup(p => p.depositAccount(1, 200)).Returns(()=>null);
            var result = TokenObj.depositAccount(1, 200);

            Assert.That(result, Is.Null);
        }
      
        

           [Test]
           public void withdraw_Called_When_Given_Valid_AccountId()
           {

               config.Setup(p => p.withdrawAccount(1, 200)).Returns(new TransactionStatus()
               {
               });

               var result = TokenObj.withdrawAccount(1, 200);

            Assert.That(result, Is.Not.Null);
        }

           [Test]
           public void withdraw_Called_When_Given_InvalidValid_AccountId()
           {
            config.Setup(p => p.withdrawAccount(1, 200)).Returns(()=>null);
            var result = TokenObj.withdrawAccount(1, 200);

            Assert.That(result, Is.Null);
        }
        
      [Test]
      public void getAllCusotmerAccounts_Called_When_LististEmpty()
      {
       config.Setup(p => p.getCustomersAllAccounts()).Returns(new List<Account> { }) ;  
       Assert.That(() => TokenObj.getCustomersAllAccounts(),
           Throws.ArgumentNullException);
   }

  

    }
}
