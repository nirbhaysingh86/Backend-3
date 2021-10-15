using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMMC.Entities;
using PMMC.Helpers;
using PMMC.Exceptions;
using System;
using Microsoft.Data.SqlClient;

namespace PMMC.UnitTests.Controllers
{
    /// <summary>
    /// Tests for auth controller
    /// </summary>
    [TestClass]
    public class AuthControllerTests : BaseTest
    {
        /// <summary>
        /// Test login with site admin role
        /// </summary>
        [TestMethod]
        public void TestLoginBySiteAdminRole()
        {
            var authRequest = new AuthRequest
            {
                Username = _testSettings.SiteAdmin.Username,
                Password = _testSettings.SiteAdmin.UserPassword,
                Server = _testSettings.ServerName,
                DbName = _testSettings.DbName
            };
            var result = _authController.Login(authRequest);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Token);
            var parsedResult = JwtHelper.ParseJwtToken(result.Token, _appSettings.Jwt);
            Assert.AreEqual(_testSettings.SiteAdmin.UserID, parsedResult.UserId);
            Assert.AreEqual(_testSettings.SiteAdmin.Username, parsedResult.Username);
            Assert.AreEqual(_testSettings.SiteAdmin.Role, parsedResult.Role);
            Assert.AreEqual(authRequest.Server, parsedResult.DbServer);
            Assert.AreEqual(authRequest.DbName, parsedResult.DbName);
        }

        /// <summary>
        /// Test login with account management role
        /// </summary>
        [TestMethod]
        public void TestLoginByAccountManagementRole()
        {
            // test azure environment vairable 
            var builder = new SqlConnectionStringBuilder(_appSettings.ConnectionString);
            Environment.SetEnvironmentVariable("AZURE_DATABASE_USERID", builder.UserID);
            Environment.SetEnvironmentVariable("AZURE_DATABASE_PASSWORD", builder.Password);
            var authRequest = new AuthRequest
            {
                Username = _testSettings.AccountManagement.Username,
                Password = _testSettings.AccountManagement.UserPassword,
                Server = _testSettings.ServerName,
                DbName = _testSettings.DbName
            };
            var result = _authController.Login(authRequest);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Token);
            var parsedResult = JwtHelper.ParseJwtToken(result.Token, _appSettings.Jwt);
            Assert.AreEqual(_testSettings.AccountManagement.UserID, parsedResult.UserId);
            Assert.AreEqual(_testSettings.AccountManagement.Username, parsedResult.Username);
            Assert.AreEqual(_testSettings.AccountManagement.Role, parsedResult.Role);
            Assert.AreEqual(authRequest.Server, parsedResult.DbServer);
            Assert.AreEqual(authRequest.DbName, parsedResult.DbName);
            Environment.SetEnvironmentVariable("AZURE_DATABASE_USERID", null);
            Environment.SetEnvironmentVariable("AZURE_DATABASE_PASSWORD", null);
        }

        /// <summary>
        /// Test login with invalid server
        /// </summary>
        [TestMethod]
        public void TestLoginByInvalidServer()
        {
            var authRequest = new AuthRequest
            {
                Username = _testSettings.SiteAdmin.Username,
                Password = _testSettings.SiteAdmin.UserPassword,
                Server = "InvalidServer",
                DbName = _testSettings.DbName
            };
            ExceptionAssert.ThrowsException<BadRequestException>(() => _authController.Login(authRequest),
                $"Error to connect to database(server='{authRequest.Server}',db='{authRequest.DbName}')");
        }

        /// <summary>
        /// Test login with invalid database
        /// </summary>
        [TestMethod]
        public void TestLoginByInvalidDatabase()
        {
            var authRequest = new AuthRequest
            {
                Username = _testSettings.SiteAdmin.Username,
                Password = _testSettings.SiteAdmin.UserPassword,
                Server = _testSettings.ServerName,
                DbName = "InvalidDBError"
            };
            ExceptionAssert.ThrowsException<BadRequestException>(() => _authController.Login(authRequest),
                $"Error to connect to database(server='{authRequest.Server}',db='{authRequest.DbName}')");
        }

        /// <summary>
        /// Test login with invalid password
        /// </summary>
        [TestMethod]
        public void TestLoginByInvalidPassword()
        {
            var authRequest = new AuthRequest
            {
                Username = _testSettings.SiteAdmin.Username,
                Password = "InvalidPassword",
                Server = _testSettings.ServerName,
                DbName = _testSettings.DbName
            };
            ExceptionAssert.ThrowsException<UnauthorizedException>(() => _authController.Login(authRequest),
                $"User with name `{authRequest.Username}` not found with given password");
        }

        /// <summary>
        /// Test login with normal user role
        /// </summary>
        [TestMethod]
        public void TestLoginByNormalUserRole()
        {
            var authRequest = new AuthRequest
            {
                Username = _testSettings.NormalUser.Username,
                Password = _testSettings.NormalUser.UserPassword,
                Server = _testSettings.ServerName,
                DbName = _testSettings.DbName
            };
            ExceptionAssert.ThrowsException<ForbiddenException>(() => _authController.Login(authRequest),
                $"User with role `{_testSettings.NormalUser.Role}` don't have permission to access");
        }

        /// <summary>
        /// Test login with null user role
        /// </summary>
        [TestMethod]
        public void TestLoginByNullRoleUser()
        {
            var authRequest = new AuthRequest
            {
                Username = _testSettings.NullRoleUser.Username,
                Password = _testSettings.NullRoleUser.UserPassword,
                Server = _testSettings.ServerName,
                DbName = _testSettings.DbName
            };
            ExceptionAssert.ThrowsException<ForbiddenException>(() => _authController.Login(authRequest),
                $"User with role `{_testSettings.NullRoleUser.Role}` don't have permission to access");
        }

        /// <summary>
        /// Test login with null auth request(not happen in real controller and will throw model validation errors)
        /// </summary>
        [TestMethod]
        public void TestLoginByNullAuthRequest()
        {
            ExceptionAssert.ThrowsException<ArgumentNullException>(() => _authController.Login(null),
                "authRequest cannot be null.");
        }
    }
}