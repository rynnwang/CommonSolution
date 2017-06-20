using System;
using System.Collections.Generic;
using Beyova.ExceptionSystem;
using Beyova.RestApi;

namespace Beyova
{
    /// <summary>
    /// Class SandboxInvoker. This class is designed to be called inside sandbox.
    /// </summary>
    [Serializable]
    public class SandboxInvoker : SandboxMarshalObject
    {
        /// <summary>
        /// Loads the name of the assembly by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>System.String.</returns>
        public string LoadAssemblyByName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                try
                {
                    AppDomain.CurrentDomain.Load(name);
                }
                catch (Exception ex)
                {
                    return ex.ToExceptionInfo().ToJson();
                }
            }

            return null;
        }

        /// <summary>
        /// Creates the instance and invoke method.
        /// </summary>
        /// <param name="typeFullName">Name of the type.</param>
        /// <param name="constructorParameters">The constructor parameters.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="methodParameters">The method parameters.</param>
        /// <returns>RemoteInvokeResult.</returns>
        /// <exception cref="InvalidObjectException"></exception>
        public SandboxMarshalInvokeResult CreateInstanceAndInvokeMethod(string typeFullName, object[] constructorParameters, string methodName, params object[] methodParameters)
        {
            SandboxMarshalInvokeResult result = new SandboxMarshalInvokeResult();

            try
            {
                typeFullName.CheckEmptyString(nameof(typeFullName));
                methodName.CheckEmptyString(nameof(methodName));

                var type = ReflectionExtension.SmartGetType(typeFullName);
                type.CheckNullObject(nameof(type));

                var instance = Activator.CreateInstance(type, (object[])constructorParameters);

                var methodInfo = type.GetMethod(methodName);
                methodInfo.CheckNullObject(nameof(methodInfo));

                if (methodInfo.IsStatic)
                {
                    throw new InvalidObjectException(nameof(methodName), data: methodName, reason: "Method is static");
                }

                result.SetValue(methodInfo.Invoke(instance, methodParameters));
            }
            catch (Exception ex)
            {
                result.SetException(ex.Handle(new { typeFullName, constructorParameters, methodName, methodParameters }));
            }

            return result;
        }

        /// <summary>
        /// Invokes the static method.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="methodParameters">The method parameters.</param>
        /// <returns>RemoteInvokeResult.</returns>
        /// <exception cref="InvalidObjectException"></exception>
        public SandboxMarshalInvokeResult InvokeStaticMethod(string typeName, string methodName, params object[] methodParameters)
        {
            SandboxMarshalInvokeResult result = new SandboxMarshalInvokeResult();

            try
            {
                typeName.CheckEmptyString(nameof(typeName));
                methodName.CheckEmptyString(nameof(methodName));

                var type = ReflectionExtension.SmartGetType(typeName);
                type.CheckNullObject(nameof(type));

                var methodInfo = type.GetMethod(methodName);
                methodInfo.CheckNullObject(nameof(methodInfo));

                if (!methodInfo.IsStatic)
                {
                    throw new InvalidObjectException(nameof(methodName), data: methodName, reason: "Method is not static");
                }

                result.SetValue(methodInfo.Invoke(null, methodParameters));
            }
            catch (Exception ex)
            {
                result.SetException(ex.Handle(new { typeName, methodName, methodParameters }));
            }

            return result;
        }

        /// <summary>
        /// Gets the API contract interfaces.
        /// </summary>
        /// <returns>RemoteInvokeResult.</returns>
        public SandboxMarshalInvokeResult GetApiContractInterfaces()
        {
            SandboxMarshalInvokeResult result = new SandboxMarshalInvokeResult();

            try
            {
                Dictionary<string, string> interfaces = new Dictionary<string, string>();

                foreach (var one in EnvironmentCore.DescendingAssemblyDependencyChain)
                {
                    foreach (var item in one.GetTypes())
                    {
                        if (item.IsInterface && item.HasAttribute<ApiContractAttribute>(false) && !item.IsGenericTypeDefinition)
                        {
                            interfaces.Merge(item.GetFullName(), item.Name, false);
                        }
                    }
                }

                result.SetValue(interfaces);
            }
            catch (Exception ex)
            {
                result.SetException(ex.Handle());
            }

            return result;
        }
    }
}