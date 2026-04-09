#include "fwsqlanalyze_helper.h"
#include "FwPointerArray.h"


/* fwsqlparser.dllから呼ぶ出す関数 */
__declspec(dllimport) void*  SnSqlParserGetFirstParse(char* pStrSql, int nCharacterCode, char* pStrErrMsg);
__declspec(dllimport) BOOL SnSqlParserGetObjectsAndAlias(void* objTree, FwObjectsInformation* objects, FwObjectMaster* pObjectMaster, fwPointerArray* displayColumnList);
__declspec(dllimport) char* SnSqlParserFormatSQLwithEnclose(void* objTree, int nCharacterCode, FwObjectsInformation* objects, char enclose_char, FwObjectMaster* pObjectMaster);
__declspec(dllimport) BOOL SnSqlParserIsSelectStmt(void* objTree, char* pStrErrMsg);
__declspec(dllimport)  BOOL SnSqlParserGetObjects(void* objTree, FwObjectsInformation* objects);
__declspec(dllimport)  char* SnSqlParserGetSQLwithOptions(void* objTree, int nCharacterCode, FwObjectsInformation* objects, char enclose_char);


int main(int argc, char** argv) {

	int iMode = 0;//SQL解析モード、0:固有の識別子、1:アクセス権のあるオブジェクト取得
	int iCharCode = atoi(argv[2]);//文字コード、0:UTF-8、その他:SJIS
	char cEnclose = ' ';//括り文字 D="",B=[],その他=無し

	char szBeforeSQL[12800];
	memset(szBeforeSQL, '\0', sizeof(szBeforeSQL));
	char szAfterSQL[12800];
	memset(szAfterSQL, '\0', sizeof(szAfterSQL));
	char szErrMsg[512];
	memset(szErrMsg, '\0', sizeof(szErrMsg));

	int iRetCode = 0;
	int i = 0;

	FILE* fpOutFile = NULL;
	FILE* fpInFile = NULL;
	char* szOutFileName[50];
	memset(szOutFileName, '\0', sizeof(szOutFileName));
	
	/* コマンドライン引数の取得 */
	if (argc == 2) {
		iMode = atoi(argv[1]);
	}
	else if (argc == 3) {
		iMode = atoi(argv[1]);
		iCharCode = atoi(argv[2]);
	}else if (argc == 4) {
		iMode = atoi(argv[1]);
		iCharCode = atoi(argv[2]);
		if (argv[3] == 'B') {
			cEnclose = 'B';
		}else if (argv[3]=='D'){
			cEnclose = 'D';
		}
	}else if (argc > 4){
		printf("コマンドライン引数の数が超過しています。\n");
		return -1;
	};
	
	

	/* モードに応じてSQL解析を実行 */
	switch (iMode){
		case 0: {
			/* SQL実行時の別名に固有の識別子を使用する */
			strcpy_s(szOutFileName, sizeof(szOutFileName), "./Result/Result_FormatSQL.txt");
			fopen_s(&fpOutFile, szOutFileName, "w");
			if (fpOutFile == NULL) {
				return -1;
			}
			fopen_s(&fpInFile, "./TestSql.txt", "r");
			if (fpInFile == NULL) {
				flose(fpOutFile);
				return -1;
			}
			while (fgets(szBeforeSQL, sizeof(szBeforeSQL), fpInFile) != NULL) {
				i++;
				if (szBeforeSQL[strlen(szBeforeSQL)-1] == '\n') {
					szBeforeSQL[strlen(szBeforeSQL)-1] = '\0';
				}
				iRetCode = iTestParserGetObjectsAndAlias(szBeforeSQL, szAfterSQL, sizeof(szAfterSQL), iCharCode, cEnclose, szErrMsg);
				fprintf_s(fpOutFile, "[CASE%d]\nBefore=[%s]\nAfter= [%s]\n\n", i, szBeforeSQL, szAfterSQL);

			}
			fclose(fpInFile);
			fclose(fpOutFile);
			
			break;
		}
		case 1: {
			/* アクセス権のあるオブジェクトを取得する */
			strcpy_s(szOutFileName, sizeof(szOutFileName), "./Result/Result_AddOwner.txt");
			fopen_s(&fpOutFile, szOutFileName, "w");
			if (fpOutFile == NULL) {
				return -1;
			}
			fopen_s(&fpInFile, "./TestSql.txt", "r");
			if (fpInFile == NULL) {
				flose(fpOutFile);
				return -1;
			}
			while (fgets(szBeforeSQL, sizeof(szBeforeSQL), fpInFile) != NULL) {
				i++;
				if (szBeforeSQL[strlen(szBeforeSQL) - 1] == '\n') {
					szBeforeSQL[strlen(szBeforeSQL) - 1] = '\0';
				}
				iRetCode = szTestParserGetSQLwithOptions(szBeforeSQL, szAfterSQL, sizeof(szAfterSQL), iCharCode, cEnclose, szErrMsg);
				if (iRetCode != 0) {
					fclose(fpInFile);
					fclose(fpOutFile);
					return -1;
				}
				fprintf_s(fpOutFile, "[CASE%d]\nBefore=[%s]\nAfter= [%s]\n\n", i, szBeforeSQL, szAfterSQL);
			}
			fclose(fpInFile);
			fclose(fpOutFile);
			
			break;
		}

		default: {
			break;
		}
	}
	return 0;
}

