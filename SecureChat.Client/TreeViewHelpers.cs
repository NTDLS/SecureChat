using SecureChat.Server.Models;

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

            var node = new TreeNode(childName)
            {
                Tag = acquaintance,
                ImageKey = acquaintance.State,
                SelectedImageKey = acquaintance.State
            };

            parentNode.Nodes.Add(node);
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

            List<TreeNode> childNodes = new List<TreeNode>();

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
