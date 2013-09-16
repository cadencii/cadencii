/*
 * MusicXmlExtentions.cs
 * Copyright © 2013 kbinani
 *
 * This file is part of cadencii.vsq.
 *
 * cadencii.vsq is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.vsq is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System.Linq;
using System.Collections.Generic;

namespace MusicXML
{
partial class time
{
    private PropertyAggregate<time, MusicXML.interchangeable, MusicXML.ItemsChoiceType9> interchangeable_;
    private PropertyAggregate<time, System.String, MusicXML.ItemsChoiceType9> beattype_;
    private PropertyAggregate<time, System.String, MusicXML.ItemsChoiceType9> beats_;
    private PropertyAggregate<time, System.String, MusicXML.ItemsChoiceType9> senzamisura_;

    public time()
    {
        interchangeable_ = new PropertyAggregate<time, MusicXML.interchangeable, MusicXML.ItemsChoiceType9>(this, MusicXML.ItemsChoiceType9.interchangeable);
        beattype_ = new PropertyAggregate<time, System.String, MusicXML.ItemsChoiceType9>(this, MusicXML.ItemsChoiceType9.beattype);
        beats_ = new PropertyAggregate<time, System.String, MusicXML.ItemsChoiceType9>(this, MusicXML.ItemsChoiceType9.beats);
        senzamisura_ = new PropertyAggregate<time, System.String, MusicXML.ItemsChoiceType9>(this, MusicXML.ItemsChoiceType9.senzamisura);
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<time, MusicXML.interchangeable, MusicXML.ItemsChoiceType9> interchangeable
    {
        get { return interchangeable_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<time, System.String, MusicXML.ItemsChoiceType9> beattype
    {
        get { return beattype_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<time, System.String, MusicXML.ItemsChoiceType9> beats
    {
        get { return beats_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<time, System.String, MusicXML.ItemsChoiceType9> senzamisura
    {
        get { return senzamisura_; }
    }

}

partial class key
{
    private PropertyAggregate<key, MusicXML.step, MusicXML.ItemsChoiceType8> keystep_;
    private PropertyAggregate<key, MusicXML.cancel, MusicXML.ItemsChoiceType8> cancel_;
    private PropertyAggregate<key, System.Decimal, MusicXML.ItemsChoiceType8> keyalter_;
    private PropertyAggregate<key, System.String, MusicXML.ItemsChoiceType8> mode_;
    private PropertyAggregate<key, System.String, MusicXML.ItemsChoiceType8> fifths_;
    private PropertyAggregate<key, MusicXML.accidentalvalue, MusicXML.ItemsChoiceType8> keyaccidental_;

    public key()
    {
        keystep_ = new PropertyAggregate<key, MusicXML.step, MusicXML.ItemsChoiceType8>(this, MusicXML.ItemsChoiceType8.keystep);
        cancel_ = new PropertyAggregate<key, MusicXML.cancel, MusicXML.ItemsChoiceType8>(this, MusicXML.ItemsChoiceType8.cancel);
        keyalter_ = new PropertyAggregate<key, System.Decimal, MusicXML.ItemsChoiceType8>(this, MusicXML.ItemsChoiceType8.keyalter);
        mode_ = new PropertyAggregate<key, System.String, MusicXML.ItemsChoiceType8>(this, MusicXML.ItemsChoiceType8.mode);
        fifths_ = new PropertyAggregate<key, System.String, MusicXML.ItemsChoiceType8>(this, MusicXML.ItemsChoiceType8.fifths);
        keyaccidental_ = new PropertyAggregate<key, MusicXML.accidentalvalue, MusicXML.ItemsChoiceType8>(this, MusicXML.ItemsChoiceType8.keyaccidental);
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<key, MusicXML.step, MusicXML.ItemsChoiceType8> keystep
    {
        get { return keystep_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<key, MusicXML.cancel, MusicXML.ItemsChoiceType8> cancel
    {
        get { return cancel_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<key, System.Decimal, MusicXML.ItemsChoiceType8> keyalter
    {
        get { return keyalter_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<key, System.String, MusicXML.ItemsChoiceType8> mode
    {
        get { return mode_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<key, System.String, MusicXML.ItemsChoiceType8> fifths
    {
        get { return fifths_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<key, MusicXML.accidentalvalue, MusicXML.ItemsChoiceType8> keyaccidental
    {
        get { return keyaccidental_; }
    }

}

partial class directiontype
{
    private PropertyAggregate<directiontype, MusicXML.emptyprintstylealign, MusicXML.ItemsChoiceType7> coda_;
    private PropertyAggregate<directiontype, MusicXML.emptyprintstylealign, MusicXML.ItemsChoiceType7> dampall_;
    private PropertyAggregate<directiontype, MusicXML.dashes, MusicXML.ItemsChoiceType7> dashes_;
    private PropertyAggregate<directiontype, MusicXML.dynamics, MusicXML.ItemsChoiceType7> dynamics_;
    private PropertyAggregate<directiontype, MusicXML.emptyprintstylealign, MusicXML.ItemsChoiceType7> eyeglasses_;
    private PropertyAggregate<directiontype, MusicXML.harppedals, MusicXML.ItemsChoiceType7> harppedals_;
    private PropertyAggregate<directiontype, MusicXML.image, MusicXML.ItemsChoiceType7> image_;
    private PropertyAggregate<directiontype, MusicXML.metronome, MusicXML.ItemsChoiceType7> metronome_;
    private PropertyAggregate<directiontype, MusicXML.octaveshift, MusicXML.ItemsChoiceType7> octaveshift_;
    private PropertyAggregate<directiontype, MusicXML.otherdirection, MusicXML.ItemsChoiceType7> otherdirection_;
    private PropertyAggregate<directiontype, MusicXML.pedal, MusicXML.ItemsChoiceType7> pedal_;
    private PropertyAggregate<directiontype, MusicXML.percussion, MusicXML.ItemsChoiceType7> percussion_;
    private PropertyAggregate<directiontype, MusicXML.principalvoice, MusicXML.ItemsChoiceType7> principalvoice_;
    private PropertyAggregate<directiontype, MusicXML.formattedtext, MusicXML.ItemsChoiceType7> rehearsal_;
    private PropertyAggregate<directiontype, MusicXML.scordatura, MusicXML.ItemsChoiceType7> scordatura_;
    private PropertyAggregate<directiontype, MusicXML.bracket, MusicXML.ItemsChoiceType7> bracket_;
    private PropertyAggregate<directiontype, MusicXML.@stringmute, MusicXML.ItemsChoiceType7> stringmute_;
    private PropertyAggregate<directiontype, MusicXML.wedge, MusicXML.ItemsChoiceType7> wedge_;
    private PropertyAggregate<directiontype, MusicXML.formattedtext, MusicXML.ItemsChoiceType7> words_;
    private PropertyAggregate<directiontype, MusicXML.emptyprintstylealign, MusicXML.ItemsChoiceType7> segno_;
    private PropertyAggregate<directiontype, MusicXML.emptyprintstylealign, MusicXML.ItemsChoiceType7> damp_;
    private PropertyAggregate<directiontype, MusicXML.accordionregistration, MusicXML.ItemsChoiceType7> accordionregistration_;

    public directiontype()
    {
        coda_ = new PropertyAggregate<directiontype, MusicXML.emptyprintstylealign, MusicXML.ItemsChoiceType7>(this, MusicXML.ItemsChoiceType7.coda);
        dampall_ = new PropertyAggregate<directiontype, MusicXML.emptyprintstylealign, MusicXML.ItemsChoiceType7>(this, MusicXML.ItemsChoiceType7.dampall);
        dashes_ = new PropertyAggregate<directiontype, MusicXML.dashes, MusicXML.ItemsChoiceType7>(this, MusicXML.ItemsChoiceType7.dashes);
        dynamics_ = new PropertyAggregate<directiontype, MusicXML.dynamics, MusicXML.ItemsChoiceType7>(this, MusicXML.ItemsChoiceType7.dynamics);
        eyeglasses_ = new PropertyAggregate<directiontype, MusicXML.emptyprintstylealign, MusicXML.ItemsChoiceType7>(this, MusicXML.ItemsChoiceType7.eyeglasses);
        harppedals_ = new PropertyAggregate<directiontype, MusicXML.harppedals, MusicXML.ItemsChoiceType7>(this, MusicXML.ItemsChoiceType7.harppedals);
        image_ = new PropertyAggregate<directiontype, MusicXML.image, MusicXML.ItemsChoiceType7>(this, MusicXML.ItemsChoiceType7.image);
        metronome_ = new PropertyAggregate<directiontype, MusicXML.metronome, MusicXML.ItemsChoiceType7>(this, MusicXML.ItemsChoiceType7.metronome);
        octaveshift_ = new PropertyAggregate<directiontype, MusicXML.octaveshift, MusicXML.ItemsChoiceType7>(this, MusicXML.ItemsChoiceType7.octaveshift);
        otherdirection_ = new PropertyAggregate<directiontype, MusicXML.otherdirection, MusicXML.ItemsChoiceType7>(this, MusicXML.ItemsChoiceType7.otherdirection);
        pedal_ = new PropertyAggregate<directiontype, MusicXML.pedal, MusicXML.ItemsChoiceType7>(this, MusicXML.ItemsChoiceType7.pedal);
        percussion_ = new PropertyAggregate<directiontype, MusicXML.percussion, MusicXML.ItemsChoiceType7>(this, MusicXML.ItemsChoiceType7.percussion);
        principalvoice_ = new PropertyAggregate<directiontype, MusicXML.principalvoice, MusicXML.ItemsChoiceType7>(this, MusicXML.ItemsChoiceType7.principalvoice);
        rehearsal_ = new PropertyAggregate<directiontype, MusicXML.formattedtext, MusicXML.ItemsChoiceType7>(this, MusicXML.ItemsChoiceType7.rehearsal);
        scordatura_ = new PropertyAggregate<directiontype, MusicXML.scordatura, MusicXML.ItemsChoiceType7>(this, MusicXML.ItemsChoiceType7.scordatura);
        bracket_ = new PropertyAggregate<directiontype, MusicXML.bracket, MusicXML.ItemsChoiceType7>(this, MusicXML.ItemsChoiceType7.bracket);
        stringmute_ = new PropertyAggregate<directiontype, MusicXML.@stringmute, MusicXML.ItemsChoiceType7>(this, MusicXML.ItemsChoiceType7.stringmute);
        wedge_ = new PropertyAggregate<directiontype, MusicXML.wedge, MusicXML.ItemsChoiceType7>(this, MusicXML.ItemsChoiceType7.wedge);
        words_ = new PropertyAggregate<directiontype, MusicXML.formattedtext, MusicXML.ItemsChoiceType7>(this, MusicXML.ItemsChoiceType7.words);
        segno_ = new PropertyAggregate<directiontype, MusicXML.emptyprintstylealign, MusicXML.ItemsChoiceType7>(this, MusicXML.ItemsChoiceType7.segno);
        damp_ = new PropertyAggregate<directiontype, MusicXML.emptyprintstylealign, MusicXML.ItemsChoiceType7>(this, MusicXML.ItemsChoiceType7.damp);
        accordionregistration_ = new PropertyAggregate<directiontype, MusicXML.accordionregistration, MusicXML.ItemsChoiceType7>(this, MusicXML.ItemsChoiceType7.accordionregistration);
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<directiontype, MusicXML.emptyprintstylealign, MusicXML.ItemsChoiceType7> coda
    {
        get { return coda_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<directiontype, MusicXML.emptyprintstylealign, MusicXML.ItemsChoiceType7> dampall
    {
        get { return dampall_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<directiontype, MusicXML.dashes, MusicXML.ItemsChoiceType7> dashes
    {
        get { return dashes_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<directiontype, MusicXML.dynamics, MusicXML.ItemsChoiceType7> dynamics
    {
        get { return dynamics_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<directiontype, MusicXML.emptyprintstylealign, MusicXML.ItemsChoiceType7> eyeglasses
    {
        get { return eyeglasses_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<directiontype, MusicXML.harppedals, MusicXML.ItemsChoiceType7> harppedals
    {
        get { return harppedals_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<directiontype, MusicXML.image, MusicXML.ItemsChoiceType7> image
    {
        get { return image_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<directiontype, MusicXML.metronome, MusicXML.ItemsChoiceType7> metronome
    {
        get { return metronome_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<directiontype, MusicXML.octaveshift, MusicXML.ItemsChoiceType7> octaveshift
    {
        get { return octaveshift_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<directiontype, MusicXML.otherdirection, MusicXML.ItemsChoiceType7> otherdirection
    {
        get { return otherdirection_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<directiontype, MusicXML.pedal, MusicXML.ItemsChoiceType7> pedal
    {
        get { return pedal_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<directiontype, MusicXML.percussion, MusicXML.ItemsChoiceType7> percussion
    {
        get { return percussion_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<directiontype, MusicXML.principalvoice, MusicXML.ItemsChoiceType7> principalvoice
    {
        get { return principalvoice_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<directiontype, MusicXML.formattedtext, MusicXML.ItemsChoiceType7> rehearsal
    {
        get { return rehearsal_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<directiontype, MusicXML.scordatura, MusicXML.ItemsChoiceType7> scordatura
    {
        get { return scordatura_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<directiontype, MusicXML.bracket, MusicXML.ItemsChoiceType7> bracket
    {
        get { return bracket_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<directiontype, MusicXML.@stringmute, MusicXML.ItemsChoiceType7> stringmute
    {
        get { return stringmute_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<directiontype, MusicXML.wedge, MusicXML.ItemsChoiceType7> wedge
    {
        get { return wedge_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<directiontype, MusicXML.formattedtext, MusicXML.ItemsChoiceType7> words
    {
        get { return words_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<directiontype, MusicXML.emptyprintstylealign, MusicXML.ItemsChoiceType7> segno
    {
        get { return segno_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<directiontype, MusicXML.emptyprintstylealign, MusicXML.ItemsChoiceType7> damp
    {
        get { return damp_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<directiontype, MusicXML.accordionregistration, MusicXML.ItemsChoiceType7> accordionregistration
    {
        get { return accordionregistration_; }
    }

}

partial class dynamics
{
    private PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> ppppp_;
    private PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> pppp_;
    private PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> f_;
    private PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> pppppp_;
    private PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> rf_;
    private PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> rfz_;
    private PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> sf_;
    private PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> sffz_;
    private PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> sfp_;
    private PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> sfpp_;
    private PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> sfz_;
    private PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> ff_;
    private PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> fff_;
    private PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> ffff_;
    private PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> fffff_;
    private PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> ffffff_;
    private PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> fp_;
    private PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> fz_;
    private PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> mf_;
    private PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> mp_;
    private PropertyAggregate<dynamics, System.String, MusicXML.ItemsChoiceType5> otherdynamics_;
    private PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> p_;
    private PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> pp_;
    private PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> ppp_;

    public dynamics()
    {
        ppppp_ = new PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.ppppp);
        pppp_ = new PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.pppp);
        f_ = new PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.f);
        pppppp_ = new PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.pppppp);
        rf_ = new PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.rf);
        rfz_ = new PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.rfz);
        sf_ = new PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.sf);
        sffz_ = new PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.sffz);
        sfp_ = new PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.sfp);
        sfpp_ = new PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.sfpp);
        sfz_ = new PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.sfz);
        ff_ = new PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.ff);
        fff_ = new PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.fff);
        ffff_ = new PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.ffff);
        fffff_ = new PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.fffff);
        ffffff_ = new PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.ffffff);
        fp_ = new PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.fp);
        fz_ = new PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.fz);
        mf_ = new PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.mf);
        mp_ = new PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.mp);
        otherdynamics_ = new PropertyAggregate<dynamics, System.String, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.otherdynamics);
        p_ = new PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.p);
        pp_ = new PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.pp);
        ppp_ = new PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5>(this, MusicXML.ItemsChoiceType5.ppp);
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> ppppp
    {
        get { return ppppp_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> pppp
    {
        get { return pppp_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> f
    {
        get { return f_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> pppppp
    {
        get { return pppppp_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> rf
    {
        get { return rf_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> rfz
    {
        get { return rfz_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> sf
    {
        get { return sf_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> sffz
    {
        get { return sffz_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> sfp
    {
        get { return sfp_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> sfpp
    {
        get { return sfpp_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> sfz
    {
        get { return sfz_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> ff
    {
        get { return ff_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> fff
    {
        get { return fff_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> ffff
    {
        get { return ffff_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> fffff
    {
        get { return fffff_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> ffffff
    {
        get { return ffffff_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> fp
    {
        get { return fp_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> fz
    {
        get { return fz_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> mf
    {
        get { return mf_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> mp
    {
        get { return mp_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, System.String, MusicXML.ItemsChoiceType5> otherdynamics
    {
        get { return otherdynamics_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> p
    {
        get { return p_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> pp
    {
        get { return pp_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<dynamics, MusicXML.empty, MusicXML.ItemsChoiceType5> ppp
    {
        get { return ppp_; }
    }

}

partial class lyric
{
    private PropertyAggregate<lyric, MusicXML.empty, MusicXML.ItemsChoiceType6> humming_;
    private PropertyAggregate<lyric, MusicXML.extend, MusicXML.ItemsChoiceType6> extend_;
    private PropertyAggregate<lyric, MusicXML.textfontcolor, MusicXML.ItemsChoiceType6> elision_;
    private PropertyAggregate<lyric, MusicXML.empty, MusicXML.ItemsChoiceType6> laughing_;
    private PropertyAggregate<lyric, MusicXML.syllabic, MusicXML.ItemsChoiceType6> syllabic_;
    private PropertyAggregate<lyric, MusicXML.textelementdata, MusicXML.ItemsChoiceType6> text_;

    public lyric()
    {
        humming_ = new PropertyAggregate<lyric, MusicXML.empty, MusicXML.ItemsChoiceType6>(this, MusicXML.ItemsChoiceType6.humming);
        extend_ = new PropertyAggregate<lyric, MusicXML.extend, MusicXML.ItemsChoiceType6>(this, MusicXML.ItemsChoiceType6.extend);
        elision_ = new PropertyAggregate<lyric, MusicXML.textfontcolor, MusicXML.ItemsChoiceType6>(this, MusicXML.ItemsChoiceType6.elision);
        laughing_ = new PropertyAggregate<lyric, MusicXML.empty, MusicXML.ItemsChoiceType6>(this, MusicXML.ItemsChoiceType6.laughing);
        syllabic_ = new PropertyAggregate<lyric, MusicXML.syllabic, MusicXML.ItemsChoiceType6>(this, MusicXML.ItemsChoiceType6.syllabic);
        text_ = new PropertyAggregate<lyric, MusicXML.textelementdata, MusicXML.ItemsChoiceType6>(this, MusicXML.ItemsChoiceType6.text);
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<lyric, MusicXML.empty, MusicXML.ItemsChoiceType6> humming
    {
        get { return humming_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<lyric, MusicXML.extend, MusicXML.ItemsChoiceType6> extend
    {
        get { return extend_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<lyric, MusicXML.textfontcolor, MusicXML.ItemsChoiceType6> elision
    {
        get { return elision_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<lyric, MusicXML.empty, MusicXML.ItemsChoiceType6> laughing
    {
        get { return laughing_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<lyric, MusicXML.syllabic, MusicXML.ItemsChoiceType6> syllabic
    {
        get { return syllabic_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<lyric, MusicXML.textelementdata, MusicXML.ItemsChoiceType6> text
    {
        get { return text_; }
    }

}

partial class articulations
{
    private PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4> unstress_;
    private PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4> tenuto_;
    private PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4> accent_;
    private PropertyAggregate<articulations, MusicXML.breathmark, MusicXML.ItemsChoiceType4> breathmark_;
    private PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4> caesura_;
    private PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4> detachedlegato_;
    private PropertyAggregate<articulations, MusicXML.emptyline, MusicXML.ItemsChoiceType4> doit_;
    private PropertyAggregate<articulations, MusicXML.emptyline, MusicXML.ItemsChoiceType4> falloff_;
    private PropertyAggregate<articulations, MusicXML.placementtext, MusicXML.ItemsChoiceType4> otherarticulation_;
    private PropertyAggregate<articulations, MusicXML.emptyline, MusicXML.ItemsChoiceType4> plop_;
    private PropertyAggregate<articulations, MusicXML.emptyline, MusicXML.ItemsChoiceType4> scoop_;
    private PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4> spiccato_;
    private PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4> staccatissimo_;
    private PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4> staccato_;
    private PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4> stress_;
    private PropertyAggregate<articulations, MusicXML.strongaccent, MusicXML.ItemsChoiceType4> strongaccent_;

    public articulations()
    {
        unstress_ = new PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4>(this, MusicXML.ItemsChoiceType4.unstress);
        tenuto_ = new PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4>(this, MusicXML.ItemsChoiceType4.tenuto);
        accent_ = new PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4>(this, MusicXML.ItemsChoiceType4.accent);
        breathmark_ = new PropertyAggregate<articulations, MusicXML.breathmark, MusicXML.ItemsChoiceType4>(this, MusicXML.ItemsChoiceType4.breathmark);
        caesura_ = new PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4>(this, MusicXML.ItemsChoiceType4.caesura);
        detachedlegato_ = new PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4>(this, MusicXML.ItemsChoiceType4.detachedlegato);
        doit_ = new PropertyAggregate<articulations, MusicXML.emptyline, MusicXML.ItemsChoiceType4>(this, MusicXML.ItemsChoiceType4.doit);
        falloff_ = new PropertyAggregate<articulations, MusicXML.emptyline, MusicXML.ItemsChoiceType4>(this, MusicXML.ItemsChoiceType4.falloff);
        otherarticulation_ = new PropertyAggregate<articulations, MusicXML.placementtext, MusicXML.ItemsChoiceType4>(this, MusicXML.ItemsChoiceType4.otherarticulation);
        plop_ = new PropertyAggregate<articulations, MusicXML.emptyline, MusicXML.ItemsChoiceType4>(this, MusicXML.ItemsChoiceType4.plop);
        scoop_ = new PropertyAggregate<articulations, MusicXML.emptyline, MusicXML.ItemsChoiceType4>(this, MusicXML.ItemsChoiceType4.scoop);
        spiccato_ = new PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4>(this, MusicXML.ItemsChoiceType4.spiccato);
        staccatissimo_ = new PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4>(this, MusicXML.ItemsChoiceType4.staccatissimo);
        staccato_ = new PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4>(this, MusicXML.ItemsChoiceType4.staccato);
        stress_ = new PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4>(this, MusicXML.ItemsChoiceType4.stress);
        strongaccent_ = new PropertyAggregate<articulations, MusicXML.strongaccent, MusicXML.ItemsChoiceType4>(this, MusicXML.ItemsChoiceType4.strongaccent);
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4> unstress
    {
        get { return unstress_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4> tenuto
    {
        get { return tenuto_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4> accent
    {
        get { return accent_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<articulations, MusicXML.breathmark, MusicXML.ItemsChoiceType4> breathmark
    {
        get { return breathmark_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4> caesura
    {
        get { return caesura_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4> detachedlegato
    {
        get { return detachedlegato_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<articulations, MusicXML.emptyline, MusicXML.ItemsChoiceType4> doit
    {
        get { return doit_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<articulations, MusicXML.emptyline, MusicXML.ItemsChoiceType4> falloff
    {
        get { return falloff_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<articulations, MusicXML.placementtext, MusicXML.ItemsChoiceType4> otherarticulation
    {
        get { return otherarticulation_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<articulations, MusicXML.emptyline, MusicXML.ItemsChoiceType4> plop
    {
        get { return plop_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<articulations, MusicXML.emptyline, MusicXML.ItemsChoiceType4> scoop
    {
        get { return scoop_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4> spiccato
    {
        get { return spiccato_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4> staccatissimo
    {
        get { return staccatissimo_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4> staccato
    {
        get { return staccato_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<articulations, MusicXML.emptyplacement, MusicXML.ItemsChoiceType4> stress
    {
        get { return stress_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<articulations, MusicXML.strongaccent, MusicXML.ItemsChoiceType4> strongaccent
    {
        get { return strongaccent_; }
    }

}

partial class technical
{
    private PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3> downbow_;
    private PropertyAggregate<technical, MusicXML.hammeronpulloff, MusicXML.ItemsChoiceType3> hammeron_;
    private PropertyAggregate<technical, MusicXML.handbell, MusicXML.ItemsChoiceType3> handbell_;
    private PropertyAggregate<technical, MusicXML.placementtext, MusicXML.ItemsChoiceType3> pluck_;
    private PropertyAggregate<technical, MusicXML.placementtext, MusicXML.ItemsChoiceType3> othertechnical_;
    private PropertyAggregate<technical, MusicXML.bend, MusicXML.ItemsChoiceType3> bend_;
    private PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3> doubletongue_;
    private PropertyAggregate<technical, MusicXML.fingering, MusicXML.ItemsChoiceType3> fingering_;
    private PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3> fingernails_;
    private PropertyAggregate<technical, MusicXML.fret, MusicXML.ItemsChoiceType3> fret_;
    private PropertyAggregate<technical, MusicXML.harmonic, MusicXML.ItemsChoiceType3> harmonic_;
    private PropertyAggregate<technical, MusicXML.heeltoe, MusicXML.ItemsChoiceType3> heel_;
    private PropertyAggregate<technical, MusicXML.hole, MusicXML.ItemsChoiceType3> hole_;
    private PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3> openstring_;
    private PropertyAggregate<technical, MusicXML.arrow, MusicXML.ItemsChoiceType3> arrow_;
    private PropertyAggregate<technical, MusicXML.hammeronpulloff, MusicXML.ItemsChoiceType3> pulloff_;
    private PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3> snappizzicato_;
    private PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3> stopped_;
    private PropertyAggregate<technical, MusicXML.@string, MusicXML.ItemsChoiceType3> @string_;
    private PropertyAggregate<technical, MusicXML.placementtext, MusicXML.ItemsChoiceType3> tap_;
    private PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3> thumbposition_;
    private PropertyAggregate<technical, MusicXML.heeltoe, MusicXML.ItemsChoiceType3> toe_;
    private PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3> tripletongue_;
    private PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3> upbow_;

    public technical()
    {
        downbow_ = new PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.downbow);
        hammeron_ = new PropertyAggregate<technical, MusicXML.hammeronpulloff, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.hammeron);
        handbell_ = new PropertyAggregate<technical, MusicXML.handbell, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.handbell);
        pluck_ = new PropertyAggregate<technical, MusicXML.placementtext, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.pluck);
        othertechnical_ = new PropertyAggregate<technical, MusicXML.placementtext, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.othertechnical);
        bend_ = new PropertyAggregate<technical, MusicXML.bend, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.bend);
        doubletongue_ = new PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.doubletongue);
        fingering_ = new PropertyAggregate<technical, MusicXML.fingering, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.fingering);
        fingernails_ = new PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.fingernails);
        fret_ = new PropertyAggregate<technical, MusicXML.fret, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.fret);
        harmonic_ = new PropertyAggregate<technical, MusicXML.harmonic, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.harmonic);
        heel_ = new PropertyAggregate<technical, MusicXML.heeltoe, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.heel);
        hole_ = new PropertyAggregate<technical, MusicXML.hole, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.hole);
        openstring_ = new PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.openstring);
        arrow_ = new PropertyAggregate<technical, MusicXML.arrow, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.arrow);
        pulloff_ = new PropertyAggregate<technical, MusicXML.hammeronpulloff, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.pulloff);
        snappizzicato_ = new PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.snappizzicato);
        stopped_ = new PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.stopped);
        @string_ = new PropertyAggregate<technical, MusicXML.@string, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.@string);
        tap_ = new PropertyAggregate<technical, MusicXML.placementtext, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.tap);
        thumbposition_ = new PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.thumbposition);
        toe_ = new PropertyAggregate<technical, MusicXML.heeltoe, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.toe);
        tripletongue_ = new PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.tripletongue);
        upbow_ = new PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3>(this, MusicXML.ItemsChoiceType3.upbow);
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3> downbow
    {
        get { return downbow_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.hammeronpulloff, MusicXML.ItemsChoiceType3> hammeron
    {
        get { return hammeron_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.handbell, MusicXML.ItemsChoiceType3> handbell
    {
        get { return handbell_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.placementtext, MusicXML.ItemsChoiceType3> pluck
    {
        get { return pluck_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.placementtext, MusicXML.ItemsChoiceType3> othertechnical
    {
        get { return othertechnical_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.bend, MusicXML.ItemsChoiceType3> bend
    {
        get { return bend_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3> doubletongue
    {
        get { return doubletongue_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.fingering, MusicXML.ItemsChoiceType3> fingering
    {
        get { return fingering_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3> fingernails
    {
        get { return fingernails_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.fret, MusicXML.ItemsChoiceType3> fret
    {
        get { return fret_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.harmonic, MusicXML.ItemsChoiceType3> harmonic
    {
        get { return harmonic_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.heeltoe, MusicXML.ItemsChoiceType3> heel
    {
        get { return heel_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.hole, MusicXML.ItemsChoiceType3> hole
    {
        get { return hole_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3> openstring
    {
        get { return openstring_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.arrow, MusicXML.ItemsChoiceType3> arrow
    {
        get { return arrow_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.hammeronpulloff, MusicXML.ItemsChoiceType3> pulloff
    {
        get { return pulloff_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3> snappizzicato
    {
        get { return snappizzicato_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3> stopped
    {
        get { return stopped_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.@string, MusicXML.ItemsChoiceType3> @string
    {
        get { return @string_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.placementtext, MusicXML.ItemsChoiceType3> tap
    {
        get { return tap_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3> thumbposition
    {
        get { return thumbposition_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.heeltoe, MusicXML.ItemsChoiceType3> toe
    {
        get { return toe_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3> tripletongue
    {
        get { return tripletongue_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<technical, MusicXML.emptyplacement, MusicXML.ItemsChoiceType3> upbow
    {
        get { return upbow_; }
    }

}

partial class ornaments
{
    private PropertyAggregate<ornaments, MusicXML.horizontalturn, MusicXML.ItemsChoiceType2> invertedturn_;
    private PropertyAggregate<ornaments, MusicXML.tremolo, MusicXML.ItemsChoiceType2> tremolo_;
    private PropertyAggregate<ornaments, MusicXML.emptytrillsound, MusicXML.ItemsChoiceType2> trillmark_;
    private PropertyAggregate<ornaments, MusicXML.horizontalturn, MusicXML.ItemsChoiceType2> turn_;
    private PropertyAggregate<ornaments, MusicXML.emptytrillsound, MusicXML.ItemsChoiceType2> verticalturn_;
    private PropertyAggregate<ornaments, MusicXML.wavyline, MusicXML.ItemsChoiceType2> wavyline_;
    private PropertyAggregate<ornaments, MusicXML.mordent, MusicXML.ItemsChoiceType2> mordent_;
    private PropertyAggregate<ornaments, MusicXML.placementtext, MusicXML.ItemsChoiceType2> otherornament_;
    private PropertyAggregate<ornaments, MusicXML.emptyplacement, MusicXML.ItemsChoiceType2> schleifer_;
    private PropertyAggregate<ornaments, MusicXML.emptytrillsound, MusicXML.ItemsChoiceType2> shake_;
    private PropertyAggregate<ornaments, MusicXML.horizontalturn, MusicXML.ItemsChoiceType2> delayedinvertedturn_;
    private PropertyAggregate<ornaments, MusicXML.horizontalturn, MusicXML.ItemsChoiceType2> delayedturn_;
    private PropertyAggregate<ornaments, MusicXML.mordent, MusicXML.ItemsChoiceType2> invertedmordent_;

    public ornaments()
    {
        invertedturn_ = new PropertyAggregate<ornaments, MusicXML.horizontalturn, MusicXML.ItemsChoiceType2>(this, MusicXML.ItemsChoiceType2.invertedturn);
        tremolo_ = new PropertyAggregate<ornaments, MusicXML.tremolo, MusicXML.ItemsChoiceType2>(this, MusicXML.ItemsChoiceType2.tremolo);
        trillmark_ = new PropertyAggregate<ornaments, MusicXML.emptytrillsound, MusicXML.ItemsChoiceType2>(this, MusicXML.ItemsChoiceType2.trillmark);
        turn_ = new PropertyAggregate<ornaments, MusicXML.horizontalturn, MusicXML.ItemsChoiceType2>(this, MusicXML.ItemsChoiceType2.turn);
        verticalturn_ = new PropertyAggregate<ornaments, MusicXML.emptytrillsound, MusicXML.ItemsChoiceType2>(this, MusicXML.ItemsChoiceType2.verticalturn);
        wavyline_ = new PropertyAggregate<ornaments, MusicXML.wavyline, MusicXML.ItemsChoiceType2>(this, MusicXML.ItemsChoiceType2.wavyline);
        mordent_ = new PropertyAggregate<ornaments, MusicXML.mordent, MusicXML.ItemsChoiceType2>(this, MusicXML.ItemsChoiceType2.mordent);
        otherornament_ = new PropertyAggregate<ornaments, MusicXML.placementtext, MusicXML.ItemsChoiceType2>(this, MusicXML.ItemsChoiceType2.otherornament);
        schleifer_ = new PropertyAggregate<ornaments, MusicXML.emptyplacement, MusicXML.ItemsChoiceType2>(this, MusicXML.ItemsChoiceType2.schleifer);
        shake_ = new PropertyAggregate<ornaments, MusicXML.emptytrillsound, MusicXML.ItemsChoiceType2>(this, MusicXML.ItemsChoiceType2.shake);
        delayedinvertedturn_ = new PropertyAggregate<ornaments, MusicXML.horizontalturn, MusicXML.ItemsChoiceType2>(this, MusicXML.ItemsChoiceType2.delayedinvertedturn);
        delayedturn_ = new PropertyAggregate<ornaments, MusicXML.horizontalturn, MusicXML.ItemsChoiceType2>(this, MusicXML.ItemsChoiceType2.delayedturn);
        invertedmordent_ = new PropertyAggregate<ornaments, MusicXML.mordent, MusicXML.ItemsChoiceType2>(this, MusicXML.ItemsChoiceType2.invertedmordent);
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<ornaments, MusicXML.horizontalturn, MusicXML.ItemsChoiceType2> invertedturn
    {
        get { return invertedturn_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<ornaments, MusicXML.tremolo, MusicXML.ItemsChoiceType2> tremolo
    {
        get { return tremolo_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<ornaments, MusicXML.emptytrillsound, MusicXML.ItemsChoiceType2> trillmark
    {
        get { return trillmark_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<ornaments, MusicXML.horizontalturn, MusicXML.ItemsChoiceType2> turn
    {
        get { return turn_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<ornaments, MusicXML.emptytrillsound, MusicXML.ItemsChoiceType2> verticalturn
    {
        get { return verticalturn_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<ornaments, MusicXML.wavyline, MusicXML.ItemsChoiceType2> wavyline
    {
        get { return wavyline_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<ornaments, MusicXML.mordent, MusicXML.ItemsChoiceType2> mordent
    {
        get { return mordent_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<ornaments, MusicXML.placementtext, MusicXML.ItemsChoiceType2> otherornament
    {
        get { return otherornament_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<ornaments, MusicXML.emptyplacement, MusicXML.ItemsChoiceType2> schleifer
    {
        get { return schleifer_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<ornaments, MusicXML.emptytrillsound, MusicXML.ItemsChoiceType2> shake
    {
        get { return shake_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<ornaments, MusicXML.horizontalturn, MusicXML.ItemsChoiceType2> delayedinvertedturn
    {
        get { return delayedinvertedturn_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<ornaments, MusicXML.horizontalturn, MusicXML.ItemsChoiceType2> delayedturn
    {
        get { return delayedturn_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<ornaments, MusicXML.mordent, MusicXML.ItemsChoiceType2> invertedmordent
    {
        get { return invertedmordent_; }
    }

}

partial class note
{
    private PropertyAggregate<note, MusicXML.grace, MusicXML.ItemsChoiceType1> grace_;
    private PropertyAggregate<note, MusicXML.empty, MusicXML.ItemsChoiceType1> chord_;
    private PropertyAggregate<note, MusicXML.empty, MusicXML.ItemsChoiceType1> cue_;
    private PropertyAggregate<note, System.Decimal, MusicXML.ItemsChoiceType1> duration_;
    private PropertyAggregate<note, MusicXML.pitch, MusicXML.ItemsChoiceType1> pitch_;
    private PropertyAggregate<note, MusicXML.rest, MusicXML.ItemsChoiceType1> rest_;
    private PropertyAggregate<note, MusicXML.tie, MusicXML.ItemsChoiceType1> tie_;
    private PropertyAggregate<note, MusicXML.unpitched, MusicXML.ItemsChoiceType1> unpitched_;

    public note()
    {
        grace_ = new PropertyAggregate<note, MusicXML.grace, MusicXML.ItemsChoiceType1>(this, MusicXML.ItemsChoiceType1.grace);
        chord_ = new PropertyAggregate<note, MusicXML.empty, MusicXML.ItemsChoiceType1>(this, MusicXML.ItemsChoiceType1.chord);
        cue_ = new PropertyAggregate<note, MusicXML.empty, MusicXML.ItemsChoiceType1>(this, MusicXML.ItemsChoiceType1.cue);
        duration_ = new PropertyAggregate<note, System.Decimal, MusicXML.ItemsChoiceType1>(this, MusicXML.ItemsChoiceType1.duration);
        pitch_ = new PropertyAggregate<note, MusicXML.pitch, MusicXML.ItemsChoiceType1>(this, MusicXML.ItemsChoiceType1.pitch);
        rest_ = new PropertyAggregate<note, MusicXML.rest, MusicXML.ItemsChoiceType1>(this, MusicXML.ItemsChoiceType1.rest);
        tie_ = new PropertyAggregate<note, MusicXML.tie, MusicXML.ItemsChoiceType1>(this, MusicXML.ItemsChoiceType1.tie);
        unpitched_ = new PropertyAggregate<note, MusicXML.unpitched, MusicXML.ItemsChoiceType1>(this, MusicXML.ItemsChoiceType1.unpitched);
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<note, MusicXML.grace, MusicXML.ItemsChoiceType1> grace
    {
        get { return grace_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<note, MusicXML.empty, MusicXML.ItemsChoiceType1> chord
    {
        get { return chord_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<note, MusicXML.empty, MusicXML.ItemsChoiceType1> cue
    {
        get { return cue_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<note, System.Decimal, MusicXML.ItemsChoiceType1> duration
    {
        get { return duration_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<note, MusicXML.pitch, MusicXML.ItemsChoiceType1> pitch
    {
        get { return pitch_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<note, MusicXML.rest, MusicXML.ItemsChoiceType1> rest
    {
        get { return rest_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<note, MusicXML.tie, MusicXML.ItemsChoiceType1> tie
    {
        get { return tie_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<note, MusicXML.unpitched, MusicXML.ItemsChoiceType1> unpitched
    {
        get { return unpitched_; }
    }

}

partial class encoding
{
    private PropertyAggregate<encoding, MusicXML.typedtext, MusicXML.ItemsChoiceType> encoder_;
    private PropertyAggregate<encoding, System.String, MusicXML.ItemsChoiceType> encodingdescription_;
    private PropertyAggregate<encoding, System.DateTime, MusicXML.ItemsChoiceType> encodingdate_;
    private PropertyAggregate<encoding, MusicXML.supports, MusicXML.ItemsChoiceType> supports_;
    private PropertyAggregate<encoding, System.String, MusicXML.ItemsChoiceType> software_;

    public encoding()
    {
        encoder_ = new PropertyAggregate<encoding, MusicXML.typedtext, MusicXML.ItemsChoiceType>(this, MusicXML.ItemsChoiceType.encoder);
        encodingdescription_ = new PropertyAggregate<encoding, System.String, MusicXML.ItemsChoiceType>(this, MusicXML.ItemsChoiceType.encodingdescription);
        encodingdate_ = new PropertyAggregate<encoding, System.DateTime, MusicXML.ItemsChoiceType>(this, MusicXML.ItemsChoiceType.encodingdate);
        supports_ = new PropertyAggregate<encoding, MusicXML.supports, MusicXML.ItemsChoiceType>(this, MusicXML.ItemsChoiceType.supports);
        software_ = new PropertyAggregate<encoding, System.String, MusicXML.ItemsChoiceType>(this, MusicXML.ItemsChoiceType.software);
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<encoding, MusicXML.typedtext, MusicXML.ItemsChoiceType> encoder
    {
        get { return encoder_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<encoding, System.String, MusicXML.ItemsChoiceType> encodingdescription
    {
        get { return encodingdescription_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<encoding, System.DateTime, MusicXML.ItemsChoiceType> encodingdate
    {
        get { return encodingdate_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<encoding, MusicXML.supports, MusicXML.ItemsChoiceType> supports
    {
        get { return supports_; }
    }

    [System.Xml.Serialization.XmlIgnore]
    public PropertyAggregate<encoding, System.String, MusicXML.ItemsChoiceType> software
    {
        get { return software_; }
    }

}

[System.Serializable]
public class PropertyAggregate<ParentType, ValueType, ItemChoiceEnumType>
    where ItemChoiceEnumType : System.IComparable
{
    private ParentType parent_;
    private ItemChoiceEnumType choice_;
    private System.Reflection.PropertyInfo values_field_;
    private System.Reflection.PropertyInfo names_field_;

    public PropertyAggregate(ParentType parent, ItemChoiceEnumType choice)
    {
        choice_ = choice;
        SetParent(parent);
    }

    internal void SetParent(ParentType parent)
    {
        parent_ = parent;
        System.Type type = typeof(ParentType);
        values_field_ = type.GetProperty("Items");
        names_field_ = type.GetProperty("ItemsElementName");
    }

    public ValueType First
    {
        get { return this[0]; }
        set { this[0] = value; }
    }

    public ValueType this[int index]
    {
        get
        {
            int actual_index = GetIndexOfChoice(index);
            if (actual_index == -1) {
                throw new System.ArgumentOutOfRangeException();
            }
            var values_field = GetValuesField();
            return (ValueType)values_field[actual_index];
        }
        set
        {
            int actual_index = GetIndexOfChoice(index);
            if (actual_index == -1) {
                throw new System.ArgumentOutOfRangeException();
            }
            var values_field = GetValuesField();
            values_field[actual_index] = value;
        }
    }

    public void Add(ValueType value)
    {
        var names_field = GetNamesField();
        var values_field = GetValuesField();

        int new_count = names_field == null ? 1 : names_field.Length + 1;
        var new_names_field = new ItemChoiceEnumType[new_count];
        var new_values_field = new object[new_count];
        for (int i = 0; i < new_count - 1; i++) {
            new_names_field[i] = names_field[i];
            new_values_field[i] = values_field[i];
        }
        new_names_field[new_count - 1] = choice_;
        new_values_field[new_count - 1] = value;

        names_field_.GetSetMethod().Invoke(parent_, new object[] { new_names_field });
        values_field_.GetSetMethod().Invoke(parent_, new object[] { new_values_field });
    }

    public int Count
    {
        get
        {
            var names_field = GetNamesField();
            if (names_field == null) {
                return 0;
            } else {
                return names_field.Count((name) => name.CompareTo(choice_) == 0);
            }
        }
    }

    private ItemChoiceEnumType[] GetNamesField()
    {
        return names_field_.GetGetMethod().Invoke(parent_, null) as ItemChoiceEnumType[];
    }

    private object[] GetValuesField()
    {
        return values_field_.GetGetMethod().Invoke(parent_, null) as object[];
    }

    private int GetIndexOfChoice(int index)
    {
        var names_field = GetNamesField();
        if (names_field == null) {
            return -1;
        }

        int counter = -1;
        for (int i = 0; i < names_field.Length; i++) {
            if (names_field[i].CompareTo(choice_) == 0) {
                counter++;
            }
            if (counter == index) {
                return i;
            }
        }
        return -1;
    }
}

}
