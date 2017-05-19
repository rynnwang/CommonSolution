using System;
using System.Collections.Generic;
using Beyova;
using Beyova.Api;
using Beyova.RestApi;

namespace Beyova.CommonServiceInterface
{
    /// <summary>
    /// Interface IProjectManagementService
    /// </summary>
    /// <typeparam name="TProjectInfo">The type of the t project information.</typeparam>
    /// <typeparam name="TProjectCriteria">The type of the t project criteria.</typeparam>
    [TokenRequired]
    public interface IProjectManagementService<TProjectInfo, TProjectCriteria>
        where TProjectInfo : SaasPlatform.ProjectBase
        where TProjectCriteria : SaasPlatform.ProjectCriteria
    {
        #region Project

        /// <summary>
        /// Creates the or update project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.Project, HttpConstants.HttpMethod.Put)]
        [ApiPermission(CommonServiceConstants.Permission.Administrator, ApiPermission.Required)]
        Guid? CreateOrUpdateProject(TProjectInfo project);

        /// <summary>
        /// Queries the project information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;TProjectInfo&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.Project, HttpConstants.HttpMethod.Post)]
        [ApiPermission(CommonServiceConstants.Permission.Administrator, ApiPermission.Required)]
        List<TProjectInfo> QueryProjectInfo(TProjectCriteria criteria);

        #endregion
    }
}
