// ローマ字→ひらがな一括変換スクリプト for Cadencii 3.1.4

using org.kbinani.vsq;

public static class Roman2Hiragana
{
    public static bool Edit(VsqFile Vsq)
    {
        for (int i = 1; i < Vsq.Track.size(); i++) {
            for (int j = 0; j < Vsq.Track.get(i).getEventCount(); j++) {
                VsqEvent item = Vsq.Track.get(i).getEvent(j);
                if (item.ID.type == VsqIDType.Anote) {
                    item.ID.LyricHandle.L0.Phrase = KanaDeRomanization.Attach(item.ID.LyricHandle.L0.Phrase);
                }
            }
        }
        return true;
    }
}
