/*
 * FormSequenceConfig.cs
 * Copyright © 2011 kbinani
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

//INCLUDE-SECTION IMPORT ../BuildJavaUI/src/org/kbinani/Cadencii/Preference.java

import java.awt.*;
import java.util.*;
import java.io.*;
import org.kbinani.*;
import org.kbinani.apputil.*;
import org.kbinani.media.*;
import org.kbinani.vsq.*;
import org.kbinani.windows.forms.*;
#else
using System;
using org.kbinani.apputil;
using org.kbinani.java.awt;
using org.kbinani.java.io;
using org.kbinani.java.util;
using org.kbinani.media;
using org.kbinani.vsq;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii
{
    using BEventArgs = System.EventArgs;
    using BFormClosingEventArgs = System.Windows.Forms.FormClosingEventArgs;
    using boolean = System.Boolean;
#endif

#if JAVA
    public class FormSequenceConfig extends BDialog
#else
    class FormSequenceConfig : BDialog
#endif
    {
        private org.kbinani.windows.forms.BLabel labelSampleRate;
        private org.kbinani.windows.forms.BComboBox comboSampleRate;

        public FormSequenceConfig()
        {
#if JAVA
            //INCLUDE-SECTION CTOR ../BuildJavaUI/src/org/kbinani/Cadencii/Preference.java
#else
            InitializeComponent();
#endif
            applyLanguage();

            // wave channel
            comboChannel.removeAllItems();
            comboChannel.addItem( _( "Monoral" ) );
            comboChannel.addItem( _( "Stereo" ) );

            // sample rate
            comboSampleRate.removeAllItems();
            comboSampleRate.addItem( "44100" );
            comboSampleRate.addItem( "48000" );
            comboSampleRate.addItem( "96000" );
            comboSampleRate.setSelectedIndex( 0 );

            registerEventHandlers();
            setResources();
        }

        #region public methods
        public void applyLanguage()
        {
            setTitle( _( "WAVE format" ) );
            btnCancel.setText( _( "Cancel" ) );
            btnOK.setText( _( "OK" ) );

            groupWaveFileOutput.setTitle( _( "Wave File Output" ) );
            lblChannel.setText( _( "Channel" ) + "(&C)" );
            radioMasterTrack.setText( _( "Master Track" ) );
            radioCurrentTrack.setText( _( "Current Track" ) );
            labelSampleRate.setText( _( "Sample rate" ) );

            int current_index = comboChannel.getSelectedIndex();
            comboChannel.removeAllItems();
            comboChannel.addItem( _( "Monoral" ) );
            comboChannel.addItem( _( "Stereo" ) );
            comboChannel.setSelectedIndex( current_index );
        }

        public int getSampleRate()
        {
            int index = comboSampleRate.getSelectedIndex();
            string str = "44100";
            if ( index < 0 ) {
                str = comboSampleRate.Text;
            } else {
                str = (string)comboSampleRate.getItemAt( index );
            }
            int ret = 44100;
            try {
                ret = PortUtil.parseInt( str );
            } catch ( Exception ex ) {
                ret = 44100;
            }
            return ret;
        }

        public void setSampleRate( int value )
        {
            comboSampleRate.setSelectedIndex( 0 );
            for ( int i = 0; i < comboSampleRate.getItemCount(); i++ ) {
                string str = (string)comboSampleRate.getItemAt( i );
                int rate = 0;
                try {
                    rate = PortUtil.parseInt( str );
                } catch ( Exception ex ) {
                    rate = 0;
                }
                if ( rate == value ) {
                    comboSampleRate.setSelectedIndex( i );
                    break;
                }
            }
        }

        public boolean isWaveFileOutputFromMasterTrack()
        {
            return radioMasterTrack.isSelected();
        }

        public void setWaveFileOutputFromMasterTrack( boolean value )
        {
            radioMasterTrack.setSelected( value );
            radioCurrentTrack.setSelected( !value );
        }

        public int getWaveFileOutputChannel()
        {
            if ( comboChannel.getSelectedIndex() <= 0 ) {
                return 1;
            } else {
                return 2;
            }
        }

        public void setWaveFileOutputChannel( int value )
        {
            if ( value == 1 ) {
                comboChannel.setSelectedIndex( 0 );
            } else {
                comboChannel.setSelectedIndex( 1 );
            }
        }
        #endregion

        #region event handlers
        public void btnOK_Click( Object sender, BEventArgs e )
        {
            setDialogResult( BDialogResult.OK );
        }

        public void btnCancel_Click( Object sender, BEventArgs e )
        {
            setDialogResult( BDialogResult.CANCEL );
        }
        #endregion

        #region helper methods
        private static String _( String id )
        {
            return Messaging.getMessage( id );
        }

        private void registerEventHandlers()
        {
            btnOK.Click += new EventHandler( btnOK_Click );
            btnCancel.Click += new EventHandler( btnCancel_Click );
        }

        private void setResources()
        {
        }
        #endregion

        #region ui implementation
#if JAVA
        //INCLUDE-SECTION FIELD ../BuildJavaUI/src/org/kbinani/cadencii/Preference.java
        //INCLUDE-SECTION METHOD ../BuildJavaUI/src/org/kbinani/cadencii/Preference.java
#else
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose( boolean disposing )
        {
            if ( disposing && (components != null) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupWaveFileOutput = new org.kbinani.windows.forms.BGroupBox();
            this.comboSampleRate = new org.kbinani.windows.forms.BComboBox();
            this.labelSampleRate = new org.kbinani.windows.forms.BLabel();
            this.radioCurrentTrack = new org.kbinani.windows.forms.BRadioButton();
            this.radioMasterTrack = new org.kbinani.windows.forms.BRadioButton();
            this.lblChannel = new org.kbinani.windows.forms.BLabel();
            this.comboChannel = new org.kbinani.windows.forms.BComboBox();
            this.btnCancel = new org.kbinani.windows.forms.BButton();
            this.btnOK = new org.kbinani.windows.forms.BButton();
            this.groupWaveFileOutput.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupWaveFileOutput
            // 
            this.groupWaveFileOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupWaveFileOutput.Controls.Add( this.comboSampleRate );
            this.groupWaveFileOutput.Controls.Add( this.labelSampleRate );
            this.groupWaveFileOutput.Controls.Add( this.radioCurrentTrack );
            this.groupWaveFileOutput.Controls.Add( this.radioMasterTrack );
            this.groupWaveFileOutput.Controls.Add( this.lblChannel );
            this.groupWaveFileOutput.Controls.Add( this.comboChannel );
            this.groupWaveFileOutput.Location = new System.Drawing.Point( 12, 12 );
            this.groupWaveFileOutput.Name = "groupWaveFileOutput";
            this.groupWaveFileOutput.Size = new System.Drawing.Size( 316, 107 );
            this.groupWaveFileOutput.TabIndex = 28;
            this.groupWaveFileOutput.TabStop = false;
            this.groupWaveFileOutput.Text = "Wave File Output";
            // 
            // comboSampleRate
            // 
            this.comboSampleRate.Items.AddRange( new object[] {
            "Mono",
            "Stereo"} );
            this.comboSampleRate.Location = new System.Drawing.Point( 135, 48 );
            this.comboSampleRate.Name = "comboSampleRate";
            this.comboSampleRate.Size = new System.Drawing.Size( 117, 20 );
            this.comboSampleRate.TabIndex = 31;
            // 
            // labelSampleRate
            // 
            this.labelSampleRate.AutoSize = true;
            this.labelSampleRate.Location = new System.Drawing.Point( 22, 51 );
            this.labelSampleRate.Name = "labelSampleRate";
            this.labelSampleRate.Size = new System.Drawing.Size( 66, 12 );
            this.labelSampleRate.TabIndex = 30;
            this.labelSampleRate.Text = "Sample rate";
            // 
            // radioCurrentTrack
            // 
            this.radioCurrentTrack.AutoSize = true;
            this.radioCurrentTrack.Checked = true;
            this.radioCurrentTrack.Location = new System.Drawing.Point( 155, 75 );
            this.radioCurrentTrack.Name = "radioCurrentTrack";
            this.radioCurrentTrack.Size = new System.Drawing.Size( 61, 16 );
            this.radioCurrentTrack.TabIndex = 29;
            this.radioCurrentTrack.TabStop = true;
            this.radioCurrentTrack.Text = "Current";
            this.radioCurrentTrack.UseVisualStyleBackColor = true;
            // 
            // radioMasterTrack
            // 
            this.radioMasterTrack.AutoSize = true;
            this.radioMasterTrack.Location = new System.Drawing.Point( 24, 75 );
            this.radioMasterTrack.Name = "radioMasterTrack";
            this.radioMasterTrack.Size = new System.Drawing.Size( 91, 16 );
            this.radioMasterTrack.TabIndex = 28;
            this.radioMasterTrack.Text = "Master Track";
            this.radioMasterTrack.UseVisualStyleBackColor = true;
            // 
            // lblChannel
            // 
            this.lblChannel.AutoSize = true;
            this.lblChannel.Location = new System.Drawing.Point( 22, 27 );
            this.lblChannel.Name = "lblChannel";
            this.lblChannel.Size = new System.Drawing.Size( 66, 12 );
            this.lblChannel.TabIndex = 25;
            this.lblChannel.Text = "Channel (&C)";
            // 
            // comboChannel
            // 
            this.comboChannel.FormattingEnabled = true;
            this.comboChannel.Items.AddRange( new object[] {
            "Mono",
            "Stereo"} );
            this.comboChannel.Location = new System.Drawing.Point( 135, 24 );
            this.comboChannel.Name = "comboChannel";
            this.comboChannel.Size = new System.Drawing.Size( 97, 20 );
            this.comboChannel.TabIndex = 27;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 240, 136 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 88, 23 );
            this.btnCancel.TabIndex = 201;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point( 146, 136 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 88, 23 );
            this.btnOK.TabIndex = 200;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // FormSequenceConfig
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 96F, 96F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 341, 175 );
            this.Controls.Add( this.groupWaveFileOutput );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.btnCancel );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSequenceConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "WAVE format";
            this.groupWaveFileOutput.ResumeLayout( false );
            this.groupWaveFileOutput.PerformLayout();
            this.ResumeLayout( false );

        }

        #endregion

        private BButton btnCancel;
        private BButton btnOK;
        private BLabel lblChannel;
        private BComboBox comboChannel;
        private BGroupBox groupWaveFileOutput;
        private BRadioButton radioCurrentTrack;
        private BRadioButton radioMasterTrack;
#endif
        #endregion

    }

#if !JAVA
}
#endif
