using System;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using cadencii;
using cadencii.media;
using cadencii.vsq;
using cadencii.java.util;

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class SmartHtml
{
    public static void Main(string[] args)
    {
        if (args.Length != 2) {
            return;
        }
        string indir = args[0];
        string outdir = args[1];
        if (!Directory.Exists(indir) || !Directory.Exists(outdir)) {
            Console.WriteLine("error; directory not exists");
            return;
        }

        List<string> files = new List<string>(Directory.GetFiles("*.htm"));
        files.AddRange(Directory.GetFiles("*.html"));
        foreach (string name in files) {
            string path = Path.Combine(indir, name);
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlTextWriter xtw = new XmlTextWriter(new FileStream(Path.Combine(outdir, name), FileMode.OpenOrCreate, FileAccess.Write), System.Text.Encoding.GetEncoding("Shift_JIS"));
            doc.WriteTo(xtw);
            xtw.Close();
        }
    }
}

/*public class Search {
    public static bool Edit( VsqFile vsq ) {
        int selectedid = -1;
        Form f;
        int track = AppManager.getSelected();
        bool begin_count = false;
        if ( AppManager.getSelectedEventCount() <= 0 ) {
            // 選択状態の音符がひとつも無い場合。
            // 曲の最初の音符から検索することにする
            for ( Iterator<VsqEvent> itr = vsq.Track[track].getNoteEventIterator(); itr.hasNext(); ) {
                VsqEvent item = itr.next();
                selectedid = item.InternalID;
                break;
            }

            if ( selectedid < 0 ) {
                // 音符が1つも配置されていない場合、何もせず戻る
                return true;
            }

            begin_count = true;
        } else {
            selectedid = AppManager.getLastSelectedEvent().original.InternalID;
        }
        for ( Iterator<VsqEvent> itr = vsq.Track[track].getNoteEventIterator(); itr.hasNext(); ) {
            VsqEvent item = itr.next();
            if ( item.InternalID == selectedid ) {
                begin_count = true;
                if ( AppManager.isSelectedEventContains( track, item.InternalID ) ) {
                    AppManager.removeSelectedEvent( item.InternalID );
                    continue;
                }
            }
            if ( begin_count ) {
                if ( item.ID.type == VsqIDType.Anote ) {
                    if ( item.ID.Length < 240 ) {
                        AppManager.addSelectedEvent( item.InternalID );
                        AppManager.setCurrentClock( item.Clock );
                        AppManager.mMainWindow.ensureCursorVisible();
                        break;
                    }
                }
            }
        }
        return true;
    }
}*/

