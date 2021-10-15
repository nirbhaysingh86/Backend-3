using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace PMMC.Models
{
    /// <summary>
    /// The other payment
    /// </summary>
    public class OtherPayment
    {
        /// <summary>
        /// The paid by
        /// </summary>
        [JsonIgnore]
        public string PaidBy { get; set; }

        /// <summary>
        /// The account number
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// The adjust code
        /// </summary>
        public string AdjustCode { get; set; }

        /// <summary>
        /// The adjust code description
        /// </summary>
        public string AdjustCodeDescription { get; set; }

        /// <summary>
        /// The adjustment type
        /// </summary>
        public string AdjustmentType { get; set; }

        /// <summary>
        /// The posting code
        /// </summary>
        public string PostingCode { get; set; }

        /// <summary>
        /// The payer id
        /// </summary>
        public string PayerId { get; set; }

        /// <summary>
        /// The plan id
        /// </summary>
        public int? PlanId { get; set; }

        /// <summary>
        /// The amount
        /// </summary>
        public double? Amount { get; set; }

        /// <summary>
        /// The import date
        /// </summary>
        public DateTime? ImportDate { get; set; }
        /// <summary>
        /// PayerNumber
        /// </summary>
        public int? PayerNumber { get; set; }
    }

    /// <summary>
    /// The other payment update
    /// </summary>
    public class OtherPaymentUpdate
    {
        /// <summary>
        /// The patient id
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// The entity
        /// </summary>
        [Required]
        public string Entity { get; set; }

        /// <summary>
        /// The increment name
        /// </summary>
        [Required]
        public string IncrementName { get; set; }

        /// <summary>
        /// The account number
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// The adjust code
        /// </summary>
        public string AdjustCode { get; set; }

        /// <summary>
        /// The payer id
        /// </summary>
        public string PayerId { get; set; }
    }
}
