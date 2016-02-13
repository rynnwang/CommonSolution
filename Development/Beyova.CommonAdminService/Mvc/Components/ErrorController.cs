using System.Web.Mvc;
using Beyova;
using Beyova.ExceptionSystem;

namespace Beyova.CommonAdminService
{
    /// <summary>
    /// Class ErrorController.
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// Indexes the specified error code.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Index(int? errorCode = null)
        {
            var exception = "Exception".GetThreadData() as BaseException;
            if (exception != null)
            {
                errorCode = (int)exception.Code.Major;
            }

            return View(Constants.ViewNames.ErrorView, (ExceptionCode.MajorCode)(errorCode ?? 0));
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