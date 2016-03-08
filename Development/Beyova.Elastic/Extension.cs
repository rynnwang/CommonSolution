using System.Collections.Generic;
using Beyova.ApiTracking;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class Extension.
    /// </summary>
    internal static class Extension
    {
        #region Constants

        /// <summary>
        /// The created stamp
        /// </summary>
        const string CreatedStamp = "CreatedStamp";

        /// <summary>
        /// The descending
        /// </summary>
        const string Descending = "desc";

        /// <summary>
        /// The exception key
        /// </summary>
        const string ExceptionKey = "ExceptionKey";

        #endregion

        /// <summary>
        /// Exceptions the criteria to elastic criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>System.Object.</returns>
        public static SearchCriteria ToElasticCriteria(this ExceptionCriteria criteria)
        {
            if (criteria == null) { return null; }

            QueryCriteria queryCriteria = null;
            var termList = new Dictionary<string, object>();
            var matchList = new Dictionary<string, object>();
            var dateTimeRangeList = new Dictionary<string, object>();

            if (criteria.Key != null)
            {
                matchList.Add("Key", criteria.Key.Value.ToString());
                criteria.Count = 1;
                queryCriteria = new QueryCriteria
                {
                    Matches = matchList
                };
            }
            else
            {
                if (criteria.ServerIdentifier != null)
                {
                    termList.Add("ServerIdentifier", criteria.ServerIdentifier);
                }

                if (criteria.ServiceIdentifier != null)
                {
                    termList.Add("ServiceIdentifier", criteria.ServiceIdentifier);
                }

                if (criteria.UserIdentifier != null)
                {
                    termList.Add("UserIdentifier", criteria.UserIdentifier);
                }

                if (criteria.MajorCode != null)
                {
                    matchList.Add("Code.Major", (int)criteria.MajorCode.Value);
                }

                if (!string.IsNullOrWhiteSpace(criteria.MinorCode))
                {
                    termList.Add("Code.Minor", criteria.MinorCode);
                }

                queryCriteria = new QueryCriteria
                {
                    Terms = termList.Count > 0 ? termList : null,
                    Matches = matchList.Count > 0 ? matchList : null
                };

                if (criteria.FromStamp != null)
                {
                    dateTimeRangeList.Add("gte", criteria.FromStamp.Value);
                }

                if (criteria.ToStamp != null)
                {
                    dateTimeRangeList.Add("lte", criteria.ToStamp.Value);
                }

                if (dateTimeRangeList.Count > 0)
                {
                    queryCriteria.Range = new { CreatedStamp = dateTimeRangeList };
                }

                queryCriteria = ToValidSearchCriteria(queryCriteria);
            }

            return new SearchCriteria
            {
                QueryCriteria = queryCriteria,
                Count = criteria.Count < 1 ? 50 : criteria.Count,
                OrderByDesc = new Dictionary<string, string> { { CreatedStamp, Descending } }
            };
        }

        /// <summary>
        /// APIs the event criteria to elastic criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>SearchCriteria.</returns>
        public static SearchCriteria ToElasticCriteria(this ApiEventCriteria criteria)
        {
            if (criteria == null)
            {
                return null;
            }

            QueryCriteria queryCriteria = null;
            var termList = new Dictionary<string, object>();
            var matchList = new Dictionary<string, object>();
            var dateTimeRangeList = new Dictionary<string, object>();

            if (criteria.Key != null)
            {
                matchList.Add("Key", criteria.Key.Value.ToString());
                criteria.Count = 1;
                queryCriteria = new QueryCriteria
                {
                    Matches = matchList
                };
            }
            else
            {
                if (criteria.ApiFullName != null)
                {
                    matchList.Add("ApiFullName", criteria.ApiFullName);
                }

                if (criteria.ClientIdentifier != null)
                {
                    termList.Add("ClientIdentifier", criteria.ClientIdentifier);
                }

                if (criteria.Content != null)
                {
                    matchList.Add("Content", criteria.Content);
                }

                if (criteria.CultureCode != null)
                {
                    termList.Add("CultureCode", criteria.CultureCode);
                }

                if (criteria.DeviceType != null)
                {
                    matchList.Add("DeviceType", (int)criteria.DeviceType);
                }

                if (criteria.ExceptionKey != null)
                {
                    termList.Add("ExceptionKey", criteria.ExceptionKey);
                }

                if (criteria.IpAddress != null)
                {
                    termList.Add("IpAddress", criteria.IpAddress);
                }

                if (criteria.ModuleName != null)
                {
                    termList.Add("ModuleName", criteria.ModuleName);
                }

                if (criteria.Platform != null)
                {
                    matchList.Add("Platform", ((int)criteria.Platform));
                }

                if (criteria.RawUrl != null)
                {
                    matchList.Add("RawUrl", criteria.RawUrl);
                }

                if (criteria.ReferrerUrl != null)
                {
                    matchList.Add("ReferrerUrl", criteria.ReferrerUrl);
                }

                if (criteria.ResourceEntityKey != null)
                {
                    termList.Add("ResourceEntityKey", criteria.ResourceEntityKey);
                }

                if (criteria.ResourceName != null)
                {
                    termList.Add("ResourceName", criteria.ResourceName);
                }

                if (criteria.ServerIdentifier != null)
                {
                    termList.Add("ServerIdentifier", criteria.ServerIdentifier);
                }

                if (criteria.ServiceIdentifier != null)
                {
                    termList.Add("ServiceIdentifier", criteria.ServiceIdentifier);
                }

                if (criteria.TraceId != null)
                {
                    matchList.Add("TraceId", criteria.TraceId);
                }

                if (criteria.UserAgent != null)
                {
                    termList.Add("UserAgent", criteria.UserAgent);
                }

                if (criteria.UserIdentifier != null)
                {
                    termList.Add("UserIdentifier", criteria.UserIdentifier);
                }

                queryCriteria = new QueryCriteria
                {
                    Terms = termList.Count > 0 ? termList : null,
                    Matches = matchList.Count > 0 ? matchList : null,
                };

                // Only when ExceptionKey is not specified, HasException has chance to use.
                if (criteria.ExceptionKey == null && criteria.HasException != null)
                {
                    queryCriteria.Filters = new FilterCriteria();
                    queryCriteria.Filters.Items = criteria.HasException.Value ? new { exists = new { field = ExceptionKey } } : new { missing = new { field = ExceptionKey } } as object;
                }

                if (criteria.FromStamp != null)
                {
                    dateTimeRangeList.Add("gte", criteria.FromStamp.Value);
                }

                if (criteria.ToStamp != null)
                {
                    dateTimeRangeList.Add("lte", criteria.ToStamp.Value);
                }

                if (dateTimeRangeList.Count > 0)
                {
                    queryCriteria.Range = new { CreatedStamp = dateTimeRangeList };
                }

                queryCriteria = ToValidSearchCriteria(queryCriteria);
            }

            return new SearchCriteria
            {
                QueryCriteria = queryCriteria,
                Count = criteria.Count < 1 ? 50 : criteria.Count,
                OrderByDesc = new Dictionary<string, string> { { CreatedStamp, Descending } }
            };
        }

        /// <summary>
        /// To the valid search criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>SearchCriteria.</returns>
        private static QueryCriteria ToValidSearchCriteria(this QueryCriteria criteria)
        {
            return (criteria == null || (
                criteria.Terms == null && criteria.FromIndex == null && criteria.Matches == null && criteria.Range == null
                )) ? null : criteria;
        }
    }
}
