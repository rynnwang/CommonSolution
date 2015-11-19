using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ifunction;
using ifunction.RestApi;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class AbstractRestApiGenerator.
    /// </summary>
    public abstract class AbstractRestApiGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractRestApiGenerator"/> class.
        /// </summary>
        public AbstractRestApiGenerator()
        {
        }

        /// <summary>
        /// Generates the code.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="doneApi">The done API.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="lastApiContractAttribute">The last API class attribute.</param>
        protected virtual void GenerateInterfacePart(StringBuilder builder, HashSet<string> doneApi, Type interfaceType, ApiContractAttribute lastApiContractAttribute = null)
        {
            if (builder != null && doneApi != null && interfaceType != null)
            {
                var apiClass = interfaceType.GetCustomAttribute<ApiContractAttribute>(true) ?? lastApiContractAttribute;

                if (apiClass != null)
                {
                    foreach (var method in interfaceType.GetMethods())
                    {
                        var apiOperationAttribute = method.GetCustomAttribute<ApiOperationAttribute>(true);

                        if (apiOperationAttribute != null)
                        {
                            GenerateMethodPart(builder, doneApi, apiClass.Version, apiOperationAttribute, method);
                        }
                    }

                    var interfaces = interfaceType.GetInterfaces();
                    if (interfaces.HasItem())
                    {
                        foreach (var one in interfaceType.GetInterfaces())
                        {
                            GenerateInterfacePart(builder, doneApi, one, apiClass);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generates the method part.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="doneApi">The done API.</param>
        /// <param name="version">The version.</param>
        /// <param name="apiOperationAttribute">The API operation attribute.</param>
        /// <param name="methodInfo">The method information.</param>
        protected abstract void GenerateMethodPart(StringBuilder builder, HashSet<string> doneApi, string version, ApiOperationAttribute apiOperationAttribute, MethodInfo methodInfo);
    }
}
