﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova;

namespace Beyova.Utility
{
    /// <summary>
    /// Class ValueDiffResult
    /// </summary>
    public class ValueDiffResult : ValueDiffResult<string>
    {
    }

    /// <summary>
    /// Class ValueDiffResult
    /// </summary>
    public class ValueDiffResult<T>
    {
        /// <summary>
        /// Gets or sets the source value.
        /// </summary>
        /// <value>
        /// The source value.
        /// </value>
        public T SourceValue { get; set; }

        /// <summary>
        /// Gets or sets the destination value.
        /// </summary>
        /// <value>
        /// The destination value.
        /// </value>
        public T DestinationValue { get; set; }

        /// <summary>
        /// Gets or sets the difference result.
        /// </summary>
        /// <value>
        /// The difference result.
        /// </value>
        public DiffResult DiffResult { get; set; }
    }
}
