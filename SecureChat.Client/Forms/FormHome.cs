using Krypton.Toolkit;
using NTDLS.Helpers;
using NTDLS.Persistence;
using NTDLS.SecureKeyExchange;
using NTDLS.WinFormsHelpers;
using SecureChat.Client.Helpers;
using SecureChat.Client.Models;
using SecureChat.Client.Properties;
using SecureChat.Library;
using SecureChat.Library.Models;
using SecureChat.Library.ReliableMessages;
using Serilog;
using System.Diagnostics;
using static SecureChat.Library.ScConstants;

namespace SecureChat.Client.Forms
{
    public partial class FormHome : KryptonForm
    {
        /// <summary>
        /// These are chat message forms per account ID. If they remain open, they will be recycled for subsequent chats.
        /// </summary>
        private readonly Dictionary<Guid, FormMessage> _accountMessageForms = new();

        private readonly ImageList _treeImages = new();
        private readonly ToolTip _treeToolTip = new();

        private FormBackground _backgroundForm = new();

        public FormHome()
        {
            InitializeComponent();

            _backgroundForm.Bounds = this.Bounds;
            this.Owner = _backgroundForm;

            BackColor = KryptonManager.CurrentGlobalPalette.GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);

            this.ResizeBegin += (object? sender, EventArgs e) =>
            {
                Exceptions.Ignore(() =>
                {
                    this.Opacity = 0.9;
                    _backgroundForm.Opacity = 0;
                });
            };

            this.ResizeEnd += (object? sender, EventArgs e) =>
            {
                Exceptions.Ignore(() =>
                {
                    this.Opacity = 0.70;
                    _backgroundForm.Opacity = 1;
                });
            };

            this.Activated += (object? sender, EventArgs e) =>
            {
                if (_backgroundForm.Visible == false)
                {
                    _backgroundForm.Visible = true;
                }

                Win32.SetWindowPos(_backgroundForm.Handle, this.Handle, 0, 0, 0, 0, Win32.SWP_NOMOVE | Win32.SWP_NOSIZE | Win32.SWP_NOACTIVATE);
            };

            this.Resize += (object? sender, EventArgs e) =>
            {
                _backgroundForm.Bounds = this.Bounds;
            };

            this.Move += (object? sender, EventArgs e) =>
            {
                _backgroundForm.Bounds = this.Bounds;
            };

