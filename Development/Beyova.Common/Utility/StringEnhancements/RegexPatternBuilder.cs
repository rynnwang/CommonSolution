using System.Text;

namespace Beyova
{
    /// <summary>
    /// Class RegexPatternBuilder. This class cannot be inherited.
    /// </summary>
    public sealed class RegexPatternBuilder
    {
        /// <summary>
        /// The builder
        /// </summary>
        private StringBuilder builder = new StringBuilder();

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexPatternBuilder"/> class.
        /// </summary>
        public RegexPatternBuilder() { }

        /// <summary>
        /// Appends the static.
        /// </summary>
        /// <param name="staticString">The static string.</param>
        public void AppendStatic(string staticString)
        {
            builder.AppendFormat(InternalGetPatternByStatic(staticString));
        }

        /// <summary>
        /// Appends the optional static.
        /// </summary>
        /// <param name="staticString">The static string.</param>
        public void AppendOptionalStatic(string staticString)
        {
            InternalAppendOptionalPattern(InternalGetPatternByStatic(staticString));
        }

        /// <summary>
        /// Appends the variable.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="constraint">The constraint.</param>
        public void AppendVariable(string name, string constraint = StringShifterExtension.defaultConstraint)
        {
            builder.Append(InternalGetVariable(name, constraint));
        }

        /// <summary>
        /// Appends the optional variable.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="constraint">The constraint.</param>
        public void AppendOptionalVariable(string name, string constraint = StringShifterExtension.defaultConstraint)
        {
            InternalAppendOptionalPattern(InternalGetVariable(name, constraint));
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return builder.ToString();
        }

        #region Private

        /// <summary>
        /// Internals the get pattern by static.
        /// </summary>
        /// <param name="staticString">The static string.</param>
        /// <returns>System.String.</returns>
        private string InternalGetPatternByStatic(string staticString)
        {
            return staticString.StaticStringToRegexPattern();
        }

        /// <summary>
        /// Internals the get variable.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="constraint">The constraint.</param>
        /// <returns>System.String.</returns>
        private string InternalGetVariable(string name, string constraint)
        {
            constraint.CheckEmptyString(nameof(constraint));

            return string.Format("(?<{0}>({1}))", name, constraint);
        }

        /// <summary>
        /// Appends the optional pattern.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        private void InternalAppendOptionalPattern(string pattern)
        {
            builder.AppendFormat("({0})?", pattern);
        }

        /// <summary>
        /// Appends the pattern.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        private void InternalAppendPattern(string pattern)
        {
            builder.Append(pattern);
        }

        #endregion Private
    }
}