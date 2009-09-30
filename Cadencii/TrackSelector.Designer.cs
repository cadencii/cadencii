/*
 * TrackSelector.Designer.cs
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
namespace Boare.Cadencii {
    using boolean = System.Boolean;
    partial class TrackSelector {
        /// <summary> 
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose( boolean disposing ) {
            if ( disposing && (components != null) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.cmenuSinger = new System.Windows.Forms.ContextMenuStrip( this.components );
            this.toolTip = new System.Windows.Forms.ToolTip( this.components );
            this.cmenuCurve = new System.Windows.Forms.ContextMenuStrip( this.components );
            this.cmenuCurveVelocity = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveAccent = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveDecay = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmenuCurveDynamics = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveVibratoRate = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveVibratoDepth = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmenuCurveReso1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveReso1Freq = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveReso1BW = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveReso1Amp = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveReso2 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveReso2Freq = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveReso2BW = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveReso2Amp = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveReso3 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveReso3Freq = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveReso3BW = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveReso3Amp = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveReso4 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveReso4Freq = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveReso4BW = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveReso4Amp = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmenuCurveHarmonics = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveBreathiness = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveBrightness = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveClearness = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveOpening = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveGenderFactor = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.cmenuCurvePortamentoTiming = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurvePitchBend = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurvePitchBendSensitivity = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurveSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.cmenuCurveEffect2Depth = new System.Windows.Forms.ToolStripMenuItem();
            this.vScroll = new System.Windows.Forms.VScrollBar();
            this.panelZoomButton = new System.Windows.Forms.Panel();
            this.cmenuCurveEnvelope = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuCurve.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmenuSinger
            // 
            this.cmenuSinger.Name = "cmenuSinger";
            this.cmenuSinger.Size = new System.Drawing.Size( 61, 4 );
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.OwnerDraw = true;
            this.toolTip.ReshowDelay = 0;
            this.toolTip.Draw += new System.Windows.Forms.DrawToolTipEventHandler( this.toolTip_Draw );
            // 
            // cmenuCurve
            // 
            this.cmenuCurve.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cmenuCurveVelocity,
            this.cmenuCurveAccent,
            this.cmenuCurveDecay,
            this.cmenuCurveSeparator1,
            this.cmenuCurveDynamics,
            this.cmenuCurveVibratoRate,
            this.cmenuCurveVibratoDepth,
            this.cmenuCurveSeparator2,
            this.cmenuCurveReso1,
            this.cmenuCurveReso2,
            this.cmenuCurveReso3,
            this.cmenuCurveReso4,
            this.cmenuCurveSeparator3,
            this.cmenuCurveHarmonics,
            this.cmenuCurveBreathiness,
            this.cmenuCurveBrightness,
            this.cmenuCurveClearness,
            this.cmenuCurveOpening,
            this.cmenuCurveGenderFactor,
            this.cmenuCurveSeparator4,
            this.cmenuCurvePortamentoTiming,
            this.cmenuCurvePitchBend,
            this.cmenuCurvePitchBendSensitivity,
            this.cmenuCurveSeparator5,
            this.cmenuCurveEffect2Depth,
            this.cmenuCurveEnvelope} );
            this.cmenuCurve.Name = "cmenuCurve";
            this.cmenuCurve.Size = new System.Drawing.Size( 185, 518 );
            // 
            // cmenuCurveVelocity
            // 
            this.cmenuCurveVelocity.Name = "cmenuCurveVelocity";
            this.cmenuCurveVelocity.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveVelocity.Text = "Velocity(&V)";
            this.cmenuCurveVelocity.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveAccent
            // 
            this.cmenuCurveAccent.Name = "cmenuCurveAccent";
            this.cmenuCurveAccent.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveAccent.Text = "Accent";
            this.cmenuCurveAccent.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveDecay
            // 
            this.cmenuCurveDecay.Name = "cmenuCurveDecay";
            this.cmenuCurveDecay.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveDecay.Text = "Decay";
            this.cmenuCurveDecay.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveSeparator1
            // 
            this.cmenuCurveSeparator1.Name = "cmenuCurveSeparator1";
            this.cmenuCurveSeparator1.Size = new System.Drawing.Size( 181, 6 );
            // 
            // cmenuCurveDynamics
            // 
            this.cmenuCurveDynamics.Name = "cmenuCurveDynamics";
            this.cmenuCurveDynamics.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveDynamics.Text = "Dynamics";
            this.cmenuCurveDynamics.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveVibratoRate
            // 
            this.cmenuCurveVibratoRate.Name = "cmenuCurveVibratoRate";
            this.cmenuCurveVibratoRate.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveVibratoRate.Text = "Vibrato Rate";
            this.cmenuCurveVibratoRate.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveVibratoDepth
            // 
            this.cmenuCurveVibratoDepth.Name = "cmenuCurveVibratoDepth";
            this.cmenuCurveVibratoDepth.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveVibratoDepth.Text = "Vibrato Depth";
            this.cmenuCurveVibratoDepth.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveSeparator2
            // 
            this.cmenuCurveSeparator2.Name = "cmenuCurveSeparator2";
            this.cmenuCurveSeparator2.Size = new System.Drawing.Size( 181, 6 );
            // 
            // cmenuCurveReso1
            // 
            this.cmenuCurveReso1.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cmenuCurveReso1Freq,
            this.cmenuCurveReso1BW,
            this.cmenuCurveReso1Amp} );
            this.cmenuCurveReso1.Name = "cmenuCurveReso1";
            this.cmenuCurveReso1.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveReso1.Text = "Resonance 1";
            // 
            // cmenuCurveReso1Freq
            // 
            this.cmenuCurveReso1Freq.Name = "cmenuCurveReso1Freq";
            this.cmenuCurveReso1Freq.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso1Freq.Text = "Frequency";
            this.cmenuCurveReso1Freq.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveReso1BW
            // 
            this.cmenuCurveReso1BW.Name = "cmenuCurveReso1BW";
            this.cmenuCurveReso1BW.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso1BW.Text = "Band Width";
            this.cmenuCurveReso1BW.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveReso1Amp
            // 
            this.cmenuCurveReso1Amp.Name = "cmenuCurveReso1Amp";
            this.cmenuCurveReso1Amp.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso1Amp.Text = "Amplitude";
            this.cmenuCurveReso1Amp.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveReso2
            // 
            this.cmenuCurveReso2.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cmenuCurveReso2Freq,
            this.cmenuCurveReso2BW,
            this.cmenuCurveReso2Amp} );
            this.cmenuCurveReso2.Name = "cmenuCurveReso2";
            this.cmenuCurveReso2.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveReso2.Text = "Resonance 2";
            // 
            // cmenuCurveReso2Freq
            // 
            this.cmenuCurveReso2Freq.Name = "cmenuCurveReso2Freq";
            this.cmenuCurveReso2Freq.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso2Freq.Text = "Frequency";
            this.cmenuCurveReso2Freq.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveReso2BW
            // 
            this.cmenuCurveReso2BW.Name = "cmenuCurveReso2BW";
            this.cmenuCurveReso2BW.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso2BW.Text = "Band Width";
            this.cmenuCurveReso2BW.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveReso2Amp
            // 
            this.cmenuCurveReso2Amp.Name = "cmenuCurveReso2Amp";
            this.cmenuCurveReso2Amp.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso2Amp.Text = "Amplitude";
            this.cmenuCurveReso2Amp.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveReso3
            // 
            this.cmenuCurveReso3.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cmenuCurveReso3Freq,
            this.cmenuCurveReso3BW,
            this.cmenuCurveReso3Amp} );
            this.cmenuCurveReso3.Name = "cmenuCurveReso3";
            this.cmenuCurveReso3.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveReso3.Text = "Resonance 3";
            // 
            // cmenuCurveReso3Freq
            // 
            this.cmenuCurveReso3Freq.Name = "cmenuCurveReso3Freq";
            this.cmenuCurveReso3Freq.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso3Freq.Text = "Frequency";
            this.cmenuCurveReso3Freq.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveReso3BW
            // 
            this.cmenuCurveReso3BW.Name = "cmenuCurveReso3BW";
            this.cmenuCurveReso3BW.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso3BW.Text = "Band Width";
            this.cmenuCurveReso3BW.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveReso3Amp
            // 
            this.cmenuCurveReso3Amp.Name = "cmenuCurveReso3Amp";
            this.cmenuCurveReso3Amp.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso3Amp.Text = "Amplitude";
            this.cmenuCurveReso3Amp.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveReso4
            // 
            this.cmenuCurveReso4.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.cmenuCurveReso4Freq,
            this.cmenuCurveReso4BW,
            this.cmenuCurveReso4Amp} );
            this.cmenuCurveReso4.Name = "cmenuCurveReso4";
            this.cmenuCurveReso4.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveReso4.Text = "Resonance 4";
            // 
            // cmenuCurveReso4Freq
            // 
            this.cmenuCurveReso4Freq.Name = "cmenuCurveReso4Freq";
            this.cmenuCurveReso4Freq.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso4Freq.Text = "Frequency";
            this.cmenuCurveReso4Freq.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveReso4BW
            // 
            this.cmenuCurveReso4BW.Name = "cmenuCurveReso4BW";
            this.cmenuCurveReso4BW.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso4BW.Text = "Band Width";
            this.cmenuCurveReso4BW.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveReso4Amp
            // 
            this.cmenuCurveReso4Amp.Name = "cmenuCurveReso4Amp";
            this.cmenuCurveReso4Amp.Size = new System.Drawing.Size( 128, 22 );
            this.cmenuCurveReso4Amp.Text = "Amplitude";
            this.cmenuCurveReso4Amp.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveSeparator3
            // 
            this.cmenuCurveSeparator3.Name = "cmenuCurveSeparator3";
            this.cmenuCurveSeparator3.Size = new System.Drawing.Size( 181, 6 );
            // 
            // cmenuCurveHarmonics
            // 
            this.cmenuCurveHarmonics.Name = "cmenuCurveHarmonics";
            this.cmenuCurveHarmonics.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveHarmonics.Text = "Harmonics";
            this.cmenuCurveHarmonics.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveBreathiness
            // 
            this.cmenuCurveBreathiness.Name = "cmenuCurveBreathiness";
            this.cmenuCurveBreathiness.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveBreathiness.Text = "Noise";
            this.cmenuCurveBreathiness.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveBrightness
            // 
            this.cmenuCurveBrightness.Name = "cmenuCurveBrightness";
            this.cmenuCurveBrightness.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveBrightness.Text = "Brightness";
            this.cmenuCurveBrightness.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveClearness
            // 
            this.cmenuCurveClearness.Name = "cmenuCurveClearness";
            this.cmenuCurveClearness.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveClearness.Text = "Clearness";
            this.cmenuCurveClearness.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveOpening
            // 
            this.cmenuCurveOpening.Name = "cmenuCurveOpening";
            this.cmenuCurveOpening.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveOpening.Text = "Opening";
            this.cmenuCurveOpening.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveGenderFactor
            // 
            this.cmenuCurveGenderFactor.Name = "cmenuCurveGenderFactor";
            this.cmenuCurveGenderFactor.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveGenderFactor.Text = "Gender Factor";
            this.cmenuCurveGenderFactor.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveSeparator4
            // 
            this.cmenuCurveSeparator4.Name = "cmenuCurveSeparator4";
            this.cmenuCurveSeparator4.Size = new System.Drawing.Size( 181, 6 );
            // 
            // cmenuCurvePortamentoTiming
            // 
            this.cmenuCurvePortamentoTiming.Name = "cmenuCurvePortamentoTiming";
            this.cmenuCurvePortamentoTiming.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurvePortamentoTiming.Text = "Portamento Timing";
            this.cmenuCurvePortamentoTiming.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurvePitchBend
            // 
            this.cmenuCurvePitchBend.Name = "cmenuCurvePitchBend";
            this.cmenuCurvePitchBend.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurvePitchBend.Text = "Pitch Bend";
            this.cmenuCurvePitchBend.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurvePitchBendSensitivity
            // 
            this.cmenuCurvePitchBendSensitivity.Name = "cmenuCurvePitchBendSensitivity";
            this.cmenuCurvePitchBendSensitivity.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurvePitchBendSensitivity.Text = "Pitch Bend Sensitivity";
            this.cmenuCurvePitchBendSensitivity.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // cmenuCurveSeparator5
            // 
            this.cmenuCurveSeparator5.Name = "cmenuCurveSeparator5";
            this.cmenuCurveSeparator5.Size = new System.Drawing.Size( 181, 6 );
            // 
            // cmenuCurveEffect2Depth
            // 
            this.cmenuCurveEffect2Depth.Name = "cmenuCurveEffect2Depth";
            this.cmenuCurveEffect2Depth.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveEffect2Depth.Text = "Effect2 Depth";
            this.cmenuCurveEffect2Depth.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // vScroll
            // 
            this.vScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.vScroll.Enabled = false;
            this.vScroll.Location = new System.Drawing.Point( 414, 0 );
            this.vScroll.Name = "vScroll";
            this.vScroll.Size = new System.Drawing.Size( 16, 193 );
            this.vScroll.TabIndex = 2;
            // 
            // panelZoomButton
            // 
            this.panelZoomButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panelZoomButton.BackColor = System.Drawing.Color.DarkGray;
            this.panelZoomButton.Location = new System.Drawing.Point( 414, 193 );
            this.panelZoomButton.Margin = new System.Windows.Forms.Padding( 0 );
            this.panelZoomButton.Name = "panelZoomButton";
            this.panelZoomButton.Size = new System.Drawing.Size( 16, 33 );
            this.panelZoomButton.TabIndex = 3;
            this.panelZoomButton.Paint += new System.Windows.Forms.PaintEventHandler( this.panelZoomButton_Paint );
            this.panelZoomButton.MouseDown += new System.Windows.Forms.MouseEventHandler( this.panelZoomButton_MouseDown );
            // 
            // cmenuCurveEnvelope
            // 
            this.cmenuCurveEnvelope.Name = "cmenuCurveEnvelope";
            this.cmenuCurveEnvelope.Size = new System.Drawing.Size( 184, 22 );
            this.cmenuCurveEnvelope.Text = "Envelope";
            this.cmenuCurveEnvelope.Click += new System.EventHandler( this.cmenuCurveCommon_Click );
            // 
            // TrackSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.Controls.Add( this.vScroll );
            this.Controls.Add( this.panelZoomButton );
            this.DoubleBuffered = true;
            this.Name = "TrackSelector";
            this.Size = new System.Drawing.Size( 430, 228 );
            this.Load += new System.EventHandler( this.TrackSelector_Load );
            this.MouseMove += new System.Windows.Forms.MouseEventHandler( this.TrackSelector_MouseMove );
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler( this.TrackSelector_MouseDoubleClick );
            this.KeyUp += new System.Windows.Forms.KeyEventHandler( this.TrackSelector_KeyUp );
            this.MouseClick += new System.Windows.Forms.MouseEventHandler( this.TrackSelector_MouseClick );
            this.MouseDown += new System.Windows.Forms.MouseEventHandler( this.TrackSelector_MouseDown );
            this.MouseUp += new System.Windows.Forms.MouseEventHandler( this.TrackSelector_MouseUp );
            this.KeyDown += new System.Windows.Forms.KeyEventHandler( this.TrackSelector_KeyDown );
            this.cmenuCurve.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cmenuSinger;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip cmenuCurve;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveVelocity;
        private System.Windows.Forms.ToolStripSeparator cmenuCurveSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveReso1;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveReso1Freq;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveReso1BW;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveReso1Amp;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveReso2;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveReso2Freq;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveReso2BW;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveReso2Amp;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveReso3;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveReso3Freq;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveReso3BW;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveReso3Amp;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveReso4;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveReso4Freq;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveReso4BW;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveReso4Amp;
        private System.Windows.Forms.ToolStripSeparator cmenuCurveSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveHarmonics;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveDynamics;
        private System.Windows.Forms.ToolStripSeparator cmenuCurveSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveBreathiness;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveBrightness;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveClearness;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveGenderFactor;
        private System.Windows.Forms.ToolStripSeparator cmenuCurveSeparator4;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurvePortamentoTiming;
        private System.Windows.Forms.ToolStripSeparator cmenuCurveSeparator5;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveEffect2Depth;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveOpening;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveAccent;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveDecay;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveVibratoRate;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveVibratoDepth;
        private System.Windows.Forms.VScrollBar vScroll;
        private System.Windows.Forms.Panel panelZoomButton;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurvePitchBend;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurvePitchBendSensitivity;
        private System.Windows.Forms.ToolStripMenuItem cmenuCurveEnvelope;

    }
}
