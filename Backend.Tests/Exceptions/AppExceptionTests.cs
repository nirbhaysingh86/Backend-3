using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMMC.Exceptions;
using System;


namespace PMMC.UnitTests.Exceptions
{
    /// <summary>
    /// Tests for App exception
    /// </summary>
    [TestClass]
    public class AppExceptionTests
    {
        /// <summary>
        /// Test app exception
        /// </summary>
        [TestMethod]
        public void TestAppException()
        {
            var error = new AppException();
            Assert.IsNotNull(error);
            Assert.AreEqual(0, error.StatusCode);
            var exception = new Exception();
            var message = "Error message";
            var innerError = new AppException(message, exception);
            Assert.IsNotNull(innerError);
            Assert.AreEqual(500, innerError.StatusCode);
            Assert.AreEqual(message, innerError.Message);
        }
    }
}
