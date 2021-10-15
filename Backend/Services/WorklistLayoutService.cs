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
using System.Linq;

namespace PMMC.Services
{
    /// <summary>
    /// The worklist layout service
    /// </summary>
    public class WorklistLayoutService : BaseService, IWorklistLayoutService
    {
        /// <summary>
        /// The get all worklist layouts sql
        /// </summary>
        internal const string GetAllWorklistLayoutsSql =
            "SELECT [c].[CustomID] AS Id,[c].[CustomName] AS Name,[c].[LayoutDescription] AS Description,[c].[DefaultView] AS IsDefault,[c].[UserID] AS UserId,[cd].[CustomDetailID] AS Id,[cd].[FieldName] AS FieldName,[cd].[FieldLocation] AS Location,[cd].[FieldWidth] AS Width, [cd].[FieldVisible] AS IsVisible  FROM [dbo].[tblColumnsCustom] [c] LEFT JOIN [dbo].[tblColumnsCustomDetail] [cd] ON [cd].[CustomID] = [c].[CustomID]  WHERE [c].[UserID]=@UserId";

        /// <summary>
        /// The get worklist layout by id sql
        /// </summary>
        internal const string GetWorklistLayoutsByIdSql =
            "SELECT [c].[CustomID] AS Id,[c].[CustomName] AS Name,[c].[LayoutDescription] AS Description,[c].[DefaultView] AS IsDefault,[c].[UserID] AS UserId,[cd].[CustomDetailID] AS Id,[cd].[FieldName] AS FieldName,[cd].[FieldLocation] AS Location,[cd].[FieldWidth] AS Width, [cd].[FieldVisible] AS IsVisible  FROM [dbo].[tblColumnsCustom] [c] LEFT JOIN [dbo].[tblColumnsCustomDetail] [cd] ON [cd].[CustomID] = [c].[CustomID]  WHERE [c].[CustomID]=@layoutId";

        /// <summary>
        /// The get worklist layout by name sql
        /// </summary>
        internal const string GetWorklistLayoutByNameSql =
            "SELECT [CustomID] AS Id FROM [dbo].[tblColumnsCustom] WHERE [UserID]=@UserId AND [CustomName]=@Name";

        /// <summary>
        /// The update default worklist layout sql
        /// </summary>
        internal const string UpdateDefaultWorklistLayoutSql =
            "UPDATE  [dbo].[tblColumnsCustom] SET [DefaultView]=0 WHERE [DefaultView]!=0 AND [UserID]=@UserId";

        /// <summary>
        /// The create worklist layout sql
        /// </summary>
        internal const string CreateWorklistLayoutSql =
            "INSERT INTO [dbo].[tblColumnsCustom]([UserID],[CustomName],[DefaultView],[LayoutDescription]) OUTPUT INSERTED.[CustomID] VALUES(@UserId,@Name,@DefaultView,@Description)";

        /// <summary>
        /// The create worklist column layout sql
        /// </summary>
        internal const string CreateWorklistColumnLayoutSql =
            "INSERT INTO [dbo].[tblColumnsCustomDetail]([CustomId],[FieldName],[FieldLocation],[FieldWidth],[FieldVisible]) OUTPUT INSERTED.[CustomDetailID] VALUES(@CustomId,@FieldName,@Location,@Width,@IsVisible)";

        /// <summary>
        /// The update worklist layout sql
        /// </summary>
        internal const string UpdateWorklistLayoutSql =
            "UPDATE [dbo].[tblColumnsCustom] SET [CustomName]=@Name,[LayoutDescription]=@Description,[DefaultView]=@DefaultView WHERE [CustomID]=@layoutId";

        /// <summary>
        /// The get worklist layout column ids sql
        /// </summary>
        internal const string GetWorklistColumnLayoutIdsByIdsSql =
            "SELECT [CustomDetailID] AS Id FROM [dbo].[tblColumnsCustomDetail] WHERE [CustomID]=@layoutId AND [CustomDetailID] IN @Ids";

