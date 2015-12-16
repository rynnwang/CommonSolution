using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Beyova.WebExtension
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
