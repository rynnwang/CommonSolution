﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ifunction;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// Enum ApiContractDataType
    /// </summary>
    public enum ApiContractDataType
    {
        /// <summary>
        /// Value indicating it is Undefined
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// Value indicating it is string
        /// </summary>
        String = 1,
        /// <summary>
        /// Value indicating it is integer
        /// </summary>
        Integer = 2,
        /// <summary>
        /// Value indicating it is float
        /// </summary>
        Float = 3,
        /// <summary>
        /// Value indicating it is decimal
        /// </summary>
        Decimal = 4,
        /// <summary>
        /// Value indicating it is date time
        /// </summary>
        DateTime = 5,
        /// <summary>
        /// Value indicating it is boolean
        /// </summary>
        Boolean = 6,
        /// <summary>
        /// Value indicating it is time span
        /// </summary>
        TimeSpan = 7,
        /// <summary>
        /// Value indicating it is unique identifier
        /// </summary>
        Guid = 8,
        /// <summary>
        /// Value indicating it is URI
        /// </summary>
        Uri = 9,
        /// <summary>
        /// Value indicating it is enum
        /// </summary>
        Enum = 10,
        /// <summary>
        /// Value indicating it is binary
        /// </summary>
        Binary = 11,
        /// <summary>
        /// Value indicating it is array
        /// </summary>
        Array = 16,
        /// <summary>
        /// Value indicating it is dictionary
        /// </summary>
        Dictionary = 17,
        /// <summary>
        /// Value indicating it is complex object
        /// </summary>
        ComplexObject = 18,
        /// <summary>
        /// Any json
        /// </summary>
        AnyJson = 19
    }
}
