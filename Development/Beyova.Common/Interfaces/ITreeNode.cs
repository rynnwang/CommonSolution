using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Interface ITreeNode
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITreeNode<T> : IIdentifier
    {
        /// <summary>
        /// Gets or sets the parent node key.
        /// </summary>
        /// <value>The parent node key.</value>
        Guid? ParentNodeKey { get; set; }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        List<T> Children { get; set; }
    }
}
