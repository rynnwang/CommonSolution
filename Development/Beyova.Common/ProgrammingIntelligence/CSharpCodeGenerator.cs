﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Beyova;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Class CSharpCodeGenerator.
    /// </summary>
    public abstract class CSharpCodeGenerator
    {
        /// <summary>
        /// The code indent
        /// </summary>
        protected const string defaultCodeIndent = "    ";

        /// <summary>
        /// The null string
        /// </summary>
        protected const string nullString = "null";

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
        /// Gets or sets the type of the base class.
        /// </summary>
        /// <value>
        /// The type of the base class.
        /// </value>
        public Type BaseClassType { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpCodeGenerator" /> class.
        /// </summary>
        /// <param name="namespace">The namespace.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="baseClassType">Type of the base class.</param>
        /// <param name="codeIndent">The code indent.</param>
        public CSharpCodeGenerator(string @namespace, string className, Type baseClassType, string codeIndent = null)
        {
            Namespace = @namespace.SafeToString("Beyova");
            ClassName = className.SafeToString("TmpClass");
            BaseClassType = baseClassType ?? this.GetType();
            CodeIndent = codeIndent.SafeToString(defaultCodeIndent);
        }

        /// <summary>
        /// Writes the file information.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected virtual void WriteFileInfo(StringBuilder builder)
        {
            CSharpCodeGenerateUtil.WriteProgrammingIntelligenceFileDescription(builder);
        }

        /// <summary>
        /// Writes the namespaces.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected virtual void WriteNamespaces(StringBuilder builder)
        {
            if (builder != null)
            {
                builder.AppendLine("using System;");
                builder.AppendLine("using System.Collections.Generic;");
                builder.AppendLine("using System.Linq;");
                builder.AppendLine("using System.Text;");
                builder.AppendLine("using Beyova.ProgrammingIntelligence;");
                builder.AppendLine("using Beyova.ExceptionSystem;");
                builder.AppendLine("using Beyova;");
                builder.AppendLine("using Newtonsoft.Json;");
            }
        }

        /// <summary>
        /// Writes the constructor.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="className">Name of the class.</param>
        protected virtual void WriteConstructor(StringBuilder builder, string className)
        {
            if (builder != null)
            {
                builder.AppendIndent(2);
                builder.AppendLineWithFormat("public {0}():base()", className);
                builder.AppendIndent(2);
                builder.AppendLine("{");
                builder.AppendIndent(2);
                builder.AppendLine("}");
                builder.AppendLine();
            }
        }

        /// <summary>
        /// Generates the class declaration part.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <param name="interfaces">The interfaces.</param>
        /// <returns></returns>
        protected virtual string GenerateClassDeclarationPart(string className, ICollection<Type> interfaces)
        {
            StringBuilder builder = new StringBuilder("public class ", 512);
            SequencedKeyDictionary<string, List<Type>> genericTypes = new SequencedKeyDictionary<string, List<Type>>();

            interfaces = interfaces ?? new Collection<Type>();

            foreach (var one in interfaces)
            {
                var genericParameterConstraints = GetGenericTypeConstraints(one);
                if (genericParameterConstraints.HasItem())
                {
                    foreach (var g in genericParameterConstraints)
                    {
                        genericTypes.Merge(g.Key, g.Value.ToList());
                    }
                }
            }

            builder.Append(className);

            if (genericTypes.HasItem())
            {
                builder.Append("<");
                builder.Append(CSharpCodeGenerateUtil.CombineCode(genericTypes, x => x.Key, 16, StringConstants.CommaChar));
                builder.Append(">");
            }

            if (this.BaseClassType != null)
            {
                builder.Append(": ");
                builder.Append(this.BaseClassType.ToCodeLook());
            }

            builder.AppendLine();

            if (interfaces.HasItem())
            {
                builder.Append(",");
                builder.AppendLine(CSharpCodeGenerateUtil.CombineCode(interfaces, x => x.ToCodeLook(), 32, StringConstants.CommaChar));
                builder.AppendLine();
            }

            // Add generic constraints
            foreach (var constraint in genericTypes)
            {
                CSharpCodeGenerateUtil.InternalWriteGenericConstraintsCodeLook(builder, constraint.Key, constraint.Value, new string(CodeIndent[0], CodeIndent.Length * 3));
                builder.AppendLine();
            }

            return builder.ToString();
        }

        #region Util

        /// <summary>
        /// Determines whether [is duplicated interface] [the specified type].
        /// </summary>
        /// <param name="handledInterfaces">The handled interfaces.</param>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is duplicated interface] [the specified type]; otherwise, <c>false</c>.</returns>
        protected bool IsDuplicatedInterface(HashSet<Type> handledInterfaces, Type type)
        {
            if (handledInterfaces == null || type == null || !type.IsInterface)
            {
                return false;
            }

            if (!handledInterfaces.Contains(type))
            {
                handledInterfaces.Add(type);
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
                return string.Format("{0}.HasValue ? {1} : null", name, SimpleVariableToStringCode(name, type.GetNullableType()));
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

        ///// <summary>
        ///// Finds the matched generic constraints.
        ///// </summary>
        ///// <param name="methodInfo">The method information.</param>
        ///// <param name="genericParameterConstraints">The generic parameter constraints.</param>
        ///// <returns>Dictionary&lt;System.String, Type[]&gt;.</returns>
        //protected static Dictionary<string, Type[]> FindMatchedGenericConstraints(MethodInfo methodInfo, Dictionary<string, Type[]> genericParameterConstraints)
        //{
        //    Dictionary<string, Type[]> result = null;

        //    if (methodInfo.IsGenericMethod && genericParameterConstraints != null)
        //    {
        //        result = new Dictionary<string, Type[]>();
        //        foreach (var one in methodInfo.GetParameters())
        //        {
        //            if (one.ParameterType.IsGenericParameter && genericParameterConstraints.ContainsKey(one.ParameterType.Name))
        //            {
        //                result.Add(one.Name, genericParameterConstraints[one.ParameterType.Name]);
        //            }
        //        }
        //    }

        //    return result;
        //}

        #endregion Util
    }
}