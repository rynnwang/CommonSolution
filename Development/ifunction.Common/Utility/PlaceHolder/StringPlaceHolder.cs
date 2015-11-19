using System;
using System.Collections.Generic;

namespace ifunction
{
    /// <summary>
    /// Class StringPlaceHolder.
    /// <remarks>This class is used for <see cref="StringHolderReplacement"/>.</remarks>
    /// </summary>
    public class StringPlaceHolder
    {
        /// <summary>
        /// Gets or sets the place key.
        /// </summary>
        /// <value>
        /// The place key.
        /// </value>
        public string PlaceKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the additional parameters.
        /// </summary>
        /// <value>
        /// The additional parameters.
        /// </value>
        public List<string> AdditionalParameters
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringPlaceHolder"/> class.
        /// </summary>
        public StringPlaceHolder()
        {
            this.AdditionalParameters = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringPlaceHolder"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="additionalParameters">The additional parameters.</param>
        public StringPlaceHolder(string key, string additionalParameters)
            : this()
        {
            this.PlaceKey = key;
            this.AdditionalParameters.AddRange(additionalParameters.SafeToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
        }

        /// <summary>
        /// To the statement.
        /// </summary>
        /// <returns></returns>
        public string ToStatement()
        {
            return StringHolderReplacement.GeneratePlaceHolderSign(this.PlaceKey, AdditionalParameters.ToArray());
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return this.PlaceKey.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            StringPlaceHolder holder = obj as StringPlaceHolder;
            var result = holder != null && holder.PlaceKey == this.PlaceKey;

            if (result)
            {
                if (holder.AdditionalParameters == null || this.AdditionalParameters == null)
                {
                    return true;
                }
            }
            else if (holder.AdditionalParameters != null && this.AdditionalParameters != null && holder.AdditionalParameters.Count == this.AdditionalParameters.Count)
            {
                for (int i = 0; i < this.AdditionalParameters.Count; i++)
                {
                    if (holder.AdditionalParameters[i] != this.AdditionalParameters[i])
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
