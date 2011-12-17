/*
 * CadenciiCommand.cs
 * Copyright © 2008-2011 kbinani
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
package com.github.cadencii;

import java.util.*;
import com.github.cadencii.vsq.*;
#else
using System;
using com.github.cadencii.vsq;
using com.github.cadencii.java.util;

namespace com.github.cadencii {
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
        private Vector<ICommand> mChild = new Vector<ICommand>();

        public CadenciiCommand( VsqCommand command ) {
            type = CadenciiCommandType.VSQ_COMMAND;
            vsqCommand = command;
            mChild = new Vector<ICommand>();
        }

        public CadenciiCommand() {
            mChild = new Vector<ICommand>();
        }

        public ICommand getParent() {
            return mParent;
        }

        public void setParent( ICommand value ) {
            mParent = value;
        }

        public Vector<ICommand> getChild() {
            return mChild;
        }
    }

#if !JAVA
}
#endif
