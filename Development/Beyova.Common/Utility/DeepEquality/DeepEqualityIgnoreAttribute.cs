using System;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class DeepEqualityIgnoreAttribute : Attribute
    {
    }
}