using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PMMC.Configs;
using PMMC.Entities;
using PMMC.Exceptions;
using PMMC.Helpers;
using PMMC.Models;
using System;
using System.Data;
using System.Transactions;

namespace PMMC.Services
{
    /// <summary>
    /// The base service for all services
    /// </summary>
    public abstract class BaseService
    {
        /// <summary>
        /// The create audit sql
        /// </summary>
        internal const string CreateAuditSql =
            "INSERT INTO [dbo].[tblAudits]([UserId],[OldValue],[NewValue],[OperationType],[ObjectType],[Timestamp]) VALUES(@UserId,@OldValue,@NewValue,@OperationType,@ObjectType,@Timestamp)";


        /// <summary>
        /// The logger used in base service
        /// </summary>
        protected readonly ILogger<BaseService> _logger;

        /// <summary>
        /// The app settings
        /// </summary>
        protected readonly AppSettings _appSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService"/> class.
        /// </summary>
        protected BaseService(ILogger<BaseService> logger, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
        }


        /// <summary>
        /// Processes the specified function with database and jwt user
        /// </summary>
        /// <typeparam name="T">The type of the function result</typeparam>
        /// <param name="function">The function to process</param>
        /// <param name="user">The jwt user</param>
        /// <returns>The result of the function</returns>
        /// <exception cref="UnauthorizedException">throws if jwt user error to connect to database</exception>
        protected T ProcessWithDb<T>(Func<IDbConnection, T> function, JwtUser user)
        {
            using (var conn = new SqlConnection(Helper.GetConnectionString(_appSettings, user.DbServer, user.DbName)))
            {
                OpenConnectionForJwtUser(conn, user);
                return function(conn);
            }
        }

        /// <summary>
        /// Processes the specified action with database and jwt user
        /// </summary>
        /// <param name="action">The action to process</param>
        /// <param name="user">The jwt user</param>
        /// <exception cref="UnauthorizedException">throws if jwt user error to connect to database</exception>
        protected void ProcessWithDb(Action<IDbConnection> action, JwtUser user)
        {
            using (var conn = new SqlConnection(Helper.GetConnectionString(_appSettings, user.DbServer, user.DbName)))
            {
                OpenConnectionForJwtUser(conn, user);
                action(conn);
            }
        }


        /// <summary>
        /// Processes the specified function with database transaction and jwt user
        /// </summary>
        /// <typeparam name="T">The type of the function result</typeparam>
        /// <param name="function">The function to process</param>
        /// <param name="user">The jwt user</param>
        /// <returns>The result of the function</returns>
        /// <exception cref="UnauthorizedException">throws if jwt user error to connect to database</exception>
        protected T ProcessWithDbTransaction<T>(Func<IDbConnection, T> function, JwtUser user)
        {
            T result = default(T);
            using (var transaction = new TransactionScope())
            {
                result = ProcessWithDb(function, user);
                transaction.Complete();
            }

            return result;
        }

        /// <summary>
        /// Processes the specified action with database transaction and jwt user
        /// </summary>
        /// <param name="action">The action to process</param>
        /// <param name="user">The jwt user</param>
        /// <exception cref="UnauthorizedException">throws if jwt user error to connect to database</exception>
        protected void ProcessWithDbTransaction(Action<IDbConnection> action, JwtUser user)
        {
            using (var transaction = new TransactionScope())
            {
                ProcessWithDb(action, user);
                transaction.Complete();
            }
        }

        /// <summary>
        /// Processes the specified function with database and auth request
        /// </summary>
        /// <typeparam name="T">The type of the function result</typeparam>
        /// <param name="function">The function to process</param>
        /// <param name="authRequest">The auth request</param>
        /// <returns>The result of the function</returns>
        /// <exception cref="BadRequestException">throws if auth user error to connect to database</exception>
        protected T ProcessWithDb<T>(Func<IDbConnection, T> function, AuthRequest authRequest)
        {
            using (var conn =
                new SqlConnection(Helper.GetConnectionString(_appSettings, authRequest.Server, authRequest.DbName)))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception e)
                {
                    throw new BadRequestException(
                        $"Error to connect to database(server='{authRequest.Server}',db='{authRequest.DbName}') with error `{e.Message}`");
                }

                return function(conn);
            }
        }

        /// <summary>
        /// Audit database operations
        /// </summary>
        /// <param name="conn">the database connection</param>
        /// <param name="audit">the audit record</param>
        protected void Audit(IDbConnection conn, Audit audit)
        {
            conn.Execute(CreateAuditSql, new
            {
                audit.UserId,
                audit.OldValue,
                audit.NewValue,
                OperationType = audit.OperationType.ToString(),
                ObjectType = audit.ObjectType,
                audit.Timestamp
            });
        }

        /// <summary>
        /// Open database connection for jwt user
        /// and will throw 401 error if error to connect to dabase later
        /// </summary>
        /// <param name="conn">the connection </param>
        /// <param name="user">the jwt user</param>
        /// <exception cref="UnauthorizedException">throws if jwt user error to connect to database</exception>
        private void OpenConnectionForJwtUser(IDbConnection conn, JwtUser user)
        {
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                throw new UnauthorizedException(
                    $"Error to connect to database(server='{user.DbServer}',db='{user.DbName}') with error `{e.Message}`");
            }
        }
    }
}