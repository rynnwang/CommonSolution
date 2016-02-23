using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova
{
    /// <summary>
    /// Class MatrixList.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MatrixList<T> : Dictionary<string, List<T>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixList{T}"/> class.
        /// </summary>
        public MatrixList()
        {
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, T value)
        {
            try
            {
                key.CheckEmptyString("key");

                if (this.ContainsKey(key))
                {
                    this[key].Add(value);
                }
                else
                {
                    var list = new List<T>();
                    list.Add(value);
                    this.Add(key, list);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("Add", new { key, value });
            }
        }
    }
}
