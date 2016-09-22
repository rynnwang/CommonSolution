using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Beyova.BooleanSearch
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBooleanComputable
    {
        /// <summary>
        /// Computes the specified json.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        bool Compute(JObject json);
    }
}
