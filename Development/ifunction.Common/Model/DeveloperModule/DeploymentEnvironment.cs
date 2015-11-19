using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ifunction.RestApi;

namespace ifunction.DeveloperModule
{
    /// <summary>
    /// Enum DeploymentEnvironment
    /// </summary>
    public enum DeploymentEnvironment
    {
        /// <summary>
        /// Value indicating it is undefined
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// Value indicating it is debug
        /// </summary>
        Debug = 1,
        /// <summary>
        /// Value indicating it is development
        /// </summary>
        Development,
        /// <summary>
        /// Value indicating it is testing
        /// </summary>
        Testing,
        /// <summary>
        /// Value indicating it is staging
        /// </summary>
        Staging,
        /// <summary>
        /// Value indicating it is uat
        /// </summary>
        UAT,
        /// <summary>
        /// Value indicating it is production
        /// </summary>
        Production,
        /// <summary>
        /// Value indicating it is demo
        /// </summary>
        Demo
    }
}
