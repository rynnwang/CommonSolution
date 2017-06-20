using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Beyova.ExceptionSystem;
using Beyova.RestApi;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class RestApiClientGenerator.
    /// </summary>
    public class RestApiClientGenerator : CSharpCodeGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiClientGenerator" /> class.
        /// </summary>
        /// <param name="namespace">The namespace.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="codeIndent">The code indent.</param>
        public RestApiClientGenerator(string @namespace, string className, string codeIndent = null)
            : base(@namespace, className, typeof(RestApiClient), codeIndent)
        {
        }

        #region Public methods

        /// <summary>
        /// Generates the type of the code by.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <returns>System.String.</returns>
        public string GenerateCodeByType(params Type[] types)
        {
            var builder = new StringBuilder();

            if (types.HasItem())
            {
                GenerateCodeByTypes(builder, types);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Generates the code.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>System.String.</returns>
        public string GenerateCode<T>()
        {
            var builder = new StringBuilder();

            GenerateCodeByTypes(builder, typeof(T));

            return builder.ToString();
        }

        /// <summary>
        /// Generates the type of the code to path by.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="types">The types.</param>
        public void GenerateCodeToPathByType(string path, params Type[] types)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                File.WriteAllText(path, GenerateCodeByType(types), Encoding.UTF8);
            }
        }

        /// <summary>
        /// Generates the code to path.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The path.</param>
        public void GenerateCodeToPath<T>(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                File.WriteAllText(path, GenerateCode<T>(), Encoding.UTF8);
            }
        }

        #endregion Public methods

        /// <summary>
        /// Generates the code by type. Type here can be both Interface and Class. Method would recursively find all interfaces.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="types">The types.</param>
        protected void GenerateCodeByTypes(StringBuilder builder, params Type[] types)
        {
            if (builder != null)
            {
                // write interface based members into temp builder.
                // because class needs to inherit from interfaces, which came from each GenerateInterfacePart call.
                var doneApiHash = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
                HashSet<Type> handledInterfaces = new HashSet<Type>();
                StringBuilder methodBuilder = new StringBuilder();

                if (types.HasItem())
                {
                    foreach (var type in types)
                    {
                        if (type.IsInterface)
                        {
                            if (!IsDuplicatedInterface(handledInterfaces, type))
                            {
                                GenerateInterfacePart(methodBuilder, doneApiHash, type);
                            }
                        }
                        foreach (var item in type.GetInterfaces())
                        {
                            if (!IsDuplicatedInterface(handledInterfaces, item))
                            {
                                GenerateInterfacePart(methodBuilder, doneApiHash, item);
                            }
                        }
                    }
                }

                // Start write string into finalized string builder.

                WriteFileInfo(builder);
                WriteNamespaces(builder);
                builder.AppendLine();

                // write namespace
                builder.AppendLineWithFormat("namespace {0}", Namespace);
                builder.AppendLine("{");

                // write class declaration
                builder.AppendIndent(CodeIndent, 1);

                builder.Append(GenerateClassDeclarationPart(ClassName, handledInterfaces));

                builder.AppendIndent(CodeIndent, 1);
                builder.AppendLine("{");

                // write constructor
                WriteConstructor(builder, ClassName);

                builder.Append(methodBuilder.ToString());

                // End of class
                builder.AppendIndent(CodeIndent, 1);
                builder.AppendLine("}");

                // End of namespace
                builder.AppendLine("}");

                builder.AppendLine();
            }
        }

        /// <summary>
        /// Generates the code.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="doneApi">The done API.</param>
        /// <param name="apiContractAttribute">The API contract attribute.</param>
        /// <param name="apiOperationAttribute">The API URI.</param>
        /// <param name="methodInfo">The method information.</param>
        /// <exception cref="InvalidObjectException">methodInfo.Parameter
        /// or
        /// methodInfo.Parameter</exception>
        protected void GenerateMethodPart(StringBuilder builder, HashSet<string> doneApi, ApiContractAttribute apiContractAttribute, ApiOperationAttribute apiOperationAttribute, MethodInfo methodInfo)
        {
            //// No need to check whether has ApiContract any more. Because:1. Inherited class can add it, 2. Version is defined by constructor parameter, it is nothing about how is defined in interface.
            if (builder != null && doneApi != null && methodInfo != null && apiOperationAttribute != null)
            {
                var routeKey = string.Format("{0}/{1}/{2}", apiOperationAttribute.ResourceName, apiOperationAttribute.HttpMethod, apiOperationAttribute.Action);

                if (!doneApi.Contains(routeKey))
                {
                    //Declaration
                    builder.AppendIndent(CodeIndent, 2);
                    builder.AppendLineWithFormat("public virtual {0}", methodInfo.ToDeclarationCodeLook());

                    builder.AppendIndent(CodeIndent, 2);
                    builder.AppendLine("{");

                    //Invoke body
                    builder.AppendIndent(CodeIndent, 3);
                    builder.AppendLine("try");
                    builder.AppendIndent(CodeIndent, 3);
                    builder.AppendLine("{");

                    //Analyze method info and call different invoke directly
                    var parameters = methodInfo.GetParameters();
                    var firstParameter = parameters.FirstOrDefault();
                    var returnVoid = methodInfo.ReturnType.IsVoid() ?? false;
                    var allowBody = apiOperationAttribute.HttpMethod.IsInString(new Collection<string>(new[] { HttpConstants.HttpMethod.Post, HttpConstants.HttpMethod.Put }), true);

                    if (firstParameter != null)
                    {
                        if (parameters.Count() > 1)
                        {
                            #region Parameter Count > 1

                            if (allowBody)
                            {
                                builder.AppendIndent(CodeIndent, 4);
                                builder.AppendLine("var parameters = new Dictionary<string, object>();");

                                foreach (var one in parameters)
                                {
                                    builder.AppendIndent(CodeIndent, 4);
                                    builder.AppendLineWithFormat("parameters.Add(\"{0}\", {0});", one.Name);
                                }

                                ////To Call: JToken InvokeUsingBody(string httpMethod, string resourceName, string resourceAction, object parameter)
                                builder.AppendIndent(CodeIndent, 4);
                                if (returnVoid)
                                {
                                    builder.AppendFormat("this.InvokeUsingBody({0}, \"{1}\", \"{2}\", \"{3}\", {4}, parameters);",
                                        string.IsNullOrWhiteSpace(apiContractAttribute.Realm) ? nullString : apiContractAttribute.Realm.AsQuotedString(),
                                        apiContractAttribute.Version,
                                        apiOperationAttribute.HttpMethod,
                                        apiOperationAttribute.ResourceName,
                                        apiOperationAttribute.Action.AsQuotedString().SafeToString(nullString));
                                }
                                else
                                {
                                    builder.AppendFormat("return this.InvokeUsingBody({0}, \"{1}\", \"{2}\", \"{3}\", {4}, parameters).SafeToObject<{5}>();",
                                        string.IsNullOrWhiteSpace(apiContractAttribute.Realm) ? nullString : apiContractAttribute.Realm.AsQuotedString(),
                                        apiContractAttribute.Version,
                                        apiOperationAttribute.HttpMethod,
                                        apiOperationAttribute.ResourceName,
                                        apiOperationAttribute.Action.AsQuotedString().SafeToString(nullString),
                                        methodInfo.ReturnType.ToCodeLook()
                                    );
                                }
                                builder.AppendLine();
                            }
                            else
                            {
                                builder.AppendIndent(CodeIndent, 4);
                                builder.AppendLine("var parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);");

                                foreach (var one in parameters)
                                {
                                    if (!one.ParameterType.IsSimpleType())
                                    {
                                        throw ExceptionFactory.CreateInvalidObjectException("methodInfo.Parameter", data: one.ParameterType.ToCodeLook(), reason: "Multiple complex objects are used in a un-body REST API method.");
                                    }

                                    builder.AppendIndent(CodeIndent, 4);
                                    builder.AppendLineWithFormat("parameters.Merge(\"{0}\",{0}.SafeToString());", one.Name, SimpleVariableToStringCode(one.Name, one.ParameterType));
                                }

                                ////To Call: JToken InvokeUsingCombinedQueryString(string httpMethod, string resourceName, string resourceAction, Dictionary<string, string> parameters)
                                builder.AppendIndent(CodeIndent, 4);
                                if (returnVoid)
                                {
                                    builder.AppendFormat("this.InvokeUsingCombinedQueryString({0}, \"{1}\", \"{2}\", \"{3}\", {4}, parameters);",
                                        string.IsNullOrWhiteSpace(apiContractAttribute.Realm) ? nullString : apiContractAttribute.Realm.AsQuotedString(),
                                        apiContractAttribute.Version,
                                        apiOperationAttribute.HttpMethod,
                                        apiOperationAttribute.ResourceName,
                                        apiOperationAttribute.Action.AsQuotedString().SafeToString(nullString));
                                }
                                else
                                {
                                    builder.AppendFormat("return this.InvokeUsingCombinedQueryString({0},\"{1}\", \"{2}\",\"{3}\",{4}, parameters).SafeToObject<{5}>();",
                                        string.IsNullOrWhiteSpace(apiContractAttribute.Realm) ? nullString : apiContractAttribute.Realm.AsQuotedString(),
                                        apiContractAttribute.Version,
                                        apiOperationAttribute.HttpMethod,
                                        apiOperationAttribute.ResourceName,
                                        apiOperationAttribute.Action.AsQuotedString().SafeToString(nullString),
                                        methodInfo.ReturnType.ToCodeLook());
                                }
                                builder.AppendLine();
                            }

                            #endregion Parameter Count > 1
                        }
                        else
                        {
                            #region Parameter Count = 1

                            if (firstParameter.ParameterType.IsSimpleType())
                            {
                                ////To Call: JToken InvokeUsingQueryString(string httpMethod, string resourceName, string resourceAction, string parameter = null)
                                builder.AppendIndent(CodeIndent, 4);
                                if (returnVoid)
                                {
                                    builder.AppendFormat("this.InvokeUsingQueryString({0}, \"{1}\", \"{2}\", \"{3}\", {4}, {5});",
                                        string.IsNullOrWhiteSpace(apiContractAttribute.Realm) ? nullString : apiContractAttribute.Realm.AsQuotedString(),
                                        apiContractAttribute.Version,
                                        apiOperationAttribute.HttpMethod,
                                        apiOperationAttribute.ResourceName,
                                        apiOperationAttribute.Action.AsQuotedString().SafeToString(nullString),
                                        SimpleVariableToStringCode(firstParameter.Name, firstParameter.ParameterType)
                                    );
                                }
                                else
                                {
                                    builder.AppendFormat("return this.InvokeUsingQueryString({0}, \"{1}\", \"{2}\", \"{3}\", {4}, {5}).SafeToObject<{6}>();",
                                        string.IsNullOrWhiteSpace(apiContractAttribute.Realm) ? nullString : apiContractAttribute.Realm.AsQuotedString(),
                                        apiContractAttribute.Version,
                                        apiOperationAttribute.HttpMethod,
                                        apiOperationAttribute.ResourceName,
                                        apiOperationAttribute.Action.AsQuotedString().SafeToString(nullString),
                                        SimpleVariableToStringCode(firstParameter.Name, firstParameter.ParameterType),
                                        methodInfo.ReturnType.ToCodeLook());
                                }
                                builder.AppendLine();
                            }
                            else if (!allowBody)
                            {
                                throw ExceptionFactory.CreateInvalidObjectException("methodInfo.Parameter", data: firstParameter.ParameterType.ToCodeLook(), reason: "Complex object is used in a un-body REST API method.");
                            }
                            else
                            {
                                //// To Call: JToken InvokeUsingBody(string httpMethod, string resourceName, string resourceAction, object parameter)
                                builder.AppendIndent(CodeIndent, 4);
                                if (returnVoid)
                                {
                                    builder.AppendFormat("this.InvokeUsingBody({0}, \"{1}\", \"{2}\", \"{3}\", {4}, {5});",
                                        string.IsNullOrWhiteSpace(apiContractAttribute.Realm) ? nullString : apiContractAttribute.Realm.AsQuotedString(),
                                        apiContractAttribute.Version,
                                        apiOperationAttribute.HttpMethod,
                                        apiOperationAttribute.ResourceName,
                                        apiOperationAttribute.Action.AsQuotedString().SafeToString(nullString),
                                        firstParameter.Name);
                                }
                                else
                                {
                                    builder.AppendFormat("return this.InvokeUsingBody({0}, \"{1}\", \"{2}\", \"{3}\", {4}, {5}).SafeToObject<{6}>();",
                                        string.IsNullOrWhiteSpace(apiContractAttribute.Realm) ? nullString : apiContractAttribute.Realm.AsQuotedString(),
                                        apiContractAttribute.Version,
                                        apiOperationAttribute.HttpMethod,
                                        apiOperationAttribute.ResourceName,
                                        apiOperationAttribute.Action.AsQuotedString().SafeToString(nullString),
                                        firstParameter.Name,
                                        methodInfo.ReturnType.ToCodeLook());
                                }

                                builder.AppendLine();
                            }

                            #endregion Parameter Count = 1
                        }
                    }
                    else
                    {
                        //// To Call: JToken InvokeUsingQueryString(string httpMethod, string resourceName, string resourceAction, string parameter = null)
                        builder.AppendIndent(CodeIndent, 4);
                        if (returnVoid)
                        {
                            builder.AppendFormat("this.InvokeUsingQueryString({0}, \"{1}\", \"{2}\", \"{3}\", {4},null);",
                                string.IsNullOrWhiteSpace(apiContractAttribute.Realm) ? nullString : apiContractAttribute.Realm.AsQuotedString(),
                                apiContractAttribute.Version,
                                apiOperationAttribute.HttpMethod,
                                apiOperationAttribute.ResourceName,
                                apiOperationAttribute.Action.AsQuotedString().SafeToString(nullString));
                        }
                        else
                        {
                            builder.AppendFormat("return this.InvokeUsingQueryString({0}, \"{1}\", \"{2}\", \"{3}\", {4},null).SafeToObject<{5}>();",
                                string.IsNullOrWhiteSpace(apiContractAttribute.Realm) ? nullString : apiContractAttribute.Realm.AsQuotedString(),
                                apiContractAttribute.Version,
                                apiOperationAttribute.HttpMethod,
                                apiOperationAttribute.ResourceName,
                                apiOperationAttribute.Action.AsQuotedString().SafeToString(nullString),
                                methodInfo.ReturnType.ToCodeLook());
                        }
                        builder.AppendLine();
                    }

                    builder.AppendIndent(CodeIndent, 3);
                    builder.AppendLine("}");

                    builder.AppendIndent(CodeIndent, 3);
                    builder.AppendLine("catch (Exception ex)");
                    builder.AppendIndent(CodeIndent, 3);
                    builder.AppendLine("{");
                    builder.AppendIndent(CodeIndent, 4);
                    builder.AppendLine("throw ex.Handle(new { " + methodInfo.MethodInputParametersToCodeLook(false) + " });");
                    builder.AppendIndent(CodeIndent, 3);
                    builder.AppendLine("}");

                    //Body end
                    builder.AppendIndent(CodeIndent, 2);
                    builder.AppendLine("}");
                    builder.AppendLine();

                    doneApi.Add(routeKey);
                }
            }
        }

        /// <summary>
        /// Generates the interface part.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="doneApi">The done API.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="lastApiContractAttribute">The last API contract attribute.</param>
        protected void GenerateInterfacePart(StringBuilder builder, HashSet<string> doneApi, Type interfaceType, ApiContractAttribute lastApiContractAttribute = null)
        {
            if (builder != null && doneApi != null && interfaceType != null)
            {
                var apiContractAttribute = interfaceType.GetCustomAttribute<ApiContractAttribute>(true) ?? lastApiContractAttribute;

                foreach (var method in interfaceType.GetMethods())
                {
                    var apiOperationAttribute = method.GetCustomAttribute<ApiOperationAttribute>(true);

                    if (apiOperationAttribute != null)
                    {
                        GenerateMethodPart(builder, doneApi, apiContractAttribute, apiOperationAttribute, method);
                    }
                }

                var interfaces = interfaceType.GetInterfaces();
                if (interfaces.HasItem())
                {
                    foreach (var one in interfaceType.GetInterfaces())
                    {
                        GenerateInterfacePart(builder, doneApi, one, apiContractAttribute);
                    }
                }
            }
        }

        /// <summary>
        /// Writes the constructor.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="className">Name of the class.</param>
        protected override void WriteConstructor(StringBuilder builder, string className)
        {
            if (builder != null)
            {
                builder.AppendIndent(CodeIndent, 2);
                builder.AppendLineWithFormat("public {0}(ApiEndpoint endpoint, bool acceptGZip = false, int? timeout = null):base(endpoint, acceptGZip, timeout)", className);
                builder.AppendIndent(CodeIndent, 2);
                builder.AppendLine("{");
                builder.AppendIndent(CodeIndent, 2);
                builder.AppendLine("}");
                builder.AppendLine();

                builder.AppendIndent(CodeIndent, 2);
                builder.Append("protected override int ClientGeneratedVersion { get{ return ");
                builder.Append(RestApiClient.BaseClientVersion);
                builder.Append("; }}");
                builder.AppendLine();
                builder.AppendLine();
            }
        }
    }
}