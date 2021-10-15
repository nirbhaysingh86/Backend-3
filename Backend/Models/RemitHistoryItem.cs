using System;
using System.Collections.Generic;

namespace PMMC.Models
{
    /// <summary>
    /// The remit history item
    /// </summary>
    public class RemitHistoryItem
    {
        /// <summary>
        /// The remit history item id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The provider
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// The payer
        /// </summary>
        public string Payer { get; set; }

        /// <summary>
        /// The bill type
        /// </summary>
        public string BillType { get; set; }

        /// <summary>
        /// The claim frequency type 
        /// </summary>
        public string ClaimFrequencyType { get; set; }

        /// <summary>
        /// The effective date
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// The check number
        /// </summary>
        public string CheckNumber { get; set; }

        /// <summary>
        /// The statement from date
        /// </summary>
        public DateTime StatementFromDate { get; set; }

        /// <summary>
        /// The statement to date
        /// </summary>
        public DateTime StatementToDate { get; set; }

        /// <summary>
        /// The claim status
        /// </summary>
        public string ClaimStatus { get; set; }

        /// <summary>
        /// The claim billed amount
        /// </summary>
        public double ClaimBilledAmount { get; set; }

        /// <summary>
        /// The claim paid amount
        /// </summary>
        public double ClaimPaidAmount { get; set; }

        /// <summary>
        /// The claim level adjust amount
        /// </summary>
        public double ClaimLevelAdjustAmount { get; set; }

        /// <summary>
        /// The claim level denial amount
        /// </summary>
        public double ClaimLevelDenialAmount { get; set; }

        /// <summary>
        /// The line level adjust amount
        /// </summary>
        public double LineLevelAdjustAmount { get; set; }

        /// <summary>
        /// The line level denial amount
        /// </summary>
        public double LineLevelDenialAmount { get; set; }

        /// <summary>
        /// The line count
        /// </summary>
        public int LineCount { get; set; }

        /// <summary>
        /// The claim level adjust reason codes
        /// </summary>
        public string ClaimLevelAdjustReasonCodes { get; set; }

        /// <summary>
        /// The claim level remark codes
        /// </summary>
        public string ClaimLevelRemarkCodes { get; set; }

        /// <summary>
        /// The claim number
        /// </summary>
        public string ClaimNumber { get; set; }

        /// <summary>
        /// The remit details
        /// </summary>
        public IList<RemitDetailItem> RemitDetails { get; set; }
    }
}
