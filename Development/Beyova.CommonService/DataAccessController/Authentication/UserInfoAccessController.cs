using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Beyova;

namespace Beyova.CommonService.DataAccessController
{
    /// <summary>
    /// Class UserInfoAccessController.
    /// </summary>
    /// <typeparam name="TUserInfo">The type of the t user information.</typeparam>
    /// <typeparam name="TUserCriteria">The type of the t user criteria.</typeparam>
    /// <typeparam name="TFunctionalRole">The type of the t functional role.</typeparam>
    public abstract class UserInfoAccessController<TUserInfo, TUserCriteria, TFunctionalRole> : BaseAuthenticationController<TUserInfo, TUserCriteria>
        where TUserInfo : IUserInfo<TFunctionalRole>, IBaseObject, new()
        where TUserCriteria : IUserCriteria<TFunctionalRole>, new()
        where TFunctionalRole : struct, IConvertible
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfoAccessController{TUserInfo, TUserCriteria, TFunctionalRole}"/> class.
        /// </summary>
        public UserInfoAccessController()
            : base()
        {
        }

        #endregion

        /// <summary>
        /// Converts the entity object.
        /// </summary>
        /// <param name="sqlDataReader">The SQL data reader.</param>
        /// <returns>
        /// UserInfo.
        /// </returns>
        protected override TUserInfo ConvertEntityObject(SqlDataReader sqlDataReader)
        {
            var result = new TUserInfo
            {
                Name = sqlDataReader[column_Name].ObjectToString(),
                Gender = sqlDataReader[column_Gender].ObjectToInt32().Int32ToEnum<Gender>(),
                Email = sqlDataReader[column_Email].ObjectToString(),
                AvatarKey = sqlDataReader[column_AvatarKey].ObjectToGuid(),
                FunctionalRole = sqlDataReader[column_FunctionalRole].ObjectToInt32().Int32ToEnum<TFunctionalRole>(),
                Language = sqlDataReader[column_Language].ObjectToString(),
                TimeZone = sqlDataReader[column_TimeZone].ObjectToInt32()
            };

            this.FillAdditionalFieldValue(result, sqlDataReader);
            FillBaseObjectFields(result, sqlDataReader);

            return result;
        }

        /// <summary>
        /// Gets the user by token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>UserInfo.</returns>
        public TUserInfo GetUserByToken(string token)
        {
            const string spName = "sp_GetUserByToken";

            try
            {
                token.CheckEmptyString("token");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_Token, token)
                };

                return this.ExecuteReader(spName, parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle( token);
            }
        }

        /// <summary>
        /// Queries the access credential.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;AccessCredentialInfo&gt;.</returns>
        public List<TUserInfo> QueryUserInfo(TUserCriteria criteria)
        {
            const string spName = "sp_QueryUserInfo";

            try
            {
                criteria.CheckNullObject("criteria");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_Key, criteria.Key),
                    this.GenerateSqlSpParameter(column_Name, criteria.Name),
                    this.GenerateSqlSpParameter(column_Email, criteria.Email),
                    this.GenerateSqlSpParameter(column_FunctionalRole, criteria.FunctionalRole.EnumToInt32()),
                    this.GenerateSqlSpParameter(column_StartIndex, criteria.StartIndex),
                    this.GenerateSqlSpParameter(column_Count, criteria.Count>1?criteria.Count:50)
                };

                FillAdditionalFieldValue(parameters, criteria);

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle( criteria);
            }
        }

        /// <summary>
        /// Updates the user avatar.
        /// </summary>
        /// <param name="userKey">The user key.</param>
        /// <param name="avatarKey">The avatar key.</param>
        /// <param name="operatorKey">The operator key.</param>
        /// <returns>List&lt;UserInfo&gt;.</returns>
        public TUserInfo UpdateUserAvatar(Guid? userKey, Guid? avatarKey, Guid? operatorKey)
        {
            const string spName = "sp_UpdateUserAvatar";

            try
            {
                userKey.CheckNullObject("userKey");
                avatarKey.CheckNullObject("avatarKey");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_UserKey,userKey),
                    this.GenerateSqlSpParameter(column_AvatarKey, avatarKey),
                    this.GenerateSqlSpParameter(column_OperatorKey, operatorKey)
                };

                return this.ExecuteReader(spName, parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle( new { userKey, avatarKey, operatorKey });
            }
        }

        /// <summary>
        /// Gets the user information by keys.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <returns>List&lt;UserInfo&gt;.</returns>
        public List<TUserInfo> GetUserInfoByKeys(ICollection<Guid> keys)
        {
            const string spName = "sp_GetUserInfoByKeys";

            try
            {
                keys.CheckNullObject("keys");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_Xml, keys.ListToXml())
                };

                return this.ExecuteReader(spName, parameters);
            }
            catch (Exception ex)
            {
                throw ex.Handle( keys);
            }
        }

        /// <summary>
        /// Gets the user information by key.
        /// </summary>
        /// <param name="userKey">The user key.</param>
        /// <returns>UserInfo.</returns>
        public TUserInfo GetUserInfoByKey(Guid? userKey)
        {
            try
            {
                return userKey == null ? default(TUserInfo) : QueryUserInfo(new TUserCriteria
                {
                    Key = userKey
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle( userKey);
            }
        }

        /// <summary>
        /// Updates the user information.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        /// <param name="operatorKey">The operator key.</param>
        /// <returns>TUserInfo.</returns>
        public TUserInfo CreateOrUpdateUserInfo(TUserInfo userInfo, Guid? operatorKey)
        {
            const string spName = "sp_CreateOrUpdateUserInfo";

            try
            {
                userInfo.CheckNullObject("userInfo");

                var parameters = new List<SqlParameter>
                {
                    this.GenerateSqlSpParameter(column_Key,userInfo.Key),
                    this.GenerateSqlSpParameter(column_Gender,userInfo.Gender.EnumToInt32()),
                    this.GenerateSqlSpParameter(column_Email,userInfo.Email),
                    this.GenerateSqlSpParameter(column_AvatarKey,userInfo.AvatarKey),
                    this.GenerateSqlSpParameter(column_AvatarUrl,userInfo.AvatarUrl),
                    this.GenerateSqlSpParameter(column_FunctionalRole,userInfo.FunctionalRole.EnumToInt32()),
                    this.GenerateSqlSpParameter(column_Language,userInfo.Language),
                    this.GenerateSqlSpParameter(column_TimeZone,userInfo.TimeZone),
                    this.GenerateSqlSpParameter(column_OperatorKey,operatorKey),
                };

                FillAdditionalFieldValue(parameters, userInfo);

                return this.ExecuteReader(spName, parameters).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle( new { userInfo, operatorKey });
            }
        }
    }
}