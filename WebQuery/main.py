#IISの初期設定実行ファイル
import iis_config
import sys
sys.dont_write_bytecode = True # 実行時にpycファイルを生成しないようにする設定
from ctypes import windll

def runas():

	windll.shell32.ShellExecuteW(
		None,
		"runas", # 管理者権限で実行
		"python", # コンソール出したくないならpythonw
		"iis_config.py", # 実行したいスクリプト
		None,
		1
		)

	exit_iis_alias = iis_config.get_virtual_directories()
	result = iis_config.create_virtual_directories(exit_iis_alias)

if __name__ == "__main__":
	runas()