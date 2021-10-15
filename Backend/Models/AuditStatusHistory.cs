using System;

namespace PMMC.Models
{
    /// <summary>
    /// The Detail Reimb
    /// </summary>
    public class AuditStatusHistory
    {
        /// <summary>
        /// The id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The audit status
        /// </summary>
        public string AuditStatus { get; set; }

        /// <summary>
        /// The date set
        /// </summary>
        public DateTime DateSet { get; set; }

        /// <summary>
        /// The changed by
        /// </summary>
        public string ChangedBy { get; set; }

        /// <summary>
        /// The current auditor
        /// </summary>
        public string CurrentAuditor { get; set; }

        /// <summary>
        /// The event date
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// The days elapsed
        /// </summary>
        public double DaysElapsed { get; set; }
    }
}
