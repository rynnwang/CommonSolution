using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Beyova.ExceptionSystem;
using Microsoft.CSharp;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class TempAssemblyProvider.
    /// </summary>
    public class TempAssemblyProvider
    {
        /// <summary>
        /// Creates the temporary assembly.
        /// </summary>
        /// <param name="sourceCodes">The source codes.</param>
        /// <param name="codeReferencedAssembly">The referenced assembly. If not specified, use <see cref="TempAssemblyProvider.GetDefaultAdditionalAssembly"/> instead as default.</param>
        /// <returns>
        /// TempAssembly instance.
        /// </returns>
        /// <exception cref="DynamicCompileException"></exception>
        public TempAssembly CreateTempAssembly(string[] sourceCodes, HashSet<string> codeReferencedAssembly = null)
        {
            try
            {
                using (CodeDomProvider provider = new CSharpCodeProvider())
                {
                    var objCompilerParameters = new CompilerParameters
                    {
                        GenerateExecutable = false,
                        GenerateInMemory = true
                    };

                    if (codeReferencedAssembly == null)
                    {
                        codeReferencedAssembly = new HashSet<string>();
                    }
                    codeReferencedAssembly.AddRange(GetDefaultAdditionalAssembly().Select(x => x.Location));

                    objCompilerParameters.ReferencedAssemblies.AddRange(codeReferencedAssembly.ToArray());

                    var compilerResult = provider.CompileAssemblyFromSource(objCompilerParameters, sourceCodes);

                    if (compilerResult.Errors.HasErrors)
                    {
                        throw new DynamicCompileException(compilerResult.Errors, sourceCodes);
                    }

                    var @namespace = compilerResult.CompiledAssembly.DefinedTypes.FirstOrDefault()?.Namespace;

                    return new TempAssembly(compilerResult, @namespace, AppDomain.CurrentDomain);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { codeReferencedAssembly, sourceCodes });
            }
        }

        /// <summary>
        /// Creates the temporary assembly.
        /// </summary>
        /// <param name="codeReferencedAssembly">The referenced assembly.</param>
        /// <param name="usingNameSpaces">The using name spaces.</param>
        /// <param name="namespace">The namespace.</param>
        /// <param name="coreCode">The core code.</param>
        /// <returns>Beyova.TempAssembly.</returns>
        public TempAssembly CreateTempAssembly(string coreCode, HashSet<string> codeReferencedAssembly = null, IEnumerable<string> usingNameSpaces = null, string @namespace = null)
        {
            try
            {
                using (CodeDomProvider provider = new CSharpCodeProvider())
                {
                    @namespace = @namespace.SafeToString("Beyova.RuntimeCompile");

                    if (codeReferencedAssembly == null)
                    {
                        codeReferencedAssembly = new HashSet<string>();
                    }

                    foreach (var one in GetDefaultAdditionalAssembly())
                    {
                        codeReferencedAssembly.Add(one.Location);
                    }

                    var nameSpaces = GetDefaultUsingNamespaces();

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
                    builder.AppendLine(coreCode);

                    //End of namespace
                    builder.Append("}");

                    return CreateTempAssembly(builder.ToString().AsArray(), codeReferencedAssembly: codeReferencedAssembly);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { codeReferencedAssembly, @namespace, coreCode });
            }
        }

        /// <summary>
        /// Gets the default using namespaces.
        /// </summary>
        /// <returns>HashSet&lt;System.String&gt;.</returns>
        private static HashSet<string> GetDefaultUsingNamespaces()
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

            return namespaces;
        }

        /// <summary>
        /// Gets the default additional assembly.
        /// </summary>
        /// <returns>HashSet&lt;Assembly&gt;.</returns>
        public static HashSet<Assembly> GetDefaultAdditionalAssembly()
        {
            var result = new HashSet<Assembly>();

            result.Add(typeof(JToken).Assembly);
            result.Add(typeof(Framework).Assembly);
            result.Add(typeof(System.Linq.Enumerable).Assembly);
            result.Add(typeof(BaseException).Assembly);
            result.Add(typeof(XElement).Assembly);
            result.Add(typeof(System.Net.HttpWebRequest).Assembly);
            result.Add(typeof(System.Text.StringBuilder).Assembly);

            return result;
        }
    }
}