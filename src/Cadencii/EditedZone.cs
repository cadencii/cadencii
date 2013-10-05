/*
 * EditedZone.cs
 * Copyright © 2010-2011 kbinani
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
using System.Collections.Generic;
using cadencii.java.util;

namespace cadencii
{

    public class EditedZone : ICloneable
    {
        private List<EditedZoneUnit> mSeries = new List<EditedZoneUnit>();

        public EditedZone()
        {
        }

        public int size()
        {
            return mSeries.Count;
        }

        public IEnumerable<EditedZoneUnit> iterator()
        {
            return mSeries;
        }

        public Object clone()
        {
            EditedZone ret = new EditedZone();
            int count = mSeries.Count;
            for (int i = 0; i < count; i++) {
                EditedZoneUnit p = mSeries[i];
                ret.mSeries.Add((EditedZoneUnit)p.clone());
            }
            return ret;
        }

        public EditedZoneCommand add(int start, int end)
        {
            EditedZoneCommand com = generateCommandAdd(start, end);
            return executeCommand(com);
        }

        public EditedZoneCommand add(EditedZoneUnit[] items)
        {
            EditedZoneCommand com = generateCommandAdd(items);
            return executeCommand(com);
        }

        public EditedZoneCommand executeCommand(EditedZoneCommand run)
        {
            foreach (var item in run.mRemove) {
                int count = mSeries.Count;
                for (int i = 0; i < count; i++) {
                    EditedZoneUnit item2 = mSeries[i];
                    if (item.mStart == item2.mStart && item.mEnd == item2.mEnd) {
                        mSeries.RemoveAt(i);
                        break;
                    }
                }
            }

            foreach (var item in run.mAdd) {
                mSeries.Add((EditedZoneUnit)item.clone());
            }

            mSeries.Sort();

            return new EditedZoneCommand(run.mRemove, run.mAdd);
        }

        private EditedZoneCommand generateCommandClear()
        {
            List<EditedZoneUnit> remove = new List<EditedZoneUnit>();
            foreach (var item in mSeries) {
                remove.Add((EditedZoneUnit)item.clone());
            }

            return new EditedZoneCommand(new List<EditedZoneUnit>(), remove);
        }

        private EditedZoneCommand generateCommandAdd(EditedZoneUnit[] areas)
        {
            EditedZone work = (EditedZone)clone();
            for (int i = 0; i < areas.Length; i++) {
                EditedZoneUnit item = areas[i];
                if (item == null) {
                    continue;
                }
                work.mSeries.Add(new EditedZoneUnit(item.mStart, item.mEnd));
            }
            work.normalize();

            // thisに存在していて、workに存在しないものをremoveに登録
            List<EditedZoneUnit> remove = new List<EditedZoneUnit>();
            foreach (var itemThis in this.iterator()) {
                bool found = false;
                foreach (var itemWork in work.iterator()) {
                    if (itemThis.mStart == itemWork.mStart && itemThis.mEnd == itemWork.mEnd) {
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    remove.Add(new EditedZoneUnit(itemThis.mStart, itemThis.mEnd));
                }
            }

            // workに存在していて、thisに存在しないものをaddに登録
            List<EditedZoneUnit> add = new List<EditedZoneUnit>();
            foreach (var itemWork in work.iterator()) {
                bool found = false;
                foreach (var itemThis in this.iterator()) {
                    if (itemThis.mStart == itemWork.mStart && itemThis.mEnd == itemWork.mEnd) {
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    add.Add(new EditedZoneUnit(itemWork.mStart, itemWork.mEnd));
                }
            }

            work = null;
            return new EditedZoneCommand(add, remove);
        }

        private EditedZoneCommand generateCommandAdd(int start, int end)
        {
            return generateCommandAdd(new EditedZoneUnit[] { new EditedZoneUnit(start, end) });
        }

        /// <summary>
        /// 重複している部分を統合する
        /// </summary>
        private void normalize()
        {
            bool changed = true;
            while (changed) {
                changed = false;
                int count = mSeries.Count;
                for (int i = 0; i < count - 1; i++) {
                    EditedZoneUnit itemi = mSeries[i];
                    if (itemi.mEnd < itemi.mStart) {
                        int d = itemi.mStart;
                        itemi.mStart = itemi.mEnd;
                        itemi.mEnd = d;
                    }
                    for (int j = i + 1; j < count; j++) {
                        EditedZoneUnit itemj = mSeries[j];
                        if (itemj.mEnd < itemj.mStart) {
                            int d = itemj.mStart;
                            itemj.mStart = itemj.mEnd;
                            itemj.mEnd = d;
                        }
                        if (itemj.mStart == itemi.mStart && itemj.mEnd == itemi.mEnd) {
                            mSeries.RemoveAt(j);
                            changed = true;
                            break;
                        } else if (itemj.mStart <= itemi.mStart && itemi.mEnd <= itemj.mEnd) {
                            mSeries.RemoveAt(i);
                            changed = true;
                            break;
                        } else if (itemi.mStart <= itemj.mStart && itemj.mEnd <= itemi.mEnd) {
                            mSeries.RemoveAt(j);
                            changed = true;
                            break;
                        } else if (itemi.mStart <= itemj.mEnd && itemj.mEnd < itemi.mEnd) {
                            itemj.mEnd = itemi.mEnd;
                            mSeries.RemoveAt(i);
                            changed = true;
                            break;
                        } else if (itemi.mStart <= itemj.mStart && itemj.mStart <= itemi.mEnd) {
                            itemi.mEnd = itemj.mEnd;
                            mSeries.RemoveAt(j);
                            changed = true;
                            break;
                        }
                    }
                    if (changed) {
                        break;
                    }
                }
            }
            mSeries.Sort();
        }

        public Object Clone()
        {
            return clone();
        }
    }

}
