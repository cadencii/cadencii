/*
 * FormRandomize.cs
 * Copyright (C) 2009-2010 kbinani
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

import org.kbinani.*;
import org.kbinani.windows.forms.*;
#else
using System;
using org.kbinani.vsq;
using org.kbinani.apputil;
using org.kbinani.windows.forms;

namespace org.kbinani.cadencii {
    using boolean = System.Boolean;
#endif

#if JAVA
    public class FormRandomize extends BForm {
#else
    public class FormRandomize : BForm {
#endif
        private static boolean lastPositionRandomizeEnabled = true;
        private static int lastPositionRandomizeValue = 3;
        private static boolean lastPitRandomizeEnabled = true;
        private static int lastResolution = 5;
        private static int lastPitRandomizeValue = 3;
        private static int lastPitRandomizePattern = 1;
        private static int lastStartBar = 1;
        private static int lastStartBeat = 1;
        private static int lastEndBar = 2;
        private static int lastEndBeat = 1;
        /// <summary>
        /// trueなら、numStartBar, numStartBeat, numEndBar, numEndBeatの値が変更されたときに、イベントハンドラを起動しない
        /// </summary>
        private boolean lockRequired = false;
        
        private NumericUpDownEx numStartBar;
        private BLabel lblStart;
        private BLabel lblStartBar;
        private NumericUpDownEx numStartBeat;
        private BLabel lblStartBeat;
        private BLabel bLabel1;
        private BLabel lblEndBeat;
        private NumericUpDownEx numEndBeat;
        private BLabel lblEndBar;
        private BLabel lblEnd;
        private NumericUpDownEx numEndBar;
        private BCheckBox chkShift;
        private BLabel lblShiftValue;
        private BComboBox comboShiftValue;
        private BComboBox comboPitPattern;
        private BLabel lblPitPattern;
        private BCheckBox chkPit;
        private BComboBox comboPitValue;
        private BLabel lblPitValue;
        private BLabel lblResolution;
        private NumericUpDownEx numResolution;
        private BButton btnCancel;
        private BButton btnOK;

        public FormRandomize() {
            InitializeComponent();
            registerEventHandlers();
            applyLanguage();
            chkShift.setSelected( lastPositionRandomizeEnabled );
            comboShiftValue.setSelectedIndex( lastPositionRandomizeValue - 1 );
            chkPit.setSelected( lastPitRandomizeEnabled );
            numResolution.setValue( lastResolution );
            comboPitPattern.setSelectedIndex( lastPitRandomizePattern - 1 );
            comboPitValue.setSelectedIndex( lastPitRandomizeValue - 1 );
            lockRequired = true;
            numStartBar.setValue( lastStartBar );
            numStartBeat.setValue( lastStartBeat );
            numEndBar.setValue( lastEndBar );
            numEndBeat.setValue( lastEndBeat );
            lockRequired = false;
        }

        /// <summary>
        /// numStartBar, numStartBeat, numEndBar, numEndBeatの値の範囲の妥当性をチェックする
        /// </summary>
        private void validateNumRange() {
            int startBar = getStartBar();
            int startBeat = getStartBeat();
            int endBar = getEndBar();
            int endBeat = getEndBeat();
            VsqFileEx vsq = AppManager.getVsqFile();
            if ( vsq == null ) {
                return;
            }

            int preMeasure = vsq.getPreMeasure();
            startBar += (preMeasure - 1); // 曲頭からの小節数は、表示上の小節数と(preMeasure - 1)だけずれているので。
            endBar += (preMeasure - 1);
            startBeat--;
            endBeat--;
            
            int startBarClock = vsq.getClockFromBarCount( startBar ); // startBar小節開始位置のゲートタイム
            Timesig startTimesig = vsq.getTimesigAt( startBarClock );    // startBar小節開始位置の拍子
            int startClock = startBarClock + startBeat * 480 * 4 / startTimesig.denominator;  // 第startBar小節の第startBeat拍開始位置のゲートタイム

            int endBarClock = vsq.getClockFromBarCount( endBar );
            Timesig endTimesig = vsq.getTimesigAt( endBarClock );
            int endClock = endBarClock + endBeat * 480 * 4 / endTimesig.denominator;

            if ( endClock <= startClock ) {
                // 選択範囲が0以下の場合、値を強制的に変更する
                // ここでは、一拍分を選択するように変更
                endClock = startClock + 480 * 4 / startTimesig.denominator;
                endBar = vsq.getBarCountFromClock( endClock );
                int remain = endClock - vsq.getClockFromBarCount( endBar );
                endTimesig = vsq.getTimesigAt( endClock );
                endBeat = remain / (480 * 4 / endTimesig.denominator);
            }

            // numStartBarの最大値・最小値を決定
            int startBarMax = endBar - 1;
            if ( startBeat < endBeat ) {
                startBarMax = endBar;
            }
            int startBarMin = 1;

            // numStartBeatの最大値・最小値を決定
            int startBeatMax = startTimesig.numerator;
            if ( startBar == endBar ) {
                startBeatMax = endBeat - 1;
            }
            int startBeatMin = 1;

            // numEndBarの最大値・最小値を決定
            int endBarMax = int.MaxValue;
            int endBarMin = startBar + 1;
            if ( startBeat < endBeat ) {
                endBarMin = startBar;
            }

            // numEndBeatの最大値・最小値の決定
            int endBeatMax = endTimesig.numerator;
            int endBeatMin = 1;
            if ( startBar == endBar ) {
                endBeatMin = startBeat + 1;
            }

            lockRequired = true;
            numStartBar.setMaximum( startBarMax );
            numStartBar.setMinimum( startBarMin );
            numStartBeat.setMaximum( startBeatMax );
            numStartBeat.setMinimum( startBeatMin );
            numEndBar.setMaximum( endBarMax );
            numEndBar.setMinimum( endBarMin );
            numEndBeat.setMaximum( endBeatMax );
            numEndBeat.setMinimum( endBeatMin );
            lockRequired = false;
        }

        public void chkShift_CheckedChanged( Object sender, EventArgs e ) {
            boolean v = chkShift.isSelected();
            comboShiftValue.setEnabled( v );
        }

        public void chkPit_CheckedChanged( Object sender, EventArgs e ) {
            boolean v = chkPit.isSelected();
            numResolution.setEnabled( v );
            comboPitPattern.setEnabled( v );
            comboPitValue.setEnabled( v );
        }

        public void numCommon_ValueChanged( Object sender, EventArgs e ) {
            if ( lockRequired ) {
                return;
            }
            validateNumRange();
        }

        public void btnCancel_Click( Object sender, EventArgs e ) {
            setDialogResult( BDialogResult.CANCEL );
        }

        public void btnOK_Click( Object sender, EventArgs e ) {
            lastPositionRandomizeEnabled = isPositionRandomizeEnabled();
            lastPositionRandomizeValue = getPositionRandomizeValue();
            lastPitRandomizeEnabled = isPitRandomizeEnabled();
            lastPitRandomizePattern = getPitRandomizePattern();
            lastPitRandomizeValue = getPitRandomizeValue();
            lastResolution = getResolution();
            lastStartBar = getStartBar();
            lastStartBeat = getStartBeat();
            lastEndBar = getEndBar();
            lastEndBeat = getEndBeat();
            setDialogResult( BDialogResult.OK );
        }

        public int getResolution() {
            return (int)numResolution.getValue();
        }

        public int getStartBar() {
            return (int)numStartBar.getValue();
        }

        public int getStartBeat() {
            return (int)numStartBeat.getValue();
        }

        public int getEndBar() {
            return (int)numEndBar.getValue();
        }

        public int getEndBeat() {
            return (int)numEndBeat.getValue();
        }

        public boolean isPositionRandomizeEnabled() {
            return chkShift.isSelected();
        }

        public int getPositionRandomizeValue() {
            int draft = comboShiftValue.getSelectedIndex() + 1;
            if ( draft <= 0 ) {
                draft = 1;
            }
            return draft;
        }

        public boolean isPitRandomizeEnabled() {
            return chkPit.isSelected();
        }

        public int getPitRandomizeValue() {
            int draft = comboPitValue.getSelectedIndex() + 1;
            if ( draft <= 0 ) {
                draft = 1;
            }
            return draft;
        }

        public int getPitRandomizePattern() {
            int draft = comboPitPattern.getSelectedIndex() + 1;
            if ( draft <= 0 ) {
                draft = 1;
            }
            return draft;
        }

        private static String _( String id ) {
            return Messaging.getMessage( id );
        }

        private void applyLanguage() {
            lblStart.setText( _( "Start" ) );
            lblStartBar.setText( _( "bar" ) );
            lblStartBeat.setText( _( "beat" ) );
            lblEnd.setText( _( "End" ) );
            lblEndBar.setText( _( "bar" ) );
            lblEndBeat.setText( _( "beat" ) );

            chkShift.setText( _( "Note Shift" ) );
            lblShiftValue.setText( _( "Value" ) );

            chkPit.setText( _( "Pitch Fluctuation" ) );
            lblResolution.setText( _( "Resolution" ) );
            lblPitPattern.setText( _( "Pattern" ) );
            lblPitValue.setText( _( "Value" ) );

            btnOK.setText( _( "OK" ) );
            btnCancel.setText( _( "Cancel" ) );

            setTitle( _( "Randomize" ) );
        }

        private void registerEventHandlers() {
            btnOK.clickEvent.add( new BEventHandler( this, "btnOK_Click" ) );
            btnCancel.clickEvent.add( new BEventHandler( this, "btnCancel_Click" ) );
            numStartBar.valueChangedEvent.add( new BEventHandler( this, "numCommon_ValueChanged" ) );
            numStartBeat.valueChangedEvent.add( new BEventHandler( this, "numCommon_ValueChanged" ) );
            numEndBar.valueChangedEvent.add( new BEventHandler( this, "numCommon_ValueChanged" ) );
            numEndBeat.valueChangedEvent.add( new BEventHandler( this, "numCommon_ValueChanged" ) );
            chkShift.checkedChangedEvent.add( new BEventHandler( this, "chkShift_CheckedChanged" ) );
            chkPit.checkedChangedEvent.add( new BEventHandler( this, "chkPit_CheckedChanged" ) );
        }

#if JAVA
#else
        #region UI impl for C#
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
            this.lblStart = new org.kbinani.windows.forms.BLabel();
            this.lblStartBar = new org.kbinani.windows.forms.BLabel();
            this.lblStartBeat = new org.kbinani.windows.forms.BLabel();
            this.bLabel1 = new org.kbinani.windows.forms.BLabel();
            this.lblEndBeat = new org.kbinani.windows.forms.BLabel();
            this.lblEndBar = new org.kbinani.windows.forms.BLabel();
            this.lblEnd = new org.kbinani.windows.forms.BLabel();
            this.chkShift = new org.kbinani.windows.forms.BCheckBox();
            this.lblShiftValue = new org.kbinani.windows.forms.BLabel();
            this.comboShiftValue = new org.kbinani.windows.forms.BComboBox();
            this.comboPitPattern = new org.kbinani.windows.forms.BComboBox();
            this.lblPitPattern = new org.kbinani.windows.forms.BLabel();
            this.chkPit = new org.kbinani.windows.forms.BCheckBox();
            this.comboPitValue = new org.kbinani.windows.forms.BComboBox();
            this.lblPitValue = new org.kbinani.windows.forms.BLabel();
            this.lblResolution = new org.kbinani.windows.forms.BLabel();
            this.btnCancel = new org.kbinani.windows.forms.BButton();
            this.btnOK = new org.kbinani.windows.forms.BButton();
            this.numResolution = new org.kbinani.cadencii.NumericUpDownEx();
            this.numEndBeat = new org.kbinani.cadencii.NumericUpDownEx();
            this.numEndBar = new org.kbinani.cadencii.NumericUpDownEx();
            this.numStartBeat = new org.kbinani.cadencii.NumericUpDownEx();
            this.numStartBar = new org.kbinani.cadencii.NumericUpDownEx();
            ((System.ComponentModel.ISupportInitialize)(this.numResolution)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEndBeat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEndBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartBeat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartBar)).BeginInit();
            this.SuspendLayout();
            // 
            // lblStart
            // 
            this.lblStart.Location = new System.Drawing.Point( 12, 26 );
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size( 35, 17 );
            this.lblStart.TabIndex = 2;
            this.lblStart.Text = "start";
            this.lblStart.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblStartBar
            // 
            this.lblStartBar.AutoSize = true;
            this.lblStartBar.Location = new System.Drawing.Point( 58, 9 );
            this.lblStartBar.Name = "lblStartBar";
            this.lblStartBar.Size = new System.Drawing.Size( 21, 12 );
            this.lblStartBar.TabIndex = 3;
            this.lblStartBar.Text = "bar";
            // 
            // lblStartBeat
            // 
            this.lblStartBeat.AutoSize = true;
            this.lblStartBeat.Location = new System.Drawing.Point( 116, 9 );
            this.lblStartBeat.Name = "lblStartBeat";
            this.lblStartBeat.Size = new System.Drawing.Size( 27, 12 );
            this.lblStartBeat.TabIndex = 5;
            this.lblStartBeat.Text = "beat";
            // 
            // bLabel1
            // 
            this.bLabel1.AutoSize = true;
            this.bLabel1.Location = new System.Drawing.Point( 173, 26 );
            this.bLabel1.Name = "bLabel1";
            this.bLabel1.Size = new System.Drawing.Size( 11, 12 );
            this.bLabel1.TabIndex = 6;
            this.bLabel1.Text = "-";
            // 
            // lblEndBeat
            // 
            this.lblEndBeat.AutoSize = true;
            this.lblEndBeat.Location = new System.Drawing.Point( 285, 9 );
            this.lblEndBeat.Name = "lblEndBeat";
            this.lblEndBeat.Size = new System.Drawing.Size( 27, 12 );
            this.lblEndBeat.TabIndex = 11;
            this.lblEndBeat.Text = "beat";
            // 
            // lblEndBar
            // 
            this.lblEndBar.AutoSize = true;
            this.lblEndBar.Location = new System.Drawing.Point( 225, 9 );
            this.lblEndBar.Name = "lblEndBar";
            this.lblEndBar.Size = new System.Drawing.Size( 21, 12 );
            this.lblEndBar.TabIndex = 9;
            this.lblEndBar.Text = "bar";
            // 
            // lblEnd
            // 
            this.lblEnd.Location = new System.Drawing.Point( 190, 26 );
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size( 26, 17 );
            this.lblEnd.TabIndex = 8;
            this.lblEnd.Text = "end";
            this.lblEnd.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkShift
            // 
            this.chkShift.AutoSize = true;
            this.chkShift.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkShift.Checked = true;
            this.chkShift.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShift.Location = new System.Drawing.Point( 14, 70 );
            this.chkShift.Name = "chkShift";
            this.chkShift.Size = new System.Drawing.Size( 76, 16 );
            this.chkShift.TabIndex = 12;
            this.chkShift.Text = "Note Shift";
            this.chkShift.UseVisualStyleBackColor = true;
            // 
            // lblShiftValue
            // 
            this.lblShiftValue.AutoSize = true;
            this.lblShiftValue.Location = new System.Drawing.Point( 51, 98 );
            this.lblShiftValue.Name = "lblShiftValue";
            this.lblShiftValue.Size = new System.Drawing.Size( 34, 12 );
            this.lblShiftValue.TabIndex = 13;
            this.lblShiftValue.Text = "Value";
            // 
            // comboShiftValue
            // 
            this.comboShiftValue.FormattingEnabled = true;
            this.comboShiftValue.Items.AddRange( new object[] {
            "1(small)",
            "2",
            "3(medium)",
            "4",
            "5(large)"} );
            this.comboShiftValue.Location = new System.Drawing.Point( 103, 95 );
            this.comboShiftValue.Name = "comboShiftValue";
            this.comboShiftValue.Size = new System.Drawing.Size( 218, 20 );
            this.comboShiftValue.TabIndex = 14;
            // 
            // comboPitPattern
            // 
            this.comboPitPattern.FormattingEnabled = true;
            this.comboPitPattern.Items.AddRange( new object[] {
            "Pattern 1",
            "Pattern 2",
            "Pattern 3"} );
            this.comboPitPattern.Location = new System.Drawing.Point( 103, 173 );
            this.comboPitPattern.Name = "comboPitPattern";
            this.comboPitPattern.Size = new System.Drawing.Size( 218, 20 );
            this.comboPitPattern.TabIndex = 17;
            // 
            // lblPitPattern
            // 
            this.lblPitPattern.AutoSize = true;
            this.lblPitPattern.Location = new System.Drawing.Point( 51, 176 );
            this.lblPitPattern.Name = "lblPitPattern";
            this.lblPitPattern.Size = new System.Drawing.Size( 42, 12 );
            this.lblPitPattern.TabIndex = 16;
            this.lblPitPattern.Text = "Pattern";
            // 
            // chkPit
            // 
            this.chkPit.AutoSize = true;
            this.chkPit.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkPit.Checked = true;
            this.chkPit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPit.Location = new System.Drawing.Point( 14, 148 );
            this.chkPit.Name = "chkPit";
            this.chkPit.Size = new System.Drawing.Size( 111, 16 );
            this.chkPit.TabIndex = 15;
            this.chkPit.Text = "Pitch Fluctuation";
            this.chkPit.UseVisualStyleBackColor = true;
            // 
            // comboPitValue
            // 
            this.comboPitValue.FormattingEnabled = true;
            this.comboPitValue.Items.AddRange( new object[] {
            "1(small)",
            "2",
            "3(medium)",
            "4",
            "5(large)"} );
            this.comboPitValue.Location = new System.Drawing.Point( 103, 199 );
            this.comboPitValue.Name = "comboPitValue";
            this.comboPitValue.Size = new System.Drawing.Size( 218, 20 );
            this.comboPitValue.TabIndex = 19;
            // 
            // lblPitValue
            // 
            this.lblPitValue.AutoSize = true;
            this.lblPitValue.Location = new System.Drawing.Point( 51, 202 );
            this.lblPitValue.Name = "lblPitValue";
            this.lblPitValue.Size = new System.Drawing.Size( 34, 12 );
            this.lblPitValue.TabIndex = 18;
            this.lblPitValue.Text = "Value";
            // 
            // lblResolution
            // 
            this.lblResolution.Location = new System.Drawing.Point( 175, 149 );
            this.lblResolution.Name = "lblResolution";
            this.lblResolution.Size = new System.Drawing.Size( 86, 15 );
            this.lblResolution.TabIndex = 20;
            this.lblResolution.Text = "Resolution";
            this.lblResolution.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point( 246, 246 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 23;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point( 165, 246 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 75, 23 );
            this.btnOK.TabIndex = 22;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // numResolution
            // 
            this.numResolution.Location = new System.Drawing.Point( 267, 147 );
            this.numResolution.Maximum = new decimal( new int[] {
            30,
            0,
            0,
            0} );
            this.numResolution.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            this.numResolution.Name = "numResolution";
            this.numResolution.Size = new System.Drawing.Size( 54, 19 );
            this.numResolution.TabIndex = 21;
            this.numResolution.Value = new decimal( new int[] {
            5,
            0,
            0,
            0} );
            // 
            // numEndBeat
            // 
            this.numEndBeat.Location = new System.Drawing.Point( 282, 24 );
            this.numEndBeat.Name = "numEndBeat";
            this.numEndBeat.Size = new System.Drawing.Size( 54, 19 );
            this.numEndBeat.TabIndex = 10;
            this.numEndBeat.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            // 
            // numEndBar
            // 
            this.numEndBar.Location = new System.Drawing.Point( 222, 24 );
            this.numEndBar.Maximum = new decimal( new int[] {
            2147483647,
            0,
            0,
            0} );
            this.numEndBar.Name = "numEndBar";
            this.numEndBar.Size = new System.Drawing.Size( 54, 19 );
            this.numEndBar.TabIndex = 7;
            this.numEndBar.Value = new decimal( new int[] {
            2,
            0,
            0,
            0} );
            // 
            // numStartBeat
            // 
            this.numStartBeat.Location = new System.Drawing.Point( 113, 24 );
            this.numStartBeat.Name = "numStartBeat";
            this.numStartBeat.Size = new System.Drawing.Size( 54, 19 );
            this.numStartBeat.TabIndex = 4;
            this.numStartBeat.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            // 
            // numStartBar
            // 
            this.numStartBar.Location = new System.Drawing.Point( 53, 24 );
            this.numStartBar.Name = "numStartBar";
            this.numStartBar.Size = new System.Drawing.Size( 54, 19 );
            this.numStartBar.TabIndex = 1;
            this.numStartBar.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
            // 
            // FormRandomize
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size( 347, 286 );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.numResolution );
            this.Controls.Add( this.lblResolution );
            this.Controls.Add( this.comboPitValue );
            this.Controls.Add( this.lblPitValue );
            this.Controls.Add( this.comboPitPattern );
            this.Controls.Add( this.lblPitPattern );
            this.Controls.Add( this.chkPit );
            this.Controls.Add( this.comboShiftValue );
            this.Controls.Add( this.lblShiftValue );
            this.Controls.Add( this.chkShift );
            this.Controls.Add( this.lblEndBeat );
            this.Controls.Add( this.numEndBeat );
            this.Controls.Add( this.lblEndBar );
            this.Controls.Add( this.lblEnd );
            this.Controls.Add( this.numEndBar );
            this.Controls.Add( this.bLabel1 );
            this.Controls.Add( this.lblStartBeat );
            this.Controls.Add( this.numStartBeat );
            this.Controls.Add( this.lblStartBar );
            this.Controls.Add( this.lblStart );
            this.Controls.Add( this.numStartBar );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRandomize";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Randomize";
            ((System.ComponentModel.ISupportInitialize)(this.numResolution)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEndBeat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEndBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartBeat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartBar)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }
        #endregion

        #endregion
#endif
    }

#if !JAVA
}
#endif
