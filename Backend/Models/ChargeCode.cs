using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace PMMC.Models
{
    /// <summary>
    /// The Charge Code Detail
    /// </summary>
    public class ChargeCodeDetail
    {
        /// <summary>
        /// The charge code id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The charge code
        /// </summary>
        [Required]
        [StringLength(30)]
        public string ChargeCode { get; set; }

        /// <summary>
        /// The revenue code
        /// </summary>
        [StringLength(10)]
        public string RevenueCode { get; set; }

        /// <summary>
        /// The denied charges
        /// </summary>
        public double DeniedCharges { get; set; }

        /// <summary>
        /// The description
        /// </summary>
        [StringLength(100)]
        public string Description { get; set; }

        /// <summary>
        /// The cpt code
        /// </summary>
        [StringLength(10)]
        public string CptCode { get; set; }

        /// <summary>
        /// The non covered charges
        /// </summary>
        public double NonCoveredCharges { get; set; }

        /// <summary>
        /// The transaction id
        /// </summary>
        [StringLength(50)]
        public string TransactionId { get; set; }

        /// <summary>
        /// The units
        /// </summary>
        public int Units { get; set; }

        /// <summary>
        /// The non billed charges
        /// </summary>
        public double NonBilledCharges { get; set; }

        /// <summary>
        /// The service date
        /// </summary>
        public DateTime? ServiceDate { get; set; }

        /// <summary>
        /// The charges
        /// </summary>
        public double Charges { get; set; }

        /// <summary>
        /// The cost
        /// </summary>
        public double Cost { get; set; }
    }
}
