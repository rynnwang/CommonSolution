using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Beyova.Api;
using Beyova.ExceptionSystem;

namespace Beyova.Web
{
    /// <summary>
    /// Class BeyovaPortalController.
    /// </summary>
    [TokenRequired]
    public abstract class BeyovaPortalController : BeyovaBaseController, IRoutable
    {
        /// <summary>
        /// Gets the error partial view.
        /// </summary>
        /// <value>The error partial view.</value>
        protected override string ErrorPartialView
        {
            get
            {
                return PortalViewNames.ErrorPartialView;
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
                return PortalViewNames.ErrorView;
            }
        }

        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        /// <value>
        /// The name of the module.
        /// </value>
        public string ModuleName { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeyovaPortalController" /> class.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="apiTracking">The API tracking.</param>
        /// <param name="returnExceptionAsFriendly">if set to <c>true</c> [return exception as friendly].</param>
        protected BeyovaPortalController(string moduleName, IApiTracking apiTracking = null, bool returnExceptionAsFriendly = false)
            : base(apiTracking, returnExceptionAsFriendly)
        {
            this.ModuleName = moduleName.SafeToString(this.GetDefaultModuleName());
        }

        /// <summary>
        /// Gets the view full path. Result is: "~/Views/shared/Beyova/features/{moduleName}/{viewName}.cshtml". In which, {moduleName} is decided by controller type.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        protected string GetViewFullPath(string viewName)
        {
            return string.Format(PortalViewNames.BeyovaComponentDefaultViewPath, this.ModuleName, viewName);
        }

        /// <summary>
        /// Renders as list partial view.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities">The entities.</param>
        /// <param name="viewPath">The view path.</param>
        /// <param name="defaultFriendlyHint">The default friendly hint.</param>
        /// <returns>PartialViewResult.</returns>
        public virtual PartialViewResult RenderAsListPartialView<T>(ICollection<T> entities, string viewPath = null, FriendlyHint defaultFriendlyHint = null)
        {
            string actualViewName = null;
            try
            {
                actualViewName = viewPath.SafeToString(string.Format(PortalViewNames.DefaultNamingPartialViewPath, this.ModuleName, typeof(T).Name));
                return PartialView(actualViewName, entities);
            }
            catch (Exception ex)
            {
                return HandleExceptionToPartialView(ex, new { entities, actualViewName }, defaultFriendlyHint);
            }
        }

        /// <summary>
        /// Renders as detail view.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="viewPath">The view path.</param>
        /// <param name="defaultFriendlyHint">The default friendly hint.</param>
        /// <returns>PartialViewResult.</returns>
        public virtual PartialViewResult RenderAsDetailView<T>(T entity, string viewPath = null, FriendlyHint defaultFriendlyHint = null)
        {
            string actualViewName = null;
            try
            {
                actualViewName = viewPath.SafeToString(string.Format(PortalViewNames.DefaultNamingDetailViewPath, this.ModuleName, typeof(T).Name));
                return PartialView(actualViewName, entity);
            }
            catch (Exception ex)
            {
                return HandleExceptionToPartialView(ex, new { entity, actualViewName }, defaultFriendlyHint);
            }
        }

        /// <summary>
        /// Renders as panel view.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewPath">The view path.</param>
        /// <param name="defaultFriendlyHint">The default friendly hint.</param>
        /// <returns></returns>
        public virtual ActionResult RenderAsPanelView<T>(string viewPath = null, FriendlyHint defaultFriendlyHint = null)
        {
            string actualViewName = null;
            try
            {
                actualViewName = viewPath.SafeToString(GetDefaultPanelViewPath(typeof(T).Name));
                return View(actualViewName);
            }
            catch (Exception ex)
            {
                return HandleExceptionToRedirection(ex, new { actualViewName }, defaultFriendlyHint);
            }
        }

        /// <summary>
        /// Gets the default panel view path.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="moduleName">Name of the module. If not specified, use <see cref="ModuleName"/> of current controller instead.</param>
        /// <returns></returns>
        public string GetDefaultPanelViewPath(string entityName, string moduleName = null)
        {
            return string.Format(PortalViewNames.DefaultNamingPanelViewPath, moduleName.SafeToString(this.ModuleName), entityName);
        }

        /// <summary>
        /// Gets the component view path.
        /// </summary>
        /// <param name="viewFullName">Full name of the view.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns></returns>
        public string GetComponentViewPath(string viewFullName, string moduleName = null)
        {
            return string.Format(PortalViewNames.BeyovaComponentDefaultViewPath, moduleName.SafeToString(this.ModuleName), viewFullName);
        }

        /// <summary>
        /// Registers the controller to route.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public virtual void RegisterControllerToRoute(RouteCollection routes)
        {
            routes.CheckNullObject(nameof(routes));

            var thisControllerType = this.GetType();
            var name = thisControllerType.Name.TrimEnd("Controller");

            routes.MapRoute(
               name: name,
               url: name + "/{action}/{key}",
               defaults: new { controller = name, action = "Index", key = UrlParameter.Optional },
               namespaces: new string[] { thisControllerType.Namespace }
           );
        }
    }
}
