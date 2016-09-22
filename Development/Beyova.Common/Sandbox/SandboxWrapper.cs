using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class SandboxWrapper.
    /// </summary>
    public abstract class SandboxWrapper
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid Key { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SandboxWrapper"/> class.
        /// </summary>
        protected SandboxWrapper() { }
    }

    /// <summary>
    /// Class SandboxWrapper.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SandboxWrapper<T> : SandboxWrapper
    {
        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        /// <value>The object.</value>
        public T Object { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SandboxWrapper{T}"/> class.
        /// </summary>
        /// <param name="objectToWrap">The object to wrap.</param>
        public SandboxWrapper(T objectToWrap) : this()
        {
            this.Object = objectToWrap;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SandboxWrapper{T}"/> class.
        /// </summary>
        protected SandboxWrapper()
        {
            SandboxWrapperContainer.Register(this);
        }
    }
}
