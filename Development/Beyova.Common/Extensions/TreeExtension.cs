using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Beyova.ExceptionSystem;

namespace Beyova
{
    /// <summary>
    /// Extension class for TreeNode
    /// </summary>
    public static class TreeExtension
    {
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
    }
}
