using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Beyova.ExceptionSystem;

namespace Beyova
{
    /// <summary>
    /// Class ContractModelExtension.
    /// </summary>
    public static class ContractModelExtension
    {
        #region Tree

        /// <summary>
        /// Builds the tree hierarchy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nodes">The nodes.</param>
        /// <param name="root">The root.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>T.</returns>
        public static T BuildTreeHierarchy<T>(this IList<T> nodes, Guid root, bool throwException = false) where T : ITreeNode<T>
        {
            try
            {
                nodes.CheckNullObject("nodes");

                var topNode = nodes.FindAndRemove(root, (item, key) =>
                {
                    return item.Key == key;
                });

                if (topNode != null)
                {
                    var groups = nodes.GroupBy(x => x.ParentNodeKey);
                    var nodeDictionary = groups.Where(g => g.Key.HasValue).ToDictionary(g => g.Key.Value, g => g.ToList());

                    HashSet<Guid> handledKeys = new HashSet<Guid>();
                    AddChildren(topNode, nodeDictionary, handledKeys, throwException);

                    return topNode;
                }

                return default(T);
            }
            catch (Exception ex)
            {
                throw ex.Handle("BuildTreeHierarchy", new { nodes, root, type = typeof(T).FullName });
            }
        }

        /// <summary>
        /// Adds the children.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">The node.</param>
        /// <param name="source">The source.</param>
        /// <param name="handledKeys">The handled keys.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <exception cref="DataConflictException"></exception>
        private static void AddChildren<T>(T node, IDictionary<Guid, List<T>> source, HashSet<Guid> handledKeys, bool throwException = false) where T : ITreeNode<T>
        {
            if (handledKeys != null && node.Key.HasValue && source.ContainsKey(node.Key.Value))
            {
                if (handledKeys.Contains(node.Key.Value))
                {
                    if (throwException)
                    {
                        throw new DataConflictException(typeof(T).FullName, node.Key.ToString());
                    }

                    return;
                }

                node.Children = source[node.Key.Value];
                handledKeys.Add(node.Key.Value);

                for (int i = 0; i < node.Children.Count; i++)
                {
                    AddChildren(node.Children[i], source, handledKeys, throwException);
                }
            }
            else
            {
                node.Children = new List<T>();
            }
        }

        /// <summary>
        /// Descendant the specified root.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="root">The root.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public static IEnumerable<T> Descendant<T>(this T root) where T : ITreeNode<T>
        {
            var nodes = new Stack<T>(new[] { root });
            while (nodes.Any())
            {
                T node = nodes.Pop();
                yield return node;
                foreach (var n in node.Children)
                {
                    nodes.Push(n);
                }
            }
        }

        #endregion

        #region MatrixList

        /// <summary>
        /// To the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matrixList">The matrix list.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>List&lt;T&gt;.</returns>
        public static List<T> ToList<T>(this MatrixList<T> matrixList, Func<string, T, bool> filter = null)
        {
            List<T> result = new List<T>();

            if (matrixList != null)
            {
                foreach (var one in matrixList)
                {
                    foreach (var item in one.Value)
                    {
                        if (filter == null || filter(one.Key, item))
                        {
                            result.Add(item);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// To the matrix.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="keyFunc">The key function.</param>
        /// <param name="keyCaseSensitive">if set to <c>true</c> [key case sensitive].</param>
        /// <returns>MatrixList&lt;T&gt;.</returns>
        public static MatrixList<T> ToMatrix<T>(this List<T> list, Func<T, string> keyFunc, bool keyCaseSensitive = true)
        {
            var result = new MatrixList<T>(keyCaseSensitive);

            if (list != null)
            {
                foreach (var one in list)
                {
                    result.Add(keyFunc == null ? string.Empty : keyFunc(one), one);
                }
            }

            return result;
        }

        /// <summary>
        /// Ases the matrix list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="key">The key.</param>
        /// <param name="keyCaseSensitive">if set to <c>true</c> [key case sensitive].</param>
        /// <returns>MatrixList&lt;T&gt;.</returns>
        public static MatrixList<T> AsMatrixList<T>(this List<T> list, string key, bool keyCaseSensitive = true)
        {
            var result = new MatrixList<T>(keyCaseSensitive);
            result.Add(key.SafeToString(), list ?? new List<T>());

            return result;
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <param name="matrixList">The matrix list.</param>
        /// <returns>XElement.</returns>
        public static XElement ToXml(this MatrixList<Guid> matrixList)
        {
            return ToXml<Guid>(matrixList, (x) => { return x.ToString(); });
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <param name="matrixList">The matrix list.</param>
        /// <returns>XElement.</returns>
        public static XElement ToXml(this MatrixList<DateTime> matrixList)
        {
            return ToXml<DateTime>(matrixList, (x) => { return x.ToFullDateTimeTzString(); });
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matrixList">The matrix list.</param>
        /// <param name="converter">The converter.</param>
        /// <returns>XElement.</returns>
        public static XElement ToXml<T>(this MatrixList<T> matrixList, Func<T, string> converter)
        {
            if (matrixList == null)
            {
                return null;
            }

            var node = XmlConstants.node_Matrix.CreateXml();
            foreach (var one in matrixList)
            {
                foreach (var item in one.Value)
                {
                    var n = XmlConstants.node_Item.CreateXml();
                    n.SetAttributeValue(XmlConstants.attribute_Key, one.Key);
                    n.SetValue(converter == null ? item.ToString() : converter(item));
                }
            }

            return node;
        }

        /// <summary>
        /// XMLs to matrix unique identifier.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns>MatrixList&lt;Guid&gt;.</returns>
        public static MatrixList<Guid> XmlToMatrixGuid(this XElement xml)
        {
            return XmlToMatrix(xml, (x) => { return x.ToGuid().Value; });
        }

        /// <summary>
        /// XMLs to matrix date time.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns>MatrixList&lt;DateTime&gt;.</returns>
        public static MatrixList<DateTime> XmlToMatrixDateTime(this XElement xml)
        {
            return XmlToMatrix(xml, (x) => { return x.ObjectToDateTime().Value; });
        }

        /// <summary>
        /// XMLs to matrix.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml">The XML.</param>
        /// <param name="converter">The converter.</param>
        /// <returns>Beyova.MatrixList&lt;T&gt;.</returns>
        public static MatrixList<T> XmlToMatrix<T>(this XElement xml, Func<string, T> converter)
        {
            if (xml == null || xml.Name.LocalName != XmlConstants.node_Matrix)
            {
                return null;
            }

            var result = new MatrixList<T>();
            foreach (var one in xml.Elements(XmlConstants.node_Item))
            {
                var key = one.GetAttributeValue(XmlConstants.attribute_Key);
                var value = one.Value;
                var typeValue = converter(value);

                if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
                {
                    if (result.ContainsKey(key))
                    {
                        result[key].Add(typeValue);
                    }
                    else
                    {
                        result.Add(key, new List<T> { typeValue });
                    }
                }
            }

            return result;
        }

        #endregion
    }
}
