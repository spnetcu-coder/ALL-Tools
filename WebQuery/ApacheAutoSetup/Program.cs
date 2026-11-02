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

//追記内容(末尾に追加)
var lines = new List<string>
{
    //Alias の登録
    $"Alias /sn/images/ \"{WebQueryHome}/cgi-bin/images/\"",
    $"Alias /sn/webquery/ \"{WebQueryHome}/webquery/\"",
    $"Alias /async/ \"{WebQueryHome}/async/\"",
    $"Alias /sn/xml/ \"{WebQueryHome}/xml/\"",
    
    //ScriptAlias の登録
    $"ScriptAlias /sn/ \"{WebQueryHome}/cgi-bin/\"",

    $"<Directory \"{WebQueryHome}\">",
    "AllowOverride None",
    "Require all granted",
   
    "</Directory>",         
    "<Directory \"/usr/local\">",
    "Options FollowSymLinks",
    "</Directory>"
};

// ファイルに追記（存在しない場合は作成）
File.AppendAllLines(path, lines);

//cgid_module有効化_ある場合にコメントアウトを解除
string text = File.ReadAllText(path);

text = text.Replace(
    "#LoadModule cgid_module modules/mod_cgid.so",
    "LoadModule cgid_module modules/mod_cgid.so"
);

File.WriteAllText(path, text);

//MIMEタイプの設定を行う
//mime.typesを開く
string path2 = $"{ApacheHome}¥conf¥mime.types";

//追記内容(末尾に追加)
var lines2 = new List<string>
{
"application/octet-stream dat",
"application/x-csv csv"
};

// ファイルに追記（存在しない場合は作成）
File.AppendAllLines(path2, lines2);


