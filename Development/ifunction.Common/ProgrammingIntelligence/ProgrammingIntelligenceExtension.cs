using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using ifunction;
using ifunction.RestApi;
using Newtonsoft.Json.Linq;

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

        /// <summary>
        /// Adjusts the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Type.</returns>
        internal static Type AdjustTypeToDefinitionStandard(this Type type)
        {
            if (type != null)
            {
                if (type.IsNullable())
                {
                    return AdjustTypeToDefinitionStandard(type.GetNullableType());
                }
                else if (type.IsCollection())
                {
                    var genericTypes = type.GetGenericArguments();
                    var newType = typeof(List<>);
                    return newType.MakeGenericType(genericTypes);
                }
                else if (type.IsDictionary())
                {
                    var genericTypes = type.GetGenericArguments();
                    var newType = typeof(Dictionary<,>);
                    return newType.MakeGenericType(genericTypes);
                }
                else if (type.InheritsFrom(typeof(JToken)))
                {
                    return typeof(DynamicObject);
                }
                else
                {
                    switch (type.Name.ToLowerInvariant())
                    {
                        case "byte":
                        case "sbyte":
                            return typeof(byte);
                        case "string":
                        case "char":
                            return typeof(string);
                        case "uint16":
                        case "int16":
                        case "uint32":
                        case "int32":
                        case "uint64":
                        case "int64":
                            return typeof(Int32);
                        case "single":
                        case "double":
                            return typeof(float);
                    }
                }
            }

            return type;
        }

        /// <summary>
        /// Displays the name of as reference.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if display as reference name, <c>false</c> otherwise.</returns>
        internal static bool DisplayAsReferenceName(this ApiContractDataType type)
        {
            return type == ApiContractDataType.Dictionary || type == ApiContractDataType.Array || type == ApiContractDataType.ComplexObject || type == ApiContractDataType.Enum;
        }
    }
}
