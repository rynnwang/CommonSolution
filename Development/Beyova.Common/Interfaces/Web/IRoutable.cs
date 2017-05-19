using System.Web.Routing;

namespace Beyova
{
    /// <summary>
    /// Interface IRoutable
    /// </summary>
    public interface IRoutable
    {
        /// <summary>
        /// Registers the controller to route.
        /// </summary>
        /// <param name="routes">The routes.</param>
        void RegisterControllerToRoute(RouteCollection routes);
    }
}
