using System;
using System.Reflection;
using ifunction;
using ifunction.RestApi;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// </summary>
    public static class ProgrammingIntelligenceExtension
    {
        /// <summary>
        /// To the string identity.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>System.String.</returns>
        public static string ToStringIdentity(this IApiMethod method)
        {
            return method == null ? string.Empty : string.Format("/{0}/{1}/{2}/{3}/", method.HttpMethod, method.Version, method.ResourceName, method.Action);
        }

        /// <summary>
        /// To the type.
        /// </summary>
        /// <param name="dataContractDefinition">The data contract definition.</param>
        /// <returns>System.Type.</returns>
        public static Type ToType(this ApiDataContractDefinition dataContractDefinition)
        {
            return dataContractDefinition != null ? ReflectionExtension.SmartGetType(dataContractDefinition.ToString()) : null;
        }
    }
}
