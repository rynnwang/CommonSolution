using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Beyova;
using Beyova.Api;
using Beyova.ExceptionSystem;
using Beyova.RestApi;

namespace Beyova.WebExtension
{
    /// <summary>
    /// Class ApiViewAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class ApiViewAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the view.
        /// </summary>
        /// <value>The name of the view.</value>
        public string ViewName { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiViewAttribute"/> class.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        public ApiViewAttribute(string viewName)
            : base()
        {
            this.ViewName = viewName;
        }
    }
}