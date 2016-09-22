using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class BaseCredential.
    /// </summary>
    public class BaseCredential : ICredential
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCredential"/> class.
        /// </summary>
        public BaseCredential() { }
    }
}
