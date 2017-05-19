using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum SqlScriptType
    {
        /// <summary>
        /// The undefined
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// The table
        /// </summary>
        Table = 1,
        /// <summary>
        /// The view
        /// </summary>
        View = 2,
        /// <summary>
        /// The function
        /// </summary>
        Function = 4,
        /// <summary>
        /// The stored procedure
        /// </summary>
        StoredProcedure = 8,
        /// <summary>
        /// The data
        /// </summary>
        Data = 0x10,
        /// <summary>
        /// All
        /// </summary>
        All = 0xFFFF
    }
}
