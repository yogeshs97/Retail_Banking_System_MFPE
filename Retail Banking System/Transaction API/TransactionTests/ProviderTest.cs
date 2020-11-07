using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Transactions_Microservice.Models;
using Transactions_Microservice.Provider;
using Transactions_Microservice.Repository;

namespace TransactionTests
{
    [TestFixture]
    public class ProviderTest
    {
        private Mock<IRepository> _repo;
        private TransactionProvider _transactionprovider;

        [SetUp]
        public void Setup()
        {
            _repo = new Mock<IRepository>();
            _transactionprovider = new TransactionProvider(_repo.Object);
           
        }

        [Test]
        public void AddToTransactionHistory_WhenCalled_Returnstrue()
        {
            _repo.Setup(repo => repo.AddToTransactionHistory(It.IsAny<TransactionStatus>(), It.IsAny<Account>())).Returns(true);
           
            var result = _transactionprovider.AddToTransactionHistory(new TransactionStatus(),new Account());

            Assert.That(result, Is.EqualTo(true));
        }
        
        [Test]
        public void AddToTransactionHistory_WhenCalled_ReturnsFalse()
        {
            _repo.Setup(repo => repo.AddToTransactionHistory(It.IsAny<TransactionStatus>(), It.IsAny<Account>())).Returns(false);


            Assert.That(()=> _transactionprovider.AddToTransactionHistory(new TransactionStatus(), new Account()),
                Throws.ArgumentNullException);
        }
       
        
        [Test]
        public void GetTransactionHistory_WhenCalled_ReturnsList()
        {
            _repo.Setup(repo => repo.GetTransactionHistory(It.IsAny<int>())).Returns(new List<TransactionHistory> { new TransactionHistory()});

            var result = _transactionprovider.GetTransactionHistory(1);

            Assert.That(result, Is.Not.Null);
        }
    }
}
