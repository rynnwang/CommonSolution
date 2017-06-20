namespace Beyova
{
    /// <summary>
    /// Abstract class for base object, with key, created stamp and last updated stamp.
    /// </summary>
    public abstract class BaseObject : SimpleBaseObject, IBaseObject
    {
        #region Properties

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the last updated by.
        /// </summary>
        /// <value>
        /// The last updated by.
        /// </value>
        public string LastUpdatedBy { get; set; }

        #endregion Properties
    }

    /// <summary>
    /// Class BaseObject.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseObject<T> : BaseObject
    {
        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        /// <value>The object.</value>
        public T Object { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseObject{T}"/> class.
        /// </summary>
        public BaseObject() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseObject{T}"/> class.
        /// </summary>
        /// <param name="genricObject">The genric object.</param>
        public BaseObject(T genricObject) : this()
        {
            Object = genricObject;
        }
    }
}