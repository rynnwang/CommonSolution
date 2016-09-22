using System;
using System.CodeDom.Compiler;
using System.Reflection;

namespace Beyova
{
    /// <summary>
    /// Class TempAssembly.
    /// </summary>
    [Serializable]
    public class TempAssembly
    {
        /// <summary>
        /// Gets or sets the assembly.
        /// </summary>
        /// <value>The assembly.</value>
        private Assembly _assembly;

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
        public string Namespace { get; protected set; }

        /// <summary>
        /// The _app domain
        /// </summary>
        private AppDomain _appDomain;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return _assembly?.GetName()?.Name;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TempAssembly" /> class.
        /// </summary>
        /// <param name="compilerResults">The compiler results.</param>
        /// <param name="_namespace">The _namespace.</param>
        /// <param name="appDomain">The application domain.</param>
        internal TempAssembly(CompilerResults compilerResults, string _namespace = null, AppDomain appDomain = null)
        {
            this._assembly = compilerResults?.CompiledAssembly;
            this.Namespace = _namespace.SafeToString();
            this._appDomain = appDomain ?? System.AppDomain.CurrentDomain;
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Object.</returns>
        public object CreateInstance(string typeName, params object[] parameters)
        {
            typeName.CheckEmptyString(nameof(typeName));

            if (!typeName.StartsWith(this.Namespace))
            {
                typeName = string.Format("{0}.{1}", this.Namespace, typeName);
            }

            return Activator.CreateInstance(ReflectionExtension.SmartGetType(typeName, false), parameters);
        }
    }
}
