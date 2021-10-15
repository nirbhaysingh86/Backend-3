using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using PMMC.Configs;
using PMMC.Controllers;
using PMMC.Entities;
using PMMC.Helpers;
using PMMC.Services;

namespace PMMC.UnitTests
{
    /// <summary>
    /// Base test for all tests
    /// </summary>
    public abstract class BaseTest
    {
        /// <summary>
        /// The test results path
        /// </summary>
        protected string TestResultsPath = Path.Combine("..", "..", "..", "TestJsonResults");

        /// <summary>
        /// The app settings
        /// </summary>
        protected AppSettings _appSettings;

        /// <summary>
        /// Test settings
        /// </summary>
        protected TestSettings _testSettings;

        /// <summary>
        /// The mock http context accessor
        /// </summary>
        protected Mock<IHttpContextAccessor> _mockHttpContextAccessor;


        /// <summary>
        /// The auth controller
        /// </summary>
        protected AuthController _authController;


        /// <summary>
        /// The lookup controller
        /// </summary>
        protected LookupController _lookupController;


        /// <summary>
        /// The configuration controller
        /// </summary>
        protected ConfigurationController _configurationController;


        /// <summary>
        /// The view controller
        /// </summary>
        protected ViewsController _viewsController;

        /// <summary>
        /// The worklist controller
        /// </summary>
        protected WorklistController _worklistController;

        /// <summary>
        /// The default constructor
        /// </summary>
        protected BaseTest()
        {
            _testSettings = TestHelper.GetTestSettings();
            _appSettings = TestHelper.GetAppSettings();
            var appSettingsOptions = Options.Create(_appSettings);
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContextAccessor = _mockHttpContextAccessor.Object;
            var userService = new UserService(GetLog<UserService>(), appSettingsOptions);
            _authController = new AuthController(GetLog<AuthController>(), userService);
            var lookupService = new LookupService(GetLog<LookupService>(), appSettingsOptions);
            _lookupController = new LookupController(GetLog<LookupController>(), lookupService, httpContextAccessor);

            var configurationService = new ConfigurationService(GetLog<ConfigurationService>(), appSettingsOptions);
            _configurationController = new ConfigurationController(GetLog<ConfigurationController>(),
                configurationService, httpContextAccessor);

            var viewService = new ViewService(GetLog<ViewService>(), appSettingsOptions);
            _viewsController = new ViewsController(GetLog<ViewsController>(), viewService, httpContextAccessor);

            var worklistLayoutService = new WorklistLayoutService(GetLog<WorklistLayoutService>(), appSettingsOptions);
            var worklistAccountService = new WorklistAccountService(GetLog<WorklistAccountService>(), appSettingsOptions, viewService, worklistLayoutService);
            _worklistController = new WorklistController(GetLog<WorklistController>(), worklistLayoutService, worklistAccountService, httpContextAccessor);
        }

        /// <summary>
        /// Sets up the environment before executing each test in this class.
        /// </summary>
        [TestInitialize]
        public virtual void SetUp()
        {
            TestHelper.PrepareDatabase();
        }


        /// <summary>
        /// Get mock logger
        /// </summary>
        /// <typeparam name="T">the type to create logger</typeparam>
        /// <returns>mock logger</returns>
        protected ILogger<T> GetLog<T>()
        {
            return new Mock<ILogger<T>>().Object;
        }

        /// <summary>
        /// Mock site admin login
        /// </summary>
        protected void SiteAdminLogin()
        {
            var context = new DefaultHttpContext();
            var siteAdmin = new JwtUser
            {
                UserId = _testSettings.SiteAdmin.UserID,
                Username = _testSettings.SiteAdmin.Username,
                Role = _testSettings.SiteAdmin.Role,
                DbServer = _testSettings.ServerName,
                DbName = _testSettings.DbName,
            };
            context.Items[Helper.UserPropertyName] = siteAdmin;
            _mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
        }

        /// <summary>
        /// Mock account management login
        /// </summary>
        protected void AccountManagementLogin()
        {
            var context = new DefaultHttpContext();
            var accountManagement = new JwtUser
            {
                UserId = _testSettings.AccountManagement.UserID,
                Username = _testSettings.AccountManagement.Username,
                Role = _testSettings.AccountManagement.Role,
                DbServer = _testSettings.ServerName,
                DbName = _testSettings.DbName,
            };
            context.Items[Helper.UserPropertyName] = accountManagement;
            _mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
        }


        /// <summary>
        /// Compares actual test result with the expected result in JSON format.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="result">The result.</param>
        /// <param name="testName">Name of the test.</param>
        protected void AssertResult<T>(T result, [CallerMemberName] string testName = null)
        {
            bool develop = true;
            if (develop)
            {
                if (!Directory.Exists(TestResultsPath))
                {
                    Directory.CreateDirectory(TestResultsPath);
                }

                string jsonResult = JsonConvert.SerializeObject(result, TestHelper.SerializerSettings);

                string filePath = Path.Combine(TestResultsPath, $"{GetType().Name}.{testName}.json");
                if (!File.Exists(filePath))
                {
                    File.WriteAllText(filePath, jsonResult);
                }
                else
                {
                    string existing = File.ReadAllText(filePath);
                    if (jsonResult != existing)
                    {
                        File.WriteAllText(filePath, jsonResult);
                        Assert.Fail("mismatch");
                    }
                }
            }
            else
            {
                string filePath = Path.Combine(TestResultsPath, $"{GetType().Name}.{testName}.json");
                string expected = File.ReadAllText(filePath);
                string actual = JsonConvert.SerializeObject(result, TestHelper.SerializerSettings);
                Assert.AreEqual(expected, actual, "Mismatch in actual and expected result when serialized to JSON.");
            }
        }
    }
}