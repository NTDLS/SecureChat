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

        public FormHome()
        {
            InitializeComponent();

            _treeImages.ColorDepth = ColorDepth.Depth32Bit;
            _treeImages.Images.Add(ScOnlineState.Offline.ToString(), Imaging.LoadIconFromResources(Resources.Offline16));
            _treeImages.Images.Add(ScOnlineState.Online.ToString(), Imaging.LoadIconFromResources(Resources.Online16));
            _treeImages.Images.Add(ScOnlineState.Away.ToString(), Imaging.LoadIconFromResources(Resources.Away16));

            treeViewAcquaintances.ImageList = _treeImages;
            treeViewAcquaintances.NodeMouseDoubleClick += TreeViewAcquaintances_NodeMouseDoubleClick;

            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 10000;
            timer.Tick += Timer_Tick;
            timer.Enabled = true;

            Shown += FormHome_Shown;
            FormClosing += FormHome_FormClosing;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (SessionState.Instance != null)
            {
                //Instead of Repopulate(), do a delta merge of acquaintances.

                var idleTime = Interop.GetIdleTime();
                if (idleTime.TotalSeconds >= ScConstants.DefaultAutoAwayIdleSeconds)
                {
                    SessionState.Instance.Client.Notify(new UpdateAccountStatus(
                            SessionState.Instance.AccountId,
                            ScOnlineState.Away,
                            SessionState.Instance.Status
                        ));
                }
                else
                {
                    SessionState.Instance.Client.Notify(new UpdateAccountStatus(
                            SessionState.Instance.AccountId,
                            SessionState.Instance.ExplicitAway ? ScOnlineState.Away : ScOnlineState.Online,
                            SessionState.Instance.Status
                        ));
                }
            }
        }

        private void TreeViewAcquaintances_NodeMouseDoubleClick(object? sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node?.Tag is AcquaintancesModel acquaintancesModel)
            {
                //Start the key exchange process then popup the chat window.

                if (SessionState.Instance == null)
                {
                    throw new Exception("The local connection is not established.");
                }

                var activeChat = SessionState.Instance.GetActiveChatByAccountId(acquaintancesModel.Id);
                if (activeChat == null)
                {
                    var compoundNegotiator = new CompoundNegotiator();
                    var negotiationToken = compoundNegotiator.GenerateNegotiationToken((int)(Math.Ceiling(ScConstants.EndToEndKeySize / 128.0)));

                    //The first thing we do when we get a connection is start a new key exchange process.
                    var queryRequestKeyExchangeReply = SessionState.Instance.Client.Query(
                        new InitiateEndToEndCryptography(SessionState.Instance.AccountId, acquaintancesModel.Id, SessionState.Instance.DisplayName, negotiationToken))
                        .ContinueWith(o =>
                        {
                            return o.Result;
                        }).Result;

                    //We received a reply to the secure key exchange, apply it.
                    compoundNegotiator.ApplyNegotiationResponseToken(queryRequestKeyExchangeReply.NegotiationToken);

                    //TODO: this is the NASCCL encryption key we will use for all user communication (but not control messages).
                    //Console.WriteLine($"SharedSecret: {Crypto.ComputeSha256Hash(compoundNegotiator.SharedSecret)}");

                    activeChat = SessionState.Instance.AddActiveChat(
                        queryRequestKeyExchangeReply.PeerConnectionId, acquaintancesModel.Id, acquaintancesModel.DisplayName, compoundNegotiator.SharedSecret);

                    activeChat.Form = new FormMessage(activeChat);
                    activeChat.Form.Show();
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
            if (SessionState.Instance != null)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void FormHome_Shown(object? sender, EventArgs e)
        {
            Repopulate();
        }

        private void Repopulate()
        {
            if (SessionState.Instance == null)
            {
                this.InvokeClose(DialogResult.Cancel);
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    SessionState.Instance.Client.Query(new GetAcquaintancesQuery()).ContinueWith(o =>
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

        private void PopulateTree(List<AcquaintancesModel> acquaintances)
        {
            if (SessionState.Instance == null)
            {
                this.InvokeClose(DialogResult.Cancel);
                return;
            }

            if (InvokeRequired)
            {
                Invoke(PopulateTree, acquaintances);
                return;
            }

            treeViewAcquaintances.Nodes.Clear();

            var rootName = SessionState.Instance.DisplayName;
            if (string.IsNullOrEmpty(SessionState.Instance.Status) == false)
            {
                rootName += $" - {SessionState.Instance.Status}";
            }
            var rootNode = new TreeNode(rootName);

            if (SessionState.Instance.ExplicitAway)
            {
                rootNode.ImageKey = ScOnlineState.Online.ToString();
                rootNode.SelectedImageKey = ScOnlineState.Online.ToString();
            }
            else
            {
                rootNode.ImageKey = ScOnlineState.Away.ToString();
                rootNode.SelectedImageKey = ScOnlineState.Away.ToString();
            }

            treeViewAcquaintances.Nodes.Add(rootNode);

            foreach (var acquaintance in acquaintances)
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

                rootNode.Nodes.Add(node);
            }

            rootNode.Expand();
        }
    }
}
