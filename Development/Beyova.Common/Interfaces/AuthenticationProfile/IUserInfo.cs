using System;

namespace Beyova
{
    /// <summary>
    /// Interface IUserInfo
    /// </summary>
    public interface IUserInfo<TFunctionalRole> : IIdentifier, ICredential, IUserInfo
        where TFunctionalRole : struct, IConvertible
    {
        /// <summary>
        /// Gets or sets the functional role.
        /// </summary>
        /// <value>The functional role.</value>
        TFunctionalRole FunctionalRole { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>The gender.</value>
        Gender Gender { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>The language.</value>
        string Language { get; set; }

        /// <summary>
        /// Gets or sets the time zone. Unit: minute
        /// </summary>
        /// <value>The time zone.</value>
        int? TimeZone { get; set; }
    }

    /// <summary>
    /// Interface IUserInfo
    /// </summary>
    public interface IUserInfo : IIdentifier, ICredential, IPermissionIdentifiers
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        string Email { get; set; }

        /// <summary>
        /// Gets or sets the avatar key.
        /// </summary>
        /// <value>The avatar key.</value>
        Guid? AvatarKey { get; set; }

        /// <summary>
        /// Gets or sets the avatar URL.
        /// </summary>
        /// <value>The avatar URL.</value>
        string AvatarUrl { get; set; }
    }
}
