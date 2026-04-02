@echo off
echo 処理開始

@rem 変数宣言
set directory=%~dp0
set build="x64 Release|x64"
set log=%directory%build_log.log
set BUILD_LIB_HOST=PC-NAME


echo ビルド開始
if %COMPUTERNAME%==%BUILD_LIB_HOST% (
	call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\VC\Auxiliary\Build\vcvarsall.bat" amd64
) else (
	call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\VC\Auxiliary\Build\vcvarsall.bat" amd64
)

del %log%
echo fwsqlparserビルド開始
devenv /rebuild %build% "./windows/share/fwsqlparser/fwsqlparser.vs2019.sln" >> %log%

echo SQLParserTestTool.exeビルド開始
devenv /rebuild %build% "./SQLParserTestTool.sln" >> %log%

echo ビルド終了。build_log.logを確認して下さい。

echo 固有の識別子テスト開始
call ./Test_FormatSQL.bat
echo アクセス権のあるオブジェクト取得テスト開始
call ./Test_AddOwner.bat


echo テスト結果をResultフォルダに出力しました。
echo 処理終了
pause