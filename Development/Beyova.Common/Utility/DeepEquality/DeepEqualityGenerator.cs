using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Beyova.ProgrammingIntelligence;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    internal static class DeepEqualityGenerator
    {
        /// <summary>
        /// The namespace
        /// </summary>
        private const string _namespace = "Beyova.ProgrammingIntelligence.DeepEquality";

        /// <summary>
        /// The equality instances
        /// </summary>
        private static Dictionary<Type, object> equalityInstances = new Dictionary<Type, object>();

        /// <summary>
        /// The locker
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static string GetTypeName(Type type)
        {
            if (type != null)
            {
                string timestamp = DateTime.UtcNow.Ticks.ToString();
                return type.IsGenericType ? string.Format("{0}_{1}_{2}", type.Name, type.GetGenericArguments().Length, timestamp) : string.Format("{0}_{1}", type.Name, timestamp);
            }

            return string.Empty;
        }

        /// <summary>
        /// Creates the deep equality instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static IDeepEquality<T> CreateDeepEqualityInstance<T>(StringComparison stringComparison)
        {
            Type type = typeof(T);
            object instance;
            if (!equalityInstances.TryGetValue(type, out instance))
            {
                lock (locker)
                {
                    if (!equalityInstances.TryGetValue(type, out instance))
                    {
                        var typeName = GetTypeName(type);
                        var codes = CreateDeepEqualityClassCode(type, typeName, stringComparison);

                        TempAssemblyProvider provider = new TempAssemblyProvider();
                        var tmpAssembly = provider.CreateTempAssembly(codes, TempAssemblyProvider.GetCurrentAppDomainAssemblyLocations(), type.Namespace.AsArray(), @namespace: _namespace);
                        instance = tmpAssembly.CreateInstance(string.Format("{0}.{1}", _namespace, typeName));
                        equalityInstances.Add(type, instance);
                    }
                }
            }

            return instance as IDeepEquality<T>;
        }

        /// <summary>
        /// Fills the and terms.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="objects">The objects.</param>
        /// <param name="getType">Type of the get.</param>
        private static void FillAndTerms<T>(StringBuilder builder, IEnumerable<T> objects, Func<T, Type> getType)
            where T : MemberInfo
        {
            if (objects.HasItem() && getType != null)
            {
                foreach (var one in objects)
                {
                    if (one.GetCustomAttribute<DeepEqualityIgnoreAttribute>() == null)
                    {
                        var type = getType(one);
                        if (type == typeof(string))
                        {
                            builder.AppendFormat(" obj1.{0}.Equals(obj2.{0}, stringComparison) &&", one.Name);
                        }
                        else if (type.IsSimpleType())
                        {
                            builder.AppendFormat(" obj1.{0}.Equals(obj2.{0}) &&", one.Name);
                        }
                        else
                        {
                            builder.AppendFormat(" DeepEquality.DeepEquals(obj1.{0}, obj2.{0}, stringComparison) &&", one.Name);
                        }
                    }
                }

                builder.RemoveLastIfMatch("&&");
            }
        }

        /// <summary>
        /// Creates the deep equality class code.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="stringComparison">The string comparison.</param>
        /// <returns></returns>
        private static string CreateDeepEqualityClassCode(Type type, string typeName, StringComparison stringComparison)
        {
            if (type != null)
            {
                var getProperties = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty);
                var getFields = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetField);

                StringBuilder builder = new StringBuilder(4096 + getProperties.Length * 256 + getFields.Length * 256);

                var deepEqualityType = typeof(IDeepEquality<>);

                builder.AppendLineWithFormat("public class {0}: {1}", typeName, deepEqualityType.MakeGenericType(type).ToCodeLook());
                CSharpCodeGenerateUtil.AppendBeginBrace(builder);

                // Constructor
                builder.AppendLineWithFormat("public {0}()", typeName);
                builder.AppendLine("{");
                builder.AppendLine("}");

                // DeepEquals
                builder.AppendLineWithFormat("public bool DeepEquals({0} obj1, {0} obj2, StringComparison stringComparison)", type.ToCodeLook());
                builder.AppendLine("{");
                builder.AppendLine("if(obj1 != null && obj2 != null)");
                builder.AppendLine("{");

                builder.Append("return");

                StringBuilder booleanTerms = new StringBuilder(getProperties.Length * 256 + getFields.Length * 256);

                FillAndTerms(booleanTerms, getProperties, x => x.PropertyType);

                if (booleanTerms.Length > 0 && getFields.HasItem())
                {
                    booleanTerms.Append(" &&");
                    FillAndTerms(booleanTerms, getFields, x => x.FieldType);
                }
                builder.Append(booleanTerms);
                builder.AppendLine(";");

                builder.AppendLine("}");
                builder.AppendLine("else");
                builder.AppendLine("{");
                builder.AppendLine("return false;");
                builder.AppendLine("}");
                builder.AppendLine("}");

                // End of DeepEquals
                CSharpCodeGenerateUtil.AppendEndBrace(builder);

                return builder.ToString();
            }

            return string.Empty;
        }
    }
}