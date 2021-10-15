namespace PMMC.Exceptions
{
    /// <summary>
    /// The bad request exception with status code =400
    /// </summary>
    public class BadRequestException : AppException
    {
        /// <summary>
        /// Constructor with message and status code is 400
        /// </summary>
        /// <param name="message">the error message</param>
        public BadRequestException(string message) : base(400, message)
        {
        }
    }
}