        /// <summary>
        /// The get worklist layout column ids sql
        /// </summary>
        internal const string GetWorklistColumnLayoutIdsSql =
            "SELECT [CustomDetailID] AS Id FROM [dbo].[tblColumnsCustomDetail] WHERE [CustomID]=@layoutId";

        /// <summary>
        /// The delete worklist column layout sql
        /// </summary>
        internal const string DeleteWorklistColumnLayoutSql =
            "DELETE [dbo].[tblColumnsCustomDetail] WHERE [CustomID]=@layoutId AND [CustomDetailID] IN @Ids";

        /// <summary>
        /// The update worklist column layout sql
        /// </summary>
        internal const string UpdateWorklistColumnLayoutSql =
            "UPDATE [dbo].[tblColumnsCustomDetail] SET [FieldName]=@FieldName,[FieldLocation]=@Location,[FieldWidth]=@Width,[FieldVisible]=@IsVisible WHERE [CustomDetailID]=@Id";

        /// <summary>
        /// The delete worklist layout sql
        /// </summary>
        internal const string DeleteWorklistLayoutSql =
            "DELETE [dbo].[tblColumnsCustom] WHERE [CustomID]=@layoutId AND [UserId] = @UserId";

        /// <summary>
        /// The validate column names sql
        /// </summary>
        internal const string ValidateColumnNamesSql =
            "SELECT [DataField] FROM [dbo].[tblColumns] WHERE [ObjectName]='fmAccountManagement' AND [DataField] IN @columnNames";


        /// <summary>
        /// Constructor with logger and app settings
        /// </summary>
        /// <param name="logger">the logger</param>
        /// <param name="appSettings">the app settings</param>
        public WorklistLayoutService(ILogger<WorklistLayoutService> logger, IOptions<AppSettings> appSettings) : base(logger, appSettings)
        {
        }

        /// <summary>
        /// Get all worklist layouts for current user
        /// </summary>
        /// <param name="user">the jwt user</param>
        /// <returns>all worklist layouts for current user</returns>
        public IEnumerable<WorklistLayout> GetAllWorklistLayouts(JwtUser user)
        {
            return _logger.Process(() =>
            {
                Helper.ValidateArgumentNotNull(user, nameof(user));
                return ProcessWithDb((conn) =>
                {
                    var lookup = new Dictionary<int, WorklistLayout>();
                    conn.Query<WorklistLayout, WorklistColumnLayout, WorklistLayout>(GetAllWorklistLayoutsSql, (worklistLayouts, worklistColumnLayouts) =>
                    {
                        if (!lookup.TryGetValue(worklistLayouts.Id, out WorklistLayout worklistLayout))
                            lookup.Add(worklistLayouts.Id, worklistLayout = worklistLayouts);
                        if (worklistLayout.Columns == null)
                            worklistLayout.Columns = new List<WorklistColumnLayout>();
                        if (worklistColumnLayouts != null)
                            worklistLayout.Columns.Add(worklistColumnLayouts);
                        return worklistLayout;
                    }, new { user.UserId }, splitOn: "Id");
                    if (lookup.Values.Count == 0)
                    {
                        // add default layout with id=1 if current user has no layout defined
                        lookup.Add(1, this.GetWorklistLayoutById(1, user));
                    }
                    return lookup.Values;
                }, user);
            }, "get all current user's worklist layouts",
                parameters: user);
        }

        /// <summary>
        /// Get worklist layout by id
        /// </summary>
        /// <param name="layoutId">the worklist layout id</param>
        /// <param name="user">the jwt user</param>
        /// <returns>match worklist layout by id</returns>
        public WorklistLayout GetWorklistLayoutById(int layoutId, JwtUser user)
        {
            return _logger.Process(() => FindValidWorklistLayout(layoutId, user, true), "get worklist layout with the given Id",
                parameters: new object[] { layoutId, user });
        }

