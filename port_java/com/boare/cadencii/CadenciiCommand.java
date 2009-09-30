/*
 * CadenciiCommand.cs
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
package com.boare.cadencii;

import java.util.*;

import com.boare.vsq.*;

/// <summary>
/// Undo/Redoを実現するためのコマンド。
/// Boare.Lib.Vsq.VsqFileレベルのコマンドは、Type=VsqCommandとして取り扱う。
/// Boare.Cadencii.VsqFileExレベルのコマンドは、Argsに処理内容を格納して取り扱う。
/// </summary>
public class CadenciiCommand implements Command {
    public CadenciiCommandType type;
    public VsqCommand vsqCommand;
    public Command m_parent;
    public Object[] args;
    private Vector<Command> m_child = new Vector<Command>();

    public CadenciiCommand( VsqCommand command ) {
        type = CadenciiCommandType.VsqCommand;
        vsqCommand = command;
        m_child = new Vector<Command>();
    }

    public CadenciiCommand() {
        m_child = new Vector<Command>();
    }

    public Command getParent(){
        return m_parent;
    }

    public void setParent( Command value ){
        m_parent = value;
    }

    public Vector<Command> getChild(){
        return m_child;
    }
}
