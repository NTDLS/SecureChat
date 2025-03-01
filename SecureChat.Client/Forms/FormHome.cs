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

            Shown += FormHome_Shown;
            FormClosing += FormHome_FormClosing;
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

            var rootNode = new TreeNode(SessionState.Instance.DisplayName);

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
                var node = new TreeNode(acquaintance.DisplayName)
                {
                    ImageKey = acquaintance.Status,
                    SelectedImageKey = acquaintance.Status
                };

                rootNode.Nodes.Add(node);
            }

            rootNode.Expand();
        }
    }
}
