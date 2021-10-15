namespace PMMC.Exceptions
{
    /// <summary>
    /// The not found exception
    /// </summary>
    public class NotFoundException : AppException
    {
        /// <summary>
        /// Constructor with message and status code is 404
        /// </summary>
        /// <param name="message">the error message</param>
        public NotFoundException(string message) : base(404, message)
        {
        }
    }
}
