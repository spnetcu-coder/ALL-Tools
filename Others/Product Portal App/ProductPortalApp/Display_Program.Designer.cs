namespace ProductPortalApp
{
    partial class Display_Program
    {

        //コンポーネント宣言
        private System.ComponentModel.IContainer components = null;

        //変数宣言
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label titleLabel;
        private Desplay_Design productPanel1;
        private Desplay_Design productPanel2;
        private Desplay_Design productPanel3;

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
            productPanel1 = new Desplay_Design();
            productPanel3 = new Desplay_Design();
            productPanel2 = new Desplay_Design();
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
            flowLayoutPanel1.Controls.Add(productPanel1);
            flowLayoutPanel1.Controls.Add(productPanel3);
            flowLayoutPanel1.Controls.Add(productPanel2);

            productPanel1.BackColor = Color.FromArgb(240, 240, 240);
            productPanel1.Location = new Point(15, 15);
            productPanel1.Name = "productPanel1";
            productPanel1.Size = new Size(250, 250);
            productPanel1.TabIndex = 0;

            productPanel2.BackColor = Color.FromArgb(240, 240, 240);
            productPanel2.Location = new Point(527, 15);
            productPanel2.Name = "productPanel2";
            productPanel2.Size = new Size(250, 250);
            productPanel2.TabIndex = 1;

            productPanel3.BackColor = Color.FromArgb(240, 240, 240);
            productPanel3.Location = new Point(271, 15);
            productPanel3.Name = "productPanel3";
            productPanel3.Size = new Size(250, 250);
            productPanel3.TabIndex = 2;

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

            //全体ウィンドウ設定
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 720);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(titleLabel);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Product Portal";
            Load += SetFont;

            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
    }
}