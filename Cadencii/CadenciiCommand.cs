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
using System;
using System.Collections.Generic;

using Boare.Lib.Vsq;
using bocoree;

namespace Boare.Cadencii {

    /// <summary>
    /// Undo/Redoを実現するためのコマンド。
    /// Boare.Lib.Vsq.VsqFileレベルのコマンドは、Type=VsqCommandとして取り扱う。
    /// Boare.Cadencii.VsqFileExレベルのコマンドは、Argsに処理内容を格納して取り扱う。
    /// </summary>
    [Serializable]
    public class CadenciiCommand : ICommand {
        public CadenciiCommandType type;
        public VsqCommand vsqCommand;
        private ICommand m_parent;
        public object[] args;
        private Vector<ICommand> m_child = new Vector<ICommand>();

        public CadenciiCommand( VsqCommand command ) {
            type = CadenciiCommandType.VSQ_COMMAND;
            vsqCommand = command;
            m_child = new Vector<ICommand>();
        }

        public CadenciiCommand() {
            m_child = new Vector<ICommand>();
        }

        public ICommand parent {
            get {
                return m_parent;
            }
            set {
                m_parent = value;
            }
        }

        public Vector<ICommand> Child {
            get {
                return m_child;
            }
        }
    }

}
