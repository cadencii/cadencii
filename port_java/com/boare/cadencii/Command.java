/*
 * ICommand.cs
 * Copyright (c) 2009 kbinani
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

public interface Command {
    /// <summary>
    /// 子コマンドのリスト
    /// </summary>
    Vector<Command> getChild();

    /// <summary>
    /// 親コマンドへの参照
    /// </summary>
    Command getParent();
    void setParent( Command value );
}
