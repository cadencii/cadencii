/*
 * CommandTree.cs
 * Copyright © 2009-2011 kbinani
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

namespace cadencii
{

#if TREECOM
    /// <summary>
    /// 分岐可能なコマンド履歴
    /// </summary>
    public partial class EditorManager {
        private ICommand s_root;
        private ICommand s_current = null;
        public VsqFileEx m_vsq = null;

        public EditorManager() {
            ID = org.kbinani.misc.getmd5( DateTime.Now.ToBinary().ToString() );
            s_root = new CadenciiCommand( Boare.Lib.Vsq.VsqCommand.generateCommandRoot() );
        }

        public VsqFileEx VsqFile {
            get {
                return m_vsq;
            }
        }

        public bool IsRedoAvailable {
            get {
                if ( s_current == null ) {
                    return false;
                } else {
                    if ( s_current.Child.Count > 0 ) {
                        return true;
                    } else {
                        return false;
                    }
                }
            }
        }

        public void ClearCommandBuffer() {
            s_root.Child.Clear();
            s_current = null;
        }

        public bool IsUndoAvailable {
            get {
                if ( s_current != null ) {
                    if ( Object.ReferenceEquals( s_root, s_current ) ) {
                        return false;
                    } else {
                        return true;
                    }
                } else {
                    return false;
                }
            }
        }

        /*public static ICommand[] GetAvailableRedoCommand() {
            
        }

        public static ICommand[] GetAvailableUndoCommand() {
        }*/

        public void Redo() {
            Redo( 0 );
        }

        public void Redo( int index ) {
            if ( s_current.Child.Count > 0 ) {
                ICommand run = s_current.Child[index];
                ICommand rev = VsqFile.executeCommand( run );
                rev.Parent = s_current;
                for ( int i = 0; i < run.Child.Count; i++ ) {
                    run.Child[i].Parent = rev;
                    rev.Child.Add( run.Child[i] );
                }
                s_current.Child[index] = rev;
                s_current = run;
            }
        }

        public void Undo() {
            ICommand run = s_current;
            ICommand rev = VsqFile.executeCommand( run );
            for ( int i = 0; i < s_current.Child.Count; i++ ) {
                s_current.Child[i].Parent = rev;
                rev.Child.Add( s_current.Child[i] );
            }
            rev.Parent = s_current.Parent;
            for ( int i = 0; i < s_current.Parent.Child.Count; i++ ) {
                if ( Object.ReferenceEquals( s_current, s_current.Parent.Child[i] ) ) {
                    s_current.Parent.Child[i] = rev;
                    break;
                }
            }
            s_current = s_current.Parent;
        }

        public void Register( ICommand command ) {
            if ( s_current == null ) {
                s_root.Child.Add( command );
                command.Parent = s_root;
            } else {
                s_current.Child.Insert( 0, command );
                command.Parent = s_current;
            }
            s_current = command;
        }
    }
#endif
}
