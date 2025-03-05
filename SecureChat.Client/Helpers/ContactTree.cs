using SecureChat.Library.Models;

namespace SecureChat.Client.Helpers
{
    public static class ContactTree
    {
        public static void AddContactNode(TreeNode parentNode, ContactModel contact)
        {
            var contactNodeText = contact.DisplayName;
            if (string.IsNullOrEmpty(contact.Profile.Tagline) == false)
            {
                contactNodeText += $" - {contact.Profile.Tagline}";
            }

            var node = new TreeNode(contactNodeText)
            {
                Tag = contact,
                ImageKey = contact.State.ToString(),
                SelectedImageKey = contact.State.ToString(),
                ToolTipText = contact.State.ToString()
            };

            parentNode.Nodes.Add(node);
        }

        public static void UpdateContactNode(TreeNode node, ContactModel contact)
        {
            var contactNodeText = contact.DisplayName;
            if (string.IsNullOrEmpty(contact.Profile.Tagline) == false)
            {
                contactNodeText += $" - {contact.Profile.Tagline}";
            }

            node.Text = contactNodeText;
            node.Tag = contact;
            node.ImageKey = contact.State.ToString();
            node.SelectedImageKey = contact.State.ToString();
            node.ToolTipText = contact.State.ToString();
        }

        public static TreeNode? FindNodeByAccountId(TreeNode parentNode, Guid accountId)
        {
            foreach (TreeNode node in parentNode.Nodes)
            {
                if (node.Tag is ContactModel contactsModel && contactsModel.Id == accountId)
                {
                    return node;
                }
            }
            return null;
        }

        public static TreeNode? FindNonContactNodeByText(TreeView treeView, string text)
        {
            foreach (TreeNode node in treeView.Nodes)
            {
                if (node.Tag is not ContactModel && node.Text == text)
                {
                    return node;
                }
            }
            return null;
        }

        public static TreeNode? FindNonContactNodeByText(TreeNode parentNode, string text)
        {
            foreach (TreeNode node in parentNode.Nodes)
            {
                if (node.Tag is not ContactModel && node.Text == text)
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
