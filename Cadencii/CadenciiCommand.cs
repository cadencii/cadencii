/*
 * CadenciiCommand.cs
 * Copyright (C) 2008-2009 kbinani
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
package org.kbinani.cadencii;

import java.util.*;
import org.kbinani.vsq.*;
#else
using System;
using org.kbinani.vsq;
using org.kbinani.java.util;

namespace org.kbinani.cadencii {
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
        private ICommand m_parent;
        public Object[] args;
        private Vector<ICommand> m_child = new Vector<ICommand>();

        public CadenciiCommand( VsqCommand command ) {
            type = CadenciiCommandType.VSQ_COMMAND;
            vsqCommand = command;
            m_child = new Vector<ICommand>();
        }

        public CadenciiCommand() {
            m_child = new Vector<ICommand>();
        }

        public ICommand getParent() {
            return m_parent;
        }

        public void setParent( ICommand value ) {
            m_parent = value;
        }

        public Vector<ICommand> getChild() {
            return m_child;
        }
    }

#if !JAVA
}
#endif
