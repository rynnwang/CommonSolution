using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Beyova.ProgrammingIntelligence;

namespace Beyova.AOP
{
    /// <summary>
    /// Class AopProxyGenerator.
    /// </summary>
    public class AopProxyGenerator<T> : CSharpCodeGenerator
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AopProxyGenerator{T}" /> class.
        /// </summary>
        /// <param name="namespace">The namespace.</param>
        /// <param name="proxyName">Name of the proxy.</param>
        internal AopProxyGenerator(string @namespace, string proxyName) : base(@namespace, proxyName, typeof(AopProxy<T>))
        {
        }

        /// <summary>
        /// Writes the namespaces.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void WriteNamespaces(StringBuilder builder)
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
                builder.AppendLine("using Newtonsoft.Json;");
                builder.AppendLine("using Newtonsoft.Json.Linq;");
            }
        }

        /// <summary>
        /// Generates the code.
        /// </summary>
        /// <returns></returns>
        public string GenerateCode()
        {
            try
            {
                var classType = typeof(T);
                CheckClassType(classType);

                var classInterfaceMethods = classType.GetInterfaceMethods();

                HashSet<string> processedMethods = new HashSet<string>();
                StringBuilder builder = new StringBuilder(512 + 256 * classInterfaceMethods.Count);

                GenerateCode(builder, this.Namespace, this.ClassName, this.BaseClassType, classType, classInterfaceMethods);

                return builder.ToString();
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Generates the code.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="namespace">The namespace.</param>
        /// <param name="targetClassName">Name of the target class.</param>
        /// <param name="inheritedClass">The inherited class.</param>
        /// <param name="selfClass">The self class.</param>
        /// <param name="methodsToBuild">The methods to build.</param>
        /// <returns></returns>
        protected void GenerateCode(StringBuilder builder, string @namespace, string targetClassName, Type inheritedClass, Type selfClass, List<MethodInfo> methodsToBuild)
        {
            try
            {
                methodsToBuild.CheckNullObject(nameof(methodsToBuild));

                //Start to build code

                WriteFileInfo(builder);
                WriteNamespaces(builder);
                builder.AppendLine();

                // write namespace
                builder.AppendLineWithFormat("namespace {0}", Namespace);
                builder.AppendLine("{");

                // write class declaration
                builder.AppendIndent(1);

                builder.Append(GenerateClassDeclarationPart(ClassName, selfClass.GetInterfaces()));

                builder.AppendIndent(1);
                builder.AppendLine("{");

                // write constructor
                WriteConstructor(builder, ClassName);

                foreach (var one in methodsToBuild)
                {
                    GenerateCoreMethod(builder, one);
                }

                // End of class
                builder.AppendIndent(1);
                builder.AppendLine("}");

                // End of namespace
                builder.AppendLine("}");

                builder.AppendLine();

                //End of code build
            }
            catch (Exception ex)
            {
                throw ex.Handle();
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
                builder.AppendIndent(2);
                builder.AppendLineWithFormat("public {0}({1} instance, {2} injectionDelegates):base(instance, injectionDelegates)", className, typeof(T).ToCodeLook(), typeof(MethodInjectionDelegates).ToCodeLook());
                builder.AppendIndent(2);
                builder.AppendLine("{");
                builder.AppendIndent(2);
                builder.AppendLine("}");
                builder.AppendLine();
            }
        }

        /// <summary>
        /// Generates the method call information declarition.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="indent">The indent.</param>
        /// <param name="methodCallInfoVariableName">Name of the method call information variable.</param>
        protected void GenerateMethodCallInfoDeclarition(StringBuilder builder, MethodInfo methodInfo, int indent, string methodCallInfoVariableName)
        {
            if (builder != null && methodInfo != null && !string.IsNullOrWhiteSpace(methodCallInfoVariableName))
            {
                var tmpMethodCallInfo = new MethodCallInfo();
                tmpMethodCallInfo.Fill(methodInfo);

                builder.AppendIndent(indent);
                builder.Append(string.Format("{0} {1} = new {0}(", typeof(MethodCallInfo).ToCodeLook(), methodCallInfoVariableName));
                builder.Append(string.Format("\"{0}\",", tmpMethodCallInfo.MethodFullName));
                builder.Append(" new Dictionary<string, object>{");
                foreach (var one in tmpMethodCallInfo.InArgs)
                {
                    builder.Append(" {\"");
                    builder.Append(one.Key);
                    builder.Append("\", ");
                    builder.Append(one.Key);
                    builder.Append("},");
                }
                builder.RemoveLastIfMatch(StringConstants.CommaChar);
                builder.Append("}");
                builder.AppendLine(")");

                builder.AppendBraceBegin(ref indent);

                builder.AppendIndent(indent);
                builder.Append("SerializableArgNames = new System.Collections.Generic.List<System.String> {");
                builder.Append(CSharpCodeGenerateUtil.CombineCode(tmpMethodCallInfo.SerializableArgNames, (x) => { return x.AsQuotedString(); }, 16));
                builder.AppendLine("}");

                builder.AppendBraceEnd( ref indent);
                builder.AppendLine(";");
            }
        }

        /// <summary>
        /// Generates the method injection invoke.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="injectionMethodName">Name of the injection method.</param>
        /// <param name="methodCallInfoVariableName">Name of the method call information variable.</param>
        /// <param name="indent">The indent.</param>
        /// <param name="ifNullCall">If null call.</param>
        /// <param name="callAfterInjection">The call after injection.</param>
        protected void GenerateMethodInjectionInvoke(StringBuilder builder, string injectionMethodName, string methodCallInfoVariableName, int indent, string ifNullCall = null, string callAfterInjection = null)
        {
            if (builder != null && !string.IsNullOrWhiteSpace(injectionMethodName) && !string.IsNullOrWhiteSpace(methodCallInfoVariableName))
            {
                builder.AppendIndent(indent);
                builder.AppendLineWithFormat("if(this._injectionDelegates.{0} != null)", injectionMethodName);
                builder.AppendBraceBegin( ref indent);

                builder.AppendIndent(indent);
                builder.AppendLineWithFormat("this._injectionDelegates.{0}({1});", injectionMethodName, methodCallInfoVariableName);

                if (!string.IsNullOrWhiteSpace(callAfterInjection))
                {
                    builder.AppendIndent(indent);
                    builder.AppendLine(callAfterInjection);
                }
                builder.AppendBraceEnd( ref indent);

                if (!string.IsNullOrWhiteSpace(ifNullCall))
                {
                    builder.AppendIndent(indent);
                    builder.AppendLineWithFormat("else");

                    builder.AppendBraceBegin( ref indent);

                    builder.AppendIndent(indent);
                    builder.AppendLine(ifNullCall);
                    builder.AppendBraceEnd( ref indent);
                }
            }
        }

        /// <summary>
        /// Generates the core method.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="methodInfo">The method information.</param>
        protected void GenerateCoreMethod(StringBuilder builder, MethodInfo methodInfo)
        {
            const string methodCallInfoVariableName = "methodCallInfo";
            var currentIndent = 2;

            if (builder != null && methodInfo != null)
            {
                //Declaration
                builder.AppendIndent(currentIndent);
                builder.AppendLineWithFormat("public virtual {0}", methodInfo.ToDeclarationCodeLook());

                builder.AppendBraceBegin( ref currentIndent);

                //Invoke body

                #region method body

                GenerateMethodCallInfoDeclarition(builder, methodInfo, currentIndent, methodCallInfoVariableName);

                builder.AppendIndent(currentIndent);
                builder.AppendLine("try");

                #region try

                builder.AppendBraceBegin( ref currentIndent);

                //Injection of before event
                GenerateMethodInjectionInvoke(builder, "MethodInvokingEvent", methodCallInfoVariableName, currentIndent);

                //Core
                builder.AppendIndent(currentIndent);
                if (methodInfo.ReturnParameter.ParameterType != typeof(void))
                {
                    builder.Append("return ");
                }
                builder.AppendLineWithFormat("_instance.{0};", methodInfo.ToInvokeCodeLook(methodInfo.GetGenericArguments(), methodInfo.GetParameters()));

                builder.AppendBraceEnd( ref currentIndent);

                #endregion try

                builder.AppendIndent(currentIndent);
                builder.AppendLine("catch (Exception ex)");

                #region catch

                builder.AppendBraceBegin( ref currentIndent);

                builder.AppendIndent(currentIndent);
                builder.AppendLineWithFormat("{0}.Exception = ex;", methodCallInfoVariableName);

                // Injection of exception handle
                GenerateMethodInjectionInvoke(builder, "ExceptionDelegate", methodCallInfoVariableName, currentIndent, string.Format("throw ex.Handle((from item in {0}.InArgs where {0}.SerializableArgNames.Contains(item.Key) select item).ToDictionary());", methodCallInfoVariableName), callAfterInjection: methodInfo.ReturnType.IsVoid(false) ? string.Empty : string.Format("return default({0});", methodInfo.ReturnType.ToCodeLook()));

                builder.AppendBraceEnd( ref currentIndent);

                #endregion catch

                builder.AppendIndent(currentIndent);
                builder.AppendLine("finally");

                #region finally

                builder.AppendBraceBegin( ref currentIndent);
                //Injection of after event
                GenerateMethodInjectionInvoke(builder, "MethodInvokedEvent", methodCallInfoVariableName, currentIndent);
                builder.AppendBraceEnd( ref currentIndent);

                #endregion finally

                //Body end

                #endregion method body

                builder.AppendBraceEnd( ref currentIndent);

                builder.AppendLine();
            }
        }

        /// <summary>
        /// Checks the type of the class.
        /// </summary>
        /// <param name="type">The type.</param>
        protected void CheckClassType(Type type)
        {
            if (type != null)
            {
                if (type.IsAbstract)
                {
                    throw ExceptionFactory.CreateInvalidObjectException(nameof(type), type?.FullName, "NotAllowAbstract");
                }

                if (type.IsGenericType)
                {
                    throw ExceptionFactory.CreateInvalidObjectException(nameof(type), type?.FullName, "NotAllowGeneric");
                }
            }
        }
    }
}