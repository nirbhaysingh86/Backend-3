using System;

namespace PMMC.Models
{
    /// <summary>
    /// The ICD code
    /// </summary>
    public class ICDCode
    {
        /// <summary>
        /// The id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The icd code
        /// </summary>
        public string IcdCode { get; set; }

        /// <summary>
        /// The ics version
        /// </summary>
        public int? IcdVersion { get; set; }

        /// <summary>
        /// The item
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The service date
        /// </summary>
        public DateTime? ServiceDate { get; set; }

        /// <summary>
        /// The rank
        /// </summary>
        public int? Rank { get; set; }

        /// <summary>
        /// The transaction id
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// The poa
        /// </summary>
        public string Poa { get; set; }
    }
}
