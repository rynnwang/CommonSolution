using System;
using System.Collections.Generic;
using Beyova.CommonService.DataAccessController;
using Beyova.CommonServiceInterface;
using Beyova.ExceptionSystem;

namespace Beyova.CommonService
{
    /// <summary>
    /// Class BaseUserPreferenceServiceCore.
    /// </summary>
    public class BaseUserPreferenceServiceCore : BaseUserPreferenceServiceCore<UserPreference, UserPreferenceCriteria>
    {
    }

    /// <summary>
    /// Class BaseUserPreferenceServiceCore.
    /// </summary>
    /// <typeparam name="TUserPreference">The type of the t user preference.</typeparam>
    /// <typeparam name="TUserPreferenceCriteria">The type of the t user preference criteria.</typeparam>
    public abstract class BaseUserPreferenceServiceCore<TUserPreference, TUserPreferenceCriteria>
        : IUserPreferenceService<TUserPreference, TUserPreferenceCriteria>
           where TUserPreference : UserPreference
           where TUserPreferenceCriteria : UserPreferenceCriteria, new()
    {
        public virtual Guid? CreateOrUpdateDefaultPreference(TUserPreference preference)
        {
            throw new NotImplementedException();
        }

        public virtual Guid? CreateOrUpdateUserPreference(TUserPreference preference)
        {
            throw new NotImplementedException();
        }

        public virtual List<TUserPreference> QueryDefaultPreference(TUserPreferenceCriteria criteria)
        {
            throw new NotImplementedException();
        }

        public virtual List<TUserPreference> QueryUserPreference(TUserPreferenceCriteria criteria)
        {
            throw new NotImplementedException();
        }
    }
}
