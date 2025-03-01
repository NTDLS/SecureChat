using NTDLS.SecureKeyExchange;
using NTDLS.WinFormsHelpers;
using SecureChat.Client.Properties;
using SecureChat.Library;
using SecureChat.Library.ReliableMessages;
using SecureChat.Server.Models;

namespace SecureChat.Client.Forms
{
    public partial class FormHome : Form
    {
        private readonly ImageList _treeImages = new();

        public FormHome()
        {
            InitializeComponent();

            _treeImages.ColorDepth = ColorDepth.Depth32Bit;
            _treeImages.Images.Add(Constants.ScOnlineStatus.Offline.ToString(), Imaging.LoadIconFromResources(Resources.Offline16));
            _treeImages.Images.Add(Constants.ScOnlineStatus.Online.ToString(), Imaging.LoadIconFromResources(Resources.Online16));
            _treeImages.Images.Add(Constants.ScOnlineStatus.Away.ToString(), Imaging.LoadIconFromResources(Resources.Away16));

            treeViewAcquaintances.ImageList = _treeImages;
            treeViewAcquaintances.NodeMouseDoubleClick += TreeViewAcquaintances_NodeMouseDoubleClick;

            Shown += FormHome_Shown;
            FormClosing += FormHome_FormClosing;
        }

        private void TreeViewAcquaintances_NodeMouseDoubleClick(object? sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node?.Tag is AcquaintancesModel acquaintancesModel)
            {
                Negotiate(acquaintancesModel.Id);
            }
        }

        private void Negotiate(Guid accountId)
        {
            if (SessionState.Instance == null)
            {
                this.InvokeClose(DialogResult.Cancel);
                return;
            }

            var compoundNegotiator = new CompoundNegotiator();
            var negotiationToken = compoundNegotiator.GenerateNegotiationToken((int)(Math.Ceiling(Constants.EndToEndKeySize / 128.0)));

            //The first thing we do when we get a connection is start a new key exchange process.
            var queryRequestKeyExchangeReply = SessionState.Instance.Client.Query(
                new InitiateEndToEndCryptography(accountId, negotiationToken)).ContinueWith(o =>
                {
                    return o.Result;
                }).Result;

            //We received a reply to the secure key exchange, apply it.
            compoundNegotiator.ApplyNegotiationResponseToken(queryRequestKeyExchangeReply.NegotiationToken);

            //TODO: this is the NASCCL encryption key we will use for all user communication (but not control messages).
            Console.WriteLine($"SharedSecret: {Crypto.ComputeSha256Hash(compoundNegotiator.SharedSecret)}");
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
                    this.InvokeMessageBox(ex.GetBaseException().Message, Constants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
                rootNode.ImageKey = Constants.ScOnlineStatus.Online.ToString();
                rootNode.SelectedImageKey = Constants.ScOnlineStatus.Online.ToString();
            }
            else
            {
                rootNode.ImageKey = Constants.ScOnlineStatus.Away.ToString();
                rootNode.SelectedImageKey = Constants.ScOnlineStatus.Away.ToString();
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
