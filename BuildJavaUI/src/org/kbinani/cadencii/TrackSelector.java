package org.kbinani.cadencii;
//SECTION-BEGIN-IMPORT

import java.awt.BorderLayout;
import java.awt.Dimension;
import javax.swing.JPanel;
import javax.swing.JScrollBar;
import javax.swing.JSeparator;
import javax.swing.JToolTip;
import org.kbinani.windows.forms.BMenu;
import org.kbinani.windows.forms.BMenuItem;
import org.kbinani.windows.forms.BPopupMenu;
import java.awt.Color;

//SECTION-END-IMPORT
public class TrackSelector extends JPanel {
    //SECTION-BEGIN-FIELD

    private static final long serialVersionUID = 1L;
    private BPopupMenu cmenuCurve = null;  //  @jve:decl-index=0:visual-constraint="140,295"
    private BMenuItem cmenuCurveVelocity = null;
    private BMenuItem cmenuCurveAccent = null;
    private BMenuItem cmenuCurveDecay = null;
    private BMenuItem cmenuCurveDynamics = null;
    private BMenuItem cmenuCurveVibratoRate = null;
    private BMenuItem cmenuCurveVibratoDepth = null;
    private BMenu cmenuCurveReso1 = null;
    private BMenuItem cmenuCurveReso1Freq = null;
    private BMenuItem cmenuCurveReso1BW = null;
    private BMenuItem cmenuCurveReso1Amp = null;
    private BMenu cmenuCurveReso4 = null;
    private BMenuItem cmenuCurveReso4Freq = null;
    private BMenuItem cmenuCurveReso4BW = null;
    private BMenuItem cmenuCurveReso4Amp = null;
    private BMenu cmenuCurveReso3 = null;
    private BMenuItem cmenuCurveReso3Freq = null;
    private BMenuItem cmenuCurveReso3BW = null;
    private BMenuItem cmenuCurveReso3Amp = null;
    private BMenu cmenuCurveReso2 = null;
    private BMenuItem cmenuCurveReso2Freq = null;
    private BMenuItem cmenuCurveReso2BW = null;
    private BMenuItem cmenuCurveReso2Amp = null;
    private BMenuItem cmenuCurveHarmonics = null;
    private BMenuItem cmenuCurveBreathiness = null;
    private BMenuItem cmenuCurveBrightness = null;
    private BMenuItem cmenuCurveClearness = null;
    private BMenuItem cmenuCurveOpening = null;
    private BMenuItem cmenuCurveGenderFactor = null;
    private BMenuItem cmenuCurvePortamentoTiming = null;
    private BMenuItem cmenuCurvePitchBend = null;
    private BMenuItem cmenuCurvePitchBendSensitivity = null;
    private BMenuItem cmenuCurveEffect2Depth = null;
    private BMenuItem cmenuCurveEnvelope = null;
    private JSeparator cmenuCurveSeparator1 = null;
    private JSeparator cmenuCurveSeparator2 = null;
    private JSeparator cmenuCurveSeparator3 = null;
    private JSeparator cmenuCurveSeparator4 = null;
    private JSeparator cmenuCurveSeparator5 = null;
    private BPopupMenu cmenuSinger = null;  //  @jve:decl-index=0:visual-constraint="305,282"
    private JToolTip toolTip = null;  //  @jve:decl-index=0:visual-constraint="468,277"

    //SECTION-END-FIELD
    /**
     * This method initializes 
     * 
     */
    public TrackSelector() {
    	super();
    	initialize();
    	this.getCmenuCurve();
    	this.getCmenuSinger();
    	this.getJLabel();
    }
    //SECTION-BEGIN-METHOD

    /**
     * This method initializes this
     * 
     */
    private void initialize() {
        this.setLayout(new BorderLayout());
        this.setSize(new Dimension(525, 200));
        this.setBackground(new Color(169, 169, 169));
    }

