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

        public int size() {
            return series.size();
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

        public EditedZoneCommand add( EditedZoneUnit[] items ) {
            EditedZoneCommand com = generateCommandAdd( items );
            return executeCommand( com );
        }

        public EditedZoneCommand executeCommand( EditedZoneCommand run ) {
            for ( Iterator itr = run.remove.iterator(); itr.hasNext(); ) {
                EditedZoneUnit item = (EditedZoneUnit)itr.next();
                int count = series.size();
                for ( int i = 0; i < count; i++ ) {
                    EditedZoneUnit item2 = series.get( i );
                    if ( item.start == item2.start && item.end == item2.end ) {
                        series.removeElementAt( i );
                        break;
                    }
                }
            }

            for ( Iterator itr = run.add.iterator(); itr.hasNext(); ) {
                EditedZoneUnit item = (EditedZoneUnit)itr.next();
                series.add( (EditedZoneUnit)item.clone() );
            }

            Collections.sort( series );

            return new EditedZoneCommand( run.remove, run.add );
        }

        private EditedZoneCommand generateCommandClear() {
            Vector<EditedZoneUnit> remove = new Vector<EditedZoneUnit>();
            for ( Iterator itr = series.iterator(); itr.hasNext(); ) {
                EditedZoneUnit item = (EditedZoneUnit)itr.next();
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
                work.series.add( new EditedZoneUnit( item.start, item.end ) );
            }
            work.normalize();

            // thisに存在していて、workに存在しないものをremoveに登録
            Vector<EditedZoneUnit> remove = new Vector<EditedZoneUnit>();
            for ( Iterator itrThis = iterator(); itrThis.hasNext(); ) {
                boolean found = false;
                EditedZoneUnit itemThis = (EditedZoneUnit)itrThis.next();
                for ( Iterator itrWork = work.iterator(); itrWork.hasNext(); ) {
                    EditedZoneUnit itemWork = (EditedZoneUnit)itrWork.next();
                    if ( itemThis.start == itemWork.start && itemThis.end == itemWork.end ) {
                        found = true;
                        break;
                    }
                }
                if ( !found ) {
                    remove.add( new EditedZoneUnit( itemThis.start, itemThis.end ) );
                }
            }

            // workに存在していて、thisに存在しないものをaddに登録
            Vector<EditedZoneUnit> add = new Vector<EditedZoneUnit>();
            for ( Iterator itrWork = work.iterator(); itrWork.hasNext(); ) {
                boolean found = false;
                EditedZoneUnit itemWork = (EditedZoneUnit)itrWork.next();
                for ( Iterator itrThis = iterator(); itrThis.hasNext(); ) {
                    EditedZoneUnit itemThis = (EditedZoneUnit)itrThis.next();
                    if ( itemThis.start == itemWork.start && itemThis.end == itemWork.end ) {
                        found = true;
                        break;
                    }
                }
                if ( !found ) {
                    add.add( new EditedZoneUnit( itemWork.start, itemWork.end ) );
                }
            }

            work = null;
            return new EditedZoneCommand( add, remove );
        }

        private EditedZoneCommand generateCommandAdd( int start, int end ) {
            return generateCommandAdd( new EditedZoneUnit[] { new EditedZoneUnit( start, end ) } );
        }

        private EditedZoneCommand generateCommandAdd_( int start, int end ) {
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
                    if ( itemi.end < itemi.start ) {
                        int d = itemi.start;
                        itemi.start = itemi.end;
                        itemi.end = d;
                    }
                    for ( int j = i + 1; j < count; j++ ) {
                        EditedZoneUnit itemj = series.get( j );
                        if ( itemj.end < itemj.start ) {
                            int d = itemj.start;
                            itemj.start = itemj.end;
                            itemj.end = d;
                        }
                        if ( itemj.start == itemi.start && itemj.end == itemi.end ) {
                            series.removeElementAt( j );
                            changed = true;
                            break;
                        } else if ( itemj.start <= itemi.start && itemi.end <= itemj.end ) {
                            series.removeElementAt( i );
                            changed = true;
                            break;
                        } else if ( itemi.start <= itemj.start && itemj.end <= itemi.end ) {
                            series.removeElementAt( j );
                            changed = true;
                            break;
                        } else if ( itemi.start <= itemj.end && itemj.end < itemi.end ) {
                            itemj.end = itemi.end;
                            series.removeElementAt( i );
                            changed = true;
                            break;
                        } else if ( itemi.start <= itemj.start && itemj.start <= itemi.end ) {
                            itemi.end = itemj.end;
                            series.removeElementAt( j );
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

        public EditedZoneCommand clear() {
            EditedZoneCommand com = generateCommandClear();
            return executeCommand( com );
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
