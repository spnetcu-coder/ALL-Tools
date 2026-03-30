using System.Drawing.Text;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace AddinErrorAnalysis
{
    public partial class AddinErrorAnalysis : Form
    {
        public AddinErrorAnalysis()
        {
            InitializeComponent();
            this.CancelButton = BTN_END;
        }

        // 「分析開始」ボタン
        private void BTN_ANALYSIS_START_Click(object sender, EventArgs e)
        {
            int iExcelBit = -1;
            int iXLLBit = -1;
            string strAddinPath = string.Empty;

            do
            {
                // Excelのアドインファイルのパスをレジストリから取得
                strAddinPath = GetXllAddinPath();

                // アドインファイルパスが取得できない場合はbreak
                if (strAddinPath == "")
                {
                    break;
                }

                // Excelのbit数をレジストリから取得
                iExcelBit = GetExcelBitFromRegistry();
                if (iExcelBit == -1)
                {
                    TXT_RESULT.Text = $"Excelのbit数が取得できませんでした。\r\n" +
                        $"Excelのインストール状態を確認してください。";
                    break;
                }

                // アドインファイルのbit数をPEヘッダから取得
                iXLLBit = GetXLLBit(strAddinPath);
                if (iXLLBit == -1)
                {
                    TXT_RESULT.Text = $"アドインファイルのbit数が取得できませんでした。\r\n" +
                        $"アドインファイルがExcelのbit数に対応しているか確認してください。";
                    break;
                }

                // ExcelとExcellentのbit数不一致のアドインエラー
                if (iExcelBit != iXLLBit)
                {
                    string strExcelBit = (iExcelBit == 0) ? "32bit" : "64bit";
                    string strXLLBit = (iXLLBit == 0) ? "32bit" : "64bit";
                    TXT_RESULT.Text = $"Excelとアドインファイルのbit数が一致していません。\r\n" +
                        $"Excelのbit数：{strExcelBit}\r\n" +
                        $"Excellentのbit数：{strXLLBit}\r\n\r\n" +
                        $"Excelと同じbit数のExcellentをインストールしてください。";
                    break;
                }

                // 紅忠コイルセンター問い合わせ対応のアドインエラー
                if (!CheckRegistry())
                {
                    break;
                }

                // 無効なアプリケーションアドインに登録されているか確認
                if (!CheckDisabledItems())
                {
                    break;
                }

                // トラストセンター関係のアドインエラー
                if (!CheckAddinOptOfTrustCenter())
                {
                    break;
                }

                // 環境変数関係のアドインエラー
                if (!CheckEnvVariables(iXLLBit))
                {
                    break;
                }

                TXT_RESULT.Text = $"このツールではアドインエラーの原因を判定できません。\r\n" +
                    $"サービスセンターにお問い合わせください。";

            } while (false);

        }

        // 「終了」ボタン
        private void BTN_END_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Excelのアドインファイル（xlt32.xll）のパスをレジストリから取得する
        private string GetXllAddinPath()
        {
            string strKeyPath = $@"Software\Microsoft\Office\16.0\Excel\Options";
            string strPath = string.Empty;

            RegistryKey? key = Registry.CurrentUser.OpenSubKey(strKeyPath);

            do
            {
                if (key == null)
                {
                    TXT_RESULT.Text = $"レジストリが正しく読み取れませんでした。\r\n" +
                        $"レジストリパス：HKEY_CURRENT_USER\\{strKeyPath}\r\n\r\n" +
                        $"Excelのオプション設定を確認してください。";
                    break;

                }

                foreach (var name in key.GetValueNames())
                {
                    if (name.StartsWith("OPEN"))
                    {
                        object? valueObj = key.GetValue(name);
                        string? strValue = valueObj?.ToString();
                        if (!string.IsNullOrEmpty(strValue) && strValue.EndsWith("xlt32.xll\""))
                        {
                            strPath = Regex.Match(strValue, "\"([^\"]+)\"").Groups[1].Value;
                        }
                    }
                }

                bool bExists = File.Exists(strPath);
                if (!bExists)
                {
                    TXT_RESULT.Text = $"アドインファイルが存在しません。\r\n" +
                        $"アドイン参照先パス：{strPath}\r\n\r\n" +
                        $"対象ファイルを配置するか、参照先を変更してください。";
                    strPath = string.Empty;
                }
            } while (false);

            return strPath;
        }



        /*Excelのbit数をレジストリから取得する
        　戻り値：0 = 32bit, 
                　1 = 64bit, 
               　-1 = エラー             
         */
        private int GetExcelBitFromRegistry()
        {
            string strKeyPath = $@"SOFTWARE\Microsoft\Office\ClickToRun\Configuration";
            int iRet = -1;
            bool bClickToRun = false;
            string[] strExVersions = { "16.0", "15.0", "14.0" };


            // ClickToRunの場合
            using (RegistryKey? key = Registry.LocalMachine.OpenSubKey(strKeyPath))
            {
                do
                {
                    if (key == null)
                    {
                        iRet = -1;
                        break;
                    }
                    string? value = key.GetValue("Platform")?.ToString();
                    if (value == "x86")
                    {
                        iRet = 0;
                    }
                    else if (value == "x64")
                    {
                        iRet = 1;
                    }
                    else
                    {
                        iRet = -1;
                        break;
                    }
                    bClickToRun = true;
                } while (false);
            }
            // ClickToRunでない場合は、インストールパスからbit数を判断する
            do
            {
                if (bClickToRun)
                {
                    break;
                }

                foreach (var ver in strExVersions)
                {
                    strKeyPath = $@"SOFTWARE\Microsoft\Office\{ver}\Excel\InstallRoot";

                    do
                    {
                        using (RegistryKey? basekey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
                        using (RegistryKey? key = basekey.OpenSubKey(strKeyPath))
                        {

                            if (key == null)
                            {
                                break;
                            }

                            string? path = key.GetValue("Path")?.ToString();
                            if ((string.IsNullOrEmpty(path)))
                            {
                                break;
                            }
                            if (path.Contains("Program Files (x86)"))
                            {
                                iRet = 0;
                                break;
                            }
                            else if (path.Contains("Program Files"))
                            {
                                iRet = 1;
                                break;
                            }

                        }
                    } while (false);

                    do
                    {
                        using (RegistryKey? basekey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                        using (RegistryKey? key = basekey.OpenSubKey(strKeyPath))
                        {

                            if (key == null)
                            {
                                break;
                            }

                            string? path = key.GetValue("Path")?.ToString();
                            if ((string.IsNullOrEmpty(path)))
                            {
                                break;
                            }
                            if (path.Contains("Program Files (x86)"))
                            {
                                iRet = 0;
                                break;
                            }
                            else if (path.Contains("Program Files"))
                            {
                                iRet = 1;
                                break;
                            }
                        }
                    } while (false);
                }
            } while (false);

            return iRet;
        }

        /* Excellentのbit数を取得する
          戻り値：0 = 32bit, 
                　1 = 64bit, 
               　-1 = エラー 
        */
        private int GetXLLBit(string strPath)
        {
            int iRet = -1;

            do
            {
                // ファイルが存在しない場合はエラー
                if (!File.Exists(strPath))
                {
                    break;
                }

                // xllファイルはPE形式の実行ファイルと同様の構造を持つため、PEヘッダを読み取ることでbit数を判断できる
                using (var fs = new FileStream(strPath, FileMode.Open, FileAccess.Read))
                using (var br = new BinaryReader(fs))
                {

                    // PEヘッダ位置のオフセットを取得
                    fs.Seek(0x3C, SeekOrigin.Begin);
                    int iPEHeaderOffset = br.ReadInt32();

                    // PEヘッダのMachineフィールドを読み取る
                    fs.Seek(iPEHeaderOffset + 4, SeekOrigin.Begin);
                    ushort machineType = br.ReadUInt16();

                    if (machineType == 0x014c) // 0x014c=x86
                    {
                        iRet = 0; // 32bit
                    }
                    else if (machineType == 0x8664) // 0x8664=x64
                    {
                        iRet = 1; // 64bit
                    }
                    else
                    {
                        iRet = -1; // 不明
                    }
                }

            } while (false);

            return iRet;

        }

        /*Excellentのインストールディレクトリが環境変数「Path」に含まれているか確認する
         戻り値：true = 環境変数「Path」に含まれている, 
                 false = 環境変数「Path」に含まれていない、またはエラー
         */
        private bool CheckEnvVariables(int iXLLBit)
        {
            string? strEXDir = string.Empty;
            bool bRet = false;

            do
            {
                // Excellentのインストールディレクトリをレジストリから取得
                if (iXLLBit == 0)
                {
                    strEXDir = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\SystemConsultant\Excellent\System", "ExcellentDir", null)?.ToString();
                }
                else if (iXLLBit == 1)
                {
                    strEXDir = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\SystemConsultant\Excellent\System", "ExcellentDir", null)?.ToString();
                }
                if (string.IsNullOrEmpty(strEXDir))
                {
                    TXT_RESULT.Text = $"Excellentのインストールディレクトリが取得できませんでした。\r\n" +
                        $"Excellentのインストール状態を確認してください。";
                    break;
                }

                // 環境変数「Path」にExcellentのインストールディレクトリが含まれているか確認
                string? strEnvVars = Environment.GetEnvironmentVariable("Path");
                if (string.IsNullOrEmpty(strEnvVars))
                {
                    TXT_RESULT.Text = $"環境変数「Path」が取得できませんでした。\r\n" +
                        $"環境変数「Path」を確認してください。";
                    break;
                }

                bool bExisits = strEnvVars.Contains(strEXDir, StringComparison.OrdinalIgnoreCase);
                if (!bExisits)
                {
                    TXT_RESULT.Text = $"環境変数「Path」にExcellentのインストールディレクトリが含まれていません。\r\n" +
                        $"Excellentのインストールディレクトリ：{strEXDir}\r\n\r\n" +
                        $"環境変数「Path」にExcellentのインストールディレクトリを追加してください。\r\n" +
                        $"１.「コントロールパネル」-「システム」-「システムの詳細設定」をクリック\r\n" +
                        $"２.「環境変数」ボタンを押下\r\n" +
                        $"３.「システム環境変数」の「Path」をダブルクリック\r\n" +
                        $"４.「新規」ボタンを押下後、「{strEXDir}」を追加\r\n" +
                        $"５. OKボタンで全ての画面を閉じた後、端末を再起動";
                    break;
                }

                bRet = true;

            } while (false);

            return bRet;

        }

        /* Excelのトラストセンターのアドイン設定を確認する
         戻り値：true = トラストセンターのアドイン設定にチェックがない, 
                 false = トラストセンターのアドイン設定のいずれかにチェックがある、またはエラー
         */
        private bool CheckAddinOptOfTrustCenter()
        {
            bool bRet = false;
            string[] strExVersions = { "16.0", "15.0", "14.0" };
            string? strDisableAllAddins = string.Empty; // すべてのアプリケーションアドイン無効
            string? strNoTBPrompt = string.Empty;       // 署名のないアドインに関する通知を無効
            string? strRequireSig = string.Empty;       // 署名のないアドイン無効

            foreach (var ver in strExVersions)
            {
                string strKeyPath = $@"SOFTWARE\Microsoft\Office\{ver}\Excel\Security";
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(strKeyPath))
                {
                    if (key == null)
                    {
                        continue;
                    }

                    strDisableAllAddins = key.GetValue("DisableAllAddins")?.ToString();
                    strNoTBPrompt = key.GetValue("NoTBPromptUnsignedAddin")?.ToString();
                    strRequireSig = key.GetValue("RequireAddinSig")?.ToString();

                    if (strDisableAllAddins == "1" || strNoTBPrompt == "1" || strRequireSig == "1")
                    {
                        TXT_RESULT.Text = $"Excelのトラストセンターの設定にてアドインが無効となる可能性があります。\r\n" +
                            $"Excelのオプションからトラストセンターのアドイン設定を確認して、チェックを全て外してください。\r\n\r\n" +
                            $"１. Excelの「ファイル」-「オプション」-「トラストセンター」をクリック\r\n" +
                            $"２.「トラストセンターの設定」ボタンを押下\r\n" +
                            $"３.「アドイン」をクリック\r\n" +
                            $"４. 3つのチェックボックスのいずれか、またはすべてにチェックが入っている場合、全てのチェックを外す\r\n" +
                            $"５. OKボタンで全ての画面を閉じた後、Excelを再起動";
                        break;
                    }
                    else
                    {
                        bRet = true;
                        break;
                    }
                }
            }

            return bRet;

        }



        /*「Xlt32.xll」が無効なアプリケーションアドインに登録されているか確認
         戻り値：true = 無効なアプリケーションアドインに登録されていない, 
                 false = 無効なアプリケーションアドインに登録されている、またはエラー     
         */
        private bool CheckDisabledItems()
        {
            bool bRet = true;
            string[] strExVersions = { "16.0", "15.0", "14.0" };
            
            string strPath = string.Empty;

            foreach (var ver in strExVersions)
            {
                string strKeyPath = $@"Software\Microsoft\Office\{ver}\Excel\Resiliency\DisabledItems";
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(strKeyPath))
                {
                    if(key == null)
                    {
                        continue;
                    }

                    foreach (var name in key.GetValueNames())
                    {
                        var data = key.GetValue(name) as byte[];
                        if (data == null)
                        {
                            continue;
                        }
                        string text = Encoding.Unicode.GetString(data);
                        
                        if(text.Contains("xlt32.xll", StringComparison.OrdinalIgnoreCase))
                        {
                            TXT_RESULT.Text = $"Excellentが「無効なアプリケーションアドイン」に含まれている可能性があります。\r\n" +
                            $"ExcelのオプションからExcellentのアドインを有効にしてください。\r\n\r\n" +
                            $"１. Excelの「ファイル」-「オプション」-「アドイン」をクリック\r\n" +
                            $"２.「無効なアプリケーションアドイン」にExcellentが含まれているか確認\r\n" +
                            $"３. 含まれている場合、管理(A)で\"使用できないアイテム\"を選択して「設定」ボタンを押下\r\n" +
                            $"４. 使用できるようにするアイテムで「Xlt32(Excellent)」を選択して「有効にする」を押下\r\n" +
                            $"５. Excelを再起動";

                            bRet = false;
                        }
                    }
                    break;
                }
            }
                return bRet;
        }

        /*紅忠コイルセンター問い合わせ対応
         対象のレジストリにアドインファイルが登録されていると、アドインが無効になる
         対象のレジストリにアドインファイルが登録されていないかを確認
         戻り値：true = レジストリに登録されていない, 
                 false = レジストリに登録されている、またはエラー     
         */
        private bool CheckRegistry()
        {
            bool bRet = true;
            string strKeyPath = $@"Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers";

            do
            {
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(strKeyPath))
                {
                    if (key == null)
                    {
                        break;
                    }

                    foreach (var name in key.GetValueNames())
                    {
                        object? value = key.GetValue(name);
                        if(value == null)
                        {
                            continue;
                        }

                        string? strValue = value.ToString();
                        if (strValue != null && strValue.Contains("xlt32.xll", StringComparison.OrdinalIgnoreCase))
                        {
                            TXT_RESULT.Text = $"下記レジストリによって、アドインが無効となっている可能性があります。\r\n" +
                            $"下記レジストリの情報を削除してください。\r\n\r\n" +
                            $"キー：{key}\r\n" +
                            $"名前：{name}\r\n" +
                            $"値：{strValue}";

                            bRet = false;
                        }

                    }
                }

            } while (false);

            return bRet;

        }
    }
}