    /**
     * This method initializes cmenuCurve	
     * 	
     * @return javax.swing.JPopupMenu	
     */
    private BPopupMenu getCmenuCurve() {
        if (cmenuCurve == null) {
            cmenuCurve = new BPopupMenu();
            cmenuCurve.add(getCmenuCurveVelocity());
            cmenuCurve.add(getCmenuCurveAccent());
            cmenuCurve.add(getCmenuCurveDecay());
            cmenuCurve.add(getCmenuCurveSeparator1());
            cmenuCurve.add(getCmenuCurveDynamics());
            cmenuCurve.add(getCmenuCurveVibratoRate());
            cmenuCurve.add(getCmenuCurveVibratoDepth());
            cmenuCurve.add(getCmenuCurveSeparator2());
            cmenuCurve.add(getCmenuCurveReso1());
            cmenuCurve.add(getCmenuCurveReso2());
            cmenuCurve.add(getCmenuCurveReso3());
            cmenuCurve.add(getCmenuCurveReso4());
            cmenuCurve.add(getCmenuCurveSeparator3());
            cmenuCurve.add(getCmenuCurveHarmonics());
            cmenuCurve.add(getCmenuCurveBreathiness());
            cmenuCurve.add(getCmenuCurveBrightness());
            cmenuCurve.add(getCmenuCurveClearness());
            cmenuCurve.add(getCmenuCurveOpening());
            cmenuCurve.add(getCmenuCurveGenderFactor());
            cmenuCurve.add(getCmenuCurveSeparator4());
            cmenuCurve.add(getCmenuCurvePortamentoTiming());
            cmenuCurve.add(getCmenuCurvePitchBend());
            cmenuCurve.add(getCmenuCurvePitchBendSensitivity());
            cmenuCurve.add(getCmenuCurveSeparator5());
            cmenuCurve.add(getCmenuCurveEffect2Depth());
            cmenuCurve.add(getCmenuCurveEnvelope());
        }
        return cmenuCurve;
    }

    /**
     * This method initializes cmenuCurveVelocity	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveVelocity() {
        if (cmenuCurveVelocity == null) {
            cmenuCurveVelocity = new BMenuItem();
            cmenuCurveVelocity.setText("Velocity");
        }
        return cmenuCurveVelocity;
    }

    /**
     * This method initializes cmenuCurveAccent	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveAccent() {
        if (cmenuCurveAccent == null) {
            cmenuCurveAccent = new BMenuItem();
            cmenuCurveAccent.setText("Accent");
        }
        return cmenuCurveAccent;
    }

    /**
     * This method initializes cmenuCurveDecay	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveDecay() {
        if (cmenuCurveDecay == null) {
            cmenuCurveDecay = new BMenuItem();
            cmenuCurveDecay.setText("Decay");
        }
        return cmenuCurveDecay;
    }

    /**
     * This method initializes cmenuCurveDynamics	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveDynamics() {
        if (cmenuCurveDynamics == null) {
            cmenuCurveDynamics = new BMenuItem();
            cmenuCurveDynamics.setText("Dynamics");
        }
        return cmenuCurveDynamics;
    }

    /**
     * This method initializes cmenuCurveVibratoRate	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveVibratoRate() {
        if (cmenuCurveVibratoRate == null) {
            cmenuCurveVibratoRate = new BMenuItem();
            cmenuCurveVibratoRate.setText("Vibrato Rate");
        }
        return cmenuCurveVibratoRate;
    }

    /**
     * This method initializes cmenuCurveVibratoDepth	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveVibratoDepth() {
        if (cmenuCurveVibratoDepth == null) {
            cmenuCurveVibratoDepth = new BMenuItem();
            cmenuCurveVibratoDepth.setText("Vibrato Depth");
        }
        return cmenuCurveVibratoDepth;
    }

    /**
     * This method initializes cmenuCurveReso1	
     * 	
     * @return javax.swing.JMenu	
     */
    private BMenu getCmenuCurveReso1() {
        if (cmenuCurveReso1 == null) {
            cmenuCurveReso1 = new BMenu();
            cmenuCurveReso1.setText("Resonance 1");
            cmenuCurveReso1.add(getCmenuCurveReso1Freq());
            cmenuCurveReso1.add(getCmenuCurveReso1BW());
            cmenuCurveReso1.add(getCmenuCurveReso1Amp());
        }
        return cmenuCurveReso1;
    }

