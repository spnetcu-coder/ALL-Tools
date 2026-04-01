namespace AddinErrorAnalysis
{
    partial class AddinErrorAnalysis
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            BTN_ANALYSIS_START = new Button();
            TXT_RESULT = new TextBox();
            BTN_END = new Button();
            SuspendLayout();
            // 
            // BTN_ANALYSIS_START
            // 
            BTN_ANALYSIS_START.Location = new Point(24, 23);
            BTN_ANALYSIS_START.Margin = new Padding(2);
            BTN_ANALYSIS_START.Name = "BTN_ANALYSIS_START";
            BTN_ANALYSIS_START.Size = new Size(100, 28);
            BTN_ANALYSIS_START.TabIndex = 0;
            BTN_ANALYSIS_START.Text = "分析開始(&A)";
            BTN_ANALYSIS_START.UseVisualStyleBackColor = true;
            BTN_ANALYSIS_START.Click += BTN_ANALYSIS_START_Click;
            // 
            // TXT_RESULT
            // 
            TXT_RESULT.BorderStyle = BorderStyle.FixedSingle;
            TXT_RESULT.Location = new Point(24, 73);
            TXT_RESULT.Margin = new Padding(2);
            TXT_RESULT.Multiline = true;
            TXT_RESULT.Name = "TXT_RESULT";
            TXT_RESULT.ReadOnly = true;
            TXT_RESULT.Size = new Size(540, 207);
            TXT_RESULT.TabIndex = 1;
            // 
            // BTN_END
            // 
            BTN_END.Location = new Point(466, 300);
            BTN_END.Name = "BTN_END";
            BTN_END.Size = new Size(100, 28);
            BTN_END.TabIndex = 2;
            BTN_END.Text = "終了(&X)";
            BTN_END.UseVisualStyleBackColor = true;
            BTN_END.Click += BTN_END_Click;
            // 
            // AddinErrorAnalysis
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 352);
            Controls.Add(BTN_END);
            Controls.Add(TXT_RESULT);
            Controls.Add(BTN_ANALYSIS_START);
            Font = new Font("Yu Gothic UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AddinErrorAnalysis";
            Text = "アドインエラー分析ツール";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BTN_ANALYSIS_START;
        private TextBox TXT_RESULT;
        private Button BTN_END;
    }
}
