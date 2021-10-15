namespace PMMC.Exceptions
{
    
    /// <summary>
    /// The unauthorized exception
    /// </summary>
    public class UnauthorizedException : AppException
    {
        /// <summary>
        /// Constructor with message and status code is 401
        /// </summary>
        /// <param name="message">the error message</param>
        public UnauthorizedException(string message): base(401, message)
        {
        }
    }
}
