# Apache自動初期設定ツール_使い方

## 概要
Apache自動初期設定ツールは、ApacheおよびWebQueryの初期設定を自動で行うツールです。

本ツールを実行することで、以下の設定を自動で実施します。

- httpd.confへのAlias設定
- httpd.confへのScriptAlias設定
- Directory設定
- cgid_moduleの有効化
- MIMEタイプ設定

設定済みの項目が存在する場合は、上書きするかどうかを選択できます。

## 使用方法

①アプリを起動します。

②Apache Homeを入力してください:
と表示されるので、Apache Homeの正確なパスを入力してください。

③WebQuery Homeを入力してください
と表示されるので、WebQuery Homeの正確なパスを入力してください。

## 注意事項

Apache Home、WebQuery Home それぞれについて、以下の場合はエラーとなり再入力が求められます。 
- 何も入力されていない場合 
- 存在しないパスが入力された場合

## 自動設定項目に関して
このプログラムでは、以下の項目が設定されます。

### ①httpd.confに登録するAlias/ScriptAlias設定
Apache Home¥conf 内のhttpd.confファイル内に次の項目が設定されます。

#### Alias の登録

~~~
Alias /sn/images/ “<WebQuery Home>/cgi-bin/images/”

Alias /sn/webquery/ “<WebQuery Home>/webquery/”

Alias /async/ “<WebQuery Home>/async/”

Alias /sn/xml/ “<WebQuery Home>/xml/”
~~~


#### ScriptAlias の登録
~~~
ScriptAlias /sn/ “<WebQuery Home>/cgi-bin/”
~~~

すでに登録されていた場合以下のメッセージが表示されます。

~~~
エイリアスは既に登録済みです。

上書きしますか？ 

(Y=Yes（上書き） / N=No(スキップ) / S=Stop（処理中断）)
~~~

Y,N,Sは必ず大文字で入力して下さい。

Y:新規のエイリアスを上書きする形で登録します。

N:上書きせずに、元のエイリアスのまま、次の処理へと進みます。

S:自動設定項目プログラム自体を終了させます。

※上記以外のコマンドや小文字のy,n,sを入力するとエラーとなり、再度入力していただきます。

登録されていなかった場合、自動で登録され次の処理に進みます。

### ②ディレクトリ設定
ディレクトリ設定が設定されていた場合、新たに設定を更新します。
更新内容は以下の通りです。
~~~
fileLines[DirectoryIndex] = $"<Directory \"{WebQueryHome}\">";
fileLines[DirectoryIndex + 1] = "AllowOverride None";
fileLines[DirectoryIndex + 2] = "Require all granted";
 fileLines[DirectoryIndex + 3] = "</Directory>";
~~~  
   
    

設定されていなかった場合には、新規に設定を行います。

### ③cgid_module有効化

以下がコメントアウトされていた場合、コメントアウトを外します。

~~~
#LoadModule cgid_module modules/mod_cgid.so
~~~

すでにコメントアウトが外れていた場合は、この手順はスキップされます。


### ④MIMEタイプ設定
以下の内容を登録します。
~~~
application/octet-stream", "application/octet-stream dat" , 

"application/x-csv", "application/x-csv csv" 
~~~

すでに登録されていた場合以下のメッセージが表示されます。

~~~
MIME設定は既に登録済みです。
上書きしますか？ 

(Y=Yes（上書き） / N=No(スキップ) / S=Stop（処理中断）)
~~~

Y,N,Sは必ず大文字で入力して下さい。

Y:新規のmimeを上書きする形で登録します。

N:上書きせずに、元のmimeのまま、次の処理へと進みます。

S:自動設定項目プログラム自体を終了させます。

※上記以外のコマンドや小文字のy,n,sを入力するとエラーとなり、再度入力していただきます。

登録されていなかった場合、自動でMIMEが登録され次の処理に進みます。


### 正常終了

正常終了した場合、以下の文章が表示されます。

~~~
Apacheの自動初期設定が完了しました！
何かキーを押すと画面を閉じます...
~~~

キーを入力して、画面を閉じてください。