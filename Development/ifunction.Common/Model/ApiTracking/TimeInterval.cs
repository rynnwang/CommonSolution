using System;
using ifunction.ExceptionSystem;

namespace ifunction.ApiTracking.Model
{
    /// <summary>
    /// Class TimeInterval.
    /// </summary>
    public class TimeInterval
    {
        #region Properties

        /// <summary>
        /// Gets or sets the n.
        /// </summary>
        /// <value>The n.</value>
        public int N
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the n unit.
        /// </summary>
        /// <value>The n unit.</value>
        public TimeUnit Unit
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("every_{0}_{1}", this.N, this.Unit.ToString().ToLowerInvariant());
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return this.ToString().Equals(obj.SafeToString());
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Parses the specified interval string.
        /// </summary>
        /// <param name="intervalString">The interval string.</param>
        /// <returns>AxisTimeInterval.</returns>
        /// <exception cref="OperationFailureException">Parse</exception>
        public static TimeInterval Parse(string intervalString)
        {
            if (!string.IsNullOrWhiteSpace(intervalString) && intervalString.StartsWith("every_", StringComparison.InvariantCultureIgnoreCase))
            {
                try
                {
                    var parts = intervalString.Split('_');
                    var n = parts[1].ToInt32();
                    var unit = (TimeUnit)Enum.Parse(typeof(TimeUnit), parts[2]);

                    return new TimeInterval { N = n, Unit = unit };
                }
                catch (Exception ex)
                {
                    throw new OperationFailureException("Parse", ex, intervalString);
                }
            }

            return null;
        }

        #region static constructors

        /// <summary>
        /// breaks your TimeFrame into minute long chunks.
        /// </summary>
        /// <value>The minutely.</value>
        public static TimeInterval Minutely
        {
            get
            {
                return new TimeInterval
                {
                    N = 1,
                    Unit = TimeUnit.Minute
                };
            }
        }

        /// <summary>
        /// breaks your TimeFrame into hour long chunks.
        /// </summary>
        /// <value>The hourly.</value>
        public static TimeInterval Hourly
        {
            get
            {
                return new TimeInterval
                {
                    N = 1,
                    Unit = TimeUnit.Hour
                };
            }
        }

        /// <summary>
        /// breaks your TimeFrame into day long chunks.
        /// </summary>
        /// <value>The daily.</value>
        public static TimeInterval Daily
        {
            get
            {
                return new TimeInterval
                {
                    N = 1,
                    Unit = TimeUnit.Day
                };
            }
        }

        /// <summary>
        /// breaks your TimeFrame into week long chunks.
        /// </summary>
        /// <value>The weekly.</value>
        public static TimeInterval Weekly
        {
            get
            {
                return new TimeInterval
                {
                    N = 1,
                    Unit = TimeUnit.Week
                };
            }
        }

        /// <summary>
        /// breaks your TimeFrame into month long chunks.
        /// </summary>
        /// <value>The monthly.</value>
        public static TimeInterval Monthly
        {
            get
            {
                return new TimeInterval
                {
                    N = 1,
                    Unit = TimeUnit.Month
                };
            }
        }

        /// <summary>
        /// breaks your TimeFrame into year long chunks.
        /// </summary>
        /// <value>The yearly.</value>
        public static TimeInterval Yearly
        {
            get
            {
                return new TimeInterval
                {
                    N = 1,
                    Unit = TimeUnit.Year
                };
            }
        }

        /// <summary>
        /// breaks your TimeFrame into chunks of the specified length
        /// </summary>
        /// <param name="n">chunk length</param>
        /// <returns>AxisTimeInterval.</returns>
        public static TimeInterval EveryNMinutes(int n)
        {
            return new TimeInterval
            {
                N = n,
                Unit = TimeUnit.Minute
            };
        }

        /// <summary>
        /// breaks your TimeFrame into chunks of the specified length
        /// </summary>
        /// <param name="n">chunk length</param>
        /// <returns>AxisTimeInterval.</returns>
        public static TimeInterval EveryNHours(int n)
        {
            return new TimeInterval
            {
                N = n,
                Unit = TimeUnit.Hour
            };
        }

        /// <summary>
        /// breaks your TimeFrame into chunks of the specified length
        /// </summary>
        /// <param name="n">chunk length</param>
        /// <returns>AxisTimeInterval.</returns>
        public static TimeInterval EveryNDays(int n)
        {
            return new TimeInterval
            {
                N = n,
                Unit = TimeUnit.Day
            };
        }

        /// <summary>
        /// breaks your TimeFrame into chunks of the specified length
        /// </summary>
        /// <param name="n">chunk length</param>
        /// <returns>AxisTimeInterval.</returns>
        public static TimeInterval EveryNWeeks(int n)
        {
            return new TimeInterval
            {
                N = n,
                Unit = TimeUnit.Week
            };
        }

        /// <summary>
        /// breaks your TimeFrame into chunks of the specified length
        /// </summary>
        /// <param name="n">chunk length</param>
        /// <returns>AxisTimeInterval.</returns>
        public static TimeInterval EveryNMonths(int n)
        {
            return new TimeInterval
            {
                N = n,
                Unit = TimeUnit.Month
            };
        }

        /// <summary>
        /// breaks your TimeFrame into chunks of the specified length
        /// </summary>
        /// <param name="n">chunk length</param>
        /// <returns>AxisTimeInterval.</returns>
        public static TimeInterval EveryNYears(int n)
        {
            return new TimeInterval
            {
                N = n,
                Unit = TimeUnit.Year
            };
        }

        #endregion
    }
}
