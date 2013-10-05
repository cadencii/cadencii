/*
 * FormBezierPointEditUi.cs
 * Copyright © 2011 kbinani
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
using System;
using cadencii;

namespace cadencii
{

    public interface FormBezierPointEditUi : UiBase
    {
        [PureVirtualFunction]
        string getDataPointClockText();

        [PureVirtualFunction]
        string getDataPointValueText();

        [PureVirtualFunction]
        string getLeftClockText();

        [PureVirtualFunction]
        string getLeftValueText();

        [PureVirtualFunction]
        string getRightClockText();

        [PureVirtualFunction]
        string getRightValueText();

        [PureVirtualFunction]
        bool isEnableSmoothSelected();

        [PureVirtualFunction]
        void setEnableSmoothSelected(bool value);

        [PureVirtualFunction]
        void setLeftClockEnabled(bool value);

        [PureVirtualFunction]
        void setLeftValueEnabled(bool value);

        [PureVirtualFunction]
        void setLeftButtonEnabled(bool value);

        [PureVirtualFunction]
        void setRightClockEnabled(bool value);

        [PureVirtualFunction]
        void setRightValueEnabled(bool value);

        [PureVirtualFunction]
        void setRightButtonEnabled(bool value);

        [PureVirtualFunction]
        void setLeftClockText(string value);

        [PureVirtualFunction]
        void setLeftValueText(string value);

        [PureVirtualFunction]
        void setRightClockText(string value);

        [PureVirtualFunction]
        void setRightValueText(string value);

        [PureVirtualFunction]
        void setDataPointClockText(string value);

        [PureVirtualFunction]
        void setDataPointValueText(string value);

        [PureVirtualFunction]
        void setTitle(string value);

        [PureVirtualFunction]
        void setGroupDataPointTitle(string value);

        [PureVirtualFunction]
        void setLabelDataPointClockText(string value);

        [PureVirtualFunction]
        void setLabelDataPointValueText(string value);

        [PureVirtualFunction]
        void setGroupLeftTitle(string value);

        [PureVirtualFunction]
        void setLabelLeftClockText(string value);

        [PureVirtualFunction]
        void setLabelLeftValueText(string value);

        [PureVirtualFunction]
        void setGroupRightTitle(string value);

        [PureVirtualFunction]
        void setLabelRightClockText(string value);

        [PureVirtualFunction]
        void setLabelRightValueText(string value);

        [PureVirtualFunction]
        void setCheckboxEnableSmoothText(string value);

        /// <summary>
        /// ダイアログの結果を設定する
        /// </summary>
        /// <param name="result">OKなら true を、そうでなければ false を設定する</param>
        [PureVirtualFunction]
        void setDialogResult(bool result);

        [PureVirtualFunction]
        void setOpacity(double opacity);

        [PureVirtualFunction]
        void close();
    }

}
