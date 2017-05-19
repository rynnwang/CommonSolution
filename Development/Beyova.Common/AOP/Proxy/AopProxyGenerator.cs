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
                builder.AppendIndent(CodeIndent, 1);

                builder.Append(GenerateClassDeclarationPart(ClassName, selfClass.GetInterfaces()));

                builder.AppendIndent(CodeIndent, 1);
                builder.AppendLine("{");

                // write constructor
                WriteConstructor(builder, ClassName);

                foreach (var one in methodsToBuild)
                {
                    GenerateCoreMethod(builder, one);
                }

                // End of class
                builder.AppendIndent(CodeIndent, 1);
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
                builder.AppendIndent(CodeIndent, 2);
                builder.AppendLineWithFormat("public {0}({1} instance, {2} injectionDelegates):base(instance, injectionDelegates)", className, typeof(T).ToCodeLook(), typeof(MethodInjectionDelegates).ToCodeLook());
                builder.AppendIndent(CodeIndent, 2);
                builder.AppendLine("{");
                builder.AppendIndent(CodeIndent, 2);
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

                builder.AppendIndent(CodeIndent, indent);
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

                this.BeginCodeScope(builder, ref indent);

                builder.AppendIndent(CodeIndent, indent);
                builder.Append("SerializableArgNames = new System.Collections.Generic.List<System.String> {");
                builder.Append(CSharpCodeGenerateUtil.CombineCode(tmpMethodCallInfo.SerializableArgNames, (x) => { return x.AsQuotedString(); }, 16));
                builder.AppendLine("}");

                this.EndCodeScope(builder, ref indent);
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
                builder.AppendIndent(CodeIndent, indent);
                builder.AppendLineWithFormat("if(this._injectionDelegates.{0} != null)", injectionMethodName);
                this.BeginCodeScope(builder, ref indent);

                builder.AppendIndent(CodeIndent, indent);
                builder.AppendLineWithFormat("this._injectionDelegates.{0}({1});", injectionMethodName, methodCallInfoVariableName);

                if (!string.IsNullOrWhiteSpace(callAfterInjection))
                {
                    builder.AppendIndent(CodeIndent, indent);
                    builder.AppendLine(callAfterInjection);
                }
                this.EndCodeScope(builder, ref indent);

                if (!string.IsNullOrWhiteSpace(ifNullCall))
                {
                    builder.AppendIndent(CodeIndent, indent);
                    builder.AppendLineWithFormat("else");

                    this.BeginCodeScope(builder, ref indent);

                    builder.AppendIndent(CodeIndent, indent);
                    builder.AppendLine(ifNullCall);
                    this.EndCodeScope(builder, ref indent);
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
                builder.AppendIndent(CodeIndent, currentIndent);
                builder.AppendLineWithFormat("public virtual {0}", methodInfo.ToDeclarationCodeLook());

                this.BeginCodeScope(builder, ref currentIndent);

                //Invoke body
                #region method body

                GenerateMethodCallInfoDeclarition(builder, methodInfo, currentIndent, methodCallInfoVariableName);

                builder.AppendIndent(CodeIndent, currentIndent);
                builder.AppendLine("try");

                #region try

                this.BeginCodeScope(builder, ref currentIndent);

                //Injection of before event
                GenerateMethodInjectionInvoke(builder, "MethodInvokingEvent", methodCallInfoVariableName, currentIndent);

                //Core
                builder.AppendIndent(CodeIndent, currentIndent);
                if (methodInfo.ReturnParameter.ParameterType != typeof(void))
                {
                    builder.Append("return ");
                }
                builder.AppendLineWithFormat("_instance.{0};", methodInfo.ToInvokeCodeLook(methodInfo.GetGenericArguments(), methodInfo.GetParameters()));

                this.EndCodeScope(builder, ref currentIndent);

                #endregion

                builder.AppendIndent(CodeIndent, currentIndent);
                builder.AppendLine("catch (Exception ex)");
                #region catch

                this.BeginCodeScope(builder, ref currentIndent);

                builder.AppendIndent(CodeIndent, currentIndent);
                builder.AppendLineWithFormat("{0}.Exception = ex;", methodCallInfoVariableName);

                // Injection of exception handle                
                GenerateMethodInjectionInvoke(builder, "ExceptionDelegate", methodCallInfoVariableName, currentIndent, string.Format("throw ex.Handle((from item in {0}.InArgs where {0}.SerializableArgNames.Contains(item.Key) select item).ToDictionary());", methodCallInfoVariableName), callAfterInjection: methodInfo.ReturnType.IsVoid(false) ? string.Empty : string.Format("return default({0});", methodInfo.ReturnType.ToCodeLook()));

                this.EndCodeScope(builder, ref currentIndent);

                #endregion

                builder.AppendIndent(CodeIndent, currentIndent);
                builder.AppendLine("finally");

                #region finally
                this.BeginCodeScope(builder, ref currentIndent);
                //Injection of after event
                GenerateMethodInjectionInvoke(builder, "MethodInvokedEvent", methodCallInfoVariableName, currentIndent);
                this.EndCodeScope(builder, ref currentIndent);
                #endregion

                //Body end
                #endregion

                this.EndCodeScope(builder, ref currentIndent);

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
