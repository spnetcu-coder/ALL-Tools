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
            SuspendLayout();
            // 
            // BTN_ANALYSIS_START
            // 
            BTN_ANALYSIS_START.Location = new Point(36, 36);
            BTN_ANALYSIS_START.Name = "BTN_ANALYSIS_START";
            BTN_ANALYSIS_START.Size = new Size(112, 34);
            BTN_ANALYSIS_START.TabIndex = 0;
            BTN_ANALYSIS_START.Text = "分析開始";
            BTN_ANALYSIS_START.UseVisualStyleBackColor = true;
            BTN_ANALYSIS_START.Click += BTN_ANALYSIS_START_Click;
            // 
            // TXT_RESULT
            // 
            TXT_RESULT.BorderStyle = BorderStyle.FixedSingle;
            TXT_RESULT.Location = new Point(36, 87);
            TXT_RESULT.Multiline = true;
            TXT_RESULT.Name = "TXT_RESULT";
            TXT_RESULT.ReadOnly = true;
            TXT_RESULT.Size = new Size(727, 212);
            TXT_RESULT.TabIndex = 1;
            // 
            // AddinErrorAnalysis
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(TXT_RESULT);
            Controls.Add(BTN_ANALYSIS_START);
            Name = "AddinErrorAnalysis";
            Text = "アドインエラー分析ツール";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BTN_ANALYSIS_START;
        private TextBox TXT_RESULT;
    }
}
