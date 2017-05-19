using System;
using System.Collections.Generic;
using Beyova.CommonService.DataAccessController;
using Beyova.CommonServiceInterface;
using Beyova.ExceptionSystem;

namespace Beyova.CommonService
{
    /// <summary>
    /// Class BaseAuthenticationServiceCore.
    /// </summary>
    /// <typeparam name="TUserInfo">The type of the t user information.</typeparam>
    /// <typeparam name="TUserCriteria">The type of the t user criteria.</typeparam>
    /// <typeparam name="TAuthenticationResult">The type of the t authentication result.</typeparam>
    /// <typeparam name="TFunctionalRole">The type of the t functional role.</typeparam>
    /// <typeparam name="TUserInfoAccessController">The type of the t user information access controller.</typeparam>
    public abstract class BaseAuthenticationServiceCore<TUserInfo, TUserCriteria, TAuthenticationResult, TFunctionalRole, TUserInfoAccessController>
        : IAuthenticationProfileService<TUserInfo, TUserCriteria, TAuthenticationResult, TFunctionalRole>
        where TUserInfo : IUserInfo<TFunctionalRole>, IBaseObject, new()
        where TUserCriteria : IUserCriteria<TFunctionalRole>, new()
        where TAuthenticationResult : IAuthenticationResult<TUserInfo, TFunctionalRole>, new()
        where TFunctionalRole : struct, IConvertible
        where TUserInfoAccessController : UserInfoAccessController<TUserInfo, TUserCriteria, TFunctionalRole>, new()
    {
        /// <summary>
        /// Gets the token expired stamp.
        /// </summary>
        /// <returns>System.Nullable&lt;DateTime&gt;.</returns>
        protected abstract DateTime? GetTokenExpiredStamp();

        /// <summary>
        /// Authenticates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>TAuthenticationResult.</returns>
        /// <exception cref="UnauthorizedAccountException">InvalidCredential;new ExceptionSystem.FriendlyHint { Message = Check credential and retry., HintCode = InvalidCredential }</exception>
        /// <exception cref="ExceptionSystem.FriendlyHint"></exception>
        public virtual TAuthenticationResult Authenticate(AuthenticationRequest request)
        {
            try
            {
                request.CheckNullObject(nameof(request));
                request.AccessIdentifier.CheckEmptyString("request.AccessIdentifier");
                request.Token.CheckEmptyString("request.Token");

                using (var accessCredentialAccessController = new AccessCredentialInfoAccessController())
                {
                    var accessCredentialInfo = accessCredentialAccessController.AuthenticateAccessCredential(request);

                    if (accessCredentialInfo != null)
                    {
                        var result = new TAuthenticationResult();

                        using (var sessionAccessController = new SessionInfoAccessController())
                        {
                            var sessionInfo = sessionAccessController.CreateSessionInfo(new SessionInfo
                            {
                                DeviceType = request.DeviceType ?? DeviceType.None,
                                IpAddress = ContextHelper.IpAddress,
                                Platform = request.Platform ?? PlatformType.Undefined,
                                UserAgent = ContextHelper.UserAgent,
                                UserKey = accessCredentialInfo.UserKey,
                                ExpiredStamp = GetTokenExpiredStamp()
                            });

                            result.TokenExpiredStamp = sessionInfo.ExpiredStamp;
                            result.Token = sessionInfo.Token;
                        }

                        using (var userAccessController = new TUserInfoAccessController())
                        {
                            result.UserInfo = userAccessController.GetUserInfoByKey(accessCredentialInfo.UserKey);
                        }

                        return result;
                    }
                    else
                    {
                        throw new UnauthorizedAccountException("InvalidCredential", new { request.AccessIdentifier }, new ExceptionSystem.FriendlyHint { Message = "Check credential and retry.", HintCode = "InvalidCredential" });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(request);
            }
        }

        /// <summary>
        /// Gets the current session information.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>SessionInfo.</returns>
        public virtual SessionInfo GetSessionInfoByToken(string token)
        {
            try
            {
                token.CheckEmptyString("token");

                using (var controller = new SessionInfoAccessController())
                {
                    return controller.GetSessionInfoByToken(token);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(token);
            }
        }

        /// <summary>
        /// Gets the user information by token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>TUserInfo.</returns>
        public virtual TUserInfo GetUserInfoByToken(string token)
        {
            try
            {
                token.CheckEmptyString(nameof(token));

                using (var controller = new TUserInfoAccessController())
                {
                    return controller.GetUserByToken(token);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(token);
            }
        }

        /// <summary>
        /// Gets the current user profile.
        /// </summary>
        /// <param name="userKey">The user key.</param>
        /// <returns>UserLogin.</returns>
        public virtual TUserInfo GetUserInfoByUserKey(Guid? userKey)
        {
            try
            {
                userKey.CheckNullObject(nameof(userKey));

                using (var controller = new TUserInfoAccessController())
                {
                    return controller.GetUserInfoByKey(userKey);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(userKey);
            }
        }

        /// <summary>
        /// Logouts the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        public virtual void Logout(string token)
        {
            try
            {
                token.CheckEmptyString(nameof(token));

                using (var controller = new SessionInfoAccessController())
                {
                    controller.DisposeSessionInfo(token);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(token);
            }
        }

        /// <summary>
        /// Queries the session information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>System.Collections.Generic.List&lt;Beyova.SessionInfo&gt;.</returns>
        public virtual List<SessionInfo> QuerySessionInfo(SessionCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject(nameof(criteria));

                using (var controller = new SessionInfoAccessController())
                {
                    return controller.QuerySessionInfo(criteria);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Queries the user information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>System.Collections.Generic.List&lt;TUserInfo&gt;.</returns>
        public virtual List<TUserInfo> QueryUserInfo(TUserCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject(nameof(criteria));

                using (var controller = new TUserInfoAccessController())
                {
                    return controller.QueryUserInfo(criteria);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Renews the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>TAuthenticationResult.</returns>
        public virtual TAuthenticationResult RenewToken(string token)
        {
            try
            {
                token.CheckEmptyString(nameof(token));

                var userInfo = GetUserInfoByToken(token);
                if (userInfo == null)
                {
                    throw new OperationForbiddenException("RenewToken", "InvalidToken", data: token);
                }

                var result = new TAuthenticationResult() { };
                using (var controller = new SessionInfoAccessController())
                {
                    var sessionInfo = controller.CreateSessionInfo(new SessionInfo
                    {
                        IpAddress = ContextHelper.IpAddress,
                        ExpiredStamp = GetTokenExpiredStamp(),
                        UserAgent = ContextHelper.UserAgent,
                        UserKey = userInfo.Key
                    });

                    result.TokenExpiredStamp = sessionInfo.ExpiredStamp;
                    result.Token = sessionInfo.Token;

                    controller.DisposeSessionInfo(token);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle(token);
            }
        }
    }
}
