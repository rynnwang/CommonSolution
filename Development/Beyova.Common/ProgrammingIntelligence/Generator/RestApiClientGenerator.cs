using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Beyova.ProgrammingIntelligence;
using Beyova;

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

            // write property
            WriteProperties(builder);

            // write constructor
            WriteConstructor(builder, ClassName);

            WriteInitializeMethod(builder);

            // write interface based members
            var doneApiHash = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var item in typeof(T).GetInterfaces())
            {
                Interfaces.Add(item.FullName, item);
                GenerateInterfacePart(builder, doneApiHash, item);
            }

            WriteInternalInterface(builder);

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
                WriteProperties(builder);

                // write constructor
                WriteConstructor(builder, ClassName);

                WriteInitializeMethod(builder);

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

                WriteInternalInterface(builder);

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

                    builder.AppendIndent(CodeIndent, 4);
                    if (methodInfo.ReturnType.IsVoid() ?? false)
                    {
                        builder.AppendLine("this.InvokeWithVoid(MethodMappings.SafeGetValue(\"" + methodInfo.Name + "\")"
                            + (methodInfo.GetParameters().Any() ? (", " + methodInfo.MethodInputParametersToCodeLook(false)) : string.Empty)
                            + ");");
                    }
                    else
                    {
                        builder.AppendLine("return this.InvokeAs<" + methodInfo.ReturnType.ToCodeLook(true) + ">(MethodMappings.SafeGetValue(\"" + methodInfo.Name + "\")"
                            + (methodInfo.GetParameters().Any() ? (", " + methodInfo.MethodInputParametersToCodeLook(false)) : string.Empty)
                            + ");");
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
                builder.AppendIndent(CodeIndent, 3);
                builder.AppendLine("this.ApiType = typeof(IInternalInterfaces);");
                builder.AppendIndent(CodeIndent, 3);
                builder.AppendLine("this.MethodMappings = new Dictionary<string, MethodInfo>();");
                builder.AppendIndent(CodeIndent, 3);
                builder.AppendLine("Initialize(this.ApiType);");
                builder.AppendIndent(CodeIndent, 2);
                builder.AppendLine("}");
                builder.AppendLine();
            }
        }

        /// <summary>
        /// Writes the internal interface.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected void WriteInternalInterface(StringBuilder builder)
        {
            builder.AppendIndent(CodeIndent, 2);
            builder.Append("protected interface IInternalInterfaces:");
            foreach (var one in Interfaces)
            {
                builder.AppendFormat("{0},", one.Value.ToCodeLook(true));
            }
            builder.RemoveLastIfMatch(',');
            builder.Append("{}");
            builder.AppendLine();
        }

        /// <summary>
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected void WriteProperties(StringBuilder builder)
        {
            if (builder != null)
            {
                builder.AppendIndent(CodeIndent, 2);
                builder.AppendLine("public Type ApiType{ get; protected set; }");
                builder.AppendLine();

                builder.AppendIndent(CodeIndent, 2);
                builder.AppendLine("public Dictionary<string, MethodInfo> MethodMappings{ get; protected set; }");
                builder.AppendLine();
            }
        }

        /// <summary>
        /// Writes the initialize method.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected void WriteInitializeMethod(StringBuilder builder)
        {
            if (builder != null)
            {
                builder.AppendIndent(CodeIndent, 2);
                builder.AppendLine("protected void Initialize(Type apiType)");
                builder.AppendIndent(CodeIndent, 2);
                builder.AppendLine("{");
                builder.AppendIndent(CodeIndent, 3);
                builder.AppendLine("if (apiType != null)");
                builder.AppendIndent(CodeIndent, 3);
                builder.AppendLine("{");
                builder.AppendIndent(CodeIndent, 4);
                builder.AppendLine("foreach (var one in apiType.GetInterfaceMethods(typeof(ApiOperationAttribute), true))");
                builder.AppendIndent(CodeIndent, 4);
                builder.AppendLine("{MethodMappings.Add(one.Name, one);}");
                builder.AppendIndent(CodeIndent, 3);
                builder.AppendLine("}");
                builder.AppendIndent(CodeIndent, 2);
                builder.AppendLine("}");
                builder.AppendLine();
            }
        }
    }
}
