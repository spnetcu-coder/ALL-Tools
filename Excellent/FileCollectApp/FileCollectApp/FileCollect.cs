using System;
using System.IO;
using System.Windows.Forms;


namespace FileCollectApp
{
    public partial class FileCollect : Form
    {
        public FileCollect()
        {
            InitializeComponent();
        }
        //[参照]ボタン
        private void BtnRef_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            // [OK]ボタンが押された場合
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // 選択したフォルダのパスをテキストボックスに表示させる
                strFolderPath.Text = dialog.SelectedPath;
            }
        }

        //[検索]ボタン
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            int iCount = 0;
            string strTargetPath = strFolderPath.Text;

            listResult.Items.Clear();

            //拡張子のチェックボックスが1つも選択されていない場合
            if (!chkAlv.Checked && !chkCtg.Checked && !chkCtgx.Checked)
            {
                MessageBox.Show("拡張子を選択してください。");
                return;
            }

            //フォルダが存在しない場合
            if (!Directory.Exists(strTargetPath))
            {
                MessageBox.Show("フォルダがありません。");
                return;
            }

            // 全ファイルを取得する
            string[] strFiles = Directory.GetFiles(strTargetPath, "*.*", SearchOption.AllDirectories);

            // 拡張子をチェックする
            for (int iIndex = 0; iIndex < strFiles.Length; iIndex++)
            {
                string strFilePath = strFiles[iIndex];
                string strExt = Path.GetExtension(strFilePath);

                //拡張子が[.alv]のファイル
                if (chkAlv.Checked == true && strExt == ".alv")
                {
                    listResult.Items.Add(strFilePath);
                    iCount++;
                }
                //拡張子が[.ctg]のファイル
                if (chkCtg.Checked == true && strExt == ".ctg")
                {
                    listResult.Items.Add(strFilePath);
                    iCount++;
                }
                //拡張子が[.ctgx]のファイル
                if (chkCtgx.Checked == true && strExt == ".ctgx")
                {
                    listResult.Items.Add(strFilePath);
                    iCount++;
                }
            }
            MessageBox.Show("検索結果：" + iCount + "件");
        }

        // [コピー] ボタン
        private void BtnCopy_Click(object sender, EventArgs e)
        {
            //ALL-Tools\Excellent\FileCollectApp\FileCollectApp\bin\Debug
            string strAppPath = Application.StartupPath;
            int iCount = 0;

            if (listResult.Items.Count == 0)
            {
                MessageBox.Show("　コピーするファイルがありません。");
                return;
            }

            for (int iIndex = 0; iIndex < listResult.Items.Count; iIndex++)
            {
                string strSourcePath = listResult.Items[iIndex].ToString();
                string strFileName = Path.GetFileName(strSourcePath);
                string strDestPath = Path.Combine(strAppPath, strFileName);

                File.Copy(strSourcePath, strDestPath, true);
                iCount++;
            }

            MessageBox.Show("　コピーが完了しました!：" + iCount + "件");
        }
    }
}
