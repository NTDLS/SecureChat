using NTDLS.SecureKeyExchange;
using NTDLS.WinFormsHelpers;
using SecureChat.Client.Properties;
using SecureChat.Library;
using SecureChat.Library.Models;
using SecureChat.Library.ReliableMessages;
using Serilog;
using System.Diagnostics;
using static SecureChat.Library.ScConstants;

namespace SecureChat.Client.Forms
{
    public partial class FormHome : Form
    {
        private readonly ImageList _treeImages = new();
        private readonly ToolTip _treeToolTip = new();

        public FormHome()
        {
            //TODO: We need to handle server disconnection:
            //  LocalSession.Current.Client.IsConnected

            InitializeComponent();

            _treeImages.ColorDepth = ColorDepth.Depth32Bit;
            _treeImages.Images.Add(ScOnlineState.Offline.ToString(), Imaging.LoadIconFromResources(Resources.Offline16));
            _treeImages.Images.Add(ScOnlineState.Online.ToString(), Imaging.LoadIconFromResources(Resources.Online16));
            _treeImages.Images.Add(ScOnlineState.Away.ToString(), Imaging.LoadIconFromResources(Resources.Away16));
            _treeImages.Images.Add(ScOnlineState.Pending.ToString(), Imaging.LoadIconFromResources(Resources.Pending16));

            treeViewContacts.ImageList = _treeImages;
            treeViewContacts.NodeMouseDoubleClick += TreeViewContacts_NodeMouseDoubleClick;
            treeViewContacts.NodeMouseClick += TreeViewContacts_NodeMouseClick;

            treeViewContacts.NodeMouseHover += TreeViewContacts_NodeMouseHover;

            // Set up the delays for the ToolTip.
            _treeToolTip.InitialDelay = 500; // Time in milliseconds before the tooltip appears
            _treeToolTip.ReshowDelay = 500;   // Time in milliseconds before subsequent tooltips appear
            _treeToolTip.AutoPopDelay = 2500; // Time in milliseconds the tooltip remains visible
            _treeToolTip.ShowAlways = false;   // Display the tooltip even when the form is not active

            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 10000;
            timer.Tick += Timer_Tick;
            timer.Enabled = true;

            FormClosing += FormHome_FormClosing;
            Load += FormHome_Load;

            GetRootNode();
        }

