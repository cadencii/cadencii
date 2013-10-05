/*
 * EditHistoryModel.cs
 * Copyright © 2011 kbinani
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
using System.Collections.Generic;
using cadencii.java.util;

namespace cadencii
{

    /// <summary>
    /// 編集操作の履歴を管理するModel
    /// </summary>
    public class EditHistoryModel
    {
        private static List<ICommand> mCommands = new List<ICommand>();
        private static int mCommandIndex = -1;

        /// <summary>
        /// ヒストリーに編集履歴を登録する
        /// </summary>
        /// <param name="command">登録する履歴</param>
        public void register(ICommand command)
        {
            if (mCommandIndex == mCommands.Count - 1) {
                // 新しいコマンドバッファを追加する場合
                mCommands.Add(command);
                mCommandIndex = mCommands.Count - 1;
            } else {
                // 既にあるコマンドバッファを上書きする場合
                mCommands[mCommandIndex + 1] = command;
                for (int i = mCommands.Count - 1; i >= mCommandIndex + 2; i--) {
                    mCommands.RemoveAt(i);
                }
                mCommandIndex++;
            }
        }

        /// <summary>
        /// 編集履歴を消去する
        /// </summary>
        public void clear()
        {
            mCommands.Clear();
            mCommandIndex = -1;
        }

        /// <summary>
        /// UNDO用のヒストリーを取得できるかどうか調べる
        /// </summary>
        /// <returns>UNDO用のヒストリーを取得できる場合trueを，そうでなければfalseを返す</returns>
        public bool hasUndoHistory()
        {
            if (mCommands.Count > 0 && 0 <= mCommandIndex && mCommandIndex < mCommands.Count) {
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
            if (mCommands.Count > 0 && 0 <= mCommandIndex + 1 && mCommandIndex + 1 < mCommands.Count) {
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
            return mCommands[mCommandIndex];
        }

        /// <summary>
        /// REDO用のコマンドを取得する
        /// </summary>
        /// <returns></returns>
        public ICommand getRedo()
        {
            return mCommands[mCommandIndex + 1];
        }

        /// <summary>
        /// UNDO処理後に発生したコマンドを登録する
        /// </summary>
        /// <param name="command"></param>
        public void registerAfterUndo(ICommand command)
        {
            mCommands[mCommandIndex] = command;
            mCommandIndex--;
        }

        /// <summary>
        /// REDO処理後に発生したコマンドを登録する
        /// </summary>
        /// <param name="command"></param>
        public void registerAfterRedo(ICommand command)
        {
            mCommands[mCommandIndex + 1] = command;
            mCommandIndex++;
        }
    }

}
