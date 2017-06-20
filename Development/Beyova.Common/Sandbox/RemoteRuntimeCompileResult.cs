using System;
using Beyova.ExceptionSystem;

namespace Beyova
{
    /// <summary>
    /// Class RemoteRuntimeCompileResult.
    /// </summary>
    [Serializable]
    public class RemoteRuntimeCompileResult
    {
        /// <summary>
        /// Gets or sets the name of the temporary assembly.
        /// </summary>
        /// <value>
        /// The name of the temporary assembly.
        /// </value>
        public string TempAssemblyName { get; set; }

        /// <summary>
        /// Gets or sets the compile exception information.
        /// </summary>
        /// <value>
        /// The compile exception information.
        /// </value>
        public string CompileExceptionInfo { get; set; }

        /// <summary>
        /// Gets the exception information.
        /// </summary>
        /// <returns></returns>
        public ExceptionInfo GetExceptionInfo()
        {
            return string.IsNullOrWhiteSpace(CompileExceptionInfo) ? null : CompileExceptionInfo.TryParseToJToken()?.ToObject<ExceptionInfo>();
        }
    }
}