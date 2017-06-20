using System;

namespace Beyova.AOP
{
    /// <summary>
    /// Class AOPInterceptorObject.
    /// </summary>
    [Obsolete("MarshalObject Based AOP is retired. Please use Proxy based AOP", true)]
    public class AOPInterceptorObject : ContextBoundObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AOPInterceptorObject"/> class.
        /// </summary>
        public AOPInterceptorObject()
            : base()
        {
        }
    }
}