/*
 * EditedZone.cs
 * Copyright © 2010-2011 kbinani
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
package cadencii;

import java.util.*;
#else
using System;
using cadencii.java.util;

namespace cadencii {
    using boolean = System.Boolean;
#endif

#if JAVA
    public class EditedZone implements Cloneable {
#else
    public class EditedZone : ICloneable {
#endif
        private Vector<EditedZoneUnit> mSeries = new Vector<EditedZoneUnit>();

        public EditedZone(){
        }

        public int size() {
            return mSeries.size();
        }

        public Iterator<EditedZoneUnit> iterator() {
            return mSeries.iterator();
        }

        public Object clone() {
            EditedZone ret = new EditedZone();
            int count = mSeries.size();
            for ( int i = 0; i < count; i++ ) {
                EditedZoneUnit p = mSeries.get( i );
                ret.mSeries.add( (EditedZoneUnit)p.clone() );
            }
            return ret;
        }

        public EditedZoneCommand add( int start, int end ) {
            EditedZoneCommand com = generateCommandAdd( start, end );
            return executeCommand( com );
        }

        public EditedZoneCommand add( EditedZoneUnit[] items ) {
            EditedZoneCommand com = generateCommandAdd( items );
            return executeCommand( com );
        }

        public EditedZoneCommand executeCommand( EditedZoneCommand run ) {
            for ( Iterator<EditedZoneUnit> itr = run.mRemove.iterator(); itr.hasNext(); ) {
                EditedZoneUnit item = itr.next();
                int count = mSeries.size();
                for ( int i = 0; i < count; i++ ) {
                    EditedZoneUnit item2 = mSeries.get( i );
                    if ( item.mStart == item2.mStart && item.mEnd == item2.mEnd ) {
                        mSeries.removeElementAt( i );
                        break;
                    }
                }
            }

            for ( Iterator<EditedZoneUnit> itr = run.mAdd.iterator(); itr.hasNext(); ) {
                EditedZoneUnit item = itr.next();
                mSeries.add( (EditedZoneUnit)item.clone() );
            }

            Collections.sort( mSeries );

            return new EditedZoneCommand( run.mRemove, run.mAdd );
        }

        private EditedZoneCommand generateCommandClear() {
            Vector<EditedZoneUnit> remove = new Vector<EditedZoneUnit>();
            for ( Iterator<EditedZoneUnit> itr = mSeries.iterator(); itr.hasNext(); ) {
                EditedZoneUnit item = itr.next();
                remove.add( (EditedZoneUnit)item.clone() );
            }

            return new EditedZoneCommand( new Vector<EditedZoneUnit>(), remove );
        }

        private EditedZoneCommand generateCommandAdd( EditedZoneUnit[] areas ) {
            EditedZone work = (EditedZone)clone();
            for ( int i = 0; i < areas.Length; i++ ) {
                EditedZoneUnit item = areas[i];
                if ( item == null ) {
                    continue;
                }
                work.mSeries.add( new EditedZoneUnit( item.mStart, item.mEnd ) );
            }
            work.normalize();

            // thisに存在していて、workに存在しないものをremoveに登録
            Vector<EditedZoneUnit> remove = new Vector<EditedZoneUnit>();
            for ( Iterator<EditedZoneUnit> itrThis = iterator(); itrThis.hasNext(); ) {
                boolean found = false;
                EditedZoneUnit itemThis = itrThis.next();
                for ( Iterator<EditedZoneUnit> itrWork = work.iterator(); itrWork.hasNext(); ) {
                    EditedZoneUnit itemWork = itrWork.next();
                    if ( itemThis.mStart == itemWork.mStart && itemThis.mEnd == itemWork.mEnd ) {
                        found = true;
                        break;
                    }
                }
                if ( !found ) {
                    remove.add( new EditedZoneUnit( itemThis.mStart, itemThis.mEnd ) );
                }
            }

            // workに存在していて、thisに存在しないものをaddに登録
            Vector<EditedZoneUnit> add = new Vector<EditedZoneUnit>();
            for ( Iterator<EditedZoneUnit> itrWork = work.iterator(); itrWork.hasNext(); ) {
                boolean found = false;
                EditedZoneUnit itemWork = itrWork.next();
                for ( Iterator<EditedZoneUnit> itrThis = iterator(); itrThis.hasNext(); ) {
                    EditedZoneUnit itemThis = itrThis.next();
                    if ( itemThis.mStart == itemWork.mStart && itemThis.mEnd == itemWork.mEnd ) {
                        found = true;
                        break;
                    }
                }
                if ( !found ) {
                    add.add( new EditedZoneUnit( itemWork.mStart, itemWork.mEnd ) );
                }
            }

            work = null;
            return new EditedZoneCommand( add, remove );
        }

        private EditedZoneCommand generateCommandAdd( int start, int end ) {
            return generateCommandAdd( new EditedZoneUnit[] { new EditedZoneUnit( start, end ) } );
        }

        /// <summary>
        /// 重複している部分を統合する
        /// </summary>
        private void normalize() {
            boolean changed = true;
            while ( changed ) {
                changed = false;
                int count = mSeries.size();
                for ( int i = 0; i < count - 1; i++ ) {
                    EditedZoneUnit itemi = mSeries.get( i );
                    if ( itemi.mEnd < itemi.mStart ) {
                        int d = itemi.mStart;
                        itemi.mStart = itemi.mEnd;
                        itemi.mEnd = d;
                    }
                    for ( int j = i + 1; j < count; j++ ) {
                        EditedZoneUnit itemj = mSeries.get( j );
                        if ( itemj.mEnd < itemj.mStart ) {
                            int d = itemj.mStart;
                            itemj.mStart = itemj.mEnd;
                            itemj.mEnd = d;
                        }
                        if ( itemj.mStart == itemi.mStart && itemj.mEnd == itemi.mEnd ) {
                            mSeries.removeElementAt( j );
                            changed = true;
                            break;
                        } else if ( itemj.mStart <= itemi.mStart && itemi.mEnd <= itemj.mEnd ) {
                            mSeries.removeElementAt( i );
                            changed = true;
                            break;
                        } else if ( itemi.mStart <= itemj.mStart && itemj.mEnd <= itemi.mEnd ) {
                            mSeries.removeElementAt( j );
                            changed = true;
                            break;
                        } else if ( itemi.mStart <= itemj.mEnd && itemj.mEnd < itemi.mEnd ) {
                            itemj.mEnd = itemi.mEnd;
                            mSeries.removeElementAt( i );
                            changed = true;
                            break;
                        } else if ( itemi.mStart <= itemj.mStart && itemj.mStart <= itemi.mEnd ) {
                            itemi.mEnd = itemj.mEnd;
                            mSeries.removeElementAt( j );
                            changed = true;
                            break;
                        }
                    }
                    if ( changed ) {
                        break;
                    }
                }
            }
            Collections.sort( mSeries );
        }

#if !JAVA
        public Object Clone(){
            return clone();
        }
#endif
    }

#if !JAVA
}
#endif
