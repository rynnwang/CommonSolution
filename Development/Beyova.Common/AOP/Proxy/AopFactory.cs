using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Beyova.AOP
{
    /// <summary>
    /// Class AopProxyFactory.
    /// </summary>
    public static class AopProxyFactory
    {
        /// <summary>
        /// The aop factory output namespace
        /// </summary>
        const string AopFactoryOutputNamespace = "Beyova.Aop.Tmp";

        /// <summary>
        /// The proxy namespace format
        /// </summary>
        const string ProxyNamespaceFormat = "Beyova.Aop.Tmp._{0}";

        /// <summary>
        /// The proxy options
        /// </summary>
        static Dictionary<Type, AopProxyOptions> proxyOptionsCollection = new Dictionary<Type, AopProxyOptions>();

        /// <summary>
        /// The proxy instances
        /// </summary>
        static Dictionary<Type, object> proxyInstances = new Dictionary<Type, object>();

        /// <summary>
        /// The locker
        /// </summary>
        static object locker = new object();

        /// <summary>
        /// Creates the aop interface proxy. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="injectionDelegates">The injection delegates.</param>
        /// <returns></returns>
        public static object CreateAopInterfaceProxy<T>(MethodInjectionDelegates injectionDelegates = null)
            where T : class, new()
        {
            return InternalAsAopInterfaceProxy(new T(), false, injectionDelegates);
        }

        /// <summary>
        /// As aop interface proxy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="injectionDelegates">The injection delegates.</param>
        /// <returns></returns>
        public static object AsAopInterfaceProxy<T>(this T instance, MethodInjectionDelegates injectionDelegates = null)
            where T : class
        {
            return InternalAsAopInterfaceProxy(instance, false, injectionDelegates);
        }

        /// <summary>
        /// As the aop interface proxy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="reuseInstance">if set to <c>true</c> [reuse instance].</param>
        /// <param name="injectionDelegates">The injection delegates.</param>
        /// <returns></returns>
        private static object InternalAsAopInterfaceProxy<T>(this T instance, bool reuseInstance, MethodInjectionDelegates injectionDelegates)
            where T : class
        {
            var type = typeof(T);
            object proxiedInstance = null;

            try
            {
                instance.CheckNullObject(nameof(instance));

                if (!proxyOptionsCollection.ContainsKey(type))
                {
                    lock (locker)
                    {
                        if (!proxyOptionsCollection.ContainsKey(type))
                        {
                            AopProxyOptions proxyOptions = new AopProxyOptions
                            {
                                Instance = instance,
                                MethodInjectionDelegates = injectionDelegates
                            };

                            PrepareProxy<T>(proxyOptions);
                            proxyOptionsCollection.Add(type, proxyOptions);

                            proxiedInstance = CreateInstance(proxyOptions, instance);
                            proxyInstances[type] = proxiedInstance;
                            return proxiedInstance;
                        }
                    }
                }

                return reuseInstance ? proxyInstances[type] : CreateInstance(proxyOptionsCollection[type], instance);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { T = type.FullName });
            }
        }

        #region Make Proxy

        /// <summary>
        /// Prepares the proxy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proxyOptions">The proxy options.</param>
        static void PrepareProxy<T>(AopProxyOptions proxyOptions)
            where T : class
        {
            if (proxyOptions != null)
            {
                var type = typeof(T);
                var typeName = string.Format("{0}AopProxy", type.Name);

                try
                {
                    var aopAttribute = type.GetCustomAttribute<BaseAOPAttribute>(true);
                    proxyOptions.MethodInjectionDelegates = aopAttribute?.MethodInjectionDelegates ?? proxyOptions.MethodInjectionDelegates;

                    var generator = new AopProxyGenerator<T>(string.Format(ProxyNamespaceFormat, type.Namespace), typeName);
                    var code = generator.GenerateCode();

                    code.CheckEmptyString(nameof(code));

                    TempAssemblyProvider assemblyProvider = new TempAssemblyProvider();
                    assemblyProvider.CreateTempAssembly(code.AsArray(), new HashSet<string>(EnvironmentCore.DescendingAssemblyDependencyChain.Select(x => x.Location)));

                    proxyOptions.ProxiedType = ReflectionExtension.SmartGetType(typeName);
                }
                catch (Exception ex)
                {
                    throw new Beyova.ExceptionSystem.InitializationFailureException(typeName, ex, minor: "AopProxyGeneration", data: new { type = type?.FullName });
                }
            }
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proxyOptions">The proxy options.</param>
        /// <param name="instance">The instance.</param>
        /// <returns>System.Object.</returns>
        static object CreateInstance<T>(AopProxyOptions proxyOptions, T instance)
            where T : class
        {
            return (proxyOptions != null && instance != null) ? Activator.CreateInstance(proxyOptions.ProxiedType, instance, proxyOptions.MethodInjectionDelegates) : null;
        }

        #endregion
    }
}
