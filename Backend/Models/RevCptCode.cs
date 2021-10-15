using System;
using System.ComponentModel.DataAnnotations;

namespace PMMC.Models
{
    /// <summary>
    /// The RevCptCode
    /// </summary>
    public class RevCptCode
    {
        /// <summary>
        /// The RevCptCode id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The revenue code
        /// </summary>
        [StringLength(100)]
        public string RevenueCode { get; set; }

        /// <summary>
        /// The revenue code description
        /// </summary>
        [StringLength(255)]
        public string RevenueCodeDescription { get; set; }

        /// <summary>
        /// The transaction id
        /// </summary>
        [StringLength(155)]
        public string TransactionId { get; set; }

        /// <summary>
        /// The cpt code
        /// </summary>
        [StringLength(9)]
        public string CptCode { get; set; }

        /// <summary>
        /// The cpt code description
        /// </summary>
        [StringLength(255)]
        public string CptCodeDescription { get; set; }

        /// <summary>
        /// The service date
        /// </summary>
        public DateTime ServiceDate { get; set; }

        /// <summary>
        /// The modifier 1
        /// </summary>
        [StringLength(10)]
        public string Modifier1 { get; set; }

        /// <summary>
        /// The modifier 2
        /// </summary>
        [StringLength(10)]
        public string Modifier2 { get; set; }

        /// <summary>
        /// The service type
        /// </summary>
        [StringLength(1)]
        public string ServiceType { get; set; }

        /// <summary>
        /// The physician id
        /// </summary>
        [StringLength(100)]
        public string PhysicianId { get; set; }

        /// <summary>
        /// The point of service
        /// </summary>
        [StringLength(10)]
        public string PointOfService { get; set; }

        /// <summary>
        /// The units
        /// </summary>
        public double Units { get; set; }

        /// <summary>
        /// The charges
        /// </summary>
        public double Charges { get; set; }

        /// <summary>
        /// The denied charges
        /// </summary>
        public double DeniedCharges { get; set; }

        /// <summary>
        /// The non covered charges
        /// </summary>
        public double NonCoveredCharges { get; set; }

        /// <summary>
        /// The non billed charges
        /// </summary>
        public double NonBilledCharges { get; set; }

        /// <summary>
        /// The cost
        /// </summary>
        public double Cost { get; set; }
    }
}
