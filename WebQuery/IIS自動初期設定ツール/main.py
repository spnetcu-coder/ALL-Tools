#IISの初期設定実行ファイル
import sys
import iis_config
from ctypes import windll

def runas():

	print("新規作成開始")

	exit_iis_alias = iis_config.get_virtual_directories() # 設定済みのエイリアスの取得
	result = iis_config.create_virtual_directories(exit_iis_alias) # 設定されていない仮想ディレクトリの作成

	if result == False: # 登録する仮想ディレクトリが存在しなかった場合
		print("登録する情報はありませんでした。")
		input("Enterキーで終了")

	
	else: # 登録する仮想ディレクトリが1つ以上存在した場合
		input("【登録完了：Enterキーで終了】")


if __name__ == "__main__":
    runas()