public static class AutoBRI
{
    public static bool Edit(cadencii.vsq.VsqFile vsq)
    {
        // 選択されているアイテム（のInternalID）をリストアップ
        System.Collections.Generic.List<int> ids = new System.Collections.Generic.List<int>();
        foreach (var entry in AppManager.itemSelection.getEventIterator()) {
            ids.Add(entry.original.InternalID);
        }

        cadencii.vsq.VsqTrack track = vsq.Track[AppManager.getSelected()];

        // コントロールカーブの時間方向の解像度を，Cadenciiの設定値から取得
        int resol = AppManager.editorConfig.getControlCurveResolutionValue();
        for (int i = 0; i < ids.Count; i++) {
            int internal_id = ids[i];

            foreach (var item in track.getNoteEventIterator()) {
                // 指定されたInternalIDと同じなら，編集する
                if (item.InternalID == internal_id) {
                    // Brightnessカーブを取得
                    cadencii.vsq.VsqBPList bri = track.getCurve("BRI");

                    // 音符の最後の位置でのBRIを取得．処理の最後で追加
                    int value_at_end = bri.getValue(item.Clock + item.ID.Length);

                    // これから編集しようとしている範囲にすでに値がある場合，邪魔なので削除する
                    bri.keyClockIterator()
                        .Where((clock) => item.Clock <= clock && clock <= item.Clock + item.ID.Length)
                        .ToList()
                        .ForEach((clock) => bri.remove(clock));

                    // 直前に指定したBRI値．最初はありえない値にしておく
                    int last_v = -1;

                    // 時間方向解像度（resol）ごとのクロックに対して，順次BRIを設定
                    for (int clock = item.Clock; clock <= item.Clock + item.ID.Length; clock += resol) {
                        // BRIを取得．x=0が音符の先頭，x=1が音符の末尾．getCurve関数は，この仕様を満たすようにBRIを返すように，お好みで定義
                        float x = (clock - item.Clock) / (float)item.ID.Length;
                        int v = getCurve(x);

                        if (last_v != v) {
                            // 直前に指定した値と違うときだけ追加．
                            bri.add(clock, v);
                        }

                        // 「直前の値」を更新
                        last_v = v;
                    }

                    // 音符末尾の位置のBRIを強制的に元の値に戻す．これをやらないと，
                    // その音符の末尾以降のBRIがそのまま編集の影響を受けてしまう
                    bri.add(item.Clock + item.ID.Length, value_at_end);
                    break;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// 音符の先頭からの位置(x)におけるBRI値を計算する．音符の先頭がx=0，音符の末尾がx=1に対応する
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private static int getCurve(float x)
    {
        return 64;
    }
}

public class AutoBRITool : IPaletteTool
{
    /// <summary>
    /// インターフェースIPaletteToolのメンバー
    /// </summary>
    /// <param name="track">編集対象のトラック</param>
    /// <param name="manager">Cadenciiのマネージャ</param>
    /// <param name="ids">クリックされたイベントのInternalIDが格納された配列</param>
    /// <param name="button">クリックされたときのマウスボタン</param>
    /// <returns></returns>
    public bool edit(cadencii.vsq.VsqTrack track, int[] ids, System.Windows.Forms.MouseButtons button)
    {
        // コントロールカーブの時間方向の解像度を，Cadenciiの設定値から取得
        int resol = AppManager.editorConfig.getControlCurveResolutionValue();
        for (int i = 0; i < ids.Length; i++) {
            int internal_id = ids[i];

            foreach (var item in track.getNoteEventIterator()) {
                // 指定されたInternalIDと同じなら，編集する
                if (item.InternalID == internal_id) {
                    // Brightnessカーブを取得
                    cadencii.vsq.VsqBPList bri = track.getCurve("BRI");

                    // 音符の最後の位置でのBRIを取得．処理の最後で追加
                    int value_at_end = bri.getValue(item.Clock + item.ID.Length);

                    // これから編集しようとしている範囲にすでに値がある場合，邪魔なので削除する
                    bri.keyClockIterator()
                        .Where((clock) => item.Clock <= clock && clock <= item.Clock + item.ID.Length)
                        .ToList()
                        .ForEach((clock) => bri.remove(clock));

                    // 直前に指定したBRI値．最初はありえない値にしておく
                    int last_v = -1;

                    // 時間方向解像度（resol）ごとのクロックに対して，順次BRIを設定
                    for (int clock = item.Clock; clock <= item.Clock + item.ID.Length; clock += resol) {
                        // BRIを取得．x=0が音符の先頭，x=1が音符の末尾．getCurve関数は，この仕様を満たすようにBRIを返すように，お好みで定義
                        float x = (clock - item.Clock) / (float)item.ID.Length;
                        int v = getCurve(x);

                        if (last_v != v) {
                            // 直前に指定した値と違うときだけ追加．
                            bri.add(clock, v);
                        }

                        // 「直前の値」を更新
                        last_v = v;
                    }

                    // 音符末尾の位置のBRIを強制的に元の値に戻す．これをやらないと，
                    // その音符の末尾以降のBRIがそのまま編集の影響を受けてしまう
                    bri.add(item.Clock + item.ID.Length, value_at_end);
                    break;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// 音符の先頭からの位置(x)におけるBRI値を計算する．音符の先頭がx=0，音符の末尾がx=1に対応する
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private int getCurve(float x)
    {
        return 64;
    }

    /// <summary>
    /// IPaletteToolのメンバー．このパレットツールの名前を返す．
    /// </summary>
    /// <param name="lang"></param>
    /// <returns></returns>
    public string getName(string lang)
    {
        return "Auto BRI";
    }

    /// <summary>
    /// IPaletteToolのメンバー．このパレットツールの概要を返す．
    /// </summary>
    /// <param name="lang"></param>
    /// <returns></returns>
    public string getDescription(string lang)
    {
        return "edit BRI automatically";
    }

    /// <summary>
    /// IPaletteToolのメンバー．このパレットツールが設定ダイアログを持っているかどうかを表すbool値を返す．
    /// </summary>
    /// <returns></returns>
    public bool hasDialog()
    {
        return false;
    }

    /// <summary>
    /// IPaletteToolのメンバー．このパレットツールの設定ダイアログを開き，ダイアログの結果を返す．
    /// </summary>
    /// <returns></returns>
    public System.Windows.Forms.DialogResult openDialog()
    {
        return System.Windows.Forms.DialogResult.Cancel;
    }

    /// <summary>
    /// IPaletteToolのメンバー．このパレットツールのUIを，指定した言語に変更する．
    /// </summary>
    /// <param name="lang"></param>
    public void applyLanguage(string lang)
    {

    }

    /// <summary>
    /// IPaletteToolのメンバー．このパレットツールのアイコンを返す．
    /// </summary>
    /// <returns></returns>
    public System.Drawing.Bitmap getIcon()
    {
        return null;
    }
}


public static class SaveMetaText
{
    public static bool Edit(cadencii.vsq.VsqFile vsq)
    {
        vsq.Track[1].printMetaText(@"c:\meta_text.txt", "Shift_JIS");
        return true;
    }
}

public static class PrintLyric
{
    public static bool Edit(cadencii.vsq.VsqFile Vsq)
    {
        System.IO.StreamWriter sw = null;
        try {
            sw = new System.IO.StreamWriter(@"c:\lyrics.txt");
            foreach (var item in Vsq.Track[1].getNoteEventIterator()) {
                int clStart = item.Clock;
                int clEnd = clStart + item.ID.Length;
                double secStart = Vsq.getSecFromClock(clStart);
                double secEnd = Vsq.getSecFromClock(clEnd);
                sw.WriteLine(secStart + "\t" + secEnd + "\t" + item.ID.LyricHandle.L0.Phrase + "\t" + item.ID.LyricHandle.L0.PhoneticSymbol);
            }
        } catch {
            return false;
        } finally {
            if (sw != null) {
                sw.Close();
            }
        }
        return true;
    }
}

public static class UpHalfStep
{
    public static bool Edit(cadencii.vsq.VsqFile Vsq)
    {
        for (int i = 1; i < Vsq.Track.Count; i++) {
            foreach (var item in Vsq.Track[i].getNoteEventIterator()) {
                if (item.ID.Note < 127) {
                    item.ID.Note++;
                }
            }
        }
        return true;
    }
}

public static class Trim32
{
    public static bool Edit(cadencii.vsq.VsqFile Vsq)
    {
        for (int i = 1; i < Vsq.Track.Count; i++) {
            foreach (var item in Vsq.Track[i].getNoteEventIterator()) {
                // 32分音符の長さは，クロック数に直すと60クロック
                if (item.ID.Length > 60) {
                    item.ID.Length -= 60;
                }
            }
        }
        return true;
    }
}
