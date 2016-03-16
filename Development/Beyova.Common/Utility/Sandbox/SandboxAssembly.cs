using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using Beyova.ExceptionSystem;
using System.Linq;
using System.IO;

namespace Beyova
{
    /// <summary>
    /// Class SandboxAssembly.
    /// </summary>
    public class SandboxAssembly
    {
        /// <summary>
        /// Gets or sets the assembly.
        /// </summary>
        /// <value>The assembly.</value>
        public Assembly Assembly { get; protected set; }

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
        public string Namespace { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SandboxAssembly" /> class.
        /// </summary>
        /// <param name="_assembly">The _assembly.</param>
        /// <param name="_namespace">The _namespace.</param>
        internal SandboxAssembly(Assembly _assembly, string _namespace)
        {
            this.Assembly = _assembly;
            this.Namespace = _namespace;
        }

        /// <summary>
        /// Executes the specified assembly name.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Object.</returns>
        public object Execute(string typeName, string method, params object[] parameters)
        {
            try
            {
                method.CheckEmptyString("method");

                var objectToRun = CreateInstance(typeName);
                objectToRun.CheckNullObject("objectToRun");

                var methodInfo = objectToRun.GetType().GetMethod(method);
                methodInfo.CheckNullObject("methodInfo");

                return methodInfo.Invoke(objectToRun, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle("Execute", new { typeName, method });
            }
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>System.Object.</returns>
        public object CreateInstance(string typeName)
        {
            typeName.CheckEmptyString("typeName");

            return this.Assembly.CreateInstance(typeName);
        }
    }
}