/* 固有の識別子 */
int iTestParserGetObjectsAndAlias(char* szInSql, char* szOutSql, int iSqlSize,int iCharCode, char cEnclose, char* szErrMsg) {
	void* tree = NULL;
	BOOL bIsSelectSql = FALSE;
	BOOL bRetCode = FALSE;
	char* szSql = NULL;

	FwObjectsInformation objects;
	FwObjectMaster stObjectMaster;
	fwPointerArray* display_columns = NULL;

	memset(&objects, 0x00, sizeof(objects));
	memset(&stObjectMaster, 0x00, sizeof(stObjectMaster));

	/* SQLを分解 */
	tree = SnSqlParserGetFirstParse(szInSql, iCharCode, szErrMsg);
	if (tree == NULL) {
		return -1;
	}
	/* SELECT文かどうか判別 */
	bIsSelectSql = SnSqlParserIsSelectStmt(tree, szErrMsg);
	if (bIsSelectSql == FALSE) {
		return -1;
	}

	/* オブジェクトと別名を取得 */
	bRetCode = SnSqlParserGetObjectsAndAlias(tree, &objects, &stObjectMaster, display_columns);
	if (bRetCode == FALSE) {
		return -1;
	}

	/* SQLを書き換える */
	szSql = SnSqlParserFormatSQLwithEnclose(tree, iCharCode, &objects, cEnclose, &stObjectMaster);
	strcpy_s(szOutSql, iSqlSize, szSql);

	return 0;
}

/* アクセス権のあるオブジェクト取得 */
int szTestParserGetSQLwithOptions(char* szInSql, char* szOutSql, int iSqlSize,int iCharCode, char cEnclose, char* szErrMsg) {
	void* tree = NULL;
	BOOL bIsSelectSql = FALSE;
	BOOL bRetCode = FALSE;
	char* szSql = NULL;

	FwObjectsInformation objects;
	memset(&objects, 0x00, sizeof(objects));
	objects.division_char = '@';

	/* SQLを分解 */
	tree = SnSqlParserGetFirstParse(szInSql, iCharCode, szErrMsg);
	if (tree == NULL) {
		return -1;
	}
	/* SELECT文かどうか判別 */
	bIsSelectSql = SnSqlParserIsSelectStmt(tree, szErrMsg);
	if (bIsSelectSql == FALSE) {
		return -1;
	}

	/* オブジェクトを取得 */
	bRetCode = SnSqlParserGetObjects(tree, &objects);
	if (bRetCode == FALSE) {
		return -1;
	}
	/* SQLを書き換え */
	szSql = SnSqlParserGetSQLwithOptions(tree, iCharCode, &objects, cEnclose);
	strcpy_s(szOutSql, iSqlSize, szSql);

	return 0;
}