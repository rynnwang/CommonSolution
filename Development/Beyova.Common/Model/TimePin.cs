using System;
using System.Text.RegularExpressions;
using Beyova.ApiTracking.Model;
using Beyova.ExceptionSystem;

namespace Beyova.Model
{
    /// <summary>
    /// Struct for TimePin
    /// </summary>
    public struct TimePin : ICloneable
    {
        #region Constants

        /// <summary>
        /// The XML_ recurrence type
        /// </summary>
        private const string xml_TimeUnit = "TimeUnit";

        /// <summary>
        /// The XML_ reference stamp
        /// </summary>
        private const string xml_ReferenceStamp = "ReferenceStamp";

        /// <summary>
        /// The XML_ end stamp
        /// </summary>
        private const string xml_EndStamp = "EndStamp";

        /// <summary>
        /// The string format
        /// </summary>
        private const string stringFormat = "{0}#{1}_{2}";

        /// <summary>
        /// The string regex
        /// </summary>
        private static Regex stringRegex = new Regex(string.Format(stringFormat,
            "<?TimeUnit>([0-9]*)",
            "<?RecurrenceTicks>([0-9]*)",
            "<?EndTicks>([0-9]*)"), RegexOptions.Compiled | RegexOptions.IgnoreCase);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the recurrence.
        /// </summary>
        /// <value>The recurrence.</value>
        public TimeUnit Recurrence
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the reference stamp.
        /// </summary>
        /// <value>The reference stamp.</value>
        public DateTime ReferenceStamp
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the end stamp.
        /// </summary>
        /// <value>The end stamp.</value>
        public DateTime? EndStamp
        {
            get;
            set;
        }

        #endregion

        #region Constuctor

        #endregion

        /// <summary>
        /// Gets the next pin.
        /// </summary>
        /// <param name="nowStandardStamp">The now standard stamp.</param>
        /// <returns>System.Nullable{DateTime}.</returns>
        public DateTime? GetNextPin(DateTime nowStandardStamp)
        {
            DateTime? result = null;
            DateTime tmp;
            switch (this.Recurrence)
            {
                //// Everyday
                case TimeUnit.Day:
                    tmp = new DateTime(
                        nowStandardStamp.Year,
                        nowStandardStamp.Month,
                        nowStandardStamp.Day,
                        ReferenceStamp.Hour,
                        ReferenceStamp.Minute,
                        ReferenceStamp.Second);
                    if (tmp < nowStandardStamp)
                    {
                        tmp = tmp.AddDays(1);
                    }
                    result = tmp;
                    break;
                //// EveryYear
                case TimeUnit.Year:
                    tmp = new DateTime(
                        nowStandardStamp.Year,
                        ReferenceStamp.Month,
                        ReferenceStamp.Day,
                        ReferenceStamp.Hour,
                        ReferenceStamp.Minute,
                        ReferenceStamp.Second);
                    if (tmp < nowStandardStamp)
                    {
                        tmp = tmp.AddYears(1);
                    }
                    result = tmp;
                    break;
                //// Weekly
                case TimeUnit.Week:
                    tmp = new DateTime(
                        nowStandardStamp.Year,
                        nowStandardStamp.Month,
                        nowStandardStamp.Day,
                        ReferenceStamp.Hour,
                        ReferenceStamp.Minute,
                        ReferenceStamp.Second);
                    if (tmp < nowStandardStamp)
                    {
                        tmp = tmp.AddDays(7);
                    }
                    result = tmp;
                    break;
                //// Monthly
                case TimeUnit.Month:
                    tmp = new DateTime(
                        nowStandardStamp.Year,
                        nowStandardStamp.Month,
                        ReferenceStamp.Day,
                        ReferenceStamp.Hour,
                        ReferenceStamp.Minute,
                        ReferenceStamp.Second);
                    if (tmp < nowStandardStamp)
                    {
                        tmp = tmp.AddMonths(1);
                    }
                    result = tmp;
                    break;
                //// EveryHour
                case TimeUnit.Hour:
                    tmp = new DateTime(
                        nowStandardStamp.Year,
                        nowStandardStamp.Month,
                        nowStandardStamp.Day,
                        nowStandardStamp.Hour,
                        ReferenceStamp.Minute,
                        ReferenceStamp.Second);
                    if (tmp < nowStandardStamp)
                    {
                        tmp = tmp.AddHours(1);
                    }
                    result = tmp;
                    break;
                //// Once
                case TimeUnit.None:
                    if (this.ReferenceStamp >= nowStandardStamp)
                    {
                        result = this.ReferenceStamp;
                    }
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            var clonedObject = new TimePin
            {
                EndStamp = this.EndStamp,
                Recurrence = this.Recurrence,
                ReferenceStamp = this.ReferenceStamp
            };

            return clonedObject;
        }

        #region String serialization

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format(stringFormat,
               (int)(this.Recurrence),
                this.ReferenceStamp.Ticks,
                this.EndStamp == null ? string.Empty : this.EndStamp.Value.Ticks.ToString());
        }

        /// <summary>
        /// Parses from string.
        /// </summary>
        /// <param name="timePinString">The time pin string.</param>
        /// <param name="timePinOutput">The time pin output.</param>
        /// <returns><c>true</c> if succeed to parse, <c>false</c> otherwise.</returns>
        /// <exception cref="InvalidObjectException">timePinString</exception>
        public static bool TryParse(string timePinString, out TimePin timePinOutput)
        {
            try
            {
                timePinOutput = Parse(timePinString);
                return true;
            }
            catch
            {
                timePinOutput = new TimePin();
                return false;
            }
        }

        /// <summary>
        /// Parses the specified time pin string.
        /// </summary>
        /// <param name="timePinString">The time pin string.</param>
        /// <returns>TimePin.</returns>
        /// <exception cref="InvalidObjectException">timePinString;null</exception>
        public static TimePin Parse(string timePinString)
        {
            timePinString.CheckNullObject("timePinString");

            var matchResult = stringRegex.Match(timePinString);
            if (matchResult.Success)
            {
                return new TimePin
                {
                    Recurrence = (TimeUnit)(matchResult.Result("{$TimeUnit}").ObjectToInt32()),
                    ReferenceStamp = new DateTime(matchResult.Result("{$RecurrenceTicks}").ObjectToInt64()),
                    EndStamp = new DateTime(matchResult.Result("{$EndTicks}").ObjectToInt64())
                };
            }
            else
            {
                throw new InvalidObjectException("timePinString", null, timePinString);
            }
        }

        #endregion
    }
}
