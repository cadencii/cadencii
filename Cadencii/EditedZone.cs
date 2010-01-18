/*
 * EditedZone.cs
 * Copyright (C) 2010 kbinani
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

#else
using System;
using org.kbinani.java.util;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

    public class EditedZone : ICloneable {
        private Vector<EditedZoneUnit> series = new Vector<EditedZoneUnit>();

        public EditedZone(){
        }

        public Iterator iterator() {
            return series.iterator();
        }

        public Object clone() {
            EditedZone ret = new EditedZone();
            int count = series.size();
            for ( int i = 0; i < count; i++ ) {
                EditedZoneUnit p = series.get( i );
                ret.series.add( (EditedZoneUnit)p.clone() );
            }
            return ret;
        }

        public EditedZoneCommand add( int start, int end ) {
            EditedZoneCommand com = generateCommandAdd( start, end );
            return executeCommand( com );
        }

        public EditedZoneCommand executeCommand( EditedZoneCommand run ) {
#if DEBUG
            PortUtil.println( "EditedZone#executeCommand; run.add.size()=" + run.add.size() + "; run.remove.size()=" + run.remove.size() );
#endif
            for ( Iterator itr = run.remove.iterator(); itr.hasNext(); ) {
                EditedZoneUnit item = (EditedZoneUnit)itr.next();
                int count = series.size();
                for ( int i = 0; i < count; i++ ) {
                    EditedZoneUnit item2 = series.get( i );
                    if ( item.start == item2.start && item.end == item2.end ) {
                        series.removeElementAt( i );
#if DEBUG
                        PortUtil.println( "EditedZone#executeCommand; removed(" + item.start + "," + item.end + ")" );
#endif
                        break;
                    }
                }
            }

            for ( Iterator itr = run.add.iterator(); itr.hasNext(); ) {
                EditedZoneUnit item = (EditedZoneUnit)itr.next();
                series.add( item );
            }

            Collections.sort( series );

            return new EditedZoneCommand( run.remove, run.add );
        }

        public EditedZoneCommand generateCommandClear() {
            Vector<EditedZoneUnit> remove = new Vector<EditedZoneUnit>();
            for ( Iterator itr = series.iterator(); itr.hasNext(); ) {
                EditedZoneUnit item = (EditedZoneUnit)itr.next();
                remove.add( (EditedZoneUnit)item.clone() );
            }

            return new EditedZoneCommand( new Vector<EditedZoneUnit>(), remove );
        }

        public EditedZoneCommand generateCommandAdd( int start, int end ) {
            Vector<EditedZoneUnit> add = new Vector<EditedZoneUnit>();
            Vector<EditedZoneUnit> remove = new Vector<EditedZoneUnit>();

            int actualStart = start;
            int actualEnd = end;

            boolean addRequired = true;
            int count = series.size();
            for ( int i = 0; i < count; i++ ) {
                EditedZoneUnit item = series.get( i );
                if ( actualStart <= item.end && item.end < actualEnd ) {
                    remove.add( new EditedZoneUnit( item.start, item.end ) );
                    actualStart = item.start;
                } else if ( actualStart < item.start && item.start <= actualEnd ) {
                    remove.add( new EditedZoneUnit( item.start, item.end ) );
                    actualEnd = item.end;
                } else if ( actualStart <= item.start && item.end < actualEnd ) {
                    remove.add( new EditedZoneUnit( item.start, item.end ) );
                } else if ( item.start <= actualStart && actualEnd < item.end ) {
                    addRequired = false;
                }
            }

            if ( addRequired ) {
                add.add( new EditedZoneUnit( actualStart, actualEnd ) );
            }
            return new EditedZoneCommand( add, remove );
        }

        /// <summary>
        /// 重複している部分を統合する
        /// </summary>
        private void normalize() {
            boolean changed = true;
            while ( changed ) {
                changed = false;
                int count = series.size();
                for ( int i = 0; i < count - 1; i++ ) {
                    EditedZoneUnit itemi = series.get( i );
                    for ( int j = i + 1; j < count; j++ ) {
                        EditedZoneUnit itemj = series.get( j );
                        if ( itemi.start <= itemj.end && itemj.end < itemi.end ) {
                            itemj.end = itemi.end;
                            series.removeElementAt( i );
                            changed = true;
                            break;
                        } else if ( itemi.start <= itemj.start && itemj.start < itemi.end ) {
                            itemi.end = itemj.end;
                            series.removeElementAt( j );
                            changed = true;
                            break;
                        } else if ( itemj.start <= itemi.start && itemi.end <= itemi.end ) {
                            series.removeElementAt( i );
                            changed = true;
                            break;
                        }
                    }
                    if ( changed ) {
                        break;
                    }
                }
            }
            Collections.sort( series );
        }

        public void clear() {
            series.clear();
        }

#if !JAVA
        public object Clone(){
            return clone();
        }
#endif
    }

#if !JAVA
}
#endif
