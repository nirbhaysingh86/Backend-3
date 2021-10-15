using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PMMC.Configs;
using PMMC.Entities;
using PMMC.Helpers;
using PMMC.Interfaces;
using PMMC.Models;
using System;
using System.Collections.Generic;

namespace PMMC.Services
{
    /// <summary>
    /// The lookup service
    /// </summary>
    public class LookupService : BaseService, ILookupService
    {
        /// <summary>
        /// Get system values by code type sql
        /// </summary>
        internal const string SystemValuesSql =
            "SELECT [Code],[Description] FROM [dbo].[tblComboBoxesSystemValues] WHERE [CodeType]=@codeType ORDER BY [RankOrder]";

        /// <summary>
        /// Constructor with logger and app settings
        /// </summary>
        /// <param name="logger">the logger</param>
        /// <param name="appSettings">the app settings</param>
        public LookupService(ILogger<LookupService> logger, IOptions<AppSettings> appSettings) : base(logger,
            appSettings)
        {
        }

        /// <summary>
        /// Get all system values by code type
        /// </summary>
        /// <param name="codeType">the code type</param>
        /// <param name="user">the jwt user</param>
        /// <returns>all system values by code type</returns>
        public IEnumerable<SystemValue> SystemValues(string codeType, JwtUser user)
        {
            return _logger.Process(() =>
                {
                    Helper.ValidateArgumentNotNullOrEmpty(codeType, nameof(codeType));
                    Helper.ValidateArgumentNotNull(user, nameof(user));
                    if (!_appSettings.CodeTypes.ContainsValue(codeType))
                    {
                        throw new ArgumentException(
                            $"The codeType `{codeType}` is invalid in [{string.Join(",", _appSettings.CodeTypes.Values())}]");
                    }

                    return ProcessWithDb((conn) => conn.Query<SystemValue>(SystemValuesSql, new {codeType}), user);
                }, "gets the system values for the given code type",
                parameters: new object[] {codeType, user});
        }
    }
}