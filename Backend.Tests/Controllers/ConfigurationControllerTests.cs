using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMMC.Exceptions;

namespace PMMC.UnitTests.Controllers
{
    /// <summary>
    /// Tests for configuration controller
    /// </summary>
    [TestClass]
    public class ConfigurationControllerTests : BaseTest
    {
        /// <summary>
        /// Test worklist columns
        /// </summary>
        [TestMethod]
        public void TestWorkListColumns()
        {
            SiteAdminLogin();
            var result = _configurationController.WorkListColumns();
            // assert
            AssertResult(result);
        }

        /// <summary>
        /// Test default view limits
        /// </summary>
        [TestMethod]
        public void TestDefaultViewLimits()
        {
            AccountManagementLogin();
            var result = _configurationController.DefaultViewLimits();
            // assert
            AssertResult(result);
        }

        /// <summary>
        /// Test default view limits without rows
        /// </summary>
        [TestMethod]
        public void TestDefaultViewLimitsWithNoRows()
        {
            SiteAdminLogin();
            TestHelper.Execute("DELETE FROM [dbo].[tblCustom_Configuration]");
            ExceptionAssert.ThrowsException<InternalServerErrorException>(
                () => _configurationController.DefaultViewLimits(), "Exist no rows for defaultViewLimits");
        }

        /// <summary>
        /// Test default view limits with more than one row
        /// </summary>
        [TestMethod]
        public void TestDefaultViewLimitsWithMoreRows()
        {
            SiteAdminLogin();
            TestHelper.Execute("INSERT INTO tblCustom_Configuration VALUES (0, 0, 1, 0, 1); ");
            ExceptionAssert.ThrowsException<InternalServerErrorException>(
                () => _configurationController.DefaultViewLimits(), "Exist more than 1 row for defaultViewLimits");
        }
    }
}