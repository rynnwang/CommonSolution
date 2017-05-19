using System;

namespace Beyova
{
    /// <summary>
    /// Class SimpleBaseObject
    /// </summary>
    public abstract class SimpleBaseObject : ISimpleBaseObject
    {
        #region Properties

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>
        /// The created stamp.
        /// </value>
        public DateTime? CreatedStamp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last updated stamp.
        /// </summary>
        /// <value>
        /// The last updated stamp.
        /// </value>
        public DateTime? LastUpdatedStamp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public ObjectState State { get; set; }

        #endregion
    }

    /// <summary>
    /// Class SimpleBaseObject.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleBaseObject<T> : SimpleBaseObject
    {
        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        /// <value>The object.</value>
        public T Object { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleBaseObject{T}"/> class.
        /// </summary>
        public SimpleBaseObject() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleBaseObject{T}"/> class.
        /// </summary>
        /// <param name="genricObject">The genric object.</param>
        public SimpleBaseObject(T genricObject) : this()
        {
            Object = genricObject;
        }
    }
}
