using System;
using System.Net;

namespace ifunction.Mvc.Extension
{
    /// <summary>
    /// Class HttpJsonServiceProxy.
    /// </summary>
    public abstract class BaseHttpJsonServiceProxy : HttpServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseHttpJsonServiceProxy"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="authenticationSchema">The authentication schema.</param>
        public BaseHttpJsonServiceProxy(Uri baseAddress, AuthenticationSchemes authenticationSchema = AuthenticationSchemes.Anonymous)
            : base(authenticationSchema, baseAddress)
        {
        }
    }
}
