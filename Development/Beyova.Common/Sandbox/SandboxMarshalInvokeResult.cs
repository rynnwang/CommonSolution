using System;
using Beyova.ExceptionSystem;

namespace Beyova
{
    /// <summary>
    /// Class SandboxMarshalInvokeResult.
    /// </summary>
    [Serializable]
    public class SandboxMarshalInvokeResult : SandboxMarshalObject
    {
        /// <summary>
        /// The value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The exception information
        /// </summary>
        public string ExceptionInfo { get; set; }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        public void SetValue<T>(T value)
        {
            var type = typeof(T);
            if (type == typeof(object))
            {
                if (value != null)
                {
                    type = value.GetType();
                }
            }
            this.Type = type.GetFullName();
            this.Value = value.ToJson();
        }

        /// <summary>
        /// Sets the exception.
        /// </summary>
        /// <param name="exceptionInfo">The exception information.</param>
        public void SetException(ExceptionInfo exceptionInfo)
        {
            this.ExceptionInfo = exceptionInfo?.ToJson();
        }

        /// <summary>
        /// Sets the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void SetException(BaseException exception)
        {
            SetException(exception?.ToExceptionInfo());
        }
    }
}
