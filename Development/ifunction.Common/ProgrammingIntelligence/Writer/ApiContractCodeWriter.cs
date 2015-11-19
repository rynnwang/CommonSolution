using System;
using System.Reflection;
using System.Text;
using ifunction.RestApi;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ifunction;
using System.Collections;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class ApiContractCodeWriter.
    /// </summary>
    public abstract class ApiContractCodeWriter
    {
        /// <summary>
        /// Gets the indent.
        /// </summary>
        /// <value>The indent.</value>
        protected virtual string Indent { get { return "  "; } }

        /// <summary>
        /// The _api contract definition
        /// </summary>
        protected ApiContractDefinition _apiContractDefinition;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiContractCodeWriter"/> class.
        /// </summary>
        protected ApiContractCodeWriter(ApiContractDefinition apiContractDefinition)
        {
            _apiContractDefinition = apiContractDefinition;
        }

        /// <summary>
        /// Internals the write API contract.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="contractDefinition">The contract definition.</param>
        protected abstract void InternalWriteApiContract(StringBuilder builder, ApiContractDefinition contractDefinition);

        /// <summary>
        /// Internals the write API operation.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="operationDefinition">The operation definition.</param>
        protected abstract void InternalWriteApiOperation(StringBuilder builder, ApiOperationDefinition operationDefinition);

        /// <summary>
        /// Internals the write data contract.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="dataDefinition">The data definition.</param>
        protected abstract void InternalWriteDataContract(StringBuilder builder, ApiDataContractDefinition dataDefinition);
    }
}
