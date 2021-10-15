using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMMC.Entities;
using PMMC.Helpers;

namespace PMMC.Controllers
{
    /// <summary>
    /// The base controller for all controller except auth controller
    /// </summary>
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// The http context accessor 
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Constructor with http context accessor
        /// </summary>
        /// <param name="httpContextAccessor">the http context accessor</param>
        protected BaseController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets the current user from context.
        /// </summary>
        ///
        /// <value>
        /// The current user.
        /// </value>
        protected JwtUser CurrentUser
        {
            get { return _httpContextAccessor.HttpContext.Items[Helper.UserPropertyName] as JwtUser; }
        }
    }
}