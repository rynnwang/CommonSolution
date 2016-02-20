using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Beyova.ProgrammingIntelligence;
using Beyova;
using System.Collections.ObjectModel;
using Beyova.ExceptionSystem;

namespace Beyova.RestApi
{
    /// <summary>
    /// Class RestApiClientGenerator.
    /// </summary>
    public class RestApiClientGenerator
    {
        /// <summary>
        /// The code indent
        /// </summary>
        protected const string defaultCodeIndent = "    ";

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
        public string Namespace { get; protected set; }

        /// <summary>
        /// Gets or sets the name of the class.
        /// </summary>
        /// <value>The name of the class.</value>
        public string ClassName { get; protected set; }

        /// <summary>
        /// Gets or sets the code indent.
        /// </summary>
        /// <value>The code indent.</value>
        public string CodeIndent { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiClientGenerator" /> class.
        /// </summary>
        /// <param name="namespace">The namespace.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="codeIndent">The code indent.</param>
        public RestApiClientGenerator(string @namespace, string className, string codeIndent = null)
        {
            Namespace = @namespace.SafeToString("Beyova.RestApi.Client");
            ClassName = className.SafeToString("RestApiClient");
            CodeIndent = codeIndent.SafeToString(defaultCodeIndent);
        }

        #region Public methods

        /// <summary>
        /// Generates the code by instance.
        /// </summary>
        /// <param name="instances">The instances.</param>
        /// <returns>System.String.</returns>
        public string GenerateCodeByInstance(params object[] instances)
        {
            var builder = new StringBuilder();

            if (instances.HasItem())
            {
                var types = instances.Select((x) => x.GetType()).ToArray();
                GenerateCodeByTypes(builder, types);
            }

            return builder.ToString();
        }

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
        /// Generates the code to path by instance.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="instances">The instances.</param>
        public void GenerateCodeToPathByInstance(string path, params object[] instances)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                File.WriteAllText(path, GenerateCodeByInstance(instances), Encoding.UTF8);
            }
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

        #endregion

