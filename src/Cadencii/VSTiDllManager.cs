/*
 * VSTiDllManager.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using cadencii;
using cadencii.java.util;
using cadencii.media;
using cadencii.vsq;
using cadencii.java.io;



namespace cadencii
{

    /// <summary>
    /// VSTiのDLLを管理するクラス
    /// </summary>
    public static class VSTiDllManager
    {
        //public static int SAMPLE_RATE = 44100;
        const float a0 = -17317.563f;
        const float a1 = 86.7312112f;
        const float a2 = -0.237323499f;
        /// <summary>
        /// 使用するボカロの最大バージョン．2までリリースされているので今は2
        /// </summary>
        const int MAX_VOCALO_VERSION = 2;

#if ENABLE_VOCALOID
        public static List<VocaloidDriver> vocaloidDriver = new List<VocaloidDriver>();
#endif

#if ENABLE_AQUESTONE
        /// <summary>
        /// AquesTone VSTi のドライバ
        /// </summary>
        private static AquesToneDriver aquesToneDriver;
        /// <summary>
        /// AquesTone2 VSTi のドライバ
        /// </summary>
        private static AquesTone2Driver aquesTone2Driver;
#endif

        /// <summary>
        /// 指定した合成器の種類に合致する合成器の新しいインスタンスを取得します
        /// </summary>
        /// <param name="kind">合成器の種類</param>
        /// <returns>指定した種類の合成器の新しいインスタンス</returns>
        public static WaveGenerator getWaveGenerator(RendererKind kind)
        {
            if (kind == RendererKind.AQUES_TONE) {
#if ENABLE_AQUESTONE
                return new AquesToneWaveGenerator(getAquesToneDriver());
            } else if (kind == RendererKind.AQUES_TONE2) {
                return new AquesTone2WaveGenerator(getAquesTone2Driver());
#endif
            } else if (kind == RendererKind.VCNT) {
                return new VConnectWaveGenerator();
            } else if (kind == RendererKind.UTAU) {
                return new UtauWaveGenerator();
            } else if (kind == RendererKind.VOCALOID1 ||
                        kind == RendererKind.VOCALOID2) {
#if ENABLE_VOCALOID
                return new VocaloidWaveGenerator();
#endif
            }
            return new EmptyWaveGenerator();
        }

        public static void init()
        {
#if ENABLE_VOCALOID
            int default_dse_version = VocaloSysUtil.getDefaultDseVersion();
            string editor_dir = VocaloSysUtil.getEditorPath(SynthesizerType.VOCALOID1);
            string ini = "";
            if (!editor_dir.Equals("")) {
                ini = Path.Combine(PortUtil.getDirectoryName(editor_dir), "VOCALOID.ini");
            }
            string vocalo2_dll_path = VocaloSysUtil.getDllPathVsti(SynthesizerType.VOCALOID2);
            string vocalo1_dll_path = VocaloSysUtil.getDllPathVsti(SynthesizerType.VOCALOID1);
            if (vocalo2_dll_path != "" &&
                    System.IO.File.Exists(vocalo2_dll_path) &&
                    !AppManager.editorConfig.DoNotUseVocaloid2) {
                VocaloidDriver vr = new VocaloidDriver(RendererKind.VOCALOID2);
                vr.path = vocalo2_dll_path;
                vr.loaded = false;
                vocaloidDriver.Add(vr);
            }
            if (vocalo1_dll_path != "" &&
                    System.IO.File.Exists(vocalo1_dll_path) &&
                    !AppManager.editorConfig.DoNotUseVocaloid1) {
                VocaloidDriver vr = new VocaloidDriver(RendererKind.VOCALOID1);
                vr.path = vocalo1_dll_path;
                vr.loaded = false;
                vocaloidDriver.Add(vr);
            }

            for (int i = 0; i < vocaloidDriver.Count; i++) {
                string dll_path = vocaloidDriver[i].path;
                bool loaded = false;
                try {
                    if (dll_path != "") {
                        // 読込み。
                        loaded = vocaloidDriver[i].open(44100, 44100);
                    } else {
                        loaded = false;
                    }
                    vocaloidDriver[i].loaded = loaded;
                } catch (Exception ex) {
                    serr.println("VSTiProxy#initCor; ex=" + ex);
                }
            }
#endif // ENABLE_VOCALOID

#if ENABLE_AQUESTONE
            reloadAquesTone();
            reloadAquesTone2();
#endif
        }

        /// <summary>
        /// 初期化した AquesTone ドライバを取得する
        /// </summary>
        /// <returns></returns>
        public static AquesToneDriver getAquesToneDriver()
        {
            string path = AppManager.editorConfig.PathAquesTone;
            if (aquesToneDriver == null && !AppManager.editorConfig.DoNotUseAquesTone && System.IO.File.Exists(path)) {
                aquesToneDriver = new AquesToneDriver(path);
            }
            return aquesToneDriver;
        }

        /// <summary>
        /// 初期化した AquesTone2 ドライバを取得する
        /// </summary>
        /// <returns></returns>
        public static AquesTone2Driver getAquesTone2Driver()
        {
            string path = AppManager.editorConfig.PathAquesTone2;
            if (aquesTone2Driver == null && !AppManager.editorConfig.DoNotUseAquesTone2 && System.IO.File.Exists(path)) {
                aquesTone2Driver = new AquesTone2Driver(path);
                if (AppManager.mMainWindow != null) {
                    // AquesTone2 は UI のインスタンスを生成してからでないと、合成時にクラッシュする。
                    // これを回避するため、UI インスタンスの生成をココで行う。
                    // Cadencii 起動時にも同様の処理が必要だが、これは Cadencii::mainWindow_Load ハンドラで行う。
                    aquesTone2Driver.getUi(AppManager.mMainWindow);
                }
            }
            return aquesTone2Driver;
        }

#if ENABLE_AQUESTONE
        public static void reloadAquesTone()
        {
            if (aquesToneDriver != null) {
                aquesToneDriver.close();
                aquesToneDriver = null;
            }
            aquesToneDriver = getAquesToneDriver();
        }

        public static void reloadAquesTone2()
        {
            if (aquesTone2Driver != null) {
                aquesTone2Driver.close();
                aquesTone2Driver = null;
            }
            aquesTone2Driver = getAquesTone2Driver();
        }
#endif

        public static bool isRendererAvailable(RendererKind renderer)
        {
#if ENABLE_VOCALOID
            for (int i = 0; i < vocaloidDriver.Count; i++) {
                if (renderer == vocaloidDriver[i].getRendererKind() && vocaloidDriver[i].loaded) {
                    return true;
                }
            }
#endif // ENABLE_VOCALOID

#if ENABLE_AQUESTONE
            if (renderer == RendererKind.AQUES_TONE && aquesToneDriver != null && aquesToneDriver.loaded) {
                return true;
            }
            if (renderer == RendererKind.AQUES_TONE2 && aquesTone2Driver != null && aquesTone2Driver.loaded) {
                return true;
            }
#endif

            if (renderer == RendererKind.UTAU) {
                // ここでは，resamplerの内どれかひとつでも使用可能であればOKの判定にする
                bool resampler_exists = false;
                int size = AppManager.editorConfig.getResamplerCount();
                for (int i = 0; i < size; i++) {
                    string path = AppManager.editorConfig.getResamplerAt(i);
                    if (System.IO.File.Exists(path)) {
                        resampler_exists = true;
                        break;
                    }
                }
                if (resampler_exists &&
                     !AppManager.editorConfig.PathWavtool.Equals("") && System.IO.File.Exists(AppManager.editorConfig.PathWavtool)) {
                    if (AppManager.editorConfig.UtauSingers.Count > 0) {
                        return true;
                    }
                }
            }
            if (renderer == RendererKind.VCNT) {
                string synth_path = Path.Combine(PortUtil.getApplicationStartupPath(), VConnectWaveGenerator.STRAIGHT_SYNTH);
                if (System.IO.File.Exists(synth_path)) {
                    int count = AppManager.editorConfig.UtauSingers.Count;
                    if (count > 0) {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void terminate()
        {
#if ENABLE_VOCALOID
            for (int i = 0; i < vocaloidDriver.Count; i++) {
                if (vocaloidDriver[i] != null) {
                    vocaloidDriver[i].close();
                }
            }
            vocaloidDriver.Clear();
#endif // !ENABLE_VOCALOID

#if ENABLE_AQUESTONE
            if (aquesToneDriver != null) { aquesToneDriver.close(); }
            if (aquesTone2Driver != null) { aquesTone2Driver.close(); }
#endif
        }

        public static int getErrorSamples(float tempo)
        {
            if (tempo <= 240) {
                return 4666;
            } else {
                float x = tempo - 240;
                return (int)((a2 * x + a1) * x + a0);
            }
        }

        public static float getPlayTime()
        {
            double pos = PlaySound.getPosition();
            return (float)pos;
        }
    }

}
