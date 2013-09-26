/*
 * CadenciiCommand.cs
 * Copyright © 2008-2011 kbinani
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
#if JAVA
package cadencii;

import java.util.*;
import cadencii.vsq.*;
#else
using System;
using System.Collections.Generic;
using cadencii.vsq;
using cadencii.java.util;

namespace cadencii {
#endif

    /// <summary>
    /// Undo/Redoを実現するためのコマンド。
    /// Boare.Lib.Vsq.VsqFileレベルのコマンドは、Type=VsqCommandとして取り扱う。
    /// Boare.Cadencii.VsqFileExレベルのコマンドは、Argsに処理内容を格納して取り扱う。
    /// </summary>
#if JAVA
    public class CadenciiCommand implements ICommand{
#else
    [Serializable]
    public class CadenciiCommand : ICommand {
#endif
        public CadenciiCommandType type;
        public VsqCommand vsqCommand;
        private ICommand mParent;
        public Object[] args;
        private List<ICommand> mChild = new List<ICommand>();

        public CadenciiCommand( VsqCommand command ) {
            type = CadenciiCommandType.VSQ_COMMAND;
            vsqCommand = command;
            mChild = new List<ICommand>();
        }

        public CadenciiCommand() {
            mChild = new List<ICommand>();
        }

        public ICommand getParent() {
            return mParent;
        }

        public void setParent( ICommand value ) {
            mParent = value;
        }

        public List<ICommand> getChild() {
            return mChild;
        }
    }

#if !JAVA
}
#endif
