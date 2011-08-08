#if JAVA

package org.kbinani.cadencii;

#else

namespace org
{
    namespace kbinani
    {
        namespace cadencii
        {

#endif

            /// <summary>
            /// メイン画面のコントローラ
            /// </summary>
            public class FormMainController : ControllerBase
            {
                /// <summary>
                /// x方向の表示倍率(pixel/clock)
                /// </summary>
                private float mScaleX;
                /// <summary>
                /// mScaleXの逆数
                /// </summary>
                private float mInvScaleX;

                public FormMainController()
                {
                    mScaleX = 0.1f;
                    mInvScaleX = 1.0f / mScaleX;
                }

                /// <summary>
                /// ピアノロールの，X方向のスケールを取得します(pixel/clock)
                /// </summary>
                /// <returns></returns>
                public float getScaleX()
                {
                    return mScaleX;
                }

                /// <summary>
                /// ピアノロールの，X方向のスケールの逆数を取得します(clock/pixel)
                /// </summary>
                /// <returns></returns>
                public float getScaleXInv()
                {
                    return mInvScaleX;
                }

                /// <summary>
                /// ピアノロールの，X方向のスケールを設定します
                /// </summary>
                /// <param name="scale_x"></param>
                public void setScaleX( float scale_x )
                {
                    mScaleX = scale_x;
                    mInvScaleX = 1.0f / mScaleX;
                }
            }

#if !JAVA
        }
    }
}
#endif