        private void RemoveContact(ContactModel contact)
        {
            if (contact.IsAccepted)
            {
                if (MessageBox.Show("Are you sure you want to remove this contact?",
                    ScConstants.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
            }
            else if (contact.IsAccepted == false)
            {
                if (MessageBox.Show("Are you sure you want to remove this contact request?",
                    ScConstants.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
            }

            Task.Run(() =>
            {
                if (LocalSession.Current != null)
                {
                    try
                    {
                        LocalSession.Current.Client.Query(new RemoveContactQuery(contact.Id)).ContinueWith(o =>
                        {
                            if (string.IsNullOrEmpty(o.Result.ErrorMessage) == false)
                            {
                                throw new Exception(o.Result.ErrorMessage);
                            }

                            if (!o.IsFaulted && o.Result.IsSuccess)
                            {
                                Repopulate();
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        this.InvokeMessageBox(ex.GetBaseException().Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            });
        }

        private void AcceptInvite(ContactModel contact)
        {
            if (MessageBox.Show("Are you sure you want to accept this contact request?",
                ScConstants.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            Task.Run(() =>
            {
                if (LocalSession.Current != null)
                {
                    try
                    {
                        LocalSession.Current.Client.Query(new AcceptContactInvite(contact.Id)).ContinueWith(o =>
                        {
                            if (string.IsNullOrEmpty(o.Result.ErrorMessage) == false)
                            {
                                throw new Exception(o.Result.ErrorMessage);
                            }

                            if (!o.IsFaulted && o.Result.IsSuccess)
                            {
                                Repopulate();
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        this.InvokeMessageBox(ex.GetBaseException().Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            });
        }


        public TreeNode? GetRootNode()
        {
            if (LocalSession.Current != null)
            {
                if (treeViewContacts.Nodes.Count > 0)
                {
                    TreeNode existingRootNode = treeViewContacts.Nodes[0];

                    if (LocalSession.Current.ExplicitAway)
                    {
                        existingRootNode.ImageKey = ScOnlineState.Away.ToString();
                        existingRootNode.SelectedImageKey = ScOnlineState.Away.ToString();
                    }
                    else
                    {
                        existingRootNode.ImageKey = LocalSession.Current.ConnectionState.ToString();
                        existingRootNode.SelectedImageKey = LocalSession.Current.ConnectionState.ToString();
                    }

                    return existingRootNode;
                }

                var rootName = LocalSession.Current.DisplayName;
                if (string.IsNullOrEmpty(LocalSession.Current.Status) == false)
                {
                    rootName += $" - {LocalSession.Current.Status}";
                }
                var rootNode = new TreeNode(rootName);

                if (LocalSession.Current.ExplicitAway)
                {
                    rootNode.ImageKey = ScOnlineState.Away.ToString();
                    rootNode.SelectedImageKey = ScOnlineState.Away.ToString();
                }
                else
                {
                    rootNode.ImageKey = LocalSession.Current.ConnectionState.ToString();
                    rootNode.SelectedImageKey = LocalSession.Current.ConnectionState.ToString();
                }

                treeViewContacts.Nodes.Add(rootNode);

                return rootNode;
            }
            return null;
        }

        private void FormHome_Load(object? sender, EventArgs e)
        {
            Repopulate();

            var currentScreen = Screen.FromPoint(Cursor.Position);
            int offsetY = 10; // Distance above the taskbar
            int offsetX = 10; // Distance from the right of the screen.
            int x = currentScreen.WorkingArea.Right - this.Width - offsetX;
            int y = currentScreen.WorkingArea.Bottom - this.Height - offsetY;
            Location = new Point(x, y);
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (LocalSession.Current != null && LocalSession.Current.Client.IsConnected)
            {
                Repopulate();

                var idleTime = Interop.GetIdleTime();
                if (idleTime.TotalSeconds >= Settings.Instance.AutoAwayIdleSeconds)
                {
                    LocalSession.Current.Client.Notify(new UpdateAccountStatus(
                            LocalSession.Current.AccountId,
                            ScOnlineState.Away,
                            LocalSession.Current.Status
                        ));
                }
                else
                {
                    LocalSession.Current.Client.Notify(new UpdateAccountStatus(
                            LocalSession.Current.AccountId,
                            LocalSession.Current.ExplicitAway ? ScOnlineState.Away : ScOnlineState.Online,
                            LocalSession.Current.Status
                        ));
                }
            }
        }

        private void TreeViewContacts_NodeMouseClick(object? sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.Node?.Tag is ContactModel contact)
            {
                if (contact.IsAccepted == false && contact.RequestedByMe == true)
                {
                    var contextMenu = new ContextMenuStrip();

                    contextMenu.Items.Add(new ToolStripMenuItem("Remove", null, (s, ev) => RemoveContact(contact)));

                    contextMenu.Show(treeViewContacts, e.Location);
                }
                else if (contact.IsAccepted == false && contact.RequestedByMe == false)
                {
                    var contextMenu = new ContextMenuStrip();

                    contextMenu.Items.Add(new ToolStripMenuItem("Accept", null, (s, ev) => AcceptInvite(contact)));
                    contextMenu.Items.Add(new ToolStripMenuItem("Remove", null, (s, ev) => RemoveContact(contact)));

                    contextMenu.Show(treeViewContacts, e.Location);
                }
                else if (contact.IsAccepted == true)
                {
                    var contextMenu = new ContextMenuStrip();

                    contextMenu.Items.Add(new ToolStripMenuItem("Remove", null, (s, ev) => RemoveContact(contact)));

                    contextMenu.Show(treeViewContacts, e.Location);
                }
            }
        }

        private void TreeViewContacts_NodeMouseHover(object? sender, TreeNodeMouseHoverEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Node?.ToolTipText))
            {
                _treeToolTip.SetToolTip(treeViewContacts, e.Node.ToolTipText);
            }
        }

        private void TreeViewContacts_NodeMouseDoubleClick(object? sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            if (e.Node?.Tag is ContactModel contactsModel)
            {
                if (contactsModel.IsAccepted == false)
                {
                    this.InvokeMessageBox("You cannot interact with this contact until they have accepted your request.", ScConstants.AppName, MessageBoxButtons.OK);
                    return;
                }

                if (e.Node.ImageKey.Equals(ScOnlineState.Offline.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    this.InvokeMessageBox("The selected contact is not online.", ScConstants.AppName, MessageBoxButtons.OK);
                    return;
                }

                //Start the key exchange process then popup the chat window.
                if (LocalSession.Current == null)
                {
                    this.InvokeMessageBox("Local connection is not established.", ScConstants.AppName, MessageBoxButtons.OK);
                    this.InvokeClose(DialogResult.No);
                    return;
                }

                var activeChat = LocalSession.Current.GetActiveChatByAccountId(contactsModel.Id);
                if (activeChat == null)
                {
                    var compoundNegotiator = new CompoundNegotiator();
                    var negotiationToken = compoundNegotiator.GenerateNegotiationToken((int)(Math.Ceiling(ScConstants.EndToEndKeySize / 128.0)));

                    //The first thing we do when we get a connection is start a new key exchange process.
                    var queryRequestKeyExchangeReply = LocalSession.Current.Client.Query(
                        new InitiateEndToEndCryptography(LocalSession.Current.AccountId, contactsModel.Id, LocalSession.Current.DisplayName, negotiationToken))
                        .ContinueWith(o =>
                        {
                            if (!o.IsFaulted && o.Result.IsSuccess)
                            {
                                return o.Result;
                            }
                            return null;
                        }).Result;

                    if (queryRequestKeyExchangeReply != null)
                    {
                        //We received a reply to the secure key exchange, apply it.
                        compoundNegotiator.ApplyNegotiationResponseToken(queryRequestKeyExchangeReply.NegotiationToken);

                        //TODO: this is the NASCCL encryption key we will use for all user communication (but not control messages).
                        //Console.WriteLine($"SharedSecret: {Crypto.ComputeSha256Hash(compoundNegotiator.SharedSecret)}");

                        activeChat = LocalSession.Current.AddActiveChat(
                            queryRequestKeyExchangeReply.PeerConnectionId, contactsModel.Id, contactsModel.DisplayName, compoundNegotiator.SharedSecret);

                        activeChat.Form = new FormMessage(activeChat);
                        activeChat.Form.Show();
                    }
                    else
                    {
                        this.InvokeMessageBox("Could not connect to the selected contact.", ScConstants.AppName, MessageBoxButtons.OK);
                    }
                }
                else if (activeChat.Form != null)
                {
                    activeChat.Form.Show();
                    activeChat.Form.BringToFront();
                    activeChat.Form.Activate();
                    activeChat.Form.Focus();
                }
            }
        }

        private void FormHome_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (LocalSession.Current != null)
            {
                e.Cancel = true;
                Hide();
            }
        }

        public void Repopulate()
        {
            if (LocalSession.Current == null)
            {
                this.InvokeMessageBox("Local connection is not established.", ScConstants.AppName, MessageBoxButtons.OK);
                this.InvokeClose(DialogResult.No);
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    LocalSession.Current.Client.Query(new GetContactsQuery()).ContinueWith(o =>
                    {
                        if (string.IsNullOrEmpty(o.Result.ErrorMessage) == false)
                        {
                            throw new Exception(o.Result.ErrorMessage);
                        }

                        if (!o.IsFaulted && o.Result.IsSuccess)
                        {
                            DeltaRepopulateTree(o.Result.Contacts);
                        }

                        return !o.IsFaulted && o.Result.IsSuccess;
                    });
                }
                catch (Exception ex)
                {
                    this.InvokeMessageBox(ex.GetBaseException().Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            });
        }

        private void DeltaRepopulateTree(List<ContactModel> contacts)
        {
            if (LocalSession.Current == null)
            {
                this.InvokeMessageBox("Local connection is not established.", ScConstants.AppName, MessageBoxButtons.OK);
                this.InvokeClose(DialogResult.No);
                return;
            }

            if (InvokeRequired)
            {
                Invoke(DeltaRepopulateTree, contacts);
                return;
            }

            var rootNode = GetRootNode();
            if (rootNode != null)
            {
                var nodesToRemove = new List<TreeNode>();

                #region Accepted contacts.

                foreach (TreeNode node in rootNode.Nodes)
                {
                    if (node?.Tag is ContactModel contactsModel)
                    {
                        var matchingContact = contacts.FirstOrDefault(o => o.IsAccepted == true && o.Id == contactsModel.Id);
                        if (matchingContact != null)
                        {
                            //Update tree node if it is in the fresh contact list.
                            node.ImageKey = matchingContact.State.ToString();
                            node.SelectedImageKey = matchingContact.State.ToString();
                            node.ToolTipText = matchingContact.State.ToString();
                        }
                        else
                        {
                            //Queue node for removal if it is missing from the fresh contact list.
                            nodesToRemove.Add(node);
                        }
                    }
                }

                //Remove nodes queued for deletion.
                foreach (var node in nodesToRemove)
                {
                    rootNode.Nodes.Remove(node);
                }
                nodesToRemove.Clear();

                //Add tree nodes for contacts that are in the fresh list but missing from the tree.
                foreach (var contact in contacts.Where(o => o.IsAccepted == true))
                {
                    var existingNode = TreeViewHelpers.FindNodeByAccountId(rootNode, contact.Id);
                    if (existingNode == null)
                    {
                        TreeViewHelpers.AddContactNode(rootNode, contact);
                    }
                }

                TreeViewHelpers.SortChildNodes(rootNode);
                rootNode.Expand();

                #endregion

                #region Outgoing contact invites.

                var requestedRootNode = TreeViewHelpers.FindNonContactNodeByText(treeViewContacts, "Requested");

                if (contacts.Any(o => o.IsAccepted == false && o.RequestedByMe == true))
                {
                    if (requestedRootNode == null)
                    {
                        requestedRootNode = new TreeNode("Requested");
                        requestedRootNode.ImageKey = ScOnlineState.Pending.ToString();
                        requestedRootNode.SelectedImageKey = ScOnlineState.Pending.ToString();
                        treeViewContacts.Nodes.Add(requestedRootNode);
                    }

                    foreach (TreeNode node in requestedRootNode.Nodes)
                    {
                        if (node?.Tag is ContactModel contactsModel)
                        {
                            var matchingContact = contacts.FirstOrDefault(o => o.IsAccepted == false && o.Id == contactsModel.Id);
                            if (matchingContact == null)
                            {
                                //Queue node for removal if it is missing from the fresh contact list.
                                nodesToRemove.Add(node);
                            }
                        }
                    }

                    //Remove nodes queued for deletion.
                    foreach (var node in nodesToRemove)
                    {
                        requestedRootNode.Nodes.Remove(node);
                    }
                    nodesToRemove.Clear();

                    foreach (var contact in contacts.Where(o => o.IsAccepted == false))
                    {
                        var existingNode = TreeViewHelpers.FindNodeByAccountId(requestedRootNode, contact.Id);
                        if (existingNode == null)
                        {
                            TreeViewHelpers.AddContactNode(requestedRootNode, contact);
                        }
                    }

                    TreeViewHelpers.SortChildNodes(requestedRootNode);
                    requestedRootNode.Expand();
                }
                else if (requestedRootNode != null)
                {
                    treeViewContacts.Nodes.Remove(requestedRootNode);
                }

                #endregion

                #region Incoming contact invites.

                var invitesRootNode = TreeViewHelpers.FindNonContactNodeByText(treeViewContacts, "Invites");

                if (contacts.Any(o => o.IsAccepted == false && o.RequestedByMe == false))
                {
                    if (invitesRootNode == null)
                    {
                        invitesRootNode = new TreeNode("Requested");
                        invitesRootNode.ImageKey = ScOnlineState.Pending.ToString();
                        invitesRootNode.SelectedImageKey = ScOnlineState.Pending.ToString();
                        treeViewContacts.Nodes.Add(invitesRootNode);
                    }

                    foreach (TreeNode node in invitesRootNode.Nodes)
                    {
                        if (node?.Tag is ContactModel contactsModel)
                        {
                            var matchingContact = contacts.FirstOrDefault(o => o.IsAccepted == false && o.Id == contactsModel.Id);
                            if (matchingContact == null)
                            {
                                //Queue node for removal if it is missing from the fresh contact list.
                                nodesToRemove.Add(node);
                            }
                        }
                    }

                    //Remove nodes queued for deletion.
                    foreach (var node in nodesToRemove)
                    {
                        invitesRootNode.Nodes.Remove(node);
                    }
                    nodesToRemove.Clear();

                    foreach (var contact in contacts.Where(o => o.IsAccepted == false))
                    {
                        var existingNode = TreeViewHelpers.FindNodeByAccountId(invitesRootNode, contact.Id);
                        if (existingNode == null)
                        {
                            TreeViewHelpers.AddContactNode(invitesRootNode, contact);
                        }
                    }

                    TreeViewHelpers.SortChildNodes(invitesRootNode);
                    invitesRootNode.Expand();
                }
                else if (invitesRootNode != null)
                {
                    treeViewContacts.Nodes.Remove(invitesRootNode);
                }

                #endregion
            }
        }

        #region Toolbar clicks.

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using var formAboutOnExit = new FormAbout(true);
                formAboutOnExit.ShowDialog();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var formAccountSearch = new FormAccountSearch();
                formAccountSearch.ShowDialog();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();

        }

        private void LogoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        #endregion
    }
}
