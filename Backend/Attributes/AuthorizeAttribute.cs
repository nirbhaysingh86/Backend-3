using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using PMMC.Entities;
using PMMC.Helpers;

namespace PMMC.Attributes
{
    /// <summary>
    /// The Authorize attribute, it will ensure exist jwt user in context and response 401 if not found
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// Check jwt user in context and response 401 if not found
        /// </summary>
        /// <param name="context">the filter context</param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Items[Helper.UserPropertyName] as JwtUser;
            if (user == null)
            {
                // not logged in
                context.Result = new JsonResult(new ApiErrorModel { Message = "Unauthorized" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
        }
    }
}