        /// <summary>
        /// Generates the code by type. Type here can be both Interface and Class. Method would recursively find all interfaces.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="types">The types.</param>
        protected void GenerateCodeByTypes(StringBuilder builder, params Type[] types)
        {
            if (builder != null)
            {
                WriteFileInfo(builder);
                WriteNamespaces(builder);
                builder.AppendLine();

                // write namespace
                builder.AppendLineWithFormat("namespace {0}", Namespace);
                builder.AppendLine("{");

                // write class declaration
                builder.AppendIndent(CodeIndent, 1);
                builder.AppendLineWithFormat("public class {0}: " + typeof(RestApiClient).ToCodeLook(true), ClassName);
                builder.AppendIndent(CodeIndent, 1);
                builder.AppendLine("{");

                // write constructor
                WriteConstructor(builder, ClassName);

                // write interface based members
                var doneApiHash = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
                HashSet<string> handledInterfaces = new HashSet<string>();

                if (types.HasItem())
                {
                    foreach (var type in types)
                    {
                        if (type.IsInterface)
                        {
                            if (!IsDuplicatedInterface(handledInterfaces, type))
                            {
                                GenerateInterfacePart(builder, doneApiHash, type);
                            }
                        }
                        foreach (var item in type.GetInterfaces())
                        {
                            if (!IsDuplicatedInterface(handledInterfaces, item))
                            {
                                GenerateInterfacePart(builder, doneApiHash, item);
                            }
                        }
                    }
                }

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
        /// <param name="apiOperationAttribute">The API URI.</param>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="genericParameterConstraints">The generic parameter constraints.</param>
        /// <exception cref="InvalidObjectException">
        /// methodInfo.Parameter
        /// or
        /// methodInfo.Parameter
        /// </exception>
        protected void GenerateMethodPart(StringBuilder builder, HashSet<string> doneApi, ApiOperationAttribute apiOperationAttribute, MethodInfo methodInfo, Dictionary<string, Type[]> genericParameterConstraints)
        {
            //// No need to check whether has ApiContract any more. Because:1. Inherited class can add it, 2. Version is defined by constructor parameter, it is nothing about how is defined in interface.
            if (builder != null && doneApi != null && methodInfo != null && apiOperationAttribute != null)
            {
                var routeKey = string.Format("{0}/{1}/{2}", apiOperationAttribute.ResourceName, apiOperationAttribute.HttpMethod, apiOperationAttribute.Action);

                if (!doneApi.Contains(routeKey))
                {
                    //Declaration
                    builder.AppendIndent(CodeIndent, 2);
                    builder.AppendLineWithFormat("public virtual {0}", methodInfo.ToCodeLook(true, genericParameterConstraints?.Keys));

                    //Check generic parameters constraints 
                    var genericParameters = FindMatchedGenericConstraints(methodInfo, genericParameterConstraints);
                    if (genericParameters.HasItem())
                    {
                        foreach (var one in genericParameters)
                        {
                            builder.AppendIndent(CodeIndent, 3);
                            builder.AppendFormat("where {0}:", one.Key);

                            foreach (var g in one.Value)
                            {
                                builder.AppendFormat("{0},", g.ToCodeLook(true));
                            }

                            builder.RemoveLastIfMatch(',');
                            builder.AppendLine();
                        }
                    }

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
                                    builder.AppendLineWithFormat("parameters.Add(\"{0}\",{0});", one.Name);
                                }

                                ////To Call: JToken InvokeUsingBody(string httpMethod, string resourceName, string resourceAction, object parameter)
                                builder.AppendIndent(CodeIndent, 4);
                                if (returnVoid)
                                {
                                    builder.AppendFormat("this.InvokeUsingBody(\"{0}\",\"{1}\",{2}, parameters);",
                                        apiOperationAttribute.HttpMethod,
                                        apiOperationAttribute.ResourceName,
                                        apiOperationAttribute.Action.AsQuotedString().SafeToString("null"));
                                }
                                else
                                {
                                    builder.AppendFormat("return this.InvokeUsingBody(\"{0}\",\"{1}\",{2}, parameters).Value<{3}>();",
                                        apiOperationAttribute.HttpMethod,
                                        apiOperationAttribute.ResourceName,
                                        apiOperationAttribute.Action.AsQuotedString().SafeToString("null"),
                                        methodInfo.ReturnType.ToCodeLook(true)
                                    );
                                }
                                builder.AppendLine();
                            }
                            else
                            {
                                builder.AppendIndent(CodeIndent, 4);
                                builder.AppendLine("var parameters = new Dictionary<string, string>();");

                                foreach (var one in parameters)
                                {
                                    if (!one.ParameterType.IsSimpleType())
                                    {
                                        throw new InvalidObjectException("methodInfo.Parameter", data: one.ParameterType.ToCodeLook(true), reason: "Multiple complex objects are used in a un-body REST API method.");
                                    }

                                    builder.AppendIndent(CodeIndent, 4);
                                    builder.AppendLineWithFormat("parameters.Add(\"{0}\",{0});", one.Name, SimpleVariableToStringCode(one.Name, one.ParameterType));
                                }

                                ////To Call: JToken InvokeUsingCombinedQueryString(string httpMethod, string resourceName, string resourceAction, Dictionary<string, string> parameters)
                                builder.AppendIndent(CodeIndent, 4);
                                if (returnVoid)
                                {
                                    builder.AppendFormat("this.InvokeUsingCombinedQueryString(\"{0}\",\"{1}\",{2}, parameters);",
                                        apiOperationAttribute.HttpMethod,
                                        apiOperationAttribute.ResourceName,
                                        apiOperationAttribute.Action.AsQuotedString().SafeToString("null"));
                                }
                                else
                                {
                                    builder.AppendFormat("return this.InvokeUsingCombinedQueryString(\"{0}\",\"{1}\",{2}, parameters).Value<{3}>();",
                                        apiOperationAttribute.HttpMethod,
                                        apiOperationAttribute.ResourceName,
                                        apiOperationAttribute.Action.AsQuotedString().SafeToString("null"),
                                        methodInfo.ReturnType.ToCodeLook(true));
                                }
                                builder.AppendLine();
                            }

                            #endregion
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
                                    builder.AppendFormat("this.InvokeUsingQueryString(\"{0}\",\"{1}\",{2}, {3});",
                                        apiOperationAttribute.HttpMethod,
                                        apiOperationAttribute.ResourceName,
                                        apiOperationAttribute.Action.AsQuotedString().SafeToString("null"),
                                        SimpleVariableToStringCode(firstParameter.Name, firstParameter.ParameterType)
                                    );
                                }
                                else
                                {
                                    builder.AppendFormat("return this.InvokeUsingQueryString(\"{0}\",\"{1}\",{2}, {3}).Value<{4}>();",
                                        apiOperationAttribute.HttpMethod,
                                        apiOperationAttribute.ResourceName,
                                        apiOperationAttribute.Action.AsQuotedString().SafeToString("null"),
                                        SimpleVariableToStringCode(firstParameter.Name, firstParameter.ParameterType),
                                        methodInfo.ReturnType.ToCodeLook(true));
                                }
                                builder.AppendLine();
                            }
                            else if (!allowBody)
                            {
                                throw new InvalidObjectException("methodInfo.Parameter", data: firstParameter.ParameterType.ToCodeLook(true), reason: "Complex object is used in a un-body REST API method.");
                            }
                            else
                            {
                                //// To Call: JToken InvokeUsingBody(string httpMethod, string resourceName, string resourceAction, object parameter)
                                builder.AppendIndent(CodeIndent, 4);
                                if (returnVoid)
                                {
                                    builder.AppendFormat("this.InvokeUsingBody(\"{0}\",\"{1}\",{2}, {3});",
                                        apiOperationAttribute.HttpMethod,
                                        apiOperationAttribute.ResourceName,
                                        apiOperationAttribute.Action.AsQuotedString().SafeToString("null"),
                                        firstParameter.Name);
                                }
                                else
                                {
                                    builder.AppendFormat("return this.InvokeUsingBody(\"{0}\",\"{1}\",{2}, {3}).Value<{4}>();",
                                        apiOperationAttribute.HttpMethod,
                                        apiOperationAttribute.ResourceName,
                                        apiOperationAttribute.Action.AsQuotedString().SafeToString("null"),
                                        firstParameter.Name,
                                        methodInfo.ReturnType.ToCodeLook(true));
                                }

                                builder.AppendLine();
                            }

                            #endregion
                        }
                    }
                    else
                    {
                        //// To Call: JToken InvokeUsingQueryString(string httpMethod, string resourceName, string resourceAction, string parameter = null)
                        builder.AppendIndent(CodeIndent, 4);
                        if (returnVoid)
                        {
                            builder.AppendFormat("this.InvokeUsingQueryString(\"{0}\",\"{1}\",{2},null);",
                                apiOperationAttribute.HttpMethod,
                                apiOperationAttribute.ResourceName,
                                apiOperationAttribute.Action.AsQuotedString().SafeToString("null"));
                        }
                        else
                        {
                            builder.AppendFormat("this.InvokeUsingQueryString(\"{0}\",\"{1}\",{2},null).Value<{3}>();",
                                apiOperationAttribute.HttpMethod,
                                apiOperationAttribute.ResourceName,
                                apiOperationAttribute.Action.AsQuotedString().SafeToString("null"),
                                methodInfo.ReturnType.ToCodeLook(true));
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
                    builder.AppendLine("throw ex.Handle(\"" + methodInfo.Name + "\", new { " + methodInfo.MethodInputParametersToCodeLook(false) + " });");
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
                var apiClass = interfaceType.GetCustomAttribute<ApiContractAttribute>(true) ?? lastApiContractAttribute;

                var genericParameterConstraints = GetGenericTypeConstraints(interfaceType);
                foreach (var method in interfaceType.GetMethods())
                {
                    var apiOperationAttribute = method.GetCustomAttribute<ApiOperationAttribute>(true);

                    if (apiOperationAttribute != null)
                    {
                        GenerateMethodPart(builder, doneApi, apiOperationAttribute, method, genericParameterConstraints);
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

        /// <summary>
        /// Writes the file information.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected void WriteFileInfo(StringBuilder builder)
        {
            if (builder != null)
            {
                builder.AppendIndent('/', 30);
                builder.AppendLine();

                builder.AppendLine("// This code is generated by Beyova.common.RestApiClientGenerator.");
                builder.AppendLineWithFormat("// UTC: {0}", DateTime.UtcNow.ToFullDateTimeString());

                builder.AppendIndent('/', 30);
                builder.AppendLine();
            }
        }

        /// <summary>
        /// Writes the namespaces.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected void WriteNamespaces(StringBuilder builder)
        {
            if (builder != null)
            {
                builder.AppendLine("using System;");
                builder.AppendLine("using System.Collections.Generic;");
                builder.AppendLine("using System.Linq;");
                builder.AppendLine("using System.Net;");
                builder.AppendLine("using System.Reflection;");
                builder.AppendLine("using System.Text;");
                builder.AppendLine("using Beyova.ProgrammingIntelligence;");
                builder.AppendLine("using Beyova.ExceptionSystem;");
                builder.AppendLine("using Beyova;");
                builder.AppendLine("using Beyova.RestApi;");
                builder.AppendLine("using Newtonsoft.Json;");
                builder.AppendLine("using Newtonsoft.Json.Linq;");
            }
        }

        /// <summary>
        /// Writes the constructor.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="className">Name of the class.</param>
        protected void WriteConstructor(StringBuilder builder, string className)
        {
            if (builder != null)
            {
                builder.AppendIndent(CodeIndent, 2);
                builder.AppendLineWithFormat("public {0}(ApiEndpoint endpoint, bool enableExceptionRestore = false):base(endpoint, enableExceptionRestore)", className);
                builder.AppendIndent(CodeIndent, 2);
                builder.AppendLine("{");
                builder.AppendIndent(CodeIndent, 2);
                builder.AppendLine("}");
                builder.AppendLine();
            }
        }

        #region Util

        /// <summary>
        /// Determines whether [is duplicated interface] [the specified type].
        /// </summary>
        /// <param name="handledInterfaces">The handled interfaces.</param>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is duplicated interface] [the specified type]; otherwise, <c>false</c>.</returns>
        protected bool IsDuplicatedInterface(HashSet<string> handledInterfaces, Type type)
        {
            if (handledInterfaces == null || type == null || !type.IsInterface)
            {
                return false;
            }

            var name = string.Format("{0}.{1}", type.Namespace, type.Name);
            if (!handledInterfaces.Contains(name))
            {
                handledInterfaces.Add(name);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Simples the variable to string code.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.String.</returns>
        protected static string SimpleVariableToStringCode(string name, Type type)
        {
            if (type.IsNullable())
            {
                return string.Format("{0} == null? null: {1}", name, SimpleVariableToStringCode(name, type.GetNullableType()));
            }
            if (type.IsEnum)
            {
                return string.Format("((int){0}).ToString()", name);
            }
            else if (type == typeof(DateTime))
            {
                return string.Format("{0}.ToFullDateTimeTzString()", name);
            }
            else if (type == typeof(TimeSpan))
            {
                return string.Format("{0}.ToString(\"c\")", name);
            }

            return string.Format("{0}.ToString()", name);
        }

        /// <summary>
        /// Gets the generic type constraints.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Dictionary&lt;System.String, Type[]&gt;.</returns>
        protected static Dictionary<string, Type[]> GetGenericTypeConstraints(Type type)
        {
            Dictionary<string, Type[]> result = null;

            if (type != null && type.IsGenericType)
            {
                result = new Dictionary<string, Type[]>();
                foreach (var one in type.GetGenericArguments())
                {
                    if (one.IsGenericParameter)
                    {
                        var constraints = one.GetGenericParameterConstraints();
                        if (constraints.Length > 0)
                        {
                            result.Add(one.Name, constraints);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Finds the matched generic constraints.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="genericParameterConstraints">The generic parameter constraints.</param>
        /// <returns>Dictionary&lt;System.String, Type[]&gt;.</returns>
        protected static Dictionary<string, Type[]> FindMatchedGenericConstraints(MethodInfo methodInfo, Dictionary<string, Type[]> genericParameterConstraints)
        {
            Dictionary<string, Type[]> result = null;

            if (methodInfo.IsGenericMethod && genericParameterConstraints != null)
            {
                result = new Dictionary<string, Type[]>();
                foreach (var one in methodInfo.GetParameters())
                {
                    if (one.ParameterType.IsGenericParameter && genericParameterConstraints.ContainsKey(one.ParameterType.Name))
                    {
                        result.Add(one.Name, genericParameterConstraints[one.ParameterType.Name]);
                    }
                }
            }

            return result;
        }

        #endregion
    }
}