        /// <summary>
        /// Create worklist layout
        /// </summary>
        /// <param name="worklistLayout">the worklist layout to create</param>
        /// <param name="user">the jwt user</param>
        /// <returns>the new created worklist layout with id</returns>
        public WorklistLayout CreateWorklistLayout(WorklistLayout worklistLayout, JwtUser user)
        {
            return _logger.Process(() =>
            {
                ValidateWorklistLayout(worklistLayout, user);
                return ProcessWithDbTransaction((conn) =>
                {
                    UpdateDefaultWorklistLayout(conn, worklistLayout, user);
                    var newId = conn.QuerySingle<int>(CreateWorklistLayoutSql,
                        new
                        {
                            user.UserId,
                            worklistLayout.Name,
                            DefaultView = worklistLayout.IsDefault ? 1 : 0,
                            worklistLayout.Description
                        });
                    worklistLayout.Id = newId;
                    worklistLayout.UserId = user.UserId;
                    for (int i = 0; i < worklistLayout.Columns.Count; i++)
                    {
                        var newColumnId = conn.QuerySingle<int>(CreateWorklistColumnLayoutSql,
                            new
                            {
                                CustomId = newId,
                                worklistLayout.Columns[i].FieldName,
                                worklistLayout.Columns[i].Location,
                                worklistLayout.Columns[i].Width,
                                IsVisible = worklistLayout.Columns[i].IsVisible ? 1 : 0
                            });
                        worklistLayout.Columns[i].Id = newColumnId;
                    }
                    Audit(conn, new Audit
                    {
                        UserId = user.UserId,
                        OldValue = null,
                        NewValue = LoggerHelper.GetObjectDescription(worklistLayout),
                        OperationType = OperationType.Create,
                        ObjectType = nameof(WorklistLayout),
                        Timestamp = DateTime.Now
                    });
                    return worklistLayout;
                }, user);
            }, "creates new worklist layout",
                parameters: new object[] { worklistLayout, user });
        }

        /// <summary>
        /// Update worklist layout
        /// </summary>
        /// <param name="layoutId">the worklist layout id</param>
        /// <param name="worklistLayout">the worklist layout to update</param>
        /// <param name="user">the jwt user</param>
        public WorklistLayout UpdateWorklistLayout(int layoutId, WorklistLayout worklistLayout, JwtUser user)
        {
            return _logger.Process(() =>
            {
                var oldWorklistLayout = FindValidWorklistLayout(layoutId, user);
                ValidateWorklistLayout(worklistLayout, user, layoutId);
                return ProcessWithDbTransaction((conn) =>
                {
                    UpdateDefaultWorklistLayout(conn, worklistLayout, user);
                    conn.Execute(UpdateWorklistLayoutSql,
                        new
                        {
                            layoutId,
                            worklistLayout.Name,
                            DefaultView = worklistLayout.IsDefault ? 1 : 0,
                            worklistLayout.Description
                        });
                    worklistLayout.Id = layoutId;
                    worklistLayout.UserId = user.UserId;
                    var idsToUpdate = worklistLayout.Columns.Select(x => x.Id).Where(x => x != 0).ToArray();
                    var existingColumnIds = conn.Query<int>(GetWorklistColumnLayoutIdsSql, new { layoutId });
                    var toBeDeleted = existingColumnIds.Except(idsToUpdate);
                    if (toBeDeleted.Any())
                    {
                        conn.Execute(DeleteWorklistColumnLayoutSql, new { layoutId, Ids = toBeDeleted.ToArray() });
                    }
                    
                    for (int i = 0; i < worklistLayout.Columns.Count; i++)
                    {
                        if (worklistLayout.Columns[i].Id > 0)
                        {
                            conn.Execute(UpdateWorklistColumnLayoutSql,
                                new
                                {
                                    worklistLayout.Columns[i].FieldName,
                                    worklistLayout.Columns[i].Location,
                                    worklistLayout.Columns[i].Width,
                                    worklistLayout.Columns[i].IsVisible,
                                    worklistLayout.Columns[i].Id,
                                });
                        } else
                        {
                            var newColumnId = conn.QuerySingle<int>(CreateWorklistColumnLayoutSql,
                            new
                            {
                                CustomId = layoutId,
                                worklistLayout.Columns[i].FieldName,
                                worklistLayout.Columns[i].Location,
                                worklistLayout.Columns[i].Width,
                                IsVisible = worklistLayout.Columns[i].IsVisible ? 1 : 0
                            });
                            worklistLayout.Columns[i].Id = newColumnId;
                        }
                    }
                    
                    Audit(conn, new Audit
                    {
                        UserId = user.UserId,
                        OldValue = LoggerHelper.GetObjectDescription(oldWorklistLayout),
                        NewValue = LoggerHelper.GetObjectDescription(worklistLayout),
                        OperationType = OperationType.Update,
                        ObjectType = nameof(WorklistLayout),
                        Timestamp = DateTime.Now
                    });
                    return worklistLayout;
                }, user);
            }, "updates worklist layout with the given Id",
                parameters: new object[] { layoutId, worklistLayout, user });
        }

