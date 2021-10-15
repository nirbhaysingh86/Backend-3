namespace PMMC.Models
{

    /// <summary>
    /// The remit detail item
    /// </summary>
    public class RemitDetailItem
    {
        /// <summary>
        /// The remit detail item id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The adjustment reason codes
        /// </summary>
        public string AdjustmentReasonCodes { get; set; }

        /// <summary>
        /// The remark codes
        /// </summary>
        public string RemarkCodes { get; set; }
    }
}
