using System;
using Newtonsoft.Json.Linq;

namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Class WorkflowOperationException.
    /// </summary>
    public class WorkflowOperationException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowOperationException" /> class.
        /// </summary>
        /// <param name="workflowName">Name of the workflow.</param>
        /// <param name="state">The state.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="scene">The scene.</param>
        public WorkflowOperationException(string workflowName, string state, Exception innerException = null, object data = null, FriendlyHint hint = null, ExceptionScene scene = null)
            : base(string.Format("Operation [{0}] on workflow [{1}] is forbidden due to state [{2}] against the workflow.", scene?.MethodName, workflowName, state),
                  new ExceptionCode { Major = ExceptionCode.MajorCode.OperationForbidden, Minor = "Workflow" }, innerException, data, hint, scene)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationForbiddenException" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="createdStamp">The created stamp.</param>
        /// <param name="message">The message.</param>
        /// <param name="scene">The scene.</param>
        /// <param name="code">The code.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="operatorCredential">The operator credential.</param>
        /// <param name="data">The data.</param>
        /// <param name="hint">The hint.</param>
        internal WorkflowOperationException(Guid key, DateTime createdStamp, string message, ExceptionScene scene, ExceptionCode code, Exception innerException, BaseCredential operatorCredential, JToken data, FriendlyHint hint)
          : base(key, createdStamp, message, scene, code, innerException, operatorCredential, data, hint)
        {
        }

        #endregion Constructor
    }
}