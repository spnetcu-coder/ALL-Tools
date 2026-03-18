using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;


namespace FileCollectApp
{
    public partial class FileCollect : Form
    {
        public FileCollect()
        {
            InitializeComponent();

            //前回のパスを表示させる
            if (!string.IsNullOrEmpty(Properties.Settings.Default.LastFolderPath)
                && Directory.Exists(Properties.Settings.Default.LastFolderPath))
            {
                strFolderPath.Text = Properties.Settings.Default.LastFolderPath;
            }
        }
        //[参照]ボタン
        private void BtnRef_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            // 前回使用したフォルダが初期表示に設定される
            dialog.SelectedPath = Properties.Settings.Default.LastFolderPath;

            // [OK]ボタンが押された場合
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // 選択したフォルダのパスをテキストボックスに表示させる
                strFolderPath.Text = dialog.SelectedPath;

                //次回起動時に初期表示させるために保存
                Properties.Settings.Default.LastFolderPath = dialog.SelectedPath;
                Properties.Settings.Default.Save();
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
            string strAppPath = Application.StartupPath; //実行しているexeファイルがあるパス
            string strBasePath = strFolderPath.Text; // 検索元のパス
            int iOKCount = 0; //コピーに成功した件数
            int iErrorCount = 0; //コピーに失敗した件数
            List<string> listErrorFiles = new List<string>(); //コピーに失敗したファイルと理由のリスト

            if (listResult.Items.Count == 0)
            {
                MessageBox.Show("　コピーするファイルがありません。");
                return;
            }

            if (!strBasePath.EndsWith("\\"))
            {
                strBasePath += "\\";
            }


            for (int iIndex = 0; iIndex < listResult.Items.Count; iIndex++)
            {
                string strSourcePath = listResult.Items[iIndex].ToString();

                try
                {
                    //検索後にファイルが削除された場合
                    if (!File.Exists(strSourcePath))
                    {
                        listErrorFiles.Add(strSourcePath + "[存在しません]");
                        iErrorCount++;
                        continue;
                    }

                    //フォルダの構成を保ったまま取得
                    string strRelativePath = strSourcePath.Substring(strBasePath.Length);
                    string strDestPath = Path.Combine(strAppPath, strRelativePath);
                    string strDestDir = Path.GetDirectoryName(strDestPath);
                    if (!string.IsNullOrEmpty(strDestDir))
                    {
                        Directory.CreateDirectory(strDestDir);
                    }

                    File.Copy(strSourcePath, strDestPath, true);
                    iOKCount++;
                }
                // アクセス拒否が起きた場合
                catch (UnauthorizedAccessException)
                {
                    listErrorFiles.Add(strSourcePath + "[アクセス拒否]");
                    iErrorCount++;
                }
                //ファイルが使用中等の場合
                catch (IOException)
                {
                    listErrorFiles.Add(strSourcePath + " [I/Oエラー]");
                    iErrorCount++;
                }
                //例外のエラー
                catch (Exception)
                {
                    listErrorFiles.Add(strSourcePath + " [予期しないエラー]");
                    iErrorCount++;
                }
            }

            //コピー完了のメッセージと内訳の表示
            string strMessage =
                "コピーが完了しました!\n" +
                "成功：" + iOKCount + "件\n" +
                "失敗：" + iErrorCount + "件\n";

            if (iErrorCount > 0)
            {
                strMessage += "\n【失敗ファイル一覧】\n" +
                              string.Join("\n", listErrorFiles);
            }

            MessageBox.Show(strMessage);
        }
    }
}
