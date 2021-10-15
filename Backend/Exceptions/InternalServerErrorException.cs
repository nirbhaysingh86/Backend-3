namespace PMMC.Exceptions
{
    /// <summary>
    /// The internal server error exception
    /// </summary>
    public class InternalServerErrorException : AppException
    {
        /// <summary>
        /// Constructor with message and status code is 500
        /// </summary>
        /// <param name="message">the error message</param>
        public InternalServerErrorException(string message) : base(500, message)
        {
        }
    }
}
