using PMMC.Entities;
using PMMC.Models;
using System.Collections.Generic;

namespace PMMC.Interfaces
{
    /// <summary>
    /// The worklist layout service interface
    /// </summary>
    public interface IWorklistLayoutService
    {
        /// <summary>
        /// Get all worklist layouts for current user
        /// </summary>
        /// <param name="user">the jwt user</param>
        /// <returns>all worklist layouts for current user</returns>
        IEnumerable<WorklistLayout> GetAllWorklistLayouts(JwtUser user);

        /// <summary>
        /// Get worklist layout by id
        /// </summary>
        /// <param name="layoutId">the worklist layout id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match worklist layout by id</returns>
        WorklistLayout GetWorklistLayoutById(int layoutId, JwtUser user);

        /// <summary>
        /// Create worklist layout
        /// </summary>
        /// <param name="worklistLayout">the worklist layout to create</param>
        /// <param name="user">the jwt user</param>
        /// <returns>the new created worklist layout with id</returns>
        WorklistLayout CreateWorklistLayout(WorklistLayout worklistLayout, JwtUser user);

        /// <summary>
        /// Update worklist layout
        /// </summary>
        /// <param name="layoutId">the worklist layout id</param>
        /// <param name="worklistLayout">the worklist layout to update</param>
        /// <param name="user">the jwt user</param>
        WorklistLayout UpdateWorklistLayout(int layoutId, WorklistLayout worklistLayout, JwtUser user);

        /// <summary>
        /// Delete worklist layout by id
        /// </summary>
        /// <param name="layoutId">the worklist layout id</param>
        /// <param name="user">the jwt user</param>
        void DeleteWorklistLayout(int layoutId, JwtUser user);
    }
}
