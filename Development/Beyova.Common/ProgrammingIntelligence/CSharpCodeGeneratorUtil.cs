using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class CSharpCodeGenerateUtil
    /// </summary>
    public static class CSharpCodeGenerateUtil
    {
        /// <summary>
        /// Appends the begin brace.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void AppendBeginBrace(this StringBuilder builder)
        {
            if (builder != null)
            {
                builder.Append("{");
            }
        }

        /// <summary>
        /// Appends the brace begin.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="indent">The indent.</param>
        public static void AppendBraceBegin(this StringBuilder builder, ref int indent)
        {
            if (builder != null)
            {
                builder.AppendIndent(indent);
                builder.AppendLine("{");
                indent++;
            }
        }

        /// <summary>
        /// Appends the end brace.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void AppendEndBrace(this StringBuilder builder)
        {
            if (builder != null)
            {
                builder.Append("}");
            }
        }

        /// <summary>
        /// Appends the brace end.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="indent">The indent.</param>
        /// <param name="appendStatementEnd">if set to <c>true</c> [append statement end].</param>
        public static void AppendBraceEnd(this StringBuilder builder, ref int indent, bool appendStatementEnd = false)
        {
            if (builder != null)
            {
                indent--;
                builder.AppendIndent(indent);
                builder.AppendLine(appendStatementEnd ? "};" : "}");                
            }
        }

        /// <summary>
        /// Methods the input parameters to code look.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="includeType">Type of the include.</param>
        /// <returns>System.String.</returns>
        public static string MethodInputParametersToCodeLook(this MethodInfo methodInfo, bool includeType = true)
        {
            Func<ParameterInfo, string> func;
            if (includeType)
            {
                func = x => { return string.Format("{0} {1}", x.ParameterType.ToCodeLook(), x.Name); };
            }
            else
            {
                func = x => { return string.Format("{0}", x.Name); };
            }

            return CombineCode(methodInfo.GetParameters(),
                func,
                16);
        }

        #region ToCodeLook

        /// <summary>
        /// To the generic constraints code look.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="indent">The indent.</param>
        /// <returns></returns>
#if DEBUG
        public
#else

        internal
# endif
            static string ToGenericConstraintsCodeLook(this Type type, string indent = null)
        {
            return type.IsGenericType ? InternalToGenericConstraintsCodeLook(type.GetGenericArguments(), indent) : string.Empty;
        }

        /// <summary>
        /// To the generic constraints code look.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="indent">The indent.</param>
        /// <returns></returns>
        public static string ToGenericConstraintsCodeLook(this MethodInfo methodInfo, string indent = null)
        {
            return methodInfo.IsGenericMethod ? InternalToGenericConstraintsCodeLook(methodInfo.GetGenericArguments(), indent) : string.Empty;
        }

        /// <summary>
        /// To the generic constraints code look. Output: "where TName: struct, A, B, C, D, ..."
        /// </summary>
        /// <param name="genericTypes">The generic types.</param>
        /// <param name="indent">The indent.</param>
        /// <returns></returns>
        private static string InternalToGenericConstraintsCodeLook(Type[] genericTypes, string indent)
        {
            if (genericTypes.HasItem())
            {
                StringBuilder builder = new StringBuilder((128 + indent.Length) * genericTypes.Length);

                foreach (var one in genericTypes)
                {
                    if (one.IsGenericParameter)
                    {
                        var constraints = one.GetGenericParameterConstraints();
                        InternalWriteGenericConstraintsCodeLook(builder, one.Name, constraints.ToList(), indent);
                        builder.AppendLine();
                    }
                }

                return builder.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Internals to generic constraints code look.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="genericName">Name of the generic.</param>
        /// <param name="constraintTypes">The constraint types.</param>
        /// <param name="indent">The indent.</param>
        internal static void InternalWriteGenericConstraintsCodeLook(StringBuilder builder, string genericName, List<Type> constraintTypes, string indent)
        {
            if (constraintTypes.HasItem())
            {
                builder.Append(indent);
                builder.AppendFormat("where {0}: ", genericName);

                var structType = constraintTypes.FindAndRemove(typeof(ValueType), (t, s) => { return t == s; });

                if (structType != null)
                {
                    builder.Append("struct,");
                }

                foreach (var g in constraintTypes)
                {
                    builder.AppendFormat("{0},", g.ToCodeLook());
                }

                builder.RemoveLastIfMatch(StringConstants.CommaChar, true);
            }
        }

        /// <summary>
        /// To the code look. Output looks like: System.Int32 NormalMethod(System.Nullable&lt;System.Int32&gt; x,System.Int32 y)
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        public static string ToDeclarationCodeLook(this MethodInfo methodInfo)
        {
            if (methodInfo != null)
            {
                if (methodInfo.IsGenericMethod)
                {
                    return string.Format("{0} {1}<{2}>({3})",
                        methodInfo.ReturnType.ToCodeLook(),
                        methodInfo.Name,
                        methodInfo.MethodGenericDefinitionArgumentsToCodeLook(),
                        methodInfo.MethodInputParametersToCodeLook());
                }
                else
                {
                    return string.Format("{0} {1}({2})", methodInfo.ReturnType.ToCodeLook(), methodInfo.Name, methodInfo.MethodInputParametersToCodeLook());
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// To the invoke code look.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="genericTypes">The generic types.</param>
        /// <param name="inputParameters">The input parameters.</param>
        /// <returns></returns>
#if DEBUG
        public
#else

        internal
#endif
            static string ToInvokeCodeLook(this MethodInfo methodInfo, Type[] genericTypes, ParameterInfo[] inputParameters)
        {
            return ToInvokeCodeLook(methodInfo, genericTypes, inputParameters.Select(x => x.Name).ToArray());
        }

#if DEBUG
        public
#else

        internal
#endif
            static string ToInvokeCodeLook(this MethodInfo methodInfo, Type[] genericTypes, string[] inputParameterNames)
        {
            if (methodInfo != null)
            {
                if (methodInfo.IsGenericMethod)
                {
                    if (methodInfo.GetGenericArguments().Length != genericTypes.Length)
                    {
                        throw ExceptionFactory.CreateInvalidObjectException(nameof(genericTypes), new { count = genericTypes.Length }, "CountNotMatch");
                    }

                    return string.Format("{0}<{1}>({2})", methodInfo.Name, methodInfo.MethodGenericArgumentsToCodeLook(genericTypes), VariableParametersToCodeLook(inputParameterNames));
                }
                else
                {
                    return string.Format("{0}({1})", methodInfo.Name, methodInfo.MethodInputParametersToCodeLook(false));
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// To the code look. This method is to convert <see cref="Type" /> to code based., such as List&lt;String&gt;, System.Nullable&lt;System.Guid&gt;, etc.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="omitNamespace">if set to <c>true</c> [omit namespace].</param>
        /// <returns>
        /// System.String.
        /// </returns>
        public static string ToCodeLook(this Type type, bool omitNamespace = false)
        {
            const string separator = ".";
            string result = string.Empty;

            if (type != null)
            {
                try
                {
                    if (type == typeof(void))
                    {
                        return "void";
                    }
                    else if (type.IsGenericParameter)
                    {
                        return type.Name;
                    }

                    if (type.IsGenericType)
                    {
                        var builder = new StringBuilder();
                        foreach (var t in type.GetGenericArguments())
                        {
                            builder.Append(t.ToCodeLook(omitNamespace) + ",");
                        }
                        if (builder.Length > 0)
                        {
                            builder.RemoveLast();
                        }
                        result = omitNamespace ?
                                string.Format("{0}<{1}>", type.Name.SubStringBeforeFirstMatch('`'), builder) :
                                string.Format("{0}{1}{2}<{3}>", type.Namespace, string.IsNullOrWhiteSpace(type.Namespace) ? string.Empty : separator, type.Name.SubStringBeforeFirstMatch('`'), builder);
                    }
                    else
                    {
                        // NOTE: if type is come from generic method, like: T1 Method(T2 t), FullName would be null.
                        result = omitNamespace ? type.Name : string.Format("{0}{1}{2}", type.Namespace, string.IsNullOrWhiteSpace(type.Namespace) ? string.Empty : separator, type.Name);
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { type = type?.Name, omitNamespace, separator });
                }
            }

            return result;
        }

        #endregion ToCodeLook

        /// <summary>
        /// Methods the generic arguments to code look.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <returns></returns>
        private static string MethodGenericDefinitionArgumentsToCodeLook(this MethodInfo methodInfo)
        {
            if (methodInfo == null || !methodInfo.IsGenericMethod)
            {
                return string.Empty;
            }

            var arguments = methodInfo.GetGenericArguments();
            return CombineCode(arguments, x => { return x.Name; }, 16, StringConstants.CommaChar);
        }

        /// <summary>
        /// Methods the generic arguments to code look.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="genericTypes">The generic types.</param>
        /// <returns></returns>
        private static string MethodGenericArgumentsToCodeLook(this MethodInfo methodInfo, params Type[] genericTypes)
        {
            if (methodInfo == null || !methodInfo.IsGenericMethod)
            {
                return string.Empty;
            }

            var arguments = methodInfo.GetGenericArguments();

            if (arguments.Length != genericTypes.Length)
            {
                throw ExceptionFactory.CreateInvalidObjectException(nameof(genericTypes), reason: "GenericCountNotMatch");
            }

            return CombineCode(arguments, x => { return x.ToCodeLook(); }, 16, StringConstants.CommaChar);
        }

        /// <summary>
        /// Variables the parameters to code look. [a,b,c, ...] =&gt; "a,b,c,..."
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        private static string VariableParametersToCodeLook(params string[] parameters)
        {
            return CombineCode(parameters, x => { return x; }, 16, StringConstants.CommaChar);
        }

        /// <summary>
        /// Combines the code. [A,B,C,...]=&gt;"a,b,c,..."
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="getCodePiece">The get code piece.</param>
        /// <param name="estimatedEachLength">Length of the estimated each.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        internal static string CombineCode<T>(IEnumerable<T> items, Func<T, string> getCodePiece, int estimatedEachLength, char separator = StringConstants.CommaChar)
        {
            if (items.HasItem() && getCodePiece != null)
            {
                if (items.Count() > 1)
                {
                    if (estimatedEachLength < 1)
                    {
                        estimatedEachLength = 32;
                    }
                    StringBuilder builder = new StringBuilder(items.Count() * estimatedEachLength);

                    foreach (var one in items)
                    {
                        builder.Append(getCodePiece(one));
                        builder.Append(separator);
                    }
                    builder.RemoveLastIfMatch(separator);

                    return builder.ToString();
                }
                else
                {
                    return getCodePiece(items.First());
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Usings the namespaces.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="namespaceNames">The namespace names.</param>
        public static void UsingNamespaces(StringBuilder builder, IEnumerable<string> namespaceNames)
        {
            if (builder != null && namespaceNames.HasItem())
            {
                foreach (var item in namespaceNames)
                {
                    builder.AppendLineWithFormat("using {0};", item);
                }
            }
        }

        /// <summary>
        /// Writes the programming intelligence file description.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void WriteProgrammingIntelligenceFileDescription(StringBuilder builder)
        {
            if (builder != null)
            {
                builder.AppendIndent('/', 30);
                builder.AppendLine();

                builder.AppendLineWithFormat("// This code is generated by Beyova Programming Intelligence Components.");
                builder.AppendLineWithFormat("// UTC: {0}", DateTime.UtcNow.ToFullDateTimeString());
                builder.AppendLine("// https://github.com/rynnwang/CommonSolution");

                builder.AppendIndent('/', 30);
                builder.AppendLine();
            }
        }

        #region Append Check statement

        /// <summary>
        /// Smarts the append check null or empty statement.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="instanceType">Type of the instance.</param>
        /// <param name="instanceName">Name of the instance.</param>
        /// <param name="indent">The indent.</param>
        public static void SmartAppendCheckNullOrEmptyStatement(this StringBuilder builder, Type instanceType, string instanceName, int indent = 0)
        {
            if (builder != null && instanceType != null && !string.IsNullOrWhiteSpace(instanceName))
            {
                if (instanceType == typeof(string))
                {
                    ApppendCheckEmptyStringStatement(builder, instanceName, indent);
                }
                else if (instanceType.IsCollection())
                {
                    ApppendCheckNullOrEmptyCollectionStatement(builder, instanceName, indent);
                }
                else if (instanceType.IsClass || instanceType.IsNullable())
                {
                    ApppendCheckNullObjectStatement(builder, instanceName, indent);
                }
            }
        }

        /// <summary>
        /// Apppends the check null object statement.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="variableName">Name of the variable.</param>
        /// <param name="indent">The indent.</param>
        public static void ApppendCheckNullObjectStatement(this StringBuilder builder, string variableName, int indent = 0)
        {
            ApppendCheckStatement(builder, "CheckNullObject", variableName, indent);
        }

        /// <summary>
        /// Apppends the check empty string statement.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="variableName">Name of the variable.</param>
        /// <param name="indent">The indent.</param>
        public static void ApppendCheckEmptyStringStatement(this StringBuilder builder, string variableName, int indent = 0)
        {
            ApppendCheckStatement(builder, "CheckEmptyString", variableName, indent);
        }

        /// <summary>
        /// Apppends the check null or empty collection statement.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="variableName">Name of the variable.</param>
        /// <param name="indent">The indent.</param>
        public static void ApppendCheckNullOrEmptyCollectionStatement(this StringBuilder builder, string variableName, int indent = 0)
        {
            ApppendCheckStatement(builder, "CheckNullOrEmptyCollection", variableName, indent);
        }

        /// <summary>
        /// Apppends the check statement.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="variableName">Name of the variable.</param>
        /// <param name="indent">The indent.</param>
        private static void ApppendCheckStatement(this StringBuilder builder, string methodName, string variableName, int indent = 0)
        {
            if (builder != null && !string.IsNullOrWhiteSpace(methodName) && !string.IsNullOrWhiteSpace(variableName))
            {
                builder.AppendIndent(indent);
                builder.AppendLineWithFormat("{0}.{1}(nameof({0}));", variableName, methodName);
            }
        }

        #endregion

        #region AppendThrowExceptionStatement

        /// <summary>
        /// Appends the throw exception statement.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="methodInfo">The method information.</param>
        public static void AppendThrowExceptionStatement(this StringBuilder builder, MethodInfo methodInfo)
        {
            AppendThrowExceptionStatement(builder, methodInfo.GetParameters().Where(x => !x.ParameterType.IsContextful).Select(x => x.Name));
        }

        /// <summary>
        /// Appends the throw exception statement.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="identifiers">The identifiers.</param>
        public static void AppendThrowExceptionStatement(this StringBuilder builder, IEnumerable<string> identifiers)
        {
            if (builder != null)
            {
                builder.AppendLine(identifiers.HasItem() ? "throw ex.Handle(new { " + CombineCode(identifiers, x => x, 16) + " });" : "throw ex.Handle();");
            }
        }

        #endregion
    }
}