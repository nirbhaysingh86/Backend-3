using System;

namespace PMMC.Models
{
    /// <summary>
    /// The Detail Reimb
    /// </summary>
    public class DetailReimb
    {
        /// <summary>
        /// The id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The code type
        /// </summary>
        public string CodeType { get; set; }

        /// <summary>
        /// The service type
        /// </summary>
        public string ServiceType { get; set; }

        /// <summary>
        /// The item
        /// </summary>
        public string Item { get; set; }

        /// <summary>
        /// The service pay or group
        /// </summary>
        public string ServicePayOrGroup { get; set; }

        /// <summary>
        /// The service date
        /// </summary>
        public DateTime? ServiceDate { get; set; }

        /// <summary>
        /// The adjusted charges
        /// </summary>
        public double? AdjustedCharges { get; set; }

        /// <summary>
        /// The units
        /// </summary>
        public double? Units { get; set; }

        /// <summary>
        /// The reimb method
        /// </summary>
        public string ReimbMethod { get; set; }

        /// <summary>
        /// The ppc
        /// </summary>
        public int? Ppc { get; set; }

        /// <summary>
        /// The rate
        /// </summary>
        public int? Rate { get; set; }

        /// <summary>
        /// The expected reimburse
        /// </summary>
        public double? ExpectedReimburse { get; set; }

        /// <summary>
        /// The terms diff
        /// </summary>
        public double? TermsDiff { get; set; }
    }
}
