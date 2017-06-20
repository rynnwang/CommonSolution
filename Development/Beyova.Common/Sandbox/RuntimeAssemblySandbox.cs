using System;
using System.Collections.Generic;
using System.Reflection;

namespace Beyova
{
    /// <summary>
    /// Class RuntimeAssemblySandbox.
    /// </summary>
    public class RuntimeAssemblySandbox : BaseSandbox
    {
        /// <summary>
        /// Gets or sets the assembly provider.
        /// </summary>
        /// <value>The assembly provider.</value>
        protected RemoteAssemblyProvider _assemblyProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeAssemblySandbox" /> class.
        /// </summary>
        /// <param name="applicationDirectory">The application directory.</param>
        public RuntimeAssemblySandbox(string applicationDirectory = null)
            : this(new SandboxSetting { ApplicationBaseDirectory = applicationDirectory })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeAssemblySandbox" /> class.
        /// </summary>
        /// <param name="setting">The setting.</param>
        public RuntimeAssemblySandbox(SandboxSetting setting)
            : base(setting)
        {
        }

        /// <summary>
        /// Initializes the specified name.
        /// </summary>
        /// <param name="setting">The setting.</param>
        protected override void Initialize(SandboxSetting setting)
        {
            base.Initialize(setting);
            this._assemblyProvider = CreateInstanceAndUnwrap<RemoteAssemblyProvider>(this.AppDomain);
        }

        /// <summary>
        /// Gets the default assembly to load.
        /// </summary>
        /// <returns>HashSet&lt;Assembly&gt;.</returns>
        protected override HashSet<Assembly> GetDefaultAssembliesToLoad()
        {
            return TempAssemblyProvider.GetDefaultAdditionalAssembly();
        }

        /// <summary>
        /// Creates the temporary assembly.
        /// </summary>
        /// <param name="sourceCodes">The source codes.</param>
        /// <param name="codeReferencedAssembly">The code referenced assembly.</param>
        /// <returns>System.String.</returns>
        public string CreateTempAssembly(string[] sourceCodes, HashSet<string> codeReferencedAssembly = null)
        {
            try
            {
                sourceCodes.CheckNullObject("sourceCodes");
                _assemblyProvider.CheckNullObject("_assemblyProvider");

                var compileResult = _assemblyProvider.CreateRemoteTempAssembly(sourceCodes, codeReferencedAssembly);
                compileResult.CheckNullObject(nameof(compileResult));

                var exceptionInfo = compileResult.GetExceptionInfo();
                if (exceptionInfo == null)
                {
                    return compileResult.TempAssemblyName;
                }
                else
                {
                    throw ExceptionFactory.CreateOperationException(data: exceptionInfo);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { sourceCodes, codeReferencedAssembly });
            }
        }

        /// <summary>
        /// Creates the temporary assembly.
        /// </summary>
        /// <param name="coreCode">The core code.</param>
        /// <param name="codeReferencedAssembly">The code referenced assembly.</param>
        /// <param name="usingNameSpaces">The using name spaces.</param>
        /// <param name="namespace">The namespace.</param>
        /// <returns>System.String.</returns>
        public string CreateTempAssembly(string coreCode, HashSet<string> codeReferencedAssembly = null, IEnumerable<string> usingNameSpaces = null, string @namespace = null)
        {
            try
            {
                coreCode.CheckNullObject(nameof(coreCode));
                _assemblyProvider.CheckNullObject(nameof(_assemblyProvider));

                var compileResult = _assemblyProvider.CreateRemoteTempAssembly(coreCode, codeReferencedAssembly, usingNameSpaces, @namespace);
                var exceptionInfo = compileResult.GetExceptionInfo();
                if (exceptionInfo == null)
                {
                    return compileResult.TempAssemblyName;
                }
                else
                {
                    throw ExceptionFactory.CreateOperationException(data: exceptionInfo);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { coreCode, codeReferencedAssembly });
            }
        }

        /// <summary>
        /// Invokes the runtime assembly.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="methodParameters">The method parameters.</param>
        /// <returns>SandboxInvokeResult.</returns>
        public SandboxInvokeResult InvokeRuntimeAssembly(string assemblyName, string typeName, string methodName, params object[] methodParameters)
        {
            return InvokeRuntimeAssembly(assemblyName, typeName, null, methodName, methodParameters);
        }

        /// <summary>
        /// Creates the instance and invoke method.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="constructorParameters">The constructor parameters.</param>
        /// <param name="methodName">The method.</param>
        /// <param name="methodParameters">The method parameters.</param>
        /// <returns>SandboxInvokeResult.</returns>
        public SandboxInvokeResult InvokeRuntimeAssembly(string assemblyName, string typeName, object[] constructorParameters, string methodName, params object[] methodParameters)
        {
            try
            {
                assemblyName.CheckEmptyString(nameof(assemblyName));
                typeName.CheckEmptyString(nameof(typeName));
                methodName.CheckEmptyString(nameof(methodName));
                _assemblyProvider.CheckNullObject(nameof(_assemblyProvider));

                return WrapToCurrentAppDomain(_assemblyProvider.CreateInstanceAndInvokeMethod(assemblyName, typeName, constructorParameters, methodName, methodParameters));
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { assemblyName, typeName, constructorParameters, methodName, methodParameters });
            }
        }
    }
}