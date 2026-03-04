using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace ProductPortalApp
{
    public enum ProductCategory
    {
        Default,    //デフォルト
        FreeWay,    //FreeWay関連製品
        WebQuery,   //WebQuery関連製品
        Excellent,  //Excellent関連製品
        DataHarbor  //DataHarbor関連製品
    }

    public class Display_Design : UserControl
    {
        private PictureBox pictureBoxIcon = null!;
        private Label labelName = null!;
        private int cornerRadius = 8;
        private bool isHovered = false;
        private Color hoverStart = Color.FromArgb(255, 183, 77);
        private Color hoverEnd = Color.FromArgb(245, 222, 179);
        private ProductCategory category = ProductCategory.Default;
        private Label labelCategory = null!;
        private Image defaultIcon;


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? LaunchPath { get; set; }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image? IconImage
        {
            get => pictureBoxIcon.Image;
            set => pictureBoxIcon.Image = value;
        }
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DisplayName
        {
            get => labelName.Text;
            set => labelName.Text = value ?? string.Empty;
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ProductCategory Category
        {
            get => category;
            set
            {
                category = value;
                CategoryColors();
            }
        }

        public Display_Design()
        {
            InitializeComponents();
            //初期カテゴリ(Default)を適用
            CategoryColors();
        }

        private void InitializeComponents()
        {
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Size = new Size(250, 250);
            this.Cursor = Cursors.Hand;

            pictureBoxIcon = new PictureBox
            {
                Size = new Size(64, 64),
                Location = new Point(12, 12),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };

            //製品タイトル用
            labelName = new Label
            {
                Location = new Point(12, 84),
                AutoSize = false,
                Size = new Size(150, 40),
                Font = new Font("Yu Gothic UI", 10F, FontStyle.Regular),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.TopCenter
            };

            //カテゴリ表示用
            labelCategory = new Label
            {
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Yu Gothic UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = hoverStart,
                Padding = new Padding(4, 1, 4, 1),
                Text = GetCategoryText(category),
            };

            this.Controls.AddRange(new Control[] { pictureBoxIcon, labelName, labelCategory });
            labelCategory.BringToFront();

            labelCategory.SizeChanged += (s, e) =>
            {
                MakeRound(labelCategory, 1);
            };


            //画面サイズ変更時テキストの位置を調整
            this.Resize += (s, e) => AdjustLayout();
            labelName.TextChanged += (s, e) => AdjustLayout();

            //初期レイアウト位置
            AdjustLayout();

            //パネル内のどこをクリックしても同じ動作になるように設定
            this.Click += OnClickAll;
            foreach (Control c in this.Controls)
            {
                c.Click += OnClickAll;
                //ホバー状態でも同じ動作になるように設定
                c.MouseEnter += (s, e) => UpdateHoverState();
                c.MouseLeave += (s, e) => UpdateHoverState();
            }

            //ホバー状態を判定
            this.MouseEnter += (s, e) => UpdateHoverState();
            this.MouseLeave += (s, e) => UpdateHoverState();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            //リサイズ時、エリアを更新し再度角丸にする
            UpdateRegion();
            Invalidate();
        }

        //操作可能な領域を角丸に設定
        private void UpdateRegion()
        {
            var rect = new Rectangle(0, 0, this.Width, this.Height);
            using var path = GetRoundedRect(rect, cornerRadius);
            this.Region?.Dispose();
            this.Region = new Region(path);
        }

        private void CategoryColors()
        {
            //ホバー時の背景色を製品カテゴリに応じて変更
            switch (category)
            {
                case ProductCategory.FreeWay:
                    hoverStart = Color.FromArgb(255, 195, 95);
                    hoverEnd = Color.FromArgb(255, 230, 234);
                    this.BackColor = Color.FromArgb(250, 250, 250);
                    break;
                case ProductCategory.WebQuery:
                    hoverStart = Color.FromArgb(173, 207, 255);
                    hoverEnd = Color.FromArgb(132, 245, 255);
                    this.BackColor = Color.FromArgb(250, 250, 250);
                    break;
                case ProductCategory.Excellent:
                    hoverStart = Color.FromArgb(203, 243, 154);
                    hoverEnd = Color.FromArgb(224, 255, 132);
                    this.BackColor = Color.FromArgb(250, 250, 250);
                    break;
                case ProductCategory.DataHarbor:
                    hoverStart = Color.FromArgb(204, 153, 255);
                    hoverEnd = Color.FromArgb(220, 204, 255);
                    this.BackColor = Color.FromArgb(240, 240, 240);
                    break;
            }
            Invalidate();

            labelCategory.Text = GetCategoryText(category);
            labelCategory.BackColor = hoverStart;
            labelCategory.BringToFront();
        }

        private void SetHover(bool hover)
        {
            if (isHovered == hover) return;
            isHovered = hover;
            Invalidate();
        }

        private void UpdateHoverState()
        {
            //Determine if the mouse cursor is currently within this control (including children)
            var pt = this.PointToClient(Cursor.Position);
            bool inside = this.ClientRectangle.Contains(pt);
            SetHover(inside);
        }

        //パネル描画処理(ホバー時はグラデーション、通常時は白)
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var path = GetRoundedRect(new Rectangle(0, 0, this.Width, this.Height), cornerRadius);
            if (isHovered)
            {
                using var brush = new LinearGradientBrush(this.ClientRectangle, hoverStart, hoverEnd, 45f);
                e.Graphics.FillPath(brush, path);
            }
            else
            {
                using var brush = new SolidBrush(this.BackColor);
                e.Graphics.FillPath(brush, path);
            }
        }

        //カテゴリー名をテキストに変換(左上に表示用)
        private static string GetCategoryText(ProductCategory c) => c switch
        {
            ProductCategory.FreeWay => "FreeWay",
            ProductCategory.WebQuery => "WebQuery",
            ProductCategory.Excellent => "Excellent",
            ProductCategory.DataHarbor => "DataHarbor",
            _ => "Default",
        };

        private void MakeRound(Control c, int radius)
        {
            var rect = new Rectangle(0, 0, c.Width, c.Height);
            int d = radius * 2;

            using (var path = new GraphicsPath())
            {
                path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                path.CloseFigure();

                c.Region = new Region(path);
            }
        }

        private GraphicsPath GetRoundedRect(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            if (d > rect.Width) d = rect.Width;
            if (d > rect.Height) d = rect.Height;

            path.AddArc(rect.Left, rect.Top, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Top, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.Left, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void AdjustLayout()
        {
            int padding = 12;
            int availableWidth = Math.Max(10, this.ClientSize.Width - padding * 2);

            //measure label height for word wrap
            var proposedSize = new Size(availableWidth, int.MaxValue);
            var textSize = TextRenderer.MeasureText(labelName.Text ?? string.Empty, labelName.Font, proposedSize, TextFormatFlags.WordBreak);
            int labelHeight = Math.Min(textSize.Height, this.ClientSize.Height);

            //center total block
            int totalHeight = pictureBoxIcon.Height + 8 + labelHeight;
            int top = Math.Max(padding, (this.ClientSize.Height - totalHeight) / 2);

            //position picture centered
            pictureBoxIcon.Left = (this.ClientSize.Width - pictureBoxIcon.Width) / 2;
            pictureBoxIcon.Top = top;

            //position label under picture
            labelName.Width = availableWidth;
            labelName.Height = labelHeight;
            labelName.Left = (this.ClientSize.Width - labelName.Width) / 2;
            labelName.Top = pictureBoxIcon.Bottom + 8;

            //製品名の文字列折り返し
            labelName.AutoSize = true;
            labelName.MaximumSize = new Size(availableWidth, 0);
        }

        private void OnClickAll(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(LaunchPath))
                return;

            try
            {
                Process.Start(new ProcessStartInfo { FileName = LaunchPath, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"起動に失敗しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
