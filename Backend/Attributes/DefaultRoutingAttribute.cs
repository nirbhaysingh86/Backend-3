using Microsoft.AspNetCore.Mvc.Routing;
using System;

namespace PMMC.Attributes
{
    /// <summary>
    /// Default routing attribute with api version as template for controllers
    /// </summary>
    public class DefaultRoutingAttribute : Attribute, IRouteTemplateProvider
    {
        public string Template => "/api/ver{version:apiVersion}/[controller]";
        /// <summary>
        /// Order is 2 to allow explicitly overriding the default route
        /// </summary>
        public int? Order => 2;

        /// <summary>
        /// The name
        /// </summary>
        public string Name { get; set; }
    }
}