    /**
     * This method initializes cmenuCurveReso1Freq	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveReso1Freq() {
        if (cmenuCurveReso1Freq == null) {
            cmenuCurveReso1Freq = new BMenuItem();
            cmenuCurveReso1Freq.setText("Frequency");
        }
        return cmenuCurveReso1Freq;
    }

    /**
     * This method initializes cmenuCurveReso1BW	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveReso1BW() {
        if (cmenuCurveReso1BW == null) {
            cmenuCurveReso1BW = new BMenuItem();
            cmenuCurveReso1BW.setText("Band Width");
        }
        return cmenuCurveReso1BW;
    }

    /**
     * This method initializes cmenuCurveReso1Amp	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveReso1Amp() {
        if (cmenuCurveReso1Amp == null) {
            cmenuCurveReso1Amp = new BMenuItem();
            cmenuCurveReso1Amp.setText("Amplitude");
        }
        return cmenuCurveReso1Amp;
    }

    /**
     * This method initializes cmenuCurveReso4	
     * 	
     * @return javax.swing.JMenu	
     */
    private BMenu getCmenuCurveReso4() {
        if (cmenuCurveReso4 == null) {
            cmenuCurveReso4 = new BMenu();
            cmenuCurveReso4.setText("Resonance 4");
            cmenuCurveReso4.add(getCmenuCurveReso4Freq());
            cmenuCurveReso4.add(getCmenuCurveReso4BW());
            cmenuCurveReso4.add(getCmenuCurveReso4Amp());
        }
        return cmenuCurveReso4;
    }

    /**
     * This method initializes cmenuCurveReso4Freq	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveReso4Freq() {
        if (cmenuCurveReso4Freq == null) {
            cmenuCurveReso4Freq = new BMenuItem();
            cmenuCurveReso4Freq.setText("Frequency");
        }
        return cmenuCurveReso4Freq;
    }

    /**
     * This method initializes cmenuCurveReso4BW	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveReso4BW() {
        if (cmenuCurveReso4BW == null) {
            cmenuCurveReso4BW = new BMenuItem();
            cmenuCurveReso4BW.setText("Band Width");
        }
        return cmenuCurveReso4BW;
    }

    /**
     * This method initializes cmenuCurveReso4Amp	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveReso4Amp() {
        if (cmenuCurveReso4Amp == null) {
            cmenuCurveReso4Amp = new BMenuItem();
            cmenuCurveReso4Amp.setText("Amplitude");
        }
        return cmenuCurveReso4Amp;
    }

    /**
     * This method initializes cmenuCurveReso3	
     * 	
     * @return javax.swing.JMenu	
     */
    private BMenu getCmenuCurveReso3() {
        if (cmenuCurveReso3 == null) {
            cmenuCurveReso3 = new BMenu();
            cmenuCurveReso3.setText("Resonance 3");
            cmenuCurveReso3.add(getCmenuCurveReso3Freq());
            cmenuCurveReso3.add(getCmenuCurveReso3BW());
            cmenuCurveReso3.add(getCmenuCurveReso3Amp());
        }
        return cmenuCurveReso3;
    }

