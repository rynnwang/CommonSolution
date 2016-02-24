using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Beyova.ProgrammingIntelligence;

namespace Beyova
{
    public static partial class ReflectionExtension
    {
        #region Constant

        /// <summary>
        /// The path_ bin folder
        /// </summary>
        public const string Path_BinFolder = "bin";

        #endregion

        /// <summary>
        /// The generic regex
        /// </summary>
        private static Regex genericClassRegex = new Regex(@"(?<TypeName>([\w\.]+`([0-9]+)))\[(?<GenericTypeName>(([\w\.\[\],`]+)))\]", RegexOptions.Compiled);

        /// <summary>
        /// The generic code look class regex
        /// </summary>
        private static Regex genericCodeLookClassRegex = new Regex(@"(?<TypeName>([\w\.]+))\<(?<GenericTypeName>(([\w\.\[\],`]+)))\>", RegexOptions.Compiled);

        /// <summary>
        /// Gets the application domain assemblies.
        /// </summary>
        /// <param name="appDomain">The application domain.</param>
        /// <returns>IEnumerable{Assembly}.</returns>
        public static ICollection<Assembly> GetAppDomainAssemblies(AppDomain appDomain = null)
        {
            return (appDomain ?? AppDomain.CurrentDomain).GetAssemblies();
        }

        /// <summary>
        /// Gets the application base directory.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetApplicationBaseDirectory(this object anyObject)
        {
            return GetApplicationBaseDirectory();
        }

        /// <summary>
        /// Gets the application base directory.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetApplicationBaseDirectory()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            bool isWeb = !string.IsNullOrWhiteSpace(HttpRuntime.AppDomainAppVirtualPath);
            baseDirectory = isWeb ? Path.Combine(baseDirectory, Path_BinFolder) : baseDirectory;
            return baseDirectory;
        }

        /// <summary>
        /// Gets the assembly public key token.
        /// </summary>
        /// <param name="anyAssembly">Any assembly.</param>
        /// <returns>System.String.</returns>
        public static string GetAssemblyPublicKeyToken(this Assembly anyAssembly)
        {
            return anyAssembly != null ? anyAssembly.GetName().GetPublicKeyToken().ToHex() : string.Empty;
        }

        /// <summary>
        /// Determines whether [is system assembly] [the specified assembly].
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>System.Boolean.</returns>
        public static bool IsSystemAssembly(this Assembly assembly)
        {
            var info = assembly?.GetName();
            return info == null ? false : info.IsSystemAssembly();
        }

        /// <summary>
        /// Determines whether [is system assembly] [the specified assembly name].
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns>System.Boolean.</returns>
        public static bool IsSystemAssembly(this AssemblyName assemblyName)
        {
            return assemblyName != null
                && (assemblyName.Name.StartsWith("Microsoft.")
                    || assemblyName.Name.StartsWith("System.")
                    || assemblyName.Name.Equals("system", StringComparison.OrdinalIgnoreCase)
                    || assemblyName.Name.Equals("mscorlib", StringComparison.OrdinalIgnoreCase));
        }

        #region Property Info

