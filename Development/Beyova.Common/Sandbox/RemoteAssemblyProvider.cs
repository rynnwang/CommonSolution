using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class RemoteAssemblyProvider.
    /// </summary>
    [Serializable]
    public sealed class RemoteAssemblyProvider : SandboxMarshalObject
    {
        /// <summary>
        /// The provider
        /// </summary>
        [NonSerialized]
        private TempAssemblyProvider provider = new TempAssemblyProvider();

        /// <summary>
        /// The temporary assemblies
        /// </summary>
        [NonSerialized]
        private Dictionary<string, TempAssembly> tempAssemblies = new Dictionary<string, TempAssembly>();

        /// <summary>
        /// Creates the temporary assembly.
        /// </summary>
        /// <param name="sourceCodes">The source codes.</param>
        /// <param name="codeReferencedAssembly">The referenced assembly.</param>
        /// <returns>TempAssembly.</returns>
        public RemoteRuntimeCompileResult CreateRemoteTempAssembly(string[] sourceCodes, HashSet<string> codeReferencedAssembly = null)
        {
            try
            {
                sourceCodes.CheckNullObject("sourceCodes");

                var tempAssembly = provider.CreateTempAssembly(sourceCodes, codeReferencedAssembly);
                var name = tempAssembly.Name;
                tempAssemblies.Add(tempAssembly.Name, tempAssembly);

                return new RemoteRuntimeCompileResult { TempAssemblyName = name };
            }
            catch (Exception ex)
            {
                return new RemoteRuntimeCompileResult { CompileExceptionInfo = ex.Handle(sourceCodes).ToExceptionInfo().ToJson() };
            }
        }

        /// <summary>
        /// Creates the temporary assembly.
        /// </summary>
        /// <param name="coreCode">The core code.</param>
        /// <param name="codeReferencedAssembly">The referenced assembly.</param>
        /// <param name="usingNameSpaces">The using name spaces.</param>
        /// <param name="namespace">The namespace.</param>
        /// <returns>Beyova.TempAssembly.</returns>
        public RemoteRuntimeCompileResult CreateRemoteTempAssembly(string coreCode, HashSet<string> codeReferencedAssembly = null, IEnumerable<string> usingNameSpaces = null, string @namespace = null)
        {
            try
            {
                coreCode.CheckEmptyString("coreCode");

                var tempAssembly = provider.CreateTempAssembly(coreCode, codeReferencedAssembly, usingNameSpaces, @namespace);
                var name = tempAssembly.Name;
                tempAssemblies.Add(tempAssembly.Name, tempAssembly);

                return new RemoteRuntimeCompileResult { TempAssemblyName = name };
            }
            catch (Exception ex)
            {
                return new RemoteRuntimeCompileResult { CompileExceptionInfo = ex.Handle(new { coreCode, codeReferencedAssembly, usingNameSpaces, @namespace }).ToExceptionInfo().ToJson() };
            }
        }

        /// <summary>
        /// Creates the instance and invoke method.
        /// NOTE: items in parameters should all inherited from <see cref="MarshalByRefObject" /> and set <see cref="SerializableAttribute" /> in class for proxy transfer.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="method">The method.</param>
        /// <param name="methodParameters">The parameters.</param>
        /// <returns>RemoteInvokeResult.</returns>
        public SandboxMarshalInvokeResult CreateInstanceAndInvokeMethod(string assemblyName, string typeName, string method, params object[] methodParameters)
        {
            SandboxMarshalInvokeResult result = new SandboxMarshalInvokeResult();

            try
            {
                assemblyName.CheckEmptyString(nameof(assemblyName));
                method.CheckEmptyString(nameof(method));

                TempAssembly assembly;
                if (tempAssemblies.TryGetValue(assemblyName, out assembly))
                {
                    var objectToRun = assembly.CreateInstance(typeName);
                    objectToRun.CheckNullObject(nameof(objectToRun));

                    var methodInfo = objectToRun.GetType().GetMethod(method);
                    methodInfo.CheckNullObject(nameof(methodInfo));

                    result.SetValue(methodInfo.Invoke(objectToRun, methodParameters));
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                result.SetException(ex.Handle(new { typeName, method }));
            }

            return result;
        }

        /// <summary>
        /// Creates the instance and invoke method.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="constructorParameters">The constructor parameters.</param>
        /// <param name="method">The method.</param>
        /// <param name="methodParameters">The method parameters.</param>
        /// <returns>RemoteInvokeResult.</returns>
        public SandboxMarshalInvokeResult CreateInstanceAndInvokeMethod(string assemblyName, string typeName, object[] constructorParameters, string method, params object[] methodParameters)
        {
            SandboxMarshalInvokeResult result = new SandboxMarshalInvokeResult();

            try
            {
                assemblyName.CheckEmptyString(nameof(assemblyName));
                method.CheckEmptyString(nameof(method));

                TempAssembly assembly;
                if (tempAssemblies.TryGetValue(assemblyName, out assembly))
                {
                    var objectToRun = assembly.CreateInstance(typeName, constructorParameters);
                    objectToRun.CheckNullObject(nameof(objectToRun));

                    var methodInfo = objectToRun.GetType().GetMethod(method);
                    methodInfo.CheckNullObject(nameof(methodInfo));

                    result.SetValue(methodInfo.Invoke(objectToRun, methodParameters));
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                result.SetException(ex.Handle(new { typeName, method }));
            }

            return result;
        }
    }
}
