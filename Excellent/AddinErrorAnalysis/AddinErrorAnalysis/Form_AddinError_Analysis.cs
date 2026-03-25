using System.Drawing.Text;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace AddinErrorAnalysis
{
    public partial class AddinErrorAnalysis : Form
    {
        public AddinErrorAnalysis()
        {
            InitializeComponent();
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
                if (iExcelBit == -1) {
                    TXT_RESULT.Text= $"Excelのbit数が取得できませんでした。\r\n" +
                        $"Excelのインストール状態を確認してください。";
                    break;
                }

                // アドインファイルのbit数をPEヘッダから取得
                iXLLBit = GetXLLBit(strAddinPath);
                if (iXLLBit == -1) {
                    TXT_RESULT.Text= $"アドインファイルのbit数が取得できませんでした。\r\n" +
                        $"アドインファイルがExcelのbit数に対応しているか確認してください。";
                    break;
                }
                
                // ExcelとExcellentのbit数不一致のアドインエラー
                if(iExcelBit != iXLLBit) {
                    string strExcelBit = (iExcelBit == 0) ? "32bit" : "64bit";
                    string strXLLBit = (iXLLBit == 0) ? "32bit" : "64bit";
                    TXT_RESULT.Text= $"Excelとアドインファイルのbit数が一致していません。\r\n" +
                        $"Excelのbit数：{strExcelBit}\r\n" +
                        $"Excellentのbit数：{strXLLBit}\r\n\r\n" +
                        $"Excelと同じbit数のExcellentをインストールしてください。";
                    break;
                }

                // 環境変数関係のアドインエラー
                if (!CheckEnvVariables(iXLLBit))
                {
                    break;
                }

                TXT_RESULT.Text = $"このツールではアドインエラーの原因を判定できません。";

            } while (false);

        }


        // Excelのアドインファイル（xlt32.xll）のパスをレジストリから取得する
        private string GetXllAddinPath()
        {
            string strKeyPath = @"Software\Microsoft\Office\16.0\Excel\Options";
            string strPath = string.Empty;

            RegistryKey? key = Registry.CurrentUser.OpenSubKey(strKeyPath);

            do
            {
                if (key == null)
                {               
                    TXT_RESULT.Text=$"レジストリが正しく読み取れませんでした。\r\n" +
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

                bool exists = File.Exists(strPath);
                if (!exists)
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
                    strKeyPath = $@"SOFTWARE\Microsoft\Office\{ver}.0\Excel\InstallRoot";
                    using (RegistryKey? key = Registry.LocalMachine.OpenSubKey(strKeyPath))
                    {

                        if (key == null)
                        {
                            continue;
                        }

                        string? path = key.GetValue("Path")?.ToString();
                        if ((string.IsNullOrEmpty(path)))
                        {
                            continue;
                        }
                        if (path.Contains("Program Files (x86)"))
                        {
                            iRet = 0;
                        }
                        else if (path.Contains("Program Files"))
                        {
                            iRet = 1;
                        }
                        else
                        {
                            iRet = -1;
                        }
                    }
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

        /*Excellentのインストールディレクトリが環境変数PATHに含まれているか確認する
         戻り値：true = 環境変数PATHに含まれている, 
                 false = 環境変数PATHに含まれていない、またはエラー
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
                if(string.IsNullOrEmpty(strEXDir))
                {
                    TXT_RESULT.Text = $"Excellentのインストールディレクトリが取得できませんでした。\r\n" +
                        $"Excellentのインストール状態を確認してください。";
                    break;
                }

                // 環境変数PATHにExcellentのインストールディレクトリが含まれているか確認
                string? strEnvVars = Environment.GetEnvironmentVariable("PATH");
                if (string.IsNullOrEmpty(strEnvVars)) 
                {
                    TXT_RESULT.Text = $"環境変数PATHが取得できませんでした。\r\n" +
                        $"環境変数PATHを確認してください。";
                    break;
                }

                bool bExisits = strEnvVars.Contains(strEXDir, StringComparison.OrdinalIgnoreCase);
                if (!bExisits)
                {
                    TXT_RESULT.Text = $"環境変数PATHにExcellentのインストールディレクトリが含まれていません。\r\n" +
                        $"Excellentのインストールディレクトリ：{strEXDir}\r\n\r\n" +
                        $"環境変数PATHにExcellentのインストールディレクトリを追加してください。";
                    break;
                }

                bRet = true;

            } while (false);

            return bRet;

        }
    }
}


