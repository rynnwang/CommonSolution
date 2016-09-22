using System;
using System.Web.Mvc;
using Beyova;
using Beyova.ExceptionSystem;
using Beyova.WebExtension;

namespace Beyova.CommonAdminService
{
    /// <summary>
    /// Class ErrorController.
    /// </summary>
    public class ErrorController : ErrorBaseController
    {
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

        /// <summary>
        /// Forbiddens the by ownership.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult ForbiddenByOwnership()
        {
            return View("ForbiddenByOwnership");
        }
    }
}