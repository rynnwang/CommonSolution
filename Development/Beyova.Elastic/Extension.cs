using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova.ApiTracking;

namespace Beyova.Elastic
{
    internal static class Extension
    {
        /// <summary>
        /// Exceptions the criteria to elastic criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>System.Object.</returns>
        public static SearchCriteria ToElasticCriteria(this ExceptionCriteria criteria)
        {
            if (criteria == null) { return null; }

            var termList = new Dictionary<string, object>();
            var matchList = new Dictionary<string, string>();
            var dateTimeRangeList = new Dictionary<string, object>();

            if (criteria.Key != null)
            {
                termList.Add("Key.raw", criteria.Key);
            }
            else
            {

                if (criteria.ServerIdentifier != null)
                {
                    termList.Add("ServerIdentifier.raw", criteria.ServerIdentifier);
                }

                if (criteria.ServiceIdentifier != null)
                {
                    termList.Add("ServiceIdentifier.raw", criteria.ServiceIdentifier);
                }

                if (criteria.UserIdentifier != null)
                {
                    matchList.Add("UserIdentifier", criteria.UserIdentifier);
                }

                var result = new SearchCriteria
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
                    result.Range = new { CreatedStamp = dateTimeRangeList };
                }

                return result;
            }

            return null;
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

            var termList = new Dictionary<string, object>();
            var matchList = new Dictionary<string, string>();
            var dateTimeRangeList = new Dictionary<string, object>();

            if (criteria.Key != null)
            {
                termList.Add("Key.raw", criteria.Key);
            }
            else
            {
                if (criteria.ApiFullName != null)
                {
                    matchList.Add("ApiFullName", criteria.ApiFullName);
                }

                if (criteria.ClientIdentifier != null)
                {
                    termList.Add("ClientIdentifier.raw", criteria.ClientIdentifier);
                }

                if (criteria.Content != null)
                {
                    matchList.Add("Content", criteria.Content);
                }

                if (criteria.CultureCode != null)
                {
                    termList.Add("CultureCode.raw", criteria.CultureCode);
                }

                if (criteria.DeviceType != null)
                {
                    termList.Add("DeviceType.raw", (int)criteria.DeviceType);
                }

                if (criteria.ExceptionKey != null)
                {
                    termList.Add("ExceptionKey", criteria.ExceptionKey);
                }

                if (criteria.IpAddress != null)
                {
                    termList.Add("IpAddress.raw", criteria.IpAddress);
                }

                if (criteria.ModuleName != null)
                {
                    termList.Add("ModuleName.raw", criteria.ModuleName);
                }

                if (criteria.Platform != null)
                {
                    termList.Add("Platform.raw", (int)criteria.Platform);
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
                    termList.Add("ResourceEntityKey.raw", criteria.ResourceEntityKey);
                }

                if (criteria.ResourceName != null)
                {
                    termList.Add("ResourceName.raw", criteria.ResourceName);
                }

                if (criteria.ServerIdentifier != null)
                {
                    termList.Add("ServerIdentifier.raw", criteria.ServerIdentifier);
                }

                if (criteria.ServiceIdentifier != null)
                {
                    termList.Add("ServiceIdentifier.raw", criteria.ServiceIdentifier);
                }

                if (criteria.TraceId != null)
                {
                    termList.Add("TraceId.raw", criteria.TraceId);
                }

                if (criteria.UserAgent != null)
                {
                    matchList.Add("UserAgent", criteria.UserAgent);
                }

                if (criteria.UserIdentifier != null)
                {
                    matchList.Add("UserIdentifier", criteria.UserIdentifier);
                }

                var result = new SearchCriteria
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
                    result.Range = new { CreatedStamp = dateTimeRangeList };
                }

                return result;
            }

            return null;
        }
    }
}
