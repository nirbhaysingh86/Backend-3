using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PMMC.Configs;
using PMMC.Entities;
using PMMC.Exceptions;
using PMMC.Helpers;
using PMMC.Interfaces;
using PMMC.Models;
using System.Collections.Generic;
using System.Linq;

namespace PMMC.Services
{
    /// <summary>
    /// The configuration service
    /// </summary>
    public class ConfigurationService : BaseService, IConfigurationService
    {
        /// <summary>
        /// The worklist columns sql
        /// </summary>
        internal const string WorkListColumnsSql =
            "SELECT [ColumnCounter] AS Id,[Caption],[DataField],[DataWidth],[ObjectName],[Width],[ReadOnly],[NumberFormat],[BackColor],[ForeColor],[MultiSelect],[AlternatingRows],[Aggregate],[AutoComplete] FROM [dbo].[tblColumns] WHERE [ObjectName]='fmAccountManagement' AND [ObjectVersion]=1";

        /// <summary>
        /// The default view limits sql
        /// </summary>
        internal const string DefaultViewLimitsSql =
            "SELECT [AMDefaultAuditor] AS Auditor,[AMDefaultFollowUp] AS FollowUp,[AMDefaultStatus] AS Status,[AMDefaultRecordAge] AS AccountAge,[AMDefaultRecordHidden] AS HiddenRecords FROM [dbo].[tblCustom_Configuration]";

        /// <summary>
        /// Constructor with logger and app settings
        /// </summary>
        /// <param name="logger">the logger</param>
        /// <param name="appSettings">the app settings</param>
        public ConfigurationService(ILogger<ConfigurationService> logger, IOptions<AppSettings> appSettings) : base(
            logger, appSettings)
        {
        }

        /// <summary>
        /// Get all worklist columns
        /// </summary>
        /// <param name="user">the jwt user</param>
        /// <returns>all worklist columns</returns>
        public IEnumerable<WorklistColumn> WorkListColumns(JwtUser user)
        {
            return _logger.Process(() =>
                {
                    Helper.ValidateArgumentNotNull(user, nameof(user));
                    return ProcessWithDb((conn) => conn.Query<WorklistColumn>(WorkListColumnsSql), user);
                }, "gets the configuration of worklist columns",
                parameters: new object[] {user});
        }

        /// <summary>
        /// Get default view limits
        /// </summary>
        /// <param name="user">the jwt user</param>
        /// <returns>default view limits</returns>
        public DefaultViewLimits DefaultViewLimits(JwtUser user)
        {
            return _logger.Process(() =>
                {
                    Helper.ValidateArgumentNotNull(user, nameof(user));
                    return ProcessWithDb((conn) =>
                    {
                        var records = conn.Query<DefaultViewLimits>(DefaultViewLimitsSql);
                        var recordsCount = records.Count();
                        if (recordsCount != 1)
                        {
                            throw new InternalServerErrorException(recordsCount == 0
                                ? "Exist no rows for defaultViewLimits"
                                : "Exist more than 1 row for defaultViewLimits");
                        }

                        return records.First();
                    }, user);
                }, "gets the default view limits",
                parameters: new object[] {user});
        }
    }
}