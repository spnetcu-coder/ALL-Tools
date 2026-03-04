using System;
using System.ComponentModel;
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

        //パネルの初期設定
        public Display_Program()
        {
            InitializeComponent();
            defaultIcon = Image.FromFile("product.png");
            InitializePanels();
            //サイズ変更時にパネルサイズを調整
            this.Resize += (s, ev) => AdjustTileSizes();
            flowLayoutPanel1.Resize += (s, ev) => AdjustTileSizes();
        }

        public void InitializePanels()
        {
            //既定の3タイルを設定
            ConfigureTile(productTile1, "FreeWay管理コンソール", defaultIcon!, "notepad.exe", ProductCategory.FreeWay);
            ConfigureTile(productTile2, "環境設定(Xltmnt.exe)", defaultIcon!, "mspaint.exe", ProductCategory.Excellent);
            ConfigureTile(productTile3, "環境設定(environ.exe)", defaultIcon!, "calc.exe", ProductCategory.WebQuery);

            //Excellent関連製品一覧
            var EX_Product = new string[]
            {
                "IndefaultIcon.exe",
                "FunctionMaintenance.exe",
                "XLT32.chm",
                "Xlttslic.exe",
                "TsUsrmnt.exe",
                "XltTsmnt.exe",
                "Xltchodm.exe",
                "XltJobPathChanger.exe",
                "XltJobPathChanger.chm"
            };
            AddDynamicTiles(EX_Product, ProductCategory.Excellent, useFileNameWithoutExtension: false);

            //freeWay関連製品一覧
            var FW_Product = new string[]
            {
                "FwSrvIni.exe",
                "Joiner64.exe",
                "FreeWayJobManagementControl.exe",
                "FreeWayManagementControl.exe",
                "FEM管理ツール",
                "FreeWay Enterprise Manager"
            };
            AddDynamicTiles(FW_Product, ProductCategory.FreeWay, useFileNameWithoutExtension: false);

            //WebQuery関連製品一覧
            var WQ_Product = new string[]
            {
               "license.exe",
               "WebQuery管理ツール",
               "WebQuery",
               "wqcsv.exe"
            };
            AddDynamicTiles(WQ_Product, ProductCategory.WebQuery, useFileNameWithoutExtension: false);

            //DataHarbor関連製品一覧
            var DH_Product = new string[]
            {
               "DataHarbor管理画面",
               "DataHarbor"
            };
            AddDynamicTiles(DH_Product, ProductCategory.DataHarbor, useFileNameWithoutExtension: false);

            //追加後にサイズを再計算
            AdjustTileSizes();
        }

        private void AddDynamicTiles(string[] files, ProductCategory category, bool useFileNameWithoutExtension)
        {
            foreach (var f in files)
            {
                var displayName = useFileNameWithoutExtension
                    ? Path.GetFileNameWithoutExtension(f)
                    : Path.GetFileName(f);

                var tile = new Display_Design();
                ConfigureTile(tile, displayName, defaultIcon!, f, category);
                tile.Tag = "dynamic";
                flowLayoutPanel1.Controls.Add(tile);
            }
        }

        //タイルの共通デザインを設定
        private void ConfigureTile(Display_Design tile, string displayName, Image defaultIcon, string launchPath, ProductCategory category)
        {
            tile.DisplayName = displayName;
            tile.IconImage = defaultIcon;
            tile.LaunchPath = launchPath;
            tile.Category = category;

            tile.BackColor = Color.FromArgb(250, 250, 250);
            tile.Margin = new Padding(20);
            tile.Padding = new Padding(6);
            tile.Size = new Size(312, 300);
        }

        //1列のパネル数を指定→均等にサイズを調整する
        private void AdjustTileSizes()
        {
            const int columns = 4;
            if (flowLayoutPanel1 == null)
                return;

            int availWidth = flowLayoutPanel1.ClientSize.Width - flowLayoutPanel1.Padding.Left - flowLayoutPanel1.Padding.Right;
            if (availWidth <= 0)
                return;

            int columnWidth = availWidth / columns;
            int horizontalMargin = productTile1.Margin.Left + productTile1.Margin.Right;
            int tileSize = Math.Max(100, columnWidth - horizontalMargin);

            foreach (Control c in flowLayoutPanel1.Controls)
            {
                if (c is Display_Design)
                {
                    c.Width = tileSize;
                    c.Height = tileSize;
                }
            }
        }

        private void productTile2_Load(object sender, EventArgs e)
        {
        }

        private void productTile3_Load(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Enumerate embedded resources for debugging
            var asm = Assembly.GetExecutingAssembly();
            Debug.WriteLine("Assembly manifest resources:");
            foreach (var n in asm.GetManifestResourceNames())
                Debug.WriteLine(n);

            try
            {
                privateFonts = new PrivateFontCollection();
                //Look for Armata-Regular.ttf as embedded resource
                var resName = asm.GetManifestResourceNames()
                    .FirstOrDefault(n => n.EndsWith("Armata-Regular.ttf", StringComparison.OrdinalIgnoreCase));

                if (resName != null)
                {
                    using var rs = asm.GetManifestResourceStream(resName);
                    if (rs != null)
                    {
                        var data = new byte[rs.Length];
                        ReadFully(rs, data);
                        var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                        try
                        {
                            privateFonts.AddMemoryFont(handle.AddrOfPinnedObject(), data.Length);
                        }
                        finally { handle.Free(); }

                        if (privateFonts.Families.Length > 0)
                            //Use the designer-set font size so runtime font preserves the size you set in the designer
                            titleLabel.Font = new Font(privateFonts.Families[0], titleLabel.Font.Size, titleLabel.Font.Style);

                        Debug.WriteLine($"Loaded embedded font: {resName}");
                    }
                }
                else
                {
                    Debug.WriteLine("Embedded font resource not found. Trying output folder...");
                    var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Armata-Regular.ttf");
                    if (File.Exists(path))
                    {
                        privateFonts.AddFontFile(path);
                        if (privateFonts.Families.Length > 0)
                            titleLabel.Font = new Font(privateFonts.Families[0], titleLabel.Font.Size, titleLabel.Font.Style);
                        Debug.WriteLine($"Loaded font from file: {path}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("フォント読み込みエラー: " + ex.Message);
            }

            //Ensure tiles are sized correctly
            AdjustTileSizes();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void titleLabel_Click(object sender, EventArgs e)
        {
        }

        private void productTile1_Load(object sender, EventArgs e)
        {
        }

        private void productTile2_Load_1(object sender, EventArgs e)
        {
        }

        private static void ReadFully(Stream s, byte[] buffer)
        {
            int offset = 0;
            while (offset < buffer.Length)
            {
                int read = s.Read(buffer, offset, buffer.Length - offset);
                if (read == 0)
                    throw new EndOfStreamException("Unable to read all bytes from stream.");
                offset += read;
            }
        }
    }
}