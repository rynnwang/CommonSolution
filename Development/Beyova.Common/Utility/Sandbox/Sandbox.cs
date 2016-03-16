using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using Beyova.ExceptionSystem;
using System.Linq;
using System.IO;
using System.Text;

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
            appDomainSetup.ApplicationBase = EnvironmentCore.ApplicationBaseDirectory;
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
        /// Creates the dynamic assembly.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="referencedAssemblies">The referenced assemblies.</param>
        /// <param name="namespace">The namespace.</param>
        /// <param name="usingNameSpaces">The using nameSpaces.</param>
        /// <param name="classCodesToCompile">The class codes to compile.</param>
        /// <returns>Assembly.</returns>
        /// <exception cref="OperationFailureException">CompileAssemblyFromSource;null</exception>
        public SandboxAssembly CreateDynamicAssembly(CodeDomProvider provider, List<string> referencedAssemblies, string @namespace, IEnumerable<string> usingNameSpaces, string classCodesToCompile)
        {
            try
            {
                provider.CheckNullObject("provider");
                classCodesToCompile.CheckEmptyString("classCodesToCompile");

                @namespace = @namespace.SafeToString("Beyova.DynamicCompile.Sandbox");

                var objCompilerParameters = new CompilerParameters
                {
                    GenerateExecutable = false,
                    GenerateInMemory = true
                };

                // Prepare references.

                var references = GetCommonAssemblyNameList();

                if (referencedAssemblies.HasItem())
                {
                    references.AddRange(referencedAssemblies);
                }

                objCompilerParameters.ReferencedAssemblies.AddRange(references.ToArray());

                // Prepare references done.

                // Prepare namespace

                var nameSpaces = GetCommonNamespaces();

                if (usingNameSpaces.HasItem())
                {
                    nameSpaces.AddRange(usingNameSpaces);
                }

                // Prepare namespace done;

                StringBuilder builder = new StringBuilder(512);
                foreach (var one in nameSpaces)
                {
                    builder.AppendLineWithFormat("using {0};", one);
                }

                builder.AppendLineWithFormat("namespace {0}", @namespace);
                //Namespace start
                builder.AppendLine("{");
                builder.AppendLine(classCodesToCompile);

                //End of namespace
                builder.Append("}");

                var compilerResult = provider.CompileAssemblyFromSource(objCompilerParameters, classCodesToCompile);

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

                return new SandboxAssembly(compilerResult.CompiledAssembly, @namespace);
            }
            catch (Exception ex)
            {
                throw ex.Handle("CreateDynamicAssembly");
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
        /// Gets the common assembly name list.
        /// </summary>
        /// <returns>HashSet&lt;System.String&gt;.</returns>
        protected static HashSet<string> GetCommonAssemblyNameList()
        {
            var assemblyToAdd = new HashSet<string>();

            assemblyToAdd.Add("System.dll");
            assemblyToAdd.Add("System.Core.dll");
            assemblyToAdd.Add("System.Xml.dll");
            assemblyToAdd.Add("System.Xml.Linq.dll");
            assemblyToAdd.Add("Microsoft.VisualBasic.dll");
            assemblyToAdd.Add(Path.Combine(EnvironmentCore.ApplicationBaseDirectory, "Newtonsoft.Json.dll"));
            assemblyToAdd.Add(Path.Combine(EnvironmentCore.ApplicationBaseDirectory, "Beyova.Common.dll"));

            return assemblyToAdd;
        }

        /// <summary>
        /// Gets the common namespaces.
        /// </summary>
        /// <returns>HashSet&lt;System.String&gt;.</returns>
        private static HashSet<string> GetCommonNamespaces()
        {
            var namespaces = new HashSet<string>();
            namespaces.Add("System");
            namespaces.Add("System.Collections.Generic");
            namespaces.Add("System.Linq");
            namespaces.Add("System.Net");
            namespaces.Add("System.Reflection");
            namespaces.Add("System.Text");
            namespaces.Add("Beyova.ExceptionSystem");
            namespaces.Add("Beyova");
            namespaces.Add("Beyova.RestApi");
            namespaces.Add("Newtonsoft.Json");
            namespaces.Add("Newtonsoft.Json.Linq");

            return namespaces;
        }
    }
}
