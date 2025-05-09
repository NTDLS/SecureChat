using Microsoft.Win32;
using SecureChat.Client.Controls;

namespace SecureChat.Client
{
    public static class Themes
    {
        /// <summary>
        /// Color used for display name when message are from the local client.
        /// </summary>
        public static Color FromMeColor = Color.Blue;

        /// <summary>
        /// Color used for display name when message are from remote clients.
        /// </summary>
        public static Color FromRemoteColor = Color.DarkRed;

        public static bool IsWindowsDarkMode()
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
                if (key != null)
                {
                    var value = key.GetValue("AppsUseLightTheme");
                    if (value is int intValue)
                    {
                        return intValue == 0; // 0 means Dark Mode
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }


        public static void ApplyDarkTheme(Control parent)
        {
            if (Settings.Instance.Theme == Library.ScConstants.Theme.Light)
            {
                return; // No need to apply dark theme
            }

            FromMeColor = Color.LightBlue;
            FromRemoteColor = Color.OrangeRed;

            // Base case: Apply to the current control
            parent.BackColor = Color.FromArgb(30, 30, 30);
            parent.ForeColor = Color.White;

            // Special handling for specific control types
            switch (parent)
            {
                case LinkLabel linkLabel:
                    // Background and text color
                    linkLabel.BackColor = Color.FromArgb(30, 30, 30);
                    linkLabel.ForeColor = Color.White;

                    // Link colors
                    linkLabel.LinkColor = Color.Cyan;                 // Normal link color
                    linkLabel.VisitedLinkColor = Color.MediumPurple;  // Visited link color
                    linkLabel.ActiveLinkColor = Color.LightGreen;     // Clicking link color
                    linkLabel.DisabledLinkColor = Color.Gray;         // Disabled state color

                    // Font settings for better readability
                    linkLabel.Font = new Font("Segoe UI", 9, FontStyle.Underline);

                    // Prevent the dotted focus rectangle (optional)
                    linkLabel.UseCompatibleTextRendering = true;
                    break;

                case TextBox textBox:
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.BackColor = Color.FromArgb(45, 45, 45);
                    textBox.ForeColor = Color.White;
                    break;

                case Button button:
                    button.FlatStyle = FlatStyle.Flat;
                    button.BackColor = Color.FromArgb(50, 50, 50);
                    button.ForeColor = Color.White;
                    button.FlatAppearance.BorderColor = Color.Gray;
                    break;

                case MenuStrip menuStrip:
                    menuStrip.RenderMode = ToolStripRenderMode.Professional;
                    menuStrip.BackColor = Color.FromArgb(30, 30, 30);
                    menuStrip.ForeColor = Color.White;
                    menuStrip.Renderer = new DarkMenuRenderer();
                    foreach (ToolStripMenuItem item in menuStrip.Items)
                    {
                        ApplyDarkThemeToMenuItem(item);
                    }
                    break;

                case TabControl tabControl:
                    tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
                    tabControl.BackColor = Color.FromArgb(30, 30, 30);
                    tabControl.ForeColor = Color.White;
                    tabControl.Padding = new Point(10, 4); // Padding for tab text
                    tabControl.DrawItem += (s, e) =>
                    {
                        var tc = s as TabControl;
                        if (tc != null)
                        {
                            TabPage tp = tc.TabPages[e.Index];
                            Rectangle tabRect = tc.GetTabRect(e.Index);

                            // Check if this tab is selected
                            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

                            // Background color based on selection state
                            Color backColor = isSelected ? Color.FromArgb(60, 60, 60) : Color.FromArgb(45, 45, 45);
                            Color textColor = isSelected ? Color.White : Color.LightGray;

                            using Brush backBrush = new SolidBrush(backColor);
                            using Brush textBrush = new SolidBrush(textColor);

                            // Draw the background of the tab
                            e.Graphics.FillRectangle(backBrush, tabRect);

                            if (e.Font != null)
                            {
                                // Draw the tab text
                                var sf = new StringFormat
                                {
                                    Alignment = StringAlignment.Center,
                                    LineAlignment = StringAlignment.Center
                                };
                                e.Graphics.DrawString(tp.Text, e.Font, textBrush, tabRect, sf);
                            }

                            // Draw a border around the tab for better visibility
                            using Pen borderPen = new Pen(Color.Gray);
                            e.Graphics.DrawRectangle(borderPen, tabRect);
                        }
                    };
                    foreach (TabPage tabPage in tabControl.TabPages)
                    {
                        ApplyDarkTheme(tabPage); // Apply to each tab page content
                    }
                    break;

                case ComboBox comboBox:
                    comboBox.BackColor = Color.FromArgb(45, 45, 45);
                    comboBox.ForeColor = Color.White;
                    comboBox.FlatStyle = FlatStyle.Flat;
                    break;

                case ListBox listBox:
                    listBox.BackColor = Color.FromArgb(45, 45, 45);
                    listBox.ForeColor = Color.White;
                    break;

                case DataGridView gridView:
                    ApplyDarkThemeToDataGridView(gridView);
                    break;

                case ListView listView:
                    ApplyDarkThemeToListView(listView);
                    break;
            }

            // Recursively apply the theme to all child controls
            foreach (Control child in parent.Controls)
            {
                ApplyDarkTheme(child);
            }
        }

        // Custom Renderer for Dark MenuStrip
        public class DarkMenuRenderer : ToolStripProfessionalRenderer
        {
            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                Color bgColor = e.Item.Selected ? Color.FromArgb(60, 60, 60) : Color.FromArgb(30, 30, 30);
                Color textColor = e.Item.Selected ? Color.White : Color.LightGray;

                using Brush b = new SolidBrush(bgColor);
                e.Graphics.FillRectangle(b, e.Item.ContentRectangle);

                // Draw text with improved visibility
                //TextRenderer.DrawText(e.Graphics, e.Item.Text, e.Item.Font, e.Item.ContentRectangle, textColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }

            protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
            {
                // Set the background color explicitly
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(45, 45, 45)), e.Item.Bounds);

                // Draw the separator line in the middle of the rectangle
                int y = e.Item.Bounds.Height / 2;

                using Pen pen = new Pen(Color.Gray);
                e.Graphics.DrawLine(pen, e.Item.Bounds.Left + 2, y, e.Item.Bounds.Right - 2, y);
            }

        }

