using System;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityEventHookAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class GravityEventHookAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the hook.
        /// </summary>
        /// <value>The hook.</value>
        public GravityEventHook Hook { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GravityEventHookAttribute"/> class.
        /// </summary>
        /// <param name="type">The type. Type should inherited from <see cref="GravityEventHook"/> and have constructor with no parameter.</param>
        public GravityEventHookAttribute(Type type)
        {
            if (type.IsAssignableFrom(typeof(GravityEventHook)))
            {
                this.Hook = Activator.CreateInstance(type) as GravityEventHook;
            }
        }
    }
}