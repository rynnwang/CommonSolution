using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ifunction.ExceptionSystem;

namespace ifunction
{
    /// <summary>
    /// Class DynamicCode.
    /// </summary>
    public static class DynamicCode
    {
        /// <summary>
        /// Dynamics the execute code.
        /// </summary>
        /// <param name="codesToExecute">The codes to execute.</param>
        /// <param name="invokeDelegate">The invoke delegate.</param>
        /// <param name="referencedAssemblies">The referenced assemblies.</param>
        /// <param name="language">The language.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="InvalidObjectException">language</exception>
        /// <exception cref="OperationFailureException">CompileAssemblyFromSource;null</exception>
        public static object DynamicExecuteCode(string codesToExecute, Func<Assembly, object> invokeDelegate, IList<string> referencedAssemblies = null, string language = "CSharp")
        {
            try
            {
                if (!CodeDomProvider.IsDefinedLanguage(language))
                {
                    throw new InvalidObjectException("language", null, language);
                }
                else
                {
                    codesToExecute.CheckEmptyString("codesToExecute");
                    invokeDelegate.CheckNullObject("invokeDelegate");

                    var provider = CodeDomProvider.CreateProvider(language);
                    var objCompilerParameters = new CompilerParameters
                    {
                        GenerateExecutable = false,
                        GenerateInMemory = true
                    };

                    if (referencedAssemblies != null)
                    {
                        objCompilerParameters.ReferencedAssemblies.AddRange(referencedAssemblies.ToArray());
                    }
                    else
                    {
                        foreach (var one in ReflectionExtension.GetAppDomainAssemblies())
                        {
                            objCompilerParameters.ReferencedAssemblies.Add(one.FullName);
                        }
                    }

                    var compilerResult = provider.CompileAssemblyFromSource(objCompilerParameters, codesToExecute);

                    if (compilerResult.Errors.HasErrors)
                    {
                        throw new OperationFailureException("CompileAssemblyFromSource", null, compilerResult.Errors);
                    }
                    else
                    {
                        return invokeDelegate.Invoke(compilerResult.CompiledAssembly);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("DynamicExecuteCode", new { codesToExecute, language });
            }
        }
    }
}
