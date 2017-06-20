using System;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityContext.
    /// </summary>
    public static class GravityContext
    {
        [ThreadStatic]
        private static ProjectInfo _currentProjectInfo = null;

        /// <summary>
        /// The project key
        /// </summary>
        [ThreadStatic]
        private static Guid? _projectKey;

        /// <summary>
        /// Gets or sets the project key.
        /// </summary>
        /// <value>The project key.</value>
        public static Guid? ProjectKey
        {
            get { return _projectKey; }
            internal set { _projectKey = value; }
        }

        /// <summary>
        /// Gets the project information.
        /// </summary>
        /// <value>The project information.</value>
        public static ProjectInfo ProjectInfo
        {
            get { return _currentProjectInfo; }
            internal set { _currentProjectInfo = value; }
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        internal static void Dispose()
        {
            _projectKey = null;
            _currentProjectInfo = null;
        }
    }
}