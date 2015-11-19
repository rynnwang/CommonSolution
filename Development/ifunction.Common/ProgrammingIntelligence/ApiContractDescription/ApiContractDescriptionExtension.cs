using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ifunction;
using ifunction.RestApi;
using Newtonsoft.Json.Linq;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class ApiContractDescriptionExtension.
    /// </summary>
    internal static class ApiContractDescriptionExtension
    {
        /// <summary>
        /// Parses the API contract definition.
        /// </summary>
        /// <param name="interfaceOrInstanceType">Type of the interface or instance.</param>
        /// <returns>Beyova.ProgrammingIntelligence.ApiContractDefinition.</returns>
        public static ApiContractDefinition ParseApiContractDefinition(this Type interfaceOrInstanceType)
        {
            try
            {
                interfaceOrInstanceType.CheckNullObject("interfaceOrInstanceType");

                HashSet<string> doneInterfaceMethods = new HashSet<string>();

                ApiContractAttribute apiContractAttribute = interfaceOrInstanceType.GetCustomAttribute<ApiContractAttribute>(true);
                if (apiContractAttribute != null)
                {
                    var tokenRequiredAttribute = interfaceOrInstanceType.GetCustomAttribute<TokenRequiredAttribute>(true);

                    var definition = new ApiContractDefinition
                    {
                        Name = interfaceOrInstanceType.Name,
                        Namespace = interfaceOrInstanceType.Namespace,
                        TokenRequired = tokenRequiredAttribute?.TokenRequired,
                        Version = apiContractAttribute.Version
                    };

                    FillApiContract(definition, doneInterfaceMethods, interfaceOrInstanceType, apiContractAttribute);

                    return definition;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("ParseApiContractDefinition", interfaceOrInstanceType);
            }
        }

        ///// <summary>
        ///// Parses the API model.
        ///// </summary>
        ///// <param name="classOrStructType">Type of the class or structure.</param>
        ///// <returns>ApiModelDefinition.</returns>
        //public static ApiDataContractDefinition ParseApiDataContract(this Type classOrStructType)
        //{
        //    try
        //    {
        //        classOrStructType.CheckNullObject("classOrStructType");

        //        if (classOrStructType.IsNullable())
        //        {
        //            var result = ParseApiDataContract(classOrStructType.GetNullableType());

        //            result.IsNullable = true;
        //            return result;
        //        }
        //        else if (classOrStructType.IsSimpleType())
        //        {
        //            var result = classOrStructType.CreateDataContract<SimpleValueTypeDataContractDefinition>();

        //            switch (classOrStructType.Name.ToLowerInvariant())
        //            {
        //                case "guid":
        //                    result.TypeCode = TypeCode.Object;
        //                    result.JsonType = JTokenType.Guid;
        //                    break;
        //                case "timespan":
        //                    result.TypeCode = TypeCode.Object;
        //                    result.JsonType = JTokenType.TimeSpan;
        //                    break;
        //                case "boolean":
        //                    result.TypeCode = TypeCode.Boolean;
        //                    result.JsonType = JTokenType.Boolean;
        //                    break;
        //                case "byte":
        //                case "sbyte":
        //                    result.TypeCode = classOrStructType.Name.ParseToEnum<TypeCode>(TypeCode.Empty);
        //                    result.JsonType = JTokenType.Bytes;
        //                    break;
        //                case "string":
        //                case "char":
        //                    result.TypeCode = classOrStructType.Name.ParseToEnum<TypeCode>(TypeCode.Empty);
        //                    result.JsonType = JTokenType.String;
        //                    break;
        //                case "uint16":
        //                case "int16":
        //                case "uint32":
        //                case "int32":
        //                case "uint64":
        //                case "int64":
        //                    result.TypeCode = classOrStructType.Name.ParseToEnum<TypeCode>(TypeCode.Empty);
        //                    result.JsonType = JTokenType.Integer;
        //                    break;
        //                case "single":
        //                case "double":
        //                case "decimal":
        //                    result.TypeCode = classOrStructType.Name.ParseToEnum<TypeCode>(TypeCode.Empty);
        //                    result.JsonType = JTokenType.Float;
        //                    break;
        //                case "datetime":
        //                    result.TypeCode = TypeCode.DateTime;
        //                    result.JsonType = JTokenType.Date;
        //                    break;
        //                default:
        //                    result.TypeCode = classOrStructType.Name.ParseToEnum<TypeCode>(TypeCode.Empty);
        //                    break;
        //            }

        //            return result;
        //        }
        //        else if (classOrStructType.IsCollection())
        //        {
        //            var result = classOrStructType.CreateDataContract<ArrayListDataContractDefinition>();
        //            result.JsonType = JTokenType.Array;
        //            result.ValueType = classOrStructType.GetGenericArguments().FirstOrDefault()?.ParseApiDataContract();

        //            return result;
        //        }
        //        else if (classOrStructType.IsDictionary())
        //        {
        //            var result = classOrStructType.CreateDataContract<DictionaryDataContractDefinition>();
        //            result.JsonType = JTokenType.Object;

        //            var genericTypes = classOrStructType.GetGenericArguments();

        //            result.KeyType = genericTypes[0].ParseApiDataContract();
        //            result.ValueType = genericTypes[1].ParseApiDataContract();

        //            return result;
        //        }
        //        else
        //        {
        //            var result = classOrStructType.CreateDataContract<ObjectDataContractDefinition>();
        //            result.JsonType = JTokenType.Object;

        //            foreach (var field in classOrStructType.GetActualAffectedFields())
        //            {
        //                result.Children.Add(field.Name, field.FieldType.ParseApiDataContract());
        //            }

        //            foreach (var property in classOrStructType.GetActualAffectedProperties())
        //            {
        //                result.Children.Add(property.Name, property.PropertyType.ParseApiDataContract());
        //            }

        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.Handle("ParseApiDataContract", classOrStructType);
        //    }
        //}

        /// <summary>
        /// Creates the data contract.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>T.</returns>
        private static T CreateDataContract<T>(this Type type, ApiContractDataType dataType) where T : ApiDataContractDefinition, new()
        {
            if (type != null)
            {
                var result = new T();
                result.Name = type.Name;
                result.Namespace = type.Namespace;

                return result;
            }

            return null;
        }

        /// <summary>
        /// Fills the API interface.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="doneInterfaceMethods">The done interface methods.</param>
        /// <param name="interfaceOrInstanceType">Type of the interface or instance.</param>
        /// <param name="parentApiContractAttribute">The parent API class attribute.</param>
        private static void FillApiContract(this ApiContractDefinition definition, HashSet<string> doneInterfaceMethods, Type interfaceOrInstanceType, ApiContractAttribute parentApiContractAttribute)
        {
            try
            {
                interfaceOrInstanceType.CheckNullObject("interfaceOrInstanceType");

                var apiContract = interfaceOrInstanceType.GetCustomAttribute<ApiContractAttribute>(true) ?? parentApiContractAttribute;

                if (apiContract != null)
                {
                    foreach (var method in interfaceOrInstanceType.GetMethods())
                    {
                        var apiOperationAttribute = method.GetCustomAttribute<ApiOperationAttribute>(true);

                        if (apiOperationAttribute != null)
                        {
                            definition.ApiOperations.AddIfNotNull(ToApiOperationDefinition(method, doneInterfaceMethods, definition));
                        }
                    }

                    var interfaces = interfaceOrInstanceType.GetInterfaces();
                    if (interfaces.HasItem())
                    {
                        foreach (var one in interfaces)
                        {
                            FillApiContract(definition, doneInterfaceMethods, one, apiContract);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("FillApiContract", new { definition, doneInterfaceMethods, interfaceOrInstanceType = interfaceOrInstanceType?.FullName });
            }
        }

        /// <summary>
        /// To the API operation definition.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="doneInterfaceMethods">The done interface methods.</param>
        /// <param name="contractDefinition">The contract definition.</param>
        /// <returns>Beyova.ProgrammingIntelligence.ApiOperationDefinition.</returns>
        private static ApiOperationDefinition ToApiOperationDefinition(this MethodInfo methodInfo, HashSet<string> doneInterfaceMethods, ApiContractDefinition contractDefinition)
        {
            try
            {
                methodInfo.CheckNullObject("methodInfo");
                contractDefinition.CheckNullObject("contractDefinition");

                var apiOperationAttribute = methodInfo.GetCustomAttribute<ApiOperationAttribute>(true);

                if (apiOperationAttribute != null)
                {
                    var tokenRequired = methodInfo.GetCustomAttribute<TokenRequiredAttribute>(true)?.TokenRequired;
                    var descriptions = methodInfo.GetCustomAttributes<ApiDescriptionAttribute>(true);
                    var obsoluteAttribute = methodInfo.GetCustomAttribute<ObsoleteAttribute>(true);

                    var result = new ApiOperationDefinition
                    {
                        TokenRequired = tokenRequired ?? contractDefinition.TokenRequired ?? false,
                        Description = (from one in descriptions where !string.IsNullOrWhiteSpace(one.Description) select one.Description).ToList(),
                        Namespace = contractDefinition.Namespace,
                        Name = methodInfo.Name
                    };

                    if (obsoluteAttribute != null)
                    {
                        result.IsObsoleted = true;
                        result.ObsoleteDescription = obsoluteAttribute.Message;
                    }

                    if (methodInfo.ReturnType != typeof(void))
                    {
                        //result.ReturnValue = methodInfo.ReturnType.ParseApiDataContract();
                    }

                    foreach (var one in methodInfo.GetParameters())
                    {
                        //result.Parameters.Add(one.Name, one.ParameterType.ParseApiDataContract());
                    }

                    return result;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex.Handle("ToApiOperationDefinition", new
                {
                    methodInfo = methodInfo?.GetFullName(),
                    doneInterfaceMethods,
                    contractDefinition
                });
            }
        }


    }
}
