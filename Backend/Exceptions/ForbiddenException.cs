namespace PMMC.Exceptions
{
    /// <summary>
    /// The forbidden exception
    /// </summary>
    public class ForbiddenException : AppException
    {
        /// <summary>
        /// Constructor with message and status code is 403
        /// </summary>
        /// <param name="message">the error message</param>
        public ForbiddenException(string message) : base(403, message)
        {
        }
    }
}
