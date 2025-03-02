using NTDLS.SecureKeyExchange;
using NTDLS.WinFormsHelpers;
using SecureChat.Client.Properties;
using SecureChat.Library;
using SecureChat.Library.ReliableMessages;
using SecureChat.Server.Models;
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

            treeViewAcquaintances.ImageList = _treeImages;
            treeViewAcquaintances.NodeMouseDoubleClick += TreeViewAcquaintances_NodeMouseDoubleClick;

            treeViewAcquaintances.NodeMouseHover += TreeViewAcquaintances_NodeMouseHover;

            // Set up the delays for the ToolTip.
            _treeToolTip.InitialDelay = 500; // Time in milliseconds before the tooltip appears
            _treeToolTip.ReshowDelay = 500;   // Time in milliseconds before subsequent tooltips appear
            _treeToolTip.AutoPopDelay = 2500; // Time in milliseconds the tooltip remains visible
            _treeToolTip.ShowAlways = false;   // Display the tooltip even when the form is not active

            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 10000;
            timer.Tick += Timer_Tick;
            timer.Enabled = true;

            Shown += FormHome_Shown;
            FormClosing += FormHome_FormClosing;
            Load += FormHome_Load;

            GetRootNode();
        }

        public TreeNode? GetRootNode()
        {
            if (LocalSession.Current != null)
            {
                if (treeViewAcquaintances.Nodes.Count > 0)
                {
                    TreeNode existingRootNode = treeViewAcquaintances.Nodes[0];

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

                treeViewAcquaintances.Nodes.Add(rootNode);

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

        private void TreeViewAcquaintances_NodeMouseHover(object? sender, TreeNodeMouseHoverEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Node?.ToolTipText))
            {
                _treeToolTip.SetToolTip(treeViewAcquaintances, e.Node.ToolTipText);
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (LocalSession.Current != null && LocalSession.Current.Client.IsConnected)
            {
                DeltaRepopulate();

                var idleTime = Interop.GetIdleTime();
                if (idleTime.TotalSeconds >= ScConstants.DefaultAutoAwayIdleSeconds)
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

        private void TreeViewAcquaintances_NodeMouseDoubleClick(object? sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node?.Tag is AcquaintanceModel acquaintancesModel)
            {
                if (e.Node.ImageKey.Equals(ScOnlineState.Offline.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    this.InvokeMessageBox("The selected acquaintance is not online.", ScConstants.AppName, MessageBoxButtons.OK);
                    return;
                }

                //Start the key exchange process then popup the chat window.
                if (LocalSession.Current == null)
                {
                    this.InvokeMessageBox("Local connection is not established.", ScConstants.AppName, MessageBoxButtons.OK);
                    this.InvokeClose(DialogResult.No);
                    return;
                }

                var activeChat = LocalSession.Current.GetActiveChatByAccountId(acquaintancesModel.Id);
                if (activeChat == null)
                {
                    var compoundNegotiator = new CompoundNegotiator();
                    var negotiationToken = compoundNegotiator.GenerateNegotiationToken((int)(Math.Ceiling(ScConstants.EndToEndKeySize / 128.0)));

                    //The first thing we do when we get a connection is start a new key exchange process.
                    var queryRequestKeyExchangeReply = LocalSession.Current.Client.Query(
                        new InitiateEndToEndCryptography(LocalSession.Current.AccountId, acquaintancesModel.Id, LocalSession.Current.DisplayName, negotiationToken))
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
                            queryRequestKeyExchangeReply.PeerConnectionId, acquaintancesModel.Id, acquaintancesModel.DisplayName, compoundNegotiator.SharedSecret);

                        activeChat.Form = new FormMessage(activeChat);
                        activeChat.Form.Show();
                    }
                    else
                    {
                        this.InvokeMessageBox("Could not connect to the selected acquaintance.", ScConstants.AppName, MessageBoxButtons.OK);
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

        private void FormHome_Shown(object? sender, EventArgs e)
        {
        }

        private void Repopulate()
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
                    LocalSession.Current.Client.Query(new GetAcquaintancesQuery()).ContinueWith(o =>
                    {
                        if (string.IsNullOrEmpty(o.Result.ErrorMessage) == false)
                        {
                            throw new Exception(o.Result.ErrorMessage);
                        }

                        if (o.Result.IsSuccess)
                        {
                            PopulateTree(o.Result.Acquaintances);
                        }

                        return o.Result.IsSuccess;
                    });
                }
                catch (Exception ex)
                {
                    this.InvokeMessageBox(ex.GetBaseException().Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            });
        }

        private void PopulateTree(List<AcquaintanceModel> acquaintances)
        {
            if (LocalSession.Current == null)
            {
                this.InvokeMessageBox("Local connection is not established.", ScConstants.AppName, MessageBoxButtons.OK);
                this.InvokeClose(DialogResult.No);
                return;
            }

            if (InvokeRequired)
            {
                Invoke(PopulateTree, acquaintances);
                return;
            }

            treeViewAcquaintances.Nodes.Clear();

            var rootNode = GetRootNode();
            if (rootNode != null)
            {
                foreach (var acquaintance in acquaintances)
                {
                    TreeViewHelpers.AddAcquaintanceNode(rootNode, acquaintance);
                }

                TreeViewHelpers.SortChildNodes(rootNode);
                rootNode.Expand();
            }
        }

        private void DeltaRepopulate()
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
                    LocalSession.Current.Client.Query(new GetAcquaintancesQuery()).ContinueWith(o =>
                    {
                        if (string.IsNullOrEmpty(o.Result.ErrorMessage) == false)
                        {
                            throw new Exception(o.Result.ErrorMessage);
                        }

                        if (o.Result.IsSuccess)
                        {
                            DeltaRepopulateTree(o.Result.Acquaintances);
                        }

                        return o.Result.IsSuccess;
                    });
                }
                catch (Exception ex)
                {
                    this.InvokeMessageBox(ex.GetBaseException().Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            });
        }

        private void DeltaRepopulateTree(List<AcquaintanceModel> acquaintances)
        {
            if (LocalSession.Current == null)
            {
                this.InvokeMessageBox("Local connection is not established.", ScConstants.AppName, MessageBoxButtons.OK);
                this.InvokeClose(DialogResult.No);
                return;
            }

            if (InvokeRequired)
            {
                Invoke(DeltaRepopulateTree, acquaintances);
                return;
            }

            if (treeViewAcquaintances.Nodes.Count == 0)
            {
                return;
            }

            var rootNode = GetRootNode();
            if (rootNode != null)
            {
                var nodesToRemove = new List<TreeNode>();

                foreach (TreeNode node in rootNode.Nodes)
                {
                    if (node?.Tag is AcquaintanceModel acquaintancesModel)
                    {
                        var matchingAcquaintance = acquaintances.FirstOrDefault(o => o.Id == acquaintancesModel.Id);
                        if (matchingAcquaintance != null)
                        {
                            var state = TreeViewHelpers.GetAcquaintanceState(matchingAcquaintance);

                            //Update tree node if it is in the fresh acquaintance list.
                            node.ImageKey = state.ToString();
                            node.SelectedImageKey = state.ToString();
                            node.ToolTipText = state.ToString();
                        }
                        else
                        {
                            //Queue node for removal if it is missing from the fresh acquaintance list.
                            nodesToRemove.Add(node);
                        }
                    }
                }

                //Remove nodes queued for deletion.
                foreach (var node in nodesToRemove)
                {
                    rootNode.Nodes.Remove(node);
                }

                //Add tree nodes for acquaintances that are in the fresh list but missing from the tree.
                foreach (var acquaintance in acquaintances)
                {
                    var existingNode = TreeViewHelpers.FindNodeByAccountId(rootNode, acquaintance.Id);
                    if (existingNode == null)
                    {
                        TreeViewHelpers.AddAcquaintanceNode(rootNode, acquaintance);
                    }
                }

                TreeViewHelpers.SortChildNodes(rootNode);
                rootNode.Expand();
            }
        }
    }
}
