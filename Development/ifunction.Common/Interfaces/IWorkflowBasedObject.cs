using System;
using ifunction.Model;

namespace ifunction
{
    /// <summary>
    /// Interface IWorkflowBasedObject
    /// </summary>
    public interface IWorkflowBasedObject : IIdentifier
    {
        #region Properties

        /// <summary>
        /// Gets or sets the state of the workflow.
        /// </summary>
        /// <value>The state of the workflow.</value>
        WorkflowState WorkflowState { get; set; }

        #endregion
    }
}
