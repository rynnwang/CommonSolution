using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Beyova
{
    /// <summary>
    /// Class BaseSandbox.
    /// </summary>
    public abstract class BaseSandbox : IDisposable
    {
        /// <summary>
        /// Gets or sets the application domain.
        /// </summary>
        /// <value>The application domain.</value>
        public AppDomain AppDomain { get; protected set; }

        /// <summary>
        /// Gets or sets the invoker.
        /// </summary>
        /// <value>The invoker.</value>
        protected SandboxInvoker _invoker;

        /// <summary>
        /// Gets or sets the application directory.
        /// </summary>
        /// <value>
        /// The application directory.
        /// </value>
        public string ApplicationDirectory { get; protected set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid Key { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSandbox" /> class.
        /// </summary>
        /// <param name="setting">The setting.</param>
        protected BaseSandbox(SandboxSetting setting)
            : this(setting, Guid.NewGuid())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSandbox"/> class.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="key">The identifier.</param>
        protected internal BaseSandbox(SandboxSetting setting, Guid key)
        {
            this.Key = key;
            Initialize(setting ?? new SandboxSetting { });
        }

        /// <summary>
        /// Initializes the specified name.
        /// </summary>
        /// <param name="setting">The setting.</param>
        protected virtual void Initialize(SandboxSetting setting)
        {
            try
            {
                setting.CheckNullObject(nameof(setting));

                var applicationDirectory = ApplicationDirectory = setting.ApplicationBaseDirectory.SafeToString(EnvironmentCore.ApplicationBaseDirectory);
                bool shareApplicationBaseDirectory = applicationDirectory.Equals(EnvironmentCore.ApplicationBaseDirectory, StringComparison.OrdinalIgnoreCase);

                if (!shareApplicationBaseDirectory && !Directory.Exists(applicationDirectory))
                {
                    Directory.CreateDirectory(applicationDirectory);
                }

                this.AppDomain = AppDomain.CreateDomain(this.Key.ToString(), null, new AppDomainSetup
                {
                    ApplicationBase = applicationDirectory,
                    PrivateBinPath = applicationDirectory
                });

                // Start to process assemblies and libraries
                HashSet<string> assemblyNameToLoad = setting.AssemblyNameListToLoad.ToHashSet() ?? new HashSet<string>();
                Dictionary<string, byte[]> assemblyBytesToLoad = new Dictionary<string, byte[]>();

                // Get all known and loaded Assemblies.
                foreach (var one in (GetDefaultAssembliesToLoad() ?? new HashSet<Assembly>()))
                {
                    assemblyNameToLoad.Add(Path.GetFileNameWithoutExtension(one.Location));
                }

                // Load assemblies 
                foreach (var one in assemblyNameToLoad)
                {
                    var destinationPath = Path.Combine(applicationDirectory, GetAssemblyFileNameWithExtension(one));

                    // Copy dlls if not share base directory
                    if (!shareApplicationBaseDirectory)
                    {
                        var sourcePath = Path.Combine(EnvironmentCore.ApplicationBaseDirectory, GetAssemblyFileNameWithExtension(one));

                        // NOTE: Need to check if file exists. Because not all loaded assembly stay in bin folder, like system dlls. So if not existed in source folder, no need to copy.
                        if (!sourcePath.Equals(destinationPath, StringComparison.OrdinalIgnoreCase) && File.Exists(sourcePath))
                        {
                            File.Copy(sourcePath, destinationPath, true);
                        }
                    }

                    if (File.Exists(destinationPath))
                    {
                        this.AppDomain.Load(new AssemblyName(Path.GetFileNameWithoutExtension(destinationPath)));
                        //  this.AppDomain.Load(File.ReadAllBytes(destinationPath));
                    }
                }

                // Load external byte base assemblies
                if (setting.ExternalAssemblyToLoad.HasItem())
                {
                    foreach (var one in setting.ExternalAssemblyToLoad)
                    {
                        if (!assemblyNameToLoad.Contains(one.Key))
                        {
                            var destinationPath = Path.Combine(applicationDirectory, GetAssemblyFileNameWithExtension(one.Key));
                            File.WriteAllBytes(destinationPath, one.Value);
                            this.AppDomain.Load(new AssemblyName(Path.GetFileNameWithoutExtension(destinationPath)));
                            // this.AppDomain.Load(one.Value);
                        }
                    }
                }

                this._invoker = CreateInstanceAndUnwrap<SandboxInvoker>(this.AppDomain);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { setting });
            }
        }

        /// <summary>
        /// Gets the default assembly to load.
        /// </summary>
        /// <returns>HashSet&lt;Assembly&gt;.</returns>
        protected virtual HashSet<Assembly> GetDefaultAssembliesToLoad()
        {
            return new HashSet<Assembly>();
        }

        /// <summary>
        /// Creates the instance and invoke method.
        /// </summary>
        /// <param name="typeFullName">Full name of the type.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>RemoteInvokeResult.</returns>
        public SandboxInvokeResult CreateInstanceAndInvokeMethod(string typeFullName, string method, params object[] parameters)
        {
            try
            {
                return CreateInstanceAndInvokeMethod(typeFullName, null, method, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { typeFullName, method, parameters });
            }
        }

        /// <summary>
        /// Creates the instance and invoke method.
        /// </summary>
        /// <param name="typeFullName">Full name of the type.</param>
        /// <param name="constructorParameters">The constructor parameters.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="methodParameters">The method parameters.</param>
        /// <returns>Beyova.RemoteInvokeResult.</returns>
        public virtual SandboxInvokeResult CreateInstanceAndInvokeMethod(string typeFullName, object[] constructorParameters, string methodName, params object[] methodParameters)
        {
            try
            {
                typeFullName.CheckEmptyString(nameof(typeFullName));
                methodName.CheckEmptyString(nameof(methodName));

                return WrapToCurrentAppDomain(_invoker?.CreateInstanceAndInvokeMethod(typeFullName, constructorParameters, methodName, methodParameters));
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { typeFullName, constructorParameters, methodName, methodParameters });
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            AppDomain.Unload(AppDomain);
        }

        #region Static methods

        /// <summary>
        /// Creates the instance and unwrap.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="appDomain">The application domain.</param>
        /// <returns>T.</returns>
        protected static T CreateInstanceAndUnwrap<T>(AppDomain appDomain)
        {
            var type = typeof(T);
            return appDomain == null ? default(T) : (T)appDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
        }

        /// <summary>
        /// Gets the assembly file name with extension.
        /// </summary>
        /// <param name="pureName">Name of the pure.</param>
        /// <returns>System.String.</returns>
        protected static string GetAssemblyFileNameWithExtension(string pureName)
        {
            return string.Format("{0}.dll", pureName);
        }

        #endregion

        /// <summary>
        /// Wraps to current application domain.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>Beyova.SandboxInvokeResult.</returns>
        public static SandboxInvokeResult WrapToCurrentAppDomain(SandboxMarshalInvokeResult result)
        {
            return result == null ? null : new SandboxInvokeResult(result);
        }
    }
}
