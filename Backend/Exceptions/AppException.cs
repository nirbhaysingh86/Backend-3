using System;

namespace PMMC.Exceptions
{
    /// <summary>
    /// The app exception used in this application
    /// </summary>
    public class AppException : Exception
    {
        /// <summary>
        /// The status code
        /// </summary>
        public int StatusCode { get; private set; }

        /// <summary>
        /// The default constructor
        /// </summary>
        public AppException()
        {
        }

        /// <summary>
        /// The constructor with status code and error message
        /// </summary>
        /// <param name="statusCode">the status code</param>
        /// <param name="message">the error message</param>
        public AppException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// The constructor with error message and inner exception
        /// </summary>
        /// <param name="message">the error message</param>
        /// <param name="innerException">the inner exception</param>
        public AppException(string message, Exception innerException) : base(message, innerException)
        {
            StatusCode = 500;
        }
    }
}