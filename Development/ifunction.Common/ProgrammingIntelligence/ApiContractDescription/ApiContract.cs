using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using ifunction;
using ifunction.RestApi;
using Newtonsoft.Json.Linq;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class ApiContract.
    /// </summary>
    public static class ApiContract
    {
        #region Private fields.

        /// <summary>
        /// The API contracts
        /// </summary>
        private static Dictionary<string, ApiContractDefinition> apiContracts = new Dictionary<string, ApiContractDefinition>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The API operations
        /// </summary>
        private static Dictionary<string, ApiOperationDefinition> apiOperations = new Dictionary<string, ApiOperationDefinition>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The API data contracts
        /// </summary>
        private static Dictionary<string, ApiDataContractDefinition> apiDataContracts = new Dictionary<string, ApiDataContractDefinition>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The locker for set
        /// </summary>
        private static object lockerForSet = new object();

        #endregion

        /// <summary>
        /// Initializes static members of the <see cref="ApiContract" /> class.
        /// </summary>
        static ApiContract()
        {
            foreach (var one in ReflectionExtension.GetAppDomainAssemblies())
            {
                if (!one.IsSystemAssembly())
                {
                    foreach (var type in one.GetTypes())
                    {
                        var apiContract = InitializeApiContract(type);

                        if (apiContract != null)
                        {
                            apiContracts.Add(type.GetFullName(), apiContract);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the API contract.
        /// </summary>
        /// <param name="uniqueName">Name of the unique.</param>
        /// <returns>IApiContractDescription.</returns>
        public static AbstractApiContractDescription GetApiContract(string uniqueName)
        {
            try
            {
                uniqueName.CheckEmptyString("uniqueName");

                if (apiContracts.ContainsKey(uniqueName))
                {
                    return apiContracts[uniqueName];
                }

                if (apiOperations.ContainsKey(uniqueName))
                {
                    return apiOperations[uniqueName];
                }

                if (apiDataContracts.ContainsKey(uniqueName))
                {
                    return apiDataContracts[uniqueName];
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex.Handle("GetApiContract", uniqueName);
            }
        }

        /// <summary>
        /// Parses to API contract definition.
        /// </summary>
        /// <param name="interfaceOrInstanceType">Type of the interface or instance.</param>
        /// <returns>Beyova.ProgrammingIntelligence.ApiContractDefinition.</returns>
        public static ApiContractDefinition ParseToApiContractDefinition(this Type interfaceOrInstanceType)
        {
            ApiContractDefinition contractDefinition = null;

            if (interfaceOrInstanceType != null)
            {
                var fullName = interfaceOrInstanceType.GetFullName();

                if (!apiContracts.ContainsKey(fullName))
                {
                    lock (lockerForSet)
                    {
                        if (!apiContracts.ContainsKey(fullName))
                        {
                            try
                            {
                                contractDefinition = InitializeApiContract(interfaceOrInstanceType);
                                apiContracts.Add(fullName, contractDefinition);
                            }
                            catch (Exception ex)
                            {
                                throw ex.Handle("ParseToApiContractDefinition", new { interfaceOrInstanceType = interfaceOrInstanceType?.FullName });
                            }
                        }
                    }
                }

                contractDefinition = apiContracts[fullName];
            }

            return contractDefinition;
        }

        /// <summary>
        /// Parses to API data contract definition.
        /// </summary>
        /// <param name="classOrStructType">Type of the interface or instance.</param>
        /// <returns>Beyova.ProgrammingIntelligence.ApiDataContractDefinition.</returns>
        public static ApiDataContractDefinition ParseToApiDataContractDefinition(this Type classOrStructType)
        {
            ApiDataContractDefinition contractDataDefinition = null;

            if (classOrStructType != null)
            {
                classOrStructType = classOrStructType.AdjustTypeToDefinitionStandard();

                var fullName = classOrStructType.GetFullName();
                if (!apiDataContracts.ContainsKey(fullName))
                {
                    lock (lockerForSet)
                    {
                        if (!apiDataContracts.ContainsKey(fullName))
                        {
                            try
                            {
                                contractDataDefinition = InitializeApiDataContractDefinition(classOrStructType);
                            }
                            catch (Exception ex)
                            {
                                throw ex.Handle("ParseToApiDataContractDefinition", new { interfaceOrInstanceType = classOrStructType?.FullName });
                            }
                        }
                    }
                }

                contractDataDefinition = apiDataContracts[fullName];
            }

            return contractDataDefinition;
        }

        #region InitializeApiContract

        /// <summary>
        /// Initializes the API contract.
        /// </summary>
        /// <param name="apiTypeOrInterfaceFullName">Full name of the API type or interface.</param>
        /// <returns>ApiContractDefinition.</returns>
        private static ApiContractDefinition InitializeApiContract(string apiTypeOrInterfaceFullName)
        {
            if (!string.IsNullOrWhiteSpace(apiTypeOrInterfaceFullName))
            {
                try
                {
                    var type = ReflectionExtension.SmartGetType(apiTypeOrInterfaceFullName);
                    return InitializeApiContract(type);
                }
                catch (Exception ex)
                {
                    throw ex.Handle("InitializeApiContract", apiTypeOrInterfaceFullName);
                }
            }

            return null;
        }

        /// <summary>
        /// Initializes the API contract.
        /// </summary>
        /// <param name="apiTypeOrInterface">The API type or interface.</param>
        /// <returns>Beyova.ProgrammingIntelligence.ApiContractDefinition.</returns>
        private static ApiContractDefinition InitializeApiContract(Type apiTypeOrInterface)
        {
            if (apiTypeOrInterface != null)
            {
                try
                {
                    apiTypeOrInterface.CheckNullObject("apiTypeOrInterface");

                    ApiContractAttribute apiContractAttribute = apiTypeOrInterface.GetCustomAttribute<ApiContractAttribute>(true);
                    if (apiContractAttribute != null)
                    {
                        var tokenRequiredAttribute = apiTypeOrInterface.GetCustomAttribute<TokenRequiredAttribute>(true);

                        var definition = new ApiContractDefinition
                        {
                            TokenRequired = tokenRequiredAttribute?.TokenRequired,
                            Version = apiContractAttribute.Version
                        };

                        definition.FillBasicTypeInfo(apiTypeOrInterface);

                        FillApiContract(definition, apiTypeOrInterface, apiContractAttribute);

                        return definition;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle("InitializeApiContract", apiTypeOrInterface?.FullName);
                }
            }

            return null;
        }

        /// <summary>
        /// Fills the API contract.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="interfaceOrInstanceType">Type of the interface or instance.</param>
        /// <param name="parentApiContractAttribute">The parent API contract attribute.</param>
        private static void FillApiContract(this ApiContractDefinition definition, Type interfaceOrInstanceType, ApiContractAttribute parentApiContractAttribute)
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
                            definition.ApiOperations.AddIfNotNull(InitializeApiOperationDefinition(method, definition));
                        }
                    }

                    var interfaces = interfaceOrInstanceType.GetInterfaces();
                    if (interfaces.HasItem())
                    {
                        foreach (var one in interfaces)
                        {
                            FillApiContract(definition, one, apiContract);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("FillApiContract", new { definition, interfaceOrInstanceType = interfaceOrInstanceType?.FullName });
            }
        }

        /// <summary>
        /// Initializes the API data contract definition.
        /// </summary>
        /// <param name="classOrStructType">Type of the class or structure.</param>
        /// <returns>Beyova.ProgrammingIntelligence.ApiDataContractDefinition.</returns>
        private static ApiDataContractDefinition InitializeApiDataContractDefinition(Type classOrStructType)
        {
            try
            {
                ApiDataContractDefinition result = null;

                classOrStructType = classOrStructType.AdjustTypeToDefinitionStandard();

                if (apiDataContracts.TryGetValue(classOrStructType.GetFullName(), out result))
                {
                    return result;
                }

                if (classOrStructType == null)
                {
                    return null;
                }

                if (classOrStructType.IsEnum)
                {
                    var enumDataContractDefinition = classOrStructType.CreateDataContract<EnumDataContractDefinition>();
                    enumDataContractDefinition.FillBasicTypeInfo(classOrStructType);

                    apiDataContracts.Add(enumDataContractDefinition.UniqueName, enumDataContractDefinition);

                    return enumDataContractDefinition;
                }
                else if (classOrStructType.IsSimpleType())
                {
                    ApiContractDataType dataType = ApiContractDataType.Undefined;

                    switch (classOrStructType.Name.ToLowerInvariant())
                    {
                        case "guid":
                            dataType = ApiContractDataType.Guid;
                            break;
                        case "timespan":
                            dataType = ApiContractDataType.TimeSpan;
                            break;
                        case "boolean":
                            dataType = ApiContractDataType.Boolean;
                            break;
                        case "byte":
                        case "sbyte":
                            dataType = ApiContractDataType.Binary;
                            break;
                        case "string":
                        case "char":
                            dataType = ApiContractDataType.String;
                            break;
                        case "uint16":
                        case "int16":
                        case "uint32":
                        case "int32":
                        case "uint64":
                        case "int64":
                            dataType = ApiContractDataType.Integer;
                            break;
                        case "single":
                        case "double":
                            dataType = ApiContractDataType.Float;
                            break;
                        case "decimal":
                            dataType = ApiContractDataType.Decimal;
                            break;
                        case "datetime":
                            dataType = ApiContractDataType.DateTime;
                            break;
                        default:
                            break;
                    }

                    var simpleValueTypeDataContractDefinition = new SimpleValueTypeDataContractDefinition(dataType);
                    simpleValueTypeDataContractDefinition.FillBasicTypeInfo(classOrStructType);
                    simpleValueTypeDataContractDefinition.UniqueName = classOrStructType.GetFullName();
                    apiDataContracts.Add(simpleValueTypeDataContractDefinition.UniqueName, simpleValueTypeDataContractDefinition);

                    return simpleValueTypeDataContractDefinition;
                }
                else if (classOrStructType.IsCollection())
                {
                    var arrayContract = classOrStructType.CreateDataContract<ArrayListDataContractDefinition>();
                    apiDataContracts.Add(arrayContract.UniqueName, arrayContract);

                    var valueType = classOrStructType.IsGenericType ? classOrStructType.GetGenericArguments().FirstOrDefault() : typeof(DynamicObject);

                    arrayContract.ValueType = valueType.ParseToApiDataContractDefinition().AsReference();

                    result = arrayContract;
                }
                else if (classOrStructType.IsDictionary())
                {
                    var dictionaryContract = classOrStructType.CreateDataContract<DictionaryDataContractDefinition>();
                    apiDataContracts.Add(dictionaryContract.UniqueName, dictionaryContract);

                    var genericTypes = classOrStructType.GetGenericArguments();

                    dictionaryContract.KeyType = genericTypes[0].ParseToApiDataContractDefinition().AsReference();
                    dictionaryContract.ValueType = genericTypes[1].ParseToApiDataContractDefinition().AsReference();

                    result = dictionaryContract;
                }
                else if (classOrStructType.InheritsFrom(typeof(JToken)) || typeof(DynamicObject) == classOrStructType)
                {
                    var dynamicConract = classOrStructType.CreateDataContract<DynamicObjectDataContractDefinition>();
                    dynamicConract.FillBasicTypeInfo(classOrStructType);

                    apiDataContracts.Add(dynamicConract.UniqueName, dynamicConract);
                    result = dynamicConract;
                }
                else
                {
                    var complexObjectContract = classOrStructType.CreateDataContract<ComplexObjectDataContractDefinition>();
                    complexObjectContract.FillBasicTypeInfo(classOrStructType);

                    apiDataContracts.Add(classOrStructType.GetFullName(), complexObjectContract);

                    foreach (var field in classOrStructType.GetActualAffectedFields())
                    {
                        complexObjectContract.Children.Add(field.Name, field.FieldType.ParseToApiDataContractDefinition().AsReference());
                    }

                    foreach (var property in classOrStructType.GetActualAffectedProperties())
                    {
                        complexObjectContract.Children.Add(property.Name, property.PropertyType.ParseToApiDataContractDefinition().AsReference());
                    }

                    result = complexObjectContract;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle("InitializeApiDataContractDefinition", new
                {
                    type = classOrStructType?.FullName
                });
            }
        }

        /// <summary>
        /// Creates the data contract.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <returns>T.</returns>
        private static T CreateDataContract<T>(this Type type) where T : ApiDataContractDefinition, new()
        {
            if (type != null)
            {
                var result = new T();
                result.Name = type.Name;
                result.Namespace = type.Namespace;
                result.UniqueName = type.GetFullName();

                return result;
            }

            return null;
        }

        #endregion

        #region ApiContractDefinition

        /// <summary>
        /// Gets the API contract definition.
        /// </summary>
        /// <param name="contractFullName">Full name of the contract.</param>
        /// <returns>ApiContractDefinition.</returns>
        public static ApiContractDefinition GetApiContractDefinition(string contractFullName)
        {
            ApiContractDefinition contractDefinition = null;

            if (!string.IsNullOrWhiteSpace(contractFullName))
            {
                lock (lockerForSet)
                {
                    if (!apiContracts.ContainsKey(contractFullName))
                    {
                        try
                        {
                            var type = ReflectionExtension.SmartGetType(contractFullName);
                            if (type != null)
                            {
                                contractDefinition = InitializeApiContract(type);
                            }

                            apiContracts.Add(contractFullName, contractDefinition);
                        }
                        catch (Exception ex)
                        {
                            throw ex.Handle("GetApiContractDefinitionByName", contractFullName);
                        }
                    }
                }
            }

            return contractDefinition;
        }

        /// <summary>
        /// Gets the name of the API contract unique.
        /// </summary>
        /// <param name="apiTypeOrInterface">The API type or interface.</param>
        /// <returns>System.String.</returns>
        internal static string GetApiContractUniqueName(this Type apiTypeOrInterface)
        {
            return apiTypeOrInterface.GetFullName();
        }

        #endregion

        #region ApiOperationDefinition

        /// <summary>
        /// Gets the API operation definition.
        /// </summary>
        /// <param name="methodFullName">Full name of the method.</param>
        /// <returns>ApiOperationDefinition.</returns>
        public static ApiOperationDefinition GetApiOperationDefinition(string methodFullName)
        {
            ApiOperationDefinition operationDefinition = null;

            if (!string.IsNullOrWhiteSpace(methodFullName))
            {
                lock (lockerForSet)
                {
                    if (!apiOperations.ContainsKey(methodFullName))
                    {
                        try
                        {
                            var classFullName = methodFullName.SubStringBeforeLastMatch(".");

                            InitializeApiContract(classFullName);
                            return GetApiOperationDefinition(methodFullName);
                        }
                        catch (Exception ex)
                        {
                            throw ex.Handle("GetApiOperationDefinition", methodFullName);
                        }
                    }
                }
            }

            return operationDefinition;
        }

        /// <summary>
        /// Initializes the API operation definition.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="contractDefinition">The contract definition.</param>
        /// <returns>Beyova.ProgrammingIntelligence.ApiOperationDefinition.</returns>
        private static ApiOperationDefinition InitializeApiOperationDefinition(MethodInfo methodInfo, ApiContractDefinition contractDefinition)
        {
            try
            {
                methodInfo.CheckNullObject("methodInfo");
                contractDefinition.CheckNullObject("contractDefinition");

                if (apiOperations.ContainsKey(methodInfo.GetFullName()))
                {
                    return apiOperations[methodInfo.GetFullName()];
                }

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

                    var obsoleteAttribute = methodInfo.GetCustomAttribute<ObsoleteAttribute>(true);
                    if (obsoleteAttribute != null)
                    {
                        result.IsObsoleted = true;
                        result.ObsoleteDescription = obsoleteAttribute.Message;
                    }

                    if (obsoluteAttribute != null)
                    {
                        result.IsObsoleted = true;
                        result.ObsoleteDescription = obsoluteAttribute.Message;
                    }

                    if (methodInfo.ReturnType != typeof(void))
                    {
                        result.ReturnValue = methodInfo.ReturnType.ParseToApiDataContractDefinition().AsReference();
                    }

                    foreach (var one in methodInfo.GetParameters())
                    {
                        result.Parameters.Add(one.Name, one.ParameterType.ParseToApiDataContractDefinition().AsReference());
                    }

                    apiOperations.Add(methodInfo.GetFullName(), result);
                    return result;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex.Handle("InitializeApiOperationDefinition", new
                {
                    method = methodInfo?.GetFullName(),
                    contractDefinition
                });
            }
        }

        #endregion

        #region ApiDataContractDefinition

        /// <summary>
        /// Gets the API data contract definition.
        /// </summary>
        /// <param name="dataContractFullName">Full name of the data contract.</param>
        /// <returns>Beyova.ProgrammingIntelligence.ApiDataContractDefinition.</returns>
        public static ApiDataContractDefinition GetApiDataContractDefinition(string dataContractFullName)
        {
            ApiDataContractDefinition contractDefinition = null;

            if (!string.IsNullOrWhiteSpace(dataContractFullName))
            {
                if (!apiContracts.ContainsKey(dataContractFullName))
                {
                    lock (lockerForSet)
                    {
                        if (!apiContracts.ContainsKey(dataContractFullName))
                        {
                            try
                            {
                                var type = ReflectionExtension.SmartGetType(dataContractFullName);

                                if (type != null)
                                {
                                    contractDefinition = InitializeApiDataContractDefinition(type);
                                }

                                return contractDefinition;
                            }
                            catch (Exception ex)
                            {
                                throw ex.Handle("GetApiDataContractDefinition", dataContractFullName);
                            }
                        }
                    }
                }
            }

            return contractDefinition;
        }

        /// <summary>
        /// Determines whether [has API data contract definition] [the specified API data model].
        /// </summary>
        /// <param name="apiDataModel">The API data model.</param>
        /// <returns><c>true</c> if [has API data contract definition] [the specified API data model]; otherwise, <c>false</c>.</returns>
        internal static bool HasApiDataContractDefinition(this Type apiDataModel)
        {
            return apiDataContracts != null && apiDataContracts.ContainsKey(apiDataModel.GetApiContractUniqueName().SafeToString());
        }

        #endregion

        #region Util

        /// <summary>
        /// Fills the basic type information.
        /// </summary>
        /// <param name="apiContractDescription">The API contract description.</param>
        /// <param name="type">The type.</param>
        private static void FillBasicTypeInfo(this AbstractApiContractDescription apiContractDescription, Type type)
        {
            if (apiContractDescription != null && type != null)
            {
                apiContractDescription.Namespace = type.Namespace;
                apiContractDescription.Name = type.Name;
                var obsoleteAttribute = type.GetCustomAttribute<ObsoleteAttribute>(true);

                if (obsoleteAttribute != null)
                {
                    apiContractDescription.IsObsoleted = true;
                    apiContractDescription.ObsoleteDescription = obsoleteAttribute.Message;
                }
            }
        }

        /// <summary>
        /// Ases the reference.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <returns>Beyova.ProgrammingIntelligence.ApiContractReference.</returns>
        internal static ApiContractReference AsReference(this ApiDataContractDefinition definition)
        {
            return definition == null ? null : new ApiContractReference { ReferenceName = definition.UniqueName };
        }

        #endregion
    }
}
