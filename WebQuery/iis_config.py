# IISの設定を行うファイル

import json
import subprocess

# IISに設定済みの仮想ディレクトリ一覧を取得する関数
def get_virtual_directories():

	cmd = "Get-WebVirtualDirectory | ConvertTo-Json" # PowerShellで実行するIIS設定取得コマンド
	ret = subprocess.run(['powershell','-Command', cmd],capture_output=True, text=True, encoding='cp932')
	exit_iis_alias = [] # IIS設定情報から取得したエイリアスを格納するための変数


	# 取得したデータが空でない場合は、データの型をリスト型に統一する
	if ret.stdout.strip():
		object_ret = json.loads(ret.stdout) # JSON形式の文字列をPythonのオブジェクトに変換

		# 取得したデータが複数件の場合(データがリスト型になっている)
		if isinstance(object_ret, list): # object_ret内に格納されているデータがリスト型になっているかどうかを判定
			
			object_list = object_ret

		# 取得したデータが1件のとき(辞書型の場合)
		else:
			object_list = [object_ret] # データをリスト型に変換

	# 取得した値が空の場合
	else:
		object_list = []


	# 取得結果表示(キー確認用)
	#print(f"取得した仮想ディレクトリ情報:{object_list}")
	#input("Enterキーを押下")


	# 値が空でない場合、IISに設定されているエイリアスを一覧から取得する
	if object_list != []:

		if isinstance(object_list, list): # リスト型になっているか判定

			for iis_alias in object_list:
				if isinstance(iis_alias, dict): # 辞書型のデータかどうか判定
					exit_iis_alias.append(iis_alias.get("path")) # path(エイリアス格納キー)を指定して取得する


		# 取得結果表示(IISのエイリアス確認用)
		print(f"存在するエイリアス{exit_iis_alias}")
		input("Enterキーを押下")

	return exit_iis_alias


# IIS情報とJSONファイル情報を比較し、指定の仮想ディレクトリを作成する関数
def create_virtual_directories(exit_iis_alias):

	# JSONファイル(ISSの初期設定情報)の読み込み
	with open('config.json', 'r') as f:
		iis_config = json.load(f) # ファイルからJSONデータを読み込む


	# 取得したIIS設定情報と、設定するエイリアスを比較する
	for json_info in iis_config["virtual_directories"]:
		json_alias = json_info["alias"] #JSONファイルのエイリアス情報を取得

		print(f"JSONファイルから取得したエイリアス:{json_alias}")

		# 既に設定されている場合
		if json_alias in exit_iis_alias:

			# デバッグ用
			print(f"設定済み：{json_alias}") 
			pass

		# エイリアスが設定されていない場合は新規に追加
		elif json_alias not in exit_iis_alias:

			#print("新規作成開始")
			input_alias = json_alias.lstrip("/") # 先頭にある/を削除する

			# 取得したエイリアスの値がsnまたはaysncの場合エイリアスを新規作成
			if input_alias == "sn" or input_alias == "async":

				# エイリアスに対応する仮想ディレクトリを割り当てる
				if input_alias == "sn":
					physical_path = "C:\\Program Files\\SystemConsultant\\WebQuery\\cgi-bin"

				else:
					physical_path = "C:\\Program Files\\SystemConsultant\\WebQuery\\async"

				# デバッグ用
				print(f"登録するエイリアス：{input_alias}")
				print(f"登録するパス：{physical_path}") 

				# 仮想ディレクトリの新規作成
				cmd = f"New-WebVirtualDirectory -Site 'Default Web Site' -Name {input_alias} -PhysicalPath '{physical_path}'"
				subprocess.run(['powershell','-Command', cmd],capture_output=True, text=True, encoding='cp932')
				print(f"{physical_path}ディレクトリの作成に成功しました。")


			# エイリアスがsn、aysncでもない場合、snディレクトリ配下に作成する
			elif input_alias == "sn/xml" or input_alias == "sn/webquery":

				if input_alias == "sn/webquery":
					input_physical_path = "C:\\Program Files\\SystemConsultant\\WebQuery\\webquery"

				else:
					input_physical_path = "C:\\Program Files\\SystemConsultant\\WebQuery\\xml"

				# デバッグ用
				print(f"登録するエイリアス：{input_alias}")
				print(f"登録するパス：{physical_path}") 

				cmd = f"New-WebVirtualDirectory -Site 'Default Web Site' -Name {input_alias} -PhysicalPath '{input_physical_path}'"
				subprocess.run(['powershell','-Command', cmd],capture_output=True, text=True, encoding='cp932')
				print(f"{input_physical_path}ディレクトリの作成に成功しました。")

			# 指定の仮想ディレクトリがすべて作成されていない場合、全て新規作成
			else:
				# JSONファイル(ISSの初期設定情報)の読み込み
				with open('config.json', 'r') as f:
					json_virtual_directories = json.load(f)
					
					# 指定のエイリアスとパスを取得
					for config in json_virtual_directories:
						input_alias = config["alias"] # JSONファイルのエイリアス情報を取得
						input_physical_path = config["path"] # JSONファイルのパス情報を取得

					# 新規作成
					cmd = f"New-WebVirtualDirectory -Site 'Default Web Site' -Name {input_alias} -PhysicalPath '{input_physical_path}'"
					subprocess.run(['powershell','-Command', cmd],capture_output=True, text=True, encoding='cp932')
					print("4つの仮想ディレクトリが作成完了しました")
				
		else:
			input("登録する情報はありませんでした。") # 全て登録済みの場合のメッセージ

input("【登録完了：Enterキーで終了】") # 関数の終わり(終了メッセージ)