using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class JsonDiffExtension.
    /// </summary>
    public static class JsonDiffExtension
    {
        /// <summary>
        /// Differences the specified identifier.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="content1">The content1.</param>
        /// <param name="content2">The content2.</param>
        /// <returns>System.Collections.Generic.List&lt;Beyova.ComparisonResult&gt;.</returns>
        public static List<ComparisonResult> Diff(string path, string identifier, JToken content1, JToken content2)
        {
            if (content1 == null || content2 == null)
            {
                if (content1 == null && content2 == null)
                {
                    return new List<ComparisonResult>();
                }
                else
                {
                    return (new ComparisonResult(identifier, identifier + ".{Object}")
                    {
                        Value1 = content1?.ToString(),
                        Value2 = content2?.ToString()
                    }).AsList();
                }
            }
            else if (content1.Type != content2.Type)
            {
                return (new ComparisonResult(identifier, identifier + ".{Type}")
                {
                    Value1 = content1.Type.ToString(),
                    Value2 = content2.Type.ToString()
                }).AsList();
            }
            else
            {
                switch (content1.Type)
                {
                    case JTokenType.Array:
                        return Diff(path, identifier, (JArray)content1, (JArray)content2);

                    case JTokenType.Object:
                        return Diff(identifier, (JObject)content1, (JObject)content2);

                    case JTokenType.Guid:
                        return SimpleValueDiff(path, identifier, content1.Value<Guid>(), content2.Value<Guid>()).AsList();

                    case JTokenType.Integer:
                        return SimpleValueDiff(path, identifier, content1.Value<int>(), content2.Value<int>()).AsList();

                    case JTokenType.Float:
                        return SimpleValueDiff(path, identifier, content1.Value<double>(), content2.Value<double>()).AsList();

                    case JTokenType.Bytes:
                        return SimpleValueDiff(path, identifier, content1.Value<byte[]>(), content2.Value<byte[]>()).AsList();

                    case JTokenType.Boolean:
                        return SimpleValueDiff(path, identifier, content1.Value<bool>(), content2.Value<bool>()).AsList();

                    case JTokenType.Date:
                        return SimpleValueDiff(path, identifier, content1.Value<DateTime>(), content2.Value<DateTime>()).AsList();

                    case JTokenType.String:
                    case JTokenType.TimeSpan:
                    case JTokenType.Uri:
                        return SimpleValueDiff(path, identifier, content1.Value<string>(), content2.Value<string>()).AsList();

                    default: break;
                }
            }

            return new List<ComparisonResult>();
        }

        /// <summary>
        /// Differences the specified identifier.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="content1">The content1.</param>
        /// <param name="content2">The content2.</param>
        /// <returns>System.Collections.Generic.List&lt;Beyova.ComparisonResult&gt;.</returns>
        private static List<ComparisonResult> Diff(string path, string identifier, JArray content1, JArray content2)
        {
            List<ComparisonResult> result = new List<ComparisonResult>();

            if (content1.Count != content2.Count)
            {
                result.Add(new ComparisonResult(path, identifier + ".{Size}") { Value1 = content1.ToString(), Value2 = content2.ToString() });
            }
            else
            {
                for (var i = 0; i < content1.Count; i++)
                {
                    result.AddRange(Diff(identifier, string.Format("[{0}]", i), content1[i], content2[i]));
                }
            }

            return result;
        }

        /// <summary>
        /// Differences the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="content1">The content1.</param>
        /// <param name="content2">The content2.</param>
        /// <returns>System.Collections.Generic.List&lt;Beyova.ComparisonResult&gt;.</returns>
        private static List<ComparisonResult> Diff(string path, JObject content1, JObject content2)
        {
            var result = new List<ComparisonResult>();
            var propertyNames = new HashSet<string>();
            propertyNames.AddRange(content1.Properties().Select((x) => x.Name));
            propertyNames.AddRange(content2.Properties().Select((x) => x.Name));

            foreach (var name in propertyNames)
            {
                result.AddRange(Diff(path, "." + name, content1.Property(name)?.Value, content2.Property(name)?.Value));
            }

            return result;
        }

        /// <summary>
        /// Simples the value difference.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The last identifier.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="content1">The content1.</param>
        /// <param name="content2">The content2.</param>
        /// <returns>Beyova.ComparisonResult.</returns>
        private static ComparisonResult SimpleValueDiff<T>(string path, string identifier, T content1, T content2)
        {
            return content1.Equals(content2) ? null : new ComparisonResult(path, identifier)
            {
                Value1 = content1,
                Value2 = content2
            };
        }
    }
}