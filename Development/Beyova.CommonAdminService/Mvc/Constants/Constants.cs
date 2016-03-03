using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova.CommonAdminService
{
    /// <summary>
    /// Class Constants.
    /// </summary>
    public static class Constants
    {
        public static class Permission
        {
            public const string Administration = "Administration";

            public const string CreateOrUpdateAdminUser = "CreateOrUpdateAdminUser";

            public const string CreateOrUpdateAdminPermission = "CreateOrUpdateAdminPermission";
        }

        public static class RouteValues
        {

        }

        /// <summary>
        /// Class ViewNames.
        /// </summary>
        public static class ViewNames
        {
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

            #region Environment Endpoint

            /// <summary>
            /// The environment view
            /// </summary>
            public const string EnvironmentView = "~/Views/shared/Beyova/features/EnvironmentEndpoint/Selection.cshtml";

            /// <summary>
            /// The environment endpoint panel view
            /// </summary>
            public const string EnvironmentEndpointPanelView = "~/Views/shared/Beyova/features/EnvironmentEndpoint/Panel.cshtml";

            /// <summary>
            /// The environment endpoint ListView
            /// </summary>
            public const string EnvironmentEndpointListView = "~/Views/shared/Beyova/features/EnvironmentEndpoint/_EndpointList.cshtml";

            #endregion

            #region API Tracking

            public const string ApiEventDetailView = "~/Views/shared/Beyova/features/ApiTracking/ApiEventDetail.cshtml";

            public const string ApiEventPanelView = "~/Views/shared/Beyova/features/ApiTracking/ApiEventPanel.cshtml";

            public const string ApiEventListView = "~/Views/shared/Beyova/features/ApiTracking/_ApiEventList.cshtml";

            public const string ApiTracePanelView = "~/Views/shared/Beyova/features/ApiTracking/ApiTracePanel.cshtml";

            public const string ApiTraceDetailView = "~/Views/shared/Beyova/features/ApiTracking/_ApiTraceDetail.cshtml";

            public const string ExceptionPanelView = "~/Views/shared/Beyova/features/ApiTracking/ExceptionPanel.cshtml";

            public const string ExceptionListView = "~/Views/shared/Beyova/features/ApiTracking/_ExceptionList.cshtml";

            public const string ExceptionDetailView = "~/Views/shared/Beyova/features/ApiTracking/_ExceptionDetail.cshtml";

            #endregion

            #region Administration

            public const string AdminUserPanel = "~/Views/shared/Beyova/features/Admin/AdminUserPanel.cshtml";

            public const string AdminUserList = "~/Views/shared/Beyova/features/Admin/_AdminUserList.cshtml";

            public const string AdminRolePanel = "~/Views/shared/Beyova/features/Admin/AdminRolePanel.cshtml";

            public const string AdminRoleList = "~/Views/shared/Beyova/features/Admin/_AdminRoleList.cshtml";

            public const string AdminPermissionPanel = "~/Views/shared/Beyova/features/Admin/AdminPermissionPanel.cshtml";

            public const string AdminPermissionList = "~/Views/shared/Beyova/features/Admin/_AdminPermissionList.cshtml";

            public const string AdminUserRoleBinding = "~/Views/shared/Beyova/features/Admin/AdminUserRoleBinding.cshtml";

            public const string AdminRolePermissionBinding = "~/Views/shared/Beyova/features/Admin/AdminRolePermissionBinding.cshtml";

            #endregion
        }
    }
}
