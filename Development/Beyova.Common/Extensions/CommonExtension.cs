﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Beyova
{
    /// <summary>
    /// Extensions for common type and common actions
    /// </summary>
    public static class CommonExtension
    {
        private const string emptyString = "";

        #region Format Constants

        /// <summary>
        /// The date time format for commonly use. Format can be used in ToString method of <c>DateTime</c>, whose result should be like 2012-12-01 12:01:02.
        /// </summary>
        public const string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// The local date time format
        /// </summary>
        public const string localDateTimeFormat = "yyyy-MM-dd HH:mm:ss zzzz";

        /// <summary>
        /// The full date time format. Format can be used in ToString method of <c>DateTime</c>, whose result should be like 2012-12-01 12:01:02.027.
        /// </summary>
        public const string fullDateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

        /// <summary>
        /// The date time format for commonly use. Format can be used in ToString method of <c>DateTime</c>, whose result should be like 2012-12-01.
        /// </summary>
        public const string dateFormat = "yyyy-MM-dd";

        /// <summary>
        /// The full date time tz format
        /// </summary>
        public const string fullDateTimeTZFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

        #endregion Format Constants

        #region Or + And

        /// <summary>
        /// Ors the specified match object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matchObject">The match object.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns><c>true</c> if match object is NOT null and equals any of given collection, <c>false</c> otherwise.</returns>
        public static bool Or<T>(this T matchObject, IEnumerable<T> collection, IEqualityComparer<T> comparer = null)
        {
            if (matchObject != null)
            {
                if (collection.HasItem())
                {
                    if (comparer == null)
                    {
                        comparer = EqualityComparer<T>.Default;
                    }

                    foreach (var one in collection)
                    {
                        if (comparer.Equals(one, matchObject))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Ands the specified collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matchObject">The match object.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>
        ///   <c>true</c> if match object is NOT null and equals all of given collection, <c>false</c> otherwise.
        /// </returns>
        public static bool And<T>(this T matchObject, IEnumerable<T> collection, IEqualityComparer<T> comparer = null)
        {
            if (matchObject != null)
            {
                if (collection.HasItem())
                {
                    if (comparer == null)
                    {
                        comparer = EqualityComparer<T>.Default;
                    }

                    foreach (var one in collection)
                    {
                        if (!comparer.Equals(one, matchObject))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            return false;
        }

        #endregion Or + And

        #region Extensions for all objects

        /// <summary>
        /// Safes the dispose.
        /// </summary>
        /// <param name="disposableObject">The disposable object.</param>
        public static void SafeDispose(this IDisposable disposableObject)
        {
            if (disposableObject != null)
            {
                disposableObject.Dispose();
            }
        }

        /// <summary>
        /// Safe equals. Difference with Equals: If both null or value equals, return true, otherwise false. Would not throw NullReferenceException.
        /// </summary>
        /// <param name="stringA">The string a.</param>
        /// <param name="stringB">The string b.</param>
        /// <param name="comparisonType">Type of the comparison. Default: StringComparison.Ordinal</param>
        /// <returns><c>true</c> if equals, <c>false</c> otherwise.</returns>
        public static bool SafeEquals(this string stringA, string stringB, StringComparison comparisonType = StringComparison.Ordinal)
        {
            //Use safe strategy like Nullable equals.
            // http://referencesource.microsoft.com/#mscorlib/system/nullable.cs,ec76599b875ff1b7,references

            if (stringA == null)
            {
                return stringB == null;
            }

            if (stringB == null)
            {
                return false;
            }

            return stringA.Equals(stringB, comparisonType);
        }

        /// <summary>
        /// Meaningful equals. Return true if both of them are null/empty, or neither is null/empty + text equals.
        /// </summary>
        /// <param name="stringA">The string a.</param>
        /// <param name="stringB">The string b.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns><c>true</c> if is meaningful, <c>false</c> otherwise.</returns>
        public static bool MeaningfulEquals(this string stringA, string stringB, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (string.IsNullOrWhiteSpace(stringA))
            {
                return string.IsNullOrWhiteSpace(stringB);
            }

            if (string.IsNullOrWhiteSpace(stringB))
            {
                return false;
            }

            return stringA.Equals(stringB, comparisonType);
        }

        /// <summary>
        /// Safe equals. Difference with Equals: If both null or value equals, return true, otherwise false. Would not throw NullReferenceException.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectA">The object a.</param>
        /// <param name="objectB">The object b.</param>
        /// <returns><c>true</c> if equals, <c>false</c> otherwise.</returns>
        public static bool SafeEquals<T>(this T objectA, T objectB)
        {
            //Use safe strategy like Nullable equals.
            // http://referencesource.microsoft.com/#mscorlib/system/nullable.cs,ec76599b875ff1b7,references

            if (objectA == null)
            {
                return objectB == null;
            }

            if (objectB == null)
            {
                return false;
            }

            return objectA.Equals(objectB);
        }

        /// <summary>
        /// Determines whether [is in values] [the specified values].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="anyObject">Any object.</param>
        /// <param name="values">The values.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns><c>true</c> if [is in values] [the specified values]; otherwise, <c>false</c>.</returns>
        public static bool IsInValues<T>(this T anyObject, ICollection<T> values, IComparer<T> comparer = null)
        {
            if (anyObject != null && values != null)
            {
                foreach (var one in values)
                {
                    if (comparer == null ? one.Equals(anyObject) : comparer.Compare(one, anyObject) == 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether [is in string] [the specified values].
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <param name="values">The values.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns><c>true</c> if [is in string] [the specified values]; otherwise, <c>false</c>.</returns>
        public static bool IsInString(this string anyString, ICollection<string> values, bool ignoreCase)
        {
            if (anyString != null && values != null)
            {
                foreach (var one in values)
                {
                    if (one.Equals(anyString, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether [is in specific string] case un-sensitively
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <param name="values">The values.</param>
        /// <returns><c>true</c> if [is in string] [the specified values]; otherwise, <c>false</c>.</returns>
        public static bool IsInString(this string anyString, params string[] values)
        {
            if (anyString != null && values != null)
            {
                foreach (var one in values)
                {
                    if (one.Equals(anyString, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether [is in specific string] explicitly (case sensitively)
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <param name="values">The values.</param>
        /// <returns><c>true</c> if [is in string explicitly] [the specified values]; otherwise, <c>false</c>.</returns>
        public static bool IsInStringExplicitly(this string anyString, params string[] values)
        {
            if (anyString != null && values != null)
            {
                foreach (var one in values)
                {
                    if (one.Equals(anyString))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Copies the inherited property value to.
        /// </summary>
        /// <typeparam name="TSource">The type of the t source.</typeparam>
        /// <typeparam name="TDestination">The type of the t destination.</typeparam>
        /// <param name="anyObject">Any object.</param>
        /// <param name="destination">The destination.</param>
        public static void CopyInheritedPropertyValueTo<TSource, TDestination>(this TSource anyObject, TDestination destination)
            where TDestination : TSource
        {
            if (anyObject != null && destination != null)
            {
                var sourceProperties = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance);
                var destinationProperties = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance);

                foreach (var one in sourceProperties)
                {
                    var destinationProperty = destinationProperties.FirstOrDefault((item) => item.Name.Equals(one.Name));
                    destinationProperty?.SetValue(destination, one.GetValue(anyObject));
                }
            }
        }

        /// <summary>
        /// Creates the XML.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns>XElement.</returns>
        public static XElement CreateXml(this string nodeName)
        {
            return XElement.Parse(string.Format("<{0}></{0}>", nodeName.SafeToString(XmlConstants.node_Item)));
        }

        /// <summary>
        /// Creates the child node.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="childNodeName">Name of the child node.</param>
        /// <param name="value">The value.</param>
        /// <returns>XElement.</returns>
        public static XElement CreateChildNode(this XElement parentNode, string childNodeName, object value = null)
        {
            if (parentNode != null && !string.IsNullOrWhiteSpace(childNodeName))
            {
                var child = childNodeName.CreateXml();

                if (value != null)
                {
                    child.SetValue(value);
                }
                parentNode.Add(child);

                return child;
            }

            return null;
        }

        /// <summary>
        /// Safe to string.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="defaultString">The default string.</param>
        /// <returns>System.String.</returns>
        public static string SafeToString(this string anyObject, string defaultString = emptyString)
        {
            return !string.IsNullOrWhiteSpace(anyObject) ? anyObject : defaultString;
        }

        /// <summary>
        /// Safe to string.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="defaultString">The default string.</param>
        /// <returns>System.String.</returns>
        public static string SafeToString(this object anyObject, string defaultString = emptyString)
        {
            return anyObject != null ? anyObject.ToString() : defaultString;
        }

        /// <summary>
        /// Checks the name of the XML node.
        /// <remarks>
        /// If <c>nodeName</c> is specified, then method would check null and whether node name is matched.
        /// Otherwise null check only.
        /// </remarks>
        /// </summary>
        /// <param name="anyXml">Any XML.</param>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        public static void CheckXmlNodeName(this XElement anyXml, string nodeName = null, bool ignoreCase = false)
        {
            nodeName.CheckEmptyString("nodeName");

            if (anyXml == null
                || (!anyXml.Name.LocalName.Equals(nodeName, ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture)))
            {
                throw ExceptionFactory.CreateInvalidObjectException(nodeName, data: new { ExpectedName = nodeName, FactName = anyXml == null ? string.Empty : anyXml.Name.LocalName, IgnoreCase = ignoreCase });
            }
        }

        #endregion Extensions for all objects

        #region Type Convert Extensions

        #region Object To XXX

        /// <summary>
        /// Objects to nullable enum.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static TEnum? ObjectToNullableEnum<TEnum>(this object data, TEnum? defaultValue = null)
            where TEnum : struct, IConvertible
        {
            int result;
            TEnum? returnObject;
            if (data == null || data == DBNull.Value || !int.TryParse(data.ToString(), out result))
            {
                returnObject = defaultValue;
            }
            else
            {
                returnObject = (TEnum)Enum.ToObject(typeof(TEnum), result);
            }

            return returnObject;
        }

        /// <summary>
        /// Objects to enum.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static TEnum ObjectToEnum<TEnum>(this object data, TEnum defaultValue = default(TEnum))
            where TEnum : struct, IConvertible
        {
            return ObjectToNullableEnum<TEnum>(data, defaultValue).Value;
        }

        /// <summary>
        /// To double.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Double.</returns>
        public static double ObjectToDouble(this object data, double defaultValue = 0)
        {
            double result;
            if (data == null || data == DBNull.Value || !double.TryParse(data.ToString(), out result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// DBs to float.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Single.</returns>
        public static float ObjectToFloat(this object data, float defaultValue = 0)
        {
            float result;
            if (data == null || data == DBNull.Value || !float.TryParse(data.ToString(), out result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// To the int32.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Int32.</returns>
        public static int ObjectToInt32(this object data, int defaultValue = 0)
        {
            int result;
            if (data == null || data == DBNull.Value || !int.TryParse(data.ToString(), out result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Databases to int64.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Int64.</returns>
        public static long ObjectToInt64(this object data, long defaultValue = 0)
        {
            long result;
            if (data == null || data == DBNull.Value || !long.TryParse(data.ToString(), out result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// To the nullable int32.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Nullable{System.Int32}.</returns>
        public static Int32? ObjectToNullableInt32(this object data, Int32? defaultValue = null)
        {
            int result;
            return (data == null || data == DBNull.Value || !Int32.TryParse(data.ToString(), out result)) ? defaultValue : result;
        }

        /// <summary>
        /// Databases to nullable int64.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Nullable&lt;Int64&gt;.</returns>
        public static Int64? ObjectToNullableInt64(this object data, Int64? defaultValue = null)
        {
            Int64 result;
            return (data == null || data == DBNull.Value || !Int64.TryParse(data.ToString(), out result)) ? defaultValue : result;
        }

        /// <summary>
        /// Databases to nullable float.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Nullable&lt;System.Single&gt;.</returns>
        public static float? ObjectToNullableFloat(this object data, float? defaultValue = null)
        {
            float result;
            return (data == null || data == DBNull.Value || !float.TryParse(data.ToString(), out result)) ? defaultValue : result;
        }

        /// <summary>
        /// Databases to nullable double.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Nullable&lt;System.Double&gt;.</returns>
        public static double? ObjectToNullableDouble(this object data, double? defaultValue = null)
        {
            double result;
            return (data == null || data == DBNull.Value || !double.TryParse(data.ToString(), out result)) ? defaultValue : result;
        }

        /// <summary>
        /// Databases to nullable decimal.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Nullable&lt;System.Decimal&gt;.</returns>
        public static decimal? ObjectToNullableDecimal(this object data, decimal? defaultValue = null)
        {
            decimal result;
            return (data == null || data == DBNull.Value || !decimal.TryParse(data.ToString(), out result)) ? defaultValue : result;
        }

        /// <summary>
        /// DBs to date time.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>System.Nullable{DateTime}.</returns>
        public static DateTime? ObjectToDateTime(this object data)
        {
            DateTime? result = null;

            if (data != null && data != DBNull.Value)
            {
                try
                {
                    result = Convert.ToDateTime(data);
                }
                catch
                {
                    result = data as DateTime?;
                }
            }

            if (result != null)
            {
                result = DateTime.SpecifyKind(result.Value, DateTimeKind.Utc);
            }

            return result;
        }

        /// <summary>
        /// To the date time.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>DateTime.</returns>
        public static DateTime ObjectToDateTime(this object data, DateTime defaultValue)
        {
            return ObjectToDateTime(data) ?? defaultValue;
        }

        /// <summary>
        /// To the date time.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="format">The format.</param>
        /// <param name="style">The style.</param>
        /// <returns>System.Nullable&lt;System.DateTime&gt;.</returns>
        public static DateTime? ToDateTime(this string data, string format = null, DateTimeStyles style = DateTimeStyles.AssumeUniversal)
        {
            DateTime dateTime;
            return DateTime.TryParseExact(data, format.SafeToString(fullDateTimeTZFormat), CultureInfo.InvariantCulture, style, out dateTime) ? dateTime : null as DateTime?;
        }

        /// <summary>
        /// To the date time.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="format">The format.</param>
        /// <param name="style">The style.</param>
        /// <returns>System.DateTime.</returns>
        public static DateTime ToDateTime(this string data, DateTime defaultValue, string format = null, DateTimeStyles style = DateTimeStyles.AssumeUniversal)
        {
            return ToDateTime(data, format, style) ?? defaultValue;
        }

        /// <summary>
        /// To GUID.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Nullable{Guid}.</returns>
        public static Guid? ObjectToGuid(this object data, Guid? defaultValue = null)
        {
            Guid result;
            return (data == null || data == DBNull.Value || !Guid.TryParse(data.ToString(), out result)) ? defaultValue : result;
        }

        /// <summary>
        /// Databases the automatic decimal.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Decimal.</returns>
        public static decimal ObjectToDecimal(this object data, decimal defaultValue = 0)
        {
            decimal result;
            return (data == null || data == DBNull.Value || !decimal.TryParse(data.ToString(), out result)) ? defaultValue : result;
        }

        /// <summary>
        /// To nullable boolean
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The nullable boolean value.</returns>
        public static bool? ObjectToNullableBoolean(this object data, bool? defaultValue = null)
        {
            bool result;
            string dataString = data.SafeToString();
            int booleanInt;

            return
                int.TryParse(dataString, out booleanInt) ?
                Convert.ToBoolean(booleanInt)
                : ((data == null || data == DBNull.Value || !bool.TryParse(dataString, out result)) ? defaultValue : result);
        }

        /// <summary>
        /// DBs to boolean.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The boolean value. If failed to convert, return <c>false</c>.</returns>
        public static bool ObjectToBoolean(this object data)
        {
            return ObjectToNullableBoolean(data, false).Value;
        }

        /// <summary>
        /// To string.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.String.</returns>
        public static string ObjectToString(this object data, string defaultValue = emptyString)
        {
            return (data == null || data == DBNull.Value) ? defaultValue : data.ToString();
        }

        /// <summary>
        /// Databases to XML.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defaultXml">The default XML.</param>
        /// <returns>XElement.</returns>
        public static XElement ObjectToXml(this object data, XElement defaultXml = null)
        {
            var xml = data.SafeToString();

            if (!string.IsNullOrWhiteSpace(xml))
            {
                try
                {
                    return XElement.Parse(xml);
                }
                catch (Exception ex)
                {
                    throw ex.Handle(data);
                }
            }

            return defaultXml;
        }

        #endregion Object To XXX

        /// <summary>
        /// To the int32.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>System.Int32.</returns>
        public static int ToInt32(this Guid guid)
        {
            byte[] seed = guid.ToByteArray();
            for (int i = 0; i < 3; i++)
            {
                seed[i] ^= seed[i + 4];
                seed[i] ^= seed[i + 8];
                seed[i] ^= seed[i + 12];
            }

            return BitConverter.ToInt32(seed, 0);
        }

        /// <summary>
        /// Enums to string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>System.String.</returns>
        public static string EnumToString<T>(this T? enumValue) where T : struct, IConvertible
        {
            return enumValue == null ? string.Empty : EnumToString(enumValue.Value);
        }

        /// <summary>
        /// Enums to string. Format: {int} ({string})
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>System.String.</returns>
        public static string EnumToString<T>(this T enumValue) where T : struct, IConvertible
        {
            return string.Format("{0} ({1})", enumValue.ToString(), enumValue.ToInt32(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Enums to int32. Format: {int} ({string})
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>System.Int32.</returns>
        public static int EnumToInt32<T>(this T enumValue) where T : struct, IConvertible
        {
            return enumValue.ToInt32(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Enums to int32.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>System.Nullable{System.Int32}.</returns>
        public static int? EnumToInt32<T>(this T? enumValue) where T : struct, IConvertible
        {
            int? result = null;
            if (enumValue.HasValue)
            {
                IConvertible convertible = enumValue.Value;
                result = convertible.ToInt32(CultureInfo.InvariantCulture);
            }

            return result;
        }

        /// <summary>
        /// Int32s to enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="intValue">The int value.</param>
        /// <returns>T.</returns>
        public static T Int32ToEnum<T>(this int intValue) where T : struct, IConvertible
        {
            return (T)Enum.ToObject(typeof(T), intValue);
        }

        /// <summary>
        /// Int32s to enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="intValue">The int value.</param>
        /// <returns>System.Nullable&lt;T&gt;.</returns>
        public static T? Int32ToEnum<T>(this int? intValue) where T : struct, IConvertible
        {
            return intValue.HasValue ? (T?)Enum.ToObject(typeof(T), intValue) : null;
        }

        /// <summary>
        /// To boolean.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns>The boolean result. If failed to concert, return <c>false</c>.</returns>
        public static bool ToBoolean(this string stringObject, bool defaultValue = false)
        {
            bool result;
            if (stringObject == "1")
            {
                result = true;
            }
            else
            {
                Boolean.TryParse(stringObject, out result);
            }

            return result;
        }

        /// <summary>
        /// To the int32.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Int32.</returns>
        public static Int32 ToInt32(this string stringObject, int defaultValue = 0)
        {
            Int32 result;
            return Int32.TryParse(stringObject, out result) ? result : defaultValue;
        }

        /// <summary>
        /// To the int64.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Int64.</returns>
        public static Int64 ToInt64(this string stringObject, long defaultValue = 0)
        {
            Int64 result;
            return Int64.TryParse(stringObject, out result) ? result : defaultValue;
        }

        /// <summary>
        /// To nullable int32.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Int32.</returns>
        public static Int32? ToNullableInt32(this string stringObject, Int32? defaultValue = null)
        {
            Int32 result = 0;
            return Int32.TryParse(stringObject, out result) ? result : defaultValue;
        }

        /// <summary>
        /// To the nullable decimal.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Nullable&lt;System.Decimal&gt;.</returns>
        public static decimal? ToNullableDecimal(this string stringObject, decimal? defaultValue = null)
        {
            decimal result = 0;
            return decimal.TryParse(stringObject, out result) ? result : defaultValue;
        }

        /// <summary>
        /// To the nullable boolean.
        /// </summary>
        /// <param name="stringObject">The data.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns>The nullable boolean value.</returns>
        public static bool? ToNullableBoolean(this string stringObject, bool? defaultValue = null)
        {
            bool result;
            int booleanInt;

            return
                int.TryParse(stringObject, out booleanInt) ?
                Convert.ToBoolean(booleanInt)
                : ((stringObject == null || !bool.TryParse(stringObject, out result)) ? defaultValue : result);
        }

        /// <summary>
        /// To the double.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Double.</returns>
        public static Double ToDouble(this string stringObject, Double defaultValue = 0)
        {
            Double result = defaultValue;
            Double.TryParse(stringObject, out result);
            return result;
        }

        /// <summary>
        /// To the decimal.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Decimal.</returns>
        public static decimal ToDecimal(this string stringObject, decimal defaultValue = 0)
        {
            Decimal result = defaultValue;
            Decimal.TryParse(stringObject, out result);
            return result;
        }

        /// <summary>
        /// To the grid.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="defaultGuid">The default GUID.</param>
        /// <returns>System.Nullable{Guid}.</returns>
        public static Guid? ToGuid(this string stringObject, Guid? defaultGuid = null)
        {
            Guid output;
            return Guid.TryParse(stringObject, out output) ?
                output
                : defaultGuid;
        }

        /// <summary>
        /// Converts from the string to date time.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="defaultDateTime">The default date time.</param>
        /// <returns>System.Nullable{DateTime}.</returns>
        public static DateTime? FromStringToDateTime(this string stringObject, DateTime? defaultDateTime = null)
        {
            DateTime output;
            return DateTime.TryParseExact(stringObject, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out output) ?
                output
                : defaultDateTime;
        }

        /// <summary>
        /// Converts from the string to date.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="defaultDate">The default date.</param>
        /// <returns>System.Nullable{DateTime}.</returns>
        public static DateTime? FromStringToDate(this string stringObject, DateTime? defaultDate = null)
        {
            DateTime output;
            return DateTime.TryParseExact(stringObject, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out output) ?
                output
                : defaultDate;
        }

        #endregion Type Convert Extensions

        #region DateTime Extensions

        /// <summary>
        /// Times the zone minute offset to time zone string.
        /// </summary>
        /// <param name="minuteOffset">The minute offset.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        public static string TimeZoneMinuteOffsetToTimeZoneString(this int? minuteOffset)
        {
            if (minuteOffset.HasValue && minuteOffset.Value != 0)
            {
                var offset = (double)(minuteOffset.Value);
                return string.Format("{0}{1}:{2}", offset < 0 ? "-" : "+", (int)(offset / 60), (int)(offset % 60));
            }

            return null;
        }

        /// <summary>
        /// To the UTC.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <param name="currentTimeZoneOffset">The current time zone offset.</param>
        /// <returns>
        /// DateTime.
        /// </returns>
        public static DateTime ToUtc(this DateTime dateTimeObject, TimeSpan currentTimeZoneOffset)
        {
            if (dateTimeObject.Kind == DateTimeKind.Unspecified)
            {
                dateTimeObject = (new DateTime(dateTimeObject.Year, dateTimeObject.Month, dateTimeObject.Day, dateTimeObject.Hour, dateTimeObject.Minute, dateTimeObject.Second, dateTimeObject.Millisecond, DateTimeKind.Utc)) - currentTimeZoneOffset;
            }

            if (dateTimeObject.Kind == DateTimeKind.Local)
            {
                dateTimeObject = dateTimeObject.ToUniversalTime();
            }

            return dateTimeObject;
        }

        /// <summary>
        /// To the UTC.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns>
        /// DateTime.
        /// </returns>
        public static DateTime ToUtc(this DateTime dateTimeObject)
        {
            return ToUtc(dateTimeObject, TimeZone.CurrentTimeZone.GetUtcOffset(dateTimeObject));
        }

        /// <summary>
        /// To the UTC.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <param name="currentTimeZoneOffsetInMinute">The current time zone offset information minute.</param>
        /// <returns>
        /// DateTime.
        /// </returns>
        public static DateTime ToUtc(this DateTime dateTimeObject, int currentTimeZoneOffsetInMinute = 0)
        {
            return ToUtc(dateTimeObject, new TimeSpan(0, currentTimeZoneOffsetInMinute, 0));
        }

        /// <summary>
        /// To the unix milliseconds date time.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns>System.Nullable&lt;System.Int64&gt;.</returns>
        public static long? ToUnixMillisecondsDateTime(this DateTime? dateTimeObject)
        {
            return dateTimeObject.HasValue ? (long?)ToUnixMillisecondsDateTime(dateTimeObject.Value) : null;
        }

        /// <summary>
        /// To the unix milliseconds date time.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns>System.Int64.</returns>
        public static long ToUnixMillisecondsDateTime(this DateTime dateTimeObject)
        {
            return (long)((dateTimeObject - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);
        }

        /// <summary>
        /// Converts Unix milliseconds to date time.
        /// </summary>
        /// <param name="unixMilliseconds">The unix milliseconds.</param>
        /// <param name="dateTimeKind">Kind of the date time.</param>
        /// <returns>System.DateTime.</returns>
        public static DateTime UnixMillisecondsToDateTime(this long unixMilliseconds, DateTimeKind dateTimeKind = DateTimeKind.Utc)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, dateTimeKind).AddMilliseconds(unixMilliseconds);
        }

        /// <summary>
        /// Converts Unix milliseconds to date time.
        /// </summary>
        /// <param name="unixMilliseconds">The unix milliseconds.</param>
        /// <param name="dateTimeKind">Kind of the date time.</param>
        /// <returns>System.Nullable&lt;System.DateTime&gt;.</returns>
        public static DateTime? UnixMillisecondsToDateTime(this long? unixMilliseconds, DateTimeKind dateTimeKind = DateTimeKind.Utc)
        {
            return unixMilliseconds.HasValue ? (DateTime?)UnixMillisecondsToDateTime(unixMilliseconds.Value, dateTimeKind) : null;
        }

        /// <summary>
        /// Converts the time zone minute to time zone.
        /// </summary>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>System.String.</returns>
        public static string ConvertTimeZoneMinuteToTimeZone(this int? timeZone)
        {
            return timeZone.HasValue ? ConvertTimeZoneMinuteToTimeZone(timeZone.Value) : string.Empty;
        }

        /// <summary>
        /// Converts the time zone minute to time zone. Output sample: +08:30
        /// </summary>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>System.String.</returns>
        public static string ConvertTimeZoneMinuteToTimeZone(this int timeZone)
        {
            TimeSpan timespan = new TimeSpan(0, timeZone, 0);
            return (timeZone > 0 ? "+" : "-") + timespan.ToString("hh:mm");
        }

        /// <summary>
        /// To different time zone.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <param name="targetTimeZoneOffset">The target time zone offset.</param>
        /// <param name="currentTimeZoneOffset">The current time zone offset.</param>
        /// <returns>DateTime.</returns>
        public static DateTime ToDifferentTimeZone(this DateTime dateTimeObject, TimeSpan targetTimeZoneOffset, TimeSpan currentTimeZoneOffset = default(TimeSpan))
        {
            var utc = dateTimeObject.ToUtc(currentTimeZoneOffset);
            return (new DateTime(utc.Year, utc.Month, utc.Day, utc.Hour, utc.Minute, utc.Second, utc.Millisecond, DateTimeKind.Local)) + targetTimeZoneOffset;
        }

        /// <summary>
        /// To the different time zone.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <param name="targetTimeZoneOffset">The target time zone offset.</param>
        /// <param name="currentTimeZoneOffset">The current time zone offset.</param>
        /// <returns></returns>
        public static DateTime? ToDifferentTimeZone(this DateTime? dateTimeObject, TimeSpan targetTimeZoneOffset, TimeSpan currentTimeZoneOffset = default(TimeSpan))
        {
            return dateTimeObject.HasValue ?
                 ToDifferentTimeZone(dateTimeObject.Value, targetTimeZoneOffset, currentTimeZoneOffset) as DateTime?
                : null;
        }

        /// <summary>
        /// To the different time zone.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns>DateTime.</returns>
        public static DateTime ToDifferentTimeZone(this DateTime dateTimeObject, TimeZoneInfo timeZone)
        {
            return TimeZoneInfo.ConvertTime(dateTimeObject, timeZone);
        }

        /// <summary>
        /// To the different time zone.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <param name="timeZone">The time zone.</param>
        /// <returns></returns>
        public static DateTime? ToDifferentTimeZone(this DateTime? dateTimeObject, TimeZoneInfo timeZone)
        {
            return dateTimeObject.HasValue ?
                 ToDifferentTimeZone(dateTimeObject.Value, timeZone) as DateTime?
                 : null;
        }

        /// <summary>
        /// To the different time zone.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <param name="targetTimeZoneOffsetInMinute">The target time zone offset information minute.</param>
        /// <param name="currentTimeZoneOffsetInMinute">The current time zone offset information minute.</param>
        /// <returns>
        /// DateTime.
        /// </returns>
        public static DateTime ToDifferentTimeZone(this DateTime dateTimeObject, int targetTimeZoneOffsetInMinute, int currentTimeZoneOffsetInMinute = 0)
        {
            var utc = dateTimeObject.ToUtc(currentTimeZoneOffsetInMinute);
            return (new DateTime(utc.Year, utc.Month, utc.Day, utc.Hour, utc.Minute, utc.Second, utc.Millisecond, DateTimeKind.Local)) + new TimeSpan(0, targetTimeZoneOffsetInMinute, 0);
        }

        /// <summary>
        /// To the different time zone.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <param name="targetTimeZoneOffsetInMinute">The target time zone offset in minute.</param>
        /// <param name="currentTimeZoneOffsetInMinute">The current time zone offset in minute.</param>
        /// <returns></returns>
        public static DateTime? ToDifferentTimeZone(this DateTime? dateTimeObject, int targetTimeZoneOffsetInMinute, int currentTimeZoneOffsetInMinute = 0)
        {
            return dateTimeObject.HasValue ?
                      ToDifferentTimeZone(dateTimeObject.Value, targetTimeZoneOffsetInMinute, currentTimeZoneOffsetInMinute) as DateTime?
                     : null;
        }

        /// <summary>
        /// To the date time string.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns>System.String.</returns>
        public static string ToDateTimeString(this DateTime? dateTimeObject)
        {
            return dateTimeObject.HasValue ? dateTimeObject.Value.ToString(dateTimeFormat, CultureInfo.InvariantCulture) : string.Empty;
        }

        /// <summary>
        /// To the date string.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns>System.String.</returns>
        public static string ToDateString(this DateTime? dateTimeObject)
        {
            return dateTimeObject.HasValue ? dateTimeObject.Value.ToString(dateFormat, CultureInfo.InvariantCulture) : string.Empty;
        }

        /// <summary>
        /// To the local date string.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns>System.String.</returns>
        public static string ToLocalDateString(this DateTime dateTimeObject)
        {
            return dateTimeObject.ToLocalTime().ToString(localDateTimeFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// To the local date string.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns>System.String.</returns>
        public static string ToLocalDateString(this DateTime? dateTimeObject)
        {
            return dateTimeObject.HasValue ? dateTimeObject.Value.ToLocalDateString() : string.Empty;
        }

        /// <summary>
        /// To the full date time string.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        public static string ToFullDateTimeString(this DateTime dateTimeObject)
        {
            return dateTimeObject.ToString(fullDateTimeFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// To the full date time string.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns>System.String.</returns>
        public static string ToFullDateTimeString(this DateTime? dateTimeObject)
        {
            return dateTimeObject.HasValue ? dateTimeObject.Value.ToString(fullDateTimeFormat, CultureInfo.InvariantCulture) : string.Empty;
        }

        /// <summary>
        /// To the full date time tz string.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns>System.String.</returns>
        public static string ToFullDateTimeTzString(this DateTime dateTimeObject)
        {
            return dateTimeObject.ToString(fullDateTimeTZFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// To the full date time tz string.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns></returns>
        public static string ToFullDateTimeTzString(this DateTime? dateTimeObject)
        {
            return dateTimeObject.HasValue ? dateTimeObject.Value.ToString(fullDateTimeTZFormat, CultureInfo.InvariantCulture) : string.Empty;
        }

        /// <summary>
        /// To the log stamp string.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns></returns>
        public static string ToLogStampString(this DateTime dateTimeObject)
        {
            return dateTimeObject.ToString(localDateTimeFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// To the log stamp string.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns>System.String.</returns>
        public static string ToLogStampString(this DateTime? dateTimeObject)
        {
            return dateTimeObject.HasValue ? dateTimeObject.Value.ToLogStampString() : string.Empty;
        }

        /// <summary>
        /// Gets the first day of month.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns>DateTime.</returns>
        public static DateTime GetFirstDayOfMonth(this DateTime dateTimeObject)
        {
            return new DateTime(dateTimeObject.Year,
                dateTimeObject.Month,
                1,
                dateTimeObject.Hour,
                dateTimeObject.Minute,
                dateTimeObject.Second);
        }

        /// <summary>
        /// Gets the last day of month.
        /// </summary>
        /// <param name="dateTimeObject">The date time object.</param>
        /// <returns>DateTime.</returns>
        public static DateTime GetLastDayOfMonth(this DateTime dateTimeObject)
        {
            return new DateTime(dateTimeObject.Year,
                dateTimeObject.Month,
                1,
                dateTimeObject.Hour,
                dateTimeObject.Minute,
                dateTimeObject.Second).AddMonths(1).AddDays(-1);
        }

        #endregion DateTime Extensions

        #region XElement Extension

        /// <summary>
        /// Tries the get child element.
        /// </summary>
        /// <param name="xElement">The x element.</param>
        /// <param name="elementNames">The element names.</param>
        /// <returns>XElement.</returns>
        public static XElement TryGetChildElement(this XElement xElement, params string[] elementNames)
        {
            var current = xElement;

            if (elementNames != null)
            {
                foreach (var one in elementNames)
                {
                    if (current == null)
                    {
                        return null;
                    }

                    if (!string.IsNullOrWhiteSpace(one))
                    {
                        current = current.Element(one);
                    }
                }
            }

            return current;
        }

        /// <summary>
        /// Tries the get child elements.
        /// </summary>
        /// <param name="xElement">The x element.</param>
        /// <param name="elementNames">The element names.</param>
        /// <returns>List&lt;XElement&gt;.</returns>
        public static List<XElement> TryGetChildElements(this XElement xElement, params string[] elementNames)
        {
            var current = new List<XElement> { xElement };
            var result = new List<XElement>();

            if (elementNames != null)
            {
                foreach (var element in current)
                {
                    foreach (var one in elementNames)
                    {
                        if (!string.IsNullOrWhiteSpace(one))
                        {
                            result.Add(element.Element(one));
                        }
                    }
                }

                current = result;
            }

            return current;
        }

        /// <summary>
        /// Tries the parse XML.
        /// </summary>
        /// <param name="xmlString">The XML string.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>XElement.</returns>
        public static XElement TryParseXml(this string xmlString, bool throwException = false)
        {
            try
            {
                xmlString.CheckEmptyString("xmlString");
                return XElement.Parse(xmlString);
            }
            catch (Exception ex)
            {
                if (throwException)
                {
                    throw ex.Handle(xmlString);
                }
            }

            return null;
        }

        /// <summary>
        /// Tries the load XML.
        /// </summary>
        /// <param name="xmlPath">The XML path.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>XDocument.</returns>
        public static XDocument TryLoadXml(this string xmlPath, bool throwException = false)
        {
            try
            {
                xmlPath.CheckEmptyString("xmlPath");
                return XDocument.Load(xmlPath);
            }
            catch (Exception ex)
            {
                if (throwException)
                {
                    throw ex.Handle(xmlPath);
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="xElement">The executable element.</param>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns>System.String.</returns>
        public static string GetXmlValue(this XElement xElement, string nodeName)
        {
            string result = string.Empty;

            if (xElement != null && !string.IsNullOrWhiteSpace(nodeName))
            {
                var row = xElement.Element(nodeName);

                if (row != null)
                {
                    result = row.Value;
                }
            }

            return result;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="value">The value.</param>
        /// <param name="omitCDATA">if set to <c>true</c> [omit CDATA].</param>
        public static void SetXmlValue(this XElement container, string nodeName, string value, bool omitCDATA = false)
        {
            if (container != null && !string.IsNullOrWhiteSpace(nodeName))
            {
                var row = nodeName.CreateXml();

                if (!omitCDATA)
                {
                    row.Add(new XCData(value));
                }
                else
                {
                    row.Value = value;
                }

                container.Add(row);
            }
        }

        /// <summary>
        /// Tries the get the value of the first child element by childNodeName. If node is found, return node value, otherwise return string.Empty.
        /// </summary>
        /// <param name="anyXml">Any XML.</param>
        /// <param name="childNodeName">Name of the child node.</param>
        /// <returns>System.String.</returns>
        public static string TryGetChildValue(this XElement anyXml, string childNodeName)
        {
            XElement child = null;

            if (anyXml != null)
            {
                child = string.IsNullOrWhiteSpace(childNodeName) ? anyXml.Elements().FirstOrDefault() : anyXml.Element(childNodeName);
            }

            return child == null ? string.Empty : child.Value;
        }

        /// <summary>
        /// Finds the child elements by tag with attribute.
        /// </summary>
        /// <param name="anyXml">Any XML.</param>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <returns>List{XElement}.</returns>
        public static List<XElement> FindChildElementsByTagWithAttribute(this XElement anyXml, string tagName, string attributeName, string attributeValue)
        {
            return FindChildElements(anyXml, tagName, attributeName, attributeValue, false);
        }

        /// <summary>
        /// Finds the child elements by tag with attribute.
        /// </summary>
        /// <param name="anyXml">Any XML.</param>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="hasAttributeName">Name of the has attribute.</param>
        /// <returns>List{XElement}.</returns>
        public static List<XElement> FindChildElementsByTagWithAttribute(this XElement anyXml, string tagName, string hasAttributeName)
        {
            return FindChildElements(anyXml, tagName, hasAttributeName, null, false);
        }

        /// <summary>
        /// Finds the child elements by attribute.
        /// </summary>
        /// <param name="anyXml">Any XML.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <returns>List{XElement}.</returns>
        public static List<XElement> FindChildElementsByAttribute(this XElement anyXml, string attributeName, string attributeValue)
        {
            return FindChildElements(anyXml, null, attributeName, attributeValue, false);
        }

        /// <summary>
        /// Finds the first child element by attribute.
        /// </summary>
        /// <param name="anyXml">Any XML.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <returns>XElement.</returns>
        public static XElement FindFirstChildElementByAttribute(this XElement anyXml, string attributeName, string attributeValue)
        {
            return FindChildElements(anyXml, null, attributeName, attributeValue, true).FirstOrDefault();
        }

        /// <summary>
        /// Finds the first child element by tag with attribute.
        /// </summary>
        /// <param name="anyXml">Any XML.</param>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <returns>XElement.</returns>
        public static XElement FindFirstChildElementByTagWithAttribute(this XElement anyXml, string tagName, string attributeName, string attributeValue)
        {
            return FindChildElements(anyXml, tagName, attributeName, attributeValue, true).FirstOrDefault();
        }

        /// <summary>
        /// Finds the child elements by tag with attribute.
        /// </summary>
        /// <param name="anyXml">Any XML.</param>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <param name="findOnlyOne">if set to <c>true</c> [find only one].</param>
        /// <returns>List{XElement}.</returns>
        private static List<XElement> FindChildElements(this XElement anyXml, string tagName, string attributeName, string attributeValue, bool findOnlyOne = false)
        {
            var result = new List<XElement>();

            if (anyXml != null)
            {
                var children = string.IsNullOrWhiteSpace(tagName) ? anyXml.Elements() : anyXml.Elements(tagName);

                foreach (var one in children)
                {
                    if (string.IsNullOrWhiteSpace(attributeName) ||
                                            (one.Attribute(attributeName) != null &&
                                                (string.IsNullOrWhiteSpace(attributeValue) || attributeValue == one.Attribute(attributeName).Value)))
                    {
                        result.Add(one);

                        if (findOnlyOne)
                        {
                            break;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the string value.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if the specified element has elements; otherwise, <c>false</c>.</returns>
        public static bool HasElements(this XElement element, string name = null)
        {
            return element != null
                && (!string.IsNullOrEmpty(name) ? element.Elements(name).Any() : element.Elements().Any());
        }

        /// <summary>
        /// Gets the attribute value.
        /// </summary>
        /// <param name="xElement">The x element.</param>
        /// <param name="attribute">The attribute.</param>
        /// <returns>System.String.</returns>
        public static string GetAttributeValue(this XElement xElement, string attribute)
        {
            string result = string.Empty;

            if (xElement != null && !string.IsNullOrWhiteSpace(attribute))
            {
                var attr = xElement.Attribute(attribute);
                if (attr != null)
                {
                    result = attr.Value;
                }
            }

            return result;
        }

        /// <summary>
        /// Sets the value as sub element.
        /// </summary>
        /// <param name="xElement">The x element.</param>
        /// <param name="subNodeName">Name of the sub node.</param>
        /// <param name="innerValue">The inner value.</param>
        /// <returns>The added element in sub.</returns>
        public static XElement SetValueAsSubElement(this XElement xElement, string subNodeName, object innerValue)
        {
            if (xElement != null && !string.IsNullOrWhiteSpace(subNodeName))
            {
                var tmp = subNodeName.CreateXml();
                if (innerValue != null)
                {
                    tmp.SetValue(innerValue);
                }
                xElement.Add(tmp);

                return tmp;
            }

            return null;
        }

        #endregion XElement Extension

        #region Random

        /// <summary>
        /// The random
        /// </summary>
        private static readonly Random random = new Random();

        /// <summary>
        /// The alpha
        /// </summary>
        private static char[] alpha = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        /// <summary>
        /// Gets the random. [min, max)
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="max">The maximum. (Can not reach)</param>
        /// <param name="min">The minimum. (Can reach)</param>
        /// <returns>System.Int32.</returns>
        public static int GetRandom(this object anyObject, int max, int min = 0)
        {
            return random.Next(min, max);
        }

        /// <summary>
        /// Gets the random number only.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="length">The length.</param>
        /// <returns>System.String.</returns>
        public static string CreateRandomNumberString(this object anyObject, int length)
        {
            var sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                sb.Append(alpha[random.Next(10)]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the random hex string.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="length">The length.</param>
        /// <returns>System.String.</returns>
        public static string CreateRandomHexString(this object anyObject, int length)
        {
            var sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                sb.Append(alpha[random.Next(0, 16)]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the random hex.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="length">The length.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] CreateRandomHex(this object anyObject, int length)
        {
            byte[] result = new byte[length];
            var sb = new StringBuilder(2);

            for (int i = 0; i < length; i++)
            {
                sb.Clear();
                sb.Append(alpha[random.Next(0, 16)]);
                sb.Append(alpha[random.Next(0, 16)]);

                result[i] = Convert.ToByte(sb.ToString(), 16);
            }

            return result;
        }

        /// <summary>
        /// Gets the random string.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="length">The length.</param>
        /// <returns>System.String.</returns>
        public static string CreateRandomString(this object anyObject, int length)
        {
            var sb = new StringBuilder(length);

            for (var i = 0; i < length; i++)
            {
                sb.Append(alpha[random.Next(36)]);
            }

            return sb.ToString();
        }

        #endregion Random

        #region Enum

        /// <summary>
        /// Gets the enum contract text.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>System.String.</returns>
        public static string GetEnumDescriptionText<T>(this T enumValue)
            where T : struct, IConvertible, IComparable, IFormattable
        {
            var fieldInfo = typeof(T).GetField(enumValue.ToString());
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return (attributes.Length > 0) ? attributes[0].Description : enumValue.ToString();
        }

        /// <summary>
        /// Parses to enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumString">The enum string.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>T.</returns>
        public static T ParseToEnum<T>(this string enumString, T defaultValue = default(T)) where T : struct, IConvertible
        {
            T value;
            return (!string.IsNullOrWhiteSpace(enumString) && Enum.TryParse(enumString, out value)) ? value : defaultValue;
        }

        /// <summary>
        /// Parses to enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>T.</returns>
        public static T ParseToEnum<T>(this int enumValue, T defaultValue = default(T)) where T : struct, IConvertible
        {
            var enumType = typeof(T);
            return (Enum.IsDefined(enumType, enumValue)) ? (T)Enum.ToObject(typeof(T), enumValue) : defaultValue;
        }

        /// <summary>
        /// Adds the enum flag.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="value">The value.</param>
        /// <returns>T.</returns>
        public static T AddEnumFlag<T>(this T enumValue, T value)
           where T : struct, IConvertible, IComparable, IFormattable
        {
            if (typeof(T).IsEnum)
            {
                return (T)(Enum.ToObject(typeof(T), enumValue.ToInt64(null) | value.ToInt64(null)));
            }
            else
            {
                return enumValue;
            }
        }

        /// <summary>
        /// Removes the enum flag.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="value">The value.</param>
        /// <returns>T.</returns>
        public static T RemoveEnumFlag<T>(this T enumValue, T value)
         where T : struct, IConvertible, IComparable, IFormattable
        {
            if (typeof(T).IsEnum)
            {
                return (T)(Enum.ToObject(typeof(T), enumValue.ToInt64(null) & (~value.ToInt64(null))));
            }
            else
            {
                return enumValue;
            }
        }

        /// <summary>
        /// Gets the enum flag values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>System.Collections.Generic.IList&lt;T&gt;.</returns>
        public static IList<T> GetEnumFlagValues<T>(this T enumValue)
               where T : struct, IConvertible, IComparable, IFormattable
        {
            IList<T> result = new List<T>();

            if (typeof(T).IsEnum)
            {
                Int64 value = enumValue.ToInt64(null);

                foreach (T one in Enum.GetValues(typeof(T)))
                {
                    if ((one.ToInt64(null) & value) > 0)
                    {
                        result.Add(one);
                    }
                }
            }

            return result;
        }

        #endregion Enum

        #region Ensure & Testify

        /// <summary>
        /// Ensures the specified object. If <c>ensureCondition</c> output is false, use <c>defaultValue</c> instead.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="ensureCondition">The ensure condition.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>T.</returns>
        public static T Ensure<T>(this T obj, Func<T, bool> ensureCondition, T defaultValue = default(T))
        {
            return (ensureCondition != null && !ensureCondition(obj)) ? defaultValue : obj;
        }

        /// <summary>
        /// Testifies the specified object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="objectName">Name of the object.</param>
        /// <param name="ensureCondition">The ensure condition.</param>
        public static void Testify<T>(this T obj, string objectName, Func<T, bool> ensureCondition)
        {
            if (ensureCondition != null && !ensureCondition(obj))
            {
                throw ExceptionFactory.CreateInvalidObjectException(objectName, obj);
            }
        }

        /// <summary>
        /// The is int32 natural
        /// </summary>
        private static Func<int, bool> isInt32Natural = x => x >= 0;

        /// <summary>
        /// The is int64 natural
        /// </summary>
        private static Func<long, bool> isInt64Natural = x => x >= 0;

        /// <summary>
        /// Ensures the natural number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Int32.</returns>
        public static int EnsureNaturalNumber(this int value, int defaultValue = 0)
        {
            return Ensure(value, isInt32Natural, defaultValue);
        }

        /// <summary>
        /// Testifies the natural number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="objectName">Name of the object.</param>
        public static void TestifyNaturalNumber(this int value, string objectName)
        {
            Testify(value, objectName, isInt32Natural);
        }

        /// <summary>
        /// Ensures the natural number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.Int64.</returns>
        public static long EnsureNaturalNumber(this long value, int defaultValue = 0)
        {
            return Ensure(value, isInt64Natural, defaultValue);
        }

        /// <summary>
        /// Testifies the natural number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="objectName">Name of the object.</param>
        public static void TestifyNaturalNumber(this long value, string objectName)
        {
            Testify(value, objectName, isInt64Natural);
        }

        #endregion Ensure & Testify

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="format">The format.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public static string ToString(this DateTime? dateTime, string format)
        {
            return dateTime.HasValue ? dateTime.ToString(format.SafeToString(fullDateTimeTZFormat)) : string.Empty;
        }

        /// <summary>
        /// Gets the underlying objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects">The objects.</param>
        /// <returns></returns>
        public static List<T> GetUnderlyingObjects<T>(this IEnumerable<BaseObject<T>> objects)
        {
            var list = new List<T>();

            if (objects.HasItem())
            {
                foreach (var one in objects)
                {
                    list.Add(one.Object);
                }
            }

            return list;
        }

        /// <summary>
        /// Gets the underlying objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects">The objects.</param>
        /// <returns></returns>
        public static List<T> GetUnderlyingObjects<T>(this IEnumerable<SimpleBaseObject<T>> objects)
        {
            var list = new List<T>();

            if (objects.HasItem())
            {
                foreach (var one in objects)
                {
                    list.Add(one.Object);
                }
            }

            return list;
        }
    }
}