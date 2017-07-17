using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova
{
    /// <summary>
    /// Class PortalViewNames.
    /// </summary>
    public static class PortalViewNames
    {
        /// <summary>
        /// The default view path. {0}: Feature name, {1}: View Name
        /// </summary>
        public const string BeyovaComponentDefaultViewPath = "~/Views/shared/Beyova/features/{0}/{1}.cshtml";

        /// <summary>
        /// The default naming view path
        /// </summary>
        public const string DefaultNamingDetailViewPath = "~/Views/shared/Beyova/features/{0}/{1}Detail.cshtml";

        /// <summary>
        /// The default naming partial view path
        /// </summary>
        public const string DefaultNamingPartialViewPath = "~/Views/shared/Beyova/features/{0}/_{1}List.cshtml";

        /// <summary>
        /// The default naming panel view path
        /// </summary>
        public const string DefaultNamingPanelViewPath = "~/Views/shared/Beyova/features/{0}/{1}Panel.cshtml";

        #region Error

        /// <summary>
        /// The error view
        /// </summary>
        public const string ErrorView = "~/Views/shared/Beyova/features/Error/Error.cshtml";

        /// <summary>
        /// The error partial view
        /// </summary>
        public const string ErrorPartialView = "~/Views/shared/Beyova/features/Error/_Error.cshtml";

        #endregion
    }
}
