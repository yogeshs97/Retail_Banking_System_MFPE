using NUnit.Framework;
using Authentication.Models;
using Authentication.Repository;
using Authentication.Provider;
using Microsoft.Extensions.Configuration;
using Moq;
using Authentication.Controllers;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationNUnitTest
{
	public class Tests
	{

		private AuthController _controller;
		private Mock<IAuthProvider> _ProviderMock;
		private JwtSecurityToken token=new JwtSecurityToken();
		private JwtSecurityToken nullToken = null;

		private Mock<IConfiguration> _config;
		private Mock<IAuthRepository> _repository;
		private IAuthProvider _provider;
		private Mock<ITokenProvider> _tokenProvider;
		[SetUp]
		public void Setup()
		{
			_ProviderMock = new Mock<IAuthProvider>();
			_ProviderMock.Setup(s => s.UserLoginProvider(It.IsAny<Login>())).Returns(token);
			_ProviderMock.Setup(s => s.UserLoginProvider(It.IsAny<Login>())).Returns(nullToken);
			_controller = new AuthController(_ProviderMock.Object);

			_config = new Mock<IConfiguration>();
			_config.Setup(c => c["Jwt:Key"]).Returns("ThisIsUserKey");

			_repository = new Mock<IAuthRepository>();
			_repository.Setup(s => s.UserLoginRepo(It.IsAny<Login>())).Returns(true);
			_repository.Setup(s => s.UserLoginRepo(It.IsAny<Login>())).Returns(false);
			_tokenProvider = new Mock<ITokenProvider>();
			_tokenProvider.Setup(s => s.GenerateJWTToken(It.IsAny<Login>())).Returns(token);
			_provider = new AuthProvider(_repository.Object, _tokenProvider.Object);
		}

		[Test]
		public void UserLoginCont_ValidInput_ReturnsJWTToken()
		{
			Login login = new Login()
			{
				Username = "Sakil",
				Password = "Sakil@55",
				IsEmployee = true
			};
			var response=_controller.UserLogin(login) as ObjectResult;
			Assert.AreEqual(200, response.StatusCode);
		}
		[Test]
		public void UserLoginCont_InvalidInput_ReturnsJWTToken()
		{
			Login login = new Login()
			{
				Username = "SakilJee",
				Password = "Sakil@55",
				IsEmployee = true
			};
			var response = _controller.UserLogin(login) as StatusCodeResult;
			Assert.AreEqual(500, response.StatusCode);
		}

		[Test]
		public void UserLoginProvider_WithValidData_ShouldReturnJWTToken()
		{
			//Arrange
			Login login = new Login()
			{
				Username = "Sakil",
				Password = "Sakil@55",
				IsEmployee = true
			};

			var actualResult=_provider.UserLoginProvider(login);
			//Assert
			Assert.IsNotNull(actualResult);
		}
		[Test]
		public void UserLoginProvider_WithInvalidUserName_ShouldReturnNull()
		{
			//Arrange
			Login login = new Login()
			{
				Username = "SakilJee",
				Password = "Sakil@55",
				IsEmployee = true
			};

			var actualResult = _provider.UserLoginProvider(login);
			//Assert
			Assert.IsNull(actualResult);
		}
		[Test]
		public void UserLoginProvider_WithInvalidPassword_ShouldReturnNull()
		{
			//Arrange
			Login login = new Login()
			{
				Username = "Sakil",
				Password = "Sakil@20",
				IsEmployee = true
			};

			var actualResult = _provider.UserLoginProvider(login);

			//Assert
			Assert.IsNull(actualResult);
		}
		[Test]
		public void UserLoginProvider_WithInvalidUserType_ShouldReturnNull()
		{
			//Arrange
			Login login = new Login()
			{
				Username = "Sakil",
				Password = "Sakil@55",
				IsEmployee = false
			};

			var actualResult = _provider.UserLoginProvider(login);
			//Assert
			Assert.IsNull(actualResult);
		}
	}
}