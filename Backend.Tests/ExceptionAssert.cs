using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PMMC.UnitTests
{
    /// <summary>
    /// The exception assert to assert exception type and error message
    /// </summary>
    public static class ExceptionAssert
    {
        /// <summary>
        /// Check expected exception with error message throws
        /// </summary>
        /// <param name="action">the action</param>
        /// <param name="message">the error message</param>
        /// <typeparam name="TException">throws if unexpected error</typeparam>
        public static void ThrowsException<TException>(Action action, string message)
        where TException : Exception
        {
            try
            {
                action();

                Assert.Fail("Exception of type {0} expected; got none exception", typeof(TException).Name);
            }
            catch (TException ex)
            {
                StringAssert.StartsWith(ex.Message, message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Exception of type {0} expected; got exception of type {1}", typeof(TException).Name, ex.GetType().Name);
            }
        }
    }
}
