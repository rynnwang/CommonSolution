﻿namespace Beyova
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