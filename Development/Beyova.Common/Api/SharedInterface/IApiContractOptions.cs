using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Beyova.RestApi;

namespace Beyova.Api
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApiContractOptions
    {
        /// <summary>
        /// Gets or sets the realm.
        /// </summary>
        /// <value>
        /// The realm.
        /// </value>
        string Realm { get; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        string Version { get; }
    }
}