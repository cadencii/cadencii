public static class Trim_Suffix_32nd
{
    public static bool Edit(cadencii.vsq.VsqFile Vsq)
    {
        for (int i = 1; i < Vsq.Track.Count; i++) {
            for (int j = 0; j < Vsq.Track[i].getEventCount(); j++) {
                cadencii.vsq.VsqEvent item = Vsq.Track[i].getEvent(j);
                if (item.ID.type == cadencii.vsq.VsqIDType.Anote) {
                    // 32分音符の長さは，クロック数に直すと60クロック
                    if (item.ID.Length > 60) {
                        item.ID.Length -= 60;
                    }
                }
            }
        }
        return true;
    }
}