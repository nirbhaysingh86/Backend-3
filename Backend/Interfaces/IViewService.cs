using PMMC.Entities;
using PMMC.Models;
using System.Collections.Generic;

namespace PMMC.Interfaces
{
    /// <summary>
    /// The view service interface
    /// </summary>
    public interface IViewService
    {
        /// <summary>
        /// Get all view fields
        /// </summary>
        /// <param name="user">the jwt user</param>
        /// <returns>all view fields</returns>
        IEnumerable<ViewField> Fields(JwtUser user);

        /// <summary>
        /// Get all field values for rules that use 'values' as the SelectionType
        /// </summary>
        /// <param name="fieldId">the field id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>all field values</returns>
        IEnumerable<FieldValue> FieldValues(int fieldId, JwtUser user);

        /// <summary>
        /// Get all views for current user
        /// </summary>
        /// <param name="user">the jwt user</param>
        /// <returns>all views for current user</returns>
        IEnumerable<View> GetAllViews(JwtUser user);

        /// <summary>
        /// Get view by id
        /// </summary>
        /// <param name="id">the view id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match view by id</returns>
        View GetViewById(int id, JwtUser user);

        /// <summary>
        /// Create view
        /// </summary>
        /// <param name="view">the view to create</param>
        /// <param name="user">the jwt user</param>
        /// <returns>the new created view with id</returns>
        View CreateView(View view, JwtUser user);

        /// <summary>
        /// Update view
        /// </summary>
        /// <param name="id">the view id</param>
        /// <param name="view">the view to update</param>
        /// <param name="user">the jwt user</param>
        /// <returns>updated view</returns>
        View UpdateView(int id, View view, JwtUser user);

        /// <summary>
        /// Delete view by id
        /// </summary>
        /// <param name="viewId">the view id</param>
        /// <param name="user">the jwt user</param>
        void DeleteView(int viewId, JwtUser user);

        /// <summary>
        /// Get all view rules
        /// </summary>
        /// <param name="viewId">the view id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>all view rules</returns>
        IEnumerable<ViewRule> GetAllViewRules(int viewId, JwtUser user);

        /// <summary>
        /// Create view rules
        /// </summary>
        /// <param name="viewId">the view id</param>
        /// <param name="viewRules">the view rules to create</param>
        /// <param name="user">the jwt user</param>
        /// <returns>the new created view rules with id</returns>
        IEnumerable<ViewRule> CreateViewRule(int viewId, IEnumerable<ViewRule> viewRules, JwtUser user);

        /// <summary>
        /// Get view rule with view id and rule id
        /// </summary>
        /// <param name="viewId">the view id</param>
        /// <param name="ruleId">the rule id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match view rule with view id and rule id</returns>
        ViewRule GetViewRule(int viewId, int ruleId, JwtUser user);

        /// <summary>
        /// Update view rule
        /// </summary>
        /// <param name="viewId">the view id</param>
        /// <param name="ruleId">the rule id</param>
        /// <param name="viewRule">the view rule to update</param>
        /// <param name="user">the jwt user</param>
        /// <returns>updated view rule</returns>
        ViewRule UpdateViewRule(int viewId, int ruleId, ViewRule viewRule, JwtUser user);

        /// <summary>
        /// Delete view rule
        /// </summary>
        /// <param name="viewId">the view id</param>
        /// <param name="ruleId">the rule id</param>
        /// <param name="user">the jwt user</param>
        void DeleteViewRule(int viewId, int ruleId, JwtUser user);
    }
}