using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

Console.WriteLine("Apache自動初期設定ツール");

//ApacheHomeを入力
Console.Write("Apache Homeを入力してください: ");
string? ApacheHome = Console.ReadLine();

if (string.IsNullOrWhiteSpace(ApacheHome))
{
    Console.WriteLine("Apache Homeが入力されていません。");
    return;
}

//WebQueryHomeを入力
Console.Write("WebQuery Homeを入力してください: ");
string? WebQueryHome = Console.ReadLine();
if (string.IsNullOrWhiteSpace(WebQueryHome))

//入力しなかった場合にエラー文
{
     Console.WriteLine("WebQuery Homeが入力されていません。");
    return;
}

//httpd.confに追記
//httpd.confを開く
string path = $"{ApacheHome}/conf/httpd.conf";


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
var fileLines = File.ReadAllLines(path).ToList();

foreach (var setting in settings)
{
    //何行目にあるか探す
    // Indexには見つかった行番号が入る
    // 見つからない場合は -1
    int Index = fileLines.FindIndex(
        x => x.TrimStart().StartsWith(setting.Key)
    );


    //見つかった場合(indexが0以上)
    if (Index >= 0)
    {
        // 上書き(行番号を探して書き換える)
        fileLines[Index] = setting.Value;  
    }
    //見つからない場合(indexが0未満)
    else
    {
        // 追加
        fileLines.Add(setting.Value);      
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

File.WriteAllLines(path, fileLines);

//cgid_module有効化_ある場合にコメントアウトを解除
string text = File.ReadAllText(path);

text = text.Replace(
    "#LoadModule cgid_module modules/mod_cgid.so",
    "LoadModule cgid_module modules/mod_cgid.so"
);

File.WriteAllText(path, text);

//MIMEタイプの設定を行う
//mime.typesを開く
string path2 = $"{ApacheHome}/conf/mime.types";

//追記内容(末尾に追加)
var lines2 = new List<string>
{
"application/octet-stream dat",
"application/x-csv csv"
};

// ファイルに追記（存在しない場合は作成）
File.AppendAllLines(path2, lines2);

//文字コードの設定





