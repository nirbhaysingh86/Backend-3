using PMMC.Entities;
using PMMC.Models;
using System.Collections.Generic;

namespace PMMC.Interfaces
{
    /// <summary>
    /// The configuration service interface
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Get all worklist columns
        /// </summary>
        /// <param name="user">the jwt user</param>
        /// <returns>all worklist columns</returns>
        IEnumerable<WorklistColumn> WorkListColumns(JwtUser user);

        /// <summary>
        /// Get default view limits
        /// </summary>
        /// <param name="user">the jwt user</param>
        /// <returns>default view limits</returns>
        DefaultViewLimits DefaultViewLimits(JwtUser user);
    }
}