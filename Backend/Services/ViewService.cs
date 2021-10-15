using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PMMC.Configs;
using PMMC.Entities;
using PMMC.Exceptions;
using PMMC.Helpers;
using PMMC.Interfaces;
using PMMC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace PMMC.Services
{
    /// <summary>
    /// The view service
    /// </summary>
    public class ViewService : BaseService, IViewService
    {
        /// <summary>
        /// The fields sql
        /// </summary>
        internal const string FieldsSql =
            "SELECT [FieldID] AS Id,[FieldName] AS Name,[Description],[Category],[SelectionType] FROM [dbo].[tblViewFields] WHERE [ExistsClause] IS NULL ORDER BY [FieldName]";

        /// <summary>
        /// The field values sql
        /// </summary>
        internal const string FieldValuesSql =
            "SELECT [FieldName] AS Name,[SelectionType] FROM [dbo].[tblViewFields] WHERE [ExistsClause] IS NULL AND [FieldID]=@fieldId";

        /// <summary>
        /// The worklist data sql
        /// </summary>
        internal const string WorkListDataSql =
            "SELECT DISTINCT [{0}]  FROM [dbo].[tblWorklistData] WHERE [{0}] IS NOT NULL";

        /// <summary>
        /// The get all views sql
        /// </summary>
        internal const string GetAllViewsSql =
            "SELECT [ViewID] AS Id,[ViewName] AS Name,[ViewDesc] AS Description,[DefaultView] AS IsDefault, [DefaultAuditor] AS Auditor,[DefaultFollowUp] AS FollowUp,[DefaultStatus] AS Status,[DefaultRecordAge] AS AccountAge, [DefaultRecordHidden] AS HiddenRecords  FROM [dbo].[tblUserViews] WHERE [ViewOwner]=@UserId";

        /// <summary>
        /// The get view by id sql
        /// </summary>
        internal const string GetViewByIdSql =
            "SELECT [ViewID] AS Id, [ViewOwner], [ViewName] AS Name,[ViewDesc] AS Description,[DefaultView] AS IsDefault, [DefaultAuditor] AS Auditor,[DefaultFollowUp] AS FollowUp,[DefaultStatus] AS Status,[DefaultRecordAge] AS AccountAge, [DefaultRecordHidden] AS HiddenRecords  FROM [dbo].[tblUserViews] WHERE [ViewID]=@id";

        /// <summary>
        /// The get view owner by id sql
        /// </summary>
        internal const string GetViewOwnerByIdSql =
            "SELECT [ViewOwner] FROM [dbo].[tblUserViews] WHERE [ViewID]=@viewId";

        /// <summary>
        /// The get view by name sql
        /// </summary>
        internal const string GetViewByNameSql =
            "SELECT [ViewID] AS Id  FROM [dbo].[tblUserViews] WHERE [ViewOwner]=@UserId AND [ViewName]=@Name";

        /// <summary>
        /// The get code sql
        /// </summary>
        internal const string GetCodeSql =
            "SELECT [Code] FROM [dbo].[tblComboBoxesSystemValues] WHERE [CodeType]='{0}'";

        /// <summary>
        /// Get system values by code type sql
        /// </summary>
        internal const string GetSystemValuesSql =
            "SELECT [Code],[Description] FROM [dbo].[tblComboBoxesSystemValues] WHERE [CodeType]='{0}' ORDER BY [RankOrder]";

        /// <summary>
        /// Get field value sql
        /// </summary>
        internal const string GetFieldValuesSql =
            "SELECT [CodeCounter] AS Id,[Code],[Description] FROM [dbo].[tblComboBoxesSystemValues] WHERE [CodeType]=@codeType AND [Code] IN @codes ORDER BY [RankOrder]";

        /// <summary>
        /// Get field value sql
        /// </summary>
        internal const string GetFieldValueSql =
            "SELECT [CodeCounter] AS Id,[Code],[Description] FROM [dbo].[tblComboBoxesSystemValues] WHERE [CodeType]=@codeType AND [CodeCounter]=@id";

        /// <summary>
        /// Get field value id sql
        /// </summary>
        internal const string GetFieldValueIdSql =
            "SELECT [CodeCounter] AS Id FROM [dbo].[tblComboBoxesSystemValues] WHERE [CodeType]=@codeType AND [Code]=@code";


        /// <summary>
        /// The update default view sql
        /// </summary>
        internal const string UpdateDefaultViewSql =
            "UPDATE  [dbo].[tblUserViews] SET [DefaultView]=0 WHERE [DefaultView]!=0 AND [ViewOwner]=@UserId";

        /// <summary>
        /// The create view sql
        /// </summary>
        internal const string CreateViewSql =
            "INSERT INTO [dbo].[tblUserViews]([ViewName],[ViewDesc],[ViewOwner],[CPTView],[DefaultView],[DefaultAuditor],[DefaultFollowUp],[DefaultStatus],[DefaultRecordAge],[DefaultRecordHidden]) OUTPUT INSERTED.[ViewID] VALUES(@Name,@Description,@UserId,0,@DefaultView,@Auditor,@FollowUp,@Status,@AccountAge,@HiddenRecords)";

        /// <summary>
        /// The update view sql
        /// </summary>
        internal const string UpdateViewSql =
            "UPDATE [dbo].[tblUserViews]  SET [ViewName]=@Name,[ViewDesc]=@Description,[DefaultView]=@DefaultView,[DefaultAuditor]=@Auditor,[DefaultFollowUp]=@FollowUp,[DefaultStatus]=@Status,[DefaultRecordAge]=@AccountAge,[DefaultRecordHidden]=@HiddenRecords WHERE [ViewID]=@id";

        /// <summary>
        /// The delete view rules sql
        /// </summary>
        internal const string DeleteViewRulesSql = "DELETE [dbo].[tblViewRules] WHERE [ViewID]=@viewId";

        /// <summary>
        /// The delete multi select view rules sql
        /// </summary>
        internal const string DeleteMultiSelectViewRulesSql = "DELETE [dbo].[tblViewRules] WHERE [ViewID]=@viewId AND [FieldID]=@fieldId";

        /// <summary>
        /// The delete view sql
        /// </summary>
        internal const string DeleteViewSql =
            "DELETE [dbo].[tblUserViews] WHERE [ViewID]=@viewId AND ViewOwner=@UserId";

        /// <summary>
        /// The get view rules sql
        /// </summary>
        internal const string GetAllViewRulesSql =
            "SELECT [RuleID] AS Id, r.[FieldID] AS FieldId,[ValueID] ,[Value],[BeginRange],[EndRange],[Operand],f.[FieldName] AS Name,f.[SelectionType],f.[ComboCodeType],f.[SystemComboCodeType],f.[DescField],f.[HiddenLinkField],f.[IntegerFieldLink] FROM [dbo].[tblViewRules] r INNER JOIN [dbo].[tblViewFields] f ON r.[FieldID]=f.[FieldID] WHERE f.[ExistsClause] IS NULL AND [ViewID]=@viewId";

        /// <summary>
        /// The get view rules sql
        /// </summary>
        internal static string GetViewRuleSql = $"{GetAllViewRulesSql} AND [RuleID]=@ruleId";

        /// <summary>
        /// The get view rules by field Id sql
        /// </summary>
        internal static string GetViewRulesByFieldIdSql = $"{GetAllViewRulesSql} AND r.[FieldID]=@fieldId";

        /// <summary>
        /// The view rule fields sql
        /// </summary>
        internal const string ViewRuleFieldSql =
            "SELECT [SelectionType],[ComboCodeType],[FieldName] AS Name,[SystemComboCodeType],[DescField],[LinkCaption],[HiddenLinkField],[IntegerFieldLink] FROM  [dbo].[tblViewFields] WHERE [ExistsClause] IS NULL AND [FieldID]=@FieldId";

        /// <summary>
        /// The create view rule sql
        /// </summary>
        internal const string CreateViewRuleSql =
            "INSERT INTO [dbo].[tblViewRules]([ViewID],[FieldID],[FacilityID],[ValueID],[Value],[BeginRange],[EndRange],[Operand])  OUTPUT INSERTED.[RuleID] VALUES(@viewId,@FieldId,1,@ValueId,@Value,@BeginRange,@EndRange,@Operand)";

        /// <summary>
        /// The count view rule sql
        /// </summary>
        internal const string CountViewRuleSql =
            "SELECT COUNT(*) FROM [dbo].[tblViewRules] WHERE [ViewID]=@viewId AND [RuleID]=@ruleId";

        /// <summary>
        /// The update view sql
        /// </summary>
        internal const string UpdateViewRuleSql =
            "UPDATE [dbo].[tblViewRules]  SET [FieldID]=@FieldId,[ValueID]=@ValueId,[Value]=@Value,[BeginRange]=@BeginRange,[EndRange]=@EndRange,[Operand]=@Operand WHERE [ViewID]=@viewId AND [RuleID]=@ruleId";

        /// <summary>
        /// The delete view rule sql
        /// </summary>
        internal const string DeleteViewRuleSql =
            "DELETE [dbo].[tblViewRules] WHERE [ViewID]=@viewId AND [RuleID]=@ruleId";

        /// <summary>
        /// The Code desc field.
        /// </summary>
        internal const string CodeDescField = "Code";

        /// <summary>
        /// The Description desc field.
        /// </summary>
        internal const string DescriptionDescField = "Description";

        /// <summary>
        /// The Code/Description desc field.
        /// </summary>
        internal const string CodeDescriptionDescField = "[Code] + ' - ' + [Description]";


        /// <summary>
        /// Constructor with logger and app settings
        /// </summary>
        /// <param name="logger">the logger</param>
        /// <param name="appSettings">the app settings</param>
        public ViewService(ILogger<ViewService> logger, IOptions<AppSettings> appSettings) : base(logger, appSettings)
        {
        }

        /// <summary>
        /// Get all view fields
        /// </summary>
        /// <param name="user">the jwt user</param>
        /// <returns>all view fields</returns>
        public IEnumerable<ViewField> Fields(JwtUser user)
        {
            return _logger.Process(() =>
                {
                    Helper.ValidateArgumentNotNull(user, nameof(user));
                    return ProcessWithDb((conn) => conn.Query<ViewField>(FieldsSql), user);
                }, "get list of all available view fields",
                parameters: user);
        }


        /// <summary>
        /// Get all field values
        /// </summary>
        /// <param name="fieldId">the field id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>all field values</returns>
        public IEnumerable<FieldValue> FieldValues(int fieldId, JwtUser user)
        {
            return _logger.Process(() =>
                {
                    Helper.ValidateArgumentPositive(fieldId, nameof(fieldId));
                    Helper.ValidateArgumentNotNull(user, nameof(user));
                    return ProcessWithDb((conn) =>
                    {
                        var field = ValidateViewField(conn, fieldId, true);
                        var valuesSql = string.Format(WorkListDataSql, field.Name);
                        var codes = conn.Query<string>(valuesSql);
                        var codeType = GetViewFieldComboCodeType(field);
                        var fieldValues = conn.Query<FieldValue>(GetFieldValuesSql, new { codes, codeType });
                        foreach (var fieldValue in fieldValues)
                        {
                            fieldValue.DisplayValue = GetFieldDisplayValue(field, fieldValue);
                        }
                        return fieldValues;
                    }, user);
                }, "get list of values for the field with SelectionType='values'",
                parameters: new object[] { fieldId, user });
        }


        /// <summary>
        /// Get all views for current user
        /// </summary>
        /// <param name="user">the jwt user</param>
        /// <returns>all views for current user</returns>
        public IEnumerable<View> GetAllViews(JwtUser user)
        {
            return _logger.Process(() =>
                {
                    Helper.ValidateArgumentNotNull(user, nameof(user));
                    return ProcessWithDb((conn) =>
                    {
                        return conn.Query<View, DefaultViewLimits, View>(GetAllViewsSql, (views, limits) =>
                        {
                            views.Limits = limits;
                            return views;
                        }, new { user.UserId }, splitOn: "Auditor");
                    }, user);
                }, "get all current user's Views'",
                parameters: user);
        }


        /// <summary>
        /// Get view by id
        /// </summary>
        /// <param name="id">the view id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match view by id</returns>
        public View GetViewById(int id, JwtUser user)
        {
            return _logger.Process(() => FindValidView(id, user), "get View with the given Id",
                parameters: new object[] { id, user });
        }


        /// <summary>
        /// Create view
        /// </summary>
        /// <param name="view">the view to create</param>
        /// <param name="user">the jwt user</param>
        /// <returns>the new created view with id</returns>
        public View CreateView(View view, JwtUser user)
        {
            return _logger.Process(() =>
                {
                    ValidateView(view, user);
                    return ProcessWithDbTransaction((conn) =>
                    {
                        UpdateDefaultView(conn, view, user);
                        var newId = conn.QuerySingle<int>(CreateViewSql,
                            new
                            {
                                view.Name,
                                view.Description,
                                user.UserId,
                                DefaultView = view.IsDefault ? 1 : 0,
                                view.Limits.Auditor,
                                view.Limits.FollowUp,
                                view.Limits.Status,
                                view.Limits.AccountAge,
                                view.Limits.HiddenRecords
                            });
                        view.Id = newId;
                        view.ViewOwner = user.UserId;
                        Audit(conn, new Audit
                        {
                            UserId = user.UserId,
                            OldValue = null,
                            NewValue = LoggerHelper.GetObjectDescription(view),
                            OperationType = OperationType.Create,
                            ObjectType = nameof(View),
                            Timestamp = DateTime.Now
                        });
                        return view;
                    }, user);
                }, "creates new View",
                parameters: new object[] { view, user });
        }

        /// <summary>
        /// Update view
        /// </summary>
        /// <param name="id">the view id</param>
        /// <param name="view">the view to update</param>
        /// <param name="user">the jwt user</param>
        /// <returns>the updated view</returns>
        public View UpdateView(int id, View view, JwtUser user)
        {
            return _logger.Process(() =>
                {
                    var oldView = FindValidView(id, user);
                    ValidateView(view, user, id);
                    return ProcessWithDbTransaction((conn) =>
                    {
                        UpdateDefaultView(conn, view, user);
                        conn.Execute(UpdateViewSql,
                            new
                            {
                                id,
                                view.Name,
                                view.Description,
                                DefaultView = view.IsDefault ? 1 : 0,
                                view.Limits.Auditor,
                                view.Limits.FollowUp,
                                view.Limits.Status,
                                view.Limits.AccountAge,
                                view.Limits.HiddenRecords
                            });
                        view.Id = id;
                        view.ViewOwner = user.UserId;
                        Audit(conn, new Audit
                        {
                            UserId = user.UserId,
                            OldValue = LoggerHelper.GetObjectDescription(oldView),
                            NewValue = LoggerHelper.GetObjectDescription(view),
                            OperationType = OperationType.Update,
                            ObjectType = nameof(View),
                            Timestamp = DateTime.Now
                        });
                        return view;
                    }, user);
                }, "updates View with the given Id",
                parameters: new object[] { id, view, user });
        }

        /// <summary>
        /// Delete view by id
        /// </summary>
        /// <param name="viewId">the view id</param>
        /// <param name="user">the jwt user</param>
        public void DeleteView(int viewId, JwtUser user)
        {
            _logger.Process(() =>
                {
                    var oldView = FindValidView(viewId, user);
                    var oldViewRules = FindAllViewRules(viewId, user);
                    ProcessWithDbTransaction((conn) =>
                    {
                        conn.Execute(DeleteViewRulesSql, new { viewId });
                        foreach (var oldViewRule in oldViewRules)
                        {
                            AuditDeleteViewRule(conn, viewId, oldViewRule, user);
                        }
                        conn.Execute(DeleteViewSql, new { viewId, user.UserId });
                        Audit(conn, new Audit
                        {
                            UserId = user.UserId,
                            OldValue = LoggerHelper.GetObjectDescription(oldView),
                            NewValue = null,
                            OperationType = OperationType.Delete,
                            ObjectType = nameof(View),
                            Timestamp = DateTime.Now
                        });
                    }, user);
                }, "deletes View with the given Id",
                parameters: new object[] { viewId, user });
        }

        /// <summary>
        /// Get all view rules
        /// </summary>
        /// <param name="viewId">the view id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>all view rules</returns>
        public IEnumerable<ViewRule> GetAllViewRules(int viewId, JwtUser user)
        {
            return _logger.Process(() =>
                {
                    EnsureViewOwner(viewId, user);
                    return FindAllViewRules(viewId, user);
                }, "gets View Rules",
                parameters: new object[] { viewId, user });
        }

        /// <summary>
        /// Create view rules
        /// </summary>
        /// <param name="viewId">the view id</param>
        /// <param name="viewRules">the view rules to create</param>
        /// <param name="user">the jwt user</param>
        /// <returns>the new created view rules with id</returns>
        public IEnumerable<ViewRule> CreateViewRule(int viewId, IEnumerable<ViewRule> viewRules, JwtUser user)
        {
            return _logger.Process(() =>
                {
                    EnsureViewOwner(viewId, user);
                    Helper.ValidateArgumentNotNull(viewRules, nameof(viewRules));
                    // var oldViewRules = FindAllViewRules(viewId, user);
                    var valueIds = new List<string>();
                    ProcessWithDb((conn) =>
                    {
                        foreach (var viewRule in viewRules)
                        {
                            valueIds.Add(viewRule.ValueId);
                            ValidateViewRule(conn, viewRule);
                            viewRule.fieldName = viewRule.viewField.Name;
                        }
                    }, user);
                    return ProcessWithDbTransaction((conn) =>
                    {
                        // // delete and audit exist rules
                        // conn.Execute(DeleteViewRulesSql, new { viewId });
                        // foreach (var oldViewRule in oldViewRules)
                        // {
                        //     AuditDeleteViewRule(conn, viewId, oldViewRule, user);
                        // }
                        var enumerator = viewRules.GetEnumerator();
                        var ruleIndex = 0;
                        while (enumerator.MoveNext())
                        {
                            var viewRule = viewRules.ElementAt(ruleIndex);
                            CreateViewRule(conn, viewId, viewRule, user);
                            // reset valueId
                            viewRule.ValueId = valueIds[ruleIndex];
                            ruleIndex++;
                        }
                        return viewRules;
                    }, user);
                }, "creates new View Rules",
                parameters: new object[] { viewId, viewRules, user });
        }

        /// <summary>
        /// Get view rule with view id and rule id
        /// </summary>
        /// <param name="viewId">the view id</param>
        /// <param name="ruleId">the rule id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match view rule with view id and rule id</returns>
        public ViewRule GetViewRule(int viewId, int ruleId, JwtUser user)
        {
            return _logger.Process(() => FindValidViewRule(viewId, ruleId, user), "gets View Rule with given Id",
                parameters: new object[] { viewId, ruleId, user });
        }

        /// <summary>
        /// Update view rule
        /// </summary>
        /// <param name="viewId">the view id</param>
        /// <param name="ruleId">the rule id</param>
        /// <param name="user">the jwt user</param>
        /// <param name="viewRule">the view rule to update</param>
        /// <returns>the updated view rule</returns>
        public ViewRule UpdateViewRule(int viewId, int ruleId, ViewRule viewRule, JwtUser user)
        {
           return _logger.Process(() =>
                {
                    var oldViewRule = FindValidViewRule(viewId, ruleId, user);
                    Helper.ValidateArgumentNotNull(viewRule, nameof(viewRule));
                    return ProcessWithDbTransaction((conn) =>
                    {
                        var valueId = viewRule.ValueId;
                        ValidateViewRule(conn, viewRule);
                        conn.Execute(UpdateViewRuleSql, new
                        {
                            viewId,
                            ruleId,
                            viewRule.FieldId,
                            viewRule.ValueId,
                            viewRule.Value,
                            viewRule.BeginRange,
                            viewRule.EndRange,
                            viewRule.Operand
                        });
                        viewRule.Id = ruleId;
                        viewRule.ValueId = valueId; //reset value id
                        viewRule.fieldName = viewRule.viewField.Name;
                        Audit(conn, new Audit
                        {
                            UserId = user.UserId,
                            OldValue = LoggerHelper.GetObjectDescription(oldViewRule),
                            NewValue = LoggerHelper.GetObjectDescription(viewRule),
                            OperationType = OperationType.Update,
                            ObjectType = nameof(ViewRule),
                            Timestamp = DateTime.Now
                        });
                        return viewRule;
                    }, user);
                }, "updates View Rule with given Id",
                parameters: new object[] { viewId, ruleId, viewRule, user });
        }

        /// <summary>
        /// Delete view rule
        /// </summary>
        /// <param name="viewId">the view id</param>
        /// <param name="ruleId">the rule id</param>
        /// <param name="user">the jwt user</param>
        public void DeleteViewRule(int viewId, int ruleId, JwtUser user)
        {
            _logger.Process(() =>
                {
                    var oldViewRule = FindValidViewRule(viewId, ruleId, user);
                    ProcessWithDbTransaction((conn) =>
                    {
                        DeleteViewRule(conn, viewId, oldViewRule, user);
                    }, user);
                }, "deletes View Rule with given Id",
                parameters: new object[] { viewId, ruleId, user });
        }

        /// <summary>
        /// Get field display value
        /// </summary>
        /// <param name="field">the field</param>
        /// <param name="fieldValue">the field value</param>
        /// <returns>the field display value</returns>
        private string GetFieldDisplayValue(ViewField field, FieldValue fieldValue)
        {
            if (DescriptionDescField.Equals(field.DescField))
            {
                return fieldValue.Description;
            }
            else if (CodeDescField.Equals(field.DescField))
            {
                return fieldValue.Code;
            }
            else if (CodeDescriptionDescField.Equals(field.DescField))
            {
                return $"{fieldValue.Code}-{fieldValue.Description}";
            }
            else
            {
                throw new InternalServerErrorException($"ViewFields by id='{field.Id}' exist not supported DescField '{field.DescField}'");
            }
        }


        /// <summary>
        /// Find view rules
        /// </summary>
        /// <param name="conn">the database connection</param>
        /// <param name="sql">the sql</param>
        /// <param name="param">the sql param</param>
        /// <returns>match view rules</returns>
        private IEnumerable<ViewRule> FindViewRules(IDbConnection conn, string sql, object param = null)
        {
            return conn.Query<ViewRule, ViewField, ViewRule>(sql, (viewRule, viewField) =>
            {
                viewField.Id = viewRule.FieldId;
                viewRule.viewField = viewField;
                viewRule.fieldName = viewField.Name;
                return viewRule;
            }, param, splitOn: "Name");
        }


        /// <summary>
        /// Find all view rules
        /// </summary>
        /// <param name="viewId">the view id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>all view rules by view id</returns>
        private IEnumerable<ViewRule> FindAllViewRules(int viewId, JwtUser user)
        {
            return ProcessWithDb((conn) =>
            {
                var rules = FindViewRules(conn, GetAllViewRulesSql, new { viewId });
                foreach (var rule in rules)
                {
                    ProcessValuesSelectionTypeViewRule(conn, rule);
                }
                return rules;
            }, user);
        }

        /// <summary>
        /// Get view by id
        /// </summary>
        /// <param name="id">the view id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match view by id</returns>
        private View FindValidView(int id, JwtUser user)
        {
            Helper.ValidateArgumentPositive(id, nameof(id));
            Helper.ValidateArgumentNotNull(user, nameof(user));
            return ProcessWithDb((conn) =>
            {
                var records = conn.Query<View, DefaultViewLimits, View>(GetViewByIdSql, (view, limits) =>
                {
                    view.Limits = limits;
                    return view;
                }, new { id }, splitOn: "Auditor");
                if (!records.Any())
                {
                    throw new NotFoundException($"Views by id='{id}' not found");
                }

                var view = records.First();
                if (view.ViewOwner != user.UserId)
                {
                    throw new ForbiddenException($"Views by id='{id}' not belongs to the current user");
                }
                return view;
            }, user);
        }

        /// <summary>
        /// Check whether selection type is valid
        /// </summary>
        /// <param name="expectedSelectionType">the expected selection type</param>
        /// <param name="selectionType">the selection type to check</param>
        /// <returns>true if selection type match otherwise false</returns>
        private bool IsSelectionType(string expectedSelectionType, string selectionType)
        {
            return string.Equals(expectedSelectionType, selectionType, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Check whether is values selection type
        /// </summary>
        /// <param name="selectionType">the selection type to check</param>
        /// <returns>true if values selection type otherwise false</returns>
        private bool IsValuesSelectionType(string selectionType)
        {
            // real db value is Values and is case insensitive in database
            return IsSelectionType(_appSettings.ValuesSelectionType, selectionType);
        }

        /// <summary>
        /// Check whether is values selection type and valid field name for view rule
        /// </summary>
        /// <param name="viewField">the view field to check</param>
        /// <returns>true if values selection type and valid field name otherwise false</returns>
        private bool IsValuesSelectionTypeField(ViewField viewField)
        {
            // real db value is Values and is case insensitive in database
            var existComboCodeType = !string.IsNullOrWhiteSpace(viewField.ComboCodeType) || !string.IsNullOrWhiteSpace(viewField.SystemComboCodeType);
            return IsValuesSelectionType(viewField.SelectionType) && existComboCodeType && _appSettings.ValuesFieldNames.Contains(viewField.Name);
        }

        /// <summary>
        /// Update default view
        /// </summary>
        /// <param name="conn">the connection</param>
        /// <param name="views">the view to update</param>
        /// <param name="user">the jwt user</param>
        private void UpdateDefaultView(IDbConnection conn, View views, JwtUser user)
        {
            if (views.IsDefault)
            {
                conn.Execute(UpdateDefaultViewSql, new { user.UserId });
            }
        }

        /// <summary>
        /// Ensure view exist and owner is current user
        /// </summary>
        /// <param name="viewId">the view id</param>
        /// <param name="user">the jwt user</param>
        /// <exception cref="NotFoundException">throws if view not found</exception>
        /// <exception cref="ForbiddenException">throws if view owner is not current user</exception>
        private void EnsureViewOwner(int viewId, JwtUser user)
        {
            Helper.ValidateArgumentPositive(viewId, nameof(viewId));
            Helper.ValidateArgumentNotNull(user, nameof(user));
            ProcessWithDb((conn) =>
            {
                var records = conn.Query<int>(GetViewOwnerByIdSql, new { viewId });
                if (!records.Any())
                {
                    throw new NotFoundException($"Views by id='{viewId}' not found");
                }

                if (records.First() != user.UserId)
                {
                    throw new ForbiddenException($"Views by id='{viewId}' not belongs to the current user");
                }
            }, user);
        }

        /// <summary>
        /// Validate view to ensure valid inputs
        /// </summary>
        /// <param name="view">the view to validate</param>
        /// <param name="user">the jwt user</param>
        /// <param name="id">the updated view id</param>
        /// <exception cref="BadRequestException">throws if view is invalid</exception>
        private void ValidateView(View view, JwtUser user, int? id = null)
        {
            Helper.ValidateArgumentNotNull(view, nameof(view));
            Helper.ValidateArgumentNotNull(user, nameof(user));
            ProcessWithDb((conn) =>
            {
                var ids = conn.Query<int>(GetViewByNameSql, new { user.UserId, view.Name });
                if (ids.Any() && (!id.HasValue || !ids.First().Equals(id.Value)))
                {
                    throw new BadRequestException($"Views with name='{view.Name}' exists in database");
                }

                var queryAuditor = string.Format(GetCodeSql, _appSettings.CodeTypes.Auditor);
                var queryFollowUp = string.Format(GetCodeSql, _appSettings.CodeTypes.FollowUp);
                var queryStatus = string.Format(GetCodeSql, _appSettings.CodeTypes.Status);
                var queryAccountAge = string.Format(GetCodeSql, _appSettings.CodeTypes.AccountAge);
                var queryHiddenRecords = string.Format(GetCodeSql, _appSettings.CodeTypes.HiddenRecords);
                var multiSQL = $"{queryAuditor};{queryFollowUp};{queryStatus};{queryAccountAge};{queryHiddenRecords}";
                using (var multi = conn.QueryMultiple(multiSQL))
                {
                    var auditors = multi.Read<int>();
                    var followUps = multi.Read<int>();
                    var statuses = multi.Read<int>();
                    var accountAges = multi.Read<int>();
                    var hiddenRecords = multi.Read<int>();
                    var errors = new List<string>();
                    if (!auditors.Contains(view.Limits.Auditor))
                    {
                        errors.Add(
                            $"Auditor with value='{view.Limits.Auditor}' is invalid in [{string.Join(",", auditors)}]");
                    }

                    if (!followUps.Contains(view.Limits.FollowUp))
                    {
                        errors.Add(
                            $"FollowUp with value='{view.Limits.FollowUp}' is invalid in [{string.Join(",", followUps)}]");
                    }

                    if (!statuses.Contains(view.Limits.Status))
                    {
                        errors.Add(
                            $"Status with value='{view.Limits.Status}' is invalid in [{string.Join(",", statuses)}]");
                    }

                    if (!accountAges.Contains(view.Limits.AccountAge))
                    {
                        errors.Add(
                            $"AccountAge with value='{view.Limits.AccountAge}' is invalid in [{string.Join(",", accountAges)}]");
                    }

                    if (!hiddenRecords.Contains(view.Limits.HiddenRecords))
                    {
                        errors.Add(
                            $"HiddenRecords with value='{view.Limits.HiddenRecords}' is invalid in [{string.Join(",", hiddenRecords)}]");
                    }

                    if (errors.Any())
                    {
                        throw new BadRequestException(string.Join(".", errors));
                    }
                }
            }, user);
        }

        /// <summary>
        /// Process values selection type for view rule
        /// </summary>
        /// <param name="conn">the connection</param>
        /// <param name="rule">the view rule</param>
        private void ProcessValuesSelectionTypeViewRule(IDbConnection conn, ViewRule rule)
        {
            var field = rule.viewField;
            if (IsValuesSelectionTypeField(field) && !string.IsNullOrWhiteSpace(rule.ValueId))
            {
                if (string.IsNullOrWhiteSpace(field.HiddenLinkField))
                {
                    var codeType = GetViewFieldComboCodeType(field);
                    var ids = conn.Query<string>(GetFieldValueIdSql, new { code = rule.ValueId, codeType });
                    if (!ids.Any())
                    {
                        throw new NotFoundException($"FieldValue by code='{rule.ValueId}' and codeType='{codeType}' not found");
                    }
                    rule.ValueId = ids.First();
                }
            }
        }

        /// <summary>
        /// Validate view field  to ensure valid inputs
        /// </summary>
        /// <param name="conn">the connection</param>
        /// <param name="fieldId">the field id</param>
        /// <param name="ensureValuesSelectionType">the ensure values selection type flag</param>
        /// <exception cref="NotFoundException">throws if field not found</exception>
        /// <exception cref="BadRequestException">throws if view field exist invalid values</exception>
        private ViewField ValidateViewField(IDbConnection conn, int fieldId, bool ensureValuesSelectionType = false)
        {
            var ruleFields = conn.Query<ViewField>(ViewRuleFieldSql, new { fieldId }).ToList();
            if (!ruleFields.Any())
            {
                throw new NotFoundException($"ViewField by id='{fieldId}' not found");
            }
            var ruleField = ruleFields.First();
            if (ensureValuesSelectionType && !IsValuesSelectionTypeField(ruleField))
            {
                throw new BadRequestException($"ViewField by id='{fieldId}' is not values selection type");
            }
            ruleField.Id = fieldId;
            return ruleField;
        }

        /// <summary>
        /// Get view field combo type
        /// </summary>
        /// <param name="field">the field</param>
        /// <returns>the view field combo type </returns>
        private string GetViewFieldComboCodeType(ViewField field)
        {
            return !string.IsNullOrWhiteSpace(field.ComboCodeType) ? field.ComboCodeType : field.SystemComboCodeType;
        }

        /// <summary>
        /// Validate date range inputs
        /// </summary>
        /// <param name="fieldValue">the field value</param>
        /// <param name="fieldName">the field name</param>
        /// <param name="formats">the date formats</param>
        /// <returns>the parsed date time</returns>
        private DateTime ValidateDateRange(string fieldValue, string fieldName, string[] formats)
        {
            DateTime date = DateTime.MinValue;
            if (!DateTime.TryParseExact(fieldValue, formats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out date))
            {
                throw new BadRequestException(
                       $"ViewRule by field {fieldName}='{fieldValue}' exist invalid date range format for [{string.Join(",", formats)}]");
            }
            return date;
        }

        /// <summary>
        /// Validate DateRange/DateTimeRange view rule
        /// </summary>
        /// <param name="viewRule">the view rule to validate</param>
        /// <param name="selectionType">the selectionType</param>
        /// <param name="selectionType">the selectionType</param>
        private void ValidateDateRange(ViewRule viewRule, string selectionType, string[] formats)
        {
            if (IsSelectionType(selectionType, viewRule.viewField.SelectionType))
            {
                Helper.ValidateArgumentNotNullOrEmpty(viewRule.BeginRange, nameof(viewRule.BeginRange));
                Helper.ValidateArgumentNotNullOrEmpty(viewRule.EndRange, nameof(viewRule.EndRange));
                Helper.ValidateArgumentNotNullOrEmpty(viewRule.Operand, nameof(viewRule.Operand));
                var beginRange = ValidateDateRange(viewRule.BeginRange, nameof(viewRule.BeginRange), formats);
                var endRange = ValidateDateRange(viewRule.EndRange, nameof(viewRule.EndRange), formats);
                if (DateTime.Compare(beginRange, endRange) > 0)
                {
                    throw new BadRequestException($"ViewRule by field id {viewRule.FieldId} must exist BeginRange<=EndRange");
                }
            }
        }

        /// <summary>
        /// Validate DateRange view rule
        /// </summary>
        /// <param name="viewRule">the view rule to validate</param>
        private void ValidateDateRangeViewRule(ViewRule viewRule)
        {
            ValidateDateRange(viewRule, _appSettings.DateRangeSelectionType, _appSettings.DateRangeFormats);
        }

        /// <summary>
        /// Validate DateTimeRange view rule
        /// </summary>
        /// <param name="viewRule">the view rule to validate</param>
        private void ValidateDateTimeRangeViewRule(ViewRule viewRule)
        {
            ValidateDateRange(viewRule, _appSettings.DateTimeRangeSelectionType, _appSettings.DateTimeRangeFormats);
        }

        /// <summary>
        /// Validate value/operand for view rule
        /// </summary>
        /// <param name="viewRule">the view rule</param>
        private void ValidateViewRuleValueOperand(ViewRule viewRule)
        {
            Helper.ValidateArgumentNotNullOrEmpty(viewRule.Value, nameof(viewRule.Value));
            Helper.ValidateArgumentNotNullOrEmpty(viewRule.Operand, nameof(viewRule.Operand));
        }

        /// <summary>
        /// validate if value or range populated
        /// </summary>
        /// <param name="viewRule">the view rule to validate</param>
        private void ValidateViewRuleValueOrRange(ViewRule viewRule)
        {
            Helper.ValidateArgumentNotNullOrEmpty(viewRule.Operand, nameof(viewRule.Operand));
            if(viewRule.Operand.Contains("BETWEEN", StringComparison.CurrentCultureIgnoreCase))
            {
                Helper.ValidateArgumentNotNullOrEmpty(viewRule.BeginRange, nameof(viewRule.BeginRange));
                Helper.ValidateArgumentNotNullOrEmpty(viewRule.EndRange, nameof(viewRule.EndRange));
            }
            else
            {
                Helper.ValidateArgumentNotNullOrEmpty(viewRule.Value, nameof(viewRule.Value));
            }
        }

        /// <summary>
        /// Validate date range inputs
        /// </summary>
        ///<param name="viewRule">the view rule</param>
        private void ValidateNumber(ViewRule viewRule)
        {
            ValidateViewRuleValueOrRange(viewRule);
            decimal val = -1;
            if (!decimal.TryParse(viewRule.Value, out val) && (!decimal.TryParse(viewRule.BeginRange, out val) || !decimal.TryParse(viewRule.EndRange, out val)))
            {
                throw new BadRequestException($"ViewRule by field {viewRule.FieldId} must exist valid number type value");
            }
        }

        /// <summary>
        /// Validate Number view rule
        /// </summary>
        /// <param name="viewRule">the view rule to validate</param>
        private void ValidateNumberViewRule(ViewRule viewRule)
        {
            if (IsSelectionType(_appSettings.NumberSelectionType, viewRule.viewField.SelectionType))
            {
                ValidateNumber(viewRule);
            }
        }

        /// <summary>
        /// Validate percent value between 0 and 1
        /// </summary>
        /// <param name="val"></param>
        /// <param name="fieldId"></param>
        private void validatePercentValue(string val, int fieldId)
        {
            decimal percent = -1;
            // will store percent as number between 0-1
            if(decimal.TryParse(val, out percent) && (percent < 0 || percent > 1))
            {
                throw new BadRequestException($"ViewRule by field {fieldId} must exist valid percent value");
            }
        }

        /// <summary>
        /// Validate Percent view rule
        /// </summary>
        /// <param name="viewRule">the view rule to validate</param>
        private void ValidatePercentViewRule(ViewRule viewRule)
        {
            if (IsSelectionType(_appSettings.PercentSelectionType, viewRule.viewField.SelectionType))
            {
                // var percent = ValidateNumber(viewRule);
                // // will store percent as number between 0-1 
                // if (percent < 0 || percent > 1)
                // {
                //     throw new BadRequestException($"ViewRule by field {viewRule.FieldId} must exist valid percent value");
                // }
                ValidateNumber(viewRule);
                validatePercentValue(viewRule.Value, viewRule.FieldId);
                validatePercentValue(viewRule.BeginRange, viewRule.FieldId);
                validatePercentValue(viewRule.EndRange, viewRule.FieldId);
            }
        }

        /// <summary>
        /// Validate Text view rule
        /// </summary>
        /// <param name="viewRule">the view rule to validate</param>
        private void ValidateTextViewRule(ViewRule viewRule)
        {
            if (IsSelectionType(_appSettings.TextSelectionType, viewRule.viewField.SelectionType))
            {
                ValidateViewRuleValueOrRange(viewRule);
            }
        }

        /// <summary>
        /// Validate view rule to ensure valid inputs
        /// </summary>
        /// <param name="conn">the connection</param>
        /// <param name="viewRule">the view rule</param>
        /// <exception cref="NotFoundException">throws if rule field not found</exception>
        /// <exception cref="BadRequestException">throws if view rule exist invalid values</exception>
        private void ValidateViewRule(IDbConnection conn, ViewRule viewRule)
        {
            var ruleField = ValidateViewField(conn, viewRule.FieldId);
            viewRule.viewField = ruleField;
            ValidateDateRangeViewRule(viewRule);
            ValidateDateTimeRangeViewRule(viewRule);
            ValidateNumberViewRule(viewRule);
            ValidatePercentViewRule(viewRule);
            ValidateTextViewRule(viewRule);

            var isValuesSelectionType = IsValuesSelectionTypeField(ruleField);
            if (isValuesSelectionType)
            {
                ValidateViewRuleValueOperand(viewRule);
                if (!_appSettings.ValuesFieldOperands.Contains(viewRule.Operand))
                {
                    throw new BadRequestException($"ViewRule by field id='{viewRule.FieldId}' is values selection type and must exist valid Operand [{string.Join(",", _appSettings.ValuesFieldOperands)}]");
                }
                Helper.ValidateArgumentNotNullOrEmpty(viewRule.ValueId, nameof(viewRule.ValueId));
                int id = -1;
                if (!int.TryParse(viewRule.ValueId, out id))
                {
                    throw new BadRequestException($"ViewRule by field id='{viewRule.FieldId}' is values selection type and must exist int type of valueId");
                }
                if (id <= 0)
                {
                    throw new BadRequestException($"ViewRule by field id='{viewRule.FieldId}' is values selection type and must exist positive valueId");
                }
                var codeType = GetViewFieldComboCodeType(ruleField);
                var fieldValues = conn.Query<FieldValue>(GetFieldValueSql, new { codeType, id });
                if (!fieldValues.Any())
                {
                    throw new NotFoundException($"FieldValue by id='{viewRule.ValueId}' not found");
                }
                var fieldValue = fieldValues.First();
                if (string.IsNullOrWhiteSpace(ruleField.HiddenLinkField))
                {
                    viewRule.ValueId = fieldValue.Code;
                }
                var displayValue = GetFieldDisplayValue(ruleField, fieldValue);
                if (!displayValue.Equals(viewRule.Value))
                {
                    throw new BadRequestException($"ViewRule by field {viewRule.FieldId} is values selection type and exist invalid value");
                }
            }

        }

        /// <summary>
        /// Get view rule with view id and rule id
        /// </summary>
        /// <param name="viewId">the view id</param>
        /// <param name="ruleId">the rule id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match view rule with view id and rule id</returns>
        private ViewRule FindValidViewRule(int viewId, int ruleId, JwtUser user)
        {
            EnsureViewOwner(viewId, user);
            Helper.ValidateArgumentPositive(ruleId, nameof(ruleId));
            return ProcessWithDb((conn) =>
            {
                var rules = FindViewRules(conn, GetViewRuleSql, new { viewId, ruleId });
                if (!rules.Any())
                {
                    throw new NotFoundException($"ViewRule by viewId='{viewId}',ruleId='{ruleId}' not found");
                }
                var rule = rules.First();
                ProcessValuesSelectionTypeViewRule(conn, rule);
                return rule;
            }, user);
        }

        /// <summary>
        /// Audit deleted view rule
        /// </summary>
        /// <param name="conn">the connection</param>
        /// <param name="viewId">the view id</param>
        /// <param name="viewRule">the view rule</param>
        /// <param name="user">the jwt user</param>
        private void AuditDeleteViewRule(IDbConnection conn, int viewId, ViewRule viewRule, JwtUser user)
        {
            Audit(conn, new Audit
            {
                UserId = user.UserId,
                OldValue = LoggerHelper.GetObjectDescription(viewRule),
                NewValue = null,
                OperationType = OperationType.Delete,
                ObjectType = nameof(ViewRule),
                Timestamp = DateTime.Now
            });
        }

        /// <summary>
        /// Delete view rule
        /// </summary>
        /// <param name="conn">the connection</param>
        /// <param name="viewId">the view id</param>
        /// <param name="viewRule">the view rule</param>
        /// <param name="user">the jwt user</param>
        private void DeleteViewRule(IDbConnection conn, int viewId, ViewRule viewRule, JwtUser user)
        {
            var ruleId = viewRule.Id;
            conn.Execute(DeleteViewRuleSql, new
            {
                viewId,
                ruleId,
            });
            AuditDeleteViewRule(conn, viewId, viewRule, user);
        }

        /// <summary>
        /// Create view rule
        /// </summary>
        /// <param name="conn">the connection</param>
        /// <param name="viewId">the view id</param>
        /// <param name="viewRule">the view rule</param>
        /// <param name="user">the jwt user</param>
        private ViewRule CreateViewRule(IDbConnection conn, int viewId, ViewRule viewRule, JwtUser user)
        {
            var newId = conn.QuerySingle<int>(CreateViewRuleSql, new
            {
                viewId,
                viewRule.FieldId,
                viewRule.ValueId,
                viewRule.Value,
                viewRule.BeginRange,
                viewRule.EndRange,
                viewRule.Operand
            });
            viewRule.Id = newId;
            Audit(conn, new Audit
            {
                UserId = user.UserId,
                OldValue = null,
                NewValue = LoggerHelper.GetObjectDescription(viewRule),
                OperationType = OperationType.Create,
                ObjectType = nameof(ViewRule),
                Timestamp = DateTime.Now
            });
            return viewRule;
        }
    }
}