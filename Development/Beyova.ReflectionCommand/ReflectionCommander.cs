using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Beyova;
using Beyova.ExceptionSystem;

namespace ifunction.ReflectionCommand
{
    /// <summary>
    ///     Class ReflectionCommander. This class cannot be inherited.
    /// </summary>
    public sealed class ReflectionCommander
    {
        /// <summary>
        ///     The regex string_ parameter expression
        /// </summary>
        private const string regexString_ParameterExpression =
            @"(\s+)(?<Name>([a-zA-Z0-9_]+))=(?<Value>(([^\n\t\s\r])+))";

        /// <summary>
        ///     The regex string_ command line
        /// </summary>
        private const string regexString_CommandLine = @"(?<CommandKey>([a-zA-Z0-9_]+))";

        /// <summary>
        ///     The instance
        /// </summary>
        private static readonly ReflectionCommander instance = new ReflectionCommander();

        #region Property

        /// <summary>
        ///     The command mappings
        /// </summary>
        private readonly Dictionary<string, MethodInfo> commandMappings =
            new Dictionary<string, MethodInfo>(StringComparer.InvariantCultureIgnoreCase);

        #endregion

        /// <summary>
        ///     The regex_ command line
        /// </summary>
        private readonly Regex regex_CommandLine = new Regex(regexString_CommandLine,
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        ///     The regex_ parameter expression
        /// </summary>
        private readonly Regex regex_ParameterExpression = new Regex(regexString_ParameterExpression,
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReflectionCommander" /> class.
        /// </summary>
        public ReflectionCommander()
        {
            InitializeCommandMappings();
        }

        #endregion

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ReflectionCommander Instance
        {
            get { return instance; }
        }

        #region Public methods

        /// <summary>
        ///     Executes the specified command line.
        /// </summary>
        /// <param name="commandLine">The command line.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.InvalidOperationException">
        ///     Invalid command line.
        ///     or
        ///     Failed to execute command.
        /// </exception>
        public object Execute(string commandLine)
        {
            object result = string.Empty;

            try
            {
                string commandKey = null;

                commandLine = commandLine.SafeToString().Trim();
                var match = regex_CommandLine.Match(commandLine);

                if (match != null && match.Success)
                {
                    commandKey = match.Result("${CommandKey}");

                    if (!string.IsNullOrWhiteSpace(commandKey))
                    {
                        var parameterString = commandLine.Substring(commandKey.Length);

                        //// Don't need to trim the space for the first parameter
                        result = Execute(commandKey, parameterString);
                    }
                }

                if (result == null)
                {
                    throw new InvalidOperationException("Invalid command line.");
                }
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to execute command.", ex);
            }

            return result;
        }

        #endregion

        #region Initialize methods

        /// <summary>
        ///     Initializes the command mappings.
        /// </summary>
        private void InitializeCommandMappings()
        {
            var configurationFile = Path.Combine(this.GetApplicationBaseDirectory(), "ReflectionCommand.xml");

            XDocument xDocument = null;
            commandMappings.Clear();

            try
            {
                xDocument = XDocument.Load(configurationFile);
                if (xDocument.Root.Name.LocalName == "Commands")
                {
                    foreach (var section in xDocument.Root.Elements("StaticType"))
                    {
                        FillCommandMappings(commandMappings, section);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new OperationFailureException("InitializeCommandMappings", ex);
            }
        }

        /// <summary>
        ///     Fills the command mappings.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="staticType">Type of the static.</param>
        /// <param name="xmlRow">The XML row.</param>
        private void FillCommandMappings(Dictionary<string, MethodInfo> container, Type staticType, XElement xmlRow)
        {
            //// <Item Key="" FullName="" />

            if (staticType != null && container != null && xmlRow != null)
            {
                var key = xmlRow.GetAttributeValue("Key");
                var fullName = xmlRow.GetAttributeValue("FullName");

                var methodInfo = staticType.GetMethod(fullName);

                if (methodInfo != null && methodInfo.IsStatic && !string.IsNullOrWhiteSpace(key))
                {
                    container.Merge(key, methodInfo);
                }
            }
        }

        /// <summary>
        ///     Fills the command mappings.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="section">The section.</param>
        private void FillCommandMappings(Dictionary<string, MethodInfo> container, XElement section)
        {
            ////  <StaticType FullName="">
            ////    <Item Key="" FullName="" />
            //// </StaticType>

            if (section != null && container != null && section.Name.LocalName.Equals("StaticType"))
            {
                var type = ReflectionExtension.SmartGetType(section.GetAttributeValue("FullName"));

                if (type != null && section.HasElements())
                {
                    foreach (var one in section.Elements("Item"))
                    {
                        FillCommandMappings(container, type, one);
                    }
                }
            }
        }

        #endregion

        #region Invoke methods

        /// <summary>
        ///     Executes the specified command key.
        /// </summary>
        /// <param name="commandKey">The command key.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.InvalidCastException">
        /// </exception>
        private object Execute(string commandKey, string parameters)
        {
            object result = null;

            try
            {
                if (commandMappings.ContainsKey(commandKey))
                {
                    var methodInfo = commandMappings[commandKey];
                    var parameterArray = PrepareMethodParameters(methodInfo, parameters);

                    result = InvokeMethod(methodInfo, parameterArray);
                }
                else
                {
                    throw new InvalidCastException(string.Format("Failed to find command: [{0}]",
                        commandKey.SafeToString()));
                }
            }
            catch (InvalidCastException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(
                    string.Format("Failed to execute command: [{0}]", commandKey.SafeToString()), ex);
            }

            return result;
        }

        /// <summary>
        ///     Invokes the method.
        ///     This can be applied on static methods only.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>System.Object.</returns>
        private object InvokeMethod(MethodInfo methodInfo, params object[] arguments)
        {
            try
            {
                methodInfo.CheckNullObject("methodInfo");

                return methodInfo.Invoke(null, arguments);
            }
            catch (Exception ex)
            {
                throw new OperationFailureException("InvokeMethod", ex);
            }
        }

        /// <summary>
        ///     Prepares the method parameters.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="parameterString">The parameter string.</param>
        /// <returns>System.Object[][].</returns>
        private object[] PrepareMethodParameters(MethodInfo methodInfo, string parameterString)
        {
            List<object> result = null;

            if (methodInfo != null)
            {
                var parameters = GetParameterKeyValuePairs(parameterString);

                var methodParameters = methodInfo.GetParameters();

                if (parameters != null && methodParameters != null && methodParameters.Length > 0)
                {
                    result = new List<object>();

                    foreach (var one in methodParameters)
                    {
                        if (parameters.ContainsKey(one.Name))
                        {
                            var parameterType = one.ParameterType;

                            var parameterValue = ReflectionExtension.ConvertToObjectByType(parameterType,
                                parameters[one.Name]);
                            result.Add(parameterValue);
                        }
                        else
                        {
                            result.Add(one.DefaultValue);
                        }
                    }
                }
            }

            return result == null ? null : result.ToArray();
        }

        /// <summary>
        ///     Gets the parameter key value pairs.
        /// </summary>
        /// <param name="parameterString">The parameter string.</param>
        /// <returns>Dictionary{System.StringSystem.String}.</returns>
        private Dictionary<string, string> GetParameterKeyValuePairs(string parameterString)
        {
            var result = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(parameterString))
            {
                var matches = regex_ParameterExpression.Matches(parameterString);

                if (matches != null && matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        var name = match.Result("${Name}");
                        var value = match.Result("${Value}");
                        result.Merge(name, value);
                    }
                }
            }

            return result;
        }

        #endregion
    }
}