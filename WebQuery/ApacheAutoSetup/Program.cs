using System;
using System.IO;
using System.Collections.Generic;
Console.WriteLine("Apache自動初期設定ツール");

//httpd.confに追記
//httpd.confを開く
string path = "/usr/local/apache2/conf/httpd.conf";

//追記内容(末尾に追加)
var lines = new List<string>
{
 "Alias /sn/images/ \"/usr/local/antre/cgi-bin/images/\"",
    "Alias /sn/webquery/ \"/usr/local/antre/webquery/\"",
    "Alias /async/ \"/usr/local/antre/async/\"",
    "Alias /sn/xml/ \"/usr/local/antre/xml/\"",
    "ScriptAlias /sn/ \"/usr/local/antre/cgi-bin/\"",

    "<Directory \"/usr/local/antre\">",
    "    AllowOverride None",
    "    Require all granted",
   
    "</Directory>",         
    "<Directory \"/usr/local\">",
    "    Options FollowSymLinks",
    "</Directory>"
};

// ファイルに追記（存在しない場合は作成）
File.AppendAllLines(path, lines);



//※ ある場合にコメントアウトを解除
string text = File.ReadAllText(path);

text = text.Replace(
    "#LoadModule cgid_module modules/mod_cgid.so",
    "LoadModule cgid_module modules/mod_cgid.so"
);

File.WriteAllText(path, text);

//MIMEタイプの設定を行う
//mime.typesを開く
string path2 = "/usr/local/apache2/conf/mime.types";

//追記内容(末尾に追加)
var lines2 = new List<string>
{
"application/octet-stream dat",
"application/x-csv csv"
};

// ファイルに追記（存在しない場合は作成）
File.AppendAllLines(path2, lines2);