using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Beyova.ExceptionSystem;
using Newtonsoft.Json;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class CentralManagementContext.
    /// </summary>
    public static class CentralManagementContext
    {
        /// <summary>
        /// </summary>
        [ThreadStatic]
        private static Guid? _parameterProjectKey;

        /// <summary>
        /// Gets the parameter project key.
        /// </summary>
        /// <value>The parameter project key.</value>
        public static Guid? ParameterProjectKey
        {
            get { return _parameterProjectKey; }
            internal set { _parameterProjectKey = value; }
        }

        /// <summary>
        /// Gets the project key.
        /// </summary>
        /// <value>The project key.</value>
        public static Guid? ProjectKey
        {
            get
            {
                var adminUser = ContextHelper.CurrentUserInfo as AdminUserInfoBase;
                adminUser.CheckNullObject(nameof(adminUser));

                return adminUser.ProjectKey ?? _parameterProjectKey;
            }
        }

        /// <summary>
        /// Ensures the project scope.
        /// </summary>
        /// <param name="projectBasedObject">The project based object.</param>
        public static void EnsureProjectScope(IProjectBased projectBasedObject)
        {
            if (projectBasedObject != null)
            {
                projectBasedObject.ProjectKey = ProjectKey;
            }
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        internal static void Dispose()
        {
            _parameterProjectKey = null;
        }
    }
}
