using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ProductPortalApp
{
    public partial class Display_Program : Form
    {
        //フォントを呼び出す
        private PrivateFontCollection privateFonts = new PrivateFontCollection();

        private Image? defaultIcon;

        //EXHomeDirを一度だけ呼び出すためのキャッシュ変数
        private string? _excellentHomeDirCache;

        //パネルの初期設定
        public Display_Program()
        {
            InitializeComponent();
            defaultIcon = Image.FromFile("product.png");
            InitializePanel();
            //サイズ変更時にパネルサイズを調整
            this.Resize += (s, ev) => AdjustPanel();
            flowLayoutPanel1.Resize += (s, ev) => AdjustPanel();
        }

        public void InitializePanel()
        {
            //既定の3パネルを設定
            SetCommonPanel(productPanel1, "FreeWay管理コンソール", defaultIcon!, "FreeWay管理コンソール.msc", ProductCategory.FreeWay);
            SetCommonPanel(productPanel2, "環境設定(Xltmnt.exe)", defaultIcon!, "XltMnt.exe", ProductCategory.Excellent);
            SetCommonPanel(productPanel3, "環境設定(environ.exe)", defaultIcon!, "environ.exe", ProductCategory.WebQuery);

            //Excellent関連製品一覧
            var EXProduct = new string[]
            {
                "Inicon.exe",
                "FunctionMaintenance.exe",
                "XLT32.chm",
                "Xlttslic.exe",
                "TsUsrmnt.exe",
                "XltTsmnt.exe",
                "Xltchodm.exe",
                "XltJobPathChanger.exe",
                "XltJobPathChanger.chm"
            };
            AddPanel(EXProduct, ProductCategory.Excellent, useFileNameWithoutExtension: false);

            //freeWay関連製品一覧
            var FWProduct = new string[]
            {
                "FwSrvIni.exe",
                "Joiner64.exe",
                "FreeWayJobManagementControl.exe",
                "FreeWayManagementControl.exe",
                "FEM管理ツール",
                "FreeWay Enterprise Manager"
            };
            AddPanel(FWProduct, ProductCategory.FreeWay, useFileNameWithoutExtension: false);

            //WebQuery関連製品一覧
            var WQProduct = new string[]
            {
               "license.exe",
               "WebQuery管理ツール",
               "WebQuery",
               "wqcsv.exe"
            };
            AddPanel(WQProduct, ProductCategory.WebQuery, useFileNameWithoutExtension: false);

            //DataHarbor関連製品一覧
            var DHProduct = new string[]
            {
               "DataHarbor管理画面",
               "DataHarbor"
            };
            AddPanel(DHProduct, ProductCategory.DataHarbor, useFileNameWithoutExtension: false);

            //追加後にパネル位置を調整
            AdjustPanel();
        }

        private void AddPanel(string[] files, ProductCategory category, bool useFileNameWithoutExtension)
        {
            foreach (var f in files)
            {
                var displayName = useFileNameWithoutExtension
                    ? Path.GetFileNameWithoutExtension(f)
                    : Path.GetFileName(f);

                var Panel = new Display_Design();
                SetCommonPanel(Panel, displayName, defaultIcon!, f, category);
                Panel.Tag = "dynamic";
                flowLayoutPanel1.Controls.Add(Panel);
            }
        }

        //パネルの共通デザインを設定
        private void SetCommonPanel(Display_Design Panel, string displayName, Image defaultIcon, string launchPath, ProductCategory category)
        {
            Panel.DisplayName = displayName;
            Panel.IconImage = defaultIcon;
            Panel.Category = category;

            Panel.BackColor = Color.FromArgb(250, 250, 250);
            Panel.Margin = new Padding(20);
            Panel.Padding = new Padding(6);
            Panel.Size = new Size(312, 300);

            //製品ごとにパスを設定（取得失敗した場合はパネルを無効化）
            try
            {
                Panel.LaunchPath = category switch
                {
                    ProductCategory.Excellent => LaunchEX(launchPath),
                    ProductCategory.FreeWay => GetFWDir(launchPath),
                    ProductCategory.WebQuery => GetWQDir(launchPath),
                    ProductCategory.DataHarbor => DHLaunchPath(launchPath),
                    _ => launchPath,
                };
            }
            catch
            {
                Panel.LaunchPath = null;
                Panel.Enabled = false;
            }
        }

        //1列のパネル数を指定→均等にサイズを調整する
        private void AdjustPanel()
        {
            const int columns = Constants.Panel.COLUMNNUM;
            if (flowLayoutPanel1 == null)
                return;

            int availWidth = flowLayoutPanel1.ClientSize.Width - flowLayoutPanel1.Padding.Left - flowLayoutPanel1.Padding.Right;
            if (availWidth <= 0)
                return;

            int columnWidth = availWidth / columns;
            int horizontalMargin = productPanel1.Margin.Left + productPanel1.Margin.Right;
            int PanelSize = Math.Max(100, columnWidth - horizontalMargin);

            foreach (Control c in flowLayoutPanel1.Controls)
            {
                if (c is Display_Design)
                {
                    c.Width = PanelSize;
                    c.Height = PanelSize;
                }
            }
        }

        //フォントの設定
        private void SetFont(object sender, EventArgs e)
        {
            //フォントファイルを埋め込みから読み込む
            var asm = Assembly.GetExecutingAssembly();
            foreach (var n in asm.GetManifestResourceNames())
                Debug.WriteLine(n);

            try
            {
                privateFonts = new PrivateFontCollection();
                //Armata-Regular.ttfが埋め込みに存在するか確認
                var resName = asm.GetManifestResourceNames()
                    .FirstOrDefault(n => n.EndsWith("Armata-Regular.ttf", StringComparison.OrdinalIgnoreCase));

                if (resName != null)
                {
                    using var rs = asm.GetManifestResourceStream(resName);
                    if (rs != null)
                    {
                        var data = new byte[rs.Length];
                        rs.ReadExactly(data);
                        var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                        try
                        {
                            privateFonts.AddMemoryFont(handle.AddrOfPinnedObject(), data.Length);
                        }
                        finally { handle.Free(); }

                        if (privateFonts.Families.Length > 0)
                            //フォントが正常に読み込まれた後タイトルに適用
                            titleLabel.Font = new Font(privateFonts.Families[0], titleLabel.Font.Size, titleLabel.Font.Style);
                    }
                }
                else
                {
                    var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Armata-Regular.ttf");
                    if (File.Exists(path))
                    {
                        privateFonts.AddFontFile(path);
                        if (privateFonts.Families.Length > 0)
                            titleLabel.Font = new Font(privateFonts.Families[0], titleLabel.Font.Size, titleLabel.Font.Style);
                    }
                }
            }
            catch (Exception)
            {
            }
            //パネルの位置を調整
            AdjustPanel();
        }




        //--------------Excellent製品の起動処理--------------//
        //Excellentのインストールフォルダ(ExcellentHomeDir)を取得
        private string GetEXDir()
        {
            if (_excellentHomeDirCache is not null)
                return _excellentHomeDirCache;

            using var key = Registry.LocalMachine.OpenSubKey(
                Constants.Registry.EXPATH);

            var v = key?.GetValue("ExcellentHomeDir")?.ToString();
            if (string.IsNullOrWhiteSpace(v))
                throw new InvalidOperationException("ExcellentHomeDir を取得できませんでした。");

            _excellentHomeDirCache = v;
            return _excellentHomeDirCache;
        }

        private string LaunchEX(string fileName) => fileName switch
        {
            "XltMnt.exe" or "Inicon.exe" or "FunctionMaintenance.exe"
                => Path.Combine(GetEXDir(), "Init", fileName),
            "XLT32.chm"
                => Path.Combine(GetEXDir(), "Bin", fileName),
            "Xlttslic.exe" or "TsUsrmnt.exe" or "XltTsmnt.exe"
                => Path.Combine(GetEXDir(), "Terminal", fileName),
            "Xltchodm.exe" or "XltJobPathChanger.exe" or "XltJobPathChanger.chm"
                => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName),
            _ => fileName,
        };


        //--------------FreeWay製品の起動処理--------------//
        //レジストリからインストールフォルダ取得
        private static string ReadFWRegistry(string valueName, string errorTarget)
        {
            using var key = Registry.LocalMachine.OpenSubKey(Constants.Registry.FWMGRPATH);
            string path = key?.GetValue(valueName)?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(path))
                throw new InvalidOperationException($"{errorTarget}のインストールフォルダが取得できませんでした。");
            return path;
        }

        private string GetFWDir(string name) => name switch
        {
            "FreeWay管理コンソール.msc"
                => Path.Combine(ReadFWRegistry("IniPathName", "FreeWay管理コンソール"), @"Bin\FreeWay管理コンソール.msc"),
            "FwSrvIni.exe"
                => Path.Combine(ReadFWRegistry("IniPathName", "FwSrvIni.exe"), @"Bin\FwSrvIni.exe"),
            "Joiner64.exe"
                => Path.Combine(ReadFWRegistry("JOINERPath", "Joiner64.exe"), "Joiner64.exe"),
            "FreeWayJobManagementControl.exe"
                => Path.Combine(ReadFWRegistry("JOBPath", "FreeWayJobManagementControl.exe"), "FreeWayJobManagementControl.exe"),
            "FreeWayManagementControl.exe"
                => Path.Combine(ReadFWRegistry("FILTERPath", "FreeWayManagementControl.exe"), "FreeWayManagementControl.exe"),
            "FEM管理ツール" or "FreeWay Enterprise Manager" or "FEM"
                => Constants.LaunchSign.SIGNFEM,
            _ => name,
        };

        private static void OpenConnectDialog(string title, int defaultPort, Color backgroundColor, Func<string, int, string> buildUrl)
        {
            using var dialog = new ConnectionDialog("localhost", defaultPort, title, backgroundColor);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string url = buildUrl(dialog.HostName, dialog.PortNumber);
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
        }

        public void LaunchFW(string name)
        {
            string target = GetFWDir(name);
            if (target == Constants.LaunchSign.SIGNFEM)
            {
                OpenConnectDialog(
                    "FEM接続設定",
                    Constants.Port.FWPORT,
                    Color.FromArgb(255, 179, 102),
                    (host, port) => name == "FEM管理ツール"
                        ? $"http://{host}:{port}{Constants.UrlTemplate.URLFEMMGR}"
                        : $"http://{host}:{port}{Constants.UrlTemplate.URLFEM}");
                return;
            }
            if (string.IsNullOrWhiteSpace(target)) return;
            if (target.StartsWith("http://") || target.StartsWith("https://"))
            {
                Process.Start(new ProcessStartInfo(target) { UseShellExecute = true });
            }
            else
            {
                var psi = new ProcessStartInfo
                {
                    FileName = target,
                    WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                    UseShellExecute = true,
                    Verb = "runas",
                };
                Process.Start(psi);
            }
        }


        //--------------WebQuery製品の起動処理--------------//
        private string GetWQDir(string name)
        {
            string WQHomeDir = Environment.GetEnvironmentVariable("WQ_HOME") ?? "";
            if (string.IsNullOrWhiteSpace(WQHomeDir))
                throw new InvalidOperationException("WebQueryのインストールフォルダが取得できませんでした。");

            return name switch
            {
                "environ.exe" => Path.Combine(WQHomeDir, "Bin", "environ.exe"),
                "license.exe" => Path.Combine(WQHomeDir, "Bin", "license.exe"),
                "wqcsv.exe" => Path.Combine(WQHomeDir, "tools", "wqcsv.exe"),
                "WebQuery管理ツール" => Constants.LaunchSign.SIGNWQMGR,
                "WebQuery" => Constants.LaunchSign.SIGNWQ,
                _ => name,
            };
        }

        public void LaunchWQ(string name)
        {
            string target = GetWQDir(name);
            if (target == Constants.LaunchSign.SIGNWQMGR || target == Constants.LaunchSign.SIGNWQ)
            {
                OpenConnectDialog(
                    "WQ接続設定",
                    Constants.Port.WQPORT,
                    Color.FromArgb(90, 224, 211),
                    (host, port) => target == Constants.LaunchSign.SIGNWQMGR
                        ? $"http://{host}:{port}{Constants.UrlTemplate.URLWQMGR}"
                        : $"http://{host}:{port}{Constants.UrlTemplate.URLWQ}");
                return;
            }
            if (string.IsNullOrWhiteSpace(target)) return;
            if (target.EndsWith(".exe") && File.Exists(target))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = target,
                    WorkingDirectory = Path.GetDirectoryName(target),
                    UseShellExecute = true,
                    Verb = "runas",
                });
            }
        }


        //--------------DataHarbor製品の起動処理--------------//
        private string DHLaunchPath(string name) => name switch
        {
            "DataHarbor管理画面" => Constants.LaunchSign.SIGNDHMGR,
            "DataHarbor" => Constants.LaunchSign.SIGNDH,
            _ => name,
        };

        public void LaunchDH(string name)
        {
            string target = DHLaunchPath(name);
            if (target == Constants.LaunchSign.SIGNDHMGR || target == Constants.LaunchSign.SIGNDH)
            {
                OpenConnectDialog(
                    "DH接続設定",
                    Constants.Port.DHPORT,
                    Color.FromArgb(204, 153, 255),
                    (host, port) => target == Constants.LaunchSign.SIGNDHMGR
                        ? $"http://{host}:{port}{Constants.UrlTemplate.URLDHMGR}"
                        : $"http://{host}:{port}{Constants.UrlTemplate.URLDH}");
                return;
            }
        }
    }
}