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
    /// Class AbstractApiContractDescription.
    /// </summary>
    public abstract class AbstractApiContractDescription
    {
        #region Properties

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the name of the primary.
        /// </summary>
        /// <value>The name of the primary.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public ApiContractType Type { get; protected set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractApiContractDescription"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        protected AbstractApiContractDescription(ApiContractType type)
        {
            this.Type = type;
        }

        #region Static methods

        /// <summary>
        /// Parses the API contract definition.
        /// </summary>
        /// <param name="interfaceOrInstanceType">Type of the interface or instance.</param>
        /// <returns>ApiContractDefinition.</returns>
        public static ApiContractDefinition ParseApiContractDefinition(Type interfaceOrInstanceType)
        {
            try
            {
                return interfaceOrInstanceType.ParseApiContractDefinition();
            }
            catch (Exception ex)
            {
                throw ex.Handle("ParseApiContractDefinition", interfaceOrInstanceType);
            }
        }


        /// <summary>
        /// Parses the API model.
        /// </summary>
        /// <param name="classOrStructType">Type of the class or structure.</param>
        /// <returns>ApiModelDefinition.</returns>
        public static ApiDataContractDefinition ParseApiDataContract(Type classOrStructType)
        {
            try
            {
                //return classOrStructType.ParseApiDataContract();
                return null;
            }
            catch (Exception ex)
            {
                throw ex.Handle("ParseApiDataContract", classOrStructType);
            }
        }

        #endregion
    }
}
