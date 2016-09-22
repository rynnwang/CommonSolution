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
        /// <summary>
        /// Class Permission.
        /// </summary>
        public static class Permission
        {
            /// <summary>
            /// The administration
            /// </summary>
            public const string Administration = "Administration";

            /// <summary>
            /// The create or update admin user
            /// </summary>
            public const string CreateOrUpdateAdminUser = "CreateOrUpdateAdminUser";

            /// <summary>
            /// The create or update admin permission
            /// </summary>
            public const string CreateOrUpdateAdminPermission = "CreateOrUpdateAdminPermission";
        }

        /// <summary>
        /// Class RouteValues.
        /// </summary>
        public static class RouteValues
        {

        }

        /// <summary>
        /// Class ViewNames.
        /// </summary>
        public static class ViewNames
        {
            /// <summary>
            /// The default view path. {0}: Feature name, {1}: View Name
            /// </summary>
            public const string BeyovaComponentDefaultViewPath = "~/Views/shared/Beyova/features/{0}/{1}.cshtml";

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
            public const string EnvironmentView = "Selection";

            /// <summary>
            /// The environment endpoint panel view
            /// </summary>
            public const string EnvironmentEndpointPanelView = "Panel";

            /// <summary>
            /// The environment endpoint ListView
            /// </summary>
            public const string EnvironmentEndpointListView = "_EndpointList";

            #endregion

            #region SSO

            /// <summary>
            /// The sso authorization partner panel view
            /// </summary>
            public const string SSOAuthorizationPartnerPanelView = "SSOAuthorizationPartnerPanel";

            /// <summary>
            /// The sso authorization partner ListView
            /// </summary>
            public const string SSOAuthorizationPartnerListView = "_SSOAuthorizationPartnerList";

            /// <summary>
            /// The sso authorization panel view
            /// </summary>
            public const string SSOAuthorizationPanelView = "SSOAuthorizationPanel";

            /// <summary>
            /// The sso authorization ListView
            /// </summary>
            public const string SSOAuthorizationListView = "_SSOAuthorizationList";

            #endregion

            #region Azure Blob Console

            /// <summary>
            /// The azure BLOB new item view
            /// </summary>
            public const string AzureBlobNewItemView = "AzureNewBlob";

            /// <summary>
            /// The azure BLOB panel view
            /// </summary>
            public const string AzureBlobPanelView = "AzureBlobPanel";

            /// <summary>
            /// The azure BLOB ListView
            /// </summary>
            public const string AzureBlobListView = "_AzureBlobList";

            #endregion

            #region Binary Storage

            /// <summary>
            /// The binary storage panel view
            /// </summary>
            public const string BinaryStoragePanelView = "BinaryStoragePanel";

            /// <summary>
            /// The new binary storage view
            /// </summary>
            public const string NewBinaryStorageView = "UploadBinaryStorage";

            /// <summary>
            /// The binary storage ListView
            /// </summary>
            public const string BinaryStorageListView = "_BinaryStorageList";

            #endregion

            #region API Tracking

            /// <summary>
            /// The API event detail view
            /// </summary>
            public const string ApiEventDetailView = "ApiEventDetail";

            /// <summary>
            /// The API event panel view
            /// </summary>
            public const string ApiEventPanelView = "ApiEventPanel";

            /// <summary>
            /// The API event ListView
            /// </summary>
            public const string ApiEventListView = "_ApiEventList";

            /// <summary>
            /// The API event statistic panel
            /// </summary>
            public const string ApiEventStatisticPanel = "ApiEventStatisticPanel";

            /// <summary>
            /// The API event statistic line bar mixed chart
            /// </summary>
            public const string ApiEventStatisticLineBarMixedChart = "_ApiEventStatisticLineBarMixedChart";

            /// <summary>
            /// The API event duration statistic ranged line chart
            /// </summary>
            public const string ApiEventDurationStatisticRangedLineChart = "_ApiEventDurationStatisticRangedLineChart";

            /// <summary>
            /// The API event server statistic bar chart
            /// </summary>
            public const string ApiEventServerStatisticBarChart = "_ApiEventServerStatisticBarChart";

            /// <summary>
            /// The API event service statistic bar chart
            /// </summary>
            public const string ApiEventServiceStatisticBarChart = "_ApiEventServiceStatisticBarChart";

            /// <summary>
            /// The API trace panel view
            /// </summary>
            public const string ApiTracePanelView = "ApiTracePanel";

            /// <summary>
            /// The API trace detail view
            /// </summary>
            public const string ApiTraceDetailView = "_ApiTraceDetail";

            /// <summary>
            /// The exception panel view
            /// </summary>
            public const string ExceptionPanelView = "ApiExceptionPanel";

            /// <summary>
            /// The exception ListView
            /// </summary>
            public const string ExceptionListView = "_ApiExceptionList";

            /// <summary>
            /// The exception detail view
            /// </summary>
            public const string ExceptionDetailView = "_ApiExceptionDetail";

            /// <summary>
            /// The API exception statistic panel
            /// </summary>
            public const string ApiExceptionStatisticPanel = "ApiExceptionStatisticPanel";

            /// <summary>
            /// The API exception statistic stack column chart
            /// </summary>
            public const string ApiExceptionStatisticStackColumnChart = "_ApiExceptionStatisticStackColumnChart";

            #endregion

            #region Administration

            /// <summary>
            /// The admin user panel
            /// </summary>
            public const string AdminUserPanel = "AdminUserPanel";

            /// <summary>
            /// The admin user list
            /// </summary>
            public const string AdminUserList = "_AdminUserList";

            /// <summary>
            /// The admin role panel
            /// </summary>
            public const string AdminRolePanel = "AdminRolePanel";

            /// <summary>
            /// The admin role list
            /// </summary>
            public const string AdminRoleList = "_AdminRoleList";

            /// <summary>
            /// The admin permission panel
            /// </summary>
            public const string AdminPermissionPanel = "AdminPermissionPanel";

            /// <summary>
            /// The admin permission list
            /// </summary>
            public const string AdminPermissionList = "_AdminPermissionList";

            /// <summary>
            /// The admin user role binding
            /// </summary>
            public const string AdminUserRoleBinding = "AdminUserRoleBinding";

            /// <summary>
            /// The admin role permission binding
            /// </summary>
            public const string AdminRolePermissionBinding = "AdminRolePermissionBinding";

            #endregion

            #region  Code Smith

            /// <summary>
            /// The code smith panel
            /// </summary>
            public const string CodeSmithPanel = "CodeSmithPanel";

            /// <summary>
            /// The code workshop
            /// </summary>
            public const string CodeWorkshop = "CodeWorkshop";

            #endregion

            #region Authentication

            /// <summary>
            /// The user panel
            /// </summary>
            public const string UserPanel = "UserPanel";

            /// <summary>
            /// The user detail
            /// </summary>
            public const string UserDetail = "UserDetail";

            /// <summary>
            /// The user list
            /// </summary>
            public const string UserList = "_UserList";

            /// <summary>
            /// The session panel
            /// </summary>
            public const string SessionPanel = "SessionPanel";

            /// <summary>
            /// The session list
            /// </summary>
            public const string SessionList = "_SessionList";

            /// <summary>
            /// The session detail
            /// </summary>
            public const string SessionDetail = "SessionDetail";

            #endregion
        }
    }
}
