using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ifunction
{
    /// <summary>
    /// Interface for object IIdentifier.
    /// </summary>
    public interface IIdentifier
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        Guid? Key { get; set; }
    }
}
