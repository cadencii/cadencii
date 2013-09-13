/*
 * NRPN.h
 * Copyright (c) 2009 kbinani
 *
 * This file is part of utauvsti
 *
 * utauvsti is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * utauvsti is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#ifndef __NRPN_h__
#define __NRPN_h__
enum NRPN {
    CVM_NM_VERSION_AND_DEVICE = 0x5000,     //(0x5000) Version number(MSB) &amp;, Device number(LSB)
    /// <summary>
    /// (0x5001) Delay in millisec(MSB, LSB)
    /// </summary>
    CVM_NM_DELAY = 0x5001,
    /// <summary>
    /// (0x5002) Note number(MSB)
    /// </summary>
    CVM_NM_NOTE_NUMBER = 0x5002,
    /// <summary>
    /// (0x5003) Velocity(MSB)
    /// </summary>
    CVM_NM_VELOCITY = 0x5003,
    /// <summary>
    /// (0x5004) Note Duration in millisec(MSB, LSB)
    /// </summary>
    CVM_NM_NOTE_DURATION = 0x5004,
    /// <summary>
    /// (0x5005) Note Location(MSB)
    /// </summary>
    CVM_NM_NOTE_LOCATION = 0x5005,
    /// <summary>
    /// (0x500c) Index of Vibrato DB(MSB: ID_H00, LSB:ID_L00)
    /// </summary>
    CVM_NM_INDEX_OF_VIBRATO_DB = 0x500c,
    /// <summary>
    /// (0x500d) Vibrato configuration(MSB: Index of Vibrato Type, LSB: Duration &amp;, Configuration parameter of vibrato)
    /// </summary>
    CVM_NM_VIBRATO_CONFIG = 0x500d,
    /// <summary>
    /// (0x500e) Vibrato Delay(MSB)
    /// </summary>
    CVM_NM_VIBRATO_DELAY = 0x500e,
    /// <summary>
    /// (0x5012) Number of phonetic symbols in bytes(MSB)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL_BYTES = 0x5012,
    /// <summary>
    /// (0x5013) Phonetic symbol 1(MSB:Phonetic symbol 1, LSB: Consonant adjustment 1)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL1 = 0x5013,
    /// <summary>
    /// (0x5014) Phonetic symbol 2(MSB:Phonetic symbol 2, LSB: Consonant adjustment 2)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL2 = 0x5014,
    /// <summary>
    /// (0x5015) Phonetic symbol 3(MSB:Phonetic symbol 3, LSB: Consonant adjustment 3)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL3 = 0x5015,
    /// <summary>
    /// (0x5016) Phonetic symbol 4(MSB:Phonetic symbol 4, LSB: Consonant adjustment 4)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL4 = 0x5016,
    /// <summary>
    /// (0x5017) Phonetic symbol 5(MSB:Phonetic symbol 5, LSB: Consonant adjustment 5)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL5 = 0x5017,
    /// <summary>
    /// (0x5018) Phonetic symbol 6(MSB:Phonetic symbol 6, LSB: Consonant adjustment 6)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL6 = 0x5018,
    /// <summary>
    /// (0x5019) Phonetic symbol 7(MSB:Phonetic symbol 7, LSB: Consonant adjustment 7)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL7 = 0x5019,
    /// <summary>
    /// (0x501a) Phonetic symbol 8(MSB:Phonetic symbol 8, LSB: Consonant adjustment 8)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL8 = 0x501a,
    /// <summary>
    /// (0x501b) Phonetic symbol 9(MSB:Phonetic symbol 9, LSB: Consonant adjustment 9)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL9 = 0x501b,
    /// <summary>
    /// (0x501c) Phonetic symbol 10(MSB:Phonetic symbol 10, LSB: Consonant adjustment 10)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL10 = 0x501c,
    /// <summary>
    /// (0x501d) Phonetic symbol 11(MSB:Phonetic symbol 11, LSB: Consonant adjustment 11)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL11 = 0x501d,
    /// <summary>
    /// (0x501e) Phonetic symbol 12(MSB:Phonetic symbol 12, LSB: Consonant adjustment 12)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL12 = 0x501e,
    /// <summary>
    /// (0x501f) Phonetic symbol 13(MSB:Phonetic symbol 13, LSB: Consonant adjustment 13)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL13 = 0x501f,
    /// <summary>
    /// (0x5020) Phonetic symbol 14(MSB:Phonetic symbol 14, LSB: Consonant adjustment 14)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL14 = 0x5020,
    /// <summary>
    /// (0x5021) Phonetic symbol 15(MSB:Phonetic symbol 15, LSB: Consonant adjustment 15)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL15 = 0x5021,
    /// <summary>
    /// (0x5022) Phonetic symbol 16(MSB:Phonetic symbol 16, LSB: Consonant adjustment 16)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL16 = 0x5022,
    /// <summary>
    /// (0x5023) Phonetic symbol 17(MSB:Phonetic symbol 17, LSB: Consonant adjustment 17)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL17 = 0x5023,
    /// <summary>
    /// (0x5024) Phonetic symbol 18(MSB:Phonetic symbol 18, LSB: Consonant adjustment 18)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL18 = 0x5024,
    /// <summary>
    /// (0x5025) Phonetic symbol 19(MSB:Phonetic symbol 19, LSB: Consonant adjustment 19)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL19 = 0x5025,
    /// <summary>
    /// (0x5026) Phonetic symbol 20(MSB:Phonetic symbol 20, LSB: Consonant adjustment 20)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL20 = 0x5026,
    /// <summary>
    /// (0x5027) Phonetic symbol 21(MSB:Phonetic symbol 21, LSB: Consonant adjustment 21)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL21 = 0x5027,
    /// <summary>
    /// (0x5028) Phonetic symbol 22(MSB:Phonetic symbol 22, LSB: Consonant adjustment 22)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL22 = 0x5028,
    /// <summary>
    /// (0x5029) Phonetic symbol 23(MSB:Phonetic symbol 23, LSB: Consonant adjustment 23)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL23 = 0x5029,
    /// <summary>
    /// (0x502a) Phonetic symbol 24(MSB:Phonetic symbol 24, LSB: Consonant adjustment 24)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL24 = 0x502a,
    /// <summary>
    /// (0x502b) Phonetic symbol 25(MSB:Phonetic symbol 25, LSB: Consonant adjustment 25)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL25 = 0x502b,
    /// <summary>
    /// (0x502c) Phonetic symbol 26(MSB:Phonetic symbol 26, LSB: Consonant adjustment 26)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL26 = 0x502c,
    /// <summary>
    /// (0x502d) Phonetic symbol 27(MSB:Phonetic symbol 27, LSB: Consonant adjustment 27)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL27 = 0x502d,
    /// <summary>
    /// (0x502e) Phonetic symbol 28(MSB:Phonetic symbol 28, LSB: Consonant adjustment 28)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL28 = 0x502e,
    /// <summary>
    /// (0x502f) Phonetic symbol 29(MSB:Phonetic symbol 29, LSB: Consonant adjustment 29)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL29 = 0x502f,
    /// <summary>
    /// (0x5030) Phonetic symbol 30(MSB:Phonetic symbol 30, LSB: Consonant adjustment 30)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL30 = 0x5030,
    /// <summary>
    /// (0x5031) Phonetic symbol 31(MSB:Phonetic symbol 31, LSB: Consonant adjustment 31)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL31 = 0x5031,
    /// <summary>
    /// (0x5032) Phonetic symbol 32(MSB:Phonetic symbol 32, LSB: Consonant adjustment 32)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL32 = 0x5032,
    /// <summary>
    /// (0x5033) Phonetic symbol 33(MSB:Phonetic symbol 33, LSB: Consonant adjustment 33)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL33 = 0x5033,
    /// <summary>
    /// (0x5034) Phonetic symbol 34(MSB:Phonetic symbol 34, LSB: Consonant adjustment 34)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL34 = 0x5034,
    /// <summary>
    /// (0x5035) Phonetic symbol 35(MSB:Phonetic symbol 35, LSB: Consonant adjustment 35)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL35 = 0x5035,
    /// <summary>
    /// (0x5036) Phonetic symbol 36(MSB:Phonetic symbol 36, LSB: Consonant adjustment 36)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL36 = 0x5036,
    /// <summary>
    /// (0x5037) Phonetic symbol 37(MSB:Phonetic symbol 37, LSB: Consonant adjustment 37)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL37 = 0x5037,
    /// <summary>
    /// (0x5038) Phonetic symbol 38(MSB:Phonetic symbol 38, LSB: Consonant adjustment 38)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL38 = 0x5038,
    /// <summary>
    /// (0x5039) Phonetic symbol 39(MSB:Phonetic symbol 39, LSB: Consonant adjustment 39)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL39 = 0x5039,
    /// <summary>
    /// (0x503a) Phonetic symbol 40(MSB:Phonetic symbol 40, LSB: Consonant adjustment 40)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL40 = 0x503a,
    /// <summary>
    /// (0x503b) Phonetic symbol 41(MSB:Phonetic symbol 41, LSB: Consonant adjustment 41)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL41 = 0x503b,
    /// <summary>
    /// (0x503c) Phonetic symbol 42(MSB:Phonetic symbol 42, LSB: Consonant adjustment 42)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL42 = 0x503c,
    /// <summary>
    /// (0x503d) Phonetic symbol 43(MSB:Phonetic symbol 43, LSB: Consonant adjustment 43)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL43 = 0x503d,
    /// <summary>
    /// (0x503e) Phonetic symbol 44(MSB:Phonetic symbol 44, LSB: Consonant adjustment 44)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL44 = 0x503e,
    /// <summary>
    /// (0x503f) Phonetic symbol 45(MSB:Phonetic symbol 45, LSB: Consonant adjustment 45)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL45 = 0x503f,
    /// <summary>
    /// (0x5040) Phonetic symbol 46(MSB:Phonetic symbol 46, LSB: Consonant adjustment 46)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL46 = 0x5040,
    /// <summary>
    /// (0x5041) Phonetic symbol 47(MSB:Phonetic symbol 47, LSB: Consonant adjustment 47)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL47 = 0x5041,
    /// <summary>
    /// (0x5042) Phonetic symbol 48(MSB:Phonetic symbol 48, LSB: Consonant adjustment 48)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL48 = 0x5042,
    /// <summary>
    /// (0x5043) Phonetic symbol 49(MSB:Phonetic symbol 49, LSB: Consonant adjustment 49)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL49 = 0x5043,
    /// <summary>
    /// (0x5044) Phonetic symbol 50(MSB:Phonetic symbol 50, LSB: Consonant adjustment 50)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL50 = 0x5044,
    /// <summary>
    /// (0x5045) Phonetic symbol 51(MSB:Phonetic symbol 51, LSB: Consonant adjustment 51)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL51 = 0x5045,
    /// <summary>
    /// (0x5046) Phonetic symbol 52(MSB:Phonetic symbol 52, LSB: Consonant adjustment 52)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL52 = 0x5046,
    /// <summary>
    /// (0x5047) Phonetic symbol 53(MSB:Phonetic symbol 53, LSB: Consonant adjustment 53)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL53 = 0x5047,
    /// <summary>
    /// (0x5048) Phonetic symbol 54(MSB:Phonetic symbol 54, LSB: Consonant adjustment 54)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL54 = 0x5048,
    /// <summary>
    /// (0x5049) Phonetic symbol 55(MSB:Phonetic symbol 55, LSB: Consonant adjustment 55)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL55 = 0x5049,
    /// <summary>
    /// (0x504a) Phonetic symbol 56(MSB:Phonetic symbol 56, LSB: Consonant adjustment 56)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL56 = 0x504a,
    /// <summary>
    /// (0x504b) Phonetic symbol 57(MSB:Phonetic symbol 57, LSB: Consonant adjustment 57)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL57 = 0x504b,
    /// <summary>
    /// (0x504c) Phonetic symbol 58(MSB:Phonetic symbol 58, LSB: Consonant adjustment 58)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL58 = 0x504c,
    /// <summary>
    /// (0x504d) Phonetic symbol 59(MSB:Phonetic symbol 59, LSB: Consonant adjustment 59)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL59 = 0x504d,
    /// <summary>
    /// (0x504e) Phonetic symbol 60(MSB:Phonetic symbol 60, LSB: Consonant adjustment 60)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL60 = 0x504e,
    /// <summary>
    /// (0x504f) Phonetic symbol continuation(MSB, 0x7f=end, 0x00=continue)
    /// </summary>
    CVM_NM_PHONETIC_SYMBOL_CONTINUATION = 0x504f,
    /// <summary>
    /// (0x5050) v1mean in Cent/5(MSB)
    /// </summary>
    CVM_NM_V1MEAN = 0x5050,
    /// <summary>
    /// (0x5051) d1mean in millisec/5(MSB)
    /// </summary>
    CVM_NM_D1MEAN = 0x5051,
    /// <summary>
    /// (0x5052) d1meanFirstNote in millisec/5(MSB)
    /// </summary>
    CVM_NM_D1MEAN_FIRST_NOTE = 0x5052,
    /// <summary>
    /// (0x5053) d2mean in millisec/5(MSB)
    /// </summary>
    CVM_NM_D2MEAN = 0x5053,
    /// <summary>
    /// (0x5054) d4mean in millisec/5(MSB)
    /// </summary>
    CVM_NM_D4MEAN = 0x5054,
    /// <summary>
    /// (0x5055) pMeanOnsetFirstNote in Cent/5(MSB)
    /// </summary>
    CVM_NM_PMEAN_ONSET_FIRST_NOTE = 0x5055,
    /// <summary>
    /// (0x5056) vMeanNoteTransition in Cent/5(MSB)
    /// </summary>
    CVM_NM_VMEAN_NOTE_TRNSITION = 0x5056,
    /// <summary>
    /// (0x5057) pMeanEndingNote in Cent/5(MSB)
    /// </summary>
    CVM_NM_PMEAN_ENDING_NOTE = 0x5057,
    /// <summary>
    /// (0x5058) AddScooptoUpIntervals &amp;, AddPortamentoToDownIntervals(MSB)
    /// </summary>
    CVM_NM_ADD_PORTAMENTO = 0x5058,
    /// <summary>
    /// (0x5059) changAfterPeak(MSB)
    /// </summary>
    CVM_NM_CHANGE_AFTER_PEAK = 0x5059,
    /// <summary>
    /// (0x505a) Accent(MSB)
    /// </summary>
    CVM_NM_ACCENT = 0x505a,
    /// <summary>
    /// (0x507f) Note message continuation(MSB)
    /// </summary>
    CVM_NM_NOTE_MESSAGE_CONTINUATION = 0x507f,
    /// <summary>
    /// (0x5075) Extended Note message; Voice Overlap(MSB, LSB)(VoiceOverlap = ((MSB &amp; 0x7f) &lt;&lt; 7) | (LSB &amp; 0x7f) - 8192)
    /// </summary>
    CVM_EXNM_VOICE_OVERLAP = 0x5075,
    /// <summary>
    /// (0x5076) Extended Note message; Flags length in bytes(MSB, LSB)
    /// </summary>
    CVM_EXNM_FLAGS_BYTES = 0x5076,
    /// <summary>
    /// (0x5077) Extended Note message, Flag(MSB)
    /// </summary>
    CVM_EXNM_FLAGS = 0x5077,
    /// <summary>
    /// (0x5078) Extended Note message, Flag continuation(MSB)(MSB, 0x7f=end, 0x00=continue)
    /// </summary>
    CVM_EXNM_FLAGS_CONINUATION = 0x5078,
    /// <summary>
    /// (0x5079) Extended Note message, Moduration(MSB, LSB)(Moduration = ((MSB &amp, 0x7f) &lt,&lt, 7) | (LSB &amp, 0x7f) - 100)
    /// </summary>
    CVM_EXNM_MODURATION = 0x5079,
    /// <summary>
    /// (0x507a) Extended Note message, PreUtterance(MSB, LSB)(PreUtterance = ((MSB &amp, 0x7f) &lt,&lt, 7) | (LSB &amp, 0x7f) - 8192)
    /// </summary>
    CVM_EXNM_PRE_UTTERANCE = 0x507a,
    /// <summary>
    /// (0x507e) Extended Note message, Envelope: value1(MSB, LSB) actual value = (value3.msb &amp, 0xf) &lt,&lt, 28 | (value2.msb &amp, 0x7f) &lt,&lt, 21 | (value2.lsb &amp, 0x7f) &lt,&lt, 14 | (value1.msb &amp, 0x7f) &lt,&lt, 7 | (value1.lsb &amp, 0x7f)
    /// </summary>
    CVM_EXNM_ENV_DATA1 = 0x507e,
    /// <summary>
    /// (0x507d) Extended Note message, Envelope: value2(MSB, LSB)
    /// </summary>
    CVM_EXNM_ENV_DATA2 = 0x507d,
    /// <summary>
    /// (0x507c) Extended Note message, Envelope: value3(MSB)
    /// </summary>
    CVM_EXNM_ENV_DATA3 = 0x507c,
    /// <summary>
    /// (0x507b) Extended Note message, Envelope: data point continuation(MSB)(MSB, 0x7f=end, 0x00=continue)
    /// </summary>
    CVM_EXNM_ENV_DATA_CONTINUATION = 0x507b,

    /// <summary>
    /// (0x6000) Version number &amp;, Device number(MSB, LSB)
    /// </summary>
    CC_BS_VERSION_AND_DEVICE = 0x6000,
    /// <summary>
    /// (0x6001) Delay in millisec(MSB, LSB)
    /// </summary>
    CC_BS_DELAY = 0x6001,
    /// <summary>
    /// (0x6002) Laugnage type(MSB, optional LSB)
    /// </summary>
    CC_BS_LANGUAGE_TYPE = 0x6002,

    /// <summary>
    /// (0x6100) Version number &amp;, device number(MSB, LSB)
    /// </summary>
    CC_CV_VERSION_AND_DEVICE = 0x6100,
    /// <summary>
    /// (0x6101) Delay in millisec(MSB, LSB)
    /// </summary>
    CC_CV_DELAY = 0x6101,
    /// <summary>
    /// (0x6102) Volume value(MSB)
    /// </summary>
    CC_CV_VOLUME = 0x6102,

    /// <summary>
    /// (0x6200) Version number &amp;, device number(MSB, LSB)
    /// </summary>
    CC_P_VERSION_AND_DEVICE = 0x6200,
    /// <summary>
    /// (0x6201) Delay in millisec(MSB, LSB)
    /// </summary>
    CC_P_DELAY = 0x6201,
    /// <summary>
    /// (0x6202) Pan value(MSB)
    /// </summary>
    CC_PAN = 0x6202,

    /// <summary>
    /// (0x6300) Version number &amp;, device number(MSB, LSB)
    /// </summary>
    CC_E_VESION_AND_DEVICE = 0x6300,
    /// <summary>
    /// (0x6301) Delay in millisec(MSB, LSB)
    /// </summary>
    CC_E_DELAY = 0x6301,
    /// <summary>
    /// (0x6302) Expression vlaue(MSB)
    /// </summary>
    CC_E_EXPRESSION = 0x6302,

    /// <summary>
    /// (0x6400) Version number &amp;, device number(MSB, LSB)
    /// </summary>
    CC_VR_VERSION_AND_DEVICE = 0x6400,
    /// <summary>
    /// (0x6401) Delay in millisec(MSB, LSB)
    /// </summary>
    CC_VR_DELAY = 0x6401,
    /// <summary>
    /// (0x6402) Vibrato Rate value(MSB)
    /// </summary>
    CC_VR_VIBRATO_RATE = 0x6402,

    /// <summary>
    /// (0x6500) Version number &amp;, device number(MSB, LSB)
    /// </summary>
    CC_VD_VERSION_AND_DEVICE = 0x6500,
    /// <summary>
    /// (0x6501) Delay in millisec(MSB, LSB)
    /// </summary>
    CC_VD_DELAY = 0x6501,
    /// <summary>
    /// (0x6502) Vibrato Depth value(MSB)
    /// </summary>
    CC_VD_VIBRATO_DEPTH = 0x6502,

    /// <summary>
    /// (0x6700) Version number &amp;, device number(MSB, LSB)
    /// </summary>
    CC_PBS_VERSION_AND_DEVICE = 0x6700,
    /// <summary>
    /// (0x6701) Delay in millisec(MSB, LSB)
    /// </summary>
    CC_PBS_DELAY = 0x6701,
    /// <summary>
    /// (0x6702) Pitch Bend Sensitivity(MSB, LSB)
    /// </summary>
    CC_PBS_PITCH_BEND_SENSITIVITY = 0x6702,

    /// <summary>
    /// (0x5300) Version number &amp;, device number(MSB, LSB)
    /// </summary>
    PC_VERSION_AND_DEVICE = 0x5300,
    /// <summary>
    /// (0x5301) Delay in millisec(MSB, LSB)
    /// </summary>
    PC_DELAY = 0x5301,
    /// <summary>
    /// (0x5302) Voice Type(MSB)
    /// </summary>
    PC_VOICE_TYPE = 0x5302,

    /// <summary>
    /// (0x5400) Version number &amp;, device number(MSB, LSB)
    /// </summary>
    PB_VERSION_AND_DEVICE = 0x5400,
    /// <summary>
    /// (0x5401) Delay in millisec(MSB, LSB)
    /// </summary>
    PB_DELAY = 0x5401,
    /// <summary>
    /// (0x5402) Pitch Bend value(MSB, LSB)
    /// </summary>
    PB_PITCH_BEND = 0x5402,

    /// <summary>
    /// (0x5500) Version number &amp;, device number(MSB, LSB)
    /// </summary>
    VCP_VERSION_AND_DEVICE = 0x5500,
    /// <summary>
    /// (0x5501) Delay in millisec(MSB, LSB)
    /// </summary>
    VCP_DELAY = 0x5501,
    /// <summary>
    /// (0x5502) Voice Change Parameter ID(MSB)
    /// </summary>
    VCP_VOICE_CHANGE_PARAMETER_ID = 0x5502,
    /// <summary>
    /// (0x5503) Voice Change Parameter value(MSB)
    /// </summary>
    VCP_VOICE_CHANGE_PARAMETER = 0x5503,
};

bool is_require_data_lsb( unsigned int nrpn ){
    switch( nrpn ){
        case CVM_NM_VERSION_AND_DEVICE:
        case CVM_NM_DELAY:
        case CVM_NM_NOTE_DURATION:
        case CVM_NM_INDEX_OF_VIBRATO_DB:
        case CVM_NM_VIBRATO_CONFIG:
        case CVM_NM_PHONETIC_SYMBOL1:
        case CVM_NM_PHONETIC_SYMBOL2:
        case CVM_NM_PHONETIC_SYMBOL3:
        case CVM_NM_PHONETIC_SYMBOL4:
        case CVM_NM_PHONETIC_SYMBOL5:
        case CVM_NM_PHONETIC_SYMBOL6:
        case CVM_NM_PHONETIC_SYMBOL7:
        case CVM_NM_PHONETIC_SYMBOL8:
        case CVM_NM_PHONETIC_SYMBOL9:
        case CVM_NM_PHONETIC_SYMBOL10:
        case CVM_NM_PHONETIC_SYMBOL11:
        case CVM_NM_PHONETIC_SYMBOL12:
        case CVM_NM_PHONETIC_SYMBOL13:
        case CVM_NM_PHONETIC_SYMBOL14:
        case CVM_NM_PHONETIC_SYMBOL15:
        case CVM_NM_PHONETIC_SYMBOL16:
        case CVM_NM_PHONETIC_SYMBOL17:
        case CVM_NM_PHONETIC_SYMBOL18:
        case CVM_NM_PHONETIC_SYMBOL19:
        case CVM_NM_PHONETIC_SYMBOL20:
        case CVM_NM_PHONETIC_SYMBOL21:
        case CVM_NM_PHONETIC_SYMBOL22:
        case CVM_NM_PHONETIC_SYMBOL23:
        case CVM_NM_PHONETIC_SYMBOL24:
        case CVM_NM_PHONETIC_SYMBOL25:
        case CVM_NM_PHONETIC_SYMBOL26:
        case CVM_NM_PHONETIC_SYMBOL27:
        case CVM_NM_PHONETIC_SYMBOL28:
        case CVM_NM_PHONETIC_SYMBOL29:
        case CVM_NM_PHONETIC_SYMBOL30:
        case CVM_NM_PHONETIC_SYMBOL31:
        case CVM_NM_PHONETIC_SYMBOL32:
        case CVM_NM_PHONETIC_SYMBOL33:
        case CVM_NM_PHONETIC_SYMBOL34:
        case CVM_NM_PHONETIC_SYMBOL35:
        case CVM_NM_PHONETIC_SYMBOL36:
        case CVM_NM_PHONETIC_SYMBOL37:
        case CVM_NM_PHONETIC_SYMBOL38:
        case CVM_NM_PHONETIC_SYMBOL39:
        case CVM_NM_PHONETIC_SYMBOL40:
        case CVM_NM_PHONETIC_SYMBOL41:
        case CVM_NM_PHONETIC_SYMBOL42:
        case CVM_NM_PHONETIC_SYMBOL43:
        case CVM_NM_PHONETIC_SYMBOL44:
        case CVM_NM_PHONETIC_SYMBOL45:
        case CVM_NM_PHONETIC_SYMBOL46:
        case CVM_NM_PHONETIC_SYMBOL47:
        case CVM_NM_PHONETIC_SYMBOL48:
        case CVM_NM_PHONETIC_SYMBOL49:
        case CVM_NM_PHONETIC_SYMBOL50:
        case CVM_NM_PHONETIC_SYMBOL51:
        case CVM_NM_PHONETIC_SYMBOL52:
        case CVM_NM_PHONETIC_SYMBOL53:
        case CVM_NM_PHONETIC_SYMBOL54:
        case CVM_NM_PHONETIC_SYMBOL55:
        case CVM_NM_PHONETIC_SYMBOL56:
        case CVM_NM_PHONETIC_SYMBOL57:
        case CVM_NM_PHONETIC_SYMBOL58:
        case CVM_NM_PHONETIC_SYMBOL59:
        case CVM_NM_PHONETIC_SYMBOL60:
        case CC_BS_VERSION_AND_DEVICE:
        case CC_BS_DELAY:
        case CC_BS_LANGUAGE_TYPE:
        case CC_CV_VERSION_AND_DEVICE:
        case CC_CV_DELAY:
        case CC_P_VERSION_AND_DEVICE:
        case CC_P_DELAY:
        case CC_E_VESION_AND_DEVICE:
        case CC_E_DELAY:
        case CC_VR_VERSION_AND_DEVICE:
        case CC_VR_DELAY:
        case CC_VD_VERSION_AND_DEVICE:
        case CC_VD_DELAY:
        case CC_PBS_VERSION_AND_DEVICE:
        case CC_PBS_DELAY:
        case CC_PBS_PITCH_BEND_SENSITIVITY:
        case PC_VERSION_AND_DEVICE:
        case PC_DELAY:
        case PB_VERSION_AND_DEVICE:
        case PB_DELAY:
        case PB_PITCH_BEND:
        case VCP_VERSION_AND_DEVICE:
        case VCP_DELAY:
        case CVM_EXNM_ENV_DATA1:
        case CVM_EXNM_ENV_DATA2:
        case CVM_EXNM_VOICE_OVERLAP:
        case CVM_EXNM_FLAGS_BYTES:
        case CVM_EXNM_MODURATION:
        case CVM_EXNM_PRE_UTTERANCE:
            return true;
        case CVM_NM_NOTE_NUMBER:
        case CVM_NM_VELOCITY:
        case CVM_NM_NOTE_LOCATION:
        case CVM_NM_VIBRATO_DELAY:
        case CVM_NM_PHONETIC_SYMBOL_BYTES:
        case CVM_NM_PHONETIC_SYMBOL_CONTINUATION:
        case CVM_NM_V1MEAN:
        case CVM_NM_D1MEAN:
        case CVM_NM_D1MEAN_FIRST_NOTE:
        case CVM_NM_D2MEAN:
        case CVM_NM_D4MEAN:
        case CVM_NM_PMEAN_ONSET_FIRST_NOTE:
        case CVM_NM_VMEAN_NOTE_TRNSITION:
        case CVM_NM_PMEAN_ENDING_NOTE:
        case CVM_NM_ADD_PORTAMENTO:
        case CVM_NM_CHANGE_AFTER_PEAK:
        case CVM_NM_ACCENT:
        case CVM_NM_NOTE_MESSAGE_CONTINUATION:
        case CC_CV_VOLUME:
        case CC_PAN:
        case CC_E_EXPRESSION:
        case CC_VR_VIBRATO_RATE:
        case CC_VD_VIBRATO_DEPTH:
        case PC_VOICE_TYPE:
        case VCP_VOICE_CHANGE_PARAMETER_ID:
        case VCP_VOICE_CHANGE_PARAMETER:
        case CVM_EXNM_ENV_DATA3:
        case CVM_EXNM_ENV_DATA_CONTINUATION:
        case CVM_EXNM_FLAGS:
        case CVM_EXNM_FLAGS_CONINUATION:
            return false;
    }
    return false;
}
#endif