    /**
     * This method initializes cmenuCurveReso3Freq	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveReso3Freq() {
        if (cmenuCurveReso3Freq == null) {
            cmenuCurveReso3Freq = new BMenuItem();
            cmenuCurveReso3Freq.setText("Frequency");
        }
        return cmenuCurveReso3Freq;
    }

    /**
     * This method initializes cmenuCurveReso3BW	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveReso3BW() {
        if (cmenuCurveReso3BW == null) {
            cmenuCurveReso3BW = new BMenuItem();
            cmenuCurveReso3BW.setText("Band Width");
        }
        return cmenuCurveReso3BW;
    }

    /**
     * This method initializes cmenuCurveReso3Amp	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveReso3Amp() {
        if (cmenuCurveReso3Amp == null) {
            cmenuCurveReso3Amp = new BMenuItem();
            cmenuCurveReso3Amp.setText("Amplitude");
        }
        return cmenuCurveReso3Amp;
    }

    /**
     * This method initializes cmenuCurveReso2	
     * 	
     * @return javax.swing.JMenu	
     */
    private BMenu getCmenuCurveReso2() {
        if (cmenuCurveReso2 == null) {
            cmenuCurveReso2 = new BMenu();
            cmenuCurveReso2.setText("Resonance 2");
            cmenuCurveReso2.add(getCmenuCurveReso2Freq());
            cmenuCurveReso2.add(getCmenuCurveReso2BW());
            cmenuCurveReso2.add(getCmenuCurveReso2Amp());
        }
        return cmenuCurveReso2;
    }