        /// <summary>
        /// Delete worklist layout by id
        /// </summary>
        /// <param name="layoutId">the worklist layout id</param>
        /// <param name="user">the jwt user</param>
        public void DeleteWorklistLayout(int layoutId, JwtUser user)
        {
            _logger.Process(() =>
            {
                var oldWorklistLayout = FindValidWorklistLayout(layoutId, user);
                ProcessWithDbTransaction((conn) =>
                {
                    if (oldWorklistLayout.Columns.Count > 0)
                    {
                        conn.Execute(DeleteWorklistColumnLayoutSql, new { layoutId, Ids = oldWorklistLayout.Columns.Select(x => x.Id).ToArray() });
                    }
                    conn.Execute(DeleteWorklistLayoutSql, new { layoutId, user.UserId });
                    Audit(conn, new Audit
                    {
                        UserId = user.UserId,
                        OldValue = LoggerHelper.GetObjectDescription(oldWorklistLayout),
                        NewValue = null,
                        OperationType = OperationType.Delete,
                        ObjectType = nameof(WorklistLayout),
                        Timestamp = DateTime.Now
                    });
                }, user);
            }, "deletes worklist layout with the given Id",
                parameters: new object[] { layoutId, user });
        }

        /// <summary>
        /// Get worklist layout by id
        /// </summary>
        /// <param name="layoutId">the worklist layout id</param>
        /// <param name="user">the jwt user</param>
        /// <param name="read">if readonly or not</param>
        /// <returns>match worklist layout by id</returns>
        private WorklistLayout FindValidWorklistLayout(int layoutId, JwtUser user, bool read = false)
        {
            Helper.ValidateArgumentPositive(layoutId, nameof(layoutId));
            Helper.ValidateArgumentNotNull(user, nameof(user));
            return ProcessWithDb((conn) =>
            {
                var lookup = new Dictionary<int, WorklistLayout>();
                var records = conn.Query<WorklistLayout, WorklistColumnLayout, WorklistLayout>(GetWorklistLayoutsByIdSql, (worklistLayouts, worklistColumnLayouts) =>
                {
                    if (!lookup.TryGetValue(worklistLayouts.Id, out WorklistLayout worklistLayout))
                        lookup.Add(worklistLayouts.Id, worklistLayout = worklistLayouts);
                    if (worklistLayout.Columns == null)
                        worklistLayout.Columns = new List<WorklistColumnLayout>();
                    if (worklistColumnLayouts != null)
                        worklistLayout.Columns.Add(worklistColumnLayouts);
                    return worklistLayout;
                }, new { layoutId }, splitOn: "Id");
                if (!lookup.Any())
                {
                    throw new NotFoundException($"Worklist layout by id='{layoutId}' not found");
                }
                var worklistLayout = lookup.First().Value;
                // layout with id=1 can be read by anyone
                if ((!read || worklistLayout.Id != 1) && worklistLayout.UserId != user.UserId)
                {
                    throw new ForbiddenException($"Worklist layout by id='{layoutId}' not belongs to the current user");
                }
                return worklistLayout;
            }, user);
        }

