/*
 * EditHistory.cs
 * Copyright © 2011 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA

package org.kbinani.cadencii;

import java.util.*;

#else

namespace org
{
    namespace kbinani
    {
        namespace cadencii
        {

#endif

            /// <summary>
            /// 編集操作の履歴を管理するModel
            /// </summary>
            public class EditHistoryModel
            {
                private static Vector<ICommand> mCommands = new Vector<ICommand>();
                private static int mCommandIndex = -1;

                /// <summary>
                /// ヒストリーに編集履歴を登録する
                /// </summary>
                /// <param name="command">登録する履歴</param>
                public void register( ICommand command )
                {
                    if( mCommandIndex == mCommands.size() - 1 ) {
                        // 新しいコマンドバッファを追加する場合
                        mCommands.add( command );
                        mCommandIndex = mCommands.size() - 1;
                    } else {
                        // 既にあるコマンドバッファを上書きする場合
                        mCommands.set( mCommandIndex + 1, command );
                        for( int i = mCommands.size() - 1; i >= mCommandIndex + 2; i-- ) {
                            mCommands.removeElementAt( i );
                        }
                        mCommandIndex++;
                    }
                }

                /// <summary>
                /// UNDO用のヒストリーを取得できるかどうか調べる
                /// </summary>
                /// <returns>UNDO用のヒストリーを取得できる場合trueを，そうでなければfalseを返す</returns>
                public bool hasUndoHistroy()
                {
                    if( mCommands.size() > 0 && 0 <= mCommandIndex && mCommandIndex < mCommands.size() ) {
                        return true;
                    } else {
                        return false;
                    }
                }

                /// <summary>
                /// REDO用のヒストリーを取得できるかどうか調べる
                /// </summary>
                /// <returns>REDO用のヒストリーを取得できる場合trueを，そうでなければfalseを返す</returns>
                public bool hasRedoHistory()
                {
                    if( mCommands.size() > 0 && 0 <= mCommandIndex + 1 && mCommandIndex + 1 < mCommands.size() ) {
                        return true;
                    } else {
                        return false;
                    }
                }

                /// <summary>
                /// UNDO用のコマンドを取得する
                /// </summary>
                /// <returns></returns>
                public ICommand getUndo()
                {
                    return mCommands.get( mCommandIndex );
                }

                /// <summary>
                /// REDO用のコマンドを取得する
                /// </summary>
                /// <returns></returns>
                public ICommand getRedo()
                {
                    return mCommands.get( mCommandIndex + 1 );
                }

                /// <summary>
                /// UNDO処理後に発生したコマンドを登録する
                /// </summary>
                /// <param name="command"></param>
                public void registerAfterUndo( ICommand command )
                {
                    mCommands.set( mCommandIndex, command );
                    mCommandIndex--;
                }

                /// <summary>
                /// REDO処理後に発生したコマンドを登録する
                /// </summary>
                /// <param name="command"></param>
                public void registerAfterRedo( ICommand command )
                {
                    mCommands.set( mCommandIndex + 1, command );
                    mCommandIndex++;
                }
            }

#if !JAVA
        }
    }
}
#endif
