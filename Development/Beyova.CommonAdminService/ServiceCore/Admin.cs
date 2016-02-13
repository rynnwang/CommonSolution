using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova.CommonAdminService.DataAccessController;

namespace Beyova.CommonAdminService
{
    partial class CommonAdminService
    {
        static int adminExpiration = 120;

        /// <summary>
        /// Authenticates the admin user.
        /// </summary>
        /// <param name="accessCredential">The access credential.</param>
        /// <returns>AdminSession.</returns>
        public AdminAuthenticationResult AuthenticateAdminUser(AccessCredential accessCredential)
        {
            try
            {
                accessCredential.CheckNullObject("accessCredential");
                AdminUserInfo userInfo = null;

                using (var controller = new AdminUserInfoAccessController())
                {
                    userInfo = controller.AuthenticateAdminUser(accessCredential);
                }

                if (userInfo != null)
                {
                    using (var controller = new AdminSessionAccessController(adminExpiration))
                    {
                        var session = controller.CreateAdminSession(new AdminSession
                        {
                            IpAddress = ContextHelper.IpAddress,
                            UserAgent = ContextHelper.UserAgent,
                            OwnerKey = userInfo.Key
                        });

                        return new AdminAuthenticationResult
                        {
                            TokenExpiredStamp = session.ExpiredStamp,
                            Token = session.Token,
                            UserInfo = userInfo
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("AuthenticateAdminUser", accessCredential);
            }

            return null;
        }

        /// <summary>
        /// Signs the out.
        /// </summary>
        /// <param name="token">The token.</param>
        public void SignOut(string token)
        {
            try
            {
                token.CheckEmptyString("token");

                using (var controller = new AdminSessionAccessController(adminExpiration))
                {
                    controller.DeleteAdminSession(token);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("SignOut", token);
            }
        }

        /// <summary>
        /// Gets the admin user information by token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>AdminUserInfo.</returns>
        public AdminUserInfo GetAdminUserInfoByToken(string token)
        {
            try
            {
                token.CheckEmptyString("token");

                using (var controller = new AdminUserInfoAccessController())
                {
                    return controller.GetAdminUserInfoByToken(token);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("GetAdminUserInfoByToken", token);
            }
        }
    }
}