        /// <summary>
        /// Validate worklist layout to ensure valid inputs
        /// </summary>
        /// <param name="worklistLayout">the worklist layout to validate</param>
        /// <param name="user">the jwt user</param>
        /// <param name="layoutId">the updated worklist layout id</param>
        /// <exception cref="BadRequestException">throws if worklist layout is invalid</exception>
        /// <exception cref="NotFoundException">throws if worklist layout is not found</exception>
        private void ValidateWorklistLayout(WorklistLayout worklistLayout, JwtUser user, int? layoutId = null)
        {
            Helper.ValidateArgumentNotNull(worklistLayout, nameof(worklistLayout));
            Helper.ValidateArgumentNotNull(user, nameof(user));
            if (worklistLayout.Columns.Count == 0)
            {
                throw new BadRequestException("Columns array cannot be empty");
            }
            var columnCount = worklistLayout.Columns.Select(x => x.Location).Distinct().Count();
            if (columnCount != worklistLayout.Columns.Count)
            {
                throw new BadRequestException("Columns locations must be unique");
            }
            columnCount = worklistLayout.Columns.Select(x => x.FieldName.ToLower()).Distinct().Count();
            if (columnCount != worklistLayout.Columns.Count)
            {
                throw new BadRequestException("Columns field names must be unique");
            }
            columnCount = worklistLayout.Columns.Where(x => x.IsVisible == true).Count();
            if (columnCount == 0)
            {
                throw new BadRequestException("At least one column must be visible");
            }
            ProcessWithDb((conn) =>
            {
                var columnNames = worklistLayout.Columns.Select(x => x.FieldName).ToArray();
                var foundColumnNames = conn.Query<string>(ValidateColumnNamesSql, new { columnNames });
                var invalidColumnNames = columnNames.Except(foundColumnNames);
                if (invalidColumnNames.Any())
                {
                    throw new BadRequestException($"Columns names: {string.Join(',',invalidColumnNames)} are not valid");
                }
                if (layoutId != null)
                {
                    var idsToUpdate = worklistLayout.Columns.Select(x => x.Id).Where(x => x != 0).ToArray();
                    var foundIds = conn.Query<int>(GetWorklistColumnLayoutIdsByIdsSql, new { layoutId, Ids = idsToUpdate });
                    var difference = idsToUpdate.Except(foundIds);
                    if (difference.Any())
                    {
                        throw new NotFoundException($"Worklist columns layout with layout id = {layoutId} and column ids = '{string.Join(',', difference.ToArray())}' not found in database");
                    }
                }
                var ids = conn.Query<int>(GetWorklistLayoutByNameSql, new { user.UserId, worklistLayout.Name });
                if (ids.Any() && (!layoutId.HasValue || !ids.First().Equals(layoutId.Value)))
                {
                    throw new BadRequestException($"Worklist layout with name='{worklistLayout.Name}' exists in database");
                }
            }, user);
        }

        /// <summary>
        /// Update default worklist layout
        /// </summary>
        /// <param name="conn">the connection</param>
        /// <param name="worklistLayout">the worklistLayout to update</param>
        /// <param name="user">the jwt user</param>
        private void UpdateDefaultWorklistLayout(IDbConnection conn, WorklistLayout worklistLayout, JwtUser user)
        {
            if (worklistLayout.IsDefault)
            {
                conn.Execute(UpdateDefaultWorklistLayoutSql, new { user.UserId });
            }
        }
    }
}
