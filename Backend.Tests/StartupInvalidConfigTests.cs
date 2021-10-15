using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMMC.Interfaces;
using System.Configuration;
namespace PMMC.UnitTests
{
    /// <summary>
    /// Test start up with invalid configuration
    /// </summary>
    [TestClass]
    public class StartupInvalidConfig : BaseTest
    {
        /// <summary>
        /// Test invalid app settings
        /// </summary>
        [TestMethod]
        public void TestInvalidAppSettings()
        {
            var emptyWebHost = TestHelper.GetWebHostBuilder("Empty").Build();
            Assert.IsNotNull(emptyWebHost);
            ExceptionAssert.ThrowsException<ConfigurationErrorsException>(() => emptyWebHost.Services.GetRequiredService<IUserService>(), "Found 25 configuration error(s) in AppSettings");
        }
    }
}
