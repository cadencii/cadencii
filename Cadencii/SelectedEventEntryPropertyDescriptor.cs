#if ENABLE_PROPERTY
/*
 * SelectedEventEntryPropertyDescriptor.cs
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
using System;
using System.ComponentModel;
using System.Reflection;
using org.kbinani.apputil;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;

    public class SelectedEventEntryPropertyDescriptor : PropertyDescriptor {
        private Type m_type;

        public SelectedEventEntryPropertyDescriptor( MemberDescriptor md )
            : base( md ) {
            m_type = typeof( SelectedEventEntry );
        }

        public override boolean ShouldSerializeValue( object component ) {
            return true;
        }

        public override void ResetValue( object component ) {
        }

        public override void SetValue( object component, object value ) {
            PropertyInfo pi = m_type.GetProperty( this.Name );
            pi.SetValue( component, value, new object[] { } );
        }

        public override object GetValue( object component ) {
            if ( component == null ) {
                return null;
            }
            PropertyInfo pi = m_type.GetProperty( base.Name );
            return pi.GetValue( component, new object[] { } );
        }

        public override boolean CanResetValue( object component ) {
            return false;
        }

        public override Type PropertyType {
            get {
                Type t = m_type.GetProperty( this.Name ).PropertyType;
                return t;
            }
        }

        public override Type ComponentType {
            get {
                return m_type;
            }
        }

        public override boolean IsReadOnly {
            get {
                return false;
            }
        }

        public override String DisplayName {
            get {
                switch ( base.Name ) {
                    case "Clock":
                        return _( "Clock" );
                    case "Length":
                        return _( "Length" );
                    case "Note":
                        return _( "Note#" );
                    case "Velocity":
                        return _( "Velocity" );
                    case "BendDepth":
                        return _( "Bend Depth" );
                    case "BendLength":
                        return _( "Bend Length" );
                    case "Decay":
                        return _( "Decay" );
                    case "Accent":
                        return _( "Accent" );
                    case "UpPortamento":
                        return _( "Up-Portamento" );
                    case "DownPortamento":
                        return _( "Down-Portamento" );
                    case "VibratoLength":
                        return _( "Vibrato Length" );
                    case "PhoneticSymbol":
                        return _( "Phonetic Symbol" );
                    case "Phrase":
                        return _( "Phrase" );
                    case "PreUtterance":
                        return _( "Pre Utterance" );
                    case "Overlap":
                        return _( "Overlap" );
                    case "Moduration":
                        return _( "Moduration" );
                    case "Vibrato":
                        return _( "Vibrato" );
                    case "Attack":
                        return _( "Attack" );
                    case "AttackDuration":
                        return _( "Attack Duration" );
                    case "AttackDepth":
                        return _( "Attack Depth" );
                }
                return _( this.Name );
            }
        }

        private static String _( String id ) {
            return Messaging.getMessage( id );
        }
    }

}
#endif
