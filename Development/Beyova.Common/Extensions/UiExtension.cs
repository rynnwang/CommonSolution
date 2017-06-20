using System;

namespace Beyova
{
    /// <summary>
    /// </summary>
    public static class UiExtension
    {
        /// <summary>
        /// To the UI date time.
        /// </summary>
        /// <param name="utcDateTime">The UTC date time.</param>
        /// <param name="defaultTimeZoneInMinute">The default time zone in minute.</param>
        /// <returns>System.Nullable&lt;DateTime&gt;.</returns>
        public static DateTime? ToUiDateTime(this DateTime? utcDateTime, int defaultTimeZoneInMinute = 480)
        {
            return utcDateTime.HasValue ? ToUiDateTime(utcDateTime.Value, defaultTimeZoneInMinute) as DateTime? : null;
        }

        /// <summary>
        /// To the UI date time.
        /// </summary>
        /// <param name="utcDateTime">The UTC date time.</param>
        /// <param name="defaultTimeZoneInMinute">The default time zone in minute.</param>
        /// <returns>System.DateTime.</returns>
        public static DateTime ToUiDateTime(this DateTime utcDateTime, int defaultTimeZoneInMinute = 480)
        {
            return utcDateTime.ToDifferentTimeZone((ContextHelper.CurrentUserInfo?.TimeZone) ?? 480);
        }

        /// <summary>
        /// To the friendly date time display.
        /// </summary>
        /// <param name="minutes">The minutes.</param>
        /// <param name="minuteUnit">The minute unit.</param>
        /// <param name="hourUnit">The hour unit.</param>
        /// <param name="dayUnit">The day unit.</param>
        /// <param name="monthUnit">The month unit.</param>
        /// <returns></returns>
        public static string ToFriendlyDateTimeDisplay(this int minutes, string minuteUnit, string hourUnit, string dayUnit, string monthUnit)
        {
            const string format = "{0} {1}";
            if (minutes < 60)
            {
                return string.Format(format, minutes, minuteUnit.SafeToString("min"));
            }
            else if (minutes < 1440)
            {
                return string.Format(format, (int)((double)minutes / 60), hourUnit.SafeToString("hr"));
            }
            else if (minutes < 43200)
            {
                return string.Format(format, (int)((double)minutes / 1440), dayUnit.SafeToString("day"));
            }
            else
            {
                return string.Format(format, (int)((double)minutes / 43200), monthUnit.SafeToString("month"));
            }
        }
    }
}