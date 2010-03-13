using org.kbinani.cadencii;

public class RunOnce {
    private static int runCount = 0;
    
    public static ScriptReturnStatus Edit( VsqFileEx vsq ) {
        if ( runCount != 0 ) {
            return ScriptReturnStatus.NOT_EDITED;
        }
        runCount++;

        // 以下に，起動時に変更するパラメータを記述する

        // ピアノロールに合成システムの名称をオーバーレイ表示するかどうか
        AppManager.drawOverSynthNameOnPianoroll = false;
        // 再生中に，WAVE波形の描画をスキップするかどうか
        AppManager.skipDrawingWaveformWhenPlaying = true;
        // 起動時のツール．デフォルトはEditTool.PENCIL
        AppManager.setSelectedTool( EditTool.PENCIL );

        return ScriptReturnStatus.NOT_EDITED;
    }
}
