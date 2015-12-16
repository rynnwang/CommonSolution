using System.Windows.Forms;

namespace Beyova
{
    /// <summary>
    /// Win form UI extensions.
    /// </summary>
    public static class WinFormUIExtension
    {
        #region TreeView

        /// <summary>
        /// Binds the tree node checkbox event.
        /// </summary>
        /// <param name="treeViewInstance">The tree view instance.</param>
        public static void BindTreeNodeCheckboxEvent(this TreeView treeViewInstance)
        {
            if (treeViewInstance != null)
            {
                treeViewInstance.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(TreeNodeAfterCheck);
            }
        }

        /// <summary>
        /// Handles the AfterCheck event of the tree view controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TreeViewEventArgs"/> instance containing the event data.</param>
        public static void TreeNodeAfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                bool isChecked = e.Node.Checked;
                TreeView treeInstance = sender as TreeView;
                if (treeInstance != null)
                {
                    treeInstance.BeginUpdate();

                    CheckAllSubItems(e.Node, isChecked);
                    CheckAllParentItems(e.Node, isChecked);

                    treeInstance.EndUpdate();
                }
            }
        }

        /// <summary>
        /// Checks all sub items.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="isChecked">if set to <c>true</c> [is checked].</param>
        public static void CheckAllSubItems(TreeNode node, bool isChecked)
        {
            if (node != null)
            {
                node.Checked = isChecked;
                if (node.Nodes != null && node.Nodes.Count > 0)
                {
                    foreach (TreeNode one in node.Nodes)
                    {
                        CheckAllSubItems(one, isChecked);
                    }
                }
            }
        }

        /// <summary>
        /// Checks all parent items.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="isChecked">if set to <c>true</c> [is checked].</param>
        public static void CheckAllParentItems(TreeNode node, bool isChecked)
        {
            if (node != null)
            {
                TreeNode parentNode = node.Parent;
                while (parentNode != null)
                {
                    if (isChecked)
                    {
                        if (IsAllSubItemsChecked(parentNode))
                        {
                            parentNode.Checked = true;
                        }
                    }
                    else
                    {
                        parentNode.Checked = false;
                    }

                    parentNode = parentNode.Parent;
                }
            }
        }

        /// <summary>
        /// Determines whether [is all sub items checked] [the specified node].
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>
        ///   <c>true</c> if [is all sub items checked] [the specified node]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAllSubItemsChecked(TreeNode node)
        {
            bool result = true; ;

            if (node != null)
            {
                if (node.Nodes != null && node.Nodes.Count > 0)
                {
                    foreach (TreeNode subNode in node.Nodes)
                    {
                        result = result & IsAllSubItemsChecked(subNode);
                        if (!result)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    result = node.Checked;
                }
            }

            return result;
        }

        #endregion
    }
}