    /**
     * This method initializes cmenuCurveReso2Freq	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveReso2Freq() {
        if (cmenuCurveReso2Freq == null) {
            cmenuCurveReso2Freq = new BMenuItem();
            cmenuCurveReso2Freq.setText("Frequency");
        }
        return cmenuCurveReso2Freq;
    }

    /**
     * This method initializes cmenuCurveReso2BW	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveReso2BW() {
        if (cmenuCurveReso2BW == null) {
            cmenuCurveReso2BW = new BMenuItem();
            cmenuCurveReso2BW.setText("Band Width");
        }
        return cmenuCurveReso2BW;
    }

    /**
     * This method initializes cmenuCurveReso2Amp	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveReso2Amp() {
        if (cmenuCurveReso2Amp == null) {
            cmenuCurveReso2Amp = new BMenuItem();
            cmenuCurveReso2Amp.setText("Amplitude");
        }
        return cmenuCurveReso2Amp;
    }

    /**
     * This method initializes cmenuCurveHarmonics	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveHarmonics() {
        if (cmenuCurveHarmonics == null) {
            cmenuCurveHarmonics = new BMenuItem();
            cmenuCurveHarmonics.setText("Harmonics");
        }
        return cmenuCurveHarmonics;
    }

    /**
     * This method initializes cmenuCurveBreathiness	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveBreathiness() {
        if (cmenuCurveBreathiness == null) {
            cmenuCurveBreathiness = new BMenuItem();
            cmenuCurveBreathiness.setText("Noise");
        }
        return cmenuCurveBreathiness;
    }

    /**
     * This method initializes cmenuCurveBrightness	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveBrightness() {
        if (cmenuCurveBrightness == null) {
            cmenuCurveBrightness = new BMenuItem();
            cmenuCurveBrightness.setText("Brightness");
        }
        return cmenuCurveBrightness;
    }

    /**
     * This method initializes cmenuCurveClearness	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveClearness() {
        if (cmenuCurveClearness == null) {
            cmenuCurveClearness = new BMenuItem();
            cmenuCurveClearness.setText("Clearness");
        }
        return cmenuCurveClearness;
    }

    /**
     * This method initializes cmenuCurveOpening	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveOpening() {
        if (cmenuCurveOpening == null) {
            cmenuCurveOpening = new BMenuItem();
            cmenuCurveOpening.setText("Opening");
        }
        return cmenuCurveOpening;
    }

    /**
     * This method initializes cmenuCurveGenderFactor	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveGenderFactor() {
        if (cmenuCurveGenderFactor == null) {
            cmenuCurveGenderFactor = new BMenuItem();
            cmenuCurveGenderFactor.setText("Gender Factor");
        }
        return cmenuCurveGenderFactor;
    }

    /**
     * This method initializes cmenuCurvePortamentoTiming	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurvePortamentoTiming() {
        if (cmenuCurvePortamentoTiming == null) {
            cmenuCurvePortamentoTiming = new BMenuItem();
            cmenuCurvePortamentoTiming.setText("Portamento Timing");
        }
        return cmenuCurvePortamentoTiming;
    }

    /**
     * This method initializes cmenuCurvePitchBend	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurvePitchBend() {
        if (cmenuCurvePitchBend == null) {
            cmenuCurvePitchBend = new BMenuItem();
            cmenuCurvePitchBend.setText("Pitch Bend");
        }
        return cmenuCurvePitchBend;
    }

    /**
     * This method initializes cmenuCurvePitchBendSensitivity	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurvePitchBendSensitivity() {
        if (cmenuCurvePitchBendSensitivity == null) {
            cmenuCurvePitchBendSensitivity = new BMenuItem();
            cmenuCurvePitchBendSensitivity.setText("Pitch Bend Sensitivity");
        }
        return cmenuCurvePitchBendSensitivity;
    }

    /**
     * This method initializes cmenuCurveEffect2Depth	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveEffect2Depth() {
        if (cmenuCurveEffect2Depth == null) {
            cmenuCurveEffect2Depth = new BMenuItem();
            cmenuCurveEffect2Depth.setText("Effect2 Depth");
        }
        return cmenuCurveEffect2Depth;
    }

    /**
     * This method initializes cmenuCurveEnvelope	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private BMenuItem getCmenuCurveEnvelope() {
        if (cmenuCurveEnvelope == null) {
            cmenuCurveEnvelope = new BMenuItem();
            cmenuCurveEnvelope.setText("Envelope");
        }
        return cmenuCurveEnvelope;
    }

    /**
     * This method initializes cmenuCurveSeparator1	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private JSeparator getCmenuCurveSeparator1() {
        if (cmenuCurveSeparator1 == null) {
            cmenuCurveSeparator1 = new JSeparator();
        }
        return cmenuCurveSeparator1;
    }

    /**
     * This method initializes cmenuCurveSeparator2	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private JSeparator getCmenuCurveSeparator2() {
        if (cmenuCurveSeparator2 == null) {
            cmenuCurveSeparator2 = new JSeparator();
        }
        return cmenuCurveSeparator2;
    }

    /**
     * This method initializes cmenuCurveSeparator3	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private JSeparator getCmenuCurveSeparator3() {
        if (cmenuCurveSeparator3 == null) {
            cmenuCurveSeparator3 = new JSeparator();
        }
        return cmenuCurveSeparator3;
    }

    /**
     * This method initializes cmenuCurveSeparator4	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private JSeparator getCmenuCurveSeparator4() {
        if (cmenuCurveSeparator4 == null) {
            cmenuCurveSeparator4 = new JSeparator();
        }
        return cmenuCurveSeparator4;
    }

    /**
     * This method initializes cmenuCurveSeparator5	
     * 	
     * @return javax.swing.JMenuItem	
     */
    private JSeparator getCmenuCurveSeparator5() {
        if (cmenuCurveSeparator5 == null) {
            cmenuCurveSeparator5 = new JSeparator();
        }
        return cmenuCurveSeparator5;
    }

    /**
     * This method initializes cmenuSinger	
     * 	
     * @return javax.swing.JPopupMenu	
     */
    private BPopupMenu getCmenuSinger() {
        if (cmenuSinger == null) {
            cmenuSinger = new BPopupMenu();
        }
        return cmenuSinger;
    }

    /**
     * This method initializes jLabel	
     * 	
     * @return javax.swing.JLabel	
     */
    private JToolTip getJLabel() {
        if (toolTip == null) {
            toolTip = new JToolTip();
            toolTip.setSize(new Dimension(73, 38));
        }
        return toolTip;
    }

    //SECTION-END-METHOD
}  //  @jve:decl-index=0:visual-constraint="10,10"
