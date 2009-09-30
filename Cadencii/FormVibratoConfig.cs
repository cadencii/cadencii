/*
 * FormVibratoConfig.cs
 * Copyright (c) 2008-2009 kbinani
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
using System.Collections.Generic;
using System.Windows.Forms;

using Boare.Lib.AppUtil;
using Boare.Lib.Vsq;
using bocoree;

namespace Boare.Cadencii {

    partial class FormVibratoConfig : Form {
        private VibratoHandle m_vibrato;
        private int m_note_length;
        private SynthesizerType m_synthesizer_type;

        /// <summary>
        /// コンストラクタ．引数vibrato_handleには，Cloneしたものを渡さなくてよい．
        /// </summary>
        /// <param name="vibrato_handle"></param>
        public FormVibratoConfig( VibratoHandle vibrato_handle, int note_length, DefaultVibratoLength default_vibrato_length, SynthesizerType type ) {
#if DEBUG
            AppManager.debugWriteLine( "FormVibratoConfig.ctor(Vsqhandle,int,DefaultVibratoLength)" );
            AppManager.debugWriteLine( "    (vibrato_handle==null)=" + (vibrato_handle == null) );
            Console.WriteLine( "    type=" + type );
#endif
            m_synthesizer_type = type;
            if ( vibrato_handle != null ) {
                m_vibrato = (VibratoHandle)vibrato_handle.Clone();
            }

            InitializeComponent();
            ApplyLanguage();

            comboVibratoType.Items.Clear();
            VibratoConfig empty = new VibratoConfig();
            empty.contents.Caption = "[Non Vibrato]";
            empty.contents.IconID = "$04040000";
            comboVibratoType.Items.Add( empty );
            comboVibratoType.SelectedIndex = 0;
            int count = 0;
            for ( Iterator itr = VocaloSysUtil.vibratoConfigIterator( m_synthesizer_type ); itr.hasNext(); ) {
                VibratoConfig vconfig = (VibratoConfig)itr.next();
                comboVibratoType.Items.Add( vconfig );
                count++;
                if ( vibrato_handle != null ) {
                    if ( vibrato_handle.IconID.Equals( vconfig.contents.IconID ) ) {
                        comboVibratoType.SelectedIndex = count;
                    }
                }
            }

            txtVibratoLength.Enabled = vibrato_handle != null;
            if ( vibrato_handle != null ) {
                txtVibratoLength.Text = (int)((float)vibrato_handle.Length / (float)note_length * 100.0f) + "";
            } else {
                switch ( default_vibrato_length ) {
                    case DefaultVibratoLength.L100:
                        txtVibratoLength.Text = "100";
                        break;
                    case DefaultVibratoLength.L50:
                        txtVibratoLength.Text = "50";
                        break;
                    case DefaultVibratoLength.L66:
                        txtVibratoLength.Text = "66";
                        break;
                    case DefaultVibratoLength.L75:
                        txtVibratoLength.Text = "75";
                        break;
                }
            }

            this.comboVibratoType.SelectedIndexChanged += new System.EventHandler( this.comboVibratoType_SelectedIndexChanged );
            this.txtVibratoLength.TextChanged += new System.EventHandler( txtVibratoLength_TextChanged );

            m_note_length = note_length;
            Misc.ApplyFontRecurse( this, AppManager.editorConfig.BaseFont );
        }

        public void ApplyLanguage() {
            Text = _( "Vibrato property" );
            lblVibratoLength.Text = _( "Vibrato length" ) + "(&L)";
            lblVibratoType.Text = _( "Vibrato Type" ) + "(&T)";
            btnOK.Text = _( "OK" );
            btnCancel.Text = _( "Cancel" );
        }

        public static String _( String id ) {
            return Messaging.GetMessage( id );
        }

        /// <summary>
        /// 編集済みのビブラート設定．既にCloneされているので，改めてCloneしなくて良い
        /// </summary>
        public VibratoHandle VibratoHandle {
            get {
                return m_vibrato;
            }
        }

        private void btnOK_Click( object sender, EventArgs e ) {
            this.DialogResult = DialogResult.OK;
        }

        private void comboVibratoType_SelectedIndexChanged( object sender, EventArgs e ) {
            int index = comboVibratoType.SelectedIndex;
            if ( index >= 0 ) {
                String s = ((VibratoConfig)comboVibratoType.Items[index]).contents.IconID;
                if ( s.Equals( "$04040000" ) ) {
                    m_vibrato = null;
                    txtVibratoLength.Enabled = false;
                    return;
                } else {
                    txtVibratoLength.Enabled = true;
                    for ( Iterator itr = VocaloSysUtil.vibratoConfigIterator( m_synthesizer_type ); itr.hasNext(); ) {
                        VibratoConfig vconfig = (VibratoConfig)itr.next();
                        if ( s.Equals( vconfig.contents.IconID ) ) {
                            int percent;
                            try {
                                percent = int.Parse( txtVibratoLength.Text );
                            } catch {
                                return;
                            }
                            m_vibrato = (VibratoHandle)vconfig.contents.clone();
                            m_vibrato.Length = (int)(m_note_length * percent / 100.0f);
                            return;
                        }
                    }
                }
            }
        }

        private void txtVibratoLength_TextChanged( object sender, System.EventArgs e ) {
#if DEBUG
            AppManager.debugWriteLine( "txtVibratoLength_TextChanged" );
            AppManager.debugWriteLine( "    (m_vibrato==null)=" + (m_vibrato == null) );
#endif
            int percent = 0;
            try {
                percent = int.Parse( txtVibratoLength.Text );
                if ( percent < 0 ) {
                    percent = 0;
                } else if ( 100 < percent ) {
                    percent = 100;
                }
            } catch {
                return;
            }
            if ( percent == 0 ) {
                m_vibrato = null;
                txtVibratoLength.Enabled = false;
            } else {
                if ( m_vibrato != null ) {
                    int new_length = (int)(m_note_length * percent / 100.0f);
                    m_vibrato.Length = new_length;
                }
            }
        }
    }

}