            FormClosing += (object? sender, FormClosingEventArgs e) =>
            {
                try
                {
                    if (ServerConnection.Current != null)
                    {
                        e.Cancel = true;
                        _backgroundForm.Hide();
                        Hide();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                    MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };


            try
            {
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

                Load += FormHome_Load;

                GetRootNode();
            }
            catch (Exception ex)
            {
                Log.Fatal($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                throw;
            }
        }

        public void RemoveMessageForm(Guid accountId)
        {
            if (_accountMessageForms.TryGetValue(accountId, out var form))
            {
                _accountMessageForms.Remove(accountId);
            }
        }

        /// <summary>
        /// We have to create the Message Form on the main window thread.
        /// </summary>
        internal FormMessage CreateMessageForm(ActiveChat activeChat)
        {
            return Invoke(() =>
            {
                //Recycle the form if it already exists for this accountId.
                if (_accountMessageForms.TryGetValue(activeChat.AccountId, out var existingForm))
                {
                    existingForm.Recycle(activeChat);
                    return existingForm;
                }

                var form = new FormMessage(activeChat);
                form.CreateControl(); //Force the window handle to be created before the form is shown,
                var handle = form.Handle; // Accessing the Handle property forces handle creation
                _accountMessageForms.Add(activeChat.AccountId, form);
                return form;
            });
        }

        internal ActiveChat? EstablishEndToEndConnectionFor(Guid accountId, string displayName)
        {
            //Start the key exchange process then popup the chat window.
            if (ServerConnection.Current == null || !ServerConnection.Current.ReliableClient.IsConnected)
            {
                MessageBox.Show("Connection to the server was lost.", ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.InvokeClose(DialogResult.Cancel);
                return null;
            }

            ActiveChat? activeChat = null;

            var sessionId = Guid.NewGuid();

            var compoundNegotiator = new CompoundNegotiator();
            var negotiationToken = compoundNegotiator.GenerateNegotiationToken((int)(Math.Ceiling(Settings.Instance.EndToEndKeySize / 128.0)));

            //The first thing we do when we get a connection is start a new key exchange process.
            var queryRequestKeyExchangeReply = ServerConnection.Current.ReliableClient.Query(
                new InitiatePeerToPeerSessionQuery(sessionId, ServerConnection.Current.AccountId, accountId, ServerConnection.Current.DisplayName, negotiationToken))
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

                activeChat = ServerConnection.Current.AddActiveChat(sessionId,
                    queryRequestKeyExchangeReply.PeerConnectionId, accountId, displayName, compoundNegotiator.SharedSecret);

                activeChat.Form.Show();
            }
            else
            {
                throw new Exception("Failed to establish a connection with the contact.");
            }

            return activeChat;
        }

        private void RemoveContact(ContactModel contact)
        {
            try
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
                    if (ServerConnection.Current != null)
                    {
                        try
                        {
                            ServerConnection.Current.ReliableClient.Query(new RemoveContactQuery(contact.Id)).ContinueWith(o =>
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
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AcceptInvite(ContactModel contact)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to accept this contact request?",
                    ScConstants.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                Task.Run(() =>
                {
                    if (ServerConnection.Current != null)
                    {
                        try
                        {
                            ServerConnection.Current.ReliableClient.Query(new AcceptContactInviteQuery(contact.Id)).ContinueWith(o =>
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
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public TreeNode? GetRootNode()
        {
            try
            {
                if (ServerConnection.Current == null || !ServerConnection.Current.ReliableClient.IsConnected)
                {
                    return null;
                }

                var rootName = ServerConnection.Current.DisplayName;
                if (string.IsNullOrEmpty(ServerConnection.Current.Profile.Tagline) == false)
                {
                    rootName += $" - {ServerConnection.Current.Profile.Tagline}";
                }

                if (treeViewContacts.Nodes.Count > 0)
                {
                    TreeNode existingRootNode = treeViewContacts.Nodes[0];

                    existingRootNode.Text = rootName;

                    if (ServerConnection.Current.ExplicitAway)
                    {
                        existingRootNode.ImageKey = ScOnlineState.Away.ToString();
                        existingRootNode.SelectedImageKey = ScOnlineState.Away.ToString();
                        existingRootNode.ToolTipText = ScOnlineState.Away.ToString();
                    }
                    else
                    {
                        existingRootNode.ImageKey = ServerConnection.Current.State.ToString();
                        existingRootNode.SelectedImageKey = ServerConnection.Current.State.ToString();
                        existingRootNode.ToolTipText = ServerConnection.Current.State.ToString();
                    }

                    return existingRootNode;
                }

                var rootNode = new TreeNode(rootName);

                if (ServerConnection.Current.ExplicitAway)
                {
                    rootNode.ImageKey = ScOnlineState.Away.ToString();
                    rootNode.SelectedImageKey = ScOnlineState.Away.ToString();
                    rootNode.ToolTipText = ScOnlineState.Away.ToString();
                }
                else
                {
                    rootNode.ImageKey = ServerConnection.Current.State.ToString();
                    rootNode.SelectedImageKey = ServerConnection.Current.State.ToString();
                    rootNode.ToolTipText = ServerConnection.Current.State.ToString();
                }

                treeViewContacts.Nodes.Add(rootNode);

                return rootNode;
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;
        }

        private void FormHome_Load(object? sender, EventArgs e)
        {
            try
            {
                Repopulate();

                var currentScreen = Screen.FromPoint(Cursor.Position);
                int offsetY = 10; // Distance above the taskbar
                int offsetX = 10; // Distance from the right of the screen.
                int x = currentScreen.WorkingArea.Right - this.Width - offsetX;
                int y = currentScreen.WorkingArea.Bottom - this.Height - offsetY;
                Location = new Point(x, y);
            }
            catch (Exception ex)
            {
                Log.Fatal($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            try
            {
                if (ServerConnection.Current != null && ServerConnection.Current.ReliableClient.IsConnected)
                {
                    Repopulate();

                    var idleTime = IdleTime.GetIdleTime();
                    if (idleTime.TotalSeconds >= Settings.Instance.AutoAwayIdleSeconds)
                    {
                        ServerConnection.Current.ReliableClient.Notify(new UpdateAccountStateNotification(
                                ServerConnection.Current.AccountId,
                                ScOnlineState.Away));
                    }
                    else
                    {
                        ServerConnection.Current.ReliableClient.Notify(new UpdateAccountStateNotification(
                                ServerConnection.Current.AccountId,
                                ServerConnection.Current.ExplicitAway ? ScOnlineState.Away : ScOnlineState.Online));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        private void TreeViewContacts_NodeMouseClick(object? sender, TreeNodeMouseClickEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TreeViewContacts_NodeMouseHover(object? sender, TreeNodeMouseHoverEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(e.Node?.ToolTipText))
                {
                    _treeToolTip.SetToolTip(treeViewContacts, e.Node.ToolTipText);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TreeViewContacts_NodeMouseDoubleClick(object? sender, TreeNodeMouseClickEventArgs e)
        {
            try
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
                        //this.InvokeMessageBox("The selected contact is not online.", ScConstants.AppName, MessageBoxButtons.OK);
                        return;
                    }

                    //Start the key exchange process then popup the chat window.
                    if (ServerConnection.Current == null || !ServerConnection.Current.ReliableClient.IsConnected)
                    {
                        MessageBox.Show("Connection to the server was lost.", ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.InvokeClose(DialogResult.Cancel);
                        return;
                    }

                    var activeChat = ServerConnection.Current.GetActiveChatByAccountId(contactsModel.Id);
                    if (activeChat == null)
                    {
                        try
                        {
                            EstablishEndToEndConnectionFor(contactsModel.Id, contactsModel.DisplayName);
                        }
                        catch (Exception ex)
                        {
                            this.InvokeMessageBox(ex.GetBaseException().Message, ScConstants.AppName, MessageBoxButtons.OK);
                        }
                    }
                    else if (activeChat.Form != null)
                    {
                        activeChat.Form.Show();

                        if (ServerConnection.Current.FormHome.WindowState == FormWindowState.Minimized)
                        {
                            ServerConnection.Current.FormHome.WindowState = FormWindowState.Normal;
                        }

                        activeChat.Form.BringToFront();
                        activeChat.Form.Activate();
                        activeChat.Form.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private readonly Lock _repopulateLock = new();
        private bool _repopulateInProgress;

        public void Repopulate()
        {
            try
            {
                lock (_repopulateLock)
                {
                    if (_repopulateInProgress)
                    {
                        return;
                    }
                    _repopulateInProgress = true;
                }

                if (ServerConnection.Current == null || !ServerConnection.Current.ReliableClient.IsConnected)
                {
                    //MessageBox.Show("Connection to the server was lost.", ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.InvokeClose(DialogResult.Cancel);
                    ServerConnection.TerminateCurrent();
                    return;
                }

                Task.Run(() =>
                {
                    try
                    {
                        ServerConnection.Current.ReliableClient.Query(new GetContactsQuery()).ContinueWith(o =>
                        {
                            try
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
                            }
                            finally
                            {
                                _repopulateInProgress = false;
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        this.InvokeMessageBox(ex.GetBaseException().Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        _repopulateInProgress = false;
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        bool _isFirstPopulate = true;
        private void DeltaRepopulateTree(List<ContactModel> contacts)
        {
            try
            {
                if (ServerConnection.Current == null || !ServerConnection.Current.ReliableClient.IsConnected)
                {
                    MessageBox.Show("Connection to the server was lost.", ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.InvokeClose(DialogResult.Cancel);
                    return;
                }

                if (InvokeRequired)
                {
                    Invoke(DeltaRepopulateTree, contacts);
                    return;
                }

                bool wasSoundPlayed = false;

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
                                if (contactsModel.OnlineState == ScOnlineState.Offline && matchingContact.OnlineState == ScOnlineState.Online)
                                {
                                    if (!wasSoundPlayed && !_isFirstPopulate)
                                    {
                                        wasSoundPlayed = true;
                                        Notifications.ContactOnline(contactsModel.DisplayName);
                                    }
                                }
                                //Update tree node if it is in the fresh contact list.
                                ContactTree.UpdateContactNode(node, matchingContact);
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
                        var existingNode = ContactTree.FindNodeByAccountId(rootNode, contact.Id);
                        if (existingNode == null)
                        {
                            if (!wasSoundPlayed && !_isFirstPopulate)
                            {
                                wasSoundPlayed = true;
                                Notifications.ContactOnline(contact.DisplayName);
                            }

                            ContactTree.AddContactNode(rootNode, contact);
                        }
                    }

                    ContactTree.SortChildNodes(rootNode);
                    rootNode.Expand();

                    #endregion

                    #region Outgoing contact invites.

                    var requestedRootNode = ContactTree.FindNonContactNodeByText(treeViewContacts, "Requested");

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
                            var existingNode = ContactTree.FindNodeByAccountId(requestedRootNode, contact.Id);
                            if (existingNode == null)
                            {
                                ContactTree.AddContactNode(requestedRootNode, contact);
                            }
                        }

                        ContactTree.SortChildNodes(requestedRootNode);
                        requestedRootNode.Expand();
                    }
                    else if (requestedRootNode != null)
                    {
                        treeViewContacts.Nodes.Remove(requestedRootNode);
                    }

                    #endregion

                    #region Incoming contact invites.

                    var invitesRootNode = ContactTree.FindNonContactNodeByText(treeViewContacts, "Invites");

                    if (contacts.Any(o => o.IsAccepted == false && o.RequestedByMe == false))
                    {
                        if (invitesRootNode == null)
                        {
                            invitesRootNode = new TreeNode("Invites");
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
                            var existingNode = ContactTree.FindNodeByAccountId(invitesRootNode, contact.Id);
                            if (existingNode == null)
                            {
                                ContactTree.AddContactNode(invitesRootNode, contact);
                            }
                        }

                        ContactTree.SortChildNodes(invitesRootNode);
                        invitesRootNode.Expand();
                    }
                    else if (invitesRootNode != null)
                    {
                        treeViewContacts.Nodes.Remove(invitesRootNode);
                    }

                    #endregion

                    _isFirstPopulate = false;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        #region Toolbar clicks.

        private void ProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    using var formProfile = new FormProfile(false);
                    if (formProfile.ShowDialog() == DialogResult.OK)
                    {
                        Repopulate();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                    MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using var formAboutOnExit = new FormAbout(false);
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
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
            }
        }

        private void LogoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Task.Run(() => ServerConnection.Current?.ReliableClient?.Disconnect());
                Exceptions.Ignore(() => LocalUserApplicationData.DeleteFromDisk(ScConstants.AppName, typeof(AutoLogin)));
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using var formSettings = new FormSettings(false);
                formSettings.ShowDialog();
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
