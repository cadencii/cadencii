/*
 * EditedZoneCommand.cs
 * Copyright © 2010-2011 kbinani
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
using System.Collections.Generic;
using cadencii.java.util;

namespace cadencii
{

    public class EditedZoneCommand
    {
        public List<EditedZoneUnit> mAdd;
        public List<EditedZoneUnit> mRemove;

        public EditedZoneCommand(int addStart, int addEnd)
            :
            this(new EditedZoneUnit[] { new EditedZoneUnit(addStart, addEnd) }, new EditedZoneUnit[] { })
        {
        }

        public EditedZoneCommand(EditedZoneUnit[] add, EditedZoneUnit[] remove)
        {
            this.mAdd = new List<EditedZoneUnit>();
            for (int i = 0; i < add.Length; i++) {
                this.mAdd.Add((EditedZoneUnit)add[i].clone());
            }
            this.mRemove = new List<EditedZoneUnit>();
            for (int i = 0; i < remove.Length; i++) {
                this.mRemove.Add((EditedZoneUnit)remove[i].clone());
            }
        }

        public EditedZoneCommand(List<EditedZoneUnit> add, List<EditedZoneUnit> remove)
            :
            this(add.ToArray(), remove.ToArray())
        {
        }
    }

}
