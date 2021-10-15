using System;

namespace PMMC.Models
{
    /// <summary>
    /// The claim detail item
    /// </summary>
    public class ClaimDetailItem
    {
        /// <summary>
        /// The claim detail item id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The line number
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// The date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The revenue code
        /// </summary>
        public string RevenueCode { get; set; }

        /// <summary>
        /// The hcpcs cpt code
        /// </summary>
        public string HcpcsCptCode { get; set; }

        /// <summary>
        /// The modifier
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// The quantity
        /// </summary>
        public double Quantity { get; set; }

        /// <summary>
        /// The charges
        /// </summary>
        public double Charges { get; set; }

        /// <summary>
        /// The non covered charges
        /// </summary>
        public double NonCoveredCharges { get; set; }
    }
}
