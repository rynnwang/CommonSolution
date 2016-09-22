using System;
using Beyova.ExceptionSystem;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class SandboxMarshalInvokeResult.
    /// </summary>
    public sealed class SandboxInvokeResult
    {
        /// <summary>
        /// The value
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// The type
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// The exception information
        /// </summary>
        public string ExceptionInfo { get; private set; }

        /// <summary>
        /// Prevents a default instance of the <see cref="SandboxInvokeResult"/> class from being created.
        /// </summary>
        private SandboxInvokeResult() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SandboxInvokeResult"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        internal SandboxInvokeResult(SandboxMarshalInvokeResult result)
        {
            if (result != null)
            {
                this.Type = result.Type;
                this.ExceptionInfo = result.ExceptionInfo;
                this.Value = result.Value;
            }
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <returns>System.Object.</returns>
        public object GetObject()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.Value) || string.IsNullOrWhiteSpace(this.Type))
                {
                    return null;
                }

                Type type = ReflectionExtension.SmartGetType(this.Type, true);
                return JToken.Parse(this.Value).ToObject(type);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { this.Value, this.Type });
            }
        }

        /// <summary>
        /// Gets the exception information.
        /// </summary>
        /// <returns>ExceptionInfo.</returns>
        public ExceptionInfo GetExceptionInfo()
        {
            return string.IsNullOrWhiteSpace(this.ExceptionInfo) ? null : JToken.Parse(this.ExceptionInfo).ToObject<ExceptionInfo>();
        }
    }
}
