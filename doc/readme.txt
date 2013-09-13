Boare.Lib.AppUtil
Boare.Lib.Media
Boare.Lib.Vsq
	ライブラリ

bocoree
	Boare.Lib.*から共通して参照されるコア・ライブラリ

BuildJavaUI
	eclipseのVisual Editorでユーザ・インターフェース（のみ）を作成するための作業用ディレクトリ

Cadencii
	Cadencii本体

DbgUtil
	デバッグ用のツールなど。内容は不定

EditOtoIni
	UTAUの音源設定ファイル(oto.ini)を編集し、さらにStraight Library用の*.stfファイルを管理するアプリケーション

GenerateKeySound
	Cadencii本体で、鍵盤を押したときに鳴らすのに用いる*.wavファイルを作成するツール

hook_rewire
	ReWireの解析

PitchEdit
	UTAU用プラグイン実装（の残骸）

PlaySound
	Cadenciiで音を鳴らすためのライブラリ

port_java
	Java移植作業用のディレクトリ。この中のファイルは、たいていpp_cs2javaのMakefileから自動作成される

port_java_OBSOLUTE
	Java移植作業の残骸。Cadenciiのコードを愚直にJavaにハンド・コンパイルしようとしていた

pp_cs2java
	Cadencii（と、その依存ライブラリ）のC#コードから、Javaのコードを自動作成するツール（実際は、単なる機能限定プリプロセッサ）

ScriptImplement
	Cadenciiのスクリプト、パレットツールを実装するための作業用

straightVoiceDB
	*.wavファイルから*.stfファイルを作成するツール

stream2base64
	ファイルをBase64エンコードするツール

utauvsti
	UTAUのコアを駆動して音声合成させるVST Instrument（の残骸）

vstidrv3
	CadenciiからVSTiを呼ぶためのライブラリの残骸

WebPOEdit
	言語設定ファイルである*.poファイルを、ブラウザ上で編集するサービスを提供する

FormMain_Layout.png
	Cadenciiのメインウィンドウ(class FormMain)内の、コンポーネントの配置関係の概略図

memo.txt
	備忘録

makefile
	Cadenciiとその依存ライブラリを、monoのC#コンパイラでコンパイルするためのMakefile（の残骸）

readme.txt
	（それは"これ"です！）

decode.cs
	VOCALOID2で使用される謎なエンコードが施されたファイル(*.vvd等)をデコードし、テキストファイルに変換するツール

diff2html.cs
	差分ファイルを色付のhtmlファイルに変換するツール

makeRes.cs
	画像などのリソースをBase64エンコードし、ソースコード内に埋め込むためのC#コードを自動生成するツール

parseBinFile.cs
	ファイルを様々な型(int, uint, ...etc.)の数値の羅列とみなし、その値をテキストファイルに出力するツール

splitDDB.cs
	VOCALOID2の音源データベースファイル(*.ddb)から、ファイル内のトップレベル・チャンク(FRM2, SND )を切り出すツール

extractEventHandlerAndResource.cs
	C#ソースコード中のInitializeComponentというメソッドから、各種イベントハンドラを追加している行と、リソースを追加している行をそれぞれ抽出するツール
