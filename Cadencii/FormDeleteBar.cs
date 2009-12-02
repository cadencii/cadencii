/*
 * FormDeleteBar.cs
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
#if JAVA
package org.kbinani.Cadencii;

//INCLUDE-SECTION IMPORT ..\BuildJavaUI\src\FormDeleteBar.java
import org.kbinani.*;
#else
using System;
using Boare.Lib.AppUtil;
using bocoree.windows.forms;

namespace Boare.Cadencii {
    using BEventArgs = System.EventArgs;
#endif

#if JAVA
    public class FormDeleteBar extends BForm{
#else
    class FormDeleteBar : BForm {
#endif
        public FormDeleteBar( int max_barcount ) {
#if JAVA
            super();
            initialize();
#else
            InitializeComponent();
#endif
            registerEventHandlers();
            setResources();
            ApplyLanguage();
            numStart.setMaximum( max_barcount );
            numEnd.setMaximum( max_barcount );
            Util.applyFontRecurse( this, AppManager.editorConfig.getBaseFont() );
        }

        public void ApplyLanguage() {
            setTitle( _( "Delete Bars" ) );
            lblStart.setText( _( "Start" ) );
            lblEnd.setText( _( "End" ) );
            btnOK.setText( _( "OK" ) );
            btnCancel.setText( _( "Cancel" ) );
        }

        public static String _( String id ) {
            return Messaging.getMessage( id );
        }

        public int getStart() {
            return (int)numStart.getValue();
        }

        public void setStart( int value ) {
            numStart.setValue( value );
        }

        public int getEnd() {
            return (int)numEnd.getValue();
        }

        public void setEnd( int value ) {
            numEnd.setValue( value );
        }

        private void btnOK_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.OK );
        }

        private void btnCancel_Click( Object sender, BEventArgs e ) {
            setDialogResult( BDialogResult.CANCEL );
        }

        private void registerEventHandlers() {
#if JAVA
            this.btnOK.clickEvent.add( new BEventHandler( this, "btnOK_Click" ) );
            btnCancel.clickEvent.add( new BEventHandler( this, "btnCancel_Click" ) );
#else
            this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
            btnCancel.Click += new EventHandler( btnCancel_Click );
#endif
        }

        private void setResources() {
        }

#if JAVA
        #region UI Impl for Java
        //INCLUDE-SECTION FIELD ..\BuildJavaUI\src\FormDeleteBar.java
        //INCLUDE-SECTION METHOD ..\BuildJavaUI\src\FormDeleteBar.java
        #endregion
#else
        #region UI Impl for C#
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose( bool disposing ) {
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
        private void InitializeComponent() {
            this.btnOK = new BButton();
            this.btnCancel = new BButton();
            this.label4 = new BLabel();
            this.label3 = new BLabel();
            this.lblEnd = new BLabel();
            this.lblStart = new BLabel();
            this.numEnd = new NumericUpDownEx();
            this.numStart = new NumericUpDownEx();
            ((System.ComponentModel.ISupportInitialize)(this.numEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStart)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point( 37, 66 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 118, 66 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 125, 38 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 33, 12 );
            this.label4.TabIndex = 13;
            this.label4.Text = ":0:000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 125, 13 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 33, 12 );
            this.label3.TabIndex = 12;
            this.label3.Text = ":0:000";
            // 
            // lblEnd
            // 
            this.lblEnd.Location = new System.Drawing.Point( 12, 38 );
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size( 49, 12 );
            this.lblEnd.TabIndex = 11;
            this.lblEnd.Text = "End";
            this.lblEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStart
            // 
            this.lblStart.Location = new System.Drawing.Point( 12, 13 );
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size( 49, 12 );
            this.lblStart.TabIndex = 10;
            this.lblStart.Text = "Start";
            this.lblStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numEnd
            // 
            this.numEnd.Location = new System.Drawing.Point( 67, 36 );
            this.numEnd.Maximum = new decimal( new int[] {
            32,
            0,
            0,
            0} );
            this.numEnd.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.numEnd.Name = "numEnd";
            this.numEnd.Size = new System.Drawing.Size( 52, 19 );
            this.numEnd.TabIndex = 9;
            this.numEnd.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            // 
            // numStart
            // 
            this.numStart.Location = new System.Drawing.Point( 67, 11 );
            this.numStart.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.numStart.Name = "numStart";
            this.numStart.Size = new System.Drawing.Size( 52, 19 );
            this.numStart.TabIndex = 8;
            this.numStart.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            // 
            // FormDeleteBar
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 204, 100 );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.label4 );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.lblEnd );
            this.Controls.Add( this.lblStart );
            this.Controls.Add( this.numEnd );
            this.Controls.Add( this.numStart );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDeleteBar";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Delete Bars";
            ((System.ComponentModel.ISupportInitialize)(this.numEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStart)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private BButton btnOK;
        private BButton btnCancel;
        private BLabel label4;
        private BLabel label3;
        private BLabel lblEnd;
        private BLabel lblStart;
        private NumericUpDownEx numEnd;
        private NumericUpDownEx numStart;
        #endregion
#endif
    }

#if !JAVA
}
#endif
