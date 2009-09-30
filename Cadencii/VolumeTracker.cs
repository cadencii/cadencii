/*
 * VolumeTracker.cs
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
using System.Drawing;
using System.Windows.Forms;

namespace Boare.Cadencii {

    using boolean = Boolean;

    partial class VolumeTracker : UserControl {
        private int m_feder = 0;
        private boolean m_solo_button_visible;
        private boolean m_muted = false;
        private boolean m_solo = false;
        private String m_number = "0";
        private String m_title = "";

        #region Constants
        public const int WIDTH = 85;
        const int _HEIGHT = 261;
        private static readonly int[,] _KEY = {
            {55, 26}, 
            {51, 27},
            {47, 28},
            {42, 30},
            {38, 31},
            {35, 33},
            {31, 34},
            {28, 36},
            {24, 37},
            {21, 39},
            {18, 40},
            {15, 42},
            {12, 43},
            {10, 45},
            {7, 46},
            {5, 48},
            {2, 49},
            {0, 51},
            {-2, 52},
            {-5, 54},
            {-7, 55},
            {-10, 57},
            {-12, 58},
            {-15, 60},
            {-18, 61},
            {-21, 63},
            {-24, 64},
            {-28, 66},
            {-31, 67},
            {-35, 69},
            {-38, 70},
            {-42, 72},
            {-47, 73},
            {-51, 75},
            {-55, 76},
            {-60, 78},
            {-65, 79},
            {-70, 81},
            {-76, 82},
            {-81, 84},
            {-87, 85},
            {-93, 87},
            {-100, 88},
            {-107, 89},
            {-114, 91},
            {-121, 92},
            {-129, 94},
            {-137, 95},
            {-145, 97},
            {-154, 98},
            {-163, 100},
            {-173, 101},
            {-183, 103},
            {-193, 104},
            {-204, 106},
            {-215, 107},
            {-227, 109},
            {-240, 110},
            {-253, 112},
            {-266, 113},
            {-280, 115},
            {-295, 116},
            {-311, 118},
            {-327, 119},
            {-344, 121},
            {-362, 122},
            {-380, 124},
            {-399, 125},
            {-420, 127},
            {-441, 128},
            {-463, 130},
            {-486, 131},
            {-510, 133},
            {-535, 134},
            {-561, 136},
            {-589, 137},
            {-617, 139},
            {-647, 140},
            {-678, 142},
            {-711, 143},
            {-745, 145},
            {-781, 146},
            {-818, 148},
            {-857, 149},
            {-898, 151},
        };
        #endregion

        public event EventHandler FederChanged;
        public event EventHandler PanpotChanged;
        public event EventHandler IsMutedChanged;
        public event EventHandler IsSoloChanged;


        public VolumeTracker() {
            InitializeComponent();
            this.SetStyle( ControlStyles.DoubleBuffer, true );
        }

        public String Title {
            get {
                return m_title;
            }
            set {
                m_title = value;
                UpdateTitle();
            }
        }

        private void UpdateTitle() {
            if ( m_number.Equals( "" ) ) {
                lblTitle.Text = m_title;
            } else if ( m_title.Equals( "" ) ) {
                lblTitle.Text = m_number;
            } else {
                lblTitle.Text = m_number + " " + m_title;
            }
        }

        public String Number {
            get {
                return m_number;
            }
            set {
                m_number = value;
                UpdateTitle();
            }
        }

        public boolean IsMuted {
            get {
                return m_muted;
            }
            set {
                boolean old = m_muted;
                m_muted = value;
                if ( old != m_muted && IsMutedChanged != null ) {
                    IsMutedChanged( this, new EventArgs() );
                }
            }
        }

        public boolean IsSolo {
            get {
                return m_solo;
            }
            set {
                boolean old = m_solo;
                m_solo = value;
                if ( old != m_solo && IsSoloChanged != null ) {
                    IsSoloChanged( this, new EventArgs() );
                }
            }
        }

        public int Panpot {
            get {
                return trackPanpot.Value;
            }
            set {
                trackPanpot.Value = value;
            }
        }

        public boolean SoloButtonVisible {
            get {
                return m_solo_button_visible;
            }
            set {
                m_solo_button_visible = value;
            }
        }

        public int Feder {
            get {
                return m_feder;
            }
            set {
                int old = m_feder;
                m_feder = value;
                if ( old != m_feder && FederChanged != null ) {
                    FederChanged( this, new EventArgs() );
                }
                int v = 177 - YCoordFromFeder( m_feder );
                trackFeder.Value = v;
            }
        }

        /// <summary>
        /// pがrcの中にあるかどうかを判定します
        /// </summary>
        /// <param name="p"></param>
        /// <param name="rc"></param>
        /// <returns></returns>
        private static boolean IsInRect( Point p, Rectangle rc ) {
            if ( rc.X <= p.X ) {
                if ( p.X <= rc.X + rc.Width ) {
                    if ( rc.Y <= p.Y ) {
                        if ( p.Y <= rc.Y + rc.Height ) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        
        private void VolumeTracker_Resize( object sender, EventArgs e ) {
            this.Width = WIDTH;
            this.Height = _HEIGHT;
        }

        private static int FederFromYCoord( int y ) {
            int feder = _KEY[0, 0];
            int min_diff = Math.Abs( _KEY[0, 1] - y );
            int index = 0;
            for ( int i = 1; i < _KEY.GetUpperBound( 0 ) + 1; i++ ) {
                int diff = Math.Abs( _KEY[i, 1] - y );
                if ( diff < min_diff ) {
                    index = i;
                    min_diff = diff;
                    feder = _KEY[i, 0];
                }
            }
            return feder;
        }

        private static int YCoordFromFeder( int feder ) {
            int y = _KEY[0, 1];
            int min_diff = Math.Abs( _KEY[0, 0] - feder );
            int index = 0;
            for ( int i = 1; i <= _KEY.GetUpperBound( 0 ); i++ ) {
                int diff = Math.Abs( _KEY[i, 0] - feder );
                if ( diff < min_diff ) {
                    index = i;
                    min_diff = diff;
                    y = _KEY[i, 1];
                }
            }
            return y;
        }

        private void trackFeder_ValueChanged( object sender, EventArgs e ) {
            m_feder = FederFromYCoord( 151 - (trackFeder.Value - 26) );
            txtFeder.Text = (m_feder / 10.0).ToString();
            if ( FederChanged != null ) {
                FederChanged( this, new EventArgs() );
            }
        }

        private void trackPanpot_ValueChanged( object sender, EventArgs e ) {
            lblPanpot.Text = trackPanpot.Value.ToString();
            if ( PanpotChanged != null ) {
                PanpotChanged( this, new EventArgs() );
            }
        }
    }

}
