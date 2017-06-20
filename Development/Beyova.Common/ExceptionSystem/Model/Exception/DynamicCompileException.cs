using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Class DynamicCompileException.
    /// </summary>
    public class DynamicCompileException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicCompileException"/> class.
        /// </summary>
        /// <param name="errorCollection">The error collection.</param>
        /// <param name="sourceCode">The source code.</param>
        public DynamicCompileException(CompilerErrorCollection errorCollection, string sourceCode) : this(errorCollection, sourceCode.AsArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicCompileException" /> class.
        /// </summary>
        /// <param name="errorCollection">The error collection.</param>
        /// <param name="sourceCodes">The source code.</param>
        public DynamicCompileException(CompilerErrorCollection errorCollection, string[] sourceCodes)
            : base(string.Format("Failed to complete dynamic complile. "), new ExceptionCode { Major = ExceptionCode.MajorCode.OperationFailure, Minor = "DynamicCompile" }, data: new { Errors = ErrorCollectionToStringArray(errorCollection), sourceCodes })
        {
        }

        #endregion Constructor

        /// <summary>
        /// Errors the collection to string array.
        /// </summary>
        /// <param name="errorCollection">The error collection.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        private static List<string> ErrorCollectionToStringArray(CompilerErrorCollection errorCollection)
        {
            List<string> errors = new List<string>();
            foreach (CompilerError one in errorCollection)
            {
                errors.Add(one.ErrorText);
            }

            return errors;
        }
    }
}