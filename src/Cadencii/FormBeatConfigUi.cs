/*
 * FormBeatConfigUi.cs
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

namespace cadencii
{
    public interface FormBeatConfigUi : UiBase
    {
        [PureVirtualFunction]
        void setFont(string fontName, float fontSize);

        [PureVirtualFunction]
        void setTitle(string value);

        [PureVirtualFunction]
        void setDialogResult(bool value);

        [PureVirtualFunction]
        void setLocation(int x, int y);

        [PureVirtualFunction]
        int getWidth();

        [PureVirtualFunction]
        int getHeight();

        [PureVirtualFunction]
        void close();


        [PureVirtualFunction]
        void setTextBar1Label(string value);

        [PureVirtualFunction]
        void setTextBar2Label(string value);

        [PureVirtualFunction]
        void setTextStartLabel(string value);

        [PureVirtualFunction]
        void setTextOkButton(string value);

        [PureVirtualFunction]
        void setTextCancelButton(string value);

        [PureVirtualFunction]
        void setTextBeatGroup(string value);

        [PureVirtualFunction]
        void setTextPositionGroup(string value);


        [PureVirtualFunction]
        void setEnabledStartNum(bool value);

        [PureVirtualFunction]
        void setMinimumStartNum(int value);

        [PureVirtualFunction]
        void setMaximumStartNum(int value);

        [PureVirtualFunction]
        int getMaximumStartNum();

        [PureVirtualFunction]
        int getMinimumStartNum();

        [PureVirtualFunction]
        void setValueStartNum(int value);

        [PureVirtualFunction]
        int getValueStartNum();



        [PureVirtualFunction]
        void setEnabledEndNum(bool value);

        [PureVirtualFunction]
        void setMinimumEndNum(int value);

        [PureVirtualFunction]
        void setMaximumEndNum(int value);

        [PureVirtualFunction]
        int getMaximumEndNum();

        [PureVirtualFunction]
        int getMinimumEndNum();

        [PureVirtualFunction]
        void setValueEndNum(int value);

        [PureVirtualFunction]
        int getValueEndNum();


        [PureVirtualFunction]
        bool isCheckedEndCheckbox();

        [PureVirtualFunction]
        void setEnabledEndCheckbox(bool value);

        [PureVirtualFunction]
        bool isEnabledEndCheckbox();

        [PureVirtualFunction]
        void setTextEndCheckbox(string value);


        [PureVirtualFunction]
        void removeAllItemsDenominatorCombobox();

        [PureVirtualFunction]
        void addItemDenominatorCombobox(string value);

        [PureVirtualFunction]
        void setSelectedIndexDenominatorCombobox(int value);

        [PureVirtualFunction]
        int getSelectedIndexDenominatorCombobox();


        [PureVirtualFunction]
        int getMaximumNumeratorNum();

        [PureVirtualFunction]
        int getMinimumNumeratorNum();

        [PureVirtualFunction]
        void setValueNumeratorNum(int value);

        [PureVirtualFunction]
        int getValueNumeratorNum();
    };

}
