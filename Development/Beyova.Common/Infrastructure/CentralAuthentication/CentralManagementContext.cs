using System;
using System.Collections.Generic;
using Beyova.ExceptionSystem;

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
                var adminUser = ContextHelper.CurrentUserInfo as AdminUserInfo;
                adminUser.CheckNullObject(nameof(adminUser));

                return adminUser.ProjectKey ?? _parameterProjectKey;
            }
        }

        /// <summary>
        /// Ensures the project scope.
        /// </summary>
        /// <param name="projectBasedObject">The project based object.</param>
        /// <exception cref="UnauthorizedOperationException">ProjectOwnership</exception>
        public static void EnsureProjectScope(this IProjectBased projectBasedObject)
        {
            if (projectBasedObject != null)
            {
                var adminUser = ContextHelper.CurrentUserInfo as AdminUserInfo;
                adminUser.CheckNullObject(nameof(adminUser));

                if (_parameterProjectKey.HasValue)
                {
                    if (!adminUser.ProjectKey.HasValue || adminUser.ProjectKey.Value == _parameterProjectKey.Value)
                    {
                        projectBasedObject.ProjectKey = _parameterProjectKey;
                    }
                    else
                    {
                        throw new UnauthorizedOperationException("ProjectOwnership", new { ProjectKey = projectBasedObject?.ProjectKey });
                    }
                }
                else
                {
                    projectBasedObject.ProjectKey = adminUser.ProjectKey ?? projectBasedObject.ProjectKey;
                }
            }
        }

        /// <summary>
        /// Ensures the project scope.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="projectBasedObjects">The project based objects.</param>
        /// <param name="forceApplyUrlParameterProjectKey">if set to <c>true</c> [force apply URL parameter project key].</param>
        public static void EnsureProjectScope<T>(this IEnumerable<T> projectBasedObjects, bool forceApplyUrlParameterProjectKey = false)
            where T : IProjectBased
        {
            if (projectBasedObjects.HasItem())
            {
                var adminUser = ContextHelper.CurrentUserInfo as AdminUserInfo;
                adminUser.CheckNullObject(nameof(adminUser));

                foreach (var projectBasedObject in projectBasedObjects)
                {
                    if (forceApplyUrlParameterProjectKey && _parameterProjectKey.HasValue)
                    {
                        projectBasedObject.ProjectKey = _parameterProjectKey;
                    }

                    projectBasedObject.ProjectKey = adminUser.ProjectKey ?? projectBasedObject.ProjectKey;
                }
            }
        }

        /// <summary>
        /// Validates the project scope.
        /// </summary>
        /// <param name="projectBasedObject">The project based object.</param>
        /// <returns>Return true if validation passed.</returns>
        public static bool ValidateProjectScope(this IProjectBased projectBasedObject)
        {
            if (projectBasedObject != null)
            {
                var adminUser = ContextHelper.CurrentUserInfo as AdminUserInfo;
                adminUser.CheckNullObject(nameof(adminUser));

                if (!adminUser.ProjectKey.HasValue || adminUser.ProjectKey.Value == projectBasedObject.ProjectKey)
                {
                    return true;
                }
            }

            return false;
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