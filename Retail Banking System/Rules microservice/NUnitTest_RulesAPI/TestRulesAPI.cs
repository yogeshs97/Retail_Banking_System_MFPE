using Moq;
using NUnit.Framework;
using RulesAPI.Controllers;
using RulesAPI.Models;
using RulesAPI.Providers.Interfaces;
using RulesAPI.Repositories;
using RulesAPI.Repositories.Interfaces;
using System.Collections.Generic;

namespace NUnitTest_RulesAPI
{
    public class TestRulesAPI
    {
        RulesRepository _rulesRepo;
        Mock<IRulesRepository> _rulesRepoMock;
        RulesController _rulesController;
        Mock<IBalanceProvider> _balanceProviderMock;
        Mock<IMonthlyJobProvider> _monthlyProviderMock;
        [SetUp]
        public void Setup()
        {
            _rulesRepo = new RulesRepository();
            _rulesRepoMock = new Mock<IRulesRepository>();
            _balanceProviderMock = new Mock<IBalanceProvider>();
            _monthlyProviderMock = new Mock<IMonthlyJobProvider>();
            _rulesController = new RulesController(_balanceProviderMock.Object,_monthlyProviderMock.Object);
            _rulesRepoMock.Setup(x => x.GetMinBalance(It.IsAny<int>())).Returns(1000);
            _balanceProviderMock.Setup(x => x.GetMinBalance(It.IsAny<int>())).Returns(1000);
        }

        [Test]
        public void EvaluateMinBal_ValidInput_int()
        {
            var result = _rulesController.EvaluateMinBal(1,1400);
            RuleStatus status = new RuleStatus { Status = "allowed"};
            Assert.AreEqual(result.Status,status.Status);
        }

        [Test]
        public void EvaluateMinBal_InValidInput_int()
        {
            var result = _rulesController.EvaluateMinBal(2, 500);
            RuleStatus status = new RuleStatus { Status = "denied" };
            Assert.AreEqual(result.Status, status.Status);
        }

        [Test]
        public void GetAccounts_Exception()
        {
            _rulesRepoMock.Setup(x => x.GetAccounts()).Returns(new List<Account> { });
            Assert.That(() => _rulesRepo.GetAccounts(), Throws.Exception);
        }

        
    }
}