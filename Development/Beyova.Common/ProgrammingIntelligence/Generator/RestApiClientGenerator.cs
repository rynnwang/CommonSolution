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
    public class RestApiClientGenerator : AbstractRestApiGenerator
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
        /// The interface full names
        /// </summary>
        private Dictionary<string, Type> Interfaces = new Dictionary<string, Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiClientGenerator" /> class.
        /// </summary>
        /// <param name="namespace">The namespace.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="codeIndent">The code indent.</param>
        public RestApiClientGenerator(string @namespace, string className, string codeIndent = null)
            : base()
        {
            Namespace = @namespace.SafeToString("Beyova.RestApi.Client");
            ClassName = className.SafeToString("RestApiClient");
            CodeIndent = codeIndent.SafeToString(defaultCodeIndent);
        }

        /// <summary>
        /// Generates the code.
        /// </summary>
        /// <param name="instances">The instances.</param>
        /// <returns>System.String.</returns>
        public string GenerateCode(params object[] instances)
        {
            var builder = new StringBuilder();

            if (instances != null && instances.Any())
            {
                GenerateCode(builder, instances);
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

            foreach (var item in typeof(T).GetInterfaces())
            {
                Interfaces.Add(item.FullName, item);
                GenerateInterfacePart(builder, doneApiHash, item);
            }

            // End of class
            builder.AppendIndent(CodeIndent, 1);
            builder.AppendLine("}");

            // End of namespace
            builder.AppendLine("}");

            builder.AppendLine();
            return builder.ToString();
        }

        /// <summary>
        /// Generates the code.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="instances">The instances.</param>
        public void GenerateCode(string path, params object[] instances)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                var builder = new StringBuilder();
                GenerateCode(builder, instances);

                File.WriteAllText(path, builder.ToString(), Encoding.UTF8);
            }
        }

        /// <summary>
        /// Generates the code.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The path.</param>
        public void GenerateCode<T>(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                File.WriteAllText(path, GenerateCode<T>(), Encoding.UTF8);
            }
        }

        /// <summary>
        /// Generates the code.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="instances">The instances.</param>
        protected void GenerateCode(StringBuilder builder, object[] instances)
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

                if (instances != null && instances.Any())
                {
                    foreach (var instance in instances)
                    {
                        foreach (var item in instance.GetType().GetInterfaces())
                        {
                            Interfaces.Add(item.FullName, item);
                            GenerateInterfacePart(builder, doneApiHash, item);
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
        /// <param name="version">The version.</param>
        /// <param name="apiOperationAttribute">The API URI.</param>
        /// <param name="methodInfo">The method information.</param>
        protected override void GenerateMethodPart(StringBuilder builder, HashSet<string> doneApi, string version, ApiOperationAttribute apiOperationAttribute, MethodInfo methodInfo)
        {
            if (builder != null && doneApi != null && methodInfo != null && apiOperationAttribute != null)
            {
                var routeKey = ApiHandlerBase.GetRouteKey(version, apiOperationAttribute.ResourceName, apiOperationAttribute.HttpMethod, apiOperationAttribute.Action);

                if (!doneApi.Contains(routeKey))
                {
                    //Declaration
                    builder.AppendIndent(CodeIndent, 2);
                    builder.AppendLineWithFormat("public virtual {0}", methodInfo.ToCodeLook(true));
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
    }
}
