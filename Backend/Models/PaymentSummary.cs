using System;

namespace PMMC.Models
{
    /// <summary>
    /// The payment summary
    /// </summary>
    public class PaymentSummary
    {
        /// <summary>
        /// The id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The paid by
        /// </summary>
        public string PaidBy { get; set; }

        /// <summary>
        /// The payer id
        /// </summary>
        public int? PayerId { get; set; }

        /// <summary>
        /// The payer name
        /// </summary>
        public string PayerName { get; set; }

        /// <summary>
        /// The contract name
        /// </summary>
        public string ContractName { get; set; }

        /// <summary>
        /// The payment date
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// The estimated payment allowable
        /// </summary>
        public double? EstPayAllowable { get; set; }

        /// <summary>
        /// The actual payment
        /// </summary>
        public double? ActualPayment { get; set; }

        /// <summary>
        /// The write off
        /// </summary>
        public double? Writeoff { get; set; }

        /// <summary>
        /// The balance due
        /// </summary>
        public double? BalanceDue { get; set; }
    
    }
}
