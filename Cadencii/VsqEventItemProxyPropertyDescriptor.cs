#if ENABLE_PROPERTY
/*
 * VsqEventItemProxyPropertyDescriptor.cs
 * Copyright (c) 2009 kbinani
 *
 * This file is part of Boare.Cadencii.
 *
 * Boare.Cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * Boare.Cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Reflection;
using System.ComponentModel;

using Boare.Lib.AppUtil;

namespace Boare.Cadencii {
    using boolean = System.Boolean;

    public class VsqEventItemProxyPropertyDescriptor : PropertyDescriptor {
        private Type m_type;

        public VsqEventItemProxyPropertyDescriptor( MemberDescriptor md )
            : base( md ) {
            m_type = typeof( VsqEventItemProxy );
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
