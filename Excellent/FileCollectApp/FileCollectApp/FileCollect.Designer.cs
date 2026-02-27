namespace FileCollectApp
{
    partial class FileCollect
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.strFolderPath = new System.Windows.Forms.TextBox();
            this.BtnRef = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkAlv = new System.Windows.Forms.CheckBox();
            this.chkCtg = new System.Windows.Forms.CheckBox();
            this.chkCtgx = new System.Windows.Forms.CheckBox();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.listResult = new System.Windows.Forms.ListBox();
            this.BtnCopy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // strFolderPath
            // 
            this.strFolderPath.Location = new System.Drawing.Point(159, 81);
            this.strFolderPath.Name = "strFolderPath";
            this.strFolderPath.Size = new System.Drawing.Size(470, 25);
            this.strFolderPath.TabIndex = 0;
            // 
            // BtnRef
            // 
            this.BtnRef.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.BtnRef.Location = new System.Drawing.Point(646, 73);
            this.BtnRef.Name = "BtnRef";
            this.BtnRef.Size = new System.Drawing.Size(74, 41);
            this.BtnRef.TabIndex = 1;
            this.BtnRef.Text = "参照";
            this.BtnRef.UseVisualStyleBackColor = true;
            this.BtnRef.Click += new System.EventHandler(this.BtnRef_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(87, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 27);
            this.label1.TabIndex = 2;
            this.label1.Text = "パス：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(69, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 27);
            this.label2.TabIndex = 3;
            this.label2.Text = "拡張子：";
            // 
            // chkAlv
            // 
            this.chkAlv.AutoSize = true;
            this.chkAlv.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkAlv.Location = new System.Drawing.Point(207, 141);
            this.chkAlv.Name = "chkAlv";
            this.chkAlv.Size = new System.Drawing.Size(69, 31);
            this.chkAlv.TabIndex = 4;
            this.chkAlv.Text = ".alv";
            this.chkAlv.UseVisualStyleBackColor = true;
            // 
            // chkCtg
            // 
            this.chkCtg.AutoSize = true;
            this.chkCtg.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkCtg.Location = new System.Drawing.Point(337, 142);
            this.chkCtg.Name = "chkCtg";
            this.chkCtg.Size = new System.Drawing.Size(71, 31);
            this.chkCtg.TabIndex = 5;
            this.chkCtg.Text = ".ctg";
            this.chkCtg.UseVisualStyleBackColor = true;
            // 
            // chkCtgx
            // 
            this.chkCtgx.AutoSize = true;
            this.chkCtgx.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkCtgx.Location = new System.Drawing.Point(471, 142);
            this.chkCtgx.Name = "chkCtgx";
            this.chkCtgx.Size = new System.Drawing.Size(81, 31);
            this.chkCtgx.TabIndex = 6;
            this.chkCtgx.Text = ".ctgx";
            this.chkCtgx.UseVisualStyleBackColor = true;
            // 
            // BtnSearch
            // 
            this.BtnSearch.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.BtnSearch.Location = new System.Drawing.Point(40, 207);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(149, 38);
            this.BtnSearch.TabIndex = 7;
            this.BtnSearch.Text = "検索";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // listResult
            // 
            this.listResult.FormattingEnabled = true;
            this.listResult.ItemHeight = 18;
            this.listResult.Location = new System.Drawing.Point(40, 261);
            this.listResult.Name = "listResult";
            this.listResult.Size = new System.Drawing.Size(728, 166);
            this.listResult.TabIndex = 8;
            // 
            // BtnCopy
            // 
            this.BtnCopy.Location = new System.Drawing.Point(219, 207);
            this.BtnCopy.Name = "BtnCopy";
            this.BtnCopy.Size = new System.Drawing.Size(132, 38);
            this.BtnCopy.TabIndex = 9;
            this.BtnCopy.Text = "コピー";
            this.BtnCopy.UseVisualStyleBackColor = true;
            this.BtnCopy.Click += new System.EventHandler(this.BtnCopy_Click);
            // 
            // FileCollect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BtnCopy);
            this.Controls.Add(this.listResult);
            this.Controls.Add(this.BtnSearch);
            this.Controls.Add(this.chkCtgx);
            this.Controls.Add(this.chkCtg);
            this.Controls.Add(this.chkAlv);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnRef);
            this.Controls.Add(this.strFolderPath);
            this.Name = "FileCollect";
            this.Text = "FileCollect";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox strFolderPath;
        private System.Windows.Forms.Button BtnRef;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkAlv;
        private System.Windows.Forms.CheckBox chkCtg;
        private System.Windows.Forms.CheckBox chkCtgx;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.ListBox listResult;
        private System.Windows.Forms.Button BtnCopy;
    }
}

