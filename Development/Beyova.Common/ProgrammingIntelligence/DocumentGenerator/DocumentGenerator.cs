using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using Beyova.Api;
using Beyova.RestApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class DocumentGenerator.
    /// </summary>
    public class DocumentGenerator
    {
        #region Format

        /// <summary>
        /// The property name
        /// </summary>
        protected const string propertyNameFormat = "<span class=\"PropertyName {2}\" title=\"{1}\">\"{0}\"</span>";

        /// <summary>
        /// The object brace
        /// </summary>
        protected const string objectBrace = "<span class=\"ObjectBrace\">{0}</span>";

        /// <summary>
        /// The array brace
        /// </summary>
        protected const string arrayBrace = "<span class=\"ArrayBrace\">{0}</span>";

        /// <summary>
        /// The enum format
        /// </summary>
        protected const string enumFormat = "<a class=\"enum\" href=\"{1}.html\">{0}</a>";

        /// <summary>
        /// The HTTP status format
        /// </summary>
        protected const string httpStatusFormat = "<li style=\"color:{3}\"><span>{0}</span> ({1}): {2}</li>";

        /// <summary>
        /// The custom header format
        /// </summary>
        protected const string customHeaderFormat = "<li>{0}</li>";

        /// <summary>
        /// The request header format
        /// </summary>
        protected const string requestHeaderFormat = "<httpHeader style=\"display:block;text-align:left;\"><label>{0}</label>: <value style=\"font-style:italic; font-weight:bold;\">{1}</value></httpHeader>";

        /// <summary>
        /// The panel
        /// </summary>
        protected const string panel = "<div id=\"{2}\" style=\"margin:5px;margin-bottom: 20px;background-color: #fff;border: 1px solid transparent;border-color: #337ab7;border-radius: 4px;box-shadow: 0 1px 1px rgba(0, 0, 0, .05);box-sizing: border-box;\"><div style=\"color: #fff;background-color: #337ab7;border-color: #337ab7;padding: 10px 15px;border-bottom: 1px solid transparent;border-top-left-radius: 3px;border-top-right-radius: 3px;\"><a style=\"color:#eeeeee;\" href=\"{3}.html\"><h3 style=\"margin-top: 0;margin-bottom: 0;font-size: 16px;color: inherit;\">{0}</h3></a></div><div style=\"padding: 15px;\">{1}</div></div>";

        //<pre class="CodeContainer">
        // <span class="ObjectBrace">{</span>
        //    <span class="PropertyName">"A"</span>: <span class="ObjectBrace">{</span>
        //        <span class="PropertyName">"B"</span>: <span class="ObjectBrace">{ }</span><span class="Comma">,</span>
        //        <span class="PropertyName">"C"</span>: <span class="String">"string"</span>
        //    <span class="ObjectBrace">}</span>
        //<span class="ObjectBrace">}</span>
        //</pre>

        #endregion Format

        #region CSS

        /// <summary>
        /// The enum table CSS
        /// </summary>
        protected const string enumTableCss = @"
table.enum-table{
border: 1px solid #ddd;
}

table.enum-table th.name,table.enum-table td.name{
width: 200px;
}

table.enum-table th.value,table.enum-table td.value{
width: 100px;
}
";

        /// <summary>
        /// The operation panel CSS
        /// </summary>
        protected const string operationPanelCss = @"
div.operation-panel{
margin:5px;
margin-bottom: 20px;
background-color: #fff;
border: 1px solid transparent;
border-color: #337ab7;
border-radius: 4px;
box-shadow: 0 1px 1px rgba(0, 0, 0, .05);
box-sizing: border-box
}

div.operation-panel > div.title{
color: #fff;
background-color: #337ab7;
border-color: #337ab7;
padding: 10px 15px;
border-bottom: 1px solid transparent;
border-top-left-radius: 3px;
border-top-right-radius: 3px;
}

div.operation-panel > div.title  a{
color:#eeeeee;
}

div.operation-panel > div.title h3{
margin-top: 0;
margin-bottom: 0;
font-size: 16px;
color: inherit;
}

div.operation-panel > div.body{
padding: 15px;
}
";

        /// <summary>
        /// The json CSS
        /// </summary>
        protected const string jsonCss = @"
.ObjectBrace {
    color: #00AA00;
    font-weight: bold;
}

.ArrayBrace {
    color: #0033FF;
    font-weight: bold;
}

.PropertyName {
    color: #CC0000;
    font-weight: bold;
}

.String {
    color: #007777;
}

.Number {
    color: #AA00AA;
}

.Boolean {
    color: #0000FF;
}

.Null {
    color: #0000FF;
}

pre.CodeContainer {
    margin-top: 0px;
    margin-bottom: 0px;
    font-family: monospace;
    font-size:14px;
}

url{
    display: block;
    font-family:monospace;
}
";

        #endregion CSS



        #region Fields

        /// <summary>
        /// Gets or sets the token key.
        /// </summary>
        /// <value>The token key.</value>
        public string TokenKey { get; protected set; }

        #endregion Fields

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentGenerator" /> class.
        /// </summary>
        /// <param name="tokenKey">The token key.</param>
        public DocumentGenerator(string tokenKey)
        {
            this.TokenKey = tokenKey;
        }

        /// <summary>
        /// Writes the HTML document to zip. If types has no item, all API in current AppDomain would be generated.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <returns>System.Byte[].</returns>
        public byte[] WriteHtmlDocumentToZipByType(params Type[] types)
        {
            var container = new Dictionary<string, byte[]>();
            WriteHtmlDocumentByType<Dictionary<string, byte[]>>((c, name, b) =>
            {
                c.Add(name, b);
            }, container, types.HasItem() ? types : GetAssemblyType().ToArray());

            return container.Any() ? container.ZipAsBytes() : null;
        }

        /// <summary>
        /// Writes the HTML document to zip by routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        /// <returns></returns>
        public byte[] WriteHtmlDocumentToZipByRoutes(params RuntimeRoute[] routes)
        {
            var container = new Dictionary<string, byte[]>();
            WriteHtmlDocumentByType<Dictionary<string, byte[]>>((c, name, b) =>
            {
                c.Add(name, b);
            }, container, (routes.HasItem() ? routes : RestApiRouter.RuntimeRoutes as IEnumerable<RuntimeRoute>)
            .Distinct(new LambdaEqualityComparer<RuntimeRoute>((x, y) => { return x?.InstanceType == y?.InstanceType; }, x => x?.InstanceType?.GetHashCode() ?? 0))
            .Select(x => new KeyValuePair<Type, IApiContractOptions>(x.InstanceType, x.ApiRouteIdentifier)).ToArray());

            return container.Any() ? container.ZipAsBytes() : null;
        }

        /// <summary>
        /// Writes the HTML document to zip.
        /// This method is mainly used by CodeSmith
        /// </summary>
        /// <param name="typeFullNames">The type full names.</param>
        /// <returns>System.Byte[].</returns>
        public byte[] WriteHtmlDocumentToZipByTypeFullNames(string[] typeFullNames)
        {
            var container = new Dictionary<string, byte[]>();
            var types = GetAssemblyType();

            if (typeFullNames.HasItem())
            {
                types = types.FindAll(x => typeFullNames.Contains(x.GetFullName()));
            }

            WriteHtmlDocumentByType<Dictionary<string, byte[]>>((c, name, b) =>
            {
                c.Add(name, b);
            }, container, types.ToArray());

            return container.Any() ? container.ZipAsBytes() : null;
        }

        /// <summary>
        /// Writes the HTML document to file. If types has no item, all API in current AppDomain would be generated.
        /// </summary>
        /// <param name="containerPath">The container path.</param>
        /// <param name="types">The types.</param>
        public void WriteHtmlDocumentToFile(string containerPath, params Type[] types)
        {
            WriteHtmlDocumentByType<string>((c, name, b) =>
            {
                File.WriteAllBytes(Path.Combine(c, name), b);
            }, containerPath, types.HasItem() ? types : GetAssemblyType().ToArray());
        }

        /// <summary>
        /// Gets the type of the assembly.
        /// </summary>
        /// <returns>List&lt;Type&gt;.</returns>
        private static List<Type> GetAssemblyType()
        {
            List<Type> result = new List<Type>();
            foreach (var assembly in ReflectionExtension.GetAppDomainAssemblies())
            {
                foreach (var one in assembly.GetTypes())
                {
                    var apiContractAttribute = one.GetCustomAttribute<ApiContractAttribute>(true);
                    if (apiContractAttribute != null)
                    {
                        result.Add(one);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Writes the type of the HTML document by.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="packageDocumentDelegate">The package document delegate.</param>
        /// <param name="container">The container.</param>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        private T WriteHtmlDocumentByType<T>(Action<T, string, byte[]> packageDocumentDelegate, T container, params Type[] types)
        {
            List<KeyValuePair<Type, IApiContractOptions>> info = new List<KeyValuePair<Type, IApiContractOptions>>();

            foreach (var one in types)
            {
                var apiContractAttribute = one.GetCustomAttribute<ApiContractAttribute>(true);
                if (apiContractAttribute != null)
                {
                    info.Add(new KeyValuePair<Type, IApiContractOptions>(one, apiContractAttribute));
                }
            }

            return WriteHtmlDocumentByType(packageDocumentDelegate, container, info.ToArray());
        }

        /// <summary>
        /// Writes the HTML document.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="packageDocumentDelegate">The package document delegate.</param>
        /// <param name="container">The container.</param>
        /// <param name="types">The types.</param>
        /// <returns>T.</returns>
        private T WriteHtmlDocumentByType<T>(Action<T, string, byte[]> packageDocumentDelegate, T container, params KeyValuePair<Type, IApiContractOptions>[] types)
        {
            var zipFiles = new Dictionary<string, byte[]>();

            HashSet<Type> enumSets = new HashSet<Type>();
            //HashSet<string> apiOperationHash = new HashSet<string>();

            //Service List
            StringBuilder builder = new StringBuilder(types.Length * 1024);

            foreach (var one in types)
            {
                if (one.Value != null)
                {
                    WriteApiServiceHtmlDocumentPanel(builder, one.Key, one.Value);
                }
            }

            packageDocumentDelegate?.Invoke(container, "index.html", Encoding.UTF8.GetBytes(WriteAsEntireHtmlFile(builder.ToString(), "API Documentation")));

            //Api Files.
            foreach (var one in types)
            {
                builder = new StringBuilder(1024);
                builder.AppendLineWithFormat("<div style=\"display:block; background-color:#000000; color: #eeeeee\"><h1>{0} ({1})</h1></div>", one.Key.Name, one.Key.Namespace);
                WriteApiHtmlDocument(builder, one.Key, one.Value, one.Key.GetCustomAttribute<TokenRequiredAttribute>(true), enumSets);
                packageDocumentDelegate?.Invoke(container, one.Key.FullName + ".html", Encoding.UTF8.GetBytes(WriteAsEntireHtmlFile(builder.ToString(), "API - " + one.Key.Name)));
            }

            if (packageDocumentDelegate != null)
            {
                foreach (var one in enumSets)
                {
                    packageDocumentDelegate(container, one.FullName + ".html", Encoding.UTF8.GetBytes(GetEnumValueTable(one)));
                }
            }

            return container;
        }

        /// <summary>
        /// Writes the API service HTML document panel.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="apiServiceType">Type of the API service.</param>
        /// <param name="apiContractOptions">The API router identifier.</param>
        protected void WriteApiServiceHtmlDocumentPanel(StringBuilder builder, Type apiServiceType, IApiContractOptions apiContractOptions)
        {
            if (builder != null && apiServiceType != null && apiContractOptions != null)
            {
                HashSet<string> apiOperationHash = new HashSet<string>();

                var bodyBuilder = new StringBuilder("<ul>");
                var baseUrl = apiServiceType.FullName + ".html#";
                var list = apiServiceType.GetMethodInfoWithinAttribute<ApiOperationAttribute>(true, BindingFlags.Instance | BindingFlags.Public).OrderBy<MethodInfo, string>((m) => m.Name);

                foreach (var one in list)
                {
                    var apiOperationAttribute = one.GetCustomAttribute<ApiOperationAttribute>(true);

                    var id = GetApiOperationIdentifier(apiOperationAttribute);

                    if (!apiOperationHash.Contains(id))
                    {
                        bodyBuilder.Append("<li>");
                        bodyBuilder.AppendFormat("<a href=\"{0}\" title=\"{1}\">{1}</a> <span style=\"font-weight: bold;font-style: italic;color: #666666;\">({2})</span>", baseUrl + one.Name, one.Name
                            , string.Format("{0}: /api/{1}/{2}/{3}", apiOperationAttribute.HttpMethod, apiContractOptions.Version, apiOperationAttribute.ResourceName, apiOperationAttribute.Action).TrimEnd('/') + "/");
                        bodyBuilder.Append("</li>");

                        apiOperationHash.Add(id);
                    }
                }
                bodyBuilder.Append("</ul>");
                builder.AppendFormat(panel, string.Format("{0} ({1})", apiServiceType.Name, apiServiceType.Namespace), bodyBuilder.ToString(), apiServiceType.Name, apiServiceType.FullName);
            }
        }

        /// <summary>
        /// Writes the enum value table to file.
        /// </summary>
        /// <param name="containerPath">The container path.</param>
        /// <param name="enumType">Type of the enum.</param>
        private void WriteEnumValueTableToFile(string containerPath, Type enumType)
        {
            if (!string.IsNullOrWhiteSpace(containerPath))
            {
                var filePath = Path.Combine(containerPath, enumType.FullName + ".html");
                var content = GetEnumValueTable(enumType);

                File.WriteAllText(filePath, content);
            }
        }

        /// <summary>
        /// Gets the enum value table.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns>System.String.</returns>
        private string GetEnumValueTable(Type enumType)
        {
            if (enumType != null)
            {
                StringBuilder builder = new StringBuilder(1024);
                WriteEnumHtmlTable(builder, enumType);

                return WriteAsEntireHtmlFile(string.Format(panel, string.Format("Enum: {0} ({1})", enumType.Name, enumType.Namespace), builder.ToString(), enumType.Name, enumType.FullName), "API Enum - " + enumType.Name);
            }

            return string.Empty;
        }

        /// <summary>
        /// Writes the enum HTML table.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="enumType">Type of the enum.</param>
        private void WriteEnumHtmlTable(StringBuilder builder, Type enumType)
        {
            if (builder != null && enumType != null)
            {
                builder.Append("<table style=\"text-align:center;margin-bottom: 20px;border: 1px solid #ddd;background-color: transparent;border-spacing: 0;border-collapse: collapse;\"><thead><tr><th style=\"border: 1px solid #ddd;\">Name</th><th style=\"border: 1px solid #ddd;\">Value (Dec) </th><th style=\"border: 1px solid #ddd;\">Value (Hex) </th></tr></thead><tbody>");

                foreach (var one in Enum.GetValues(enumType))
                {
                    var number = (int)one;
                    builder.AppendFormat("<tr><td style=\"border: 1px solid #ddd;width: 200px;\">{0}</td><td style=\"border: 1px solid #ddd;width: 100px;\">{1}</td><td style=\"border: 1px solid #ddd;width: 100px;\">0x{2}</td></tr>", one.ToString(), number, number.ToString("X"));
                }

                builder.Append("</tbody></table>");
            }
        }

        /// <summary>
        /// Writes the API HTML document.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="apiServiceType">Type of the API service.</param>
        /// <param name="apiContractOptions">The API contract options.</param>
        /// <param name="classTokenRequiredAttribute">The class token required attribute.</param>
        /// <param name="enumSets">The enum sets.</param>
        protected void WriteApiHtmlDocument(StringBuilder builder, Type apiServiceType, IApiContractOptions apiContractOptions, TokenRequiredAttribute classTokenRequiredAttribute, HashSet<Type> enumSets)
        {
            if (builder != null && apiServiceType != null && apiContractOptions != null)
            {
                foreach (MethodInfo one in apiServiceType.GetMethodInfoWithinAttribute<ApiOperationAttribute>(true, BindingFlags.Instance | BindingFlags.Public))
                {
                    // Considering in interface, can NOT tell is async or not, check return type is Task or Task<T>.
                    bool isAsync = one.IsAsync() || one.ReturnType.IsTask();

                    var apiOperationAttribute = one.GetCustomAttribute<ApiOperationAttribute>(true);
                    if (apiOperationAttribute != null)
                    {
                        StringBuilder bodyBuilder = new StringBuilder(4096);

                        #region Entity Synchronization Status

                        var entitySynchronizationAttribute = one.GetCustomAttribute<EntitySynchronizationModeAttribute>(true);
                        if (entitySynchronizationAttribute != null && !EntitySynchronizationModeAttribute.IsReturnTypeMatched(one.ReturnType))
                        {
                            entitySynchronizationAttribute = null;
                        }

                        #endregion Entity Synchronization Status

                        //Original declaration

                        bodyBuilder.Append("<h3>.NET Declaration</h3>");
                        bodyBuilder.AppendFormat(isAsync ? "<div><span style=\"color:red;font-weight:bold;\" title=\"Async\">[A] </span> {0}</div>" : "<div>{0}</div>", one.ToDeclarationCodeLook().ToHtmlEncodedText());
                        bodyBuilder.Append("<hr />");

                        //Try append description
                        var apiDescriptionAttributes = one.GetCustomAttributes<ApiDescriptionAttribute>(true);
                        if (apiDescriptionAttributes != null && apiDescriptionAttributes.Any())
                        {
                            foreach (var description in apiDescriptionAttributes)
                            {
                                if (!string.IsNullOrWhiteSpace(description.Description))
                                {
                                    bodyBuilder.AppendFormat("<div>{0}</div>", description.Description.ToHtmlEncodedText());
                                }
                            }
                        }

                        // Customized headers
                        var apiCustomizedHeaderAttributes = one.GetCustomAttributes<ApiHeaderAttribute>(true);
                        if (apiCustomizedHeaderAttributes.HasItem())
                        {
                            bodyBuilder.Append("<h3>Customized headers</h3><hr />");
                            bodyBuilder.Append("<ul>");
                            foreach (var item in apiCustomizedHeaderAttributes)
                            {
                                bodyBuilder.AppendFormat(customHeaderFormat, item.HeaderKey);
                            }

                            bodyBuilder.Append("</ul>");
                        }

                        var obsolete = one.GetCustomAttribute<ObsoleteAttribute>(true);
                        if (obsolete != null)
                        {
                            bodyBuilder.AppendFormat("<div style=\"color:red;\"> Obsoleted: {0}</div>", obsolete.Message.ToHtmlEncodedText());
                        }

                        bodyBuilder.Append("<div>Following sample shows how to use via REST API.</div>");

                        #region Request

                        bodyBuilder.Append("<h3>Request</h3><hr />");
                        bodyBuilder.Append("<url>");

                        if (string.IsNullOrWhiteSpace(apiContractOptions.Realm))
                        {
                            bodyBuilder.AppendFormat("{0} /api/{1}/{2}/", apiOperationAttribute.HttpMethod, apiContractOptions.Version, apiOperationAttribute.ResourceName);
                        }
                        else
                        {
                            bodyBuilder.AppendFormat("{0} /{1}/api/{2}/{3}/", apiContractOptions.Realm, apiOperationAttribute.HttpMethod, apiContractOptions.Version, apiOperationAttribute.ResourceName);
                        }

                        if (!string.IsNullOrWhiteSpace(apiOperationAttribute.Action))
                        {
                            bodyBuilder.AppendFormat("{0}/", apiOperationAttribute.Action);
                        }

                        var parameters = one.GetParameters();
                        var parameterIsHandled = false;

                        if (parameters.Length == 0)
                        {
                            parameterIsHandled = true;
                        }
                        else if (parameters.Length == 1 && (apiOperationAttribute.HttpMethod.Equals(HttpConstants.HttpMethod.Get, StringComparison.OrdinalIgnoreCase)
                        || apiOperationAttribute.HttpMethod.Equals(HttpConstants.HttpMethod.Delete, StringComparison.OrdinalIgnoreCase)))
                        {
                            if (parameters[0].ParameterType == typeof(string) || parameters[0].ParameterType.IsValueType)
                            {
                                bodyBuilder.AppendFormat("<span style=\"font-style:italic; font-weight:bold;color:#CC0000;\" title=\"Sample value for {0}\">", parameters[0].Name);
                                FillSampleValue(bodyBuilder, parameters[0].ParameterType, enumSets, 0, fieldName: parameters[0].Name, ignoreQuote: true);
                                bodyBuilder.Append("</span>");
                                parameterIsHandled = true;
                            }
                        }
                        else if (parameters.Length > 1 && (apiOperationAttribute.HttpMethod.Equals(HttpConstants.HttpMethod.Get, StringComparison.OrdinalIgnoreCase)
                        || apiOperationAttribute.HttpMethod.Equals(HttpConstants.HttpMethod.Delete, StringComparison.OrdinalIgnoreCase)))
                        {
                            bodyBuilder.Append("?");

                            foreach (var parameterItem in parameters)
                            {
                                bodyBuilder.AppendFormat("{0}=", parameterItem.Name);
                                FillSampleValue(bodyBuilder, parameterItem.ParameterType, enumSets, 0, parameterItem.Name, true, true);
                                bodyBuilder.Append("&");
                            }

                            bodyBuilder.RemoveLastIfMatch('&', true);
                            parameterIsHandled = true;
                        }

                        bodyBuilder.Append("</url>");

                        var currentTokenRequiredAttribute = one.GetCustomAttribute<TokenRequiredAttribute>(true) ?? classTokenRequiredAttribute;
                        if (currentTokenRequiredAttribute != null && currentTokenRequiredAttribute.TokenRequired)
                        {
                            bodyBuilder.AppendLineWithFormat(requestHeaderFormat, TokenKey, "[YourTokenValue]");
                        }

                        if (entitySynchronizationAttribute != null)
                        {
                            bodyBuilder.AppendLineWithFormat(requestHeaderFormat, entitySynchronizationAttribute.IfModifiedSinceKey, DateTime.UtcNow.AddDays(-2).ToFullDateTimeString());
                        }

                        bodyBuilder.Append("<pre class=\"CodeContainer\" style=\"font-family: monospace;font-size:14px;\">");

                        if (!parameterIsHandled)
                        {
                            if (parameters.Length == 1)
                            {
                                FillSampleValue(bodyBuilder, parameters[0].ParameterType, enumSets, 0, fieldName: parameters[0].Name, followingProperty: false);
                            }
                            else
                            {
                                builder.AppendFormat(objectBrace, "{");

                                foreach (var parameterItem in parameters)
                                {
                                    FillProperty(bodyBuilder, parameterItem.Name, parameterItem.ParameterType);
                                    AppendColon(bodyBuilder);
                                    FillSampleValue(bodyBuilder, parameterItem.ParameterType, enumSets, 1, followingProperty: true);

                                    AppendComma(bodyBuilder);
                                }

                                RemoveUnnecessaryColon(bodyBuilder);
                                bodyBuilder.AppendFormat(objectBrace, "}");
                            }
                        }

                        bodyBuilder.Append("</pre>");

                        #endregion Request

                        #region Response

                        bodyBuilder.Append("<h3>Response</h3><hr />");

                        bodyBuilder.AppendLineWithFormat(requestHeaderFormat, HttpConstants.HttpHeader.ContentType, apiOperationAttribute.ContentType.SafeToString(HttpConstants.ContentType.Json));

                        if (entitySynchronizationAttribute != null)
                        {
                            bodyBuilder.AppendLineWithFormat(requestHeaderFormat, entitySynchronizationAttribute.LastModifiedKey, DateTime.UtcNow.ToFullDateTimeString());
                        }

                        bodyBuilder.Append("<pre class=\"CodeContainer\" style=\"font-family: monospace;font-size:14px;\">");

                        var returnType = one.ReturnType;
                        if (isAsync)
                        {
                            returnType = returnType.GetTaskUnderlyingType() ?? returnType;
                        }

                        if (returnType.IsVoid() ?? true)
                        {
                            bodyBuilder.Append("<span style=\"font-style:italic; font-weight: bold; color: #999999;\">void</span>");
                        }
                        else
                        {
                            FillSampleValue(bodyBuilder, returnType, enumSets, 0);
                        }

                        bodyBuilder.Append("</pre>");

                        #endregion Response

                        #region Http Status

                        // Http Status
                        bodyBuilder.Append("<h3>Http Status &amp; Exceptions</h3><hr />");
                        bodyBuilder.Append("<ul>");

                        if (one.ReturnType.IsVoid() ?? false)
                        {
                            bodyBuilder.AppendFormat(httpStatusFormat, (int)HttpStatusCode.NoContent, HttpStatusCode.NoContent.ToString(), "If no error or exception.", "green");
                        }
                        else
                        {
                            bodyBuilder.AppendFormat(httpStatusFormat, (int)HttpStatusCode.OK, HttpStatusCode.OK.ToString(), "If no error or exception.", "green");

                            if (entitySynchronizationAttribute != null)
                            {
                                bodyBuilder.AppendFormat(httpStatusFormat, (int)HttpStatusCode.NotModified, HttpStatusCode.NotModified.ToString(), "If no modified since specific time stamp", "green");
                            }
                        }

                        bodyBuilder.AppendFormat(httpStatusFormat, (int)HttpStatusCode.BadRequest, HttpStatusCode.BadRequest.ToString(), "If input value or/and format is invalid.", "red");

                        if (currentTokenRequiredAttribute != null && currentTokenRequiredAttribute.TokenRequired)
                        {
                            bodyBuilder.AppendFormat(httpStatusFormat, (int)HttpStatusCode.Unauthorized, HttpStatusCode.Unauthorized.ToString(), "If token is invalid or not given.", "orange");
                        }

                        bodyBuilder.AppendFormat(httpStatusFormat, (int)HttpStatusCode.Forbidden, HttpStatusCode.Forbidden.ToString(), "If action is forbidden.", "orange");
                        bodyBuilder.AppendFormat(httpStatusFormat, (int)HttpStatusCode.NotFound, HttpStatusCode.NotFound.ToString(), "If resource is not found.", "orange");
                        bodyBuilder.AppendFormat(httpStatusFormat, (int)HttpStatusCode.Conflict, HttpStatusCode.Conflict.ToString(), "If data conflicts.", "orange");
                        bodyBuilder.AppendFormat(httpStatusFormat, (int)HttpStatusCode.InternalServerError, HttpStatusCode.InternalServerError.ToString(), "If server feature is not working or has defect(s).", "red");
                        bodyBuilder.AppendFormat(httpStatusFormat, (int)HttpStatusCode.NotImplemented, HttpStatusCode.NotImplemented.ToString(), "If server feature is not implemented yet.", "red");
                        bodyBuilder.Append("</ul>");

                        #endregion Http Status

                        builder.AppendFormat(panel, one.Name, bodyBuilder.ToString(), one.Name, "#");
                    }
                }
            }
        }

        /// <summary>
        /// Fills the sample value.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="type">The type.</param>
        /// <param name="enumSets">The enum sets.</param>
        /// <param name="indent">The indent.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="ignoreQuote">if set to <c>true</c> [ignore quote].</param>
        /// <param name="followingProperty">if set to <c>true</c> [following property].</param>
        /// <param name="objectChain">The object chain.</param>
        public static void FillSampleValue(StringBuilder builder, Type type, HashSet<Type> enumSets, int indent, string fieldName = null, bool ignoreQuote = false, bool followingProperty = false, List<Type> objectChain = null)
        {
            if (builder != null && type != null && enumSets != null)
            {
                if (objectChain == null)
                {
                    objectChain = new List<Type>();
                }
                else if (OverThreshold(objectChain, type))
                {
                    builder.Append("<span class=\"Null\" style=\"color: #0000FF;font-style:italic; font-weight:bold;\">null</span>");
                    return;
                }

                if (type.IsNullable())
                {
                    FillSampleValue(builder, type.GetNullableType(), enumSets, indent, fieldName, ignoreQuote, followingProperty, AppendChain(objectChain, type));
                    return;
                }

                if (type.IsEnum)
                {
                    var firstEnum = Enum.GetValues(type).FirstOrDefault<object>();
                    enumSets.Add(type);
                    if (!followingProperty)
                    {
                        builder.AppendIndent(indent);
                    }
                    builder.AppendFormat(enumFormat, (firstEnum == null) ? string.Empty : ((int)firstEnum).ToString(), type.FullName);
                }
                else if (type == typeof(JObject))
                {
                    builder.Append("<span class=\"JSON\" style=\"font-style:italic; font-weight:bold;color:#3abd4d;\" title=\"Any JObject\">{ Any JObject }</span>");
                }
                else if (type == typeof(JArray))
                {
                    builder.Append("<span class=\"JSON\" style=\"font-style:italic; font-weight:bold;color:#3abd4d;\" title=\"Any JArray\">[ Any JArray ]</span>");
                }
                else if (type == typeof(JToken))
                {
                    builder.Append("<span class=\"JSON\" style=\"font-style:italic; font-weight:bold;color:#3abd4d;\" title=\"Any JToken\">{ Any JSON }</span>");
                }
                else if (type.IsDictionary())
                {
                    // If dictionary type is inherited from dictionary (native), the generic might be totally different.
                    if (type.BaseType.IsDictionary())
                    {
                        FillSampleValue(builder, type.BaseType, enumSets, indent, fieldName, ignoreQuote, followingProperty, objectChain);
                    }
                    else
                    {
                        var keyType = type.GetGenericArguments()[0];
                        var valueType = type.GetGenericArguments()[1];

                        if (!followingProperty)
                        {
                            builder.AppendIndent(indent);
                        }

                        builder.AppendLineWithFormat(objectBrace, "{");

                        FillSampleValue(builder, keyType, enumSets, indent + 1, fieldName: "Key", followingProperty: false, objectChain: AppendChain(objectChain, type));
                        AppendColon(builder);
                        FillSampleValue(builder, valueType, enumSets, indent + 1, fieldName: "Value", followingProperty: true, objectChain: AppendChain(objectChain, type));
                        builder.AppendLine();

                        builder.AppendIndent(indent);
                        builder.AppendLineWithFormat(objectBrace, "}");
                    }
                }
                else if (type.IsCollection())
                {
                    var subType = type.GetGenericArguments().FirstOrDefault();

                    if (!followingProperty)
                    {
                        builder.AppendIndent(indent);
                    }
                    builder.AppendLineWithFormat(arrayBrace, "[");

                    if (!OverThreshold(objectChain, subType))
                    {
                        FillSampleValue(builder, subType, enumSets, indent + 1, "Item1", followingProperty: false, objectChain: AppendChain(objectChain, type));
                        builder.AppendIndent(indent + 1);
                        AppendComma(builder);
                        FillSampleValue(builder, subType, enumSets, indent + 1, "Item2", followingProperty: false, objectChain: AppendChain(objectChain, type));

                        builder.AppendLine();
                    }

                    builder.AppendIndent(indent);
                    builder.AppendLineWithFormat(arrayBrace, "]");
                }
                else
                {
                    switch (type.FullName)
                    {
                        case "System.Boolean":
                            if (!followingProperty)
                            {
                                builder.AppendIndent(indent);
                            }
                            builder.Append("<span class=\"Boolean\">true</span>");
                            break;

                        case "System.String":
                            if (!followingProperty)
                            {
                                builder.AppendIndent(indent);
                            }
                            fieldName = string.IsNullOrWhiteSpace(fieldName) ? "AnyString" : fieldName + "String";
                            builder.AppendFormat(ignoreQuote ? "{0}" : "\"{0}\"", ignoreQuote ? fieldName : fieldName.SplitSentenceByUpperCases());
                            break;

                        case "System.Guid":
                            if (!followingProperty)
                            {
                                builder.AppendIndent(indent);
                            }
                            builder.AppendFormat(ignoreQuote ? "{0}" : "\"{0}\"", Guid.NewGuid());
                            break;

                        case "System.UInt16":
                        case "System.UInt32":
                        case "System.UInt64":
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                            if (!followingProperty)
                            {
                                builder.AppendIndent(indent);
                            }
                            builder.AppendFormat("<span class=\"Number\" style=\"color: #AA00AA;\">123</span>");
                            break;

                        case "System.Single":
                        case "System.Double":
                        case "System.Decimal":
                            if (!followingProperty)
                            {
                                builder.AppendIndent(indent);
                            }
                            builder.AppendFormat("<span class=\"Number\" style=\"color: #AA00AA;\">1.5</span>");
                            break;

                        case "System.DateTime":
                            if (!followingProperty)
                            {
                                builder.AppendIndent(indent);
                            }
                            builder.AppendFormat(ignoreQuote ? "{0}" : "\"{0}\"", ignoreQuote ? DateTime.Now.ToFullDateTimeTzString().ToUrlPathEncodedText() : DateTime.Now.ToFullDateTimeTzString());
                            break;

                        case "System.TimeSpan":
                            if (!followingProperty)
                            {
                                builder.AppendIndent(indent);
                            }
                            builder.AppendFormat(ignoreQuote ? "{0}" : "\"{0}\"", ignoreQuote ? new TimeSpan(1234).ToString().ToUrlPathEncodedText() : new TimeSpan(1234).ToString());
                            break;

                        default:
                            if (!followingProperty)
                            {
                                builder.AppendIndent(indent);
                            }
                            builder.AppendLineWithFormat(objectBrace, "{");

                            foreach (var one in type.GetActualAffectedProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty).Where((x) => { return x.GetCustomAttribute<JsonIgnoreAttribute>() == null; }))
                            {
                                var valueType = one.PropertyType;

                                builder.AppendIndent(indent + 1);
                                FillProperty(builder, one.Name, one.PropertyType);

                                AppendColon(builder);

                                FillSampleValue(builder, valueType, enumSets, indent + 1, one.Name, followingProperty: true, objectChain: AppendChain(objectChain, type));
                                AppendComma(builder);
                            }

                            foreach (var one in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField))
                            {
                                var valueType = one.FieldType;

                                builder.AppendIndent(indent + 1);
                                FillProperty(builder, one.Name, one.FieldType);

                                AppendColon(builder);

                                FillSampleValue(builder, valueType, enumSets, 0, one.Name, followingProperty: true, objectChain: AppendChain(objectChain, type));
                                AppendComma(builder);
                            }

                            RemoveUnnecessaryColon(builder);

                            builder.AppendIndent(indent);
                            builder.AppendLineWithFormat(objectBrace, "}");

                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Writes as entire HTML file.
        /// </summary>
        /// <param name="bodyContent">Content of the body.</param>
        /// <param name="title">The title.</param>
        /// <returns>System.String.</returns>
        protected string WriteAsEntireHtmlFile(string bodyContent, string title)
        {
            StringBuilder builder = new StringBuilder(bodyContent?.Length ?? 0 + 512);

            builder.AppendLine("<html><head>");
            builder.Append("<title>");
            builder.AppendLine(title);
            builder.AppendLine("</title>");
            builder.AppendLine("<meta name=\"author\" content=\"https://github.com/rynnwang/commonsolution/\" />");

            //builder.AppendLine("<link rel=\"stylesheet\" href=\"bootstrap.css\">");

            builder.AppendLine("<style>");
            builder.AppendLine(jsonCss);
            builder.AppendLine("</style>");

            builder.AppendLine("<script src=\"jquery.min.js\"></script>");
            //builder.AppendLine("<script src=\"bootstrap.min.js\"></script>");

            builder.AppendLine("<script>");
            builder.AppendLine("</script>");

            builder.AppendLine("</head><body style=\"font-family: Calibri;font-size:14px;\">");
            builder.AppendLine("<div class=\"row\"><div class=\" col-sm-12\">");
            builder.Append(bodyContent);
            builder.AppendLine("</div></div>");
            builder.AppendLine("</body></html>");

            return builder.ToString();
        }

        /// <summary>
        /// Removes the unnecessary colon.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private static void RemoveUnnecessaryColon(StringBuilder builder)
        {
            builder.RemoveLastIfMatch(StringConstants.CommaChar, true);
            builder.AppendLine();
        }

        /// <summary>
        /// Fills the property.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyType">Type of the property.</param>
        private static void FillProperty(StringBuilder builder, string propertyName, Type propertyType)
        {
            var nullable = propertyType.IsFieldNullable();
            var title = string.Format("C# Type: {0}, {1}", propertyType.ToCodeLook(), nullable ? "Nullable" : "Not Null");
            builder.AppendFormat(propertyNameFormat, propertyName, title, nullable ? "nullable" : string.Empty);
        }

        /// <summary>
        /// Appends the comma.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private static void AppendComma(StringBuilder builder)
        {
            builder.TrimEnd();
            builder.AppendLine(", ");
        }

        /// <summary>
        /// Appends the colon.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private static void AppendColon(StringBuilder builder)
        {
            builder.Append(": ");
        }

        /// <summary>
        /// Appends the chain.
        /// </summary>
        /// <param name="chain">The chain.</param>
        /// <param name="type">The type.</param>
        /// <returns>List&lt;Type&gt;.</returns>
        private static List<Type> AppendChain(List<Type> chain, Type type)
        {
            var list = new List<Type>(chain);
            list.Add(type);

            return list;
        }

        /// <summary>
        /// Gets the API operation identifier.
        /// </summary>
        /// <param name="apiOperation">The API operation.</param>
        /// <returns>System.String.</returns>
        private static string GetApiOperationIdentifier(ApiOperationAttribute apiOperation)
        {
            return apiOperation == null ? string.Empty : string.Format("{0}/{1}/{2}", apiOperation.HttpMethod, apiOperation.ResourceName, apiOperation.Action);
        }

        /// <summary>
        /// Overs the threshold.
        /// </summary>
        /// <param name="chain">The chain.</param>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if it is over threshold, <c>false</c> otherwise.</returns>
        private static bool OverThreshold(List<Type> chain, Type type)
        {
            const int threshold = 2;
            int total = 0;

            if (chain != null && type != null && chain.Count > 0)
            {
                foreach (var one in chain)
                {
                    if (one == type)
                    {
                        total++;
                    }
                }
            }

            return total > threshold;
        }
    }
}