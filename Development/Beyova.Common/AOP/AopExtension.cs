using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace Beyova.AOP
{
    /// <summary>
    ///
    /// </summary>
    public static class AopExtension
    {
        /// <summary>
        /// Fills the specified method call information.
        /// </summary>
        /// <param name="methodCallInfo">The method call information.</param>
        /// <param name="methodBase">The method base.</param>
        /// <param name="inArgsData">The in arguments data.</param>
        internal static void Fill(this MethodCallInfo methodCallInfo, MethodBase methodBase, object[] inArgsData = null)
        {
            if (methodCallInfo != null && methodBase != null)
            {
                methodCallInfo.MethodFullName = methodBase.GetFullName();
                var inArgs = new Dictionary<string, object>();

                var parameters = methodBase.GetParameters();

                for (var i = 0; i < parameters.Length; i++)
                {
                    inArgs.Add(parameters[i].Name, inArgsData == null ? null : inArgsData[i]);

                    var parameterType = parameters[i].ParameterType;
                    if (!(parameterType.IsContextful || parameterType.IsCOMObject))
                    {
                        methodCallInfo.SerializableArgNames.Add(parameters[i].Name);
                    }
                }

                methodCallInfo.InArgs = inArgs;
            }
        }

        /// <summary>
        /// To the method call information.
        /// </summary>
        /// <param name="methodMessage">The method message.</param>
        /// <returns></returns>
        public static MethodCallInfo ToMethodCallInfo(this IMethodCallMessage methodMessage)
        {
            if (methodMessage == null)
            {
                return null;
            }
            var result = new MethodCallInfo();
            result.Fill(methodMessage.MethodBase, methodMessage.Args);

            return result;
        }

        /// <summary>
        /// Fills the return message.
        /// </summary>
        /// <param name="callInfo">The call information.</param>
        /// <param name="methodMessage">The method message.</param>
        public static void FillReturnMessage(this MethodCallInfo callInfo, IMethodReturnMessage methodMessage)
        {
            if (callInfo != null && methodMessage != null)
            {
                callInfo.Exception = methodMessage.Exception;
                callInfo.OutArgs = methodMessage.OutArgs;
                callInfo.ReturnValue = methodMessage.ReturnValue;
            }
        }
    }
}