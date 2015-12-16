using System;
using System.ComponentModel;
using System.Reflection;
using Beyova.ExceptionSystem;

namespace Beyova
{
    /// <summary>
    /// Class ReflectionExtension.
    /// </summary>
    public static partial class ReflectionExtension
    {
        #region Convert Object

        /// <summary>
        /// Converts the objects by <see cref="Type" /> instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>System.Object.</returns>
        public static object ConvertToObjectByType(Type type, string value, bool throwException = true)
        {
            MethodInfo method = typeof(ReflectionExtension).GetMethod("ConvertToObject");

            MethodInfo genericMethod = method.MakeGenericMethod(new Type[] { type });
            return genericMethod.Invoke(null, new object[] { value, throwException });
        }

        /// <summary>
        /// Converts to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>``0.</returns>
        /// <exception cref="InvalidObjectException"></exception>
        public static T ConvertToObject<T>(this string value, bool throwException = true)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                {
                    //Cast ConvertFromString(string text) : object to (T)
                    return (T)converter.ConvertFromString(value);
                }
            }
            catch (Exception ex)
            {
                if (throwException)
                {
                    throw new InvalidObjectException(ex);
                }
            }

            return default(T);
        }

        #endregion

        #region Invoke methods

        /// <summary>
        /// Invokes the method.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="invokeParameters">The invoke parameters.</param>
        /// <param name="genericTypes">The generic types.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.InvalidCastException">Neither objectType or instance is valid.</exception>
        public static object InvokeMethod(Type objectType, object instance, string methodName, object[] invokeParameters, Type[] genericTypes = null, bool throwException = false)
        {
            try
            {
                if (objectType == null && instance == null)
                {
                    throw new InvalidCastException("Neither objectType or instance is valid.");
                }
                else
                {
                    if (objectType == null)
                    {
                        objectType = instance.GetType();
                    }

                    MethodInfo methodInfo = objectType.GetMethod(methodName);
                    methodInfo.CheckNullObject("methodName");

                    if (!methodInfo.IsStatic)
                    {
                        instance.CheckNullObject("instance");
                    }
                    else
                    {
                        instance = null;
                    }

                    if (methodInfo.IsGenericMethod)
                    {
                        genericTypes.CheckNullObject("genericTypes");
                        methodInfo = methodInfo.MakeGenericMethod(genericTypes);
                    }

                    return methodInfo.Invoke(instance, invokeParameters);
                }
            }
            catch (Exception ex)
            {
                if (throwException)
                {
                    throw ex.Handle(string.Format("InvokeMethod of [{0}] in [{1}]", methodName.SafeToString(), (objectType == null ? "null" : objectType.FullName)));
                }
            }

            return null;
        }

        /// <summary>
        /// Invokes the static generic method.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="invokeParameters">The invoke parameters.</param>
        /// <param name="genericTypes">The generic types.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>System.Object.</returns>
        public static object InvokeStaticGenericMethod(Type objectType, string methodName, object[] invokeParameters, Type[] genericTypes = null, bool throwException = false)
        {
            return InvokeMethod(objectType, null, methodName, invokeParameters, genericTypes, throwException);
        }

        #endregion

        #region CreateSampleObject

        /// <summary>
        /// Creates the sample object.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        public static object CreateSampleObject(this Type type)
        {
            object result = null;

            if (type != null)
            {
                result = Activator.CreateInstance(type);
                var publicProperties = type.GetProperties(BindingFlags.Public | BindingFlags.SetProperty);

                foreach (var one in publicProperties)
                {
                    one.SetValue(result, CreatePropertyValue(one));
                }
            }

            return result;
        }

        /// <summary>
        /// Creates the sample object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>``0.</returns>
        public static T CreateSampleObject<T>() where T : new()
        {
            T result = new T();

            var objectType = result.GetType();
            var publicProperties = objectType.GetProperties(BindingFlags.Public | BindingFlags.SetProperty);

            foreach (var one in publicProperties)
            {
                one.SetValue(result, CreatePropertyValue(one));
            }

            return result;
        }

        /// <summary>
        /// Creates the property value.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns>System.Object.</returns>
        public static object CreatePropertyValue(this PropertyInfo propertyInfo)
        {
            object result = null;

            if (propertyInfo != null)
            {
                var type = propertyInfo.PropertyType;

                if (type.IsNullable())
                {
                    result = CreateSampleObject(type.GenericTypeArguments[0]);
                }
                else if (type.IsEnum)
                {
                    var enumValues = Enum.GetValues(type);
                    result = (enumValues != null && enumValues.Length > 0) ? enumValues.GetValue(0) : null;
                }
                else
                {
                    switch (type.FullName)
                    {
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                            result = GetRandomNumber(1000, 1);
                            break;
                        case "System.Single":
                        case "System.Double":
                            result = Single.Parse(GetRandomNumber(10, 0) + "." + GetRandomNumber(100, 0));
                            break;
                        case "System.String":
                            result = "String Value";
                            break;
                        case "System.Guid":
                            result = Guid.NewGuid();
                            break;
                        case "System.DateTime":
                            result = DateTime.Now;
                            break;
                        case "System.TimeSpan":
                            result = new TimeSpan(GetRandomNumber(23, 0), GetRandomNumber(60, 0), GetRandomNumber(60, 0));
                            break;
                        default:
                            break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the random number.
        /// </summary>
        /// <param name="max">The maximum.</param>
        /// <param name="min">The minimum.</param>
        /// <returns>System.Int32.</returns>
        private static int GetRandomNumber(int max, int min = 0)
        {
            var random = new Random();
            return random.Next(min, max);
        }

        #endregion
    }
}
