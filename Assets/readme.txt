★確認済みのバグ/不具合
	ダンジョン生成時の書き換えの回数が3の場合に予期せぬ回転角のエラーが出る。
	ダンジョンの見た目に影響は出ない。
	リファクタリング前は出なかったバグ。

	ダンジョン生成時の書き換えの回数が4以上の場合に直線通路の回転がおかしい箇所が出る。
	Cornerの形状に挟まれた位置の通路がおかしくなる？
	生成した後に全ての通路をFixPassSetに挿入してみたが直らなかった。

	ダンジョン生成時のアニメーションでTween数のエラーが出る。
	ダンジョンのプレハブに敵とお宝を追加したため？
	アセットの数を減らす事で対処可能だと思われる
	OutOfRangeExeption

	発見時のアニメーションが反対を向いて再生される不具合

	発見時のアニメーションをした箇所とは全然別のターゲットに向かって走っていく不具合

	ダンジョン生成の前にawaitを挟むと1フレームawaitではgrid生成のRayが正常に反応しない不具合
	生成アニメーション分待機すれば正常に反応するのだが、だとしたらなぜawaitを挟まない場合は
	1フレーム待機できちんとRayが反応するのか？

	リザルトUIの背景とボタンがプレイ中に入れ替わる。
	そのせいでリトライボタンがクリック出来ない、原因不明。

★備考
	★ゲームのタイトル
		いま○○できないなら絶対に○○しないでください。
		ダンジョンが難しすぎてほぼ100％その場でやられてしまいます。
	★セーブ＆ロード
        優先度が低い、時間が余ればやる
    ★プレイヤーのできる操作
        キーボードは使用しない、マウスのみ
        タイトル/リザルトだとUIボタンのクリックのみ
        インゲームではボタンをクリックして罠を選択状態
        罠を選択状態で任意の個所をクリックで罠設置
        罠設置後には選択状態を解除
        罠選択中に他のボタンをクリックするとその罠を選択できる
    ★ゲームオーバー時の処理
        罠選択中なら選択を解除する

★概要
	クリッカーゲー
	セーブロード対応(出来れば)
	1つのシーンでゲームを制御する
	昼夜がある
	操作はマウスオンリー
	一定間隔で冒険者がダンジョンにやってくる

★アセット&ライブラリ
	Hierarchy2
	UniRx
	UniTask
	Dotween
	MessagePipe
	VContainer
	ZString

    2DCasualUIHD

★外部素材

