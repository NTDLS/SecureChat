using SecureChat.Library;
using SecureChat.Server.Models;
using static SecureChat.Library.ScConstants;

namespace SecureChat.Client
{
    public static class TreeViewHelpers
    {
        public static void AddAcquaintanceNode(TreeNode parentNode, AcquaintanceModel acquaintance)
        {
            var childName = acquaintance.DisplayName;
            if (string.IsNullOrEmpty(acquaintance.Status) == false)
            {
                childName += $" - {acquaintance.Status}";
            }

            var state = GetAcquaintanceState(acquaintance);

            var node = new TreeNode(childName)
            {
                Tag = acquaintance,
                ImageKey = state.ToString(),
                SelectedImageKey = state.ToString(),
                ToolTipText = state.ToString()
            };

            parentNode.Nodes.Add(node);
        }

        public static ScOnlineState GetAcquaintanceState(AcquaintanceModel acquaintance)
        {
            var state = Enum.Parse<ScOnlineState>(acquaintance.State);

            if (acquaintance.LastSeen == null)
            {
                //The acquaintance has never been online.
                state = ScOnlineState.Offline;
            }
            else if (state == ScOnlineState.Online || state == ScOnlineState.Away)
            {
                //If the acquaintance is "online" or "Away" but was last seen a "long time ago" then show them as offline.
                if ((DateTime.UtcNow - acquaintance.LastSeen.Value).TotalSeconds > ScConstants.OfflineLastSeenSeconds)
                {
                    state = ScOnlineState.Offline;
                }
            }

            return state;
        }

        public static TreeNode? FindNodeByAccountId(TreeNode parentNode, Guid accountId)
        {
            foreach (TreeNode node in parentNode.Nodes)
            {
                if (node?.Tag is AcquaintanceModel acquaintancesModel && acquaintancesModel.Id == accountId)
                {
                    return node;
                }
            }
            return null;
        }

        public static void SortChildNodes(TreeNode parentNode)
        {
            var selectedNode = parentNode.TreeView?.SelectedNode;

            if (parentNode == null)
                throw new ArgumentNullException(nameof(parentNode));

            var childNodes = new List<TreeNode>();

            foreach (TreeNode node in parentNode.Nodes)
            {
                childNodes.Add(node);
            }

            childNodes.Sort(new NodeTextComparer());

            parentNode.Nodes.Clear();

            foreach (TreeNode node in childNodes)
            {
                parentNode.Nodes.Add(node);
            }

            if (parentNode.TreeView != null && selectedNode != null)
            {
                parentNode.TreeView.SelectedNode = selectedNode;
            }
        }

        public class NodeTextComparer : IComparer<TreeNode>
        {
            public int Compare(TreeNode? x, TreeNode? y)
            {
                if (x == null || y == null)
                    throw new ArgumentException("Both parameters should be of type TreeNode.");

                return string.Compare(x.Text, y.Text);
            }
        }
    }
}
