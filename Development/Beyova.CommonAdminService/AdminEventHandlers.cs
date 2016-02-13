using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova.RestApi;

namespace Beyova.CommonAdminService
{
    /// <summary>
    /// Class AdminEventHandlers.
    /// </summary>
    public class AdminEventHandlers : RestApiEventHandlers
    {
        static CommonAdminService service = new CommonAdminService();
        public override ICredential GetCredentialBySecuredKey(string securedKey, out string privateKey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the credential by token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>ICredential.</returns>
        public override ICredential GetCredentialByToken(string token)
        {
            return string.IsNullOrWhiteSpace(token) ? null : service.GetAdminUserInfoByToken(token);
        }
    }
}
