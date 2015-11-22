using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ifunction;
using ifunction.RestApi;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Interface IApiContractDescription
    /// </summary>
    public interface IApiContractDescription: IJsonSerializable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
        string Namespace { get; }

        /// <summary>
        /// Gets or sets the name of the primary.
        /// </summary>
        /// <value>The name of the primary.</value>
        string Name { get; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        ApiContractType Type { get; }

        #endregion
    }
}
