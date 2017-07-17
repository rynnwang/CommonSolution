using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova.Web
{
    /// <summary>
    /// Class ConfigurableAction.
    /// </summary>
    public class ConfigurableAction
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the display.
        /// </summary>
        /// <value>
        /// The display.
        /// </value>
        public string Display { get; set; }

        /// <summary>
        /// Gets or sets the name of the area.
        /// </summary>
        /// <value>
        /// The name of the area.
        /// </value>
        public string AreaName { get; set; }

        /// <summary>
        /// Gets or sets the name of the controller.
        /// </summary>
        /// <value>
        /// The name of the controller.
        /// </value>
        public string ControllerName { get; set; }

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        /// <value>
        /// The name of the action.
        /// </value>
        public string ActionName { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            ConfigurableAction action = obj as ConfigurableAction;
            return action != null && Equals(action.ActionName, action.ControllerName, action.AreaName);
        }

        /// <summary>
        /// Equalses the specified action name.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="areaName">Name of the area.</param>
        /// <returns><c>true</c> if equals, <c>false</c> otherwise.</returns>
        public bool Equals(string actionName, string controllerName, string areaName)
        {
            return string.Equals(actionName, this.ActionName, StringComparison.InvariantCultureIgnoreCase)
                          && string.Equals(controllerName, this.ControllerName, StringComparison.InvariantCultureIgnoreCase)
                          && string.Equals(areaName, this.AreaName, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return (this.ActionName.SafeToString().ToLowerInvariant()
                + this.ControllerName.SafeToString().ToLowerInvariant()
                + this.AreaName.SafeToString().ToLowerInvariant()).GetHashCode();
        }
    }
}
