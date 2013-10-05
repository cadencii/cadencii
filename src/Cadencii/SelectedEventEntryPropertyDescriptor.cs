#if ENABLE_PROPERTY
/*
 * SelectedEventEntryPropertyDescriptor.cs
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
using System.ComponentModel;
using System.Reflection;
using cadencii.apputil;



namespace cadencii
{

    public class SelectedEventEntryPropertyDescriptor : PropertyDescriptor
    {
        private Type m_type;

        public SelectedEventEntryPropertyDescriptor(MemberDescriptor md)
            : base(md)
        {
            m_type = typeof(SelectedEventEntry);
        }

        public override bool ShouldSerializeValue(Object component)
        {
            return true;
        }

        public override void ResetValue(Object component)
        {
        }

        public override void SetValue(Object component, Object value)
        {
            PropertyInfo pi = m_type.GetProperty(this.Name);
            pi.SetValue(component, value, new Object[] { });
        }

        public override Object GetValue(Object component)
        {
            if (component == null) {
                return null;
            }
            PropertyInfo pi = m_type.GetProperty(base.Name);
            return pi.GetValue(component, new Object[] { });
        }

        public override bool CanResetValue(Object component)
        {
            return false;
        }

        public override Type PropertyType
        {
            get
            {
                Type t = m_type.GetProperty(this.Name).PropertyType;
                return t;
            }
        }

        public override Type ComponentType
        {
            get
            {
                return m_type;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public override string DisplayName
        {
            get
            {
                return getDisplayName(base.Name);
            }
        }

        public string getDisplayName(string name)
        {
            if (name.Equals("Clock")) {
                return _("Clock");
            } else if (name.Equals("Length")) {
                return _("Length");
            } else if (name.Equals("Note")) {
                return _("Note#");
            } else if (name.Equals("Velocity")) {
                return _("Velocity");
            } else if (name.Equals("BendDepth")) {
                return _("Bend Depth");
            } else if (name.Equals("BendLength")) {
                return _("Bend Length");
            } else if (name.Equals("Decay")) {
                return _("Decay");
            } else if (name.Equals("Accent")) {
                return _("Accent");
            } else if (name.Equals("UpPortamento")) {
                return _("Up-Portamento");
            } else if (name.Equals("DownPortamento")) {
                return _("Down-Portamento");
            } else if (name.Equals("VibratoLength")) {
                return _("Vibrato Length");
            } else if (name.Equals("PhoneticSymbol")) {
                return _("Phonetic Symbol");
            } else if (name.Equals("Phrase")) {
                return _("Phrase");
            } else if (name.Equals("PreUtterance")) {
                return _("Pre Utterance");
            } else if (name.Equals("Overlap")) {
                return _("Overlap");
            } else if (name.Equals("Moduration")) {
                return _("Moduration");
            } else if (name.Equals("Vibrato")) {
                return _("Vibrato");
            } else if (name.Equals("Attack")) {
                return _("Attack");
            } else if (name.Equals("AttackDuration")) {
                return _("Attack Duration");
            } else if (name.Equals("AttackDepth")) {
                return _("Attack Depth");
            } else if (name == "StartPoint") {
                return _("StartPoint");
            } else if (name == "Intensity") {
                return _("Intensity");
            }
            return _(name);
        }

        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }
    }

}
#endif
