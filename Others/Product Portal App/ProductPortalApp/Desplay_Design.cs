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

    //初期表示のパネルデザインを定義
    public class Desplay_Design : UserControl
    {
        private PictureBox picIcon = null!;
        private Label lLabelName = null!;
        private int iRadius = Constants.Panel.RADIUSNUM;
        private bool bHovered = false;
        private Color cStartColor = Color.FromArgb(255, 183, 77);
        private Color cEndColor = Color.FromArgb(245, 222, 179);
        private ProductCategory pCategory = ProductCategory.Default;
        private Label lLabelCategory = null!;


        //製品の起動に使用するパス(実行ファイルのパスやURLなど)を保持
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? LaunchPath { get; set; }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image? IconImage
        {
            get => picIcon.Image;
            set => picIcon.Image = value;
        }
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DisplayName
        {
            get => lLabelName.Text;
            set => lLabelName.Text = value ?? string.Empty;
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ProductCategory Category
        {
            get => pCategory;
            set
            {
                pCategory = value;
                SetLabelColor();
            }
        }

        public Desplay_Design()
        {
            Init_Controls();
            SetLabelColor();
        }

        //表示の初期化
        private void Init_Controls()
        {
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Size = new Size(250, 250);
            this.Cursor = Cursors.Hand;

            picIcon = new PictureBox
            {
                Size = new Size(64, 64),
                Location = new Point(12, 12),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };

            //製品タイトル用
            lLabelName = new Label
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
            lLabelCategory = new Label
            {
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Yu Gothic UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = cStartColor,
                Padding = new Padding(4, 1, 4, 1),
                Text = GetCategoryText(pCategory),
            };

            this.Controls.AddRange(new Control[] { picIcon, lLabelName, lLabelCategory });
            lLabelCategory.BringToFront();

            lLabelCategory.SizeChanged += (s, e) =>
            {
                MakeRound(lLabelCategory, 1);
            };


            //画面サイズ変更時テキストの位置を調整
            this.Resize += (s, e) => AdjustLayout();
            this.Resize += Display_Resize;
            lLabelName.TextChanged += (s, e) => AdjustLayout();

            //初期レイアウト位置
            AdjustLayout();

            //パネル内のどこをクリックしても同じ動作になるように設定
            this.Click += Click_to_Launch;
            foreach (Control c in this.Controls)
            {
                c.Click += Click_to_Launch;
                //ホバー状態でも同じ動作になるように設定
                c.MouseEnter += (s, e) => UpdateHover();
                c.MouseLeave += (s, e) => UpdateHover();
            }

            //ホバー状態を判定
            this.MouseEnter += (s, e) => UpdateHover();
            this.MouseLeave += (s, e) => UpdateHover();
        }

        private void Display_Resize(object? sender, EventArgs e)
        {
            //リサイズ時、エリアを更新し再度角丸にする
            Resize_Radius();
            Invalidate();
        }

        //操作可能な領域を角丸に設定
        private void Resize_Radius()
        {
            var rect = new Rectangle(0, 0, this.Width, this.Height);
            using var path = GetRoundPath(rect, iRadius);
            this.Region?.Dispose();
            this.Region = new Region(path);
        }

        private void SetLabelColor()
        {
            //製品別にラベルの背景色を変更
            switch (pCategory)
            {
                case ProductCategory.FreeWay:
                    cStartColor = Color.FromArgb(255, 195, 95);
                    cEndColor = Color.FromArgb(255, 230, 234);
                    this.BackColor = Color.FromArgb(250, 250, 250);
                    break;
                case ProductCategory.WebQuery:
                    cStartColor = Color.FromArgb(173, 207, 255);
                    cEndColor = Color.FromArgb(132, 245, 255);
                    this.BackColor = Color.FromArgb(250, 250, 250);
                    break;
                case ProductCategory.Excellent:
                    cStartColor = Color.FromArgb(203, 243, 154);
                    cEndColor = Color.FromArgb(224, 255, 132);
                    this.BackColor = Color.FromArgb(250, 250, 250);
                    break;
                case ProductCategory.DataHarbor:
                    cStartColor = Color.FromArgb(204, 153, 255);
                    cEndColor = Color.FromArgb(220, 204, 255);
                    this.BackColor = Color.FromArgb(240, 240, 240);
                    break;
            }
            Invalidate();

            lLabelCategory.Text = GetCategoryText(pCategory);
            lLabelCategory.BackColor = cStartColor;
            lLabelCategory.BringToFront();
        }

        private void SetHover(bool hover)
        {
            if (bHovered == hover) return;
            bHovered = hover;
            Invalidate();
        }

        private void UpdateHover()
        {
            //マウスの位置を座標に変換して、パネル内にいるかを判定
            var pt = this.PointToClient(Cursor.Position);
            bool inside = this.ClientRectangle.Contains(pt);
            SetHover(inside);
        }

        //パネル描画処理(ホバー時はグラデーション、通常時は白)
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var path = GetRoundPath(new Rectangle(0, 0, this.Width, this.Height), iRadius);
            if (bHovered)
            {
                using var brush = new LinearGradientBrush(this.ClientRectangle, cStartColor, cEndColor, 45f);
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

        private GraphicsPath GetRoundPath(Rectangle rect, int radius)
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

            //ラベルの高さをテキストの内容に合わせて調整
            var proposedSize = new Size(availableWidth, int.MaxValue);
            var textSize = TextRenderer.MeasureText(lLabelName.Text ?? string.Empty, lLabelName.Font, proposedSize, TextFormatFlags.WordBreak);
            int labelHeight = Math.Min(textSize.Height, this.ClientSize.Height);

            //アイコンとラベルの高さを計算
            int totalHeight = picIcon.Height + 8 + labelHeight;
            int top = Math.Max(padding, (this.ClientSize.Height - totalHeight) / 2);

            //要素を中央に配置
            picIcon.Left = (this.ClientSize.Width - picIcon.Width) / 2;
            picIcon.Top = top;

            //ラベルの位置とサイズを調整
            lLabelName.Width = availableWidth;
            lLabelName.Height = labelHeight;
            lLabelName.Left = (this.ClientSize.Width - lLabelName.Width) / 2;
            lLabelName.Top = picIcon.Bottom + 8;

            //製品名の文字列折り返し
            lLabelName.AutoSize = true;
            lLabelName.MaximumSize = new Size(availableWidth, 0);
        }

        //パネルクリック時の処理(LaunchPathに応じて製品を起動)
        private void Click_to_Launch(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(LaunchPath))
                return;

            try
            {
                var parent = FindForm() as Display_Program;
                if (parent != null)
                {
                    switch (LaunchPath)
                    {
                        case Constants.LaunchSign.SIGNFEM:
                            parent.LaunchFW(LaunchPath);
                            return;
                        case Constants.LaunchSign.SIGNWQMGR:
                        case Constants.LaunchSign.SIGNWQ:
                            parent.LaunchWQ(LaunchPath);
                            return;
                        case Constants.LaunchSign.SIGNDHMGR:
                        case Constants.LaunchSign.SIGNDH:
                            parent.LaunchDH(LaunchPath);
                            return;
                    }
                }

                Process.Start(new ProcessStartInfo { FileName = LaunchPath, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"起動に失敗しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
