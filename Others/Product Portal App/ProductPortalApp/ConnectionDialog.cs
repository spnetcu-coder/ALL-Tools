using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProductPortalApp
{
    public partial class ConnectionDialog : Form
    {
        //ホスト名とポート番号を表示
        public string HostName => textBoxHost.Text.Trim();
        public int PortNumber => int.TryParse(textBoxPort.Text, out var p) ? p : 8080;

        //Display_Programから呼び出す際に、ホスト名・ポート番号のデフォルト値を指定できるようにする
        public ConnectionDialog(string defaultHost = "localhost", int defaultPort = 8080, string? title = null, Color? backgroundColor = null)
        {
            InitializeComponent();
            if (!string.IsNullOrWhiteSpace(title))
                Text = title;
            if (backgroundColor.HasValue)
                BackColor = backgroundColor.Value;
            textBoxHost.Text = defaultHost;
            textBoxPort.Text = defaultPort.ToString();
        }

        private void buttonOK_Click(object send, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object send, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
