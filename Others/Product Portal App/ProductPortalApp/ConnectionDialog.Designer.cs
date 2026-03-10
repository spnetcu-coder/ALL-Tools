namespace ProductPortalApp
{
    //接続ダイアログ用
    partial class ConnectionDialog
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox textBoxHost;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label labelHost;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        //表示の初期化
        private void InitializeComponent()
        {
            textBoxHost = new TextBox();
            textBoxPort = new TextBox();
            labelHost = new Label();
            labelPort = new Label();
            OK = new Button();
            Cancel = new Button();
            SuspendLayout();

            //ホスト名・ポート番号のテキストボックスとラベル、OK/キャンセルのボタン配置
            textBoxHost.Location = new Point(111, 20);
            textBoxHost.Margin = new Padding(4, 5, 4, 5);
            textBoxHost.Name = "textBoxHost";
            textBoxHost.Size = new Size(213, 31);
            textBoxHost.TabIndex = 1;

            textBoxPort.Location = new Point(111, 68);
            textBoxPort.Margin = new Padding(4, 5, 4, 5);
            textBoxPort.Name = "textBoxPort";
            textBoxPort.Size = new Size(213, 31);
            textBoxPort.TabIndex = 3;

            labelHost.AutoSize = true;
            labelHost.Location = new Point(17, 25);
            labelHost.Margin = new Padding(4, 0, 4, 0);
            labelHost.Name = "labelHost";
            labelHost.Size = new Size(75, 25);
            labelHost.TabIndex = 0;
            labelHost.Text = "ホスト名:";

            labelPort.AutoSize = true;
            labelPort.Location = new Point(17, 73);
            labelPort.Margin = new Padding(4, 0, 4, 0);
            labelPort.Name = "labelPort";
            labelPort.Size = new Size(91, 25);
            labelPort.TabIndex = 2;
            labelPort.Text = "ポート番号:";

            OK.Location = new Point(48, 133);
            OK.Margin = new Padding(4, 5, 4, 5);
            OK.Name = "OK";
            OK.Size = new Size(107, 38);
            OK.TabIndex = 4;
            OK.Text = "OK";
            OK.UseVisualStyleBackColor = true;
            OK.Click += buttonOK_Click;

            Cancel.Location = new Point(185, 133);
            Cancel.Margin = new Padding(4, 5, 4, 5);
            Cancel.Name = "Cancel";
            Cancel.Size = new Size(107, 38);
            Cancel.TabIndex = 5;
            Cancel.Text = "キャンセル";
            Cancel.UseVisualStyleBackColor = true;
            Cancel.Click += buttonCancel_Click;

            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(334, 202);
            Controls.Add(Cancel);
            Controls.Add(OK);
            Controls.Add(textBoxPort);
            Controls.Add(labelPort);
            Controls.Add(textBoxHost);
            Controls.Add(labelHost);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ConnectionDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FEM接続設定";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
