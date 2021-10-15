using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PMMC.UnitTests.Controllers
{
    /// <summary>
    /// Tests for lookup controller
    /// </summary>
    [TestClass]
    public class LookupControllerTests : BaseTest
    {
        /// <summary>
        /// Test system values by AMAuditorType
        /// </summary>
        [TestMethod]
        public void TestSystemValuesByAMAuditorType()
        {
            SiteAdminLogin();
            var result = _lookupController.SystemValues("AMAuditorType");
            // assert
            AssertResult(result);
        }

        /// <summary>
        /// Test system values by FollowUpType
        /// </summary>
        [TestMethod]
        public void TestSystemValuesByAMFollowUpType()
        {
            AccountManagementLogin();
            var result = _lookupController.SystemValues("AMFollowUpType");
            // assert
            AssertResult(result);
        }

        /// <summary>
        /// Test system values by AMRecordAgeType
        /// </summary>
        [TestMethod]
        public void TestSystemValuesByAMRecordAgeType()
        {
            SiteAdminLogin();
            var result = _lookupController.SystemValues("AMRecordAgeType");
            // assert
            AssertResult(result);
        }

        /// <summary>
        /// Test system values by AMRecordHiddenType
        /// </summary>
        [TestMethod]
        public void TestSystemValuesByAMRecordHiddenType()
        {
            SiteAdminLogin();
            var result = _lookupController.SystemValues("AMRecordHiddenType");
            // assert
            AssertResult(result);
        }

        /// <summary>
        /// Test system values by AMStatusType
        /// </summary>
        [TestMethod]
        public void TestSystemValuesByAMStatusType()
        {
            SiteAdminLogin();
            var result = _lookupController.SystemValues("AMStatusType");
            // assert
            AssertResult(result);
        }

        /// <summary>
        /// Test system values by null code type
        /// </summary>
        [TestMethod]
        public void TestSystemValuesByNullCodeType()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<ArgumentNullException>(() => _lookupController.SystemValues(null),
                "codeType cannot be null.");
        }

        /// <summary>
        /// Test system values by empty code type
        /// </summary>
        [TestMethod]
        public void TestSystemValuesByEmptyCodeType()
        {
            SiteAdminLogin();
            ExceptionAssert.ThrowsException<ArgumentException>(() => _lookupController.SystemValues(" "),
                "codeType cannot be empty.");
        }

        /// <summary>
        /// Test system values by invalid code type
        /// </summary>
        [TestMethod]
        public void TestSystemValuesByInvalidCodeType()
        {
            SiteAdminLogin();
            var codeType = "invalid";
            ExceptionAssert.ThrowsException<ArgumentException>(() => _lookupController.SystemValues(codeType),
                $"The codeType `{codeType}` is invalid in [{string.Join(",", _appSettings.CodeTypes.Values())}]");
        }

        /// <summary>
        /// Test icdCodes
        /// </summary>
        [TestMethod]
        public void TestIcdCodes()
        {
            SiteAdminLogin();
            Assert.ThrowsException<NotImplementedException>(() => _lookupController.IcdCodes());
        }

        /// <summary>
        /// Test RevCptCodes
        /// </summary>
        [TestMethod]
        public void TestRevCptCodes()
        {
            SiteAdminLogin();
            Assert.ThrowsException<NotImplementedException>(() => _lookupController.RevCptCodes());
        }
    }
}