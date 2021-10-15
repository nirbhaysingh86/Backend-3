using System.Collections.Generic;

namespace PMMC.Models
{
    /// <summary>
    /// The claim and remit history
    /// </summary>
    public class ClaimAndRemitHistory
    {
        /// <summary>
        /// The claims
        /// </summary>
        public IList<ClaimHistoryItem> Claims { get; set; }

        /// <summary>
        /// The remits
        /// </summary>
        public IList<RemitHistoryItem> Remits { get; set; }
    }
}
