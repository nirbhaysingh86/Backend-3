using System;
using System.Collections.Generic;

namespace PMMC.Models
{
    /// <summary>
    /// The claim history item
    /// </summary>
    public class ClaimHistoryItem
    {
        /// <summary>
        /// The claim history item id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The patient control number
        /// </summary>
        public string PatientControlNumber { get; set; }

        /// <summary>
        /// The primary payer
        /// </summary>
        public string PrimaryPayer { get; set; }

        /// <summary>
        /// The bill type
        /// </summary>
        public string BillType { get; set; }

        /// <summary>
        /// The claim frequency type
        /// </summary>
        public string ClaimFrequencyType { get; set; }

        /// <summary>
        /// The billing date
        /// </summary>
        public DateTime BillingDate { get; set; }

        /// <summary>
        /// The claim number
        /// </summary>
        public string ClaimNumber { get; set; }

        /// <summary>
        /// The statement from date
        /// </summary>
        public DateTime StatementFromDate { get; set; }

        /// <summary>
        /// The statement to date
        /// </summary>
        public DateTime StatementToDate { get; set; }

        /// <summary>
        /// The total charges
        /// </summary>
        public double TotalCharges { get; set; }

        /// <summary>
        /// The total non covered charges
        /// </summary>
        public double TotalNonCoveredCharges { get; set; }

        /// <summary>
        /// The patient est amt due
        /// </summary>
        public string PatientEstAmtDue { get; set; }

        /// <summary>
        /// The line count
        /// </summary>
        public int LineCount { get; set; }

        /// <summary>
        /// The destination payer
        /// </summary>
        public string DestinationPayer { get; set; }

        /// <summary>
        /// The destination payer responsibility
        /// </summary>
        public string DestinationPayerResponsibility { get; set; }

        /// <summary>
        /// The claim details
        /// </summary>
        public IList<ClaimDetailItem> ClaimDetails { get; set; }
    }
}
