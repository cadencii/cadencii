/*
 * VsqBPList.cs
 * Copyright © 2008-2011 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using cadencii;
using cadencii.java.io;
using cadencii.java.util;

namespace cadencii.vsq
{

    /// <summary>
    /// コントロールカーブのデータ点リスト
    /// </summary>
    [Serializable]
    public class VsqBPList : ICloneable
    {
        private int[] clocks;
        private VsqBPPair[] items;
        private int length = 0; // clocks, itemsに入っているアイテムの個数
        private int defaultValue = 0;
        private int maxValue = 127;
        private int minValue = 0;
        private long maxId = 0;
        private string name = "";

        const int INIT_BUFLEN = 512;

        class KeyClockIterator : IEnumerator<int>, IEnumerable<int>
        {
            private VsqBPList m_list;
            private int m_pos;

            public KeyClockIterator(VsqBPList list)
            {
                m_list = list;
                Reset();
            }

            public void Dispose() { }

            public int Current
            {
                get
                {
                    return m_list.clocks[m_pos];
                }
            }

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }

            public bool MoveNext()
            {
                m_pos++;
                return m_pos + 1 < m_list.length;
            }

            public void Reset()
            {
                m_pos = -1;
            }

            public IEnumerator<int> GetEnumerator()
            {
                return this;
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this;
            }
        }

        public VsqBPList()
            :
 this("", 0, 0, 64)
        {
        }

        /// <summary>
        /// コンストラクタ。デフォルト値はココで指定する。
        /// </summary>
        /// <param name="default_value"></param>
        public VsqBPList(string name, int default_value, int minimum, int maximum)
        {
            this.name = name;
            defaultValue = default_value;
            maxValue = maximum;
            minValue = minimum;
            maxId = 0;
        }

        private void ensureBufferLength(int length)
        {
            if (clocks == null) {
                clocks = new int[INIT_BUFLEN];
            }
            if (items == null) {
                items = new VsqBPPair[INIT_BUFLEN];
            }
            if (length > clocks.Length) {
                int newLength = length;
                if (this.length <= 0) {
                    newLength = (int)(length * 1.2);
                } else {
                    int order = length / clocks.Length;
                    if (order <= 1) {
                        order = 2;
                    }
                    newLength = clocks.Length * order;
                }
                Array.Resize(ref clocks, newLength);
                Array.Resize(ref items, newLength);
            }
        }

        public int Default
        {
            get
            {
                return getDefault();
            }
            set
            {
                setDefault(value);
            }
        }

        public string getName()
        {
            if (name == null) {
                name = "";
            }
            return name;
        }

        public void setName(string value)
        {
            if (value == null) {
                name = "";
            } else {
                name = value;
            }
        }

        public string Name
        {
            get
            {
                return getName();
            }
            set
            {
                setName(value);
            }
        }

        public long getMaxID()
        {
            return maxId;
        }

        /// <summary>
        /// このBPListのデフォルト値を取得します
        /// </summary>
        public int getDefault()
        {
            return defaultValue;
        }

        public void setDefault(int value)
        {
            defaultValue = value;
        }

        /// <summary>
        /// データ点のIDを一度クリアし，新たに番号付けを行います．
        /// IDは，Redo,Undo用コマンドが使用するため，このメソッドを呼ぶとRedo,Undo操作が破綻する．XMLからのデシリアライズ直後のみ使用するべき．
        /// </summary>
        public void renumberIDs()
        {
            maxId = 0;
            for (int i = 0; i < length; i++) {
                maxId++;
                VsqBPPair v = items[i];
                v.id = maxId;
                items[i] = v;
            }
        }

        /// <summary>
        /// XMLシリアライズ用
        /// </summary>
        public string Data
        {
            get
            {
                return getData();
            }
            set
            {
                setData(value);
            }
        }

        public string getData()
        {
            StringBuilder ret = new StringBuilder();
            for (int i = 0; i < length; i++) {
                ret.Append((i == 0 ? "" : ",") + clocks[i] + "=" + items[i].value);
            }
            return ret.ToString();
        }

        public void setData(string value)
        {
            length = 0;
            maxId = 0;
            string[] spl = PortUtil.splitString(value, ',');
            for (int i = 0; i < spl.Length; i++) {
                string[] spl2 = PortUtil.splitString(spl[i], '=');
                if (spl2.Length < 2) {
                    continue;
                }
                try {
                    int clock = int.Parse(spl2[0]);
                    ensureBufferLength(length + 1);
                    clocks[length] = clock;
                    items[length] = new VsqBPPair(int.Parse(spl2[1]), maxId + 1);
                    maxId++;
                    length++;
                } catch (Exception ex) {
                    serr.println("VsqBPList#setData; ex=" + ex);
#if DEBUG
                    sout.println("    i=" + i + "; spl2[0]=" + spl2[0] + "; spl2[1]=" + spl2[1]);
#endif
                }
            }
        }

        /// <summary>
        /// このVsqBPListの同一コピーを作成します
        /// </summary>
        /// <returns></returns>
        public Object clone()
        {
            VsqBPList res = new VsqBPList(name, defaultValue, minValue, maxValue);
            res.ensureBufferLength(length);
            for (int i = 0; i < length; i++) {
                res.clocks[i] = clocks[i];
                res.items[i] = items[i];
            }
            res.length = length;
            res.maxId = maxId;
            return res;
        }

        public object Clone()
        {
            return clone();
        }

        public int Maximum
        {
            get
            {
                return getMaximum();
            }
            set
            {
                setMaximum(value);
            }
        }

        /// <summary>
        /// このリストに設定された最大値を取得します。
        /// </summary>
        public int getMaximum()
        {
            return maxValue;
        }

        public void setMaximum(int value)
        {
            maxValue = value;
        }

        public int Minimum
        {
            get
            {
                return getMinimum();
            }
            set
            {
                setMinimum(value);
            }
        }

        /// <summary>
        /// このリストに設定された最小値を取得します
        /// </summary>
        public int getMinimum()
        {
            return minValue;
        }

        public void setMinimum(int value)
        {
            minValue = value;
        }

        public void remove(int clock)
        {
            ensureBufferLength(length);
            int index = findIndexFromClock(clock);
            removeElementAt(index);
        }

        public void removeElementAt(int index)
        {
            if (index >= 0) {
                for (int i = index; i < length - 1; i++) {
                    clocks[i] = clocks[i + 1];
                    items[i] = items[i + 1];
                }
                length--;
            }
        }

        public bool isContainsKey(int clock)
        {
            ensureBufferLength(length);
            return (findIndexFromClock(clock) >= 0);
        }

        /// <summary>
        /// 時刻clockのデータを時刻new_clockに移動します。
        /// 時刻clockにデータがなければ何もしない。
        /// 時刻new_clockに既にデータがある場合、既存のデータは削除される。
        /// </summary>
        /// <param name="clock"></param>
        /// <param name="new_clock"></param>
        public void move(int clock, int new_clock, int new_value)
        {
            ensureBufferLength(length);
            int index = findIndexFromClock(clock);
            if (index < 0) {
                return;
            }
            VsqBPPair item = items[index];
            for (int i = index; i < length - 1; i++) {
                clocks[i] = clocks[i + 1];
                items[i] = items[i + 1];
            }
            length--;
            int index_new = findIndexFromClock(new_clock);
            if (index_new >= 0) {
                item.value = new_value;
                items[index_new] = item;
                return;
            } else {
                length++;
                ensureBufferLength(length);
                clocks[length - 1] = new_clock;
                Array.Sort(clocks, 0, length);
                index_new = findIndexFromClock(new_clock);
                item.value = new_value;
                for (int i = length - 1; i > index_new; i--) {
                    items[i] = items[i - 1];
                }
                items[index_new] = item;
            }
        }

        public void clear()
        {
            length = 0;
        }

        public int getElement(int index)
        {
            return getElementA(index);
        }

        public int getElementA(int index)
        {
            return items[index].value;
        }

        public VsqBPPair getElementB(int index)
        {
            return items[index];
        }

        public int getKeyClock(int index)
        {
            return clocks[index];
        }

        public int findValueFromID(long id)
        {
            for (int i = 0; i < length; i++) {
                VsqBPPair item = items[i];
                if (item.id == id) {
                    return item.value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 指定したid値を持つVsqBPPairを検索し、その結果を返します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VsqBPPairSearchContext findElement(long id)
        {
            VsqBPPairSearchContext context = new VsqBPPairSearchContext();
            for (int i = 0; i < length; i++) {
                VsqBPPair item = items[i];
                if (item.id == id) {
                    context.clock = clocks[i];
                    context.index = i;
                    context.point = item;
                    return context;
                }
            }
            context.clock = -1;
            context.index = -1;
            context.point = new VsqBPPair(defaultValue, -1);
            return context;
        }

        public void setValueForID(long id, int value)
        {
            for (int i = 0; i < length; i++) {
                VsqBPPair item = items[i];
                if (item.id == id) {
                    item.value = value;
                    items[i] = item;
                    break;
                }
            }
        }

        public int getValue(int clock, ByRef<int> index)
        {
            if (length == 0) {
                return defaultValue;
            } else {
                if (index.value < 0) {
                    index.value = 0;
                }
                for (int i = index.value; i < length; i++) {
                    int keyclock = clocks[i];
                    if (clock < keyclock) {
                        if (i > 0) {
                            index.value = i - 1;
                            return items[i - 1].value;
                        } else {
                            index.value = i - 1;
                            return defaultValue;
                        }
                    }
                }
                index.value = length - 1;
                return items[length - 1].value;
            }
        }

        private void printCor(ITextWriter writer, int start_clock, string header)
        {
            writer.writeLine(header);
            int lastvalue = defaultValue;
            bool value_at_start_written = false;
            for (int i = 0; i < length; i++) {
                int key = clocks[i];
                if (start_clock == key) {
                    writer.writeLine(key + "=" + items[i].value);
                    value_at_start_written = true;
                } else if (start_clock < key) {
                    if (!value_at_start_written && lastvalue != defaultValue) {
                        writer.writeLine(start_clock + "=" + lastvalue);
                        value_at_start_written = true;
                    }
                    int val = items[i].value;
                    writer.writeLine(key + "=" + val);
                } else {
                    lastvalue = items[i].value;
                }
            }
            if (!value_at_start_written && lastvalue != defaultValue) {
                writer.writeLine(start_clock + "=" + lastvalue);
            }
        }

        /// <summary>
        /// このBPListの内容をテキストファイルに書き出します
        /// </summary>
        /// <param name="writer"></param>
        public void print(StreamWriter writer, int start, string header)
        {
            printCor(new WrappedStreamWriter(writer), start, header);
        }

        /// <summary>
        /// このBPListの内容をテキストファイルに書き出します
        /// </summary>
        /// <param name="writer"></param>
        public void print(ITextWriter writer, int start, string header)
        {
            printCor(writer, start, header);
        }

        /// <summary>
        /// テキストファイルからデータ点を読込み、現在のリストに追加します
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public string appendFromText(TextStream reader)
        {
#if DEBUG
            sout.println("VsqBPList#appendFromText; start");
            double started = PortUtil.getCurrentTime();
            int count = 0;
#endif
            int clock = 0;
            int value = 0;
            int minus = 1;
            int mode = 0; // 0: clockを読んでいる, 1: valueを読んでいる
            while (reader.ready()) {
                char ch = reader.get();
                if (ch == '\n') {
                    if (mode == 1) {
                        addWithoutSort(clock, value * minus);
                        mode = 0;
                        clock = 0;
                        value = 0;
                        minus = 1;
                    }
                    continue;
                }
                if (ch == '[') {
                    if (mode == 1) {
                        addWithoutSort(clock, value * minus);
                        mode = 0;
                        clock = 0;
                        value = 0;
                        minus = 1;
                    }
                    reader.setPointer(reader.getPointer() - 1);
                    break;
                }
                if (ch == '=') {
                    mode = 1;
                    continue;
                }
                if (ch == '-') {
                    minus = -1;
                    continue;
                }
                if (Char.IsNumber(ch)) {
                    int num = 0;
                    if (ch == '1') {
                        num = 1;
                    } else if (ch == '2') {
                        num = 2;
                    } else if (ch == '3') {
                        num = 3;
                    } else if (ch == '4') {
                        num = 4;
                    } else if (ch == '5') {
                        num = 5;
                    } else if (ch == '6') {
                        num = 6;
                    } else if (ch == '7') {
                        num = 7;
                    } else if (ch == '8') {
                        num = 8;
                    } else if (ch == '9') {
                        num = 9;
                    }
                    if (mode == 0) {
                        clock = clock * 10 + num;
                    } else {
                        value = value * 10 + num;
                    }
                }
            }
            return reader.readLine();
        }

        public int size()
        {
            return length;
        }

        public IEnumerable<int> keyClockIterator()
        {
            return new KeyClockIterator(this);
        }

        /// <summary>
        /// 指定した時刻にあるデータ点のインデックスを検索します．
        /// 見つかればインデックスを，そうでなければ-1を返します．
        /// </summary>
        /// <param name="clock"></param>
        /// <returns></returns>
        public int findIndexFromClock(int value)
        {
            return Array.BinarySearch(clocks, 0, length, value);
            //return Array.IndexOf( clocks, value, 0, length );
        }

        /// <summary>
        /// 並べ替え，既存の値との重複チェックを行わず，リストの末尾にデータ点を追加する
        /// </summary>
        /// <param name="clock"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private void addWithoutSort(int clock, int value)
        {
            ensureBufferLength(length + 1);
            clocks[length] = clock;
            maxId++;
            items[length].value = value;
            items[length].id = maxId;
            length++;
        }

        public long add(int clock, int value)
        {
            ensureBufferLength(length);
            int index = findIndexFromClock(clock);
            if (index >= 0) {
                VsqBPPair v = items[index];
                v.value = value;
                items[index] = v;
                return v.id;
            } else {
                length++;
                ensureBufferLength(length);
                clocks[length - 1] = clock;
                Array.Sort(clocks, 0, length);
                index = findIndexFromClock(clock);
                maxId++;
                for (int i = length - 1; i > index; i--) {
                    items[i] = items[i - 1];
                }
                items[index] = new VsqBPPair(value, maxId);
                return maxId;
            }
        }

        public void addWithID(int clock, int value, long id)
        {
            ensureBufferLength(length);
            int index = findIndexFromClock(clock);
            if (index >= 0) {
                VsqBPPair v = items[index];
                v.value = value;
                v.id = id;
                items[index] = v;
            } else {
                length++;
                ensureBufferLength(length);
                clocks[length - 1] = clock;
                Array.Sort(clocks, 0, length);
                index = findIndexFromClock(clock);
                for (int i = length - 1; i > index; i--) {
                    items[i] = items[i - 1];
                }
                items[index] = new VsqBPPair(value, id);
                maxId = Math.Max(maxId, id);
            }
        }

        public void removeWithID(long id)
        {
            for (int i = 0; i < length; i++) {
                if (items[i].id == id) {
                    for (int j = i; j < length - 1; j++) {
                        items[j] = items[j + 1];
                        clocks[j] = clocks[j + 1];
                    }
                    length--;
                    break;
                }
            }
        }

        public int getValue(int clock)
        {
            ensureBufferLength(length);
            int index = findIndexFromClock(clock);
            if (index >= 0) {
                return items[index].value;
            } else {
                if (length <= 0) {
                    return defaultValue;
                } else {
                    int draft = -1;
                    for (int i = 0; i < length; i++) {
                        int c = clocks[i];
                        if (clock < c) {
                            break;
                        }
                        draft = i;
                    }
                    if (draft < 0) {
                        return defaultValue;
                    } else {
                        return items[draft].value;
                    }
                }
            }
        }
    }

}
