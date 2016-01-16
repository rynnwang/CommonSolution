using System;

namespace Beyova
{
    /// <summary>
    ///     Enum values for object status.
    /// </summary>
    [Flags]
    public enum ObjectState
    {
        /// <summary>
        ///     The value indicating that object is normal.(0x0)
        /// </summary>
        Normal = 0,
        /// <summary>
        ///     The value indicating that object is deleted logically. (0x1)
        /// </summary>
        Deleted = 0x01,
        /// <summary>
        ///     The value indicating that object is invisible. (0x2)
        /// </summary>
        Invisible = 0x02,
        /// <summary>
        ///     The value indicating that object is readonly. (0x4)
        /// </summary>
        ReadOnly = 0x04,
        /// <summary>
        ///     The value indicating that object is disabled. (0x8)
        /// </summary>
        Disabled = 0x08,
        /// <summary>
        ///     The value indicating that operation is succeed. (0x10)
        /// </summary>
        [Obsolete("Use WorkflowState instead.")]
        Succeed = 0x10,
        /// <summary>
        ///     The value indicating that operation is failed. (0x11)
        /// </summary>
        [Obsolete("Use WorkflowState instead.")]
        Failed = 0x11,
        /// <summary>
        ///     The value indicating that object or operation is pending. (0x100)
        /// </summary>
        [Obsolete("Use WorkflowState instead.")]
        Pending = 0x100,
        /// <summary>
        ///     The value indicating that object or operation is approved. (0x110)
        /// </summary>
        [Obsolete("Use WorkflowState instead.")]
        Approved = 0x110,
        /// <summary>
        ///     The value indicating that object or operation is rejected. (0x120)
        /// </summary>
        [Obsolete("Use WorkflowState instead.")]
        Rejected = 0x120,
        /// <summary>
        ///     The value indicating that operation is in process. (0x140)
        /// </summary>
        [Obsolete("Use WorkflowState instead.")]
        InProcess = 0x140,
        /// <summary>
        ///     The value indicating that object or operation is rejected. (0x180)
        /// </summary>
        [Obsolete("Use WorkflowState instead.")]
        Completed = 0x180
    }
}