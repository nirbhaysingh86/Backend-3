using PMMC.Entities;
using PMMC.Models;
using System.Collections.Generic;

namespace PMMC.Interfaces
{
    /// <summary>
    /// The lookup service interface
    /// </summary>
    public interface ILookupService
    {
        /// <summary>
        /// Get all system values by code type
        /// </summary>
        /// <param name="codeType">the code type</param>
        /// <param name="user">the jwt user</param>
        /// <returns>all system values by code type</returns>
        IEnumerable<SystemValue> SystemValues(string codeType, JwtUser user);
    }
}