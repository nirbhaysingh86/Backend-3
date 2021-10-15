using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PMMC.Exceptions;
using PMMC.Helpers;
using System;
using System.IO;
using System.Reflection;

namespace PMMC.UnitTests.Helpers
{
    /// <summary>
    /// Tests for logger helper
    /// </summary>
    [TestClass]
    public class LoggerHelperTests
    {
        /// <summary>
        /// Test get object description with invalid object
        /// </summary>
        [TestMethod]
        public void TestGetObjectDescriptionWithInvalidObject()
        {
            LoggerHelper.GetObjectDescription(new MemoryStream());
        }

        /// <summary>
        /// Test log method entry with wrong method arguments
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestLogMethodEntry()
        {
            LoggerHelper.LogMethodEntry(GetLogger(), MethodBase.GetCurrentMethod(), new object[] { 1, true });
        }

        /// <summary>
        /// Test process with exception
        /// </summary>
        [TestMethod]
        public void TestProcess()
        {
            var methodDescription = "methodDescription";
            ExceptionAssert.ThrowsException<AppException>(() => LoggerHelper.Process(() =>
            {
                throw new Exception("test");
                // throw to ensure log helper will handle unexpected exception properly
            }, GetLogger(), methodDescription, "methodName"), $"Error occurred while {methodDescription}.");
        }

        /// <summary>
        /// Get mock logger
        /// </summary>
        /// <returns>the mock logger</returns>
        private ILogger GetLogger()
        {
            return new Mock<ILogger<LoggerHelperTests>>().Object;
        }
    }
}
