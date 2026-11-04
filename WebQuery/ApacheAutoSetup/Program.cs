using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

Console.WriteLine("Apache自動初期設定ツール");

//ApacheHomeを入力
Console.Write("Apache Homeを入力してください: ");
string? ApacheHome = Console.ReadLine();


//ApacheHomeが入力されたかの確認
if (string.IsNullOrWhiteSpace(ApacheHome))
{
    Console.WriteLine("Apache Homeが入力されていません。");
    return;
}

//パスが存在しているかの確認
if (!Directory.Exists(ApacheHome))
{
    Console.WriteLine("指定されたApache Homeは存在しません。");
    return;
}

//WebQueryHomeを入力
Console.Write("WebQuery Homeを入力してください: ");
string? WebQueryHome = Console.ReadLine();

//入力されたかの確認
if (string.IsNullOrWhiteSpace(WebQueryHome))
{
     Console.WriteLine("WebQuery Homeが入力されていません。");
    return;
}

//WebQueryHomeがパスが存在しているかの確認
if (!Directory.Exists(WebQueryHome))
{
Console.WriteLine("指定されたWebQuery Homeは存在しません。");
return;
}

//httpd.confに追記
//httpd.confを開く
string httpdConfPath = $"{ApacheHome}/conf/httpd.conf";


// httpd.confに登録するAlias/ScriptAlias設定
var settings = new Dictionary<string, string>

{
    //Alias の登録
    { "Alias /sn/images/", $"Alias /sn/images/ \"{WebQueryHome}/cgi-bin/images/\"" },
    { "Alias /sn/webquery/", $"Alias /sn/webquery/ \"{WebQueryHome}/webquery/\"" },
    { "Alias /async/", $"Alias /async/ \"{WebQueryHome}/async/\"" },
    { "Alias /sn/xml/", $"Alias /sn/xml/ \"{WebQueryHome}/xml/\"" },
    //ScriptAlias の登録
    { "ScriptAlias /sn/", $"ScriptAlias /sn/ \"{WebQueryHome}/cgi-bin/\"" }
};

// httpd.conf読込
var fileLines = File.ReadAllLines(httpdConfPath).ToList();

foreach (var setting in settings)
{
    //何行目にあるか探す
    // Indexには見つかった行番号が入る
    // 見つからない場合は -1
    int Index = fileLines.FindIndex(
       //完全一致で検索 
       x => x.Trim().Equals(setting.Value)
    );


    //見つかった場合(indexが0以上)
    if (Index >= 0)
    {
         Console.WriteLine(
            $"エイリアス「{setting.Key}」は既に登録済みです。"
        );
        Console.Write("上書きしますか？ (Y=Yes / N=No / S=Stop) : ");

        string? answer = Console.ReadLine()?.Trim().ToUpper();

        switch (answer)
        {
            case "Y":
                fileLines[Index] = setting.Value;
                Console.WriteLine("上書きしました。");
                break;

            case "N":
                Console.WriteLine("登録をスキップしました。");
                break;

            case "S":
                Console.WriteLine("処理を中断します。");
                return;

            default:
                Console.WriteLine("無効な入力のためスキップしました。");
                break;
        }
    }

    
    //見つからない場合(indexが0未満)
    else
    {
        //エイリアスの追加
        fileLines.Add(setting.Value);      
      Console.WriteLine($"エイリアスを追加しました。");
    }
}

//アクセス権の設定
// <Directory "WebQueryHome"> が何行目にあるか探す
//見つかった場合、何行目かがDirectoryIndexの値に入る
int DirectoryIndex = fileLines.FindIndex(
    x => x.TrimStart().StartsWith($"<Directory \"{WebQueryHome}\">")
);

//見つかった場合
if (DirectoryIndex >= 0)
{
    // 既存のDirectory設定を更新(<Directory “<WebQuery Home>">から順に4行)
    fileLines[DirectoryIndex]     = $"<Directory \"{WebQueryHome}\">";
    fileLines[DirectoryIndex + 1] = "AllowOverride None";
    fileLines[DirectoryIndex + 2] = "Require all granted";
    fileLines[DirectoryIndex + 3] = "</Directory>";
}
//見つからなかった場合
else
{
    // Directory設定を追加
    fileLines.Add($"<Directory \"{WebQueryHome}\">");
    fileLines.Add("AllowOverride None");
    fileLines.Add("Require all granted");
    fileLines.Add("</Directory>");
}

File.WriteAllLines(httpdConfPath, fileLines);

//cgid_module有効化_ある場合にコメントアウトを解除
string text = File.ReadAllText(httpdConfPath);

text = text.Replace(
    "#LoadModule cgid_module modules/mod_cgid.so",
    "LoadModule cgid_module modules/mod_cgid.so"
);

File.WriteAllText(httpdConfPath, text);

//MIMEタイプの設定を行う
// MIMEタイプ設定
string mimeTypesPath= $"{ApacheHome}/conf/mime.types";

var lines = new List<string>
{
    "application/octet-stream dat",
    "application/x-csv csv"
};

//mime.typesを開く
var mimeLines = File.ReadAllLines(mimeTypesPath).ToList();
foreach (var line in lines)
{
    if (!mimeLines.Contains(line))
    {
        mimeLines.Add(line);
    }
}
// ファイルに追記
File.WriteAllLines(mimeTypesPath, mimeLines); 

//文字コードの設定





