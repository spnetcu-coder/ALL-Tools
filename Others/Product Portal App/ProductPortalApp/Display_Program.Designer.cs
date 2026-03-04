namespace ProductPortalApp
{
    partial class Display_Program
    {

        //コンポーネント宣言
        private System.ComponentModel.IContainer components = null;

        //変数宣言
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label titleLabel;
        private Display_Design productTile1;
        private Display_Design productTile2;
        private Display_Design productTile3;

        //クローズ時処理(メモリの解放)
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        //画面描画処理
        private void InitializeComponent()
        {
            flowLayoutPanel1 = new FlowLayoutPanel();
            productTile1 = new Display_Design();
            productTile3 = new Display_Design();
            productTile2 = new Display_Design();
            titleLabel = new Label();

            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();

            //パネル配置エリア
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.BackColor = SystemColors.Control;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 141);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(12);
            flowLayoutPanel1.Size = new Size(1200, 579);
            flowLayoutPanel1.TabIndex = 0;

            //位置を指定してパネルを追加
            flowLayoutPanel1.Controls.Add(productTile1);
            flowLayoutPanel1.Controls.Add(productTile3);
            flowLayoutPanel1.Controls.Add(productTile2);

            //productTile1(左)
            productTile1.BackColor = Color.FromArgb(240, 240, 240);
            productTile1.Location = new Point(15, 15);
            productTile1.Name = "productTile1";
            productTile1.Size = new Size(250, 250);
            productTile1.TabIndex = 0;
            productTile1.Load += productTile1_Load;

            //productTile3(中央)
            productTile3.BackColor = Color.FromArgb(240, 240, 240);
            productTile3.Location = new Point(271, 15);
            productTile3.Name = "productTile3";
            productTile3.Size = new Size(250, 250);
            productTile3.TabIndex = 2;

            //productTile2(右)
            productTile2.BackColor = Color.FromArgb(240, 240, 240);
            productTile2.Location = new Point(527, 15);
            productTile2.Name = "productTile2";
            productTile2.Size = new Size(250, 250);
            productTile2.TabIndex = 1;

            //ヘッダー部分
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Font = new Font("Segoe UI", 32F);
            titleLabel.ForeColor = Color.Black;
            titleLabel.Location = new Point(0, 0);
            titleLabel.Name = "titleLabel";
            titleLabel.Padding = new Padding(40, 8, 0, 8);
            titleLabel.Size = new Size(1200, 141);
            titleLabel.TabIndex = 1;
            titleLabel.Text = "Product Portal App";
            titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            titleLabel.Click += titleLabel_Click;

            //全体ウィンドウ設定
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 720);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(titleLabel);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Product Portal";
            Load += Form1_Load;

            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
    }
}