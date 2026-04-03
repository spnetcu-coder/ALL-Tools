# SQk解析テストツールの使い方

__SQLParserTestTool__ とは  
FreeWayのSQL解析機能の開発時に、テストモジュールのビルド～テストまで一括で実行できるアプリケーションです。

##内容物
・SQLParserTestTool.bat：テストモジュールのビルド～テスト実行まで実行するバッチです。
・TestSql.txt：テストしたいSQLを記述するファイルです。

以下のファイルはSQLParserTestTool.batによって呼び出されるファイルです
・Test_FormatSQL.bat：「SQL実行時の別名に固有の識別子を使用する」機能をテストを実施するバッチです。
・Test_AddOwner.bat：「アクセス権のあるオブジェクトを取得する」機能のテストを実施するバッチです。

## 準備手順
※SQLParserTestTool.slnが存在するディレクトリを{HOME}とします。

1.mklinkコマンドを使用して、{HOME}に「FW_Program\windows」までのシンボリックリンクを追加します<br>
  例) mklink /D windows ..\..\..\FW_Program\windows<br>
2.{HOME}/TestSql.txtにテストしたいSQLを入力して下さい。


## 使用手順
1. SQL解析のコードを編集します。<br>
2. SQLParserTestTool.batを起動すると、fwsqlparser.dll、SQLParserTestTool.exeのビルドが行われます。<br>
   ビルドの結果は{HOME}/build_log.logに出力されます。
3. そのままテストが実行され、テスト結果が以下のファイルに出力されます 。<br>
・Result_FormatSQL.txt：「SQL実行時の別名に固有の識別子を使用する」機能のテスト結果
・Result_AddOwner.txt：「アクセス権のあるオブジェクトを取得する」機能のテスト結果
4.テスト結果ファイルには以下のように解析の前後のSQLが出力されています。<br>
  期待通りの結果となっているか確認して下さい。
ーーーーーーーーーーーーーーーーーーーーーーーーーー
[CASE1]
Before=[select clm1 from test;]
After= [SELECT "clm1" FROM "test"]

[CASE2]
Before=[select clm1 clm2 from test;]
After= [SELECT "clm1" "clm2" FROM "test"]

[CASE3]
Before=[select clm1 from test@schema;]
After= [SELECT "clm1" FROM "schema"."test"]
ーーーーーーーーーーーーーーーーーーーーーーーーーー
以上。