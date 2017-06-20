using System;
using System.Collections.Generic;

namespace Beyova.AOP
{
    /// <summary>
    /// Class MethodCallInfo
    /// </summary>
    public sealed class MethodCallInfo
    {
        /// <summary>
        /// Gets the in argument count.
        /// </summary>
        /// <value>
        /// The in argument count.
        /// </value>
        public int InArgCount { get { return InArgs?.Count ?? 0; } }

        /// <summary>
        /// Gets the in arguments.
        /// </summary>
        /// <value>
        /// The in arguments.
        /// </value>
        public Dictionary<string, object> InArgs { get; internal set; }

        /// <summary>
        /// Gets the out argument count.
        /// </summary>
        /// <value>
        /// The out argument count.
        /// </value>
        public int OutArgCount { get { return OutArgs?.Length ?? 0; } }

        /// <summary>
        /// Gets the out arguments.
        /// </summary>
        /// <value>
        /// The out arguments.
        /// </value>
        public object[] OutArgs { get; internal set; }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets the return value.
        /// </summary>
        /// <value>
        /// The return value.
        /// </value>
        public object ReturnValue { get; internal set; }

        /// <summary>
        /// Gets the full name of the method.
        /// </summary>
        /// <value>
        /// The full name of the method.
        /// </value>
        public string MethodFullName { get; internal set; }

        /// <summary>
        /// Gets the serializable argument names.
        /// </summary>
        /// <value>
        /// The serializable argument names.
        /// </value>
        public List<string> SerializableArgNames { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodCallInfo"/> class.
        /// </summary>
        internal MethodCallInfo()
        {
            this.SerializableArgNames = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodCallInfo" /> class.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="inArgs">The in arguments.</param>
        public MethodCallInfo(string methodName, Dictionary<string, object> inArgs) : this()
        {
            this.MethodFullName = methodName;
            this.InArgs = inArgs;
        }

        /// <summary>
        /// Gets the exception reference object.
        /// </summary>
        /// <returns></returns>
        public object GetExceptionReferenceObject()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            foreach (var one in SerializableArgNames)
            {
                result.Add(one, this.InArgs[one]);
            }

            return result;
        }
    }
}