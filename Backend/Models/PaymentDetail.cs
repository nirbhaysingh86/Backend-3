using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace PMMC.Models
{
    /// <summary>
    /// The payment detail
    /// </summary>
    public class PaymentDetail
    {
        /// <summary>
        /// The id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The id
        /// </summary>
        [JsonIgnore]
        public int PatientId { get; set; }

        /// <summary>
        /// The is other payment
        /// </summary>
        [JsonIgnore]
        public byte IsOtherPayment { get; set; }

        /// <summary>
        /// The paid by
        /// </summary>
        [Required]
        [StringLength(20)]
        public string PaidBy { get; set; }

        /// <summary>
        /// The payer payment type
        /// </summary>
        [Required]
        [StringLength(20)]
        public string PaymentType { get; set; }

        /// <summary>
        /// The import date
        /// </summary>
        public DateTime? ImportDate { get; set; }

        /// <summary>
        /// The posting date
        /// </summary>
        public DateTime? PostingDate { get; set; }

        /// <summary>
        /// The amount
        /// </summary>
        public double? Amount { get; set; }

        /// <summary>
        /// The excluded amount
        /// </summary>
        public double? ExcludedAmount { get; set; }

        /// <summary>
        /// The adjust code
        /// </summary>
        [StringLength(50)]
        public string AdjustCode { get; set; }

        /// <summary>
        /// The adjust code description
        /// </summary>
        [StringLength(255)]
        public string AdjustCodeDescription { get; set; }

        public int PayerNumber { get; set; }
    }
}
