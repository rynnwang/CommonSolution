using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
        public Sandbox(string name)
        {
            AppDomain = AppDomain.CreateDomain(name);
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
        /// <exception cref="System.ArgumentNullException">method</exception>
        /// <exception cref="System.NullReferenceException">
        /// objectToRun
        /// or
        /// methodInfo
        /// </exception>
        /// <exception cref="System.InvalidOperationException">Failed to Execute.</exception>
        public object Execute(string assemblyName, string typeName, string method, params object[] parameters)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(method))
                {
                    throw new ArgumentNullException("method");
                }

                var objectToRun = CreateInstance(assemblyName, typeName);

                if (objectToRun == null)
                {
                    throw new NullReferenceException("objectToRun");
                }

                var methodInfo = objectToRun.GetType().GetMethod(method);

                if (methodInfo == null)
                {
                    throw new NullReferenceException("methodInfo");
                }

                return methodInfo.Invoke(objectToRun, parameters);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to Execute.", ex);
            }
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>System.Object.</returns>
        protected object CreateInstance(string assemblyName, string typeName)
        {
            if (string.IsNullOrWhiteSpace(assemblyName))
            {
                throw new ArgumentNullException("assemblyName");
            }

            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentNullException("typeName");
            }

            return AppDomain.CreateInstanceAndUnwrap(assemblyName, typeName);
        }
    }
}
