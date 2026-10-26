Console.WriteLine("Apache自動初期設定ツール");

//httpd.confに追記

//httpd.confを開く
using System;
using System.IO;
using System.Collections.Generic;
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