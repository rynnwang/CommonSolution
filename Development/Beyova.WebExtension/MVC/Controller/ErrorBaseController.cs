using System.Web.Mvc;
using Beyova;
using Beyova.ExceptionSystem;

namespace Beyova.Web
{
    /// <summary>
    /// Class ErrorController.
    /// </summary>
    public abstract class ErrorBaseController : BeyovaBaseController
    {
        /// <summary>
        /// Indexes the specified error code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="minor">The minor.</param>
        /// <param name="message">The message.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Index(int? code = null, string minor = null, string message = null)
        {
            var exceptionInfo = new ExceptionInfo
            {
                Code = new ExceptionCode
                {
                    Major = ((ExceptionCode.MajorCode?)code) ?? ExceptionCode.MajorCode.Undefined,
                    Minor = minor
                },
                Message = message
            };

            return View(ErrorView, exceptionInfo);
        }
    }
}