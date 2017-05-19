namespace Beyova.RestApi
{
    /// <summary>
    /// Class RestApiEventHandlers. This class is to save sensitive event handlers for <see cref="ApiHandlerBase"/> or <see cref="RestApiRouter"/>. Such as initialize thread user info by token.
    /// </summary>
    public abstract class RestApiEventHandlers
    {
        /// <summary>
        /// Gets the credential by token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>ICredential.</returns>
        public abstract ICredential GetCredentialByToken(string token);
    }
}
