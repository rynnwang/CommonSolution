using System;

namespace Beyova
{
    /// <summary>
    /// class Schedule
    /// </summary>
    public class Schedule : ICloneable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the recurrence.
        /// </summary>
        /// <value>The recurrence.</value>
        public TimeScope Recurrence
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the hit reference.
        /// </summary>
        /// <value>The hit reference.</value>
        public DateTime HitReference
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the start stamp.
        /// </summary>
        /// <value>The start stamp.</value>
        public DateTime? StartStamp
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

        #endregion Properties

        /// <summary>
        /// Gets the next occurrence.
        /// </summary>
        /// <param name="nowStandardStamp">The now standard stamp.</param>
        /// <returns>System.Nullable&lt;DateTime&gt;.</returns>
        public DateTime? GetNextOccurrence(DateTime nowStandardStamp)
        {
            DateTime? result = null;
            DateTime tmp;
            switch (this.Recurrence)
            {
                //// Everyday
                case TimeScope.Day:
                    tmp = new DateTime(
                        nowStandardStamp.Year,
                        nowStandardStamp.Month,
                        nowStandardStamp.Day,
                        HitReference.Hour,
                        HitReference.Minute,
                        HitReference.Second);
                    if (tmp < nowStandardStamp)
                    {
                        tmp = tmp.AddDays(1);
                    }
                    result = tmp;
                    break;
                //// EveryYear
                case TimeScope.Year:
                    tmp = new DateTime(
                        nowStandardStamp.Year,
                        HitReference.Month,
                        HitReference.Day,
                        HitReference.Hour,
                        HitReference.Minute,
                        HitReference.Second);
                    if (tmp < nowStandardStamp)
                    {
                        tmp = tmp.AddYears(1);
                    }
                    result = tmp;
                    break;
                //// Weekly
                case TimeScope.Week:
                    tmp = new DateTime(
                        nowStandardStamp.Year,
                        nowStandardStamp.Month,
                        nowStandardStamp.Day,
                        HitReference.Hour,
                        HitReference.Minute,
                        HitReference.Second);
                    if (tmp < nowStandardStamp)
                    {
                        tmp = tmp.AddDays(7);
                    }
                    result = tmp;
                    break;
                //// Monthly
                case TimeScope.Month:
                    tmp = new DateTime(
                        nowStandardStamp.Year,
                        nowStandardStamp.Month,
                        HitReference.Day,
                        HitReference.Hour,
                        HitReference.Minute,
                        HitReference.Second);
                    if (tmp < nowStandardStamp)
                    {
                        tmp = tmp.AddMonths(1);
                    }
                    result = tmp;
                    break;
                //// EveryHour
                case TimeScope.Hour:
                    tmp = new DateTime(
                        nowStandardStamp.Year,
                        nowStandardStamp.Month,
                        nowStandardStamp.Day,
                        nowStandardStamp.Hour,
                        HitReference.Minute,
                        HitReference.Second);
                    if (tmp < nowStandardStamp)
                    {
                        tmp = tmp.AddHours(1);
                    }
                    result = tmp;
                    break;
                //// Once
                case TimeScope.None:
                    if (this.HitReference >= nowStandardStamp)
                    {
                        result = this.HitReference;
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
            return new Schedule
            {
                EndStamp = this.EndStamp,
                Recurrence = this.Recurrence,
                HitReference = this.HitReference,
                StartStamp = this.StartStamp
            };
        }
    }

    /// <summary>
    /// Class Schedule.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Schedule<T> : Schedule
    {
        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        /// <value>The object.</value>
        public T Object { get; set; }
    }
}