using System;
using System.Web.Mvc;
using Beyova.Gravity;

namespace Beyova.Web
{
    /// <summary>
    /// Class ProductBasedActionAttribute. This attribute would try to initialize product key in CentralManagementContext.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ProductBasedActionAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Gets or sets the name of the key.
        /// </summary>
        /// <value>The name of the key.</value>
        public string KeyName { get; protected set; }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductBasedActionAttribute"/> class.
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        public ProductBasedActionAttribute(string keyName = null) : base()
        {
            this.KeyName = keyName.SafeToString("productKey");
        }

        #endregion

        /// <summary>
        /// Called when [result executed].
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);

            CentralManagementContext.Dispose();
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var productKey = (filterContext.RequestContext.RouteData.Values[KeyName] ?? filterContext.HttpContext.Request.QueryString[KeyName]).ObjectToGuid();
            if (productKey.HasValue)
            {
                CentralManagementContext.ParameterProductKey = productKey;
            }
        }
    }
}