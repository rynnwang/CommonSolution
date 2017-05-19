using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Beyova.AOP
{
    /// <summary>
    /// Class AopProxy.
    /// </summary>
    public abstract class AopProxy<T>
    {
        /// <summary>
        /// The instance
        /// </summary>
        protected T _instance;

        /// <summary>
        /// The injection delegates
        /// </summary>
        protected MethodInjectionDelegates _injectionDelegates;

        /// <summary>
        /// Initializes a new instance of the <see cref="AopProxy{T}" /> class.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="injectionDelegates">The injection delegates.</param>
        protected AopProxy(T instance, MethodInjectionDelegates injectionDelegates)
        {
            _instance = instance;
            _injectionDelegates = injectionDelegates ?? new MethodInjectionDelegates();
        }
    }
}
