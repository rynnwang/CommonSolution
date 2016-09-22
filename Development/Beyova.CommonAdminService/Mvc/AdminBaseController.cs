using System;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using System.Web.Routing;
using Beyova.ExceptionSystem;
using Beyova.WebExtension;

namespace Beyova.CommonAdminService
{
    /// <summary>
    /// Class AdminBaseController.
    /// </summary>
    [RestApiContextConsistence]
    public abstract class AdminBaseController : BeyovaBaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdminBaseController" /> class.
        /// </summary>
        protected AdminBaseController(bool returnExceptionAsFriendly = false)
            : base(Framework.ApiTracking, returnExceptionAsFriendly)
        {
        }

        /// <summary>
        /// Gets the view full path.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <returns>System.String.</returns>
        protected abstract string GetViewFullPath(string viewName);

        /// <summary>
        /// Gets the error partial view.
        /// </summary>
        /// <value>The error partial view.</value>
        protected override string ErrorPartialView
        {
            get
            {
                return Constants.ViewNames.ErrorPartialView;
            }
        }

        /// <summary>
        /// Gets the error view.
        /// </summary>
        /// <value>The error view.</value>
        protected override string ErrorView
        {
            get
            {
                return Constants.ViewNames.ErrorView;
            }
        }
    }
}