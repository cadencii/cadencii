/*
 * FormInsertBar.cs
 * Copyright © 2008-2011 kbinani
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
#if JAVA
package cadencii;

//INCLUDE-SECTION IMPORT ./ui/java/FormInsertBar.java

import cadencii.*;
import cadencii.apputil.*;
import cadencii.windows.forms.*;
#else
using System;
using cadencii.apputil;
using cadencii.windows.forms;

namespace cadencii
{
    using BEventArgs = System.EventArgs;
    using boolean = System.Boolean;
    using BEventHandler = System.EventHandler;
#endif

#if JAVA
    public class FormInsertBar extends BDialog {
#else
    public class FormInsertBar : BDialog
    {
#endif
        public FormInsertBar( int max_position )
        {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif
            registerEventHandlers();
            setResources();
            applyLanguage();
            numPosition.setMaximum( max_position );
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        #region public methods
        public void applyLanguage()
        {
            setTitle( _( "Insert Bars" ) );
            String th_prefix = _( "_PREFIX_TH_" );
            if ( th_prefix.Equals( "_PREFIX_TH_" ) ) {
                lblPositionPrefix.setText( "" );
            } else {
                lblPositionPrefix.setText( th_prefix );
            }
            lblPosition.setText( _( "Position" ) );
            lblLength.setText( _( "Length" ) );
            lblThBar.setText( _( "th bar" ) );
            lblBar.setText( _( "bar" ) );
            btnOK.setText( _( "OK" ) );
            btnCancel.setText( _( "Cancel" ) );
#if !JAVA
            lblPositionPrefix.Left = numPosition.Left - lblPositionPrefix.Width;
#endif
        }

        public int getLength()
        {
            return (int)numLength.getFloatValue();
        }

        public void setLength( int value )
        {
            numLength.setFloatValue( value );
        }

        public int getPosition()
        {
            return (int)numPosition.getFloatValue();
        }

        public void setPosition( int value )
        {
            numPosition.setFloatValue( value );
        }
        #endregion

        #region helper methods
        private static String _( String id )
        {
            return Messaging.getMessage( id );
        }

        private void registerEventHandlers()
        {
            btnOK.Click += new BEventHandler( btnOK_Click );
            btnCancel.Click += new BEventHandler( btnCancel_Click );
        }

        private void setResources()
        {
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

        #region UI implementation
#if JAVA
        //INCLUDE-SECTION FIELD ./ui/java/FormInsertBar.java
        //INCLUDE-SECTION METHOD ./ui/java/FormInsertBar.java
#else
        private System.ComponentModel.IContainer components = null;
        private NumericUpDownEx numPosition;
        private NumericUpDownEx numLength;
        private BLabel lblPosition;
        private BLabel lblLength;
        private BLabel lblThBar;
        private BLabel lblBar;
        private BButton btnCancel;
        private BButton btnOK;
        private BLabel lblPositionPrefix;

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

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblPosition = new cadencii.windows.forms.BLabel();
            this.lblLength = new cadencii.windows.forms.BLabel();
            this.lblThBar = new cadencii.windows.forms.BLabel();
            this.lblBar = new cadencii.windows.forms.BLabel();
            this.btnCancel = new cadencii.windows.forms.BButton();
            this.btnOK = new cadencii.windows.forms.BButton();
            this.lblPositionPrefix = new cadencii.windows.forms.BLabel();
            this.numLength = new cadencii.NumericUpDownEx();
            this.numPosition = new cadencii.NumericUpDownEx();
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPosition)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPosition
            // 
            this.lblPosition.AutoSize = true;
            this.lblPosition.Location = new System.Drawing.Point( 12, 14 );
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size( 46, 12 );
            this.lblPosition.TabIndex = 2;
            this.lblPosition.Text = "Position";
            // 
            // lblLength
            // 
            this.lblLength.AutoSize = true;
            this.lblLength.Location = new System.Drawing.Point( 12, 39 );
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size( 39, 12 );
            this.lblLength.TabIndex = 3;
            this.lblLength.Text = "Length";
            // 
            // lblThBar
            // 
            this.lblThBar.AutoSize = true;
            this.lblThBar.Location = new System.Drawing.Point( 136, 14 );
            this.lblThBar.Name = "lblThBar";
            this.lblThBar.Size = new System.Drawing.Size( 35, 12 );
            this.lblThBar.TabIndex = 4;
            this.lblThBar.Text = "th bar";
            // 
            // lblBar
            // 
            this.lblBar.AutoSize = true;
            this.lblBar.Location = new System.Drawing.Point( 136, 39 );
            this.lblBar.Name = "lblBar";
            this.lblBar.Size = new System.Drawing.Size( 21, 12 );
            this.lblBar.TabIndex = 5;
            this.lblBar.Text = "bar";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 134, 67 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point( 53, 67 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // lblPositionPrefix
            // 
            this.lblPositionPrefix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPositionPrefix.AutoSize = true;
            this.lblPositionPrefix.Location = new System.Drawing.Point( 62, 14 );
            this.lblPositionPrefix.Name = "lblPositionPrefix";
            this.lblPositionPrefix.Size = new System.Drawing.Size( 0, 12 );
            this.lblPositionPrefix.TabIndex = 8;
            this.lblPositionPrefix.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // numLength
            // 
            this.numLength.Location = new System.Drawing.Point( 78, 37 );
            this.numLength.Maximum = new decimal( new int[] {
            32,
            0,
            0,
            0} );
            this.numLength.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.numLength.Name = "numLength";
            this.numLength.Size = new System.Drawing.Size( 52, 19 );
            this.numLength.TabIndex = 1;
            this.numLength.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            // 
            // numPosition
            // 
            this.numPosition.Location = new System.Drawing.Point( 78, 12 );
            this.numPosition.Name = "numPosition";
            this.numPosition.Size = new System.Drawing.Size( 52, 19 );
            this.numPosition.TabIndex = 0;
            // 
            // FormInsertBar
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 96F, 96F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 221, 102 );
            this.Controls.Add( this.numLength );
            this.Controls.Add( this.numPosition );
            this.Controls.Add( this.lblPositionPrefix );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.lblBar );
            this.Controls.Add( this.lblThBar );
            this.Controls.Add( this.lblLength );
            this.Controls.Add( this.lblPosition );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormInsertBar";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Insert bar";
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPosition)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

#endif
        #endregion

    }

#if !JAVA
}
#endif
