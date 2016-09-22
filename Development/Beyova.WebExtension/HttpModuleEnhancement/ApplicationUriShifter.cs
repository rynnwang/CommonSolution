using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace Beyova.WebExtension
{
    /// <summary>
    /// Class ApplicationUriShifter.
    /// </summary>
    public sealed class ApplicationUriShifter : IHttpModule
    {
        /// <summary>
        /// The _shifters
        /// </summary>
        internal static List<StringShifter> _shifters = new List<StringShifter>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUriShifter" /> class.
        /// </summary>
        internal ApplicationUriShifter()
        {
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule" />.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Initializes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Init(HttpApplication context)
        {
            string destination = null;
            string originalUrl = context.Request.RawUrl;

            foreach (var one in _shifters)
            {
                if (one.Shift(originalUrl, out destination))
                {
                    try
                    {
                        context.Context.RewritePath(destination);
                    }
                    catch (Exception ex)
                    {
                        Framework.ApiTracking?.LogException(ex.Handle(data: new { originalUrl, destination }).ToExceptionInfo());
                        context.Context.RewritePath(originalUrl);
                    }
                    break;
                }
            }
        }

        #region 

        private static bool alreadyRegistered = false;

        /// <summary>
        /// Registers the application URI shifts.
        /// </summary>
        /// <param name="shifters">The shifters.</param>
        /// <returns>System.Boolean.</returns>
        public static bool RegisterApplicationUriShifts(params StringShifter[] shifters)
        {
            if (alreadyRegistered)
            {
                return false;
            }

            if (shifters.HasItem())
            {
                _shifters.AddRange(shifters);
                DynamicModuleUtility.RegisterModule(typeof(ApplicationUriShifter));
                alreadyRegistered = true;

                return true;
            }

            return false;
        }

        #endregion
    }
}