        /// <summary>
        /// Determines whether the specified property information is override.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns><c>true</c> if the specified property information is override; otherwise, <c>false</c>.</returns>
        public static bool IsOverride(this PropertyInfo propertyInfo)
        {
            if (propertyInfo != null)
            {
                var getMethod = propertyInfo.GetGetMethod();
                if ((getMethod.Attributes & MethodAttributes.Virtual) != 0 &&
                    (getMethod.Attributes & MethodAttributes.NewSlot) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified property information is virtual.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns><c>true</c> if the specified property information is virtual; otherwise, <c>false</c>.</returns>
        public static bool IsVirtual(this PropertyInfo propertyInfo)
        {
            if (propertyInfo != null)
            {
                var getMethod = propertyInfo.GetGetMethod();
                if ((getMethod.Attributes & MethodAttributes.Virtual) != 0 &&
                    (getMethod.Attributes & MethodAttributes.NewSlot) != 0)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        /// <summary>
        /// Gets the actual affected properties.
        /// </summary>
        /// <param name="anyType">Any type.</param>
        /// <param name="bindingFlags">The binding flags.</param>
        /// <returns>List&lt;PropertyInfo&gt;.</returns>
        public static List<PropertyInfo> GetActualAffectedProperties(this Type anyType, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
        {
            if (anyType != null)
            {
                List<string> propertyNameList = new List<string>();
                var returnedProperties = new List<PropertyInfo>();

                foreach (var one in anyType.GetProperties(bindingFlags))
                {
                    if (!propertyNameList.Contains(one.Name))
                    {
                        returnedProperties.Add(one);
                        propertyNameList.Add(one.Name);
                    }
                }

                return returnedProperties.ToList();
            }

            return new List<PropertyInfo>();
        }

        /// <summary>
        /// Gets the actual affected fields.
        /// </summary>
        /// <param name="anyType">Any type.</param>
        /// <param name="bindingFlags">The binding flags.</param>
        /// <returns>List&lt;FieldInfo&gt;.</returns>
        public static List<FieldInfo> GetActualAffectedFields(this Type anyType, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField)
        {
            if (anyType != null)
            {
                List<string> fieldNameList = new List<string>();
                var returnedFields = new List<FieldInfo>();

                foreach (var one in anyType.GetFields(bindingFlags))
                {
                    if (!fieldNameList.Contains(one.Name))
                    {
                        returnedFields.Add(one);
                        fieldNameList.Add(one.Name);
                    }
                }

                return returnedFields.ToList();
            }

            return new List<FieldInfo>();
        }

        #region Create instance

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>System.Object.</returns>
        public static object CreateInstance(this Type type, object[] arguments = null)
        {
            if (type != null)
            {
                return Activator.CreateInstance(type, arguments);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>System.Object.</returns>
        public static object CreateInstance<T>()
        {
            return Activator.CreateInstance<T>();
        }

        /// <summary>
        /// Creates the generic instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="genericTypes">The generic types.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>System.Object.</returns>
        public static object CreateGenericInstance(this Type type, Type[] genericTypes, object[] arguments = null)
        {
            if (type != null)
            {
                var genericType = type.MakeGenericType(genericTypes);
                return Activator.CreateInstance(genericType, arguments);
            }

            return null;
        }

        #endregion

        #region Static utility methods

        /// <summary>
        /// Gets the assembly file version.
        /// </summary>
        /// <param name="assemblyObject">The assembly object.</param>
        /// <returns>System.String.</returns>
        public static string GetAssemblyFileVersion(this Assembly assemblyObject)
        {
            if (assemblyObject != null)
            {
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assemblyObject.Location);
                return fvi.FileVersion;
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the assembly version.
        /// </summary>
        /// <param name="assemblyObject">The assembly object.</param>
        /// <returns>Version.</returns>
        public static Version GetAssemblyVersion(this Assembly assemblyObject)
        {
            return (assemblyObject != null) ? assemblyObject.GetName().Version : null;
        }

        /// <summary>
        /// Gets the name of the generic type. e.g.: System.Collections.Generic.List`1[Beyova.ServiceCredential]
        /// </summary>
        /// <param name="fullName">The full name.</param>
        /// <param name="isCodeLook">The is code look.</param>
        /// <param name="genericTypeNames">The generic type names.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>
        ///   <c>true</c> if succeed to get, <c>false</c> otherwise.</returns>
        internal static bool GetGenericTypeName(string fullName, bool isCodeLook, out string[] genericTypeNames, out string typeName)
        {
            typeName = string.Empty;
            genericTypeNames = new string[] { };

            var match = (isCodeLook ? genericCodeLookClassRegex : genericClassRegex).Match(fullName);
            if (match.Success)
            {
                genericTypeNames = match.Result("${GenericTypeName}").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                typeName = match.Result("${TypeName}");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the type smartly. It would find type in all related assemblies. And even if type name is generic based, it can still give right result.
        /// e.g.: System.Collections.Generic.List`1[Beyova.ServiceCredential]
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="isCodeLook">The is code look.</param>
        /// <returns>Type.</returns>
        public static Type SmartGetType(string typeName, bool isCodeLook = false)
        {
            try
            {
                string baseTypeName;
                string[] genericTypeNames;
                if (GetGenericTypeName(typeName, isCodeLook, out genericTypeNames, out baseTypeName))
                {
                    var baseType = SmartGetType(baseTypeName, isCodeLook);

                    if (baseType == typeof(Nullable))
                    {
                        baseType = typeof(Nullable<>);
                    }
                    return baseType.MakeGenericType((from one in genericTypeNames select SmartGetType(one)).ToArray());
                }
                else
                {
                    var assemblies = GetAppDomainAssemblies();

                    if (assemblies != null)
                    {
                        return assemblies.SelectMany(one => one.GetTypes()).FirstOrDefault(type => type.Name.Equals(typeName) || type.FullName.Equals(typeName));
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                var typeLoadException = ex as ReflectionTypeLoadException;

                if (typeLoadException != null && typeLoadException.LoaderExceptions.Any())
                {
                    throw typeLoadException.LoaderExceptions.First().Handle("GetType", typeName);
                }
                else
                {
                    throw ex.Handle("GetType", typeName);
                }
            }
        }

        /// <summary>
        /// Matches the interface method.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <param name="attributeInherit">if set to <c>true</c> [attribute inherit].</param>
        /// <returns><c>true</c> if matches interface method, <c>false</c> otherwise.</returns>
        private static bool MatchInterfaceMethod(MethodInfo methodInfo, Type attributeType, bool attributeInherit)
        {
            return (attributeType == null || methodInfo.GetCustomAttribute(attributeType, attributeInherit) != null);
        }


        /// <summary>
        /// Gets the interface method.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <param name="attributeInherit">if set to <c>true</c> [attribute inherit].</param>
        /// <returns>MethodInfo.</returns>
        public static MethodInfo GetInterfaceMethod(this Type type, string name, bool ignoreCase = false, Type attributeType = null, bool attributeInherit = true)
        {
            if (!string.IsNullOrWhiteSpace(name) && type != null)
            {
                if (type.IsInterface)
                {
                    foreach (var item in type.GetMethods())
                    {
                        if (item.Name.Equals(name, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)
                            && MatchInterfaceMethod(item, attributeType, attributeInherit))
                        {
                            return item;
                        }
                    }
                }

                foreach (var one in type.GetInterfaces())
                {
                    foreach (var item in one.GetMethods())
                    {
                        if (item.Name.Equals(name, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)
                            && MatchInterfaceMethod(item, attributeType, attributeInherit))
                        {
                            return item;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the interface methods.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <param name="attributeInherit">if set to <c>true</c> [attribute inherit].</param>
        /// <returns>List&lt;MethodInfo&gt;.</returns>
        public static List<MethodInfo> GetInterfaceMethods(this Type type, Type attributeType, bool attributeInherit)
        {
            HashSet<MethodInfo> result = new HashSet<MethodInfo>();

            if (type != null)
            {
                if (type.IsInterface)
                {
                    foreach (var item in type.GetMethods())
                    {
                        if (MatchInterfaceMethod(item, attributeType, attributeInherit))
                        {
                            result.Add(item);
                        }
                    }
                }

                foreach (var one in type.GetInterfaces())
                {
                    foreach (var item in one.GetMethods())
                    {
                        if (MatchInterfaceMethod(item, attributeType, attributeInherit))
                        {
                            result.Add(item);
                        }
                    }
                }
            }

            return result.ToList();
        }

        /// <summary>
        /// Gets the type of the generic.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="genericTypeNames">The generic type names.</param>
        /// <returns>Type.</returns>
        public static Type GetGenericType(string typeName, string[] genericTypeNames)
        {
            Type result = null;
            var containerType = SmartGetType(typeName);

            result = containerType.MakeGenericType(genericTypeNames.Select(one => SmartGetType(one)).ToArray());

            return result;
        }

        /// <summary>
        /// Gets the property names.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static IEnumerable<string> GetPropertyNames(object obj)
        {
            IEnumerable<string> result = null;

            if (obj != null)
            {
                result = from one in obj.GetType().GetProperties() select one.Name;
            }

            return result;
        }

        /// <summary>
        /// Gets the type of the methods from.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="methodAccessFiltering">The method access filtering.</param>
        /// <returns>List{MethodInfo}.</returns>
        public static List<MethodInfo> GetMethodsFromType(Type type, MethodFiltering methodAccessFiltering = null)
        {
            List<MethodInfo> result = new List<MethodInfo>();

            if (type != null)
            {
                if (methodAccessFiltering == null)
                {
                    methodAccessFiltering = new MethodFiltering();
                }
                var methods = type.GetMethods();

                foreach (var one in methods)
                {
                    if (methodAccessFiltering.IncludedAttribute == null || one.HasAttribute(methodAccessFiltering.IncludedAttribute))
                    {
                        result.Add(one);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the parameter type from method info.
        /// </summary>
        /// <param name="methodInfo">The method info.</param>
        /// <param name="index">The index.</param>
        /// <returns>Type.</returns>
        public static Type GetParameterTypeFromMethodInfo(MethodInfo methodInfo, int index = 0)
        {
            Type result = null;

            if (methodInfo != null)
            {
                var parameters = methodInfo.GetParameters();
                if (parameters != null && index < parameters.Length)
                {
                    result = parameters[index].ParameterType;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the parameter type from method info.
        /// </summary>
        /// <param name="methodInfo">The method info.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>Type.</returns>
        public static Type GetParameterTypeFromMethodInfo(MethodInfo methodInfo, string parameterName)
        {
            Type result = null;

            if (methodInfo != null && !string.IsNullOrWhiteSpace(parameterName))
            {

                var parameters = methodInfo.GetParameters();
                result = (from one in parameters where one.Name.Equals(parameterName) select one.ParameterType).FirstOrDefault();
            }

            return result;
        }

        #endregion

        #region Extensions

        /// <summary>
        /// Determines whether the specified type is nullable.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type is nullable; otherwise, <c>false</c>.</returns>
        public static bool IsNullable(this Type type)
        {
            return type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Determines whether is field nullable (is class or is nullable&lt;&gt;).
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is field nullable] [the specified type]; otherwise, <c>false</c>.</returns>
        public static bool IsFieldNullable(this Type type)
        {
            return type != null && (type.IsClass || type.IsNullable());
        }

        /// <summary>
        /// Determines whether the specified type is dictionary.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type is dictionary; otherwise, <c>false</c>.</returns>
        public static bool IsDictionary(this Type type)
        {
            return type != null && type.HasInterface(typeof(IDictionary<,>));
        }

        /// <summary>
        /// Determines whether the specified type is collection.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is collection; otherwise, <c>false</c>.</returns>
        public static bool IsCollection(this Type type)
        {
            return type != null && type.HasInterface(typeof(ICollection<>));
        }

        #region Nullable 

        /// <summary>
        /// Gets the type of the nullable.
        /// <remarks>
        /// If specific type is not Nullable, return type itself.
        /// </remarks>
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Type.</returns>
        public static Type TryGetNullableType(this Type type)
        {
            return GetNullableType(type) ?? type;
        }

        /// <summary>
        /// Gets the type of the nullable.
        /// <remarks>
        /// If specific type is not Nullable, return null.
        /// </remarks>
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Type.</returns>
        public static Type GetNullableType(this Type type)
        {
            return Nullable.GetUnderlyingType(type);
        }

        #endregion

        /// <summary>
        /// Inheritses from.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="inheritedType">Type of the inherited.</param>
        /// <returns><c>true</c> if inherits from specific type, <c>false</c> otherwise.</returns>
        public static bool InheritsFrom(this Type type, Type inheritedType)
        {
            return type != null && inheritedType != null && (inheritedType.IsAssignableFrom(type) || (
                type.IsGenericType && type.GetGenericTypeDefinition() == inheritedType
                ));
        }

        /// <summary>
        /// Determines whether [is simple type] [the specified type]. Like: Guid, string, int32, int64, etc.
        /// If it is Nullable&lt;T&gt;, it would detect T directly.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is simple type] [the specified type]; otherwise, <c>false</c>.</returns>
        public static bool IsSimpleType(this Type type)
        {
            return type != null &&
                type.IsNullable() ? IsSimpleType(type.GetNullableType())
                : (type.IsPrimitive || (typeof(string) == type) || typeof(Guid) == type || typeof(DateTime) == type || typeof(decimal) == type || typeof(TimeSpan) == type || typeof(Uri) == type || type.IsEnum);
        }

        /// <summary>
        /// Determines whether the specified method has attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method">The method.</param>
        /// <param name="inherit">The inherit.</param>
        /// <returns><c>true</c> if the specified method has attribute; otherwise, <c>false</c>.</returns>
        public static bool HasAttribute<T>(this MethodInfo method, bool inherit = true) where T : Attribute
        {
            return method != null && method.GetCustomAttribute<T>(inherit) != null;
        }

        /// <summary>
        /// Determines whether the specified method has attribute.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if the specified method has attribute; otherwise, <c>false</c>.</returns>
        public static bool HasAttribute(this MethodInfo method, Attribute attribute, bool inherit = true)
        {
            return method != null && method.GetCustomAttribute(attribute.GetType(), inherit) != null;
        }

        /// <summary>
        /// Determines whether the specified type has attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if the specified type has attribute; otherwise, <c>false</c>.</returns>
        public static bool HasAttribute<T>(this Type type, bool inherit = true) where T : Attribute
        {
            return type?.GetCustomAttribute<T>(inherit) != null;
        }

        /// <summary>
        /// Determines whether the specified type has attribute.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns><c>true</c> if the specified type has attribute; otherwise, <c>false</c>.</returns>
        public static bool HasAttribute(this Type type, Attribute attribute, bool inherit = true)
        {
            return type?.GetCustomAttribute(attribute.GetType(), inherit) != null;
        }

        /// <summary>
        /// Determines whether the specified type has interface.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <returns>System.Boolean.</returns>
        public static bool HasInterface(this Type type, Type interfaceType)
        {
            return type != null && interfaceType != null && type.GetInterface(interfaceType.Name) != null;
        }

        /// <summary>
        /// Methods the input parameters to code look.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="includeType">Type of the include.</param>
        /// <returns>System.String.</returns>
        internal static string MethodInputParametersToCodeLook(this MethodInfo methodInfo, bool includeType = true)
        {
            StringBuilder builder = new StringBuilder();

            if (methodInfo != null)
            {
                foreach (var param in methodInfo.GetParameters())
                {
                    if (includeType)
                    {
                        builder.AppendFormat("{0} {1},", param.ParameterType.ToCodeLook(true), param.Name);
                    }
                    else
                    {
                        builder.AppendFormat("{0},", param.Name);
                    }
                }
            }

            if (builder.Length > 0)
            {
                builder.RemoveLast();
            }

            return builder.ToString();
        }

        #region ToCodeLook

        /// <summary>
        /// To the code look.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="includeType">Type of the include.</param>
        /// <returns>System.String.</returns>
        public static string ToCodeLook(this MethodInfo methodInfo, bool includeType = true)
        {
            return ToCodeLook(methodInfo, includeType, null);
        }

        /// <summary>
        /// To the code look.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="includeType">Type of the include.</param>
        /// <param name="genericParameterTypeNames">The generic parameter type names.</param>
        /// <returns>System.String.</returns>
        internal static string ToCodeLook(this MethodInfo methodInfo, bool includeType, ICollection<string> genericParameterTypeNames)
        {
            if (methodInfo != null)
            {
                if (methodInfo.IsGenericMethod)
                {
                    var builder = new StringBuilder();
                    foreach (var t in methodInfo.GetGenericArguments())
                    {
                        builder.Append(t.Name + ",");
                    }
                    if (builder.Length > 0)
                    {
                        builder.RemoveLast(1);
                    }

                    return string.Format("{0} {1}<{2}>({3})", methodInfo.ReturnType.ToCodeLook(true), methodInfo.Name, builder, methodInfo.MethodInputParametersToCodeLook(includeType));
                }
                else if (genericParameterTypeNames.HasItem())
                {
                    HashSet<string> genericParameters = new HashSet<string>();
                    genericParameters.AddRange(methodInfo.ReturnType.GetGenericParameterNames(genericParameterTypeNames));

                    foreach (var one in methodInfo.GetParameters())
                    {
                        genericParameters.AddRange(one.ParameterType.GetGenericParameterNames(genericParameterTypeNames));
                    }

                    if (genericParameters.Count > 0)
                    {
                        return string.Format("{0} {1}<{2}>({3})", methodInfo.ReturnType.ToCodeLook(true), methodInfo.Name, genericParameters.Join(","), methodInfo.MethodInputParametersToCodeLook(includeType));
                    }
                }

                return string.Format("{0} {1}({2})", methodInfo.ReturnType.ToCodeLook(true), methodInfo.Name, methodInfo.MethodInputParametersToCodeLook(includeType));
            }

            return string.Empty;
        }

        /// <summary>
        /// To the code look. This method is to convert <see cref="Type" /> to code based., such as List&lt;String&gt;, System.Nullable&lt;System.Guid&gt;, etc.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="includingNamespace">The including namespace.</param>
        /// <param name="seperator">The seperator.</param>
        /// <returns>System.String.</returns>
        public static string ToCodeLook(this Type type, bool includingNamespace = false, string seperator = ".")
        {
            string result = string.Empty;

            if (type != null)
            {
                if (type == typeof(void))
                {
                    return "void";
                }
                else if (type.IsGenericParameter)
                {
                    return type.Name;
                }

                if (type.IsGenericType)
                {
                    var builder = new StringBuilder();
                    foreach (var t in type.GetGenericArguments())
                    {
                        builder.Append(t.ToCodeLook(includingNamespace) + ",");
                    }
                    if (builder.Length > 0)
                    {
                        builder.RemoveLast();
                    }
                    result = includingNamespace ?
                            string.Format("{0}{1}{2}<{3}>", type.Namespace, string.IsNullOrWhiteSpace(type.Namespace) ? string.Empty : seperator, type.Name.SubStringBeforeFirstMatch('`'), builder) :
                            string.Format("{0}<{1}>", type.Name.SubStringBeforeFirstMatch('`'), builder);
                }
                else
                {
                    // NOTE: if type is come from generic method, like: T1 Method(T2 t), FullName would be null.
                    result = includingNamespace ? string.Format("{0}{1}{2}", type.Namespace, string.IsNullOrWhiteSpace(type.Namespace) ? string.Empty : seperator, type.Name) : type.Name;
                }
            }

            return result;
        }

        #endregion

        /// <summary>
        /// Gets the methods.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="methodAccessFiltering">The method access filtering.</param>
        /// <returns>List{MethodInfo}.</returns>
        public static List<MethodInfo> GetMethods(this Type type, MethodFiltering methodAccessFiltering)
        {
            List<MethodInfo> result = new List<MethodInfo>();

            if (type != null)
            {
                if (methodAccessFiltering == null)
                {
                    methodAccessFiltering = new MethodFiltering();
                }
                var methods = type.GetMethods();

                foreach (var one in methods)
                {
                    if (methodAccessFiltering.IncludedAttribute == null || one.HasAttribute(methodAccessFiltering.IncludedAttribute))
                    {
                        result.Add(one);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the parameter type from method info.
        /// </summary>
        /// <param name="methodInfo">The method info.</param>
        /// <param name="index">The index.</param>
        /// <returns>Type.</returns>
        public static Type GetParameterType(this MethodInfo methodInfo, int index = 0)
        {
            Type result = null;

            if (methodInfo != null)
            {
                var parameters = methodInfo.GetParameters();
                if (parameters != null && index < parameters.Length)
                {
                    result = parameters[index].ParameterType;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>System.Object.</returns>
        public static object GetPropertyValue(object anyObject, string propertyName)
        {
            object result = null;

            if (anyObject != null && !string.IsNullOrWhiteSpace(propertyName))
            {
                var property = anyObject.GetType().GetProperty(propertyName);

                if (property != null)
                {
                    result = property.GetValue(anyObject);
                }
            }

            return result;
        }

        #endregion

        #region StackTrace

        /// <summary>
        /// Gets the current executing method.
        /// </summary>
        /// <returns>MethodBase.</returns>
        public static MethodBase GetCurrentExecutingMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return sf.GetMethod();
        }

        #endregion

        /// <summary>
        /// Gets the method information within attribute. As MSDN said, you have to assign either <see cref="BindingFlags.Instance"/> or <see cref="BindingFlags.Static"/> for bindingFlags, so that you can get right result.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the t attribute.</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="inherit">The inherit.</param>
        /// <param name="bindingFlags">The binding flags.</param>
        /// <returns>IEnumerable&lt;MethodInfo&gt;.</returns>
        public static IEnumerable<MethodInfo> GetMethodInfoWithinAttribute<TAttribute>(this Type type, bool inherit = true, BindingFlags bindingFlags = BindingFlags.Default) where TAttribute : Attribute
        {
            List<MethodInfo> result = new List<MethodInfo>();

            if (type != null)
            {
                result.AddRange(from item in type.GetMethods(bindingFlags)
                                where item.HasAttribute<TAttribute>(inherit)
                                select item);

                if (inherit)
                {
                    foreach (var one in type.GetInterfaces())
                    {
                        result.AddRange(GetMethodInfoWithinAttribute<TAttribute>(one, inherit, bindingFlags));
                    }

                    result = result.Distinct().ToList();
                }
            }

            return result;
        }

        /// <summary>
        /// Determines whether the specified type is void.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type is void; otherwise, <c>false</c>.</returns>
        public static bool? IsVoid(this Type type)
        {
            return type == null ? null : (type == typeof(void)) as bool?;
        }

        /// <summary>
        /// Gets the generic parameter names.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="genericParameterTypeNames">The generic parameter type names.</param>
        /// <returns>System.Collections.Generic.List&lt;System.String&gt;.</returns>
        internal static List<string> GetGenericParameterNames(this Type type, ICollection<string> genericParameterTypeNames)
        {
            List<string> result = new List<string>();

            if (type != null)
            {
                if (type.IsGenericType)
                {
                    foreach (var one in type.GetGenericArguments())
                    {
                        result.AddRange(GetGenericParameterNames(one, genericParameterTypeNames));
                    }
                }
                else if (type.IsGenericParameter)
                {
                    result.Add(type.Name);
                }
            }

            return result;
        }

        #region Assembly Dependency Chain

        /// <summary>
        /// Class AssemblyDependency.
        /// </summary>
        class AssemblyDependency
        {
            /// <summary>
            /// Gets or sets the last.
            /// </summary>
            /// <value>The last.</value>
            public AssemblyDependency Last { get; set; }

            /// <summary>
            /// Gets or sets the next.
            /// </summary>
            /// <value>The next.</value>
            public AssemblyDependency Next { get; set; }

            /// <summary>
            /// Gets or sets the assembly.
            /// </summary>
            /// <value>The assembly.</value>
            public Assembly Assembly { get; set; }

            /// <summary>
            /// Gets or sets the level.
            /// </summary>
            /// <value>The level.</value>
            public int Level { get; set; }

            /// <summary>
            /// Gets or sets the referenced count.
            /// </summary>
            /// <value>The referenced count.</value>
            public int ReferencedCount { get; set; }
        }

        /// <summary>
        /// Gets the assembly dependency chain.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns>List&lt;Assembly&gt;.</returns>
        public static List<Assembly> GetAssemblyDependencyChain(this ICollection<Assembly> assemblies)
        {
            List<AssemblyDependency> container = new List<AssemblyDependency>();

            foreach (var one in assemblies)
            {
                if (!one.GetName().IsSystemAssembly() && !one.IsDynamic)
                {
                    FillAssemblyDependency(container, one, null, 0);
                }
            }
            List<Assembly> result = new List<Assembly>();
            foreach (var one in (from item in container orderby item.Level descending, item.ReferencedCount descending select item))
            {
                result.Add(one.Assembly);
            }

            return result;
        }

        /// <summary>
        /// Tries the load assembly.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns>System.Reflection.Assembly.</returns>
        public static Assembly TryLoadAssembly(this AssemblyName assemblyName)
        {
            try
            {
                return Assembly.Load(assemblyName);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Fills the assembly dependency.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="lastDependency">The last dependency.</param>
        /// <param name="currentLevel">The current level.</param>
        private static void FillAssemblyDependency(List<AssemblyDependency> container, Assembly assembly, AssemblyDependency lastDependency, int currentLevel)
        {
            if (assembly == null || container == null)
            {
                return;
            }

            try
            {
                var found = (from item in container where item.Assembly == assembly select item).FirstOrDefault();
                AssemblyDependency thisDependency = null;

                if (found == null)
                {
                    thisDependency = new AssemblyDependency
                    {
                        Last = lastDependency,
                        Assembly = assembly,
                        Level = currentLevel
                    };

                    container.Add(thisDependency);
                }
                else
                {
                    thisDependency = found;

                    if (thisDependency.Level > 256)
                    {
                        return;
                    }

                    if (currentLevel > thisDependency.Level)
                    {
                        thisDependency.Level = currentLevel;
                        if (thisDependency.Last != null)
                        {
                            thisDependency.Last.Next = thisDependency.Next;
                        }

                        if (thisDependency.Next != null)
                        {
                            thisDependency.Next.Last = thisDependency.Last;
                        }

                        thisDependency.Last = lastDependency;
                        thisDependency.Next = null;
                    }
                }

                thisDependency.ReferencedCount++;

                foreach (var one in assembly.GetReferencedAssemblies())
                {
                    if (!one.IsSystemAssembly())
                    {
                        FillAssemblyDependency(container, one.TryLoadAssembly(), thisDependency, currentLevel + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("FillAssemblyDependency", new { assembly = assembly?.FullName, currentLevel });
            }
        }
        #endregion

        #region Assembly Attributes

        /// <summary>
        /// Gets the component attribute.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>Beyova.ProgrammingIntelligence.BeyovaComponentAttribute.</returns>
        public static BeyovaComponentAttribute GetComponentAttribute(this Assembly assembly)
        {
            return assembly?.GetCustomAttribute<BeyovaComponentAttribute>();
        }

        #endregion

        #region GetFullName

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <param name="methodBase">The method base.</param>
        /// <param name="requiredAttribute">The required attribute.</param>
        /// <returns>System.String.</returns>
        public static string GetFullName(this MethodBase methodBase, Type requiredAttribute = null)
        {
            return (methodBase != null && (requiredAttribute == null || methodBase.GetCustomAttribute(requiredAttribute) != null)) ?
                string.Format("{0}.{1}", methodBase.DeclaringType.GetFullName(), methodBase.Name)
                : string.Empty;
        }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="requiredAttribute">The required attribute.</param>
        /// <returns>System.String.</returns>
        public static string GetFullName(this Type type, Type requiredAttribute = null)
        {
            if (type != null && (requiredAttribute == null || type.GetCustomAttribute(requiredAttribute) != null))
            {
                return type.ToCodeLook(true, ".");
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion
    }


}
