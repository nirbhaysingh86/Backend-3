using System;
namespace PMMC.Models
{

    /// <summary>
    /// The audit to log all database changes
    /// </summary>
    public class Audit
    {
        /// <summary>
        /// The view id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The old value
        /// </summary>
        public string OldValue { get; set; }

        /// <summary>
        /// The JSON representation of new object
        /// </summary>
        public string NewValue { get; set; }

        /// <summary>
        /// The JSON representation of existed object
        /// </summary>
        public OperationType OperationType { get; set; }

        /// <summary>
        /// The object type
        /// </summary>
        public string ObjectType { get; set; }

        /// <summary>
        /// The timestamp when operation was performed
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The user id who performed the operation
        /// </summary>
        public int UserId { get; set; }
    }
}
