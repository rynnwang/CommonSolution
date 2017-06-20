using System;

namespace Beyova
{
    /// <summary>
    /// Class SandboxMarshalObject.
    /// </summary>
    public abstract class SandboxMarshalObject : MarshalByRefObject
    {
        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease" /> used to control the lifetime policy for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime" /> property.</returns>
        public override object InitializeLifetimeService()
        {
            // To avoid be cleaned by app domain.
            // http://blogs.microsoft.co.il/sasha/2008/07/19/appdomains-and-remoting-life-time-service/

            return null;
        }
    }
}