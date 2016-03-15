using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using Beyova.ExceptionSystem;
using System.Linq;

namespace Beyova
{
    /// <summary>
    /// Class Sandbox.
    /// </summary>
    public class Sandbox : IDisposable
    {
        /// <summary>
        /// Gets or sets the application domain.
        /// </summary>
        /// <value>The application domain.</value>
        public AppDomain AppDomain { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sandbox"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public Sandbox(string name = null)
        {
            var appDomainSetup = new AppDomainSetup();
            appDomainSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            AppDomain = AppDomain.CreateDomain(name.SafeToString(Guid.NewGuid().ToString()), null, appDomainSetup);
        }

        /// <summary>
        /// Loads the library.
        /// </summary>
        /// <param name="assemblyBytes">The assembly bytes.</param>
        public void LoadLibrary(byte[] assemblyBytes)
        {
            if (assemblyBytes != null && assemblyBytes.Length > 0)
            {
                AppDomain.Load(assemblyBytes);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            AppDomain.Unload(AppDomain);
        }

        /// <summary>
        /// Executes the specified assembly name.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Object.</returns>
        public object Execute(string assemblyName, string typeName, string method, params object[] parameters)
        {
            try
            {
                method.CheckEmptyString("method");

                var objectToRun = CreateInstance(assemblyName, typeName);
                objectToRun.CheckNullObject("objectToRun");

                var methodInfo = objectToRun.GetType().GetMethod(method);
                methodInfo.CheckNullObject("methodInfo");

                return methodInfo.Invoke(objectToRun, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle("Execute", new { assemblyName, typeName, method });
            }
        }

        /// <summary>
        /// Adds the dynamic assembly.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="referencedAssemblies">The referenced assemblies.</param>
        /// <param name="codesToCompile">The codes to compile.</param>
        /// <returns>Assembly.</returns>
        /// <exception cref="OperationFailureException">CompileAssemblyFromSource;null</exception>
        public Assembly AddDynamicAssembly(CodeDomProvider provider, List<string> referencedAssemblies, string codesToCompile)
        {
            try
            {
                provider.CheckNullObject("provider");
                codesToCompile.CheckEmptyString("codesToCompile");

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

                var compilerResult = provider.CompileAssemblyFromSource(objCompilerParameters, codesToCompile);

                if (compilerResult.Errors.HasErrors)
                {
                    List<dynamic> errors = new List<dynamic>();
                    foreach (CompilerError one in compilerResult.Errors)
                    {
                        errors.Add(new
                        {
                            one.ErrorText,
                            one.ErrorNumber,
                            one.Line
                        });
                    }

                    throw new OperationFailureException("CompileAssemblyFromSource", null, errors);
                }

                return compilerResult.CompiledAssembly;
            }
            catch (Exception ex)
            {
                throw ex.Handle("AddDynamicAssembly");
            }
        }

        /// <summary>
        /// Creates the and get instance.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>System.Object.</returns>
        public object CreateAndGetInstance(string assemblyName, string typeName)
        {
            return CreateInstance(assemblyName, typeName);
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>System.Object.</returns>
        protected object CreateInstance(string assemblyName, string typeName)
        {
            assemblyName.CheckEmptyString("assemblyName");
            assemblyName.CheckEmptyString("typeName");

            return AppDomain.CreateInstanceAndUnwrap(assemblyName, typeName);
        }
    }
}