        // Apply dark theme to each ToolStripMenuItem recursively
        private static void ApplyDarkThemeToMenuItem(ToolStripMenuItem menuItem)
        {
            menuItem.BackColor = Color.FromArgb(30, 30, 30);
            menuItem.ForeColor = Color.White;
            foreach (ToolStripItem subItem in menuItem.DropDownItems)
            {
                if (subItem is ToolStripMenuItem subMenuItem)
                {
                    ApplyDarkThemeToMenuItem(subMenuItem);
                }
            }
        }

        private static void ApplyDarkThemeToDataGridView(DataGridView dgv)
        {
            // General styling
            dgv.BackgroundColor = Color.FromArgb(30, 30, 30);
            dgv.ForeColor = Color.White;
            dgv.GridColor = Color.Gray;
            dgv.BorderStyle = BorderStyle.None;

            // Row and cell styling
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgv.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            // Column header styling
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(70, 70, 70);
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            // Row header styling
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
            dgv.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.RowHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(70, 70, 70);
            dgv.RowHeadersDefaultCellStyle.SelectionForeColor = Color.White;

            // Default cell styling
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(35, 35, 35);
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 60, 60);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9);

            // Alternating row styling for better readability
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 60, 60);
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;

            // Cell style for specific columns (optional)
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.DefaultCellStyle.BackColor = Color.FromArgb(35, 35, 35);
                col.DefaultCellStyle.ForeColor = Color.White;
                col.DefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 60, 60);
                col.DefaultCellStyle.SelectionForeColor = Color.White;
            }
        }

        private static int hoveredIndex = -1; // Track the hovered index for ListView

        private static void ApplyDarkThemeToListView(ListView listView)
        {
            // Enable double buffering for smoother rendering
            listView.DoubleBuffered(true);

            // General styling
            listView.BackColor = Color.FromArgb(30, 30, 30);
            listView.ForeColor = Color.White;
            listView.BorderStyle = BorderStyle.FixedSingle;

            // View mode and style
            listView.View = View.Details;
            listView.FullRowSelect = true;
            listView.OwnerDraw = true;

            // Track mouse movement to implement hover effect
            listView.MouseMove += (sender, e) =>
            {
                var hitTest = listView.HitTest(e.Location);
                int newHoveredIndex = hitTest.Item?.Index ?? -1;

                if (newHoveredIndex != hoveredIndex)
                {
                    hoveredIndex = newHoveredIndex;
                    listView.Invalidate(); // Redraw to update hover state
                }
            };

            listView.MouseLeave += (sender, e) =>
            {
                hoveredIndex = -1;
                listView.Invalidate(); // Clear hover when mouse leaves
            };

            // Handle DrawColumnHeader for customizing headers
            listView.DrawColumnHeader += (sender, e) =>
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(45, 45, 45)), e.Bounds);
                TextRenderer.DrawText(e.Graphics, e.Header?.Text ?? "", listView.Font, e.Bounds, Color.White);
                e.DrawDefault = false;
            };


            // Handle DrawItem and DrawSubItem for customizing rows
            listView.DrawItem += (sender, e) =>
            {
                // Background color based on selection state
                if ((e.State & ListViewItemStates.Selected) != 0)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(45, 45, 45)), e.Bounds);
                    e.Graphics.DrawRectangle(Pens.Gray, e.Bounds); // Subtle border
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(30, 30, 30)), e.Bounds);
                }
                e.DrawText();
            };

            listView.DrawSubItem += (sender, e) =>
            {
                if (e.ColumnIndex > 0)
                {
                    // Text color based on selection state
                    Color textColor = (e.ItemState & ListViewItemStates.Selected) != 0 ? Color.White : Color.LightGray;
                    e.Graphics.DrawString(e.SubItem?.Text ?? string.Empty, listView.Font, new SolidBrush(textColor), e.Bounds);
                }
            };
        }
    }
}
